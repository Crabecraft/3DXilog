using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class полкодержатель : крепеж {
	public float ДопСмещение;

    public override void Reset (float value)
	{
		centr.localPosition = new Vector3(0,-value/1000/2-ДопСмещение/1000,0);
	}

	public override void test ()
	{
		outRay.gameObject.SetActive(false);
		RaycastHit[] hit;  
		hit =  Physics.RaycastAll(centr.position,centr.up,0.007f);

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
