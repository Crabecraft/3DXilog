using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colison : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		try{
		GameObject temp = other.transform.parent.parent.gameObject;
		if(temp.tag != "Detal") return;
			if(transform.parent.parent.parent.parent.parent.parent.gameObject != temp)
			  temp.GetComponent<Detal>().add_krepej(transform.parent);
		}catch{}
	}

	void OnTriggerExit(Collider other)
	{
		try{
		GameObject temp = other.transform.parent.parent.gameObject;
		if(temp.tag != "Detal") return;
			if(transform.parent.parent.parent.parent.parent.parent.gameObject != temp)
			temp.GetComponent<Detal>().delete_krepej(transform.parent);
		}catch{}
	}
}
