using UnityEngine;
using System.Collections;

public class PanelMobileButtonHandler : MonoBehaviour {
	
	public enum PanelMobileButtonEnum {
		Focus,
		Rotate,
		Delete,
	}
	
	public PanelMobileButtonEnum panelMobileButton;
	
	private InfoController infoController;
	
	void Start(){
		infoController = GameObject.FindWithTag("GameController").GetComponentInChildren<InfoController>();
	}
	
	void OnClick(){
		switch(panelMobileButton){
			case PanelMobileButtonEnum.Focus:
				infoController.FocusObject();
				break;
			case PanelMobileButtonEnum.Rotate:
				infoController.RotateObject ();
				break;
			case PanelMobileButtonEnum.Delete:
				infoController.DeleteObject ();
				break;
		}
	}
}
