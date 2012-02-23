using UnityEngine;
using System.Collections;

public class CuboOrientacao : MonoBehaviour {
	
	public GameObject mainCamera;
		
	// Update is called once per frame
	void Update () {
			//transform.localEulerAngles = -camera.transform.localEulerAngles;
			transform.rotation = Quaternion.Inverse(mainCamera.transform.rotation);
	}
}