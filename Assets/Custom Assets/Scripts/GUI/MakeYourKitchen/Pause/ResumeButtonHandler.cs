using UnityEngine;
using System.Collections;

public class ResumeButtonHandler : MonoBehaviour {

	void OnClick ()
	{
		GameController.GetInstance ().GetInterfaceManager ().SetInterface ("Main");
		Camera.main.GetComponent<CameraController>().freeCamera.FreeCamera ();
	}
}
