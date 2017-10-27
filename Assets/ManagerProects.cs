using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.ComponentModel;
using System.Threading;
using System.Security.Cryptography;

public class ManagerProects : MonoBehaviour {

	public movCam камера;
	public Feditor editor;
	public Размеры_шкафа шкаф;
	public GameObject базаДеталь;
	public GameObject панель;
	public InputField stroka;
	public ListBox2 list;
	int mod;
	int tekWindow = 0;
	float  timer;
	ArrayList windows = new ArrayList();

	public Project CreateProject()
	{
		Project project = new Project(){размеры_шкафа = new Vector3(шкаф.ширина,шкаф.высота,шкаф.глубина)};

		ArrayList детали = new ArrayList();

		for(int i=0; i < editor.детали.Length;i++)
			детали.Add(new SerializableDetal().StartSerializableDetal(editor.детали[i]));

		project.детали = (SerializableDetal[]) детали.ToArray(typeof(SerializableDetal));

		return project;
	}

	void Update ()
	{
		if(timer > 0)
		{
			timer-=Time.deltaTime;
			if(timer<=0)saveWindiw();
		}
	}
	public void startSave()
	{
		панель.SetActive(true);
		refreshList("");
		mod = 1;
	}

	public void startLoad()
	{
		панель.SetActive(true);
		refreshList("");
		mod = 2;
	}

	public void Next()
	{
		if(!(tekWindow < windows.Count-1)) return;
		tekWindow++;
		LoadProject((Project)windows[tekWindow]);
	}

	public void Back()
	{
		if(tekWindow == 0) return;
		tekWindow--;
		LoadProject((Project)windows[tekWindow]);
	}

	public void AddWindow()
	{
		timer = 0.5f;
	}

	void saveWindiw()
	{
		if(windows == null) windows = new ArrayList();
		windows.Add(CreateProject());
		tekWindow = windows.Count-1;
	}

	public void OkClic()
	{
		if(stroka.text == "") return;

		if(mod == 1)
		{
			string patch = "C:Projects\\" + stroka.text + ".xml";
			if(!Directory.Exists("C:Projects"))
				Directory.CreateDirectory("C:Projects");
			SaveToPatch(patch,CreateProject());
		}
		else if(mod == 2)
		{
			string patch = "C:Projects\\" + stroka.text + ".xml";
			LoadProject(LoadFromPatch(patch));
		}

		mod = 0;
		панель.SetActive(false);
	}

	public void Cancel()
	{
		панель.SetActive(false);
		mod = 0;
	}

	public void RefreshStroka(int index)
	{
		try{
		stroka.text = list.Items[index];
		}catch{}
	}

	public void refreshList(string text)
	{
		string patch = "C:Projects";
		if(!Directory.Exists(patch)) return;

		string[] files = Directory.GetFiles(patch);
		list.Clear();


		if(text == "")
			for(int i=0; i < files.Length; i++)
			{
				string[] fileNameMass = files[i].Split('\\');
				list.add(fileNameMass[fileNameMass.Length-1].Replace(".xml",""));
			}
	   else
		for(int i=0; i < files.Length; i++)
				if(files[i].ToUpper().Contains(text.ToUpper()))
			{
				string[] fileNameMass = files[i].Split('\\');
				list.add(fileNameMass[fileNameMass.Length-1].Replace(".xml",""));
			}

	}

	public void SaveToPatch(string patch,Project project)
	{			
		try{	 
		File.Delete(patch);	
		XmlSerializer formatter = new XmlSerializer(typeof(Project));
		using (FileStream fs = new FileStream(patch, FileMode.OpenOrCreate))
		{
			formatter.Serialize(fs, project);
		}
		}catch{}
	}

	public Project LoadFromPatch(string patch)
	{
		windows = new ArrayList();
		Project project = new Project();
		//if (!File.Exists(patch)) return ;
		try
		{
			XmlSerializer formatter = new XmlSerializer(typeof(Project));
			using (FileStream fs = new FileStream(patch, FileMode.OpenOrCreate))
			{
				project = (Project)formatter.Deserialize(fs);
			}
		}
		catch { }
		windows.Add(project);
		tekWindow = 0;
		камера.offset();
		return project;
	}

	public void LoadProject(Project project)
	{
		шкаф.Set(project.размеры_шкафа);
		ArrayList spisok = new ArrayList();
		for(int i =0;  i < project.детали.Length;i++)
		{
		  GameObject temp = GameObject.Instantiate(базаДеталь) as GameObject;
		  temp.transform.SetParent (editor.offset.transform);
		  temp.transform.localPosition = new Vector3(0,0,0);
		  temp.transform.localRotation =  new Quaternion(0,0,0,0);
		  Detal деталь = temp.GetComponent<Detal>();
			деталь.DX = project.детали[i].size.x;
			деталь.DY = project.детали[i].size.y;
			деталь.DZ = project.детали[i].size.z;
			деталь.sizeSet();

			деталь.setParent(деталь.vert[project.детали[i].tekVert]);
			деталь.scalleToch = project.детали[i].scaleOffset;
			temp.name = project.детали[i].name;
			деталь.PosX = project.детали[i].position.x;
			деталь.PosY = project.детали[i].position.y;
			деталь.PosZ = project.детали[i].position.z;
			деталь.прозрачность(editor.прозрачность.isOn);
			деталь.SetPosition();
			деталь.parent.transform.Rotate(project.детали[i].rotation);

			деталь.левая.stroka = project.детали[i].Левая;
			деталь.правая.stroka = project.детали[i].Правая;
			деталь.передняя.stroka = project.детали[i].Передняя;
			деталь.задняя.stroka = project.детали[i].Задняя;
			деталь.id = project.детали[i].id;
			деталь.обновить_присадку();

			spisok.Add(деталь);
		}

		if(editor.детали != null) 
		{ 
			for(int i=0; i  <editor.детали.Length;i++)
			{
				GameObject.Destroy(editor.детали[i].offset.gameObject);
				GameObject.Destroy(editor.детали[i].gameObject);
			}
		}

		editor.детали = (Detal[]) spisok.ToArray(typeof(Detal));



		for(int i =0;  i < project.детали.Length;i++)
		{
			


			GameObject offsetDetal = new GameObject();
			offsetDetal.transform.parent  = editor.offset.transform;
			offsetDetal.transform.localPosition = project.детали[i].offset;
			offsetDetal.transform.localScale = project.детали[i].scaleOffset;
			editor.детали[i].setOffset(offsetDetal.transform,editor.offset.transform);
			GameObject.Destroy(offsetDetal);

			vertOffset vertoffset = editor.детали[i].offset.gameObject.GetComponent<vertOffset>();
			vertoffset.indexOffsetDetal = project.детали[i].indexOffsetDetal;
			vertoffset.indexVert = project.детали[i].indexVert;
			if( project.детали[i].indexVert < 0) 
			{
				vertoffset.ClearIndex();
				continue;
			}

		    if(project.детали[i].indexOffsetDetal == "-1")
			 vertoffset.offset = шкаф.vertex[project.детали[i].indexVert].transform;
		    else
			{
				for(int j=0;j < editor.детали.Length; j ++)
					if(project.детали[i].indexOffsetDetal == editor.детали[j].id)
					{
				       vertoffset.offset = editor.детали[j].vert[project.детали[i].indexVert].transform.GetChild(0);
					   break;
					}
			}
		}

		editor.RelogList();

	}

}
