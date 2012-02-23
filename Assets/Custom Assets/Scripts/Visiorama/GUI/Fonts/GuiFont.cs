using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuiFont : MonoBehaviour {
	
	public FontClass[] fonts;
	
	#region Singleton
	private static FontClass[] s_fonts = null;
	public static GUIStyle GetFont (string name) {
			
		CheckFonts();
		
		foreach (FontClass fc in s_fonts) {
			
			if (fc.nameFont	== name) {
				
				GUIStyle style = new GUIStyle(GUIStyle.none);
				
				style.font = fc.font;
				style.fontSize = ScreenUtils.ScaledInt(fc.fontSize);
				style.fontStyle = fc.fontStyle;
				style.normal.textColor = fc.fontColor;
				
				return style;
			}
			
		}
		
		return null;
	}
	
	public static void ChangeFont (GUIStyle guiStyle, string name) {
			
		CheckFonts();
		
		foreach (FontClass fc in s_fonts) {
			if (fc.nameFont	== name) {
				
				guiStyle.font = fc.font;
				guiStyle.fontSize = ScreenUtils.ScaledInt(fc.fontSize);
				guiStyle.fontStyle = fc.fontStyle;
				guiStyle.normal.textColor = fc.fontColor;
			}
			
		}
	}
	
	private static void CheckFonts(){
		if(s_fonts == null){
			s_fonts = GameObject.Find("Fontes").GetComponent<GuiFont>().fonts;
		}	
	}
	#endregion End Singleton
};
