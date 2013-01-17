using UnityEngine;
using System.Collections;

public class DefaultPopup : MonoBehaviour {
		
	public bool IsEnabled { get; set; }
	
	
	#region wnds
	public Rect wndAppendix;
	public Rect[] wndBounds;
	public Rect[] wndCorners;
	#endregion
	
	#region textures
	public Texture[] bounds;
	public Texture[] corners;
	public Texture appendixTexture;
	#endregion
	
	int i = 0;
	
	void Start () {
		
	}
	
	public void Draw(){
		/* up,down,left,right */
		for(i = 0; i != 4;++i){
			GUI.DrawTexture(wndBounds[i], bounds[i]);		
			//GUI.DrawTexture(wndCorners[i],corners[i]);
		}
	}
}
