using UnityEngine;
using System.Collections;

[AddComponentMenu("Arquiteto Virtual/Gui/Intro/Gui Seleção Linguagem")]
public class GuiSelecaoLinguagem : MonoBehaviour {
	
	#region Images
	public Texture2D 		background;
	public GUIStyle 		btnInfo;
	#endregion
	
	#region Windows
	private Rect			wndBackground;
	private Rect			wndInfo;
	private Rect			wndButtonBra;
	private Rect			wndButtonEua;
	private Rect			wndButtonEsp;
	#endregion
	
	#region Bandeiras Buttons
	public GUIStyle			braFlagButton;
	public GUIStyle			euaFlagButton;
	public GUIStyle			espFlagButton;
	#endregion
	
	void Start () {
		ScreenUtils.Initialize(1024, 640);
		
		wndBackground = ScreenUtils.ScaledRect((ScreenUtils.RealWidth / 2) - (background.width / 2), 
		                                  (ScreenUtils.RealHeight / 2) - (background.height / 2), 
		                                  background.width, background.height);
		
		wndInfo = ScreenUtils.ScaledRect(	background.width - 74f, 
		                                 	background.height - 130f,
		                      				btnInfo.active.background.width,
		                      				btnInfo.active.background.height);
		
		
		wndButtonBra = ScreenUtils.ScaledRect( 	40f, 200f,
		                                      	braFlagButton.normal.background.width / 2, 
		                                      	braFlagButton.normal.background.height / 2);
		
		wndButtonEua = ScreenUtils.ScaledRect( 	190f, 200f, 
		                                      	euaFlagButton.normal.background.width / 2, 
		                                      	euaFlagButton.normal.background.height / 2);
		
		wndButtonEsp = ScreenUtils.ScaledRect( 	340f, 200f, 
		                                      	espFlagButton.normal.background.width / 2, 
		                                      	espFlagButton.normal.background.height / 2);
	}
	
	void OnGUI () {
		GUI.BeginGroup(wndBackground, background);
		
		if (GUI.Button(wndButtonBra, "", braFlagButton)) {
			I18n.ChangeLanguage(0);
			GetComponent<GuiIntro>().enabled = true;
			enabled = false;
		}
		if (GUI.Button(wndButtonEua, "", euaFlagButton)) {
			I18n.ChangeLanguage(1);
			GetComponent<GuiIntro>().enabled = true;
			enabled = false;
		}
		if (GUI.Button(wndButtonEsp, "", espFlagButton)) {
			I18n.ChangeLanguage(2);
			GetComponent<GuiIntro>().enabled = true;
			enabled = false;
		}
		
//		if (GUI.Button(wndInfo, "", btnInfo)) {
//		}
		
		GUI.EndGroup();
	}
}
