using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Visiorama.Utils;

public class ColorPickerNGUI : MonoBehaviour {
	
	public Color color;
	public UISlider slider;
	public UITexture colorCircle;
	public UITexture picker;
	
	private Color lastColor, currentColor;
	private float whiteblack;
	private UITexture texture;
	private UIRoot uiRoot;
	
	public void Awake ()
	{
		uiRoot = transform.root.GetComponentInChildren<UIRoot> ();
		lastColor = currentColor;
	}
	
	//Call Color Picker when a object could be change color
	public void CallColorPicker (GameObject gameObject)
	{
		this.gameObject.SetActive (true);
	}
	
	//Call Color Picker when a object could be change color
	public void CloseColorPicker ()
	{
		this.gameObject.SetActive (false);
	}
	
	
    void OnPress (bool isDown)
    {
		UpdateColor ();
    }

    void OnDrag (Vector2 delta)
    {
		UpdateColor ();
    }

	void UpdateColor ()
	{
		Rect bp = BoundsToScreenRect(collider.bounds);
		Vector2 realPosition = new Vector2(bp.x, bp.y);
		
		color = ColorCalculation (realPosition, bp.width, color);
	}
	
	Color ColorCalculation (Vector2 position, float size, Color c)
	{
		Rect r = new Rect(position.x, position.y, size, size);
		
		HSBColor hsb = new HSBColor (c);//It is much easier to work with HSB colours in this case
		
		Vector2 cp = new Vector2 (r.x+(r.width/2),r.y+(r.height/2));
		
		Vector2 InputVector = Vector2.zero;
		InputVector.x = cp.x - UICamera.lastTouchPosition.x;
		InputVector.y = cp.y - (Screen.height - UICamera.lastTouchPosition.y);
		
		InputVector *= uiRoot.pixelSizeAdjustment;
		
		if (!CheckSphere2d (transform.localPosition, (size/2)*uiRoot.pixelSizeAdjustment, InputVector, 0f)) return lastColor;
		
		float hyp = Mathf.Sqrt( (InputVector.x * InputVector.x) + (InputVector.y * InputVector.y) );
		if (hyp <= r.width/2)
		{
			hyp = Mathf.Clamp (hyp,0,r.width/2);
			float a = Vector3.Angle(new Vector3(-1,0,0), InputVector);
			
			if (InputVector.y<0) {
				a = 360 - a;
			}
			
			hsb.h = a / 360;
			hsb.s = hyp / (r.width/2);
		}
		
		HSBColor hsb2 = new HSBColor (c);
		hsb2.b = 1;
		Color c2 = hsb2.ToColor ();
		hsb.b = slider.sliderValue;
		
//		Vector2 pos = (new Vector2 (Mathf.Cos (hsb.h*360*Mathf.Deg2Rad),-Mathf.Sin (hsb.h*360*Mathf.Deg2Rad))*r.width*hsb.s/2);
//		picker.transform.localPosition = new Vector3(pos.x-5+cp.x,pos.y-5+cp.y, -1f);
//		picker.transform.localPosition = new Vector3(pos.x-5,-(pos.y-5), -1f);
		
//		if (CheckSphere2d (transform.localPosition, ((size-(picker.mainTexture.width/2))/2)*uiRoot.pixelSizeAdjustment, InputVector, 0f))
//		{
//			picker.transform.localPosition = new Vector3(-InputVector.x, InputVector.y, picker.transform.localPosition.z);
//		}
	
		picker.transform.localPosition = new Vector3(-InputVector.x, InputVector.y, picker.transform.localPosition.z);
		picker.color = c + new Color(0,0,0,1f);
		
		c = hsb.ToColor ();
		
		if (c == Color.black) {
			currentColor = c;
		} else {
			if (currentColor == Color.black) {
				c = lastColor;
				currentColor = c;
			}
			lastColor = c;
		}
		
		return c;
	}
	
	bool CheckSphere2d (Vector2 pos1, float size1, Vector2 pos2, float size2)
	{
		float magnitude = Vector2.Distance (pos1, pos2);
			
		float distance = size1 + size2;
		
//		Debug.Log ("Distance: " + pos1 + " - " + pos2);
//		Debug.Log ("Magnitude: " + magnitude + " - Distance: " + distance);
			
		if (magnitude <= distance)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	Rect BoundsToScreenRect(Bounds bounds)
	{
	    // Get mesh origin and farthest extent (this works best with simple convex meshes)
	    Vector3 origin = UICamera.currentCamera.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
	    Vector3 extent = UICamera.currentCamera.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));
	 
	    // Create rect in screen space and return - does not account for camera perspective
	    return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);
	}
	
	void OnSliderChange ()
	{
		whiteblack = 1f - slider.sliderValue;
	    color = currentColor - new Color(whiteblack, whiteblack, whiteblack, 0f);
	}
}