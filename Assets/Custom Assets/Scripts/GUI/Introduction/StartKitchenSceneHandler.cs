using UnityEngine;
using System.Collections;

public class StartKitchenSceneHandler : MonoBehaviour {
	
	private AsyncOperation ao;
	
	void OnGUI () {
		if (ao != null) {
			GUI.Box(new Rect(50, (Screen.height - 25) - 50, 200, 25), "Carregando: " + (int)(ao.progress * 100f) + "%");
		}
	}
	
	void OnClick ()
	{
		ao = Application.LoadLevelAsync(1);
	}
}
