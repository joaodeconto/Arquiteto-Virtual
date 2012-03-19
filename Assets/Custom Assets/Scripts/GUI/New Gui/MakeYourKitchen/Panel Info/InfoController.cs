using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class InfoController : MonoBehaviour {
	
	// Data furniture selected in camera
	private InformacoesMovel furnitureData;
	// Item selected
	public GameObject item {get; private set;}
	
	[System.Serializable]
	public class InfoLabels {
		public string name;
		public UILabel itemName;
		public UILabel itemValue;
		
		public void SetLabels (string itemName, string itemValue) {
			this.itemName.text = itemName;
			this.itemValue.text = itemValue;
		}
	}
	
	public InfoLabels[] infoLabels;
	public GameObject panelInfo;
	public Material topMaterial;
	
	#region vars Choose Tampo Type
	private string currentTop = "";
	#endregion
	
	bool isOpen;
	// Use this for initialization
	void Awake ()
	{
		Close ();
	}
	
	void Update () {
		if (!isOpen)
			return;
		if (Input.GetKeyUp (KeyCode.Delete)) {
			Close ();
			DestroyImmediate (GameObject.FindWithTag("MovelSelecionado"));
		}
		if (Input.GetKeyUp (KeyCode.R))
		{
			GameObject.FindWithTag ("MovelSelecionado").transform.RotateAround (Vector3.up, Mathf.PI / 2);
			GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<RenderBounds> ().UpdateObj ();
		}
	}
	
	#region Panel info controller
	public void Open (InformacoesMovel furnitureData){
		this.furnitureData = furnitureData;
		panelInfo.SetActiveRecursively(true);
		isOpen = true;
		Invoke ("GetInfo", 0.001f);
		Invoke ("GetMobile", 0.001f);
		Invoke ("ResolveCheckBoxColors", 0.001f);
		Invoke ("ResolveCheckBoxTops", 0.001f);
		Invoke ("ResolveCheckBoxTextures", 0.001f);
	}
	
	void GetMobile () {
		item = GameObject.FindGameObjectWithTag("MovelSelecionado");
	}
	
	public void Close (){
		this.furnitureData = null;
		item = null;
		panelInfo.SetActiveRecursively(false);
		DrawEmptyWindow();
		isOpen = false;
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
	
		infoLabels[0].SetLabels(I18n.t("Código"), "");
		infoLabels[1].SetLabels(I18n.t("LXPXA"), "");
		infoLabels[2].SetLabels(I18n.t("Descrição"), "");
		infoLabels[3].SetLabels(I18n.t("Linha"), "");
		infoLabels[4].SetLabels(I18n.t("Categoria"), "");
	}
	#endregion
	
	private void GetInfo () {
		infoLabels[0].SetLabels(I18n.t("Código"), furnitureData.Codigo);
		infoLabels[1].SetLabels(I18n.t("LXPXA"), furnitureData.Medidas);
		infoLabels[2].SetLabels(I18n.t("Descrição"), furnitureData.Nome);
		infoLabels[3].SetLabels(I18n.t("Linha"), Line.CurrentLine.Name);
		infoLabels[4].SetLabels(I18n.t("Categoria"), furnitureData.Categoria);
	}
		
	private void ResolveCheckBoxColors ()
	{
		GameObject checkBoxCores = GameObject.Find ("CheckBox Cores");
		if (checkBoxCores == null)
		{
			Debug.LogError ("O nome do checkbox das cores deve ser \"CheckBox Cores\" renome-o por favor.");
			Debug.Break ();
		}
		else
		{
			Transform checkBox;
			Vector3 rootColorGuiPosition = new Vector3 (-54f, 2.4f, 0f);
			float xOffset = 37f;
			
			//levar as checkboxes far, far away
			foreach (Transform color in checkBoxCores.transform)
			{
				if (color.name.Equals("Label")) continue;
				color.gameObject.SetActiveRecursively (false);
			}
			
			BrandColorEnum[] brandColors = Line.CurrentLine.colors;
			
			for (int i = 0; i != brandColors.Length; ++i)
			{
				checkBox = checkBoxCores.transform.Find ("Check " + BrandColor.GetColorName (brandColors [i]));
				if (checkBox == null) {
					Debug.LogError ("Verifique os nomes das checkbox de cores. " + 
									"\n Cor pedida e não encontrada: " + BrandColor.GetColorName (brandColors [i]));
					Debug.Break ();
					return;
				}
				//colocando checkbox para o lado
				checkBox.transform.localPosition = rootColorGuiPosition + (Vector3.right * xOffset * i);
				checkBox.gameObject.SetActiveRecursively(true);
				
				if (i == 0) {
					checkBox.GetComponent<UICheckbox> ().isChecked = true;
				}				
			}
		}
	}
	
	private void ResolveCheckBoxTops () {
		//isWithoutTop = isWithTampo = isCooktop = isSink = false;
		currentTop = "";
		
		//Quando termina de abrir a janela seleciona o objeto
		string regStrings = "(sem tampo|s tampo|com tampo|c tampo|com cooktop|com cook top|com pia|para pia)";
		if (Regex.Match(item.name,regStrings).Success) {
			string nameItemRegexPrefix = Regex.Match(item.name,".*(?=sem tampo|s tampo|com tampo|c tampo|com cooktop|com cook top|com pia|para pia)", RegexOptions.IgnoreCase).Value;
			string currentTampoTypeRegexPrefix = Regex.Match(item.name,regStrings, RegexOptions.IgnoreCase).Value;
			currentTop = currentTampoTypeRegexPrefix;
			currentTampoTypeRegexPrefix.ToLower();
			
			int categoryIndex = 0;
			
			//Find index of mobile's category 
			List<Category> categories = Line.CurrentLine.categories;
			for( categoryIndex = Line.CurrentLine.categories.Count - 1; categoryIndex != -1; --categoryIndex ){
				if(categories[categoryIndex].Name.Equals(furnitureData.Categoria)){
					break;		
				}
			}
			
			if (categoryIndex == -1)
			{
				Debug.LogError ("Categoria do módulo não existe: " + furnitureData.Categoria);
				Debug.Break ();
			}
			
			GameObject checkBoxTops = GameObject.Find ("CheckBox Tops");			
			if (checkBoxTops == null) {
				Debug.LogError ("O nome do checkbox de tampos deve ser \"CheckBox Tops\" renome-o por favor.");
				Debug.Break ();
			}
			else
			{
				Transform checkBox;
				Vector3 rootTopGuiPosition = new Vector3 (-54f, 2.4f, 0f);
				float xOffset = 37f;
				
				//levar as checkboxes far, far away
				foreach (Transform tops in checkBoxTops.transform)
				{
					if (tops.name.Equals("Label")) continue;
					tops.GetComponent<UICheckbox>().isChecked = false;
					tops.gameObject.SetActiveRecursively (false);
				}
				
				//Find the "Brother" of the current mobile
				List<GameObject> furniture = Line.CurrentLine.categories[categoryIndex].Furniture;
				
				foreach(GameObject mobile in furniture){
					string nameMobileRegexPrefix = Regex.Match(mobile.name,".*(?=sem tampo|s tampo|com tampo|c tampo|com cooktop|com cook top|com pia|para pia)", RegexOptions.IgnoreCase).Value;
					if(nameItemRegexPrefix.Equals(nameMobileRegexPrefix) && 
						Regex.Match(mobile.name, regStrings, RegexOptions.IgnoreCase).Success){
						string tampoTypeRegexPrefix = Regex.Match(mobile.name,regStrings, RegexOptions.IgnoreCase).Value;
						tampoTypeRegexPrefix.ToLower();
						
						checkBox = null;
						
						//If this is true, in the GUI, show the option
						if (tampoTypeRegexPrefix.Equals("sem tampo") || tampoTypeRegexPrefix.Equals("s tampo"))
							checkBox = checkBoxTops.transform.Find ("Check1");
						if (tampoTypeRegexPrefix.Equals("com tampo") || tampoTypeRegexPrefix.Equals("c tampo"))
							checkBox = checkBoxTops.transform.Find ("Check2");
						if (tampoTypeRegexPrefix.Equals("com cooktop") || tampoTypeRegexPrefix.Equals("com cook top"))
							checkBox = checkBoxTops.transform.Find ("Check3");
						if (tampoTypeRegexPrefix.Equals("com pia") || tampoTypeRegexPrefix.Equals("para pia"))
							checkBox = checkBoxTops.transform.Find ("Check4");
						
						if (checkBox == null) {
							Debug.LogError ("WTF? Ele nao achou nenhum check dos TOPS o.o");
							Debug.Break ();
							return;
						}
						
						checkBox.gameObject.SetActiveRecursively(true);
						checkBox.GetComponent<CheckBoxTopHandler>().item = mobile;
						
						//Checks if the this "Tampo" has the same name as current "Tampo" a little while ago
						if (currentTampoTypeRegexPrefix.Equals(tampoTypeRegexPrefix)) checkBox.GetComponent<UICheckbox>().isChecked = true;
					}
				}
			}
		}
	}

	private void ResolveCheckBoxDoorSide ()
	{	
		GameObject checkBoxDoorSide = GameObject.Find ("CheckBox Door Side");
		if (checkBoxDoorSide == null)
		{
			Debug.LogError ("O nome do checkbox de troca de lado de porta deve ser \"CheckBox Door Side\" renome-o por favor.");
			Debug.Break ();
		}
		else
		{
			//levar as checkboxes far, far away
			foreach (Transform check in checkBoxDoorSide.transform)
			{
				check.gameObject.SetActiveRecursively (false);
				
				if (check.gameObject.GetComponent<UICheckbox>() != null)
					check.gameObject.GetComponent<UICheckbox>().isChecked = false;
			}
			
			if (!Regex.Match(item.name, ".*(direita|esquerda).*").Success)
			{
				//Módulo não possui lado de porta
				return;	
			}
			
			foreach (Transform check in checkBoxDoorSide.transform)
			{
				check.gameObject.SetActiveRecursively (true);
			}
			
			bool isOpenRightDoor = Regex.Match(item.name,".*direita.*").Success;
			foreach (Transform check in checkBoxDoorSide.transform)
			{
				if (check.gameObject.GetComponent<UICheckbox>() == null)
					continue;
		
				switch (check.gameObject.GetComponent<CheckBoxDoorSideHandler>().doorSideEnum)
				{
				case DoorSideEnum.LEFT:
					check.gameObject.GetComponent<UICheckbox>().isChecked = !isOpenRightDoor;
					break;
				case DoorSideEnum.RIGHT:
					check.gameObject.GetComponent<UICheckbox>().isChecked = isOpenRightDoor;
					break;
				}
			}
		}
	}
	
	private void ResolveCheckBoxTextures () {
		GameObject checkBoxTextures = GameObject.Find ("CheckBox Tops Textures");
		
		
		if (checkBoxTextures == null) {
			Debug.LogError ("O nome do checkbox de tampos deve ser \"CheckBox Tops Textures\" renome-o por favor.");
			Debug.Break ();
		}
		else
		{
			if( currentTop.Equals("com tampo") || currentTop.Equals("c tampo") || 
				currentTop.Equals("com cooktop") || currentTop.Equals("com cook top"))
			{
				foreach (Transform textures in checkBoxTextures.transform)
				{
					if (textures.name.Equals("Label")) continue;
					textures.gameObject.SetActiveRecursively (true);
					
					if (topMaterial.mainTexture == textures.GetComponent<CheckBoxTextureHandler>().texture)
						textures.GetComponent<UICheckbox>().isChecked = true;
				}
			}
			else 
			{
				foreach (Transform textures in checkBoxTextures.transform)
				{
					if (textures.name.Equals("Label")) continue;
					textures.GetComponent<UICheckbox>().isChecked = false;
					textures.gameObject.SetActiveRecursively (false);
				}
			}
		}
	}
	
	private void ReplaceCheckBoxes ()
	{
		GameObject checkBoxCores = GameObject.Find ("CheckBox Cores");
		if (checkBoxCores == null)
		{
			Debug.Break ();
			return;
		}
		
		Transform checksTransform = checkBoxCores.transform.parent;
		
		int yOffset = -50;
		Vector3 rootPosition = new Vector3(0,-50, 0);
		int activeCheckGroups = 0;
		foreach (Transform checkGroup in checksTransform)
		{
			if (checkGroup.gameObject.active == true)
			{
				checkGroup.position = rootPosition + Vector3.up * yOffset * activeCheckGroups++;
			}
		}
		
		//Arrumando scala do tween do background do menu
		Transform background = checksTransform.parent.FindChild ("_BGMobiles");
		
		if (background == null)
		{
			Debug.LogError ("O nome do background do menu de opções está com o nome errado! Nome esperado: " + "_BGMobiles");
			Debug.Break ();
			return;
		}
		
		Debug.Log ("Para fazer o resize do background do menu opções funcionar deve-se se ter um iTweenMotion com o nome de \"descer\"");
		
		yOffset = 60;
		Vector3 backgroundScaleRoot = new Vector3(162, 100, 0);
		foreach (iTweenMotion motion in background.gameObject.GetComponents<iTweenMotion>())
		{
			if (motion.Name == "descer")
			{
				motion.to = backgroundScaleRoot + Vector3.up * yOffset * activeCheckGroups;
			}
		}
	}

}