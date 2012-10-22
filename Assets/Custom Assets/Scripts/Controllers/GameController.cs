using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Game/Controllers/Game Controller")]
public class GameController : MonoBehaviour
{
	private InterfaceManager im;

	void Start ()
	{
		Application.runInBackground = true;
		
		GetInterfaceManager().Init();
	}

	public InterfaceManager GetInterfaceManager()
	{
		if (im == null)
		{
			GameObject interfaceManager = GameObject.Find ("InterfaceManager");
			if (interfaceManager == null)
			{
				interfaceManager = new GameObject ("InterfaceManager");
				interfaceManager.AddComponent <InterfaceManager> ();
			}

			GameController.AppendController (interfaceManager);

			im = interfaceManager.GetComponent <InterfaceManager> ();
		}

		return im;
	}

	/* Estático */
	private static GameController instance;
	public static GameController GetInstance ()
	{
		if (instance == null)
		{
			GameObject gameController = GameObject.Find ("GameController");
			if (gameController == null)
			{
				gameController = new GameObject ("GameController");
				gameController.AddComponent <GameController> ();
			}
			instance = gameController.GetComponent <GameController> ();
		}
		return instance;
	}

	private static void AppendController (GameObject controller)
	{
		controller.transform.parent = GetInstance ().transform;
	}
}
