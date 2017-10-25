using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Threading;

public class Detal : MonoBehaviour {
	float _DX,_DY,_DZ;
	public float DX,DY,DZ;
	Plain Left,Right,Front,Back,Down,Up;
	public GameObject box;
	public MeshFilter mesh;
	public MeshRenderer meshRender;
	public GameObject parent;
	public GameObject[] vert;
	material_manager mat;
	LineRenderer линии;

	public bool selected;
	public bool alfa;
	Vector3  screenPos;

	public float PosX;
	public float PosY;
	public float PosZ;

	public ЛеваяСторона левая;
	public ПраваяСторона правая;
	public ПередняяСторона передняя;
	public ЗадняяСторона задняя;

	MeshCollider colider;
	public GameObject tek_viewBox;
//	public string thisname;

	public viewNumbarsPains номера;
	public Transform offset;

	public Transform[] внешние_крепежи;

	public GameObject baseVert;

	public Vector3 scalleToch;
	public int tekParentIndex;

	public GameObject линияЛица;

	public long id;


//	float timer;
//	ManagerProects проект;
	void Start()
	{
		mat= transform.gameObject.GetComponent<material_manager>();
		colider = box.gameObject.GetComponent<MeshCollider>();
		setID();
	//	проект = Camera.main.GetComponent<ManagerProects>();
	}

	public void setID()
	{
		if(id == 0)
			id = long.Parse(gameObject.GetInstanceID().ToString() + DateTime.Now.ToString().Replace(":","").Replace("/","").Replace(" ","").Replace("PM","").Replace("AM","").Trim());
	}


	public Detal Copy()
	{
		GameObject temp =  Instantiate(gameObject,parent.transform.parent.transform.parent.transform)as GameObject;
		GameObject tempView = Instantiate(offset.gameObject)as GameObject;
		tempView.transform.parent = offset.parent;

		Detal detalTemp = temp.GetComponent<Detal>();
		detalTemp.offset = tempView.transform;
		detalTemp.id = 0;
		tempView.transform.localPosition = offset.localPosition;
		tempView.transform.localRotation = offset.localRotation;
		return detalTemp;
	}


	public void SetPosition()
	{
		try{
		Vector3 temp = repairVector(offset.localPosition);	
		parent.transform.localPosition = new Vector3(PosX/1000+temp.x,PosY/1000+temp.y,PosZ/1000+temp.z);
		}catch{}
	}

	void Update ()
	{
		if(selected)
		if(Input.GetKey(KeyCode.Escape))
		{
			select(false,alfa);
		}
		testPosition();
	}

	public void ReadPosition()
	{
		Vector3 temp = parent.transform.localPosition - offset.localPosition;   
		temp = repairVector(temp);
		PosX = temp.x*1000;
		PosY = temp.y*1000;
		PosZ = temp.z*1000;
	}

	void ПоказатьЛицо(bool value)
	{
	   //	timer = 3;
		линияЛица.SetActive(true);
	}

	void testPosition()
	{
		try{
			Vector3 temp = repairVector(offset.localPosition);	
			Vector3 temp1 = repairVector(parent.transform.localPosition);	
			Vector3 temp2 = repairVector(new Vector3(PosX/1000+temp.x,PosY/1000+temp.y,PosZ/1000+temp.z));
		    if(temp1 != temp2) parent.transform.localPosition = temp2;
			//проект.AddWindow();
		}catch{}
	}


	public void setName(string name)
	{
		if(name == "")
		this.name = DX.ToString()+"х"+ DY.ToString()+"х"+ DZ.ToString();
		else
			this.name = name;
	}
	public void sizeSet()
	{
		transform.localScale = new Vector3(1,1,1);
		this._DX = DX/1000;
		this._DY = DY/1000;
		this._DZ = DZ/1000;
		левая.fullReset();
		правая.fullReset();
		передняя.fullReset();
		задняя.fullReset();
		MeshCollider colider = box.gameObject.GetComponent<MeshCollider>();
		colider.sharedMesh = mesh.mesh;
		create_plains();
		обновить_присадку();
	}

	public void wievTouchVert()
	{
		try{
		tek_viewBox.SetActive(true);
		}catch{}
		renderMesh(false);
		viewTouch();
	}

	public void viewTouch()
	{
		for(int i=0; i < vert.Length;i++)
			vert[i].SetActive(true);
		
	}
	public void hideTouchVert()
	{
		try{
		tek_viewBox.SetActive(false);
		}catch{}
		for(int i=0; i < vert.Length;i++)
			vert[i].SetActive(false);
		renderMesh(true);
	}

	public void renderMesh(bool value)
	{
		try{
		if(meshRender == null) meshRender = box.GetComponent<MeshRenderer>();
		meshRender.enabled = colider.enabled  = value;
		}catch{}
	}

	public void setPanelsPrisadka(присадка прис)
	{
		левая.панель = прис.левая;
		правая.панель = прис.правая;
		задняя.панель = прис.задняя;
		передняя.панель = прис.передняя;

		прис.левая.сторона = левая;
		прис.правая.сторона = правая;
		прис.передняя.сторона = передняя;
		прис.задняя.сторона = задняя;

		левая.RelogList();
		правая.RelogList();
		задняя.RelogList();
		передняя.RelogList();
	}

	public void select(bool value, bool прозрачность)
	{
		if(mat == null)mat= transform.gameObject.GetComponent<material_manager>();
		selected = value;
		if(value)
		{			
			if(!прозрачность)
		     {
			    meshRender.material = mat.синий;
		     }else
				if(прозрачность)
		       {
			     meshRender.material = mat.прозрачный_материал_актиный;
		       }
			линияЛица.SetActive(value);

		}
		    else
			{
			   if(!прозрачность)
			   {
				meshRender.material = mat.простой_материал;
			   }else if(прозрачность)
			      {
				   meshRender.material = mat.прозрачный_материал;
			      }

			}
		линияЛица.SetActive(value);
	}

	public void add_krepej(Transform trans)
	{
		ArrayList temp = new ArrayList();
		if(внешние_крепежи != null)
		for(int i=0; i < внешние_крепежи.Length;i++)
		{
			if(внешние_крепежи.Length > 0)
			if(внешние_крепежи[i] == trans) return;

			if(внешние_крепежи[i] != null)
			temp.Add(внешние_крепежи[i]);
		}
		temp.Add(trans);
		внешние_крепежи = (Transform[]) temp.ToArray(typeof(Transform));
	}

	public void delete_krepej(Transform trans)
	{
		ArrayList temp = new ArrayList();
		if(внешние_крепежи != null)
		for(int i=0; i < внешние_крепежи.Length;i++)
		{
			if(внешние_крепежи[i] != null)
			if(внешние_крепежи[i] != trans)temp.Add(внешние_крепежи[i]);
		}
		внешние_крепежи = (Transform[]) temp.ToArray(typeof(Transform));
		тест_крепеж();
	}


	public void setParent(GameObject parent)
	{		
		for(int i =0; i < vert.Length;i++)
		{
			if(parent == vert[i])
			{
				tekParentIndex = i;
				break;
			}
		}

		GameObject clone = Instantiate(parent) as GameObject;
		parent.transform.localScale = new Vector3(1,1,1);
//		Destroy(clone.transform.GetChild(0).gameObject,0.5f);
		tek_viewBox = clone.transform.GetChild(0).gameObject;
		tek_viewBox.GetComponent<Collider>().enabled = false;
		tek_viewBox.SetActive(false);
		Renderer temp =tek_viewBox.gameObject.GetComponent<Renderer>();
		tek_viewBox.transform.localScale =new Vector3(tek_viewBox.transform.localScale.x*1.001f,tek_viewBox.transform.localScale.y*1.001f,tek_viewBox.transform.localScale.z*1.001f);
		mat= transform.gameObject.GetComponent<material_manager>();

		temp.sharedMaterial = mat.синий;
		clone.SetActive(true);
		clone.transform.parent = box.transform;
		clone.transform.localPosition = parent.transform.localPosition;
		clone.transform.parent = transform;
		clone.transform.rotation = box.transform.parent.transform.rotation;
		box.transform.parent = clone.transform;
		Destroy(this.parent,0.5f);
		this.parent = clone;
		this.parent.transform.localPosition = new Vector3(0,0,0);
		тест_крепеж();
	}


	public void EditParent(GameObject parent)
	{		
		GameObject clone = Instantiate(parent) as GameObject;
		clone.name = "новый парент";
		clone.transform.parent = parent.transform;
		clone.transform.localPosition = Vector3.zero;
		clone.transform.parent = this.parent.transform;
		Vector3 temp = clone.transform.position;
		Vector3 temp2= this.parent.transform.position;
		temp = new Vector3((float)Math.Round(temp.x,4),(float)Math.Round(temp.y,4),(float)Math.Round(temp.z,4));
		temp2 = new Vector3((float)Math.Round(temp2.x,4),(float)Math.Round(temp2.y,4),(float)Math.Round(temp2.z,4));
		GameObject.Destroy(clone);
		setParent(parent);
		Vector3 temp3 = temp - temp2;
		temp = temp2+temp3;
		this.parent.transform.position = temp;
		this.parent.transform.localPosition = repairVector(this.parent.transform.localPosition);
	}

	public Vector3 repairVector(Vector3 vek)
	{
		Double x = vek.x;
		Double y = vek.y;
		Double z = vek.z;

		x += 1;
		y += 1;
		z += 1;

		x = Math.Round(x,4);
		y = Math.Round(y,4);
		z = Math.Round(z,4);

		x -= 1;
		y -= 1;
		z -= 1;
		return new Vector3((float)x,(float)y,(float)z);
	}

	GameObject tekViewToch;
	public void setOffset(Transform point, Transform шкаф)
	{
		GameObject clone = Instantiate(baseVert) as GameObject;
		scalleToch = point.localScale;
		clone.transform.parent = point.transform;
		clone.transform.localPosition = Vector3.zero;
		clone.transform.localScale = new Vector3(1,1,1);
		clone.transform.rotation = point.rotation;
		clone.transform.parent = шкаф;
		clone.GetComponent<vertOffset>().SetOffset(point);
		clone.tag = "vertOffset";
		if(offset != null) GameObject.Destroy(offset.gameObject);
		offset = clone.transform;
	}

	public void EditOffset(Transform point, Transform шкаф)
	{
		GameObject clone = Instantiate(baseVert) as GameObject;
		scalleToch = point.localScale;
		clone.transform.parent = point.transform;
		clone.transform.localPosition = Vector3.zero;
		clone.transform.localScale = new Vector3(1,1,1);
		clone.transform.rotation = point.rotation;
		clone.transform.parent = шкаф;
		clone.GetComponent<vertOffset>().SetOffset(point);
		clone.tag = "vertOffset";
		Vector3 temp = parent.transform.localPosition - clone.transform.localPosition;   
		temp = repairVector(temp);
		PosX = temp.x*1000;
		PosY = temp.y*1000;
		PosZ = temp.z*1000;

		if(offset != null) GameObject.Destroy(offset.gameObject);
		offset = clone.transform;
	}

	public void viewTochShkaf()
	{
		tekViewToch = Instantiate(tek_viewBox)as GameObject;
		tekViewToch.transform.parent  = offset.parent;
		tekViewToch.transform.localPosition = offset.localPosition;
		tekViewToch.transform.localRotation = new Quaternion(0,0,0,0);
		tekViewToch.transform.localScale = new Vector3(scalleToch.x*1.01f,scalleToch.y*1.01f,scalleToch.z*1.01f);

		tekViewToch.transform.Rotate(offset.localEulerAngles.x,offset.localEulerAngles.y,offset.localEulerAngles.z);
		tekViewToch.SetActive(true);
	}

	public void HideTochShkaf()
	{
		try{
		GameObject.Destroy(tekViewToch);
		}catch{}
	}

	public void обновить_присадку()
	{
		левая.Allrelog();
		правая.Allrelog();
		передняя.Allrelog();
		задняя.Allrelog();
	}

	void create_plains()
	{
		линии = box.gameObject.GetComponent<LineRenderer>();
		Left = new Plain(new Vector3(0,0,0),new Vector3(0,_DZ,0),new Vector3(0,_DZ,_DY),new Vector3(0,0,_DY));
		Right = new Plain(new Vector3(-_DX,0,_DY),new Vector3(-_DX,_DZ,_DY),new Vector3(-_DX,_DZ,0),new Vector3(-_DX,0,0));

		Front = new Plain(new Vector3(0,0,_DY),new Vector3(0,_DZ,_DY),new Vector3(-_DX,_DZ,_DY),new Vector3(-_DX,0,_DY));
		Back = new Plain(new Vector3(-_DX,0,0),new Vector3(-_DX,_DZ,0),new Vector3(0,_DZ,0),new Vector3(0,0,0));

		Up = new Plain(new Vector3(0,_DZ,_DY),new Vector3(0,_DZ,0),new Vector3(-_DX,_DZ,0),new Vector3(-_DX,_DZ,_DY));
		Down = new Plain(new Vector3(-_DX,0,_DY),new Vector3(-_DX,0,0),new Vector3(0,0,0),new Vector3(0,0,_DY));


		Vector3[] точки = new Vector3[16];
		точки[0] = new Vector3(0,0,0);
		точки[1] = new Vector3(0,_DZ,0);
		точки[2] = new Vector3(0,_DZ,_DY);
		точки[3] = new Vector3(0,0,_DY);
		точки[4] = new Vector3(-_DX,0,_DY);
		точки[5] = new Vector3(-_DX,_DZ,_DY);
		точки[6] = new Vector3(-_DX,_DZ,0);
		точки[7] = new Vector3(-_DX,0,0);
		точки[8] = new Vector3(-_DX,0,_DY);
		точки[9] = new Vector3(-_DX,_DZ,_DY);
		точки[10] = new Vector3(0,_DZ,_DY);
		точки[11] = new Vector3(0,_DZ,0);
		точки[12] = new Vector3(-_DX,_DZ,0);
		точки[13] = new Vector3(-_DX,0,0);
		точки[14] = new Vector3(0,0,0);
		точки[15] = new Vector3(0,0,_DY);
		линии.SetPositions(точки);
		линияЛица.GetComponent<LineRenderer>().SetPositions(new Vector3[]{
			new Vector3(-_DX/2,_DZ+0.05f,0),
			new Vector3(-_DX/2,_DZ,0),
			new Vector3(-_DX/2-0.007f,_DZ+0.015f,0),
			new Vector3(-_DX/2,_DZ,0),
			new Vector3(-_DX/2+0.007f,_DZ+0.015f,0)
		});


		vert[0].transform.localPosition = Left.Point0;
		vert[1].transform.localPosition = Left.Point1;
		vert[2].transform.localPosition = Left.Point2;
		vert[3].transform.localPosition = Left.Point3;
		vert[4].transform.localPosition = Right.Point0;
		vert[5].transform.localPosition = Right.Point1;
		vert[6].transform.localPosition = Right.Point2;
		vert[7].transform.localPosition = Right.Point3;

		vert[8].transform.localPosition = new Vector3(0,_DZ/2,0);
		vert[9].transform.localPosition = new Vector3(-_DX,_DZ/2,0);
		vert[10].transform.localPosition = new Vector3(0,_DZ/2,_DY);
		vert[11].transform.localPosition = new Vector3(-_DX,_DZ/2,_DY);

		vert[12].transform.localPosition = new Vector3(0,0,_DY/2);
		vert[13].transform.localPosition = new Vector3(-_DX/2,0,0);
		vert[14].transform.localPosition = new Vector3(-_DX,0,_DY/2);
		vert[15].transform.localPosition = new Vector3(-_DX/2,0,_DY);

		vert[16].transform.localPosition = new Vector3(0,_DZ,_DY/2);
		vert[17].transform.localPosition = new Vector3(-_DX/2,_DZ,0);
		vert[18].transform.localPosition = new Vector3(-_DX,_DZ,_DY/2);
		vert[19].transform.localPosition = new Vector3(-_DX/2,_DZ,_DY);

		vert[20].transform.localPosition = new Vector3(0,_DZ/2,_DY/2);
		vert[21].transform.localPosition = new Vector3(-_DX/2,_DZ/2,0);
		vert[22].transform.localPosition = new Vector3(-_DX,_DZ/2,_DY/2);
		vert[23].transform.localPosition = new Vector3(-_DX/2,_DZ/2,_DY);

		vert[24].transform.localPosition = new Vector3(-_DX/2,0,_DY/2);
		vert[25].transform.localPosition = new Vector3(-_DX/2,_DZ/2,_DY/2);
		vert[26].transform.localPosition = new Vector3(-_DX/2,_DZ,_DY/2);
		create_mesh();

		левая.зона1.localPosition = vert[9].transform.localPosition;
		правая.зона1.localPosition = vert[8].transform.localPosition;
		задняя.зона1.localPosition = vert[11].transform.localPosition;
		передняя.зона1.localPosition = vert[9].transform.localPosition;

		левая.зона2.localPosition = vert[11].transform.localPosition;
		правая.зона2.localPosition = vert[10].transform.localPosition;
		задняя.зона2.localPosition = vert[10].transform.localPosition;
		передняя.зона2.localPosition = vert[8].transform.localPosition;

		номера.setNumbers();
	}





	void create_mesh()
	{
		System.Collections.Generic.List<Vector3>  temp = new System.Collections.Generic.List<Vector3>();

		//		перед(Front)
		temp.Add(Front.Point0);
		temp.Add(Front.Point3);
		temp.Add(Front.Point1);
		temp.Add(Front.Point2);
		//		перед

		//		верх(Up)
		temp.Add(Up.Point1);
		temp.Add(Up.Point2);
		//		верх


		//		зад(Back)
		temp.Add(Back.Point3);
		temp.Add(Back.Point0);
		//		зад

		//		верх
		temp.Add(Up.Point0);
		temp.Add(Up.Point3);
		//		верх

		//		зад
		temp.Add(Back.Point2);
		temp.Add(Back.Point1);
		//		зад

		//		низ(Down)
		temp.Add(Down.Point2);
		temp.Add(Down.Point3);
		temp.Add(Down.Point0);
		temp.Add(Down.Point1);
		//		низ
		//
		//		право(Right)
		temp.Add(Right.Point0);
		temp.Add(Right.Point1);
		temp.Add(Right.Point2);
		temp.Add(Right.Point3);
		//      право

		//		лево(left)
		temp.Add(Left.Point0);
		temp.Add(Left.Point1);
		temp.Add(Left.Point2);
		temp.Add(Left.Point3);
		//		лево


		mesh.mesh.SetVertices(temp);
	}

	public void вращать(string ось)
	{		

		switch(ось)
		{
		case "X": parent.transform.Rotate(new Vector3(90,0,0)); break;
		case "Y": parent.transform.Rotate(new Vector3(0,90,0)); break;
		case "Z": parent.transform.Rotate(new Vector3(0,0,90)); break;
		}
		тест_крепеж();
	}

	public void перевернуть_DX()
	{

		int tekIndex = tekParentIndex;
		EditParent(vert[25]);
		parent.transform.Rotate(new Vector3(0,0,180));
		EditParent(vert[зеркальная_точкаDX(tekIndex)]);
		тест_крепеж();
		//int tekIndex = tekParentIndex;
	    //parent.transform.Rotate(new Vector3(0,0,180));
		//setParent(vert[зеркальная_точка(tekIndex)]);

	}

	public void перевернуть_DY()
	{

		int tekIndex = tekParentIndex;
		EditParent(vert[25]);
		parent.transform.Rotate(new Vector3(180,0,0));
		EditParent(vert[зеркальная_точкаDY(tekIndex)]);
		тест_крепеж();
		//int tekIndex = tekParentIndex;
		//parent.transform.Rotate(new Vector3(0,0,180));
		//setParent(vert[зеркальная_точка(tekIndex)]);

	}

	int зеркальная_точкаDX(int i)
	{
		switch(i)
	  {
	    case 0: return 6; 
		case 1: return 7; 
		case 2: return 4; 
		case 3: return 5; 
		case 4: return 2; 
		case 5: return 3; 
		case 6: return 0; 
		case 7: return 1; 
		case 8: return 9; 
		case 9: return 8; 
		case 10: return 11; 
		case 11: return 10; 
		case 12: return 18; 
		case 13: return 17; 
		case 14: return 16; 
		case 15: return 19; 
		case 16: return 14; 
		case 17: return 13; 
		case 18: return 12; 
		case 19: return 15; 
		case 20: return 22; 
		case 21: return 21; 
		case 22: return 20; 
		case 23: return 23; 
		case 24: return 26; 
		case 25: return 25; 
		case 26: return 24; 
	  }
		return 25;

	}


	int зеркальная_точкаDY(int i)
	{
		switch(i)
		{
		case 0: return 2; 
		case 1: return 3; 
		case 2: return 0; 
		case 3: return 1; 
		case 4: return 6; 
		case 5: return 7; 
		case 6: return 4; 
		case 7: return 5; 
		case 8: return 10; 
		case 9: return 11; 
		case 10: return 8; 
		case 11: return 9; 
		case 12: return 16; 
		case 13: return 19; 
		case 14: return 18; 
		case 15: return 17; 
		case 16: return 12; 
		case 17: return 15; 
		case 18: return 14; 
		case 19: return 13; 
		case 20: return 20; 
		case 21: return 23; 
		case 22: return 22; 
		case 23: return 21; 
		case 24: return 26; 
		case 25: return 25; 
		case 26: return 24; 
		}
		return 25;

	}
    public void	прозрачность(bool value)
	{
		alfa = value;
		if(value)
		   {
			  if(selected)
				meshRender.material = mat.прозрачный_материал_актиный;
			    else
			    meshRender.material = mat.прозрачный_материал;
		   }
		else 
		{
			  if(selected)
				meshRender.material = mat.синий;
			    else
				meshRender.material = mat.простой_материал;
		}
	}


	public void тест_крепеж()
	{
		GameObject[] go = GameObject.FindGameObjectsWithTag("крепеж");
		if(go == null) return;
		for(int i =0; i < go.Length;i++)
			go[i].GetComponent<крепеж>().test();
	}
}

public class SerializableDetal{

	public string name;

	public Vector3 size;
	public Vector3 position;

	public Vector3 scaleOffset;

	public Vector3 rotation;
	public Vector3 offset;

	public int tekVert;

	public string[] Левая;
	public string[] Правая;
	public string[] Передняя;
	public string[] Задняя;

	public long indexOffsetDetal;
	public int indexVert;
	public long id;

	public SerializableDetal(){}
	public SerializableDetal StartSerializableDetal(Detal деталь)
	{		
		name = деталь.name;
		size = new Vector3(деталь.DX,деталь.DY,деталь.DZ);
		position = new Vector3(деталь.PosX,деталь.PosY,деталь.PosZ);
		rotation = деталь.repairVector(деталь.parent.transform.localEulerAngles);
		offset = деталь.repairVector(деталь.offset.localPosition);
		scaleOffset = деталь.scalleToch;
		tekVert = деталь.tekParentIndex;
		Левая = деталь.левая.stroka;
		Правая = деталь.правая.stroka;
		Задняя = деталь.задняя.stroka;
		Передняя = деталь.передняя.stroka;
		vertOffset vertOffset = деталь.offset.gameObject.GetComponent<vertOffset>();
		indexOffsetDetal = vertOffset.indexOffsetDetal;
		indexVert = vertOffset.indexVert;
		id = деталь.id;
		return this;
	}



}

public class Project{

	public Vector3 размеры_шкафа;
	public SerializableDetal[] детали;
}



public class Plain{
	
	public Vector3 Point0,Point1,Point2,Point3;
	public Plain(){}
	public Plain(Vector3 Point0,Vector3 Point1,Vector3 Point2,Vector3 Point3)
	{
		this.Point0 = Point0;
		this.Point1 = Point1;
		this.Point2 = Point2;
		this.Point3 = Point3;
	}
}

