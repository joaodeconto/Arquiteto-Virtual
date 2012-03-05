using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class GuiDescription : MonoBehaviour, GuiBase {
	
	[System.Serializable]
	public class TamposTextures {
		public GUIStyle iconButton;
		public Texture2D texture;
	}
	
	private InformacoesMovel furnitureData;
	
	#region Gui Movement
	private float lerpStep;
	private float lerpTime;
	private float cLerp;
	private int minLeftPosition;
	#endregion
	
	#region Draw Window
	private int leftBorderPosition;
	internal Rect window;
	private Rect[] btnsActions;
	private Rect[] wndItemName;
	private Rect[] wndItemValue;
	#endregion
	
	#region visual
	public Texture BgTexture;
	public GUIStyle[] actionStyles;
	public GUIStyle itemNameStyle;
	public GUIStyle itemValueStyle;
	public TamposTextures[] tamposTextures;
	public Material materialTampo;
	#endregion
	
	#region Respected Mobile Item Selected
	private GameObject item;
	private bool SelectedLeftDoor;
	#endregion
	
	#region Improviso do Tampo
	private int indexTampo = 0;
	#endregion
	
	void Awake(){
		
		lerpStep 	= 0.1F;
		lerpTime 	= 3.0F;
		cLerp 		= 0.1F;
		
		window = ScreenUtils.ScaledRect(1024 - 256 - GuiScript.BORDER_MARGIN, 
		                                			 GuiScript.BORDER_MARGIN,
		                                128, 256);
		
		leftBorderPosition = (int)ScreenUtils.ScaleWidth(1024);
		minLeftPosition    = (int)ScreenUtils.ScaleWidth(1024f - 128f);//Largura da tela - Largura da imagem
				
		wndItemName = new Rect[6];
		wndItemName[0] = ScreenUtils.ScaledRect(15,12,32,16);
		wndItemName[1] = ScreenUtils.ScaledRect(15,40,32,17);
		wndItemName[2] = ScreenUtils.ScaledRect(15,66,32,16);
		wndItemName[3] = ScreenUtils.ScaledRect(15,94,32,17);
		wndItemName[4] = ScreenUtils.ScaledRect(15,120,32,17);
		wndItemName[5] = ScreenUtils.ScaledRect(15,148,32,17);
		
		wndItemValue = new Rect[6];
		wndItemValue[0] = ScreenUtils.ScaledRect(45,12,70,16);
		wndItemValue[1] = ScreenUtils.ScaledRect(45,40,70,17);
		wndItemValue[2] = ScreenUtils.ScaledRect(45,66,70,16);
		wndItemValue[3] = ScreenUtils.ScaledRect(45,94,70,17);
		wndItemValue[4] = ScreenUtils.ScaledRect(45,120,70,17);
		wndItemValue[5] = ScreenUtils.ScaledRect(45,148,70,17);
		
		btnsActions = new Rect[5];
		btnsActions[0] = ScreenUtils.ScaledRect(16,180,32,32);
		btnsActions[1] = ScreenUtils.ScaledRect(48,203,32,32);
		btnsActions[2] = ScreenUtils.ScaledRect(80,180,32,32);
		btnsActions[3] = ScreenUtils.ScaledRect(16,212,32,32);
		btnsActions[4] = ScreenUtils.ScaledRect(80,212,32,32);
		
		Tooltip.AddDynamicTip(I18n.t("tip-rotacao-objeto"));
		Tooltip.AddDynamicTip(I18n.t("tip-focar-objeto"));
		Tooltip.AddDynamicTip(I18n.t("tip-excluir-objeto"));
	}
	
	#region animation
	public void Hide(){
		InvokeRepeating("Hiding",0.1F,lerpStep);
	}
	
	private void Hiding(){
		cLerp -= 1F / (lerpTime);
		
		if(cLerp < 0F){
			CancelInvoke("Hiding");
		}
		
		leftBorderPosition = (int)(Mathf.Lerp(Screen.width, minLeftPosition ,cLerp));
	}
	
	public void Show(){
		InvokeRepeating("Showing",0.1F,lerpStep);
	}
	
	private void Showing(){
		cLerp += 1F / (lerpTime);
		if(cLerp > 1F){
			//Quando termina de abrir a janela seleciona o objeto
			item = GameObject.FindGameObjectWithTag("MovelSelecionado");
			CancelInvoke("Showing");
		}
		
		leftBorderPosition = (int)(Mathf.Lerp(Screen.width, minLeftPosition,cLerp));
		
	}
	#endregion

	#region get/set furniture data
	public void UpdateData(InformacoesMovel furnitureData){
		this.furnitureData = furnitureData;
	}
	
	public InformacoesMovel GetData(){
		return furnitureData;		
	}
	
	private void DrawEmptyWindow (){
		
		GUI.DrawTexture(window,BgTexture);
		GUI.BeginGroup(window);
			GUI.Label(wndItemName[0],I18n.t("Lin"),itemNameStyle);
			GUI.Label(wndItemName[1],I18n.t("Cat"),itemNameStyle);
			GUI.Label(wndItemName[2],I18n.t("Ref"),itemNameStyle);
			GUI.Label(wndItemName[3],I18n.t("Lar"),itemNameStyle);
			GUI.Label(wndItemName[4],I18n.t("Alt"),itemNameStyle);
			GUI.Label(wndItemName[5],I18n.t("Pro"),itemNameStyle);
		
			GUI.Label(wndItemValue[0],"",itemValueStyle);
			GUI.Label(wndItemValue[1],"",itemValueStyle);
			GUI.Label(wndItemValue[2],"",itemValueStyle);
			GUI.Label(wndItemValue[3],"",itemValueStyle);
			GUI.Label(wndItemValue[4],"",itemValueStyle);
			GUI.Label(wndItemValue[5],"",itemValueStyle);
		GUI.EndGroup();
		
		Tooltip.DoTips();
	}

	#endregion

	#region GuiBase implementation
	public void Draw(){
		
		window.x = leftBorderPosition;
		
		if(furnitureData == null){
			DrawEmptyWindow();
			return;	
		}
		
		GUI.DrawTexture(window,BgTexture);
		GUI.BeginGroup(window); {
			GUI.Label(wndItemName[0],I18n.t("Lin"),itemNameStyle);
			GUI.Label(wndItemName[1],I18n.t("Cat"),itemNameStyle);
			GUI.Label(wndItemName[2],I18n.t("Ref"),itemNameStyle);
			GUI.Label(wndItemName[3],I18n.t("Lar"),itemNameStyle);
			GUI.Label(wndItemName[4],I18n.t("Alt"),itemNameStyle);
			GUI.Label(wndItemName[5],I18n.t("Pro"),itemNameStyle);
		
			GUI.Label(wndItemValue[0],Line.CurrentLine.Name,itemValueStyle);
			GUI.Label(wndItemValue[1],furnitureData.Categoria,itemValueStyle);
			GUI.Label(wndItemValue[2],furnitureData.Codigo,itemValueStyle);
			GUI.Label(wndItemValue[3],furnitureData.Largura,itemValueStyle);
			GUI.Label(wndItemValue[4],furnitureData.Altura,itemValueStyle);
			GUI.Label(wndItemValue[5],furnitureData.Comprimento,itemValueStyle);
	
			if(GUI.Button(btnsActions[0], new GUIContent("", I18n.t("tip-rotacao-objeto")), actionStyles[0])){
				SomClique.Play();
				Vector3 cPosition = transform.position;
				item.transform.RotateAround(Vector3.up, Mathf.PI / 2);
				//if(item.GetComponent<SnapBehaviour>().CheckCollisonOnWalls()){
					transform.position = cPosition;
			
					//}
				GameObject.FindGameObjectWithTag("Player").GetComponent<RenderBounds>().UpdateObj();
			}
			
			if(GUI.Button(btnsActions[1], new GUIContent("", I18n.t("tip-focar-objeto")), actionStyles[1])){
				SomClique.Play();
				Vector3 focusItemPosition = item.transform.position;
				Vector3 focusItemRotation = item.transform.localEulerAngles;
				focusItemPosition.x -= 2;
				focusItemPosition.y += 2;
				focusItemPosition.z -= 2;
				focusItemRotation.x = 25; focusItemRotation.y = 45;
				transform.parent.position = focusItemPosition;
				transform.parent.localEulerAngles = focusItemRotation;
//			    Vector3 focusItemPosition = item.transform.position;
//        		Vector3 focusItemRotation = item.transform.localEulerAngles;
//        		focusItemPosition += (item.transform.forward * 2);
//		        focusItemRotation += new Vector3(0, 180, 0);
//      
// 	    		iTween.MoveTo(transform.parent.gameObject, iTween.Hash(  iT.MoveTo.position, focusItemPosition, 
//  	        	                                                     iT.MoveTo.time, 2f));
//      
//		        iTween.RotateTo(transform.parent.gameObject, iTween.Hash(iT.RotateTo.rotation, focusItemRotation,
//      	                                                             iT.RotateTo.time, 2f));
			
			}
			if(GUI.Button(btnsActions[2], new GUIContent("", I18n.t("tip-excluir-objeto")), actionStyles[2])){
				SomClique.Play();
				Hide();
				DestroyImmediate(item);
			}
		
			if(furnitureData.top != Top.NENHUM){
				if(GUI.Button(btnsActions[3], "Tampo", actionStyles[3])){
					SomClique.Play();
					
					indexTampo = (indexTampo + 1) % tamposTextures.Length;
					furnitureData.ChangeTexture(tamposTextures[indexTampo].texture, "Tampos");
					materialTampo.mainTexture = tamposTextures[indexTampo].texture;
					GameObject[] furniture = GameObject.FindGameObjectsWithTag("Movel");
					if(furniture != null && furniture.Length != 0) {
						foreach(GameObject mobile in furniture) {
							mobile.GetComponent<InformacoesMovel>().ChangeTexture(tamposTextures[indexTampo].texture, "Tampos");
						}
					}
				}
			}
		
			if( furnitureData.gameObject.name.Contains("direita") ||
				furnitureData.gameObject.name.Contains("esquerda") ) {
				if(GUI.Button(btnsActions[3], "Porta", actionStyles[3])){
					SelectedLeftDoor = !SelectedLeftDoor;
					
					SomClique.Play();
					
					furnitureData.ToogleDoorSide();
				
					UpdateData(furnitureData);
				
					Hide();
				}
			}
		
			if( Regex.Match(furnitureData.gameObject.name, ".*(com tampo|c tampo)*.").Success) {
				if(GUI.Button(btnsActions[4], "Porta", actionStyles[3])){
					SomClique.Play();
					
					furnitureData.ToogleDoorSide();
				
					Hide();
				}
			}
		
		
			/*#region Conteúdo do Tampo no Accordion
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
			#endregion*/
		
			/*#region Selecionar porta esquerda ou direita
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
			#endregion*/
		}
		GUI.EndGroup();
		Tooltip.DoTips();
	}
	
	public Rect[] GetWindows(){
		//Todo o resto da Gui está dentro deste window
		return new Rect[1]{window};
	}
	#endregion
		
	void changeItemColor(uint colorIndex){
		if(colorIndex == Line.CurrentLine.colors.Length - 1)
			item.GetComponent<InformacoesMovel>().Colorize(colorIndex,0);
		else 
			item.GetComponent<InformacoesMovel>().Colorize(colorIndex,colorIndex + 1);
		
	}
}
