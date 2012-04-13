using UnityEngine;
using System.Collections;

public class NewColorPicker : MonoBehaviour {
	
	public Color color = Color.white;
	public Transform colorCircle;
	public Transform picker;
	public UISlider slider;
	public Camera camera;
	
	// Use this for initialization
	void Start () {
	
	}
	
	void OnColorCircleUpdate () {
		//color = GUIControls.RGBCircle(camera, color, colorCircle, picker, slider);
	}
	
	// Update is called once per frame
	void Update () {
		color = GUIControls.RGBCircle(camera, color, colorCircle, picker, slider);
	}
}
