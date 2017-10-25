using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class miniWindow : MonoBehaviour {

	public Dropdown зона,наименование;
	public InputField начало,шаг,количество;
	public ManagerProects проекты;
	public присадкаСтороны сторона;
	int index;

	public void load(int value)
	{
		index = value;
		if(value > -1)
		{
			relog();
		}
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public void save()
	{
		string temp = createStroka();
		if(temp == "") return;
		if(index > -1)
		{	
			if(сторона.stroka[index] != temp)
			{
				сторона.stroka[index] = temp;
				сторона.relogStroka(index);
			}
		}else
			сторона.add(temp);
		
		Hide();
		проекты.AddWindow();
	}

	void relog()
	{
		string temp = сторона.stroka[index];

		if(!temp.Contains("Зона 1")) 
		{
			зона.captionText.text = "Зона 2";
			temp = temp.Replace("(Зона 2)","");
		}else
		{
			зона.captionText.text = "Зона 1";
			temp = temp.Replace("(Зона 1)","");
		}

		for(int i=0; i < наименование.options.Count;i++)
			if(temp.Contains(наименование.options[i].text))
			{
				наименование.value = i;
				наименование.captionText.text = наименование.options[i].text;
				temp = temp.Replace(наименование.options[i].text,"");
				break;
			}

		string[] tempMass = temp.Split('(');

		tempMass[0] = tempMass[0].Trim();
		начало.text = tempMass[0].Split('-')[0];
		шаг.text    = tempMass[0].Split('-')[1];
		количество.text = tempMass[1].Split('ш')[0].Trim();
	}

	//		"(Зона 2)  эксцентрик 33-224(2 шт.)"

	 string createStroka()
	{		
		if(зона.options[зона.value].text == "" || наименование.options[наименование.value].text == "" || количество.text == "") return "";

			string текШаг= "0";
			string текНачало= "0";
		if(начало.text != "") текНачало = начало.text.Replace(",",".");
		if(шаг.text != "") текШаг = шаг.text.Replace(",",".");

		return "("+ зона.options[зона.value].text +") "+наименование.options[наименование.value].text +" "+текНачало+"-"+текШаг + "("+ количество.text +"шт.)";
	}
}
