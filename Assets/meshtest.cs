using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshtest : MonoBehaviour {
	public MeshFilter mesh;
	public Vector3[] vert;
	// Use this for initialization
	void Start () 
	{
		vert = new Vector3[24];
		for(int i = 0; i < mesh.mesh.vertexCount;i++)
//			temp2+=" _ " + mesh.mesh.vertices[i];

			vert[i] = mesh.mesh.vertices[i];

		System.Collections.Generic.List<Vector3>  temp = new System.Collections.Generic.List<Vector3>();




//		перед
		temp.Add(new Vector3(0,0,0.3f));
		temp.Add(new Vector3(-0.564f,0,0.3f));
		temp.Add(new Vector3(0,0.018f,0.3f));
		temp.Add(new Vector3(-0.564f,0.018f,0.3f));
//		перед

//		верх
		temp.Add(new Vector3(0,0.018f,0));
		temp.Add(new Vector3(-0.564f,0.018f,0));
//		верх


//		зад
		temp.Add(new Vector3(0,0,0));
		temp.Add(new Vector3(-0.564f,0,0));
//		зад

//		верх
		temp.Add(new Vector3(0,0.018f,0.3f));
		temp.Add(new Vector3(-0.564f,0.018f,0.3f));
//		верх

//		зад
		temp.Add(new Vector3(0,0.018f,0));
		temp.Add(new Vector3(-0.564f,0.018f,0));
//		зад

//		низ
		temp.Add(new Vector3(0,0,0));
		temp.Add(new Vector3(0,0,0.3f));
		temp.Add(new Vector3(-0.564f,0,0.3f));
		temp.Add(new Vector3(-0.564f,0,0));
//		низ
//
//		право
		temp.Add(new Vector3(-0.564f, 0     , 0.3f));
		temp.Add(new Vector3(-0.564f, 0.018f, 0.3f));
		temp.Add(new Vector3(-0.564f, 0.018f, 0   ));
		temp.Add(new Vector3(-0.564f, 0     , 0   ));
//      право

//		лево
		temp.Add(new Vector3(0, 0     , 0   ));
		temp.Add(new Vector3(0, 0.018f, 0   ));
		temp.Add(new Vector3(0, 0.018f, 0.3f));
		temp.Add(new Vector3(0, 0     , 0.3f));
//		лево
	

		mesh.mesh.SetVertices(temp);


//		Debug.Log(temp2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void set()
	{
		System.Collections.Generic.List<Vector3>  temp = new System.Collections.Generic.List<Vector3>();
		for(int i=0; i  <vert.Length; i ++)
		temp.Add(vert[i]);
		mesh.mesh.SetVertices(temp);
	}
}
