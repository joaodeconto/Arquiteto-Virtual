using UnityEngine;
using System.Collections;

public class WallBuilderHelpHandler : MonoBehaviour {
	
	public GameObject PanelHelp;
	
	private bool showHelp = false;
	
	void Start () {
		PanelHelp.SetActiveRecursively(showHelp);
	}
		
	void OnClick () {
		showHelp = !showHelp;
		PanelHelp.SetActiveRecursively(showHelp);
	}
}
