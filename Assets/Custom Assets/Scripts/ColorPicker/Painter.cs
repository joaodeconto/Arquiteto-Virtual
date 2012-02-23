using UnityEngine;
using System.Collections;
using System;
using Visiorama.Utils;

public class Painter: MonoBehaviour {
	public Texture2D colorCircle;
	public Color color = Color.white;
	public GUIStyle pickerColor;
	public GUIStyle slider;
	public Texture2D dropper;
	public string[] tags;
	private Renderer render;
	private Vector2 position, sizeDropper, halfSizeDropper;
	private Rect rectWindow, rectReset, rectGetAll, rectDropper;
	private Rect[] rectRGBA, rectFieldRGBA;
	private Color lastColor;
	private bool dropperBool, dropperBoolLast;
	private string nameObject;
	private GuiCatalogo guiCatalogo;
	private GuiCamera guiCamera;
	private GuiDescription guiDescription;
	
	void Start () {
		ScreenUtils.Initialize(1024, 768);
		
		rectWindow = ScreenUtils.ScaledRect(200, 24, 120, 320);
		//não precisa usar ScreenUtils, dentro da função isso já está sendo feito
		position = new Vector2(10, 30);
		//rectReset = ScreenUtils.ScaledRect(0, 100, 100, 30);
		rectReset = ScreenUtils.ScaledRect(10, 140, 100, 20);
		rectGetAll = ScreenUtils.ScaledRect(10, 170, 100, 20);
		rectRGBA = new Rect[3];
		rectFieldRGBA = new Rect[rectRGBA.Length];
		Rect rRGBA_Standart = ScreenUtils.ScaledRect(10, 200, 60, 15);
		Rect rRGBA_field_Standart = ScreenUtils.ScaledRect(10 + 65, 200, 35, 20);
		for (int i = 0; i != rectRGBA.Length; ++i) {
			rectRGBA[i] = rRGBA_Standart;
			rectFieldRGBA[i] = rRGBA_field_Standart;
			rectRGBA[i].y += rRGBA_field_Standart.height * i + ((rRGBA_field_Standart.height - rRGBA_Standart.height)/2);
			rectFieldRGBA[i].y += rRGBA_field_Standart.height * i;
		}
		sizeDropper = ScreenUtils.ScaledVector2(dropper.width, dropper.height);
		halfSizeDropper = new Vector2(dropper.width / 2, dropper.height / 2);
		rectDropper = ScreenUtils.ScaledRect(10 + 50 - 15, 270, 30, 30);
		dropperBool = false;
		guiCatalogo = GetComponent<GuiCatalogo>();
		guiCamera = GetComponent<GuiCamera>();
		guiDescription = GetComponent<GuiDescription>();
	}
	
	void  OnGUI (){
		GUI.depth = 1;
		if (MouseUtils.MouseButtonDoubleClickDown(0, 0.3f)) {
			if (!MouseUtils.MouseClickedInArea(guiCatalogo.wndAccordMain) &&
			    !MouseUtils.MouseClickedInArea(guiCamera.wndOpenMenu) &&
			    !MouseUtils.MouseClickedInArea(guiDescription.window)) {
				if (dropperBool) {
					if (!MouseUtils.MouseClickedInArea(rectWindow)) {
						Ray ray = transform.parent.camera.ScreenPointToRay(Input.mousePosition);
						RaycastHit hit;
						if (Physics.Raycast(ray, out hit)) {
							foreach (string tag in tags) {
								if (hit.transform.tag == tag) {
									color = hit.transform.renderer.material.color;
								}
							}
						}
						dropperBool = false;
						dropperBoolLast = true;
					}
				}
				else {
					Ray ray = transform.parent.camera.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					if (!dropperBoolLast) {
						bool breaker = false;
						if (render != null) {
							if (MouseUtils.MouseClickedInArea(rectWindow)) breaker = true;
						}
						
						if (!breaker) {
							if (Physics.Raycast(ray, out hit)) {
								foreach (string tag in tags) {
									if (hit.transform.tag == tag) {
										render = hit.transform.renderer;
										color = hit.transform.renderer.material.color;
										if (hit.transform.name == "ParentTeto") {
											nameObject = "Teto";
										}
										else if (hit.transform.name == "ParedesBack") {
											nameObject = "Parade Atrás";
										}
										else if (hit.transform.name == "ParedesFront") {
											nameObject = "Parade Frente";
										}
										else if (hit.transform.name == "ParedesLeft") {
											nameObject = "Parade Esquerdo";
										}
										else if (hit.transform.name == "ParedesRight") {
											nameObject = "Parade Direito";
										}
										return;
									}
								}
								render = null;
							}
							if (!Physics.Raycast(ray, out hit)) {
								render = null;
							}
						}
					}
					else dropperBoolLast = false;
				}
			}
		}
		
		if (dropperBool) {
			Vector2 mp = Event.current.mousePosition;
			GUI.DrawTexture(new Rect(mp.x, mp.y - halfSizeDropper.y, sizeDropper.x, sizeDropper.y), dropper);
			if (Screen.showCursor) Screen.showCursor = false;
		}
		else { if (!Screen.showCursor) Screen.showCursor = true; }
		
//		position.x = Input.mousePosition.x;
//		position.y = Screen.height - Input.mousePosition.y;
		if (render != null) {
			GUI.BeginGroup(rectWindow, nameObject, "box");
			color = GUIControls.RGBCircle (position, color,"",colorCircle, pickerColor, slider);
			if (GUI.Button(rectReset, "Reset in white")) { color = Color.white; }
			if (GUI.Button(rectGetAll, "Paint all this color")) {
				GameObject[] walls = GameObject.FindGameObjectsWithTag("ParedeParent");
				foreach (GameObject wall in walls) {
					wall.renderer.material.color = color;
				}
				GameObject.FindWithTag("TetoParent").renderer.material.color = color;
			}
//			string rs = GUI.TextField(rectRGBA[0], Convert.ToString((color.r * 255)));
//			if (float.TryParse(rs, out color.r)) {
//		        color.r = Mathf.Clamp(color.r / 255f, 0f, 1f);
//		    }
//		    else if (rs == "") color.r = 0f;
//			string gs = GUI.TextField(rectRGBA[1], Convert.ToString((color.g * 255)));
//			if (float.TryParse(gs, out color.g)) {
//		        color.g = Mathf.Clamp(color.g / 255f, 0f, 1f);
//		    }
//		    else if (gs == "") color.g = 0f;
//			string bs = GUI.TextField(rectRGBA[2], Convert.ToString((color.b * 255)));
//			if (float.TryParse(bs, out color.b)) {
//		        color.b = Mathf.Clamp(color.b / 255f, 0f, 1f);
//		    }
//		    else if (bs == "") color.b = 0f;
			
			color.r = GUI.HorizontalSlider(rectRGBA[0], color.r, 0f, 1f);
			GUI.TextField(rectFieldRGBA[0], Convert.ToString((int)Mathf.Ceil(color.r * 255)));
			color.g = GUI.HorizontalSlider(rectRGBA[1], color.g, 0f, 1f);
			GUI.TextField(rectFieldRGBA[1], Convert.ToString((int)Mathf.Ceil(color.g * 255)));
			color.b = GUI.HorizontalSlider(rectRGBA[2], color.b, 0f, 1f);
			GUI.TextField(rectFieldRGBA[2], Convert.ToString((int)Mathf.Ceil(color.b * 255)));
			
			dropperBool = GUI.Toggle(rectDropper, dropperBool, "Dropper", "button");
			GUI.EndGroup();
			render.material.color = color;
		} else {
			if (dropperBool) {
				dropperBool = false;
			}
		}
		GUI.depth = 0;
	}
}