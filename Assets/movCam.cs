using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movCam : MonoBehaviour {
	public float speed;

    public Transform шкаф;
	public Vector3 деталь;
	public Transform Система_координат;
    public float xSpeed = 12.0f;
    public float ySpeed = 12.0f;
    public float scrollSpeed = 10.0f;

	Vector3 tekPoint;

    public float zoomMin = 1.0f;

    public float zoomMax = 20.0f;

    public float distance;

    public Vector3 position;

    public bool isActivated;
	public bool isActivatedXY;
	float x,y;


	void Start () 
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
	}

	public void offset()
	{
		transform.localRotation = new Quaternion(0,0,0,0);
		transform.rotation = new Quaternion(0,0,0,0);
		transform.Rotate(new Vector3(0,360,0));
		transform.localPosition = new Vector3(0,0,-2);
	}

	public void viewDetal()
	{
		Detal тек_деталь = gameObject.GetComponent<Feditor>().тек_деталь;
		деталь = тек_деталь.vert[25].transform.position;
		transform.position = new Vector3(деталь.x,деталь.y,деталь.z-1);
	}


	void Update () 
	{
		

	}

    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
            isActivated = true;
        if (Input.GetMouseButtonUp(1))
            isActivated = false;
		
		if (Input.GetMouseButtonDown(2))
		  { 
			isActivatedXY = true;
			tekPoint = Input.mousePosition;
		  }
		if (Input.GetMouseButtonUp(2))
			isActivatedXY = false;

        if (Input.GetMouseButton(1))
        {

			float stepX = Input.GetAxis("Mouse X");
			float stepY = Input.GetAxis("Mouse Y");
			if(stepX < 0) stepX*=-1;
			if(stepY < 0) stepY*=-1;



		    if(stepX > stepY)
		    {
				шкаф.Rotate( Vector3.up, -Input.GetAxis("Mouse X") * speed, Space.World);
		    	Система_координат.Rotate(Vector3.up, -Input.GetAxis("Mouse X") * speed, Space.World);
			}
			else
			{
				шкаф.Rotate(Vector3.left, -Input.GetAxis("Mouse Y") * speed, Space.World);
				Система_координат.Rotate(Vector3.left, -Input.GetAxis("Mouse Y") * speed, Space.World);
			}

        }

		if (Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			/*
			distance = Vector3.Distance(transform.position, деталь);
		    distance = ZoomLimit(distance - Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, zoomMin, zoomMax);
		    position = -(transform.forward * distance) + new Vector3(transform.position.x,transform.position.y,деталь.z);
			transform.position = position;
			*/

			transform.position = transform.position + new Vector3(0,0,Input.GetAxis("Mouse ScrollWheel")*scrollSpeed);
	    }

		if(isActivatedXY)
		{
			transform.position += new Vector3(-Input.GetAxis("Mouse X")/20,-Input.GetAxis("Mouse Y")/20,0f);
		}
    }

    public static float ZoomLimit(float dist, float min, float max)
    {
        if (dist < min)
            dist = min;
        if (dist > max)
            dist = max;
        return dist;
    }




}
