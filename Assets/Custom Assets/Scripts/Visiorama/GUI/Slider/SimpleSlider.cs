using UnityEngine;
using System.Collections.Generic;

public class SimpleSlider : MonoBehaviour, GuiBase {

	public Texture bgTexture;		//Editor
	public GUIStyle leftBtnStyle;	//Editor
	public GUIStyle rightBtnStyle;	//Editor
	public GUIStyle exitBtnStyle;	//Editor
	public float Margin;			//Editor
	public bool UseI18n;			//Editor 
	
	protected Rect wndBackground;
	
	//protected Rect wndTitle;
	//protected Rect wndText;
	
	protected List<KeyValuePair<string,string>> texts; 
	protected int cSlide;
	
	private GUIStyle titleStyle;
	private GUIStyle textStyle;
	
	private GUIStyle hScrollStyle;
	private GUIStyle vScrollStyle;
	
	protected Rect wndFrame;
	protected Rect wndContent;
	protected Vector2 scrollPosSlide;
	
	private bool WasChanged;
	
	// Use this for initialization
	void Start () {
		
		wndBackground = ScreenUtils.ScaledRect(	ScreenUtils.RealWidth / 2  - bgTexture.width / 2, 
												ScreenUtils.RealHeight / 2 - bgTexture.height / 2, 
												bgTexture.width, bgTexture.height);
		
		wndFrame = ScreenUtils.ScaledRect ( ScreenUtils.RealWidth / 2  - bgTexture.width / 2 + Margin, 
											ScreenUtils.RealHeight / 2 - bgTexture.height / 2 + Margin, 
											bgTexture.width - Margin * 2,bgTexture.height - Margin * 2);
		
		//wndTitle = ScreenUtils.ScaledRect(0,100, bgTexture.width - Margin * 4, 20);
		//wndText	 = ScreenUtils.ScaledRect(0,140, bgTexture.width - Margin * 4, 400);
		
		titleStyle = GuiFont.GetFont("Trebuchet");
		textStyle = GuiFont.GetFont("Trebuchet");
		titleStyle.alignment = TextAnchor.UpperCenter;
		titleStyle.wordWrap = true;
		textStyle.alignment = TextAnchor.UpperCenter;
		textStyle.wordWrap = true;
	
		hScrollStyle = new GUIStyle();
		vScrollStyle = new GUIStyle();
	
		scrollPosSlide = Vector2.zero;	
	}
	
	public void Add(string title, string text){
	
		if(texts == null){
			texts = new List<KeyValuePair<string, string>>();
		}
		
		texts.Add(new KeyValuePair<string,string>(title,text));
		
		WasChanged = true;
	}
	
	void OnGUI(){
		this.Draw();
	}

	#region GuiBase implementation
	public void Draw (){
	
		if(WasChanged){
			wndContent = new Rect(wndFrame);
			wndContent.x = wndContent.y = 0;
			wndContent.width *= texts.Count;
			WasChanged = false;
		}
		
		GUI.DrawTexture(wndBackground, bgTexture);
				
		
		GUI.BeginScrollView (wndFrame, scrollPosSlide, wndContent, hScrollStyle, vScrollStyle);
		for (int i = 0; i != texts.Count; i++) {
			
			if(UseI18n){
//					Debug.LogWarning ("scrollPosSlide.x: " + scrollPosSlide.x );
				GUI.Label( new Rect(wndFrame.width * i, 100, wndFrame.width, 20),I18n.t(texts[i].Value),titleStyle);
				GUI.Label( new Rect(wndFrame.width * i, 140, wndFrame.width, wndFrame.height), I18n.t(texts[i].Key),textStyle);
			} else {
				GUI.Label( new Rect(wndFrame.width * i, 100, wndFrame.width, 20),texts[i].Value,titleStyle);
				GUI.Label( new Rect(wndFrame.width * i, 140, wndFrame.width, wndFrame.height),texts[i].Key,textStyle);
			}
			
			//				if ( (( i * 90 ) + 20) < scrollPosSlide.x || 
			//		    		 (( i * 90 ) + 90 + 20) > (scrollPosSlide.x + wndGround.width)) {
			//					Debug.Log("scrollPosSlide.x + wndGround.width: " + (scrollPosSlide.x + wndGround.width));
			//					Debug.Log("i * 90: " + (i * 90));
			//					continue;
			//				}
			/*
			if (GUI.Button (ScreenUtils.ScaledRect (20, 1 + (64 * i), 64, 64), groundTextures[i])) {
				GameObject[] Grounds = GameObject.FindGameObjectsWithTag ("Chao");
				foreach (GameObject walls in Grounds) {
					Renderer[] renders = walls.GetComponentsInChildren<Renderer> ();
					foreach (Renderer r in renders) {
						r.material.mainTexture = groundTextures[i];
					}
				}
			}*/
		}
		GUI.EndScrollView ();
		
		if (scrollPosSlide.x > 0) {
			if (GUI.Button (ScreenUtils.ScaledRect (275,450,leftBtnStyle.active.background.width,
														  	leftBtnStyle.active.background.height), "", leftBtnStyle)) {
				SomClique.Play ();
				scrollPosSlide.x -= wndFrame.width;
			}
		}
		if (scrollPosSlide.x < wndContent.width - wndFrame.width) {
			if (GUI.Button (ScreenUtils.ScaledRect (715,450,rightBtnStyle.active.background.width,
														  	rightBtnStyle.active.background.height), "", rightBtnStyle)) {
				SomClique.Play ();
				scrollPosSlide.x += wndFrame.width;
			}			
		} else if(GUI.Button (ScreenUtils.ScaledRect (715,450,rightBtnStyle.active.background.width,
														  	rightBtnStyle.active.background.height), "", exitBtnStyle)){
														  	//TODO mudar isso e deixar reutilizável
//														  	GetComponent<CreateWalls>().enabled = true;
														  	enabled = false;
		}
	}

	public Rect[] GetWindows (){
		return new Rect[1]{wndBackground};	
	}
	#endregion
}