using UnityEngine;
using System.Collections;

public class ScreenshotButtonHandler : MonoBehaviour {

	void OnClick ()
	{
		GameObject.FindWithTag("GameController").GetComponentInChildren<GUICameraController>().Screenshot ();
	}
	
}
