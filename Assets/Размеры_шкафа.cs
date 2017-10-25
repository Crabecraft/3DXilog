using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Размеры_шкафа : MonoBehaviour {
	public float ширина,высота,глубина;
	public GameObject[] vertex;
	public ManagerProects проекты;
	public InputField DX,DY,DZ;

	public Transform offset;
	void Start () {
		reset_pos();
	}


	void SetSize()
	{
		float fdx,fdy,fdz;

		fdx = ширина;
		fdy = высота;
		fdz = глубина;

		offset.localPosition = new Vector3(fdx/1000/2, -fdy/1000/2,-fdz/1000/2);

		vertex[0].transform.localPosition = new Vector3(-fdx/1000,0,0);
		vertex[1].transform.localPosition = new Vector3(0,0,0);
		vertex[2].transform.localPosition = new Vector3(0,fdy/1000,0);
		vertex[3].transform.localPosition = new Vector3(-fdx/1000,fdy/1000,0);

		vertex[4].transform.localPosition = new Vector3(-fdx/1000,0,fdz/1000);
		vertex[5].transform.localPosition = new Vector3(0,0,fdz/1000);
		vertex[6].transform.localPosition = new Vector3(0,fdy/1000,fdz/1000);
		vertex[7].transform.localPosition = new Vector3(-fdx/1000,fdy/1000,fdz/1000);

		vertex[8].transform.localPosition = new Vector3(-fdx/1000/2,0,fdz/1000);
		vertex[9].transform.localPosition = new Vector3(0,fdy/1000/2,fdz/1000);
		vertex[10].transform.localPosition = new Vector3(-fdx/1000/2,fdy/1000,fdz/1000);
		vertex[11].transform.localPosition = new Vector3(-fdx/1000,fdy/1000/2,fdz/1000);

		vertex[12].transform.localPosition = new Vector3(-fdx/1000/2,0,0);
		vertex[13].transform.localPosition = new Vector3(0,fdy/1000/2,0);
		vertex[14].transform.localPosition = new Vector3(-fdx/1000/2,fdy/1000,0);
		vertex[15].transform.localPosition = new Vector3(-fdx/1000,fdy/1000/2,0);

		vertex[16].transform.localPosition = new Vector3(-fdx/1000/2,fdy/1000/2,fdz/1000);
		vertex[17].transform.localPosition = new Vector3(-fdx/1000/2,fdy/1000/2,fdz/1000/2);
		vertex[18].transform.localPosition = new Vector3(-fdx/1000/2,fdy/1000/2,0);

		vertex[19].transform.localPosition = new Vector3(-fdx/1000/2,0,fdz/1000/2);
		vertex[20].transform.localPosition = new Vector3(-fdx/1000/2,fdy/1000,fdz/1000/2);

		for(int i =0;i< offset.childCount;i++)
		{	
			Transform temp = offset.GetChild(i);
			if(temp.gameObject.tag == "Detal")
				temp.gameObject.GetComponent<Detal>().SetPosition();
		}


	}

	public void Set(Vector3 s)
	{
		ширина = s.x;
		высота = s.y;
		глубина =s.z;

		DX.text = ширина.ToString();
		DY.text = высота.ToString();
		DZ.text = глубина.ToString();

		SetSize();
	}

	public void reset_pos()
	{
		try{
			 ширина = float.Parse(DX.text);
			 высота = float.Parse(DY.text);
			 глубина = float.Parse(DZ.text);
	   }catch{}

		SetSize();
		проекты.AddWindow();
	}

	void Update () {
		
	}
}
