using UnityEngine;
using System.Collections;

public class PauseButtonHandler : MonoBehaviour {

	void OnClick ()
	{
		GameController.GetInstance ().GetInterfaceManager ().SetInterface ("Pause");
		Time.timeScale = 0f;
	}
}
