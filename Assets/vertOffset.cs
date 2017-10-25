using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vertOffset : MonoBehaviour {

	public Transform offset;
	public long indexOffsetDetal;
	public int indexVert;
	Detal деталь_привязки;

	Transform cashOffset;

	void Update () {
		if(offset != null)
			transform.position = offset.position;
		else
			ClearIndex();
	}

	public void ClearIndex()
	{
		indexOffsetDetal = indexVert = -1;
		offset = null;
	}

	public void SetOffset(Transform point)
	{
		Feditor editor = Camera.main.GetComponent<Feditor>();
		Размеры_шкафа шкаф = editor.offset.transform.parent.gameObject.GetComponent<Размеры_шкафа>();
		offset = point;
		for(int i=0; i < шкаф.vertex.Length;i++)
			if(шкаф.vertex[i].transform == point)
			{
				indexOffsetDetal = -1;
				indexVert = i;
				return;
			}

		for(int i=0; i < editor.детали.Length;i++)
			for(int j=0; j < editor.детали[i].vert.Length;j++)
			  if(editor.детали[i].vert[j].transform.GetChild(0).transform == point)
			{ 
				indexOffsetDetal = editor.детали[i].id;
				indexVert = j;
				return;
			}

		//indexOffsetDetal = -1;
		//indexVert = -1;
	}

	public void начать_замену(Detal деталь)
	{
		if(offset == null) return;
		try{
			
	    	 if(offset.parent.parent.parent.parent.gameObject == деталь.gameObject)
		    {
			  cashOffset = offset;
		      offset = null;
		      деталь_привязки = деталь;
		    }

		}catch{}
	}

	public void закончить_замену(Detal деталь)
	{
		if(деталь_привязки == null)return;
		if(деталь_привязки != деталь) return;

		for(int i=0; i < деталь.vert.Length;i++)
		{
			Transform temp = деталь.vert[i].transform.GetChild(0);
			if(transform.position == temp.position)
			{
				SetOffset(temp);
				break;
			}
		}

		деталь_привязки = null;
	}
}
