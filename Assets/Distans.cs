using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Security.Policy;
using System.CodeDom.Compiler;

public class Distans : MonoBehaviour {

	public ManagerProects проекты;
	public Text текст_Ниша;
	public Toggle isX,isY,isZ;
	Detal obj1,obj2;

	Transform point1,point2;

	public bool FindDistans3;
	public bool FindDistans1;

	public Feditor editor;
	Detal Center1,Center2;

	Detal прижимаемая, прижать_к;
	bool прижатие;

	public float TikTimerTestKrepej;
	float timer;
	public void SetDetal(Detal деталь)
	{
		obj1 = obj2;
		obj2 = деталь;
		getDistans();
	}

	public void StartFindCenter3()
	{
		FindDistans3 = true;
		editor.тек_деталь.box.SetActive(false);
	}

	public void StartFindCenter1()
	{
		FindDistans1 = true;
		editor.тек_деталь.box.SetActive(false);
	}

	public void CancelFindCenter()
	{
		FindDistans3 = false;
		FindDistans1 = false;
		editor.тек_деталь.box.SetActive(true);
		Center1 = Center2 = null;
	}

	void SetOsi(Vector3 center)
	{
		Vector3 repairTemp = editor.тек_деталь.repairVector(editor.тек_деталь.parent.transform.localPosition);
		float x = repairTemp.x;
		float y = repairTemp.y;
		float z = repairTemp.z;



		editor.тек_деталь.parent.transform.localPosition = center;
		GameObject tempVert = new GameObject();
		tempVert.transform.parent = editor.тек_деталь.parent.transform;
		tempVert.transform.localPosition = Vector3.zero;
		tempVert.transform.localRotation = new Quaternion(0,0,0,0);
		tempVert.transform.parent = editor.тек_деталь.vert[25].transform;
		Vector3 ret2 = tempVert.transform.localPosition;
		GameObject temp = new GameObject();
		temp.transform.parent = editor.тек_деталь.parent.transform;
		temp.transform.localPosition = ret2;
		temp.transform.parent = temp.transform.parent.parent;
		Vector3 ret1 = editor.тек_деталь.repairVector(temp.transform.localPosition);


		float ret1x =  (float)Math.Round(temp.transform.localPosition.x,4);
		float ret1y =  (float)Math.Round(temp.transform.localPosition.y,4);
		float ret1z =  (float)Math.Round(temp.transform.localPosition.z,4);

		if(isX.isOn)ret1x = x;
		if(isY.isOn)ret1y = y;
		if(isZ.isOn)ret1z = z;

		editor.тек_деталь.parent.transform.localPosition = new Vector3(ret1x,ret1y,ret1z);
		editor.тек_деталь.ReadPosition();
		editor.RelogDetal();
		GameObject.Destroy(tempVert);
		GameObject.Destroy(temp);
		Center1.select(false,editor.прозрачность.isOn);
		FindDistans3 = false;
		FindDistans1 = false;
		Center1 = Center2 = null;
		проекты.AddWindow();
	}

	public void addFindCenter(Detal деталь)
	{
		if(Center1 == null)
		{
			Center1 = деталь;
			Center1.select(true,editor.прозрачность.isOn);
			return;
		}
		editor.тек_деталь.box.SetActive(true);
		Center2 = деталь;

		if(Center1 == Center2)
			SetOsi(getCenter());
		else
		    SetOsi(getCenter3());
	}

	Vector3 getCenter()
	{
		GameObject p1 = new GameObject();

		p1.transform.parent = Center1.vert[25].transform.GetChild(0);
		p1.transform.localPosition = Vector3.zero;
		p1.transform.parent = editor.тек_деталь.parent.transform.parent;
		Vector3 center = p1.transform.localPosition;
		GameObject.Destroy(p1);
		return center;
	}

	Vector3 getCenter3()
	{
		GameObject p1 = new GameObject();
		GameObject p2 = new GameObject();

		p1.transform.parent = getMInDistans(Center1.vert[25],new GameObject[]{Center2.vert[24],Center2.vert[26],Center2.vert[21],Center2.vert[23],Center2.vert[20],Center2.vert[22]}).transform;
		p2.transform.parent = getMInDistans(Center2.vert[25],new GameObject[]{Center1.vert[24],Center1.vert[26],Center1.vert[21],Center1.vert[23],Center1.vert[20],Center1.vert[22]}).transform;
		p1.transform.localPosition = Vector3.zero;
		p2.transform.localPosition = Vector3.zero;
		p1.transform.parent = editor.тек_деталь.parent.transform.parent;
		p2.transform.parent = editor.тек_деталь.parent.transform.parent;
		Vector3 center = getSenter(p1.transform.localPosition,p2.transform.localPosition);
		GameObject.Destroy(p1);
		GameObject.Destroy(p2);
		return center;
	}

	GameObject getMInDistans(GameObject центр, GameObject[] list)
	{
		GameObject temp = list[0];
		float distans = Vector3.Distance(центр.transform.position,list[0].transform.position);

		for(int i=1; i < list.Length;i++)
		{
			float tekDistans =Vector3.Distance(центр.transform.position,list[i].transform.position);
			if(tekDistans < distans)
			{
				distans = tekDistans;
				temp = list[i];
			}
		}
		return temp;
	}

	Vector3 getSenter(Vector3 p1,Vector3 p2)
	{
		float x = vecValue(p1.x,p2.x);
		float y = vecValue(p1.y,p2.y);
		float z = vecValue(p1.z,p2.z);

		return new Vector3(x,y,z);
	}

	float vecValue(float v1,float v2)
	{
		float a = 0;
		float b = 0;

		if(v1 > v2)
		{
			a = v1;
			b = v2;
		}else if(v1<v2)
		{
			b = v1;
			a = v2;
		}else if(v1 == v2) return v1;

		return (a-b)/2+b;
	}

	void getDistans()
	{
		if(obj1 == null || obj2 == null)return;

		GameObject go1 = new GameObject();
		if(point1 !=null) GameObject.Destroy(point1.gameObject);

		point1 = go1.transform;

		point1.parent = getploskosti(new GameObject[]{obj2.vert[24],obj2.vert[26],obj2.vert[21],obj2.vert[23],obj2.vert[20],obj2.vert[22]},new GameObject[]{obj1.vert[24],obj1.vert[26],obj1.vert[21],obj1.vert[23],obj1.vert[20],obj1.vert[22]});
		point1.localPosition = Vector3.zero;
		point1.parent = getploskosti(new GameObject[]{obj1.vert[24],obj1.vert[26],obj1.vert[21],obj1.vert[23],obj1.vert[20],obj1.vert[22]},new GameObject[]{obj2.vert[24],obj2.vert[26],obj2.vert[21],obj2.vert[23],obj2.vert[20],obj2.vert[22]});

		Double ret  = Math.Round(point1.localPosition.x*1000,1);
		if(ret < 0) ret*=-1;

		string text1 = ret.ToString();
		float n = (float)ret-36;
		if(n >= 0) text1 +=" ("+ (n/3).ToString() + ")";
		текст_Ниша.text  = text1;
	}


	Transform getploskosti(GameObject[] p1,GameObject[] p2)
	{
		GameObject _p = new GameObject();
		Transform p = _p.transform;

		Transform tekP = p;
		int colView =0;

		for(int i=0; i < p1.Length;i++)
		{
			int tekCol = 0;
			for(int j=0; j < p2.Length;j++)
			{
				p.parent = p2[j].transform;
				p.localPosition = Vector3.zero;
				p.parent = p1[i].transform;
				if(p.localPosition.x > -0.001f)
					tekCol++;
			}
			if(tekCol > colView)
			{
				colView = tekCol;
				tekP = p1[i].transform;
			}
		}

		GameObject.Destroy(_p);
		return tekP;
	}

	void Update () 
	{
		timer += Time.deltaTime;
		if(timer >= TikTimerTestKrepej)
		{
			timer = 0;
			тест_крепеж();
		}

		if(Input.GetKeyDown(KeyCode.LeftShift))прижатие = true;
		if(Input.GetKeyUp(KeyCode.LeftShift))прижатие = false;
	}

	public void тест_крепеж()
	{
		GameObject[] go = GameObject.FindGameObjectsWithTag("крепеж");
		if(go == null) return;
		for(int i =0; i < go.Length;i++)
			go[i].GetComponent<крепеж>().test();
	}

	public void addPrijatDetal(Detal деталь)
	{
		if(прижимаемая == null) 
		{
			прижимаемая = деталь;
			return;
		}

		прижать_к = деталь;
		prijat();
	}

	void prijat()
	{
		 










		прижимаемая = null;
		прижать_к = null;
		проекты.AddWindow();
	}
}
