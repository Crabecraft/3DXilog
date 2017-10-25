using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class movCam2 : MonoBehaviour {

	public Transform parentX;
	public Transform parentY;

	public float scrollSpeed;

	bool isActivatedXY;

	bool выравнивание;
	Vector3 вектор_выравнивания;
	public float speed;

	float speedX;
	float speedY;

	void Update () {
		
	}

	public void offset()
	{
		parentX.localPosition = new Vector3(0,0,0);
		parentY.localPosition = new Vector3(0,0,0);
		parentX.localRotation = new Quaternion(0,0,0,0);
		parentY.localRotation = new Quaternion(0,0,0,0);
		transform.localPosition= new Vector3(0,0,-2.5f);
	}

	void StartXY()
	{
		speedX = (transform.localPosition.x/50*speed);
		speedY = (transform.localPosition.y/50*speed);
		выравнивание = true;
	}

	public void viewDetal()
	{
	   Detal тек_деталь = gameObject.GetComponent<Feditor>().тек_деталь;
	   transform.parent = parentX.parent;
	   parentX.position = тек_деталь.vert[25].transform.position;
	   transform.parent = parentY.transform;
		StartXY();
	  // transform.localPosition = new Vector3(0,0,transform.localPosition.z);
	}
	void Start () 
	{
		
	}

	void Выравнивание()
	{
		if(выравнивание){
			float tempX = transform.localPosition.x;
			float tempY = transform.localPosition.y;

		
			if((tempX >0.07f || tempX <-0.07f) || (tempY >0.07f || tempY <-0.07f))
			{		tempX-= speedX*Time.deltaTime;
			        tempY-= speedY*Time.deltaTime;
			}
			else
				выравнивание = false;

			transform.localPosition = new Vector3(tempX,tempY,transform.localPosition.z);
		}

	}

	void LateUpdate()
	{
		if (Input.GetMouseButtonDown(2))
		{ 
			isActivatedXY = true;
		}
		if (Input.GetMouseButtonUp(2))
			isActivatedXY = false;


		if (Input.GetMouseButton(1))
		{
			float asixX = Input.GetAxis("Mouse X");
			float asixY = Input.GetAxis("Mouse Y");

			if(asixX < 0) asixX*=-1;
			if(asixY < 0) asixY*=-1;

			Выравнивание();

			if(asixX > asixY)
			{
				parentX.Rotate(0,Input.GetAxis("Mouse X") *3,0);
			}
			else
			{
				parentY.Rotate(-Input.GetAxis("Mouse Y") *3,0,0);
			}
	 
		}

		if (Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			transform.localPosition = transform.localPosition + new Vector3(0,0,Input.GetAxis("Mouse ScrollWheel")*scrollSpeed);
		}

		if(isActivatedXY)
		{
			transform.localPosition += new Vector3(-Input.GetAxis("Mouse X")/20,-Input.GetAxis("Mouse Y")/20);
			выравнивание = false;
		}
	}
}
