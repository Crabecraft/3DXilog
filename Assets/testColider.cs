using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testColider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider colider)
	{

		Debug.Log(colider.gameObject.name);

	}
}
