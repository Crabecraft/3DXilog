using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.CompilerServices;
using System;

public class pgmCreator : MonoBehaviour {
	string patchDirectory = @"C:\UnityPGM";
	public Feditor editor;
	public Message MessageBox;
	bool createDirectory()
	{
		string patchDirectory = @"C:\UnityPGM";
		try{
			Directory.Delete(patchDirectory,true);
		}catch{}
		if(Directory.Exists(patchDirectory)) return false;

		try{
		Directory.CreateDirectory(patchDirectory);
		}catch{return false;}

		return true;
	}

	public void createProject()
	{
		if(editor.детали.Length == 0) return;

		for(int i=0;i<editor.детали.Length;i++)
			editor.детали[i].внешние_крепежи = null;

		GameObject[] go = GameObject.FindGameObjectsWithTag("крепеж");
		if(go == null) return;

		ArrayList всяФурнитура = new ArrayList();
		for(int i=0; i < go.Length;i++)
		{ 
			go[i].GetComponent<крепеж>().Ray();
			всяФурнитура.Add(go[i].name);
		}

		int[] количество = new int[всяФурнитура.Count];

		for(int i=0; i < всяФурнитура.Count;i++)
		{
			if(всяФурнитура[i].ToString() != "")
			{    количество[i] = 1;
				for(int j=i+1;j < всяФурнитура.Count;j++)
				{
					if(всяФурнитура[i].ToString() == всяФурнитура[j].ToString())
					{
						количество[i]++;
						всяФурнитура[j] = "";
					}
				}
			}
		}



		if(!createDirectory())return;

		ArrayList tempFur = new ArrayList();
		for(int i=0; i < всяФурнитура.Count;i++)
			if(всяФурнитура[i].ToString() != "")
			{
				tempFur.Add(всяФурнитура[i].ToString().Replace("(Clone)","") +" - "+ количество[i].ToString());
			}

		tempFur.Add("");
		ArrayList всеДетали = new ArrayList();
		for(int i=0; i < editor.детали.Length;i++)
		{
			Detal деталь = editor.детали[i];
			string лицо = "",зад = "";

			присадка(деталь,ref лицо,ref зад);
			присадкаТорцов(ref лицо,деталь);

			//лицо = присадкаЛицо(деталь);
			//зад = присадкаЗад(деталь);

			if(лицо != "")
			{
				лицо =  "H DX="+деталь.DX+" DY="+деталь.DY+" DZ="+деталь.DZ+" -J C=0 V71 T=1073741824 R=1 *MM /\"def.tlg\"\n" +"PUSH_OFF\n" + лицо;
				savePgm(лицо,деталь.name);
			}
			if(зад != "")
				{
				зад =  "H DX="+деталь.DX+" DY="+деталь.DY+" DZ="+деталь.DZ+" -J C=0 V71 T=1073741824 R=1 *MM /\"def.tlg\"\n" +"PUSH_OFF\n" + зад;
				savePgm(зад,деталь.name);
				}

			string saveName = деталь.DX.ToString() + "х" + деталь.DY.ToString() + "х" + деталь.DZ.ToString();
			всеДетали.Add(saveName);
		}

		количество = new int[всеДетали.Count];

		for(int i=0; i < всеДетали.Count;i++)
		{
			if(всеДетали[i].ToString() != "")
			{    количество[i] = 1;
				for(int j=i+1;j < всеДетали.Count;j++)
				{
					if(всеДетали[i].ToString() == всеДетали[j].ToString())
					{
						количество[i]++;
						всеДетали[j] = "";
					}
				}
			}
		}

		for(int i=0; i < всеДетали.Count;i++)
			if(всеДетали[i].ToString() != "")
			{
				tempFur.Add(всеДетали[i].ToString() +" - "+ количество[i].ToString());
			}

		File.WriteAllLines(patchDirectory + "\\Комплектующие.txt",(string[])tempFur.ToArray(typeof(string)));

		MessageBox.Show("Готово!");
	}

	string присадкаЛицо(Detal value,ref string prog1,ref string prog2)
	{
		string temp = "";
		присадкаТорцов(ref temp,value);
		if(value.внешние_крепежи != null)
		if(value.внешние_крепежи.Length > 0)
			for(int i=0; i < value.внешние_крепежи.Length;i++)
				if(value.внешние_крепежи[i] != null)
				{
				  крепеж  тек_крепеж = value.внешние_крепежи[i].gameObject.GetComponent<крепеж>();
				if(Vector3.Distance(value.vert[6].transform.position,value.внешние_крепежи[i].position) < Vector3.Distance(value.vert[7].transform.position,value.внешние_крепежи[i].position) || (тек_крепеж.Остриие && тек_крепеж.глубинаСверления > value.DZ))
				{
					GameObject tekObj = new GameObject();
					tekObj.transform.parent = тек_крепеж.centr;
					tekObj.transform.localPosition = new Vector3(0,0,0);
					tekObj.transform.localRotation = new Quaternion(0,0,0,0);
					tekObj.transform.localScale = new Vector3(1,1,1);
					tekObj.transform.parent = value.vert[6].transform;
					string тип_сверла = "P";
					if(тек_крепеж.Остриие) тип_сверла = "L";
					Double x = Math.Round(tekObj.transform.localPosition.x*1000,1);
					Double y = Math.Round(tekObj.transform.localPosition.z*1000,1);
					GameObject.Destroy(tekObj);
					temp +="XBO X="+x.ToString()+" Y="+y.ToString()+" Z="+ тек_крепеж.глубинаСверления.ToString() +" D="+тек_крепеж.диаметр.ToString()+" R=1 x=0 y=0 N=\""+ тип_сверла +"\" F=1 K=0 P=0\n";
				}
				}
		return temp;
	}
	// лицо верт 6
	// зад верт 0
	// проверка верт 6 и 7

	void присадка(Detal value,ref string prog1,ref string prog2)
	{
		ArrayList лицо = new ArrayList();
		ArrayList зад = new ArrayList();

		if(value.внешние_крепежи == null|| value.внешние_крепежи.Length == 0) return;

			for(int i=0; i < value.внешние_крепежи.Length;i++)
				if(value.внешние_крепежи[i] != null)
				{
					крепеж  тек_крепеж = value.внешние_крепежи[i].gameObject.GetComponent<крепеж>();

				  if(!(тек_крепеж is конфирмат)){
					if(Vector3.Distance(value.vert[6].transform.position,value.внешние_крепежи[i].position) < Vector3.Distance(value.vert[7].transform.position,value.внешние_крепежи[i].position))
					{
						string[] temp = getStroka(тек_крепеж,value.vert[6].transform);
						лицо.Add(new string[]{ getTipKrepej(тек_крепеж),temp[0],temp[1]});

					}else if(Vector3.Distance(value.vert[6].transform.position,value.внешние_крепежи[i].position) > Vector3.Distance(value.vert[7].transform.position,value.внешние_крепежи[i].position)){

						string[] temp = getStroka(тек_крепеж,value.vert[0].transform);
						 зад.Add(new string[]{ getTipKrepej(тек_крепеж),temp[0],temp[1]});
					}
					value.внешние_крепежи[i] = null;
				   }
		        }



		for(int i=0; i < value.внешние_крепежи.Length;i++)
			if(value.внешние_крепежи[i] != null)
			{
				крепеж  тек_крепеж = value.внешние_крепежи[i].gameObject.GetComponent<крепеж>();

				if(тек_крепеж is конфирмат){
					
						string[] temp = getStroka(тек_крепеж,value.vert[6].transform);
					 	string[] retTemp = new string[]{ getTipKrepej(тек_крепеж),temp[0],temp[1]};
					    bool нашел = false;
					    if(лицо.Count > 0)
					    for(int j=0; j < лицо.Count;j++)
					    {
					    	string[] tekStroka = (string[]) лицо[j];
							if(tekStroka[0] == "шкант")
						    if(tekStroka[1] == temp[0])
						    {
							  нашел = true;
							  лицо.Add(retTemp);
							  value.внешние_крепежи[i] = null;
							  break;
						    }
					    }

					if(!нашел)
					{
						temp = getStroka(тек_крепеж,value.vert[0].transform);
					    retTemp = new string[]{ getTipKrepej(тек_крепеж),temp[0],temp[1]};

						if(зад.Count > 0)
					    for(int j=0; j < зад.Count;j++)
					    {
						  string[] tekStroka = (string[]) зад[j];
						  if(tekStroka[0] == "шкант")
						  if(tekStroka[1] == temp[0])
						  {
							зад.Add(retTemp);
							value.внешние_крепежи[i] = null;
							break;
						  }
					    }
					}
				}
			}


		for(int i=0; i < value.внешние_крепежи.Length;i++)
			if(value.внешние_крепежи[i] != null)
			{
				крепеж  тек_крепеж = value.внешние_крепежи[i].gameObject.GetComponent<крепеж>();

				if(тек_крепеж is конфирмат){
					if(getColKrepej(тек_крепеж.GetType().ToString(),лицо)>getColKrepej(тек_крепеж.GetType().ToString(),зад))
					{
						string[] temp = getStroka(тек_крепеж,value.vert[6].transform);
						лицо.Add(new string[]{ getTipKrepej(тек_крепеж),temp[0],temp[1]});
						value.внешние_крепежи[i] = null;
					}else if(getColKrepej(тек_крепеж.GetType().ToString(),лицо) < getColKrepej(тек_крепеж.GetType().ToString(),зад))
					{
						string[] temp = getStroka(тек_крепеж,value.vert[0].transform);
						зад.Add(new string[]{ getTipKrepej(тек_крепеж),temp[0],temp[1]});
						value.внешние_крепежи[i] = null;
					}else 
					{
						if(лицо.Count >= зад.Count)
						{
							string[] temp = getStroka(тек_крепеж,value.vert[6].transform);
							лицо.Add(new string[]{ getTipKrepej(тек_крепеж),temp[0],temp[1]});
							value.внешние_крепежи[i] = null;
						}else
						  {
							string[] temp = getStroka(тек_крепеж,value.vert[0].transform);
							зад.Add(new string[]{ getTipKrepej(тек_крепеж),temp[0],temp[1]});
							value.внешние_крепежи[i] = null;
						  }
					}
				}
			}

		if(лицо.Count > 0)
		for(int i=0;i < лицо.Count;i++)
			prog1+= ((string[])лицо[i])[2] + "\n";

		if(зад.Count > 0)
		for(int i=0;i < зад.Count;i++)
			prog2+= ((string[])зад[i])[2] + "\n";

	}

	int getColKrepej(string value,ArrayList list)
	{
		int ret = 0;
		if(list.Count == 0) return 0;
		for(int i=0;i < list.Count;i++)
			if(((string[])list[i])[0] == value)
				ret++;
		
		return ret;
	}

	string[] getStroka(крепеж тек_крепеж,Transform pos)
	{
		GameObject tekObj = new GameObject();
		tekObj.transform.parent = тек_крепеж.centr;
		tekObj.transform.localPosition = new Vector3(0,0,0);
		tekObj.transform.localRotation = new Quaternion(0,0,0,0);
		tekObj.transform.localScale = new Vector3(1,1,1);
		tekObj.transform.parent = pos;
		string тип_сверла = "P";
		if(тек_крепеж.Остриие) тип_сверла = "L";
		Double x = Math.Round(tekObj.transform.localPosition.x*1000,1);
		Double y = Math.Round(tekObj.transform.localPosition.z*1000,1);

		if(x < 0) x *=-1;
		//if(y < 0) y *=-1;

		GameObject.Destroy(tekObj);
		return new string[]{ x.ToString() , "XBO X="+x.ToString()+" Y="+y.ToString()+" Z="+ тек_крепеж.глубинаСверления.ToString() +" D="+тек_крепеж.диаметр.ToString()+" R=1 x=0 y=0 N=\""+ тип_сверла +"\" F=1 K=0 P=0\n"};
    }

	string getTipKrepej(крепеж value)
	{
		return value.GetType().ToString();
//		switch(value.GetType().ToString())
//		{
//		  case  (Type)шкант: return "шкант";
//		  case  (Type)эксцентрик: return "эксцентрик";
//		  case  (Type)конфирмат: return "конфирмат";
//	      case  (Type)полкодержатель: return "полкодержатель";
//		}
	}

	string присадкаЗад(Detal value)
	{
		string temp = "";
		if(value.внешние_крепежи != null)
		if(value.внешние_крепежи.Length > 0)
			for(int i=0; i < value.внешние_крепежи.Length;i++)
				if(value.внешние_крепежи[i] != null)
				{
					крепеж  тек_крепеж = value.внешние_крепежи[i].gameObject.GetComponent<крепеж>();
				  if(Vector3.Distance(value.vert[6].transform.position,value.внешние_крепежи[i].position) > Vector3.Distance(value.vert[7].transform.position,value.внешние_крепежи[i].position) && (тек_крепеж.глубинаСверления < value.DZ))
				  {
					GameObject tekObj = new GameObject();
					tekObj.transform.parent = тек_крепеж.centr;
					tekObj.transform.localPosition = new Vector3(0,0,0);
					tekObj.transform.localRotation = new Quaternion(0,0,0,0);
					tekObj.transform.localScale = new Vector3(1,1,1);
				    tekObj.transform.parent = value.vert[0].transform;
					string тип_сверла = "P";
					if(тек_крепеж.Остриие) тип_сверла = "L";
					Double x = Math.Round(tekObj.transform.localPosition.x*1000*-1,1);
					Double y = Math.Round(tekObj.transform.localPosition.z*1000,1);
					GameObject.Destroy(tekObj);
					temp +="XBO X="+x.ToString()+" Y="+y.ToString()+" Z="+ тек_крепеж.глубинаСверления.ToString() +" D="+тек_крепеж.диаметр.ToString()+" R=1 x=0 y=0 N=\""+ тип_сверла +"\" F=1 K=0 P=0\n";
				  }
				}
		return temp;
	}

	void savePgm(string text,string name)
	{
		for(int i=1;;i++)
		{
			string patch = patchDirectory+"\\" + name + " ("+ i.ToString()+").xxl"; 
			if(!File.Exists(patch))
			{
				File.WriteAllText(patch,text);
				break;
			}
		}
	}

	void присадкаТорцов(ref string value,Detal деталь)
	{
		деталь.левая.GetPrisadka(ref value);
		деталь.правая.GetPrisadka(ref value);
		деталь.передняя.GetPrisadka(ref value);
		деталь.задняя.GetPrisadka(ref value);
	}


}
