using UnityEngine;
using System.Collections;
using System;
using Visiorama;

public class Painter: MonoBehaviour {
	public Texture2D background;
	public Texture2D topBackground;
	
	#region COLOR vars
	public Texture2D colorCircle;
	public Color color = Color.white;
	public GUIStyle pickerColor;
	public GUIStyle sliderColorPicker;
	public GUIStyle slider, thumb, button;
	public Texture2D dropper;
	#endregion
	
	#region COLOR vars
	public Texture2D[] wallTextures;
	#endregion
	
	public Font font, fontBold;
	public string[] tags, categoryNames;
	public bool dropperBool {get; private set;}
	public Renderer render {get; private set;}
	internal Rect rectWindow;
	private GameObject GO;
	private Rect rectTopColor, rectTopTexture;
	
	#region COLOR vars
	private Vector2 position, sizeDropper, halfSizeDropper;
	private Rect rectReset, rectGetAll, rectDropper;
	private Rect[] rectRGBA, rectFieldRGBA;
	private Color lastColor;
	private bool colorOption, textureOption;
	private bool dropperBoolLast, clicked;
	#endregion
	
	#region TEXTURE vars
	private Rect rectScrollTexture, rectListTexture;
	private Rect[] rectTextures;
	private Vector2 scrollPositionTexture;
	#endregion
	
	private string nameObject, tagObject;
	private GUIStyle groupStyle, topColorGroupStyle, topTextureGroupStyle, labelStyle;
	private int fontSizeGroup, fontSizeButton, fontSizeLabel, thumbSize;
	
	private GameObject[] cameras;
	
	// Setando posições (Chamar ela no Start e no ScreenSizeChange)
	void SetPositions () {
		rectWindow = ScreenUtils.ScaledRectInSenseHeight(Screen.width - 165, 24, 165, topBackground.height * 2);
		
		rectTopColor = ScreenUtils.ScaledRectInSenseHeight(0, 0, topBackground.width, topBackground.height);
		rectTopTexture = ScreenUtils.ScaledRectInSenseHeight(0, topBackground.height, topBackground.width, topBackground.height);
		topColorGroupStyle.normal.background = topBackground;
		topColorGroupStyle.fontSize = (int)Mathf.Ceil(ScreenUtils.ScaleHeight(fontSizeGroup));
		topTextureGroupStyle.normal.background = topBackground;
		topTextureGroupStyle.fontSize = (int)Mathf.Ceil(ScreenUtils.ScaleHeight(fontSizeGroup));
		print("topTextureGroupStyle.fontSize: " + topTextureGroupStyle.fontSize);
		
		#region Option COLOR vars
		
		//não precisa usar ScreenUtils, dentro da função isso já está sendo feito
		position = new Vector2(20, 50);
		
		rectReset = ScreenUtils.ScaledRectInSenseHeight(25, 150, 100, 20);
		rectGetAll = ScreenUtils.ScaledRectInSenseHeight(25, 180, 100, 20);
		rectRGBA = new Rect[3];
		rectFieldRGBA = new Rect[rectRGBA.Length];
		Rect rRGBA_Standart = ScreenUtils.ScaledRectInSenseHeight(25, 200, 60, 15);
		Rect rRGBA_field_Standart = ScreenUtils.ScaledRectInSenseHeight(25 + 65, 200, 40, 20);
		for (int i = 0; i != rectRGBA.Length; ++i) {
			rectRGBA[i] = rRGBA_Standart;
			rectFieldRGBA[i] = rRGBA_field_Standart;
			rectRGBA[i].y += rRGBA_field_Standart.height * i + ((rRGBA_field_Standart.height - rRGBA_Standart.height)/2);
			rectFieldRGBA[i].y += rRGBA_field_Standart.height * i;
		}
		sizeDropper = new Vector2(ScreenUtils.ScaleHeight(dropper.width), ScreenUtils.ScaleHeight(dropper.height));
		halfSizeDropper = new Vector2(dropper.width / 2, dropper.height);
		rectDropper = ScreenUtils.ScaledRectInSenseHeight(25 + 50 - 15, 270, 30, 30);
		#endregion
		
		float rectWindowWidth = ScreenUtils.DescaledHeight(rectWindow.width);
		
		#region Option TEXTURE vars
		if (wallTextures.Length > 3)rectScrollTexture = ScreenUtils.ScaledRectInSenseHeight(0, topBackground.height * 2 + 8, rectWindowWidth - 10, (320 + topBackground.height) - (topBackground.height * 2 + 8) - 10);
		else 						rectScrollTexture = ScreenUtils.ScaledRectInSenseHeight(0, topBackground.height * 2 + 8, rectWindowWidth, (320 + topBackground.height) - (topBackground.height * 2 + 8));
		if (wallTextures.Length > 3)rectListTexture = ScreenUtils.ScaledRectInSenseHeight(0, 0, rectWindowWidth - 30, 92 * wallTextures.Length);
		else 						rectListTexture = ScreenUtils.ScaledRectInSenseHeight(0, 0, rectWindowWidth, 92 * wallTextures.Length);
		rectTextures = new Rect[wallTextures.Length];
		for (int i = 0; i != rectTextures.Length; ++i) {
			rectTextures[i] = ScreenUtils.ScaledRectInSenseHeight((rectWindowWidth / 2) - (76 / 2), 92 * i, 76, 76);
		}
		#endregion
		
		button.fontSize = (int)Mathf.Ceil(ScreenUtils.ScaleHeight(fontSizeButton));
		labelStyle.fontSize = (int)Mathf.Ceil(ScreenUtils.ScaleHeight(fontSizeLabel));
		thumb.padding.top = thumb.padding.bottom = thumb.padding.left = thumb.padding.right = ScreenUtils.ScaledInt(thumbSize/2);
	}
	
	void Start () {
		ScreenUtils.Initialize(1024, 640);

		dropperBool = colorOption = textureOption = false;
		tagObject = "";
		
		fontSizeGroup = fontSizeButton = fontSizeLabel = 16;
		
		print("fontSizeGroup: " + fontSizeGroup);
		
		groupStyle = new GUIStyle();
		groupStyle.normal.background = background;
		groupStyle.alignment = TextAnchor.UpperCenter;
		
		topColorGroupStyle = topTextureGroupStyle = new GUIStyle();
		topColorGroupStyle.font = topTextureGroupStyle.font = fontBold;
		topColorGroupStyle.alignment = topTextureGroupStyle.alignment = TextAnchor.MiddleCenter;
		topColorGroupStyle.fontStyle = topTextureGroupStyle.fontStyle = FontStyle.Bold;
		topColorGroupStyle.normal.textColor = topTextureGroupStyle.normal.textColor = Color.white;
		
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
		
		cameras = GameObject.FindGameObjectsWithTag("GUICamera");
	}

	void OnGUI (){
		if (ScreenUtils.ScreenSizeChange()) {
			SetPositions ();
		}

		GUI.depth = 1;
		if (dropperBool) {
			if (Input.GetMouseButtonUp(0)) {
			
				//Testa se clicou dentro de alguma coisa da gui
				//se clicou sai do método
				if (NGUIUtils.ClickedInGUI (cameras, "GUI"))
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
				if (NGUIUtils.ClickedInGUI (cameras,"GUI"))
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
			// Buttons Options
			if (GUI.Button(rectTopColor, I18n.t("Cor"), topColorGroupStyle)) { 
				colorOption = !colorOption; 
				textureOption = false;
				if (colorOption) {
					rectTopTexture.y = ScreenUtils.ScaleHeight(320);
					rectWindow.height = ScreenUtils.ScaleHeight(320 + topBackground.height);
				} else {
					rectTopTexture.y = ScreenUtils.ScaleHeight(topBackground.height);
					rectWindow.height = ScreenUtils.ScaleHeight(topBackground.height * 2);
				}
			}
			
			if(tagObject.Equals("MovelSelecionado"))
				GUI.enabled = false;
			
			if (GUI.Button(rectTopTexture, "Textura", topTextureGroupStyle)) { 
				textureOption = !textureOption; 
				colorOption = false;
				if (textureOption) {
					rectWindow.height = ScreenUtils.ScaleHeight(320 + topBackground.height);
				} else {
					rectWindow.height = ScreenUtils.ScaleHeight(topBackground.height * 2);
				}
				rectTopTexture.y = ScreenUtils.ScaleHeight(topBackground.height);
			}
			GUI.enabled = true;
			
			// Option Color
			if (colorOption) {
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
				if (render.materials [0].color != color)
				{
					render.materials[0].color = color;

					if (render.transform.parent.name.ToLower().Contains ("wall"))
					{
						//Pega tudo da parede atual e troca a cor
						Renderer[] renders = GetWallRoot (render.transform).
																GetComponentsInChildren<Renderer>();
						for( int j = 0; j != renders.Length; ++j )
						{
							renders[j].materials[0].color = color;
							renders[j].transform.
											parent.
												GetComponent<InfoWall> ().color = color;

						}
					}
				}
			}
			
			// Option Texture
			if (textureOption)
			{
				if (wallTextures.Length > 3)
					scrollPositionTexture = GUI.BeginScrollView(rectScrollTexture,
																scrollPositionTexture,
																rectListTexture, false, true);
				else
					GUI.BeginGroup(rectScrollTexture);
				for (int i = 0; i != wallTextures.Length; ++i) {
					if (GUI.Button(rectTextures[i], wallTextures[i], button))
					{
						//Pega tudo da parede atual e troca a textura
						Renderer[] renders = GetWallRoot (render.transform).
																GetComponentsInChildren<Renderer>();
						for( int j = 0; j != renders.Length; ++j )
						{
							renders[j].sharedMaterial.mainTexture = wallTextures [i];
							renders[j].transform.
											parent.
												GetComponent<InfoWall> ().texture = wallTextures [i];
						}
					}
				}
				if (wallTextures.Length > 3)
					GUI.EndScrollView();
				else
					GUI.EndGroup();
			}
			
			GUI.EndGroup();
		} else {
			if (dropperBool) {
				dropperBool = false;
			}
			if (colorOption) {
				colorOption = false;
				rectTopTexture.y = ScreenUtils.ScaleHeight(topBackground.height);
				rectWindow.height = ScreenUtils.ScaleHeight(topBackground.height * 2);
			}
			if (textureOption) {
				textureOption = false;
				rectWindow.height = ScreenUtils.ScaleHeight(topBackground.height * 2);
			}
		}
		GUI.depth = 0;
	}
	
	IEnumerator WaitClick(float timer)
	{
		clicked = true;
		yield return new WaitForSeconds(timer);
		clicked = false;
	}

	Transform GetWallRoot(Transform trnsWall)
	{
		if (trnsWall.parent.name.Equals ("ParentParede"))
			return trnsWall;
		else
			return GetWallRoot(trnsWall.parent);
	}
}
