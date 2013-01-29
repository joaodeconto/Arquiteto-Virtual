using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Visiorama.Utils;

public class ColorPickerNGUI : MonoBehaviour {
	
	public Color color;
	public Camera camera;
	public UISlider slider;
	public UITexture colorCircle;
	public UITexture picker;
//	public UIInput[] inputs; // tem de ser 3 Inputs
	
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

		this.gameObject.SetActiveRecursively(true);
	}
	
	public void CloseColorPicker () {
//		render = null;
		this.gameObject.SetActiveRecursively(false);
	}
	
	
	// Update is called once per frame
	void Update () {
//		if (render == null) {
//			CloseColorPicker();
//			return;
//		}
		
//		texture.material = color;
//		colorCircle.color = new Color(slider.sliderValue, slider.sliderValue, slider.sliderValue, 1f);
		
	    // Only when we press the mouse
//	    if (!Input.GetMouseButton (0))
//	        return;
		
		if (Input.GetMouseButton (0))
		{
			Rect bp = BoundsToScreenRect(collider.bounds);
			Vector2 realPosition = new Vector2(bp.x, bp.y);
			
			Debug.Log (uiRoot.pixelSizeAdjustment);
			
			color = TesterColor (realPosition, bp.width, color);
		}
		
	    // Only if we hit something, do we continue
//	    RaycastHit hit;
//		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
//	    if (!Physics.Raycast (ray, out hit))
//	        return;
//		
//		if (UICamera.lastHit.transform != colorCircle.transform)
//			return;

	}
	
	void OnGUI ()
	{
		Rect bp = BoundsToScreenRect(collider.bounds);
		
		GUI.color = Color.black;
		GUI.Box( new Rect(bp.x, bp.y, bp.width, bp.height), "");
		
		GUI.backgroundColor = Color.red;
		GUI.Box( new Rect(UICamera.lastTouchPosition.x, (Screen.height - UICamera.lastTouchPosition.y), 10f, 10f), "");
	}
	
	Color TesterColor (Vector2 position, float size, Color c)
	{
//		Rect r = BoundsToScreenRect (collider.bounds);
//		Rect r2 = new Rect(position.x, position.y, size, size);
		
		Rect r = new Rect(position.x, position.y, size, size);
		
//		r.height = r.width -= 15;
		HSBColor hsb = new HSBColor (c);//It is much easier to work with HSB colours in this case
		
		Vector2 cp = new Vector2 (r.x+(r.width/2),r.y+(r.height/2));
//		cp += (Vector2.one * uiRoot.pixelSizeAdjustment);
		
		Vector2 InputVector = Vector2.zero;
		InputVector.x = cp.x - UICamera.lastTouchPosition.x;
		InputVector.y = cp.y - (Screen.height - UICamera.lastTouchPosition.y);
		
//		Debug.Log (InputVector + " : " + size);
		
//		if (!CheckSphere2d (InputVector, size*uiRoot.pixelSizeAdjustment, transform.localPosition, 0f)) return lastColor;
		
//		if (Mathf.Abs(InputVector.x) > size/2 || Mathf.Abs(InputVector.y) > size/2) return lastColor;
		
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