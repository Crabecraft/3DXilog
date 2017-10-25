using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Feditor : MonoBehaviour {
	public GameObject offset;
	public Dropdown list;
	public GameObject панельДетали;
	public Toggle прозрачность;
	public присадка Присадка;
	public ListBox2 test;
	public Detal тек_деталь;
	public InputField x,y,z,dx,dy,dz;
	public InputField nameDetal;
	public Distans дистанция;
	public Camera Mycamera;
	public ManagerProects проекты;
	public movCam2 _camera;
	bool TouchDetalVert;
	bool TouchShkafVert;
	bool EditDetalVert;
	bool EditShkafVert;

	public Detal[] детали;
	// Use this for initialization
	void Start () {
		list.captionText.text = "Список деталей";
		RelogList();
	}

	// Update is called once per frame
	void Update () {
		
	

		if(тек_деталь != null)
		{
		    if(Input.GetKey(KeyCode.Escape))
		    {
				if(дистанция.FindDistans1 || дистанция.FindDistans3)дистанция.CancelFindCenter();
			    DiactiveDetal(тек_деталь);
				тек_деталь = null;
		    }

			if(Input.GetKey(KeyCode.Delete))
			{
				DeleteDetal();
			}
		}

		if(тек_деталь == null && панельДетали.activeSelf)
		{
			панельДетали.SetActive(false);
			Присадка.panel.SetActive(false);
		}
		if(тек_деталь != null && !панельДетали.activeSelf)
		{
			панельДетали.SetActive(true);
			Присадка.view(Присадка.chek.isOn);
		}

		if(!дистанция.FindDistans1 && !дистанция.FindDistans3){

		if (TouchDetalVert)
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;          //Точка касания
			Ray ray = Mycamera.ScreenPointToRay(Input.mousePosition);//луч
			if (Physics.Raycast(ray,out hit)) {
				if (hit.collider != null)
				{
					if(hit.collider.gameObject.tag == "wertTouch")
					{
						тек_деталь.setParent(hit.collider.transform.parent.gameObject);
						тек_деталь.SetPosition();
						closeTouchVert();
							проекты.AddWindow();
						return;
					}
				}
			}
		}

		if (TouchShkafVert)
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;          //Точка касания
			Ray ray = Mycamera.ScreenPointToRay(Input.mousePosition);//луч
			bool active = true;
			if (Physics.Raycast(ray,out hit)) {
				if (hit.collider != null && active)
				{
					active = false;
					if(hit.collider.gameObject.tag == "wertTouch")
					{
						тек_деталь.setOffset(hit.collider.transform,offset.transform);
						тек_деталь.SetPosition();
						closeTouchVert();
						проекты.AddWindow();
						return;
					}
				}
			}
		}

			if (EditDetalVert)
			if (Input.GetMouseButtonDown(0)) {
				RaycastHit hit;          //Точка касания
				Ray ray = Mycamera.ScreenPointToRay(Input.mousePosition);//луч
				if (Physics.Raycast(ray,out hit)) {
					if (hit.collider != null)
					{
						if(hit.collider.gameObject.tag == "wertTouch")
						{
							тек_деталь.EditParent(hit.collider.transform.parent.gameObject);
							тек_деталь.ReadPosition();
							RelogDetal();
							closeTouchVert();
							проекты.AddWindow();
							return;
						}
					}
				}
			}

			if (EditShkafVert)
			if (Input.GetMouseButtonDown(0)) {
				RaycastHit hit;          //Точка касания
				Ray ray = Mycamera.ScreenPointToRay(Input.mousePosition);//луч
				if (Physics.Raycast(ray,out hit)) {
					if (hit.collider != null)
					{
						if(hit.collider.gameObject.tag == "wertTouch")
						{
							тек_деталь.EditOffset(hit.collider.transform,offset.transform);
							тек_деталь.ReadPosition();
							RelogDetal();
							closeTouchVert();
							проекты.AddWindow();
							return;
						}
					}
				}
			}

			if (!TouchShkafVert && !TouchDetalVert && !EditDetalVert && !EditShkafVert)
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;          //Точка касания
			Ray ray = Mycamera.ScreenPointToRay(Input.mousePosition);//луч
			bool active = true;
			if (Physics.Raycast(ray,out hit)) {
				if (hit.collider != null && active)
				{   
					active =false;
					if(hit.collider.gameObject.tag == "boxColider")
					{
						if(тек_деталь != null) DiactiveDetal(тек_деталь);
						тек_деталь = hit.collider.transform.parent.transform.parent.GetComponent<Detal>();
						дистанция.SetDetal(тек_деталь);
						тек_деталь.select(true,прозрачность.isOn);
						тек_деталь.setPanelsPrisadka(Присадка);
						_camera.viewDetal();
							for(int i=0; i < детали.Length;i++)
							{
								if(тек_деталь == детали[i])
									list.value = i;
							}

						RelogDetal();
						return;
					}
				}
			}
		}
		}else
		{

			if (Input.GetMouseButtonDown(0)) {
				RaycastHit hit;          //Точка касания
				Ray ray = Mycamera.ScreenPointToRay(Input.mousePosition);//луч
				if (Physics.Raycast(ray,out hit)) {
					if (hit.collider != null)
					{   
						//active =false;
						if(hit.collider.gameObject.tag == "boxColider")
						{
							дистанция.addFindCenter(hit.collider.transform.parent.transform.parent.GetComponent<Detal>());
							return;
						}
					}
				}
			}
		}
	}



	public void viewTouchDetalVert()
	{
		if (TouchDetalVert)return;
		TouchDetalVert = true;
		if(тек_деталь != null)
		{
			for(int i=0; i < детали.Length;i++)
			{   
				if( детали[i] != тек_деталь) 
					детали[i].gameObject.SetActive(false);
		    }
			тек_деталь.wievTouchVert();
		}
	}

	GameObject tekShkafVert;
	public void viewTouchShkafVert()
	{
		if (TouchShkafVert)return;
		if(тек_деталь != null)
		{
			TouchShkafVert = true;
			тек_деталь.gameObject.SetActive(false);
			for(int i=0; i < offset.transform.childCount;i++)
			{   
				Transform temp =  offset.transform.GetChild(i);
				if(temp.tag == "wertTouch")
				{
					temp.gameObject.SetActive(true);
				}
			}
			for(int i = 0; i < детали.Length; i ++)
			{
				детали[i].viewTouch();
				детали[i].renderMesh(false);
			}

			тек_деталь.viewTochShkaf();

		}
	}


	public void closeTouchVert()
	{
		тек_деталь.hideTouchVert();
		for(int i=0; i < детали.Length;i++)
		{   
				детали[i].gameObject.SetActive(true);
		}

		for(int i=0; i < offset.transform.childCount;i++)
		{   
			Transform temp =  offset.transform.GetChild(i);
			if(temp.tag == "wertTouch")
			{
				temp.gameObject.SetActive(false);
			}
		}

		for(int i = 0; i < детали.Length; i ++)
		{
			детали[i].renderMesh(true);
			детали[i].hideTouchVert();
		}
		
		///тек_деталь.gameObject.SetActive(true);
		TouchDetalVert = false;
		TouchShkafVert = false;
		EditDetalVert = false;
		EditShkafVert = false;
		тек_деталь.HideTochShkaf();
		//if(tekShkafVert != null) GameObject.Destroy(tekShkafVert);
	}

	public void setPosition()
	{
		try{
			тек_деталь.PosX = float.Parse(x.text.Replace(",","."));
			тек_деталь.PosY = float.Parse(y.text.Replace(",","."));
			тек_деталь.PosZ = float.Parse(z.text.Replace(",","."));
			тек_деталь.SetPosition();
			проекты.AddWindow();
		}catch{}
	}

	public void RelogDetal()
	{
		x.text = тек_деталь.PosX.ToString();
		y.text = тек_деталь.PosY.ToString();
		z.text = тек_деталь.PosZ.ToString();

		dx.text = тек_деталь.DX.ToString();
		dy.text = тек_деталь.DY.ToString();
		dz.text = тек_деталь.DZ.ToString();

		nameDetal.text = тек_деталь.name;
	}

	public void RelogList()
	{
		list.options.Clear();


		for(int i=0; i < детали.Length;i++)
		{
			string name =детали[i].name;
			list.options.Add(new Dropdown.OptionData(name));
			if(тек_деталь == детали[i])
				list.value = i;
		}
//		if(тек_деталь == null) list.value = 100;
		if(тек_деталь == null)nameDetal.text ="";
		list.captionText.text = "Список деталей";

		if(тек_деталь != null)
		{
			тек_деталь.select(true,прозрачность.isOn);
		}

	}

	public void setDetalName()
	{
		if(nameDetal.text != "")
			тек_деталь.setName(nameDetal.text);
		RelogList();
		проекты.AddWindow();
	}
	public void СоздатьКопию()
	{
		if(тек_деталь == null) return;
		ArrayList temp = new ArrayList();
		for(int i =0; i< детали.Length;i++)
			temp.Add(детали[i]);
		DiactiveDetal(тек_деталь);

		Detal tempDetal = тек_деталь.Copy();
		temp.Add(tempDetal);

		детали = (Detal[]) temp.ToArray(typeof(Detal));
		setDetal(детали.Length-1);
		RelogList();
		проекты.AddWindow();
	}


	public void addDetal(Detal[] value)
	{
		ArrayList temp = new ArrayList();
		for(int i=0; i< детали.Length;i++)
			temp.Add(детали[i]);
		
		for(int j=0;j< value.Length;j++)
		{
			value[j].alfa = прозрачность.isOn;
			temp.Add(value[j]);
		}

		детали = (Detal[])temp.ToArray(typeof(Detal));
		RelogList();
		проекты.AddWindow();
	}

	public void DeleteDetal()
	{
		
		if(тек_деталь == null) return;
		DiactiveDetal(тек_деталь);
		GameObject.Destroy(тек_деталь.offset.gameObject);
		ArrayList temp = new ArrayList();

		for(int i = 0; i< детали.Length;i++)
			if(тек_деталь != детали[i])
				temp.Add(детали[i]);
		
		детали =  (Detal[])temp.ToArray(typeof(Detal));
		Destroy(тек_деталь.gameObject);
		RelogList();
		проекты.AddWindow();
	}



	public void setDetal(int value)
	{
		if(тек_деталь != null)
		{
			if(дистанция.FindDistans3)
			{
				if(тек_деталь == детали[value])return;
				дистанция.addFindCenter(детали[value]);
				return;
			}

			DiactiveDetal(тек_деталь);
		}
		тек_деталь = детали[value];
		тек_деталь.select(true,прозрачность.isOn);
		тек_деталь.setPanelsPrisadka(Присадка);
		_camera.viewDetal();
		RelogDetal();
	}

	public void setDetalSize()
	{
		тек_деталь.DX = float.Parse(dx.text.Replace(",","."));
		тек_деталь.DY = float.Parse(dy.text.Replace(",","."));
		тек_деталь.DZ = float.Parse(dz.text.Replace(",","."));
		тек_деталь.sizeSet();
		RelogDetal();
		проекты.AddWindow();
	}

	public void ВокругX()
	{
		if(тек_деталь == null)return;  
		тек_деталь.вращать("X");
		проекты.AddWindow();
	}

	public void ПеревернутьDX()
	{
		if(тек_деталь == null)return;  
		начать_замену_привязки();
		тек_деталь.перевернуть_DX();
		закончить_замену();
		проекты.AddWindow();
	}

	public void ПеревернутьDY()
	{
		if(тек_деталь == null)return;  
		начать_замену_привязки();
		тек_деталь.перевернуть_DY();
		закончить_замену();
		проекты.AddWindow();
	}

	public void ВокругY()
	{
		if(тек_деталь == null)return;
		тек_деталь.вращать("Y");
		проекты.AddWindow();
	}
	public void ВокругZ()
	{
		if(тек_деталь == null)return;
		тек_деталь.вращать("Z");
		проекты.AddWindow();
	}

	public void setAlfa(bool value)
	{
		for(int i=0; i < детали.Length;i++)
			детали[i].прозрачность(value);
	}

	void DiactiveDetal(Detal деталь)
	{
		деталь.select(false,прозрачность.isOn);
		деталь.hideTouchVert();
		closeTouchVert();
	}

	public void editParent()
	{
		if (EditDetalVert)return;
		EditDetalVert = true;
		if(тек_деталь != null)
		{
			for(int i=0; i < детали.Length;i++)
			{   
				if( детали[i] != тек_деталь) 
					детали[i].gameObject.SetActive(false);
			}
			тек_деталь.wievTouchVert();
		}
	}

	public void editOffset()
	{
		if (EditShkafVert)return;
		if(тек_деталь != null)
		{
			EditShkafVert = true;
			тек_деталь.gameObject.SetActive(false);
			for(int i=0; i < offset.transform.childCount;i++)
			{   
				Transform temp =  offset.transform.GetChild(i);
				if(temp.tag == "wertTouch")
				{
					temp.gameObject.SetActive(true);
				}
			}
			for(int i = 0; i < детали.Length; i ++)
			{
				детали[i].viewTouch();
				детали[i].renderMesh(false);
			}

			тек_деталь.viewTochShkaf();

		}
	}

	public void отвязать()
	{
		if(тек_деталь == null) return;
		тек_деталь.offset.gameObject.GetComponent<vertOffset>().ClearIndex();
	}

	void начать_замену_привязки()
	{
		GameObject[] go = GameObject.FindGameObjectsWithTag("vertOffset");
		for(int i=0;i < go.Length;i++)
			go[i].GetComponent<vertOffset>().начать_замену(тек_деталь);
	}

	void закончить_замену()
	{
		GameObject[] go = GameObject.FindGameObjectsWithTag("vertOffset");
		for(int i=0;i < go.Length;i++)
			go[i].GetComponent<vertOffset>().закончить_замену(тек_деталь);
	}


}
