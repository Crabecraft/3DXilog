using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class присадка : MonoBehaviour {
	public Toggle chek ;
	public Feditor editor;
	public GameObject panel;

	public GameObject miniWindow;

	public панельПрисадки левая,правая,передняя,задняя;

	public void view(bool value)
	{
		if(value && editor.тек_деталь != null)
		{
			panel.SetActive(true);

		}else if(!value)
			{
				panel.SetActive(false);
			}
	}

//	public void setDetal(Detal деталь)
//	{
//		деталь.
//		левая = деталь.левая;
//		правая = деталь.правая;
//		передняя = деталь.передняя;
//		задняя = деталь.задняя;	
//	}

}
