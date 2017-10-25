using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalTransform : MonoBehaviour {
	public Vector3 position;
	public Transform dsp;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void FixedUpdate () {
		dsp.localPosition = new Vector3 (position.x/1000 + transform.localScale.x / 2, position.y/1000 + transform.localScale.y / 2, position.z/1000 + transform.localScale.z / 2);
	}
}
