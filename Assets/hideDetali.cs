using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideDetali : MonoBehaviour {

	public bool hide;
	public Camera Mycamera;
	public Feditor editor;

	bool keyHide;
	Vector3 mousePos;

	void Update () {
		
		if (Input.GetKeyDown(KeyCode.LeftControl)) keyHide = true;
		if (Input.GetKeyUp(KeyCode.LeftControl) && keyHide == true) keyHide = false;

		if (hide)
		{
			if (Input.GetMouseButtonDown(0)) mousePos = Input.mousePosition;
		    if (Input.GetMouseButtonUp(0))  if(mousePos == Input.mousePosition) hideDetal();
		}

		if(keyHide && Input.GetMouseButtonUp(0)) hideDetal();
	}

	void hideDetal()
	{
		RaycastHit hit;          //Точка касания
		Ray ray = Mycamera.ScreenPointToRay(Input.mousePosition);//луч
		bool active = true;
		if (Physics.Raycast(ray,out hit)) {
			if (hit.collider != null && active)
			{   
				active =false;

					if(hit.collider.gameObject.tag == "boxColider")
					{
						hit.collider.transform.parent.gameObject.SetActive(false);
						mousePos = Vector3.zero;
						hide = false;
					}
			}
		}
	}

	public void startHide()
	{
		hide = true;
	}

	public void View()
	{
		if(editor.тек_деталь == null) return;
		editor.тек_деталь.parent.SetActive(true);
	}

	public void ViewAll()
	{
		for(int i=0; i < editor.детали.Length ;i++)
			editor.детали[i].parent.SetActive(true);
	}

}
