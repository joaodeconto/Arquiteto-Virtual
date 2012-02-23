using UnityEngine;
using System.Collections;

public class GuiRepaintKitchen : MonoBehaviour  {
	/*
	public Texture bgTexture;
	private Rect wnd;
	
	#region Walls Menu
	public Texture[] wallsTextures;
	private Rect wndWalls;
	private Rect rListWalls;
	private Vector2 scrollPosWalls = Vector2.zero;
	private Rect btnLeftWalls;
	private Rect btnRightWalls;
	#endregion
	

	#region Styles
	public GUIStyle hScrollStyle;
	public GUIStyle vScrollStyle;
	public GUIStyle[] randomStyles;
	public GUIStyle btnLeftStyle;
	public GUIStyle btnRightStyle;
	#endregion
	
	void Awake(){
/*	
		wnd = ScreenUtils.ScaledRect(8,8,216,136);
		
		wndWalls = ScreenUtils.ScaledRect(20,20,90,90);
		btnLeftWalls  = ScreenUtils.ScaledRect(30,100,32,32);
		btnRightWalls = new Rect(ScreenUtils.ScaledRect(60,100,32,32));
		rListWalls = new Rect(ScreenUtils.ScaledRect(20,20,90 * wallsTextures.Length,90));
		
		wndGround = new Rect(ScreenUtils.ScaledRect(110,20,90,90));
		btnLeftGround  = new Rect(ScreenUtils.ScaledRect(wndGround.x + 10,100,32,32));
		btnRightGround = new Rect(ScreenUtils.ScaledRect(wndGround.x + 40,100,32,32));
		rListGround = new Rect(ScreenUtils.ScaledRect(110,20,90 + 90 * groundTextures.Length,90));
		
		lerpStep 	= 0.1F;
		lerpTime 	= 3.0F;
		cLerp 		= 0f;
	/*
	}

	#region GuiBase implementation
	public void Draw (){
		GUI.DrawTexture(wnd,bgTexture);
		GUI.DrawTexture(wndWalls,bgTexture);
		
		
		#region Ground
		GUI.BeginScrollView(wndGround, scrollPosGround, rListGround, hScrollStyle,vScrollStyle);
			for (int i = 0; i != groundTextures.Length; i++){
		
//				if ( (( i * 90 ) + 20) < scrollPosGround.x || 
//		    		 (( i * 90 ) + 90 + 20) > (scrollPosGround.x + wndGround.width)) {
//					Debug.Log("scrollPosGround.x + wndGround.width: " + (scrollPosGround.x + wndGround.width));
//					Debug.Log("i * 90: " + (i * 90));
//					continue;
//				}
		
				if(GUI.Button(ScreenUtils.ScaledRect(wndGround.x + (90 * i) ,20,90,90), groundTextures[i])){
					Debug.Log("Clicou!");
					GameObject[] grounds = GameObject.FindGameObjectsWithTag("Chao");
					foreach(GameObject groundPiece in grounds){
						Renderer[] renders = groundPiece.GetComponentsInChildren<Renderer>();
						foreach (Renderer r in renders){
							r.material.mainTexture = groundTextures[i];
						}
					}
				}
			}
		GUI.EndScrollView();
		if(groundTextures.Length > 2 && scrollPosGround.x > 0){
			if(GUI.Button(btnLeftGround,"",btnLeftStyle)){
				SomClique.Play();
				scrollToX  	= (int)scrollPosGround.x - 90;
				scrollFromX = (int)scrollPosGround.x; 
				scrollFromX = (int)scrollPosGround.x;
				ScrollGround();
			}
		}
		if(groundTextures.Length > 2 && scrollPosGround.x + wndGround.width + 90 <= ( ( wndGround.x + 90 * groundTextures.Length ) )){
			if(GUI.Button(btnRightGround,"",btnRightStyle)){
				SomClique.Play();
				scrollToX  	= (int)scrollPosGround.x + 90;
				scrollFromX = (int)scrollPosGround.x; 
				ScrollGround();
			}
		}
		#endregion
	}
	
	public Rect[] GetWindows(){
		return new Rect[1]{wnd};
	}
	#endregion*/
}
