//#define DRAW_LINE_DEBUG

using UnityEngine;
using System.Collections.Generic;

public class CalculateBounds : MonoBehaviour {
	
	bool colliderBased = true;
	
	private Vector3 topFrontLeft;
	private Vector3 topFrontRight;
	private Vector3 topBackLeft;
	private Vector3 topBackRight;
	private Vector3 bottomFrontLeft;
	private Vector3 bottomFrontRight;
	private Vector3 bottomBackLeft;
	private Vector3 bottomBackRight;
	
	float alpha;
	public float Alpha {
		get { return alpha; }
	}
	
	private List<Vector3> corners;
		
	private Bounds bound;
	
	private Quaternion quat;
	
	void Start (){
		
		corners = new List<Vector3>();
		alpha = transform.rotation.eulerAngles.y;
	
	}
	
	void UpdateBounds(){
		if(colliderBased){
			bound  = GetComponent<Collider>().bounds;
		} else{
			bound = new Bounds(transform.position, Vector3.zero);
			MeshFilter[] meshes;
			meshes = GetComponentsInChildren<MeshFilter>();
			Debug.Log("renderers" + meshes.Length);
			foreach(MeshFilter ms in meshes)	{
				bound.Encapsulate(ms.renderer.bounds);
			}
		}
	}
	
	void  UpdateCorners (){
		
		UpdateBounds();		
		
		Vector3 scale;
				
		Vector3 bc = bound.center;
//		bc.x -= bound.size.x;
//		bc.z -= bound.size.z;
		
		corners = new List<Vector3>();
		topFrontRight = bc - quat * Vector3.Scale(bound.extents, new Vector3(1, 1, 1)); 
		corners.Add(topFrontRight);
		topFrontLeft = bc - quat * Vector3.Scale(bound.extents, new Vector3(-1, 1, 1)); 
		corners.Add(topFrontLeft);
		topBackRight = bc - quat * Vector3.Scale(bound.extents, new Vector3(1, 1, -1)); 
//		corners.Add(topBackRight);
		topBackLeft = bc - quat * Vector3.Scale(bound.extents, new Vector3(-1, 1, -1)); 
		corners.Add(topBackLeft);
		corners.Add(topBackRight);
		bottomFrontRight = bc - quat * Vector3.Scale(bound.extents, new Vector3(1, -1, 1)); 
		corners.Add(bottomFrontRight);
		bottomFrontLeft = bc - quat * Vector3.Scale(bound.extents, new Vector3(-1, -1, 1)); 
		corners.Add(bottomFrontLeft);
		bottomBackRight = bc - quat * Vector3.Scale(bound.extents, new Vector3(1, -1, -1)); 
		//corners.Add(bottomBackRight);
		bottomBackLeft = bc - quat * Vector3.Scale(bound.extents, new Vector3(-1, -1, -1)); 
		corners.Add(bottomBackLeft);
		corners.Add(bottomBackRight);
		
	}
	
	void Update (){
		
		UpdateCorners();
		
#if DRAW_LINE_DEBUG
		Debug.DrawLine(topFrontLeft, topFrontRight, Color.green);
		Debug.DrawLine(bottomFrontLeft, bottomFrontRight, Color.green);
		Debug.DrawLine(topBackLeft, topBackRight, Color.green);
		Debug.DrawLine(bottomBackLeft, bottomBackRight, Color.green);
		Debug.DrawLine(topFrontLeft, topBackLeft, Color.green);
		Debug.DrawLine(topFrontRight, topBackRight, Color.green);
		Debug.DrawLine(bottomFrontLeft, bottomBackLeft, Color.green);
		Debug.DrawLine(bottomFrontRight, bottomBackRight, Color.green);
		Debug.DrawLine(topFrontLeft, bottomFrontLeft, Color.green);
		Debug.DrawLine(topFrontRight, bottomFrontRight, Color.green);
		Debug.DrawLine(topBackLeft, bottomBackLeft, Color.green);
		Debug.DrawLine(topBackRight, bottomBackRight, Color.green);
#endif
	}
	
	public List<Vector3> Corners {
		get { 	
				UpdateCorners();
			
				return corners; 
			}
	}
}