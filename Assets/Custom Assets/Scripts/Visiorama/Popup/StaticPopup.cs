#define DEBUG_POPUPS

using UnityEngine;
using System.Collections;

public class StaticPopup : MonoBehaviour, GuiBase {

	public Texture bgPopup;
	
	public int x;
	public int y;
	
	public bool UseScaledDimensions;
	
	public Rect WndPopup { get { return wndPopup; } }
	private Rect wndPopup;
	
	// Use this for initialization
	public void Start () {
		if(UseScaledDimensions){
			wndPopup = ScreenUtils.ScaledRect(x,y,bgPopup.width,bgPopup.height);
		} else {
			wndPopup = new Rect(x,y,bgPopup.width,bgPopup.height);
		}
	}
	
#if DEBUG_POPUPS
	public void Update(){
		if(UseScaledDimensions){
			wndPopup.x = ScreenUtils.ScaleWidth(x);
			wndPopup.y = ScreenUtils.ScaleWidth(y);
		} else {
			wndPopup.x = x;
			wndPopup.y = y;		
		}
	}
#endif
	
	#region GuiBase implementation
	virtual public void Draw(){
		GUI.DrawTexture(wndPopup,bgPopup);		
	}
	virtual public Rect[] GetWindows(){
		return new Rect[1]{wndPopup};
	}
	#endregion
}