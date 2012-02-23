using UnityEngine;
using System.Collections;

[AddComponentMenu("Arquiteto Virtual/Gui/Intro/Nova Cozinha")]
public class GuiNovaCozinha : MonoBehaviour {

	#region Images
	public Texture2D 		background;
	public GUIStyle 		btnInfo;
	#endregion
	
	#region Windows
	private Rect			wndBackground;
	private Rect			wndInfo;
	private Rect			wndLabelTop;
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
		labelStyle = GuiFont.GetFont("Trebuchet");
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
		
		wndLabelTop 	= ScreenUtils.ScaledRect(	(background.width / 2) - 70f, 96f, 140f, 32f);
		wndButtonNew 	= ScreenUtils.ScaledRect( 34f, 136f, 219f, 111f);
		wndButtonModel 	= ScreenUtils.ScaledRect( 260f, 136f, 219f, 111f);
		wndButtonTutorial = ScreenUtils.ScaledRect( 34f, 262f, 219f, 111f);
		wndButtonCatalog  = ScreenUtils.ScaledRect( 260f, 262f, 219f, 111f);
			
		GetComponent<SimpleSlider>().Add("Construtor-texto-1","Construtor");
		GetComponent<SimpleSlider>().Add("Construtor-texto-2","Construtor");
		GetComponent<SimpleSlider>().Add("Construtor-texto-3","Construtor");
	}
	
	void OnGUI () {
	
		GUI.BeginGroup(wndBackground, background);
		
		GUI.Label(wndLabelTop, I18n.t("Nova Cozinha"), labelStyle);
		
		if (GUI.Button(wndButtonNew, I18n.t("Construir cozinha"), buttonsStyle)) {
			GetComponent<SimpleSlider> ().enabled = true;
			enabled = false;
		}
		
		if (GUI.Button(wndButtonModel, I18n.t("Cozinha padrão"), nonButtonsStyle)){
			Debug.Log ("Entrou cozinha padrão");
		}
		
		if (GUI.Button(wndButtonTutorial, I18n.t("Tutorial"), nonButtonsStyle)){
			Debug.Log ("Entrou tutorial");
		}
		
		if (GUI.Button(wndButtonCatalog, I18n.t("Catalogo"), nonButtonsStyle)){
			Debug.Log ("Entrou catalogo");
		}
		
		if (GUI.Button(wndInfo, "", btnInfo)) {
		}
		
		GUI.EndGroup();
	}}
