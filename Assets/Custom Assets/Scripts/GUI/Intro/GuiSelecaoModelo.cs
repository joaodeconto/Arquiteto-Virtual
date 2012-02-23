using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Arquiteto Virtual/Gui/Intro/Seleção Modelo")]
public class GuiSelecaoModelo : MonoBehaviour {
	public Texture2D		background;
	public Texture2D		button;
	
	#region Buttons Models
	private List<GUIStyle>	btnsModels;
	private float 			btnModelsWidth;
	private float 			btnModelsHeight;
	#endregion
	
	#region Buttons bottom's
	public GUIStyle			btnInfo;
	public GUIStyle			btnScrollLeft;
	public GUIStyle			btnScrollRight;
	public GUIStyle			btnClose;
	#endregion
	
	#region Models names and datas
	private List<string>	models;
	#endregion
	
	#region Window(Rect) of Models
	private Rect			wndBackground;
	private Rect			wndButtonInitial;
	private List<Rect>		wndButtons;
	private Rect			wndScrollFrame;
	private Rect 			wndList;
	private Vector2			wndScrollPosition;
	private Vector2			wndButtonsMargin;
	#endregion
	
	#region Window(Rect) of others
	private Rect			wndInfo;
	private Rect			wndScrollLeft;
	private Rect			wndScrollRight;
	private Rect			wndClose;
	#endregion
	
	#region Control of pages
	private int				numRows;
	private int				page;
	private int				maxPage;
	#endregion
	
	#region Scrollbars Deleters
	private GUIStyle		scrollDeleter;
	#endregion
	
	void Start () {
		wndButtons = new List<Rect>();
		models = new List<string>();
		btnsModels = new List<GUIStyle>();
		
		scrollDeleter = new GUIStyle();
		scrollDeleter.name = "ScrollDeleter";
		
		wndBackground = ScreenUtils.ScaledRect(	(ScreenUtils.RealWidth / 2) - (background.width / 2), 
		                                  		(ScreenUtils.RealHeight / 2) - (background.height / 2), 
		                                  		background.width, background.height);
		
		wndButtonInitial = ScreenUtils.ScaledRect(10f, 10f,
		                                          128f, 128f);
		
		btnModelsWidth = wndButtonInitial.width;
		btnModelsHeight = wndButtonInitial.height;
		
		wndButtonsMargin = ScreenUtils.ScaledVector2(9.332f, 10f);
		
		wndInfo = ScreenUtils.ScaledRect(	background.width - 48f, 
		                                 	background.height - 48f,
		                      				btnInfo.active.background.width,
		                      				btnInfo.active.background.height);
		
		wndScrollLeft = ScreenUtils.ScaledRect(	(background.width / 2) - 39f, 
		                                		background.height - 48f,
		                      					btnScrollLeft.active.background.width,
		                      					btnScrollLeft.active.background.height);
		
		wndScrollRight = ScreenUtils.ScaledRect((background.width / 2) + 6f, 
				                                background.height - 48f,
				                      			btnScrollRight.active.background.width,
				                      			btnScrollRight.active.background.height);
		
		wndClose = ScreenUtils.ScaledRect(	16f, 
				                          	background.height - 48f,
				                      		btnClose.active.background.width,
				                      		btnClose.active.background.height);
		
		bool buttonX = false;
		Rect wndButtonActual = wndButtonInitial;
		page = 0;
		for (int i = 0; i != 160; ++i) {
			wndButtons.Add(wndButtonActual);
			models.Add("M-"+i);
			btnsModels.Add(new GUIStyle());
			btnsModels[i].normal.background = button;
			btnsModels[i].fontSize = ScreenUtils.ScaledInt(12);
			btnsModels[i].contentOffset = ScreenUtils.ScaledVector2(0f, 32f);
			if (buttonX) {
				wndButtonActual.x += (wndButtonsMargin.x + btnModelsWidth);
				wndButtonActual.y -= (wndButtonsMargin.y + btnModelsHeight);
				buttonX = false;
			} else {
				wndButtonActual.y += (wndButtonsMargin.y + btnModelsHeight);
				buttonX = true;
			}
			numRows = i;
		}
		
		print("numRows: " + numRows);
		
		Vector2 marginList = ScreenUtils.ScaledVector2(50f, 10f);
		Vector2 listSize = new Vector2(wndBackground.width - (2 * marginList.x), wndBackground.height - (2 * marginList.y));
		wndScrollFrame = new Rect(marginList.x, marginList.y, listSize.x, listSize.y);
		
		maxPage = 0;
		float lin = 0;
		if (numRows	!= 0)
		{
			float v = numRows / 6f;
			v = v - (int)v > 0 ? (int)v + 1 : (int)v;
			lin = wndScrollFrame.width * v;
			maxPage = (int)v - 1;
		}
		wndList = new Rect(0, 0, lin, ScreenUtils.ScaleHeight(btnModelsHeight + 40f));
	}
	
	void OnGUI () {
		GUI.BeginGroup(wndBackground, background);
		
		wndScrollPosition = GUI.BeginScrollView (wndScrollFrame, wndScrollPosition, wndList, scrollDeleter, scrollDeleter);
		
			for (int i = 0; i != models.Count; ++i) {
				if (GUI.Button(wndButtons[i], models[i], btnsModels[i])) {
				}
			}
		
		GUI.EndScrollView();
		
		if (GUI.Button(wndClose, "", btnClose)) {
			GetComponent<GuiIntro>().enabled = true;
			enabled = false;
		}
		
		if (page != 0) {
			if (GUI.Button(wndScrollLeft, "", btnScrollLeft)) {
				page--;
				wndScrollPosition.x -= wndScrollFrame.width;
			}
		}
		if (page != maxPage) {
			if (GUI.Button(wndScrollRight, "", btnScrollRight)) {
				page++;
				wndScrollPosition.x += wndScrollFrame.width;
			}
		}
		
		if (GUI.Button(wndInfo, "", btnInfo)) {
		}
		
		GUI.EndGroup();
	}
}
