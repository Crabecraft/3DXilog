using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Security.Cryptography.X509Certificates;


public class ListBox2 : MonoBehaviour {

//	bool one_click = false;
//	float timer_for_double_click;
	public int ШиринаОкна;
	public int ВысотаОкна;
	public int ВысотаСтроки;
	public string[] Items;

	Rect window;
	public int seletcItem = -1;
	Rect[] RectItem;
	Vector3 screenPos;

	float dubleClickTime;

//	public Color TextColor;
//	public Color BackGroundColor;
//
	public ListBoxClickEvent Click;
	public ListBoxClickEvent DubleClick;

	public GUIStyle stil;
	public GUIStyle stilSelect;
	void Update () 
	{

		dubleClickTime -= Time.deltaTime;

//		if(Input.GetMouseButtonDown(0))
//		{
//			if(!one_click)
//			{
//				one_click = true;
//				timer_for_double_click = Time.time;
//			}
//			else
//			{
//				one_click = false;
//			}
//		}
//
//		if(one_click)
//		{
//			if((Time.time - timer_for_double_click) > 0.2f)
//			{
//				one_click = false;
//			}
//
//		}
	}

	void Start()
	{
		screenPos = Camera.main.WorldToScreenPoint(transform.position);
	}

	public void add(string value)
	{
		ArrayList temp = new ArrayList();
		for(int i=0;i < Items.Length;i++)
			temp.Add(Items[i]);
		temp.Add(value);
		Items = (string[]) temp.ToArray(typeof(string));
	}

	public void RemoveItem(int index)
	{
		ArrayList temp = new ArrayList();
		for(int i=0;i < Items.Length;i++)
			if(i != index) temp.Add(Items[i]);
		Items = (string[]) temp.ToArray(typeof(string));
	}

	public void Clear()
	{
		Items = new string[0];
		seletcItem = -1;
	}

	void OnGUI()
	{
		RectItem = new Rect[Items.Length];
		for(int i=0; i < Items.Length;i++)
			RectItem[i] = new Rect(screenPos.x,Screen.height- screenPos.y+ВысотаСтроки*(i+1)- ВысотаСтроки,ШиринаОкна,ВысотаСтроки); 

		window = new Rect(screenPos.x,Screen.height- screenPos.y,ШиринаОкна,ВысотаОкна);
		if(Event.current.isMouse && Event.current.button == 0 && dubleClickTime <= 0)
		{
			Vector2 mouse = new Vector2(Input.mousePosition.x,Screen.height-Input.mousePosition.y);
			if(window.Contains(mouse))
			{
				for(int i=0; i < RectItem.Length;i++)
					if(RectItem[i].Contains(mouse))
					{
						seletcItem  = i;
						if(Event.current.clickCount > 1)
						{	
							dubleClickTime = 0.3f;
							DubleClick.Invoke(i);
						}
						else
							Click.Invoke(i);
						break;
			        }
		   }
		}


	   


		for(int i=0; i < Items.Length;i++)
		{   if(seletcItem == i)
			GUI.Label(RectItem[i],Items[i],stilSelect);
			else
			GUI.Label(RectItem[i],Items[i],stil);
	    }
	}

}

[Serializable]
public class ListBoxClickEvent : UnityEvent<int>
{
//	public ButtonClickedEvent (){}
}