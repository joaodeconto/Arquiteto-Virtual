using UnityEngine;
using System.Collections.Generic;

public class ResumeButtonHandler : MonoBehaviour {

	void OnClick ()
	{
		GameController.GetInstance ().GetInterfaceManager ().SetInterface ("Main");
		Camera.main.GetComponent<BlurEffect>().enabled = false;
		Camera.main.GetComponent<CameraController>().freeCamera.FreeCamera ();
		
		List<GameObject> furnitures = new List<GameObject>();
		furnitures.AddRange(GameObject.FindGameObjectsWithTag("Movel"));
		if (GameObject.FindGameObjectWithTag("MovelSelecionado")) furnitures.Add(GameObject.FindGameObjectWithTag("MovelSelecionado"));
		foreach (GameObject furniture in furnitures)
		{
			furniture.GetComponent<SnapBehaviour> ().enabled = true;
		}
	}
}
