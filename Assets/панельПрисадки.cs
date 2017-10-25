using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class панельПрисадки : MonoBehaviour {

	public ListBox2 list;
	public присадкаСтороны сторона;
	public miniWindow addWindow;
	public БуфурОбмена Буфер;
	public ManagerProects проекты;
	public void addShow()
	{
		addWindow.сторона = сторона;
		addWindow.load(-1);
	}

	public void loadMod(int value)
	{
		addWindow.сторона = сторона;
		addWindow.load(value);
	}

	public void удалить()
	{
		сторона.удалить();
		проекты.AddWindow();
	}

	public void копировать()
	{
		if(list.seletcItem > -1)
		Буфер.value  = list.Items[list.seletcItem];
	}

	public void вставить()
	{
		if(Буфер.value.Trim() == "") return;
		сторона.add(Буфер.value);
		проекты.AddWindow();
	}
}
