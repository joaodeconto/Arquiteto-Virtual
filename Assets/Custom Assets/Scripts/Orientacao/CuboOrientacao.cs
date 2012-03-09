using UnityEngine;
using System.Collections;

public class CuboOrientacao : MonoBehaviour {
		
	private Transform mainCamera; 
		
	void Start()
	{
		mainCamera = GameObject.Find("Main Camera").transform;
	}
	
	void Update ()
	{
		//transform.localEulerAngles = -camera.transform.localEulerAngles;
		transform.rotation = Quaternion.Inverse(mainCamera.transform.rotation);
	}
}