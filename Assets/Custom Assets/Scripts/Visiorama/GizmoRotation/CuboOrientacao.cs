using UnityEngine;
using System.Collections;

public class CuboOrientacao : MonoBehaviour {
		
	private Transform mainCamera; 
		
	void Start()
	{
		mainCamera = GameObject.FindWithTag("MainCamera").transform;
	}
	
	void Update ()
	{
		//transform.localEulerAngles = -camera.transform.localEulerAngles;
		transform.rotation = Quaternion.Inverse(mainCamera.transform.rotation);
	}
}