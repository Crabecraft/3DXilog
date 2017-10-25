using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class эксцентрик : крепеж {



}

public class крепеж: MonoBehaviour {
	public Transform centr;
	public Transform outRay,centerRay;
	public Transform coliderStart;
	public bool Остриие;
	public float диаметр;
	public float глубинаСверления;
	public virtual void Reset(float value){}

	public void Ray()
	{
		RaycastHit hit;          //Точка касания
	//	Ray ray = new Ray()//луч
		if(Physics.Raycast(outRay.position,-outRay.right,out hit,0.01f)) 
		{
			if (hit.collider != null)
			{
				if(hit.collider.transform.parent.parent.gameObject.tag == "Detal")
				{
					Detal temp = hit.collider.transform.parent.parent.gameObject.GetComponent<Detal>();
					temp.add_krepej(transform);
					return;
				}
			}
		}
	}

	public virtual void test()
	{
		outRay.gameObject.SetActive(false);
		RaycastHit[] hit;  
		hit =  Physics.RaycastAll(outRay.position,-outRay.right,0.023f);
		if(hit == null) return;
		for(int i=0; i < hit.Length;i++)
		{
			if(hit[i].transform.parent.gameObject.tag == "крепеж")
			{	
				if(hit[i].transform.gameObject != gameObject)
				{
					outRay.gameObject.SetActive(true);
					break;
				}
			}
		}

		hit =  Physics.RaycastAll(coliderStart.position,coliderStart.forward,0.006f);
		if(hit == null) return;
		for(int i=0; i < hit.Length;i++)
		{
			if(hit[i].transform.parent.gameObject.tag == "крепеж")
			{	
				if(hit[i].transform.gameObject != gameObject)
				{
					outRay.gameObject.SetActive(true);
					break;
				}
			}
		}

		hit =  Physics.RaycastAll(coliderStart.position,-coliderStart.forward,0.006f);
		if(hit == null) return;
		for(int i=0; i < hit.Length;i++)
		{
			if(hit[i].transform.parent.gameObject.tag == "крепеж")
			{	
				if(hit[i].transform.gameObject != gameObject)
				{
					outRay.gameObject.SetActive(true);
					break;
				}
			}
		}
	}

}