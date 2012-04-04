using UnityEngine;
using System.Collections;
using System;
using Visiorama.Utils;

public class Painter: MonoBehaviour {
	public Texture2D background;
	public Texture2D topBackground;
	public Texture2D colorCircle;
	public Color color = Color.white;
	public GUIStyle pickerColor;
	public GUIStyle sliderColorPicker;
	public GUIStyle slider, thumb, button;
	public Texture2D dropper;
	public Font font, fontBold;
	public string[] tags, categoryNames;
	public bool dropperBool {get; private set;}
	internal Rect rectWindow;
	private GameObject GO;
	private Renderer render;
	private Vector2 position, sizeDropper, halfSizeDropper;
	private Rect rectTop, rectReset, rectGetAll, rectDropper;
	private Rect[] rectRGBA, rectFieldRGBA;
	private Color lastColor;
	private bool dropperBoolLast, clicked;
	private string nameObject, tagObject;
	private GUIStyle groupStyle, topGroupStyle, labelStyle;
	private int fontSizeGroup, fontSizeButton, fontSizeLabel, thumbSize;
	private CameraGUIController cameraGUIController;
	
	// Setando posições (Chamar ela no Start e no ScreenSizeChange)
	void SetPositions () {
		rectWindow = ScreenUtils.ScaledRectInSenseHeight(Screen.width - 165, 24, 165, 320);
		
		rectTop = ScreenUtils.ScaledRectInSenseHeight(0, 0, topBackground.width, topBackground.height);
		topGroupStyle.normal.background = topBackground;
		topGroupStyle.fontSize = (int)Mathf.Ceil(ScreenUtils.ScaleHeight(fontSizeGroup));
		
		//não precisa usar ScreenUtils, dentro da função isso já está sendo feito
		position = new Vector2(20, 50);
		rectReset = ScreenUtils.ScaledRectInSenseHeight(25, 150, 100, 20);
		rectGetAll = ScreenUtils.ScaledRectInSenseHeight(25, 180, 100, 20);
		rectRGBA = new Rect[3];
		rectFieldRGBA = new Rect[rectRGBA.Length];
		Rect rRGBA_Standart = ScreenUtils.ScaledRectInSenseHeight(25, 200, 60, 15);
		Rect rRGBA_field_Standart = ScreenUtils.ScaledRectInSenseHeight(25 + 65, 200, 35, 20);
		for (int i = 0; i != rectRGBA.Length; ++i) {
			rectRGBA[i] = rRGBA_Standart;
			rectFieldRGBA[i] = rRGBA_field_Standart;
			rectRGBA[i].y += rRGBA_field_Standart.height * i + ((rRGBA_field_Standart.height - rRGBA_Standart.height)/2);
			rectFieldRGBA[i].y += rRGBA_field_Standart.height * i;
		}
		sizeDropper = new Vector2(ScreenUtils.ScaleHeight(dropper.width), ScreenUtils.ScaleHeight(dropper.height));
		halfSizeDropper = new Vector2(dropper.width / 2, dropper.height);
		rectDropper = ScreenUtils.ScaledRectInSenseHeight(25 + 50 - 15, 270, 30, 30);	
		button.fontSize = (int)Mathf.Ceil(ScreenUtils.ScaleHeight(fontSizeButton));
		labelStyle.fontSize = (int)Mathf.Ceil(ScreenUtils.ScaleHeight(fontSizeLabel));
		thumb.padding.top = thumb.padding.bottom = thumb.padding.left = thumb.padding.right = ScreenUtils.ScaledInt(thumbSize/2);
	}
	
	void Start () {
		ScreenUtils.Initialize(1024, 640);

		dropperBool = false;
		tagObject = "";
		
		fontSizeGroup = fontSizeButton = fontSizeLabel = 14;
		
		groupStyle = new GUIStyle();
		groupStyle.normal.background = background;
		groupStyle.alignment = TextAnchor.UpperCenter;
		
		topGroupStyle = new GUIStyle();
		topGroupStyle.font = fontBold;
		topGroupStyle.alignment = TextAnchor.MiddleCenter;
		topGroupStyle.fontStyle = FontStyle.Bold;
		topGroupStyle.normal.textColor = Color.white;
		
		button.alignment = TextAnchor.MiddleCenter;
		button.font = font;
		button.fontStyle = FontStyle.Bold;
		button.normal.textColor = Color.white;
		
		labelStyle = new GUIStyle("label");
		labelStyle.font = font;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.normal.textColor = Color.white;
		
		thumbSize = thumb.normal.background.height;
		thumb.fixedWidth = thumb.fixedHeight = 0;
		
		SetPositions ();
		
		cameraGUIController = GameObject.Find("CameraController").GetComponentInChildren<CameraGUIController> ();
	}

	void  OnGUI (){
		if (ScreenUtils.ScreenSizeChange()) {
			SetPositions ();
		}

		GUI.depth = 1;
		if (dropperBool) {
			if (Input.GetMouseButtonUp(0)) {
			
				//Testa se clicou dentro de alguma coisa da gui
				//se clicou sai do método
				if (cameraGUIController.ClickInGUI ())
					return;
						
				if (!MouseUtils.MouseClickedInArea(rectWindow)) {
					Ray ray = transform.parent.camera.ScreenPointToRay(Input.mousePosition);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit)) {
						foreach (string tag in tags) {
							if (hit.transform.tag.Equals(tag)) {
								if (hit.transform.tag.Equals("Parede")) {
									color = hit.transform.GetComponentInChildren<Renderer>().material.color;
								}
								else if (hit.transform.name.Equals("Teto")) {
									color = hit.transform.renderer.material.color;
								}
							}
						}
						foreach (string categoryName in categoryNames) 
						{
							if ( hit.transform.gameObject.GetComponent<InformacoesMovel>() != null)
							{
								if (hit.transform.gameObject.GetComponent<InformacoesMovel>().Categoria == categoryName) 
								{
									color = hit.transform.GetComponentInChildren<Renderer>().materials[0].color;
								}
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
				
				//Testa se clicou dentro de alguma coisa da gui
				//se clicou sai do método
				if (cameraGUIController.ClickInGUI ())
					return;
					
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
			if (MouseUtils.GUIMouseButtonDoubleClick(0)) {
				if (!dropperBoolLast) {
					bool breaker = false;
					if (render != null) {
						if (MouseUtils.MouseClickedInArea(rectWindow)) breaker = true;
					}

					if (!breaker) {
						Ray ray = transform.parent.camera.ScreenPointToRay(Input.mousePosition);
						RaycastHit hit;
						if (Physics.Raycast(ray, out hit)) {
							foreach (string tag in tags) {
								print("hit.transform.tag: " + hit.transform.tag + " : " + hit.transform.tag.Equals("Parede"));
								if (hit.transform.tag.Equals(tag)) {
									print("render: " + render);
									if (hit.transform.tag.Equals("Parede")) {
										nameObject = I18n.t("Parede");
										render = hit.transform.GetComponentInChildren<Renderer>();
									}
									else if (hit.transform.tag.Equals("Teto")) {
										nameObject = I18n.t("Teto");
										render = hit.transform.renderer;
									}
									color = render.material.color;
									tagObject = hit.transform.tag;
									StartCoroutine(WaitClick(0.3f));
									return;
								}
							}
							foreach (string categoryName in categoryNames)
							{
								if (hit.transform.GetComponent<InformacoesMovel> () != null)
								{
									if (hit.transform.GetComponent<InformacoesMovel> ().Categoria == categoryName)
									{
										GO = hit.transform.gameObject;
										render = hit.transform.GetComponentInChildren<Renderer>();
										tagObject = hit.transform.tag;
										color = render.materials[0].color;
										
										nameObject = hit.transform.GetComponent<InformacoesMovel>().Nome;
										
										StartCoroutine(WaitClick(0.3f));
										return;
									}
								}
							}
							render = null;
						}
					}
				} else dropperBoolLast = false;
			}
		}

		if (dropperBool) {
			GUI.depth = 2;
			Vector2 mp = Event.current.mousePosition;
			GUI.DrawTexture(new Rect(mp.x, mp.y - halfSizeDropper.y, sizeDropper.x, sizeDropper.y), dropper);
			if (Screen.showCursor) Screen.showCursor = false;
			GUI.depth = 1;
		}
		else { if (!Screen.showCursor) Screen.showCursor = true; }

		if (render != null) {
			rectWindow.x = Screen.width - rectWindow.width;
			GUI.BeginGroup(rectWindow, "", groupStyle);
			GUI.Label(rectTop, I18n.t("Cor"), topGroupStyle);
			color = GUIControls.RGBCircle (position, color, "", colorCircle, pickerColor, sliderColorPicker, thumb);
			if (GUI.Button(rectReset, I18n.t("Descolorir"), button)) { color = Color.white; }
			GUI.Label(rectGetAll, "RGB:", labelStyle);
			color.r = GUI.HorizontalSlider(rectRGBA[0], color.r, 0f, 1f, slider, thumb);
			GUI.Label(rectFieldRGBA[0], Convert.ToString((int)Mathf.Ceil(color.r * 255)), labelStyle);
			color.g = GUI.HorizontalSlider(rectRGBA[1], color.g, 0f, 1f, slider, thumb);
			GUI.Label(rectFieldRGBA[1], Convert.ToString((int)Mathf.Ceil(color.g * 255)), labelStyle);
			color.b = GUI.HorizontalSlider(rectRGBA[2], color.b, 0f, 1f, slider, thumb);
			GUI.Label(rectFieldRGBA[2], Convert.ToString((int)Mathf.Ceil(color.b * 255)), labelStyle);

			dropperBool = GUI.Toggle(rectDropper, dropperBool, dropper, button);
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