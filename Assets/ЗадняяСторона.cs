using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ЗадняяСторона : присадкаСтороны {

	public override void Rotate(Transform transform)
	{
		transform.Rotate(new Vector3(0,90,0));
	}

	public override Vector3 getVector (float value)
	{
		return new Vector3(value,getOtcraya(),0);
	}

	public override string пополам()
	{
		return (деталь.DX/2).ToString();
	}

	public override void GetPrisadka (ref string value)
	{
		for(int i=0; i < stroka.Length;i++)
		{
			string[] temp = readStr(i);

			if((temp[0] == "0" || temp[0] == "") && temp[2] == "1")
				temp[0] = (деталь.DX/2).ToString();
			
			if(!stroka[i].Contains("Зона 1")) 
			{ 
				temp[0] = "DX-" + temp[0];
				if(temp[1] != "0")
				temp[1] = "-" + temp[1];
			}

			if(stroka[i].Contains("Эксцентрик"))
			{
				value += "XBO X="+ temp[0] +" Y="+ getStrOtcraya().ToString() +" Z=26 D=8 R="+ temp[2] +" x="+ temp[1] +" y=0 N=\"P\" F=4 K=0 P=0\n";
				value += "XBO X="+ temp[0] +" Y=DY-34 Z=13 D=15 R="+ temp[2] +" x="+ temp[1] +" y=0 N=\"P\" F=1 K=0 P=0\n";
			}
			else
				if(stroka[i].Contains("Шкант"))
					value += "XBO X="+ temp[0] +" Y="+ getStrOtcraya().ToString() +" Z=26 D=8 R="+ temp[2] +" x="+ temp[1] +" y=0 N=\"P\" F=4 K=0 P=0\n";
			    else
				if(stroka[i].Contains("Конфирмат"))
					value += "XBO X="+ temp[0] +" Y="+ getStrOtcraya().ToString() +" Z=34 D=5 R="+ temp[2] +" x="+ temp[1] +" y=0 N=\"P\" F=4 K=0 P=0\n";
		}
	}

	public override string getShag(string начало,string шаг,string количество)
	{
		if(шаг != "0") return шаг;
		try{
			if(количество != "1") return Math.Round(((деталь.DX-(float.Parse(начало)*2))/(int.Parse(количество)-1)),1).ToString();
		}catch{}
		return "0";
	}
}
