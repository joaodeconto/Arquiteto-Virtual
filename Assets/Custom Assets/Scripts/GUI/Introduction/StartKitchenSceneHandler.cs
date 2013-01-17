using UnityEngine;
using System.Collections;

public class StartKitchenSceneHandler : MonoBehaviour {
	
	private AsyncOperation ao;
	
	void OnGUI () {
		if (ao != null) {
			GUI.Box(new Rect((Screen.width / 2) - 100f, (Screen.height / 2) - 12.5f, 200f, 25f), "Carregando: " + (int)(ao.progress * 100f) + "%");
		}
	}
	
	private bool onClick;
	void OnClick ()
	{
		if (!onClick)
		{
			ao = Application.LoadLevelAsync(1);
			onClick = true;
		}
	}
}
