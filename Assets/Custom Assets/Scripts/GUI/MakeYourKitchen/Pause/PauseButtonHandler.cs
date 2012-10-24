using UnityEngine;
using System.Collections.Generic;

public class PauseButtonHandler : MonoBehaviour {

	void OnClick ()
	{
		GameController.GetInstance ().GetInterfaceManager ().SetInterface ("Pause");
		Camera.main.GetComponent<CameraController>().freeCamera.FreezeCamera ();
		
		List<GameObject> furnitures = new List<GameObject>();
		furnitures.AddRange(GameObject.FindGameObjectsWithTag("Movel"));
		if (GameObject.FindGameObjectWithTag("MovelSelecionado")) furnitures.Add(GameObject.FindGameObjectWithTag("MovelSelecionado"));
		foreach (GameObject furniture in furnitures)
		{
			furniture.GetComponent<SnapBehaviour> ().enabled = false;
		}
	}
}
