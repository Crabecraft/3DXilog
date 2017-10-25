using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inputList : MonoBehaviour {

	public Dropdown myDropdown; // declare your dropdown
	public Feditor editor;
  

	void Start()
	{
		
	}
	public void Dropdown_IndexChanged(int index)
	{
		editor.setDetal(index);
	}
}
