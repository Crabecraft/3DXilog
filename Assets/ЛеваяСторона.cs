using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ЛеваяСторона : присадкаСтороны {

	public override void GetPrisadka (ref string value)
	{

		for(int i=0; i < stroka.Length;i++)
		{
			if(stroka[i] == null) continue;
			string[] temp = readStr(i);

			if((temp[0] == "0" || temp[0] == "") && temp[2] == "1")
				temp[0] = (деталь.DY/2).ToString();

			if(!stroka[i].Contains("Зона 1")) 
			{ 
				temp[0] = "DY-" + temp[0];
				if(temp[1] != "0")
			    temp[1] = "-" + temp[1];
			}

			if(stroka[i].Contains("Эксцентрик"))
			{
				value += "XBO X="+ getStrOtcraya().ToString() +" Y="+ temp[0] +" Z=26 D=8 R="+ temp[2] +" x=0 y="+ temp[1] +" N=\"P\" F=3 K=0 P=0\n";
			    value += "XBO X=34 Y="+ temp[0] +" Z=13 D=15 R="+ temp[2] +" x=0 y="+ temp[1] +" N=\"P\" F=1 K=0 P=0\n";
			}
			else
				if(stroka[i].Contains("Шкант"))
					value += "XBO X="+ getStrOtcraya().ToString() +" Y="+ temp[0] +" Z=26 D=8 R="+ temp[2] +" x=0 y="+ temp[1] +" N=\"P\" F=3 K=0 P=0\n";
			else
				if(stroka[i].Contains("Конфирмат"))
						value += "XBO X="+ getStrOtcraya().ToString() +" Y="+ temp[0] +" Z=34 D=5 R="+ temp[2] +" x=0 y="+ temp[1] +" N=\"P\" F=3 K=0 P=0\n";
		}
	}
}

public class присадкаСтороны : MonoBehaviour {
	public string[] stroka;
	public GameObject[] strokaObject;
	public панельПрисадки панель;
	public Transform зона1,зона2;
	public Фурнитура фурнитура;
	public Detal деталь;
	public ManagerProects проекты;

	public void RelogList()
	{
		if(stroka == null) return;
		панель.list.Items = new string[stroka.Length];
		if(stroka.Length == 0)
		{
			панель.list.seletcItem = -1;
			return;
		}

		for(int i=0; i < stroka.Length;i++)
			панель.list.Items[i] = stroka[i];
	}

	public virtual void Rotate(Transform transform){}
	public virtual void GetPrisadka(ref string value){}
	public virtual Vector3 getVector(float value){return new Vector3(0,getOtcraya(),value);}

	public float getStrOtcraya()
	{
		if(деталь.DZ > 22) return деталь.DZ-9;
		return деталь.DZ/2;
	}

	public float getOtcraya()
	{
		if(деталь.DZ > 22) return (деталь.DZ/2 - 9)/1000;
		return 0;
	}

	public bool testKrepej(Transform value)
	{
		for(int i=0; i< strokaObject.Length; i++)
			if(strokaObject[i].transform == value) return false;
		return true;
	}

	public string[] readStr(int index)
	{
		string[] ret= new string[3];
		string value = stroka[index];

		value = value.Replace("(Зона 1)","").Replace("(Зона 2)","");
		value = value.Replace("Эксцентрик","").Replace("Шкант","").Replace("Конфирмат","").Replace("Полкодержатель","");

		string[] tempMass = value.Split('(');

		tempMass[0]   = tempMass[0].Trim();
		ret[0] = tempMass[0].Split('-')[0];
		ret[2] = tempMass[1].Split('ш')[0].Trim();
		ret[1] = getShag(ret[0],tempMass[0].Split('-')[1],ret[2]);

		return ret;
	}

	public virtual string getShag(string начало,string шаг,string количество)
	{
		if(шаг != "0") return шаг;
		if(количество != "1")
		try{
			 return Math.Round(((деталь.DY-(float.Parse(начало)*2))/(int.Parse(количество)-1)),1).ToString();
		}catch{}
		return "0";
	}
	 
	public void add(string value)
	{
		ArrayList temp = new ArrayList();
		for(int i=0;i < stroka.Length;i++)
			temp.Add(stroka[i]);
		temp.Add(value);
		stroka = (string[]) temp.ToArray(typeof(string));
		temp = new ArrayList();
		int j =0;
		for( ;j < strokaObject.Length;j++)
			temp.Add(strokaObject[j]);
		temp.Add(createObject(stroka[stroka.Length-1]));
		strokaObject = (GameObject[]) temp.ToArray(typeof(GameObject));
		RelogList();
		Reset(strokaObject.Length-1);
	}

	public void relogStroka (int value)
	{
		if(strokaObject[value] != null) GameObject.Destroy(strokaObject[value]);
		strokaObject[value] = createObject(stroka[value]);
		Reset(value);
		RelogList();
	}
	public void Allrelog ()
	{
		if(strokaObject != null)
		for(int i=0; i < strokaObject.Length;i++)
				GameObject.Destroy(strokaObject[i]);

		strokaObject = new GameObject[stroka.Length];
		for(int i =0; i < stroka.Length;i++)
		{
			try{
		    if(strokaObject[i] != null) GameObject.Destroy(strokaObject[i]);
			}catch{}
		strokaObject[i] = createObject(stroka[i]);
		Reset(i);
		//RelogList();
		}
	}
	public void Reset(int index)
	{
		Detal temp = gameObject.GetComponent<Detal>();
		for(int i=0; i < strokaObject[index].transform.childCount;i++)
			strokaObject[index].transform.GetChild(i).gameObject.GetComponent<крепеж>().Reset(temp.DZ);
	}

	public void fullReset()
	{
		for(int i=0; i < strokaObject.Length;i++) Reset(i);
	}

	public GameObject createObject(string value)
	{
		int зона = 1;
		if(!value.Contains("Зона 1")) 
		{
			зона = 2;
			value = value.Replace("(Зона 2)","");
		}else
		{
			value = value.Replace("(Зона 1)","");
		}

		GameObject крепеж = null;

		if(value.Contains("Эксцентрик"))
			крепеж = фурнитура.эксцентрик;
		else
			if(value.Contains("Шкант"))
				крепеж = фурнитура.шкант;
			else
				if(value.Contains("Конфирмат"))
					крепеж = фурнитура.конфирмат;
				else
					if(value.Contains("Полкодержатель"))
						крепеж = фурнитура.полкодержатель;
		

		value = value.Replace("Эксцентрик","").Replace("Шкант","").Replace("Конфирмат","").Replace("Полкодержатель","");

		string[] tempMass = value.Split('(');

		tempMass[0]  = tempMass[0].Trim();

		string _начало  = tempMass[0].Split('-')[0];
		string _количество = tempMass[1].Split('ш')[0].Trim();
		float шаг    = float.Parse(getShag(_начало,tempMass[0].Split('-')[1],_количество))/1000;

		if((_начало == "0" || _начало == "") && _количество == "1")_начало = пополам();

		float начало = float.Parse(_начало)/1000;
		int col      = int.Parse(_количество);

		return createPrisadka(зона,начало,шаг,col,крепеж);
	}

	public virtual string пополам()
	{
	   return (деталь.DY/2).ToString();
	}


	public GameObject createPrisadka(int зона,float начало,float шаг, int col, GameObject baseObj)
	{
		GameObject ret = new GameObject();

		switch(зона)
		{
		case 1: ret.transform.parent = зона1;break;
		case 2: ret.transform.parent = зона2;break;
		}

		ret.transform.localPosition = new Vector3(0,0,0);
		ret.transform.localRotation = new Quaternion(0,0,0,0);

		for(int i=1; i < col+1;i++)
		{
			GameObject obj = GameObject.Instantiate(baseObj);
			obj.transform.parent = ret.transform;

			if(зона == 1)
			{
				if(col == 1)
				 obj.transform.localPosition = getVector(начало);
				else
				 obj.transform.localPosition = getVector(начало+(шаг*i-шаг));
			}
			else
			{
				if(col == 1)
					obj.transform.localPosition = getVector(начало*(-1)); 
				else
				    obj.transform.localPosition = getVector((начало+(шаг*i-шаг))*(-1));
			}
			obj.transform.localRotation = new Quaternion(0,0,0,0);
			Rotate(obj.transform);
		}


		return ret;
	}

	public void удалить()
	{
		int select = панель.list.seletcItem;
		if(select == -1) return;

		GameObject.Destroy(strokaObject[select]);

		ArrayList temp = new ArrayList();
		for(int i=0;i<stroka.Length;i++)
			if(i != select) temp.Add(stroka[i]);
		stroka = (string[]) temp.ToArray(typeof(string));

		temp = new ArrayList();
		for(int i=0;i< strokaObject.Length;i++)
			if(i != select) temp.Add(strokaObject[i]);
		strokaObject = (GameObject[]) temp.ToArray(typeof(GameObject));
		RelogList();
	}


}
