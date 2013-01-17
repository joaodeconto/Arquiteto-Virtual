using UnityEngine;
using System.Collections;
using Visiorama.Utils;

public class GUIControls {

	public static Color RGBSlider (Color c, string label){
		GUI.color = c;
		GUILayout.Label (label);
		GUI.color = Color.red;
		c.r = GUILayout.HorizontalSlider (c.r,0,1);
		GUI.color = Color.green;
		c.g = GUILayout.HorizontalSlider (c.g,0,1);
		GUI.color=Color.blue;
		c.b = GUILayout.HorizontalSlider (c.b,0,1);
		GUI.color = Color.white;
		return c;
	}
	
	static Color actualColor, lastColor, lastColor2;
	public static Color RGBCircle (Vector2 position, Color c, string label, Texture2D colorCircle, GUIStyle pickerColor, GUIStyle slider, GUIStyle thumb){
		#region Forma GUI Unity
		/*Rect r = new Rect(position.x, position.y, 100, 100);
		//Rect r = GUILayoutUtility.GetAspectRect (1);
		r.height = r.width -= 15;
		Rect r2 = new Rect(r.x + r.width + 5,r.y,10,r.height);
		HSBColor hsb = new HSBColor (c);//It is much easier to work with HSB colours in this case
		
		Vector2 cp = new Vector2 (r.x+r.width/2,r.y+r.height/2);
		
		if (Input.GetMouseButton (0)) {
			Vector2 InputVector = Vector2.zero;
			InputVector.x = cp.x - Event.current.mousePosition.x;
			InputVector.y = cp.y - Event.current.mousePosition.y;
			
			float hyp = Mathf.Sqrt( (InputVector.x * InputVector.x) + (InputVector.y * InputVector.y) );
			if (hyp <= r.width/2 + 5) {
				hyp = Mathf.Clamp (hyp,0,r.width/2);
				float a = Vector3.Angle(new Vector3(-1,0,0), InputVector);
				
				if (InputVector.y<0) {
					a = 360 - a;
				}
				
				hsb.h = a / 360;
				hsb.s = hyp / (r.width/2);
			}
		}
		
		HSBColor hsb2 = new HSBColor (c);
		hsb2.b = 1;
		Color c2 = hsb2.ToColor ();
		GUI.color = c2;
		hsb.b = GUI.VerticalSlider (r2,hsb.b,1.0f,0.0f,slider,"verticalsliderthumb");
		
		GUI.color = Color.white * hsb.b;
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
		GUI.Box (r,colorCircle,GUIStyle.none);
		
		Vector2 pos = (new Vector2 (Mathf.Cos (hsb.h*360*Mathf.Deg2Rad),-Mathf.Sin (hsb.h*360*Mathf.Deg2Rad))*r.width*hsb.s/2);
		
		GUI.color = c;
		GUI.Box ( new Rect(pos.x-5+cp.x,pos.y-5+cp.y,10,10),"",pickerColor);
		GUI.color = Color.white;
		
		c = hsb.ToColor ();
		return c;*/
		#endregion
		
		#region Forma GUI Unity com ScreenUtils
		Rect r = new Rect (ScreenUtils.ScaleHeight(position.x), ScreenUtils.ScaleHeight(position.y), 
		                   ScreenUtils.ScaleHeight(100), ScreenUtils.ScaleHeight(100));
		r.height = r.width -= ScreenUtils.ScaledFloat(15);
		Rect r2 = new Rect(r.x + r.width + ScreenUtils.ScaleHeight(15),r.y,ScreenUtils.ScaleHeight(18),r.height);
		HSBColor hsb = new HSBColor (c);//It is much easier to work with HSB colours in this case
		
		Vector2 cp = new Vector2 (r.x+r.width/2,r.y+r.height/2);
		
		if (Input.GetMouseButton (0)) {
			Vector2 InputVector = Vector2.zero;
			InputVector.x = cp.x - Event.current.mousePosition.x;
			InputVector.y = cp.y - Event.current.mousePosition.y;
			
			float hyp = Mathf.Sqrt( (InputVector.x * InputVector.x) + (InputVector.y * InputVector.y) );
			if (hyp <= r.width/2 + ScreenUtils.ScaleHeight(5)) {
				hyp = Mathf.Clamp (hyp,0,r.width/2);
				float a = Vector3.Angle(new Vector3(-1,0,0), InputVector);
				
				if (InputVector.y<0) {
					a = 360 - a;
				}
				
				hsb.h = a / 360;
				hsb.s = hyp / (r.width/2);
			}
		}
		
		HSBColor hsb2 = new HSBColor (c);
		hsb2.b = 1;
		Color c2 = hsb2.ToColor ();
		if (c == Color.black) {
			c2 = lastColor2;
		} else {
			lastColor2 = c2;
		}
		GUI.color = c2;
		hsb.b = GUI.VerticalSlider (r2,hsb.b,1.0f,0.0f,slider,thumb/*"verticalsliderthumb"*/);
		
		GUI.color = Color.white * hsb.b;
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
		GUI.Box (r,colorCircle,GUIStyle.none);
		
		Vector2 pos = (new Vector2 (Mathf.Cos (hsb.h*360*Mathf.Deg2Rad),-Mathf.Sin (hsb.h*360*Mathf.Deg2Rad))*r.height*hsb.s/2);
		
		GUI.color = c;
		GUI.Box ( new Rect(pos.x-ScreenUtils.ScaleHeight(5)+cp.x,pos.y-ScreenUtils.ScaleHeight(5)+cp.y,ScreenUtils.ScaleHeight(10),ScreenUtils.ScaleHeight(10)),"",pickerColor);
		GUI.color = Color.white;
		
		c = hsb.ToColor ();
		if (c == Color.black) {
			actualColor = c;
		} else {
			if (actualColor == Color.black) {
				c = lastColor;
				actualColor = Color.white;
			}
			lastColor = c;
		}
		return c;
		#endregion
	}
	
	#region Forma GUI com NGUI
	public static Color RGBCircle (Camera camera, Color c, Transform colorCircle, Transform picker, UISlider slider){
		//Vector3 newPos = camera.WorldToScreenPoint(colorCircle.localPosition);
		Rect r = new Rect (	colorCircle.localPosition.x, colorCircle.localPosition.y, 
							colorCircle.localScale.x, colorCircle.localScale.y);
		r.height = r.width -= 15;
//		Rect r2 = new Rect(r.x + r.width + ScreenUtils.ScaleWidth(5),r.y,ScreenUtils.ScaleWidth(10),r.height);
		HSBColor hsb = new HSBColor (c);//It is much easier to work with HSB colours in this case
		
		Vector2 cp = new Vector2 (r.x+r.width/2,r.y+r.height/2);
//		Vector2 cp = Vector3.zero;
		
		if (Input.GetMouseButton (0)) {
			Vector2 InputVector = Vector2.zero;
			InputVector.x = cp.x - Input.mousePosition.x;
			InputVector.y = cp.y - (Screen.height - Input.mousePosition.y);
			
			float hyp = Mathf.Sqrt( (InputVector.x * InputVector.x) + (InputVector.y * InputVector.y) );
			if (hyp <= r.width/2 + ScreenUtils.ScaledFloat(5)) {
				hyp = Mathf.Clamp (hyp,0,r.width/2);
				float a = Vector3.Angle(new Vector3(-1,0,0), InputVector);
				
				if (InputVector.y<0) {
					a = 360 - a;
				}
				
				hsb.h = a / 360;
				hsb.s = hyp / (r.width/2);
			}
		}
		
		HSBColor hsb2 = new HSBColor (c);
		hsb2.b = 1;
		Color c2 = hsb2.ToColor ();
//		GUI.color = c2;
		hsb.b = slider.sliderValue; //GUI.VerticalSlider (r2,hsb.b,1.0f,0.0f,slider,"verticalsliderthumb");
		
//		GUI.color = Color.white * hsb.b;
//		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
//		GUI.Box (r,colorCircle,GUIStyle.none);
		
		Vector2 pos = (new Vector2 (Mathf.Cos (hsb.h*360*Mathf.Deg2Rad),Mathf.Sin (hsb.h*360*Mathf.Deg2Rad))*r.height*hsb.s/2);
		
//		GUI.color = c;
		
		picker.localPosition = new Vector3( (pos.x-5+cp.x)/r.width, (pos.y-5+cp.y)/r.height, picker.position.z);
//		GUI.color = Color.white;
		
		c = hsb.ToColor ();
		return c;
	}
	#endregion
}