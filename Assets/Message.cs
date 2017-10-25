using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour {
	public Text text;
	public void Show(string text)
	{
		this.text.text = text;
		gameObject.SetActive(true);
	}
	public void Close()
	{
		gameObject.SetActive(false);
		text.text = "";
	}
}
