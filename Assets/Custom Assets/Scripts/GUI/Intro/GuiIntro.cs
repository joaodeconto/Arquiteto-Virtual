using UnityEngine;
using System.Collections;

[AddComponentMenu("Arquiteto Virtual/Gui/Intro/Gui Intro")]
public class GuiIntro : MonoBehaviour {
	
	#region Images
	public Texture2D 		background;
	public GUIStyle 		btnInfo;
	#endregion
	
	#region Windows
	private Rect			wndBackground;
	private Rect			wndInfo;
	private Rect			wndLabelWelcome;
	private Rect			wndButtonNew;
	private Rect			wndButtonModel;
	private Rect			wndButtonTutorial;
	private Rect			wndButtonCatalog;
	#endregion
	
	#region Style
	private GUIStyle		labelStyle;
	public GUIStyle			buttonsStyle;
	public GUIStyle			nonButtonsStyle;
	#endregion
	
	void Start () {
		ScreenUtils.Initialize(1024, 640);
		labelStyle = GuiFont.GetFont("Trebuchet");
		labelStyle.alignment = TextAnchor.UpperCenter;
		GuiFont.ChangeFont(buttonsStyle, "Trebuchet");
		buttonsStyle.contentOffset = ScreenUtils.ScaledVector2(-16f, -16f);
		GuiFont.ChangeFont(nonButtonsStyle, "TrebuchetGray");
		nonButtonsStyle.contentOffset = ScreenUtils.ScaledVector2(-16f, -16f);
		
		wndBackground = ScreenUtils.ScaledRect((ScreenUtils.RealWidth / 2) - (background.width / 2), 
		                                  (ScreenUtils.RealHeight / 2) - (background.height / 2), 
		                                  background.width, background.height);
		
		wndInfo = ScreenUtils.ScaledRect(	background.width - 74f, 
		                                 	background.height - 130f,
		                      				btnInfo.active.background.width,
		                      				btnInfo.active.background.height);
		
		wndLabelWelcome = ScreenUtils.ScaledRect( 0, 96f, background.width, 32f);
		
		wndButtonNew = ScreenUtils.ScaledRect( 34f, 136f, 219f, 111f);
		
		wndButtonModel = ScreenUtils.ScaledRect( 260f, 136f, 219f, 111f);
		
		wndButtonTutorial = ScreenUtils.ScaledRect( 34f, 262f, 219f, 111f);
		
		wndButtonCatalog = ScreenUtils.ScaledRect( 260f, 262f, 219f, 111f);
	}
	
	void OnGUI () {
		GUI.BeginGroup(wndBackground, background);
		
		GUI.Label(wndLabelWelcome, I18n.t("Bem-vindo à Cozinha Virtual"), labelStyle);
		
		if (GUI.Button(wndButtonNew, I18n.t("Nova Cozinha"), buttonsStyle)) {
			GetComponent<GuiNovaCozinha>().enabled = true;
			enabled = false;
		}
		
		if (GUI.Button(wndButtonModel, I18n.t("Modelo"), buttonsStyle)) {
			GetComponent<GuiSelecaoModelo>().enabled = true;
			enabled = false;
		}
		
		if (GUI.Button(wndButtonTutorial, I18n.t("Tutorial"), nonButtonsStyle));
		
		if (GUI.Button(wndButtonCatalog, I18n.t("Catalogo"), nonButtonsStyle));
		
		if (GUI.Button(wndInfo, "", btnInfo)) {
		}
		
		GUI.EndGroup();
	}
}
