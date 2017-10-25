using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class viewNumbarsPains : MonoBehaviour {
	
	public Toggle chek;
	public Transform[] numbers;
	public Detal деталь;
	Vector3 screenPos;

	public GUIStyle StyleFront;
	public GUIStyle StyleBack;
	public GUIStyle StyleLeft;
	public GUIStyle StyleRight;

	public void setNumbers()
	{
		if(деталь == null) деталь = gameObject.GetComponent<Detal>();

		numbers[0].localPosition = деталь.vert[8].transform.localPosition +new Vector3(-0.035f,0,0);
		numbers[1].localPosition = деталь.vert[9].transform.localPosition +new Vector3(0.035f,0,0);
		numbers[2].localPosition = деталь.vert[10].transform.localPosition +new Vector3(-0.035f,0,0);
		numbers[3].localPosition = деталь.vert[11].transform.localPosition +new Vector3(0.035f,0,0);

		numbers[4].localPosition = деталь.vert[8].transform.localPosition +new Vector3(0,0,0.035f);
		numbers[7].localPosition = деталь.vert[11].transform.localPosition +new Vector3(0,0,-0.035f);
		numbers[5].localPosition = деталь.vert[9].transform.localPosition +new Vector3(0,0,0.035f);
		numbers[6].localPosition = деталь.vert[10].transform.localPosition +new Vector3(0,0,-0.035f);
	}

	void OnGUI()
	{
		if(chek == null) chek = Camera.main.gameObject.GetComponent<присадка>().chek;


		if(деталь.selected && chek.isOn)
		{
			screenPos = Camera.main.WorldToScreenPoint(numbers[0].position);
			GUI.Label(new Rect(screenPos.x,Screen.height- screenPos.y,10,10),"2",StyleFront);
			screenPos = Camera.main.WorldToScreenPoint(numbers[1].position);
			GUI.Label(new Rect(screenPos.x,Screen.height- screenPos.y,10,10),"1",StyleFront);

			screenPos = Camera.main.WorldToScreenPoint(numbers[2].position);
			GUI.Label(new Rect(screenPos.x,Screen.height- screenPos.y,10,10),"2",StyleBack);
			screenPos = Camera.main.WorldToScreenPoint(numbers[3].position);
			GUI.Label(new Rect(screenPos.x,Screen.height- screenPos.y,10,10),"1",StyleBack);

			screenPos = Camera.main.WorldToScreenPoint(numbers[4].position);
			GUI.Label(new Rect(screenPos.x,Screen.height- screenPos.y,10,10),"1",StyleRight);
			screenPos = Camera.main.WorldToScreenPoint(numbers[6].position);
			GUI.Label(new Rect(screenPos.x,Screen.height- screenPos.y,10,10),"2",StyleRight);


			screenPos = Camera.main.WorldToScreenPoint(numbers[5].position);
			GUI.Label(new Rect(screenPos.x,Screen.height- screenPos.y,10,10),"1",StyleLeft);
			screenPos = Camera.main.WorldToScreenPoint(numbers[7].position);
			GUI.Label(new Rect(screenPos.x,Screen.height- screenPos.y,10,10),"2",StyleLeft);
		}
	}

	void Update () {
		
	}
}
