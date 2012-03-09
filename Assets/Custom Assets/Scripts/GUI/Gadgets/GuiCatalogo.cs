using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Visiorama.Utils;

[System.Serializable]
public class PrefabsTop {
	public GUIStyle iconButton;
	public GameObject objeto;
}

[System.Serializable]
public class Slider {
	public float min, max;
}

public class GuiCatalogo : MonoBehaviour, GuiBase {
	
	#region Common
	public GUIStyle bgStyle;//Editor
	#endregion
	
	#region Accordion Vars
	public Rect wndAccordMain {get; private set;}
	private Rect wndAccordOption;
	private Rect btnAccordion;
	public GUIStyle[] accordionStyle;//Editor
	#endregion
	
	#region Ground, walls  and Ceil Menu
	public Texture2D[] groundTextures;//Editor
	public Texture2D[] ceilTextures;  //Editor
	
	private Rect wndFrameTextures;
	
	private Rect rListGround;
	private Vector2 scrollPosGround = Vector2.zero;
	private Rect btnUpGround;
	private Rect btnDownGround;
	
	private Rect rListCeil;
	private Vector2 scrollPosCeil = Vector2.zero;
	private Rect btnUpCeil;
	private Rect btnDownCeil;
	
	public int CurrentTextureIndex { get; private set; }
	#endregion
	
	#region Tampo Menu
	private Rect rListTampo;
	private Vector2 scrollPosTampo = Vector2.zero;
	private Rect scrollFraTampo;
	#endregion
	
	#region Category Menu
	private Rect rListCategory;
	private Vector2 scrollPosCategory = Vector2.zero;
	private Vector2 scrollPosItem = Vector2.zero;
	private Rect scrollFraCategory;
	private int cCategory = -1;
	private int nCategories;
	private bool wasInitialized;
	private Rect btnUp;
	private Rect btnDown;
	#endregion
	
	#region Item Menu
	private Rect rListItem;
	private Vector2 scroll;
	private Rect scrollFraItem;
	private List<GameObject> items;
	bool showItems = false;
	#endregion
	
	#region Extras Menu
	private Rect rListExtras;
	private Vector2 scrollPosExtras;
	private Rect scrollFraExtras;
	private List<GameObject> extrasObjects;
	#endregion
	
	#region Troca de portas
	public GUIStyle LeftDoorStyle; //Editor
	public GUIStyle RightDoorStyle; //Editor
	#endregion
	
	#region Main Light Menu
	public Transform transformLight; //Editor
	public GUIStyle lightSliderStyle; //Editor
	public GUIStyle lightThumbStyle; //Editor
	public Slider verticalValue; //Editor
	public Slider horizontalValue; //Editor
	public GameObject lampPoint, lampSpot; //Editor
	
	private float rotationLightValueX;
	private float rotationLightValueY;
	#endregion
	
	#region Extras
	public GameObject extras;
	#endregion
	
	#region Fonts
	GUIStyle labelSubAccordion;
	#endregion
	
	#region Accordion items vars
	private int selectColorBase = 0, selectColorDetalhe = 0, selectTampoTexture = 0;
	private bool SelectedLeftDoor;
	#endregion
	
	#region Gui Movement
	private float lerpStep;
	private float lerpTime;
	private float cLerp;
	#endregion
	
	#region Accordion Content Vars
	public PrefabsTop[] prefabsTop; //Editor
	public Material	materialTampo; //Editor
	public Texture2D radioSelect; //Editor
	Rect wndAccordionSelectedOption;
	float wndAccordionHeight = 400f;
	int id = 3, lastId = 3;
	#endregion
	
	#region Styles
	public GUIStyle hScrollStyle; //Editor
	public GUIStyle vScrollStyle; //Editor
	public GUIStyle[] randomStyles; //Editor
	public GUIStyle btnUpStyle; //Editor
	public GUIStyle btnDownStyle; //Editor
	public GUIStyle accordionBtnStyle; //Editor
	#endregion
	
	#region shared vars
    RaycastHit hit;
    Ray ray;
	#endregion
	
	private string[] tips;
	private string[] labels;
	
	#region MonoBehaviour Methods 
	public void Start() {
		
		lerpStep 	= 0.1F;
		lerpTime 	= 3.0F;
		cLerp 		= 0f;
		
		wndAccordOption = ScreenUtils.ScaledRect(8f, 24f, 119f, 400f);
		btnAccordion = ScreenUtils.ScaledRect(13f, 24f, 107f, 22f);
		
		items = new List<GameObject>();
		
		btnUp 	= ScreenUtils.ScaledRect(42f, 155f, 37f, 37f);
		btnDown = ScreenUtils.ScaledRect(42f, 585f, 37f, 37f);
		
		btnUpGround	  = ScreenUtils.ScaledRect(0f, 155f, 32f, 32f);
		btnDownGround = ScreenUtils.ScaledRect(23f, 400f, 25f, 25f);
		
		rotationLightValueX = transformLight.transform.localEulerAngles.x;
		rotationLightValueY = transformLight.transform.localEulerAngles.y;
		
		labelSubAccordion = GuiFont.GetFont("Trebuchet12");
		labelSubAccordion.alignment = TextAnchor.UpperCenter;
		labelSubAccordion.wordWrap = true;
		
		labels = new string[5];
		labels[0] = I18n.t("menu-catalogo-accordion-textura");	
//		labels[1] = I18n.t("menu-catalogo-accordion-tampo");	
//		labels[2] = I18n.t("menu-catalogo-accordion-portas");	
		labels[1] = I18n.t("menu-catalogo-accordion-cor-detalhe");	
		labels[2] = I18n.t("menu-catalogo-accordion-iluminacao");
		labels[3] = I18n.t("menu-catalogo-accordion-moveis");
		labels[4] = "Extras";

		tips = new string[labels.Length];
		tips[0] = I18n.t("tip-menu-catalogo-accordion-textura");	
//		tips[1] = I18n.t("tip-menu-catalogo-accordion-tampo");	
//		tips[2] = I18n.t("tip-menu-catalogo-accordion-portas");	
		tips[1] = I18n.t("tip-menu-catalogo-accordion-cor-detalhe");	
		tips[2] = I18n.t("tip-menu-catalogo-accordion-iluminacao");	
		tips[3] = I18n.t("tip-menu-catalogo-accordion-moveis");	
		tips[4] = "Extras";	
	
	    Tooltip.AddDynamicTip(tips[0]);
	    Tooltip.AddDynamicTip(tips[1]);
	    Tooltip.AddDynamicTip(tips[2]);
	    Tooltip.AddDynamicTip(tips[3]);
	    Tooltip.AddDynamicTip(tips[4]);
//	    Tooltip.AddDynamicTip(tips[5]);
	    
	    GuiFont.ChangeFont(accordionBtnStyle,"Trebuchet14");
	    
		wndAccordMain = new Rect(wndAccordOption.x, wndAccordOption.y - ScreenUtils.ScaleHeight(16f), ScreenUtils.ScaleWidth(119f), ScreenUtils.ScaleHeight(627f));
		
	}
	#endregion
	
	#region Draw Content
	public void Draw () {
		if(!wasInitialized){
			if(Line.WasInitialized != null){
			
			    wndFrameTextures = ScreenUtils.ScaledRect(15, 205, 80, 64 * 3);
			    
			    foreach(Category category in Line.CurrentLine.categories){
					Tooltip.AddDynamicTip(category.Name);
			    }
				
				extrasObjects = new List<GameObject>();
				foreach (Transform ex in extras.transform) {
					ex.GetComponent<InformacoesMovel>().Categoria = "";
					extrasObjects.Add(ex.gameObject);
					Tooltip.AddDynamicTip(ex.GetComponent<InformacoesMovel>().NomeP);
				}
			    
				nCategories	  = Line.CurrentLine.categories.Count;
				rListCategory = ScreenUtils.ScaledRect(0, 0, 64, 64 * nCategories);
				rListTampo  = ScreenUtils.ScaledRect(0, 0, 90, 90 * 4);
				rListGround = ScreenUtils.ScaledRect(0, 0, 90, 64 * groundTextures.Length);
				rListExtras = ScreenUtils.ScaledRect(0, 0, 64, 64 * extrasObjects.Count);
				
				//Pegar a camera mãe com os materiais que devem ser mudados
				GameObject cameraMae = gameObject.transform.parent.gameObject;
				
				wasInitialized = true;
			}
			return;
		}
		
		GUI.Box(wndAccordMain, "", accordionStyle[0]);
		id = GUIUtils.AccordionVertical(btnAccordion, id, labels, tips, btnAccordion.height, wndAccordOption.height, accordionBtnStyle);
		if (id != lastId) {
		
			//Tampo, portas, cor base, cor detalhe
			if (id == 1) {
			
				GameObject selectedMobile = GameObject.FindGameObjectWithTag("MovelSelecionado");
				
				//Se não foi selecionado nenhum móvel sai e mantém a última aba selecionada
				if (selectedMobile == null){
					id = lastId;
					return;
				}
			}
			if (id != -1) {
				wndAccordionSelectedOption = new Rect(wndAccordOption.x,  wndAccordOption.y + (btnAccordion.height * (id + 1) + ScreenUtils.ScaleHeight(48)), ScreenUtils.ScaleWidth(200), wndAccordOption.height);
			}
			lastId = id;
		}
		
		ShowAccordion();
		
		Tooltip.DoTips();
	}
	#endregion
	
	#region Accordion Content
	void ShowAccordion () {
		if (id != -1) {
			switch (id) {
				case 0 : //Textura
					#region Selecionar texturas de chão.
						#region Ground
						/*
						foreach(ProceduralPropertyDescription tweak in TweaksList)
						{
							string tweakSpaces = tweak.name.Replace('_',' ');
							
							
							//TODO make exposes works 
							if (tweak.type == ProceduralPropertyType.Color3 || tweak.type == ProceduralPropertyType.Color4)
							{
								colorPickers[tweak.name].drawUI(ScreenUtils.ScaledRect(50,50,100,30));
							}
						}
						*/	
						
						GUI.Label(ScreenUtils.ScaledRect(0,140,125,50), I18n.t("Piso"), labelSubAccordion);
						
						scrollPosGround	 = GUI.BeginScrollView(wndFrameTextures, scrollPosGround, rListGround, hScrollStyle, vScrollStyle);
							for (int i = 0; i != groundTextures.Length; i++){
						 
				//				if ( (( i * 90 ) + 20) < scrollPosGround.x || 
				//		    		 (( i * 90 ) + 90 + 20) > (scrollPosGround.x + wndGround.width)) {
				//					Debug.Log("scrollPosGround.x + wndGround.width: " + (scrollPosGround.x + wndGround.width));
				//					Debug.Log("i * 90: " + (i * 90));
				//					continue;
				//				}
						
								if(GUI.Button(ScreenUtils.ScaledRect(20, 1 + (64 * i), 64, 64), groundTextures[i])){
										CurrentTextureIndex = i;
									GameObject[] Grounds = GameObject.FindGameObjectsWithTag("ChaoParent");
									foreach(GameObject walls in Grounds){
										Renderer[] renders = walls.GetComponentsInChildren<Renderer>();
										foreach (Renderer r in renders){
											r.material.mainTexture = groundTextures[i];
											//(r.sharedMaterial as ProceduralMaterial).SetProceduralTexture("Input",groundTextures[i]);
											//(r.sharedMaterial as ProceduralMaterial).RebuildTexturesImmediately();
										}
									}
								}
							}
						GUI.EndScrollView();
						
						if (scrollPosGround.y > 32) {
							if(GUI.Button(new Rect(	wndAccordOption.x + ScreenUtils.ScaleWidth(42),
						                                			wndAccordOption.y + (btnAccordion.height * (id + 1)) + ScreenUtils.ScaleHeight(120),
					                 								ScreenUtils.ScaleWidth(accordionStyle[1].normal.background.width), 
						                                			ScreenUtils.ScaleHeight(accordionStyle[1].normal.background.height)), "", accordionStyle[1])){
								SomClique.Play();
								scrollPosGround.y -= ScreenUtils.ScaleHeight(64);
							}
						}
						if (scrollPosGround.y + ScreenUtils.ScaleHeight(64) * 3 < rListGround.height) {
							if(GUI.Button(new Rect(	wndAccordOption.x + ScreenUtils.ScaleWidth(42),
									                                wndAccordOption.y - (btnAccordion.height * (1 - id)) + wndAccordOption.height,
							           								ScreenUtils.ScaleWidth(accordionStyle[2].normal.background.width), 
							                                  		ScreenUtils.ScaleHeight(accordionStyle[2].normal.background.height)),"",accordionStyle[2])){
								SomClique.Play();
								scrollPosGround.y += ScreenUtils.ScaleHeight(64);
							}
						} 
						#endregion
					#endregion
					break;
				/*case 1 : //Tampo
					#region Conteúdo do Tampo no Accordion
							GUI.BeginGroup(wndAccordionSelectedOption);
								for (int i = 0; i != tamposTextures.Length; ++i) {
									if(GUI.Button(ScreenUtils.ScaledRect(wndAccordOption.x + 20, 72 * i, 64, 64), "", tamposTextures[i].iconButton)){
										if (selectTampoTexture != i) {
											InformacoesMovel selectedMobile = GameObject.FindGameObjectWithTag("MovelSelecionado").GetComponent<InformacoesMovel>();
											if (selectedMobile != null) {
												selectedMobile.ChangeTexture(tamposTextures[i].texture, "Tampos");
												materialTampo.mainTexture = tamposTextures[i].texture;
											}
											else {
												return;
											}
											
											GameObject[] furniture = GameObject.FindGameObjectsWithTag("Movel");
											if(furniture != null && furniture.Length != 0) {
												foreach(GameObject mobile in furniture) {
													mobile.GetComponent<InformacoesMovel>().ChangeTexture(tamposTextures[i].texture, "Tampos");
												}
											}
											selectTampoTexture = i;
										}
									}
								}
							GUI.EndGroup();
					#endregion
					break;
				case 2 : //Portas
						#region Selecionar porta esquerda ou direita
							GUI.BeginGroup(wndAccordionSelectedOption);
									if(GUI.Button(ScreenUtils.ScaledRect(30, 0, 64, 32),  "", LeftDoorStyle) ||
									   GUI.Button(ScreenUtils.ScaledRect(30, 48, 64, 32), "", RightDoorStyle)){
										
										SelectedLeftDoor = !SelectedLeftDoor;
										
										SomClique.Play();
								
										GameObject selectedMobile = GameObject.FindGameObjectWithTag("MovelSelecionado");
										
										if(selectedMobile != null){
											selectedMobile.GetComponent<InformacoesMovel>().ToogleDoorSide();
										}
										
									}
								GUI.Box(ScreenUtils.ScaledRect(24, 48 * (SelectedLeftDoor ? 0 : 1) ,64, 32), radioSelect);
							GUI.EndGroup();
						#endregion
					break;*/
				case 1 : //Cor Detalhe
					#region Mudar cor detalhe
						GUI.BeginGroup(wndAccordionSelectedOption);
							for (int i = 0; i != Line.CurrentLine.colors.Length; ++i) {
								if(GUI.Button(ScreenUtils.ScaledRect(24, 48 * i, 64, 32), Line.CurrentLine.colorsImg[i])){
								
									SomClique.Play();
									selectColorDetalhe = i;
							
									Line.CurrentLine.GlobalDetailColorIndex = (uint)selectColorDetalhe;
							
									GameObject selectedMobile = GameObject.FindGameObjectWithTag("MovelSelecionado");
									if(selectedMobile != null){
										selectedMobile.GetComponent<InformacoesMovel>().ChangeDetailColor((uint)selectColorDetalhe);										
									}
							
									GameObject[] furniture = GameObject.FindGameObjectsWithTag("Movel");
									if(furniture != null && furniture.Length != 0){
										foreach(GameObject mobile in furniture){
											mobile.GetComponent<InformacoesMovel>().ChangeDetailColor((uint)selectColorDetalhe);										
										}
									}
								}
							}
							GUI.Box(ScreenUtils.ScaledRect(24, 48 * selectColorDetalhe, 64, 32), radioSelect);
						GUI.EndGroup();
					#endregion
					break;
				case 2 : //Iluminação
					#region Iluminação
						GUI.BeginGroup(wndAccordionSelectedOption);
							GUI.Label(ScreenUtils.ScaledRect(7.5f, 0f, 106f, 50f), I18n.t("Posicionamento Solar"), labelSubAccordion);
							GUI.Label(ScreenUtils.ScaledRect(7.5f, 40f, 106f, 50f), "Vertical", labelSubAccordion);
							rotationLightValueX = GUI.HorizontalSlider(ScreenUtils.ScaledRect(5f, 80f, 106f, 15f), rotationLightValueX, verticalValue.min, verticalValue.max, lightSliderStyle, lightThumbStyle);
							GUI.Label(ScreenUtils.ScaledRect(7.5f, 120f, 106f, 50f), "Horizontal", labelSubAccordion);
							rotationLightValueY = GUI.HorizontalSlider(ScreenUtils.ScaledRect(5f, 160f, 106f, 15f), rotationLightValueY, horizontalValue.min, horizontalValue.max, lightSliderStyle, lightThumbStyle);
							transformLight.transform.localEulerAngles = new Vector3(	rotationLightValueX,
				                                                          				rotationLightValueY,
				                                                          				0);
							if (GUI.Button(ScreenUtils.ScaledRect(25f, 200f, 
				                                      	64, 
				                                      	64), 
				               							lampPoint.GetComponent<InformacoesMovel>().Imagem)) {
								InstanceNewObject(lampPoint);
							}
							if (GUI.Button(ScreenUtils.ScaledRect(25f, 264, 
				                                      	64, 
				                                      	64), 
				               							lampSpot.GetComponent<InformacoesMovel>().Imagem)) {
								InstanceNewObject(lampSpot);
							}
						GUI.EndGroup();
					#endregion
					#region Mudar cor da estrutura 
//						GUI.BeginGroup(wndAccordionSelectedOption);
//							for (int i = 0; i != Line.CurrentLine.colors.Length; ++i) {
//								if(GUI.Button(ScreenUtils.ScaledRect(24, 48 * i, 64, 32), Line.CurrentLine.colorsImg[i])){
//									SomClique.Play();
//									selectColorBase = i;
//								
//									Line.CurrentLine.GlobalBaseColorIndex = (uint)selectColorBase;
//							
//									GameObject selectedMobile = GameObject.FindGameObjectWithTag("MovelSelecionado");
//									if(selectedMobile != null){
//										selectedMobile.GetComponent<InformacoesMovel>().ChangeBaseColor((uint)selectColorBase);										
//									}
//							
//									GameObject[] furniture = GameObject.FindGameObjectsWithTag("Movel");
//									if(furniture != null && furniture.Length != 0){
//										foreach(GameObject mobile in furniture){
//											mobile.GetComponent<InformacoesMovel>().ChangeBaseColor((uint)selectColorBase);										
//										}
//									}
//								}
//							}
//							GUI.Box(ScreenUtils.ScaledRect(24, 48 * selectColorBase,
//						                 64, 32), radioSelect);
//						GUI.EndGroup();
					#endregion
					break;
				case 3 : //Instanciação de novos objetos
					#region Instanciação de novos objetos
						if (!showItems) {
							scrollFraCategory = new Rect(0, wndAccordOption.y + (btnAccordion.height * id) + ScreenUtils.ScaleHeight(116), 
					                             ScreenUtils.ScaleWidth(100), wndAccordOption.height - ScreenUtils.ScaleHeight(144));
							scrollPosCategory = GUI.BeginScrollView(scrollFraCategory, scrollPosCategory, rListCategory, hScrollStyle,vScrollStyle);
								for (int i = 0; i != nCategories; ++i) {
									if(GUI.Button(new Rect(wndAccordOption.x + ScreenUtils.ScaleWidth(24), ScreenUtils.ScaleHeight(64) * i,
								                 ScreenUtils.ScaleWidth(64), ScreenUtils.ScaleHeight(64)), new GUIContent(Line.CurrentLine.categories[i].Image, Line.CurrentLine.categories[i].Name))) {
										SomClique.Play();
										cCategory = i;
										items = new List<GameObject>(Line.CurrentLine.categories[cCategory].Furniture);
										
										for (int j = 0; j != items.Count; ++j){
											if (Regex.Match(items[j].name,".*(com tampo|c tampo|cook top|cooktop|pia|direita).*",RegexOptions.IgnoreCase).Success) {
												items.RemoveAt(j);
												--j;
												continue;
											}
											items[j].GetComponent<InformacoesMovel>().Initialize();
											Tooltip.AddDynamicTip(items[j].GetComponent<InformacoesMovel>().Nome);
										}
							
										rListItem = ScreenUtils.ScaledRect(0, 0, 64, 64 * items.Count);
										showItems = !showItems;
									}
								}
							GUI.EndScrollView();
							
							if (scrollPosCategory.y > 0) {
								if (GUI.Button(new Rect(	wndAccordOption.x + ScreenUtils.ScaleWidth(40),
							                                      		wndAccordOption.y + (btnAccordion.height * (id + 1)) + ScreenUtils.ScaleHeight(56),
						                 								ScreenUtils.ScaleWidth(accordionStyle[1].normal.background.width), 
							                                      		ScreenUtils.ScaleHeight(accordionStyle[1].normal.background.height)), "", accordionStyle[1]))
									scrollPosCategory.y -= ScreenUtils.ScaleHeight(64);
							}
						
							if (scrollPosCategory.y + ScreenUtils.ScaleHeight(64) * 4 < rListCategory.height) {
								if (GUI.Button(new Rect(	wndAccordOption.x + ScreenUtils.ScaleWidth(40), 
							                                      		wndAccordOption.y - (btnAccordion.height * (1 - id)) + wndAccordOption.height,
						                 								ScreenUtils.ScaleWidth(accordionStyle[2].normal.background.width), 
							                                      		ScreenUtils.ScaleHeight(accordionStyle[2].normal.background.height)), "", 
							               								accordionStyle[2]))
									scrollPosCategory.y += ScreenUtils.ScaleHeight(64);
							}
						} else {
							
							scrollFraItem = new Rect(0, wndAccordOption.y + (btnAccordion.height * id) + ScreenUtils.ScaleHeight(116), 
					                         ScreenUtils.ScaleWidth(100), wndAccordOption.height - ScreenUtils.ScaleHeight(144));
							scrollPosItem = GUI.BeginScrollView(scrollFraItem, scrollPosItem, rListItem, hScrollStyle, btnUpStyle);
								for (int i = 0; i != items.Count; ++i){
									if(GUI.Button(new Rect(	wndAccordOption.x + ScreenUtils.ScaleWidth(24), 
															ScreenUtils.ScaleHeight(64) * i,
								                 			ScreenUtils.ScaleWidth(64), 
								                 			ScreenUtils.ScaleHeight(64)), 
								                 				new GUIContent(	items[i].GetComponent<InformacoesMovel>().Imagem, 
								                 								items[i].GetComponent<InformacoesMovel>().Nome))){
										SomClique.Play();
										InstanceNewObject(i);
									}
								}
						
							GUI.EndScrollView ();
							
							if (GUI.Button(new Rect(	wndAccordOption.x + ScreenUtils.ScaleWidth(40), 
						                                      		wndAccordOption.y + (btnAccordion.height * (id + 1)) + ScreenUtils.ScaleHeight(16),
					                 								ScreenUtils.ScaleWidth(accordionStyle[3].normal.background.width), 
						                                      		ScreenUtils.ScaleHeight(accordionStyle[3].normal.background.height)), "", accordionStyle[3])) {
								showItems = false;
								scrollPosItem = Vector3.zero;
							}
							
							if (scrollPosItem.y > 0) {
								if (GUI.Button(new Rect(	wndAccordOption.x + ScreenUtils.ScaleWidth(40), 
							                                      		wndAccordOption.y + (btnAccordion.height * (id + 1)) + ScreenUtils.ScaleHeight(56),
						                 								ScreenUtils.ScaleWidth(accordionStyle[1].normal.background.width), 
							                                      		ScreenUtils.ScaleHeight(accordionStyle[1].normal.background.height)), "", accordionStyle[1]))
									scrollPosItem.y -= ScreenUtils.ScaleHeight(64);
							}
					
							if (scrollPosItem.y + ScreenUtils.ScaleHeight(64) * 4 < rListItem.height) {
								if (GUI.Button(new Rect(	wndAccordOption.x + ScreenUtils.ScaleWidth(40), 
							                                      		wndAccordOption.y - (btnAccordion.height * (1 - id)) + wndAccordOption.height,
						                 								ScreenUtils.ScaleWidth(accordionStyle[2].normal.background.width), 
							                                      		ScreenUtils.ScaleHeight(accordionStyle[2].normal.background.height)), "", 
							               								accordionStyle[2]))
									scrollPosItem.y += ScreenUtils.ScaleHeight(64);
							}
						}
						break;
					#endregion
				case 4 : //Extras
					#region Extras			
						scrollFraExtras = new Rect(0, wndAccordOption.y + (btnAccordion.height * id) + ScreenUtils.ScaleHeight(116), 
				                         ScreenUtils.ScaleWidth(100), wndAccordOption.height - ScreenUtils.ScaleHeight(144));
						scrollPosExtras = GUI.BeginScrollView(scrollFraExtras, scrollPosExtras, rListExtras, hScrollStyle, btnUpStyle);
						
							for (int i = 0; i != extrasObjects.Count; ++i){
						
								if(GUI.Button(new Rect(	wndAccordOption.x + ScreenUtils.ScaleWidth(24), 
														ScreenUtils.ScaleHeight(64) * i,
							                 			ScreenUtils.ScaleWidth(64), 
							                 			ScreenUtils.ScaleHeight(64)), 
							                 				new GUIContent(	extrasObjects[i].GetComponent<InformacoesMovel>().Imagem, 
							                 								extrasObjects[i].GetComponent<InformacoesMovel>().NomeP))){
									SomClique.Play();
									InstanceNewObject(extrasObjects[i]);
								}
							}
					
						GUI.EndScrollView ();
						
						if (scrollPosExtras.y > 0) {
							if (GUI.Button(new Rect(	wndAccordOption.x + ScreenUtils.ScaleWidth(40), 
						                                      		wndAccordOption.y + (btnAccordion.height * (id + 1)) + ScreenUtils.ScaleHeight(56),
					                 								ScreenUtils.ScaleWidth(accordionStyle[1].normal.background.width), 
						                                      		ScreenUtils.ScaleHeight(accordionStyle[1].normal.background.height)), "", accordionStyle[1]))
								scrollPosExtras.y -= ScreenUtils.ScaleHeight(64);
						}
				
						if (scrollPosExtras.y + ScreenUtils.ScaleHeight(64) * 4 < rListExtras.height) {
							if (GUI.Button(new Rect(	wndAccordOption.x + ScreenUtils.ScaleWidth(40), 
						                                      		wndAccordOption.y - (btnAccordion.height * (1 - id)) + wndAccordOption.height,
					                 								ScreenUtils.ScaleWidth(accordionStyle[2].normal.background.width), 
						                                      		ScreenUtils.ScaleHeight(accordionStyle[2].normal.background.height)), "", 
						               								accordionStyle[2]))
								scrollPosExtras.y += ScreenUtils.ScaleHeight(64);
						}
						break;
					#endregion
				default : //Problemas Oõ
					Debug.LogError("Erro!");
					break;
			}
		}
	}
	#endregion
	
	//Só pro GERAL menos EXTRAS
	#region Método de Instanciar Móvel
	void InstanceNewObject (int id) {
       	ray = transform.parent.camera.ScreenPointToRay(new Vector2(Screen.width / 2,Screen.height / 2));
		
        if (Physics.Raycast(ray, out hit)){
			
			#region travando a posição de todos os móveis
			GameObject[] furniture = GameObject.FindGameObjectsWithTag("Movel");
			for(int i = 0; i != furniture.Length; ++i){
				furniture[i].rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			}
			#endregion
			
			GameObject newFurniture = Instantiate(items[id]) as GameObject;
								
			newFurniture.tag = "Movel";
			newFurniture.layer = LayerMask.NameToLayer("Moveis");
			
			foreach (Animation anim in newFurniture.GetComponentsInChildren<Animation>()) {
				anim.Stop();
				anim.playAutomatically = false;
			}
			
			newFurniture.AddComponent<SnapBehaviour>();
			newFurniture.AddComponent<CalculateBounds>();
			newFurniture.GetComponent<InformacoesMovel>().Initialize();
			newFurniture.GetComponent<InformacoesMovel>().Categoria = Line.CurrentLine.categories[cCategory].Name;
			newFurniture.AddComponent<Rigidbody>();
			if (newFurniture.GetComponent<InformacoesMovel>().tipoMovel == TipoMovel.FIXO) {
				newFurniture.rigidbody.constraints = RigidbodyConstraints.FreezePositionY | 
													 RigidbodyConstraints.FreezeRotation;
			} else {
				newFurniture.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			}
							
			GameObject MoveisGO = GameObject.Find("Moveis GO");
		
			if(MoveisGO == null){
				MoveisGO = new GameObject("Moveis GO");
			}
		
			#region Setando a posicao inicial do móvel se não estiver sobre um "Chao"
			GameObject[] ground = GameObject.FindGameObjectsWithTag("ChaoVazio");
			GameObject nearestAvailableGround = null;
			float shortestDistance = float.MaxValue;
			float distance;
			foreach(GameObject groundPiece in ground){
				distance = Vector3.Distance(groundPiece.transform.position,hit.point);
				if(distance < shortestDistance){
					shortestDistance = distance;
					nearestAvailableGround = groundPiece;
				}
			}
		
			print("nearestAvailableGround.transform.position: " + nearestAvailableGround.transform.position);
			Vector3 newPosition = nearestAvailableGround.transform.position;
			newFurniture.transform.position = newPosition;
			#endregion
			
			#region colocar objeto virado para a câmera
			float yRotation  = GameObject.FindGameObjectWithTag("Player").transform.rotation.eulerAngles.y;
			
			Debug.Log("yRotation: " + yRotation);
			
			if(yRotation < 55 || yRotation > 325) {
				newFurniture.transform.eulerAngles = new Vector3(0,180,0);
			}
			else if(yRotation < 145 && yRotation > 55) {
				newFurniture.transform.eulerAngles = new Vector3(0,270,0);
			}
			else if(yRotation < 235 && yRotation > 145) {
				newFurniture.transform.eulerAngles = new Vector3(0,0,0);
			}
			else if(yRotation < 325 && yRotation > 235) {
				newFurniture.transform.eulerAngles = new Vector3(0,90,0);
			} else { Debug.LogError(" Something gone wrong! ");}
			#endregion
			
			newFurniture.transform.parent = MoveisGO.transform;
		}
	}
	
	//Só pro EXTRAS
	void InstanceNewObject (GameObject gameObject) {
       	ray = transform.parent.camera.ScreenPointToRay(new Vector2(Screen.width / 2,Screen.height / 2));
		
        if (Physics.Raycast(ray, out hit)){
			
			#region travando a posição de todos os móveis
			GameObject[] furniture = GameObject.FindGameObjectsWithTag("Movel");
			for(int i = 0; i != furniture.Length; ++i){
				furniture[i].rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			}
			#endregion
			
			GameObject newFurniture = Instantiate(gameObject) as GameObject;
								
			newFurniture.tag = "Movel";
			newFurniture.layer = LayerMask.NameToLayer("Moveis");
			
			foreach (Animation anim in newFurniture.GetComponentsInChildren<Animation>()) {
				anim.Stop();
				anim.playAutomatically = false;
			}
			
			newFurniture.AddComponent<SnapBehaviour>();
			newFurniture.AddComponent<CalculateBounds>();
			newFurniture.GetComponent<InformacoesMovel>().Initialize();
			newFurniture.GetComponent<InformacoesMovel>().Categoria = "";
			newFurniture.AddComponent<Rigidbody>();
			
			if (newFurniture.GetComponent<InformacoesMovel>().tipoMovel == TipoMovel.FIXO) {
				newFurniture.rigidbody.constraints = RigidbodyConstraints.FreezePositionY | 
													 RigidbodyConstraints.FreezeRotation;
			} else {
				newFurniture.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
			}
							
			GameObject MoveisGO = GameObject.Find("Moveis GO");
		
			if(MoveisGO == null){
				MoveisGO = new GameObject("Moveis GO");
			}
		
			#region Setando a posicao inicial do móvel se não estiver sobre um "Chao"
			GameObject[] ground = GameObject.FindGameObjectsWithTag("ChaoVazio");
			GameObject nearestAvailableGround = null;
			float shortestDistance = float.MaxValue;
			float distance;
			foreach(GameObject groundPiece in ground){
				distance = Vector3.Distance(groundPiece.transform.position,hit.point);
				if(distance < shortestDistance){
					shortestDistance = distance;
					nearestAvailableGround = groundPiece;
				}
			}
		
			print("nearestAvailableGround.transform.position: " + nearestAvailableGround.transform.position);
			Vector3 newPosition = nearestAvailableGround.transform.position;
			newFurniture.transform.position = newPosition;
			#endregion
			
			#region colocar objeto virado para a câmera
			float yRotation  = GameObject.FindGameObjectWithTag("Player").transform.rotation.eulerAngles.y;
			
			Debug.Log("yRotation: " + yRotation);
			
			if(yRotation < 55 || yRotation > 325) {
				newFurniture.transform.eulerAngles = new Vector3(0,180,0);
			}
			else if(yRotation < 145 && yRotation > 55) {
				newFurniture.transform.eulerAngles = new Vector3(0,270,0);
			}
			else if(yRotation < 235 && yRotation > 145) {
				newFurniture.transform.eulerAngles = new Vector3(0,0,0);
			}
			else if(yRotation < 325 && yRotation > 235) {
				newFurniture.transform.eulerAngles = new Vector3(0,90,0);
			} else { Debug.LogError(" Something gone wrong! ");}
			#endregion
			
			newFurniture.transform.parent = MoveisGO.transform;
		}
	}
	#endregion

	#region GuiBase implementation
	public Rect[] GetWindows()
	{
		return new Rect[2]{wndAccordOption,wndAccordOption};
	}
	#endregion
}
