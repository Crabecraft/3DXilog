using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class addDetalWindow : MonoBehaviour {
	public InputField textDX;
	public InputField textDY;
	public InputField textDZ;
	public InputField count;

	public Feditor editor;

	public Transform шкаф;

	public GameObject базаДеталь;

	public Размеры_шкафа габариты_шкафа;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Show()
	{
		gameObject.SetActive(true);
		textDX.text = textDY.text ="";
		textDZ.text = "18";
		count.text = "1";
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}

	public void addDetal()
	{
		if(textDX.text ==  "" || textDY.text ==  "" || textDZ.text ==  "") return;
		Detal[] list = new Detal[int.Parse(count.text)];

		for(int i=0;i < list.Length;i++)
		{
			GameObject temp = GameObject.Instantiate(базаДеталь) as GameObject;
			temp.transform.SetParent (шкаф);
			temp.transform.localPosition = new Vector3(0,0,0);
			temp.transform.localRotation =  new Quaternion(0,0,0,0);
			list[i] = temp.GetComponent<Detal>();
			list[i].DX = float.Parse(textDX.text);
			list[i].DY = float.Parse(textDY.text);
			list[i].DZ = float.Parse(textDZ.text);
			list[i].setOffset(габариты_шкафа.vertex[1].transform,шкаф);
			list[i].sizeSet();
			list[i].setParent(list[i].vert[13]);
			list[i].setName("");
		}
		Close();
		editor.addDetal(list);

	}
}
