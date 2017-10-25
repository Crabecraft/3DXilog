using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ПраваяСторона : присадкаСтороны {

	public override void Rotate(Transform transform)
	{
		transform.Rotate(new Vector3(0,180,0));
	}

	public override void GetPrisadka (ref string value)
	{
		for(int i=0; i < stroka.Length;i++)
		{
			string[] temp = readStr(i);

			if((temp[0] == "0" || temp[0] == "") && temp[2] == "1")
				temp[0] = (деталь.DY/2).ToString();
			
			if(!stroka[i].Contains("Зона 1")) 
			{ 
				temp[0] = "DY-" + temp[0];
				if(temp[1] != "0")
				temp[1] = "-" + temp[1];
			}

			if(stroka[i].Contains("Эксцентрик"))
			{
				value += "XBO X="+ getStrOtcraya().ToString() +" Y="+ temp[0] +" Z=26 D=8 R="+ temp[2] +" x=0 y="+ temp[1] +" N=\"P\" F=2 K=0 P=0\n";
				value += "XBO X=DX-34 Y="+ temp[0] +" Z=13 D=15 R="+ temp[2] +" x=0 y="+ temp[1] +" N=\"P\" F=1 K=0 P=0\n";
			}
			else
				if(stroka[i].Contains("Шкант"))
					value += "XBO X="+ getStrOtcraya().ToString() +" Y="+ temp[0] +" Z=26 D=8 R="+ temp[2] +" x=0 y="+ temp[1] +" N=\"P\" F=2 K=0 P=0\n";
				else
					if(stroka[i].Contains("Конфирмат"))
						value += "XBO X="+ getStrOtcraya().ToString() +" Y="+ temp[0] +" Z=34 D=5 R="+ temp[2] +" x=0 y="+ temp[1] +" N=\"P\" F=2 K=0 P=0\n";
		}
	}
}
