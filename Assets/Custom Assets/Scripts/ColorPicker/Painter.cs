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
	public string[] tags, objectsNames;
	public bool dropperBool {get; private set;}
	internal Rect rectWindow;
	private GameObject GO;
	private Renderer render;
	private Vector2 position, sizeDropper, halfSizeDropper;
	private Rect rectReset, rectGetAll, rectDropper;
	private Rect[] rectRGBA, rectFieldRGBA;
	private Color lastColor;
	private bool dropperBoolLast, clicked;
	private string nameObject, tagObject;
	private GuiCatalogo guiCatalogo;
	private GuiCamera guiCamera;
	private GuiDescription guiDescription;
	private GUIStyle groupStyle, buttonStyle, labelStyle;
	
	void Start () {
		//ScreenUtils.Initialize(1024, 768);
		
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
		halfSizeDropper = new Vector2(dropper.width / 2, dropper.height);
		rectDropper = ScreenUtils.ScaledRect(10 + 50 - 15, 270, 30, 30);
		dropperBool = false;
		guiCatalogo = GetComponent<GuiCatalogo>();
		guiCamera = GetComponent<GuiCamera>();
		guiDescription = GetComponent<GuiDescription>();
		tagObject = "";
		
		groupStyle = new GUIStyle("box");
		groupStyle.fontSize = ScreenUtils.ScaledInt(10);
		
		buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = ScreenUtils.ScaledInt(10);
	}
	
	void  OnGUI (){
		GUI.depth = 1;
		if (!MouseUtils.MouseClickedInArea(guiCatalogo.wndAccordMain) &&
		    !MouseUtils.MouseClickedInArea(guiCamera.wndOpenMenu) &&
		    !MouseUtils.MouseClickedInArea(guiDescription.window)) {
			if (dropperBool) {
				if (Input.GetMouseButtonUp(0)) {
					if (!MouseUtils.MouseClickedInArea(rectWindow)) {
						Ray ray = transform.parent.camera.ScreenPointToRay(Input.mousePosition);
						RaycastHit hit;
						if (Physics.Raycast(ray, out hit)) {
							foreach (string tag in tags) {
								if (hit.transform.tag == tag) {
									color = hit.transform.renderer.material.color;
								}
							}
							foreach (string name in objectsNames) {
								string objName = name + "(Clone)";
								if (hit.transform.name == objName) {
									color = hit.transform.GetComponentInChildren<Renderer>().materials[0].color;
								}
							}
							if (tagObject == "MovelSelecionado") {
								GO.tag = tagObject;
							}
							if (hit.transform.tag == "MovelSelecionado") {
								hit.transform.tag = "Movel";
							}
						}
						dropperBool = false;
						dropperBoolLast = true;
					}
				}
			}
			else {

				if (Input.GetMouseButtonDown(0) &&
					!clicked) {
					if (!dropperBoolLast) {
						bool breaker = false;
						if (render != null) {
							if (MouseUtils.MouseClickedInArea(rectWindow)) breaker = true;
						}
						
						if (!breaker) {
							Ray ray = transform.parent.camera.ScreenPointToRay(Input.mousePosition);
							RaycastHit hit;
							if (Physics.Raycast(ray, out hit)) {
								render = null;
							}
							if (!Physics.Raycast(ray, out hit)) {
								render = null;
							}
						}
					} else dropperBoolLast = false;
				}
				if (MouseUtils.GUIMouseButtonDoubleClickDown(0, 0.3f)) {
					if (!dropperBoolLast) {
						bool breaker = false;
						if (render != null) {
							if (MouseUtils.MouseClickedInArea(rectWindow)) breaker = true;
						}
						
						if (!breaker) {
							Ray ray = transform.parent.camera.ScreenPointToRay(Input.mousePosition);
							RaycastHit hit;
							rectWindow.x = Input.mousePosition.x;
							if (rectWindow.x > Screen.width - rectWindow.width)
								rectWindow.x -= rectWindow.width;
							rectWindow.y = Screen.height - Input.mousePosition.y;
							if (rectWindow.y > Screen.height - rectWindow.height)
								rectWindow.y -= rectWindow.height;
							if (Physics.Raycast(ray, out hit)) {
								foreach (string tag in tags) {
									if (hit.transform.tag == tag) {
										render = hit.transform.renderer;
										color = hit.transform.renderer.material.color;
										if (hit.transform.name == "ParentTeto") {
											nameObject = I18n.t("Teto");
										}
										else if (hit.transform.name == "ParedesBack") {
											nameObject = I18n.t("Parede Atrás");
										}
										else if (hit.transform.name == "ParedesFront") {
											nameObject = I18n.t("Parede Frente");
										}
										else if (hit.transform.name == "ParedesLeft") {
											nameObject = I18n.t("Parede Esquerdo");
										}
										else if (hit.transform.name == "ParedesRight") {
											nameObject = I18n.t("Parede Direito");
										}
										tagObject = hit.transform.tag;
										StartCoroutine(WaitClick(0.3f));
										return;
									}
								}
								foreach (string name in objectsNames) {
//									int index = hit.transform.name.IndexOf("(Clone)");
//									string objName = hit.transform.name.Remove(index);
//									print(objName + " : " + name);
									string objName = name + "(Clone)";
									print (hit.transform.name + " : " + objName);
									if (hit.transform.name == objName) {
										GO = hit.transform.gameObject;
										render = hit.transform.GetComponentInChildren<Renderer>();
										tagObject = hit.transform.tag;
										color = render.materials[0].color;
										if (hit.transform.GetComponent<InformacoesMovel>() != null) {
											nameObject = hit.transform.GetComponent<InformacoesMovel>().Nome;
										}
										else {
											nameObject = "";
										}
										StartCoroutine(WaitClick(0.3f));
										return;
									}
								}
								render = null;
							}
						}
					} else dropperBoolLast = false;
				}
			}
		}
		
		if (dropperBool) {
			Vector2 mp = Event.current.mousePosition;
			GUI.DrawTexture(new Rect(mp.x, mp.y - halfSizeDropper.y, sizeDropper.x, sizeDropper.y), dropper);
			if (Screen.showCursor) Screen.showCursor = false;
		}
		else { if (!Screen.showCursor) Screen.showCursor = true; }
		
		if (render != null) {
			GUI.BeginGroup(rectWindow, nameObject, groupStyle);
			color = GUIControls.RGBCircle (position, color, "", colorCircle, pickerColor, slider);
			if (GUI.Button(rectReset, I18n.t("Branquear Objeto"), buttonStyle)) { color = Color.white; }
//			if (GUI.Button(rectGetAll, "Paint all this color")) {
//				GameObject[] walls = GameObject.FindGameObjectsWithTag("ParedeParent");
//				foreach (GameObject wall in walls) {
//					wall.renderer.material.color = color;
//				}
//				GameObject.FindWithTag("TetoParent").renderer.material.color = color;
//			}
			GUI.Label(rectGetAll, "RGB:");
			color.r = GUI.HorizontalSlider(rectRGBA[0], color.r, 0f, 1f);
			GUI.TextField(rectFieldRGBA[0], Convert.ToString((int)Mathf.Ceil(color.r * 255)));
			color.g = GUI.HorizontalSlider(rectRGBA[1], color.g, 0f, 1f);
			GUI.TextField(rectFieldRGBA[1], Convert.ToString((int)Mathf.Ceil(color.g * 255)));
			color.b = GUI.HorizontalSlider(rectRGBA[2], color.b, 0f, 1f);
			GUI.TextField(rectFieldRGBA[2], Convert.ToString((int)Mathf.Ceil(color.b * 255)));
			
			dropperBool = GUI.Toggle(rectDropper, dropperBool, dropper, "button");
			GUI.EndGroup();
			render.materials[0].color = color;
		} else {
			if (dropperBool) {
				dropperBool = false;
			}
		}
		GUI.depth = 0;
	}
	
	IEnumerator WaitClick(float timer) {
		clicked = true;
		yield return new WaitForSeconds(timer);
		clicked = false;
	}
}