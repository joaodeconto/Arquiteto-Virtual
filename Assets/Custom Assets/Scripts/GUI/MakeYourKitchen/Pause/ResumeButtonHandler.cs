using UnityEngine;
using System.Collections;

public class ResumeButtonHandler : MonoBehaviour {

	void OnClick ()
	{
		GameController.GetInstance ().GetInterfaceManager ().SetInterface ("Main");
		Time.timeScale = 1f;
	}
}
