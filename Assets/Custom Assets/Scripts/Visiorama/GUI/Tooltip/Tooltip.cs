using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TooltipAlignment {
	CENTER,
	LEFT,
	RIGHT,
	MOUSE,
}

public class Tooltip : MonoBehaviour {
	
	public GUIStyle Style;

	private static GUIStyle sTooltipStyle;
	private static string lastTooltip;
	
	private static List<KeyValuePair<string,Rect>> sDinamicTips;
	private static List<KeyValuePair<string,Rect>> sStaticTips;
	
	private static void CheckNullVars(){
		if (sTooltipStyle == null) {
			sTooltipStyle = GameObject.Find("Configuration").GetComponent<Tooltip>().Style;
			GuiFont.ChangeFont(sTooltipStyle,"Trebuchet14");
			
			sDinamicTips = new List<KeyValuePair<string, Rect>>();
			sStaticTips = new List<KeyValuePair<string, Rect>>();
			lastTooltip = "";
		}
	} 
	
	public static void AddDynamicTip(string text){
		AddDynamicTip(Screen.width / 4, Screen.height / 3, text);
	}
	
	public static void AddDynamicTip(float width, float height, string text){
		
		CheckNullVars();
		
		sDinamicTips.Add (new KeyValuePair<string,Rect>(text, new Rect(0,0,width,height)));
		
	}
		
	public static void AddStaticTip(TooltipAlignment align, string text){
		switch(align){
			case TooltipAlignment.LEFT:
				AddStaticTip(new Rect(Screen.width / 8 * 1, 20, Screen.width / 4,Screen.height ), text);
				break;
			case TooltipAlignment.CENTER:
				AddStaticTip(new Rect(Screen.width / 8 * 3, 20, Screen.width / 4,Screen.height ), text);
				break;		
			case TooltipAlignment.RIGHT:
				AddStaticTip(new Rect(Screen.width / 8 * 5, 20, Screen.width / 4,Screen.height ), text);
				break;
		}
	}
	
	public static void AddStaticTip (Rect wndTooltip, string text){
		
		CheckNullVars();
		
		sStaticTips.Add (new KeyValuePair<string,Rect>(text, wndTooltip));
		
	}

	public static void DoTips (){
		if (Event.current.type == EventType.Repaint && GUI.tooltip != lastTooltip) {
			
			if (lastTooltip != "")
				return;
			
			if (GUI.tooltip == "") {
				lastTooltip = GUI.tooltip;
			} else {
				string name = GUI.tooltip;
				
				foreach (KeyValuePair<string,Rect> tip in sStaticTips) {
					if (name == tip.Key) {
						GUI.Box (tip.Value, tip.Key, sTooltipStyle);
						return;
					}
				}
				
				foreach (KeyValuePair<string,Rect> tip in sDinamicTips) {
					if (name == tip.Key) {
						Vector3 mousePos = Input.mousePosition;
						Rect tipRect = tip.Value;
						
						//Monkeys ¬¬
						
						GUI.Box ( new Rect(Mathf.Max(10f, mousePos.x /*- (tipRect.width / 4f)*/),
						                   /*Mathf.Max(10f,*/Screen.height - (mousePos.y + tipRect.height)/*)*/,
						                   tipRect.width,
						                   tipRect.height),
						         tip.Key, sTooltipStyle);
						return;
						
					}
				}
			}
		}	
	}
}