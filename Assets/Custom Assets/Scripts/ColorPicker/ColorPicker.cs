using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Visiorama.Utils;

public class ColorPicker : MonoBehaviour {
	
	public Color color;
	public Camera camera;
	public UISlider slider;
	public GameObject colorCircle;
	public UIInput[] inputs; // tem de ser 3 Inputs
	private Color lastColor, realColor;
	private float whiteblack;
	private Renderer render;
	
	//Call Color Picker when a object could be change color
	public void CallColorPicker (GameObject gameObject) {
		render = gameObject.renderer;
		color = gameObject.renderer.material.color;
		if (color.r >= color.g) {
			if (color.r >= color.b) {
				slider.sliderValue = color.r;
			}
		}
		else if (color.g >= color.b) {
			slider.sliderValue = color.g;
		} else slider.sliderValue = color.b;
		
		whiteblack = 1f - slider.sliderValue;
		
		realColor = color;
		realColor.r += whiteblack;
		realColor.g += whiteblack;
		realColor.b += whiteblack;
		
		inputs[0].label.text = ""+color.r*255f;
		inputs[1].label.text = ""+color.g*255f;
		inputs[2].label.text = ""+color.b*255f;
		
//		List<Ray> rays = new List<Ray>();
//		for (int i = 0; i != 100; ++i) {
//			for (int j = 0; j != 100; ++j) {
//				rays.Add(new Ray(new Vector3(transform.position.x + ((j * 0.003f) - (50f * 0.003f)),
//										  transform.position.y + ((i * 0.003f) - (50f * 0.003f)),
//										  transform.position.z - 0.5f), Vector3.forward));
//			}
//		}
//		
//		foreach (Ray ray in rays) {
//			RaycastHit hit;
//			
//			if (Physics.Raycast (ray, out hit))
//	        	continue;
//			
//		    Renderer renderer = hit.collider.renderer;
//		
//			Texture2D tex = (Texture2D)renderer.material.mainTexture;
//		    Vector2 pixelUV = hit.textureCoord;
//			Color thisColor = tex.GetPixelBilinear(pixelUV.x, pixelUV.y);
//			
//			if ((thisColor.r >= color.r - 0.01f) && (thisColor.r <= color.r + 0.01f) &&
//				(thisColor.g >= color.g - 0.01f) && (thisColor.g <= color.g + 0.01f) &&
//				(thisColor.b >= color.b - 0.01f) && (thisColor.b <= color.b + 0.01f) &&
//				thisColor.a < 1f) {
//				picker.transform.position = new Vector3(ray.origin.x, ray.origin.y, transform.position.z);
//			}
//			
//		}
		
//		print((50 * realColor.r) + " " + (-25 * realColor.b) + " " + (-25 * realColor.g) + " = " + ((50 * realColor.r) + (-25 * realColor.b) + (-25 * realColor.g)));
//		print((0 * color.r) + " " + (-42.5f * color.b) + " " + (42.5f * color.g) + " = " + ((0 * color.r) + (-42.5f * color.b) + (42.5f * color.g)));
//		float pickerX = (50 * realColor.r) + (-25 * realColor.b) + (-25 * realColor.g);
//		float pickerX = (0 * realColor.r) + (-42.5f * realColor.b) + (42.5f * realColor.g);
//		picker.transform.position = new Vector3((50 * color.r) + (-25 * color.b) + (-25 * color.g),
//			(0 * realColor.r) + (-42.5f * realColor.b) + (42.5f * realColor.g), picker.transform.position.z);
		
//		print(gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x * transform.localScale.x);
//		print(gameObject.GetComponent<MeshFilter>().mesh.bounds.size.y * transform.localScale.y);
//		print(gameObject.GetComponent<MeshFilter>().mesh.bounds.size.z * transform.localScale.z);
		this.gameObject.SetActiveRecursively(true);
	}
	
	public void CloseColorPicker () {
		render = null;
		this.gameObject.SetActiveRecursively(false);
	}
	
	
	// Update is called once per frame
	void Update () {
		if (render == null) {
			CloseColorPicker();
			return;
		}
		
		render.material.color = color;
		colorCircle.renderer.material.color = new Color(slider.sliderValue, slider.sliderValue, slider.sliderValue, 1f);
		
	    // Only when we press the mouse
	    if (!Input.GetMouseButton (0))
	        return;
	
	    // Only if we hit something, do we continue
	    RaycastHit hit;
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
	    if (!Physics.Raycast (ray, out hit))
	        return;
		
		if (hit.transform != colorCircle.transform)
			return;
		
		if (hit.transform.name != "Slider" &&
			hit.transform.name != "ColorCircle" &&
			hit.transform.name != "Thumb")
			return;
		
	    // Just in case, also make sure the collider also has a renderer
	    // material and texture. Also we should ignore primitive colliders.
	    Renderer renderer = hit.collider.renderer;
	    MeshCollider meshCollider = hit.collider as MeshCollider;
	    if (renderer == null || renderer.sharedMaterial == null ||
	        renderer.sharedMaterial.mainTexture == null || meshCollider == null)
	        return;
		
	    // Now draw a pixel where we hit the object
	    Texture2D tex = (Texture2D)renderer.material.mainTexture;
	    Vector2 pixelUV = hit.textureCoord;
		
		whiteblack = 1f - slider.sliderValue;
		realColor = tex.GetPixelBilinear(pixelUV.x, pixelUV.y);
	    color = realColor - new Color(whiteblack, whiteblack, whiteblack, 0f);
		if (color.a < 1f){
			color = lastColor;
		}
		lastColor = color;
	}
	
	void OnSliderChange () {
		whiteblack = 1f - slider.sliderValue;
	    color = realColor - new Color(whiteblack, whiteblack, whiteblack, 0f);
	}
}
