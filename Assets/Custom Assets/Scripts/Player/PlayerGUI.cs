using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {
	
	public Texture BackToTheOtherCamTexture;
	public GameObject otherCam;
	private Rect wndBackToTheOtherCam;
	
	// Use this for initialization
	void Awake () {
		wndBackToTheOtherCam = ScreenUtils.ScaledRect(GuiScript.BORDER_MARGIN,
		                                              GuiScript.BORDER_MARGIN,
		                                              256,
		                                              128);
	}
	
	void Update(){
		if(Input.GetKeyUp(KeyCode.Escape)){
			gameObject.SetActiveRecursively(false); 
			otherCam.SetActiveRecursively(true);
			SnapBehaviour.ActivateAll();			
			foreach (Transform child in GameObject.Find("RotacaoCubo").transform) {
				child.gameObject.SetActiveRecursively(true);
			}
		}
	}
	
	void OnGUI(){
		GUI.DrawTexture(wndBackToTheOtherCam,BackToTheOtherCamTexture);
	}
}
