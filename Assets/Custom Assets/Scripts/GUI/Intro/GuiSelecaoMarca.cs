using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Arquiteto Virtual/Gui/Intro/Seleção Marca")]
public class GuiSelecaoMarca : MonoBehaviour {
	public Initialization 	initialization;
	public GameObject		secCan;
	public Texture2D		background;
	
	#region Buttons Kitchen
	private List<GUIStyle>	btnsKitchen;
	private float 			btnKitchenWidth;
	private float 			btnKitchenHeight;
	#endregion
	
	#region Buttons bottom's
	public GUIStyle			btnInfo;
	public GUIStyle			btnScrollLeft;
	public GUIStyle			btnScrollRight;
	public GUIStyle			btnClose;
	public GUIStyle			labelStyle;
	#endregion
	
	#region Brands names and datas
	private List<Transform>	brands;
	private List<string>	brandsName;
	#endregion
	
	#region Window(Rect) of kitchens
	private Rect			wndBackground;
	private Rect			wndLabelWelcome;
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
		initialization = transform.parent.GetComponent<Initialization>();
		
		wndButtons 	= new List<Rect>();
		brands 		= new List<Transform>();
		brandsName 	= new List<string>();
		btnsKitchen = new List<GUIStyle>();
		
		labelStyle = GuiFont.GetFont("Trebuchet");
		labelStyle.alignment = TextAnchor.UpperCenter;
		
		scrollDeleter 	   = new GUIStyle();
		scrollDeleter.name = "ScrollDeleter";
		
		wndBackground = ScreenUtils.ScaledRect((ScreenUtils.RealWidth / 2) - (background.width / 2), 
		                                  (ScreenUtils.RealHeight / 2) - (background.height / 2), 
		                                  background.width, background.height);
		
		wndLabelWelcome = ScreenUtils.ScaledRect( 0, 10f, background.width, 32f);
		
		wndButtonInitial = ScreenUtils.ScaledRect(10, 10,
		                                          128, 128);
		
		btnKitchenWidth = wndButtonInitial.width;
		btnKitchenHeight = wndButtonInitial.height;
		
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
		int i = 0;
		page = 0;
		Rect wndButtonActual = wndButtonInitial;
		foreach (GameObject b in initialization.everything) {
			wndButtons.Add(wndButtonActual);
			brands.Add(b.transform);
			brandsName.Add(b.name);
			btnsKitchen.Add(new GUIStyle());
			btnsKitchen[i].normal.background = b.GetComponent<MakeBrand>().ico;
			btnsKitchen[i].contentOffset = ScreenUtils.ScaledVector2(0f, 32f);
			if (buttonX) {
				wndButtonActual.x += (wndButtonsMargin.x + btnKitchenWidth);
				wndButtonActual.y -= (wndButtonsMargin.y + btnKitchenHeight);
				buttonX = false;
			} else {
				wndButtonActual.y += (wndButtonsMargin.y + btnKitchenHeight);
				buttonX = true;
			}
			++i;
		}
		
		numRows = i;
		print("numRows: " + numRows);
		
		Vector2 marginList = ScreenUtils.ScaledVector2(50f, 20f);
		Vector2 listSize = new Vector2(wndBackground.width - (2 * marginList.x), wndBackground.height - (2 * marginList.y));
		wndScrollFrame = new Rect(marginList.x, marginList.y, listSize.x, listSize.y);
		
		maxPage = 0;
		float lin = 0;
		if (numRows	!= 0)
		{
			float v = numRows / 6f;
			print("V: " + v);
			v = v - (int)v > 0 ? (int)v + 1 : (int)v;
			lin = wndScrollFrame.width * v;
			print("lin: " + lin);
			maxPage = (int)v - 1;
			print("maxPage: " + maxPage);
		}
		wndList = new Rect(0, 0, lin, ScreenUtils.ScaleHeight(btnKitchenWidth + 40f));
		
//		wndButtons[0] = ScreenUtils.ScaledRect(ScreenUtils.RealWidth / 2 - 144f, ScreenUtils.RealHeight / 2 - 128f, 128f, 128f);
//		wndButtons[1] = ScreenUtils.ScaledRect(ScreenUtils.RealWidth / 2, ScreenUtils.RealHeight / 2 - 128f, 128f, 128f);
	}
	
	void OnGUI () {
		
		GUI.BeginGroup(wndBackground, background);
		
		GUI.Label(wndLabelWelcome, I18n.t("Escolha uma marca"), labelStyle);
		
		wndScrollPosition = GUI.BeginScrollView (wndScrollFrame, wndScrollPosition, wndList, scrollDeleter, scrollDeleter);
		
			for (int i = 0; i != brands.Count; ++i) {
				if (GUI.Button(wndButtons[i], "", btnsKitchen[i])) {
					initialization.LoadObjects(i);
					brands[i].GetComponent<MakeBrand>().ChangeDoor();
				
					//region desativando teto, paredes e chão
					GameObject[] walls = GameObject.FindGameObjectsWithTag("ParedeParent");
					foreach(GameObject wall in walls){
						wall.GetComponent<MeshRenderer>().enabled = true;	
					}			
					GameObject[] grounds = GameObject.FindGameObjectsWithTag("ChaoParent");
					foreach(GameObject ground in grounds){
						ground.GetComponent<MeshRenderer>().enabled = true;
					}	
					GameObject[] ceilings = GameObject.FindGameObjectsWithTag("TetoParent");
					foreach(GameObject ceiling in ceilings){
						ceiling.GetComponent<MeshRenderer>().enabled = true;
					}
					
					this.gameObject.SetActiveRecursively(false); 
					secCan.SetActiveRecursively(true);
				
				}
			}
		
		GUI.EndScrollView();
		
		if (GUI.Button(wndClose, "", btnClose)) {
//			GetComponent<CreateWalls>().enabled = true;
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
