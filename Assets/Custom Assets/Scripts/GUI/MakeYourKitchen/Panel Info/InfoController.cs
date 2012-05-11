using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class InfoController : MonoBehaviour {
	
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
	
	// Item selected
	public GameObject item {get; private set;}
	
	public InfoLabels[] infoLabels;
	public GameObject panelInfo;
	public GameObject panelMobile;
	public Material topMaterial;
	
	public GameObject checkBoxCores;
	public GameObject checkBoxTops;
	public GameObject checkBoxDoorSide;
	public GameObject checkBoxTextures;
	
	#region vars Choose Tampo Type
	private string currentTop = "";
	#endregion
	
	// Data furniture selected in camera
	private InformacoesMovel furnitureData;
	public int selectedColorIndex {get; set;}
		
	private bool isOpen;
	
	private GameObject mainCamera;
	private bool panelChange = false;
	
	//Painter to Extras
	private Painter painter;
	
	void Awake ()
	{
		mainCamera = GameObject.FindWithTag("MainCamera");
		
		if (checkBoxCores == null)
		{
			Debug.LogError ("A referencia do checkbox de cores deve ser colocada neste script.");
			Debug.Break ();
			return;
		}
		
		if (checkBoxTops == null)
		{
			Debug.LogError ("A referencia do checkbox de tops deve ser colocada neste script.");
			Debug.Break ();
			return;
		}
		
		if (checkBoxDoorSide == null)
		{
			Debug.LogError ("A referencia do checkbox de troca de lado de porta deve ser colocada neste script.");
			Debug.Break ();
			return;
		}
			
		if (checkBoxTextures == null) {
			Debug.LogError ("A referencia do checkbox de textura de tampo deve ser colocada neste script.");
			Debug.Break ();
			return;
		}
		
		if (panelInfo == null) {
			Debug.LogError ("A referencia do painel info deve ser colocada neste script: " + this.ToString());
			Debug.Break ();
			return;
		}
		
		painter = mainCamera.GetComponentInChildren<Painter>();
		
		panelInfo.SetActiveRecursively(true);
		TweenPlayerButton btn = panelInfo.GetComponentInChildren<TweenPlayerButton>();
		btn.SendMessage("OnClick");
		btn.SendMessage("OnClick");
		Close();
		panelInfo.SetActiveRecursively(false);
		
		panelMobile.SetActiveRecursively(false);
		
		selectedColorIndex = 0;
		print ("24f p/ sec.: " + (1f / 24f));
	}
	
	void Update () {
		if (!isOpen && item == null)
			return;
		
		if (painter.render != null) {
			Close();
		}
		
		if (item.GetComponent<SnapBehaviour>().wasDragged) {
			if (panelChange) {
				panelChange = false;
				panelMobile.SetActiveRecursively(false);
				CancelInvoke("UpdatePainelMobile");
			}
		} else {
			if (!panelChange) {
				panelChange = true;
				panelMobile.SetActiveRecursively(true);
				InvokeRepeating("UpdatePainelMobile", 0.1f, 0.1f);
			}
		}
		
		if (Input.GetKeyUp (KeyCode.Delete))
		{
			DeleteObject ();
		}
		
		if (Input.GetKeyUp (KeyCode.R))
		{
			RotateObject ();
		}
		
		if (Input.GetKeyUp (KeyCode.F))
		{
			FocusObject ();
		}
	}
	
	void UpdatePainelMobile () {
		//Vector3 positionReal = new Vector3(item.transform.position.x, item.collider.bounds.center.y, item.transform.position.z);
		if (item == null) {
			return;
		}
		Vector3 panelMobilePosition = mainCamera.camera.WorldToScreenPoint(item.collider.bounds.center);
		panelMobilePosition.x = panelMobilePosition.x - (Screen.width/2);
		panelMobilePosition.y = panelMobilePosition.y - (Screen.height/2);
		panelMobilePosition.z = 0;
		panelMobile.transform.localPosition = panelMobilePosition;
	}
	
	#region Flying Buttons
	public void DeleteObject () {
		DestroyImmediate (item);
		Close ();
	}
	
	public void RotateObject () {
		item.transform.RotateAround (Vector3.up, Mathf.PI / 2);
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<RenderBounds> ().UpdateObj ();
	}
	
	public void FocusObject () {
		GameObject mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		
		if (!mainCamera.GetComponent<CameraController>().freeCamera.CanMoveCamera) return;
		
		Vector3 focusItemPosition = item.collider.bounds.center;
		Vector3 focusItemRotation = item.transform.localEulerAngles;
		
		if (item.collider.bounds.size.x > item.collider.bounds.size.y &&
			item.collider.bounds.size.x > item.collider.bounds.size.z) {
			focusItemPosition += (item.transform.forward * item.collider.bounds.size.x);
		} else if (item.collider.bounds.size.y > item.collider.bounds.size.z) {
			focusItemPosition += (item.transform.forward * item.collider.bounds.size.y);
		} else {
			focusItemPosition += (item.transform.forward * item.collider.bounds.size.z);
		}
		
		focusItemRotation += new Vector3(0, 180, 0);
		
		mainCamera.GetComponent<CameraController>().freeCamera.FreezeCamera ();
		
		iTween.MoveTo(mainCamera, iTween.Hash(	iT.MoveTo.position, focusItemPosition, 
	                                            iT.MoveTo.time, 2f,
												iT.MoveTo.oncomplete, "ReleaseCamera",
												iT.MoveTo.oncompletetarget, this.gameObject));
	
		iTween.RotateTo(mainCamera, iTween.Hash(iT.RotateTo.rotation, focusItemRotation,
	                                            iT.RotateTo.time, 2f));
	}
	#endregion
	
	void ReleaseCamera ()
	{
//		Debug.LogWarning ("Was called ReleaseCamera");
		GameObject mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		mainCamera.GetComponent<CameraController>().freeCamera.FreeCamera ();
	}

	
	#region Panel info controller
	public void Open (InformacoesMovel furnitureData) {
		
		panelInfo.SetActiveRecursively(true);
		
		isOpen = true;
		
		SetInfo(furnitureData);
		item = GameObject.FindGameObjectWithTag("MovelSelecionado");
		
		panelMobile.SetActiveRecursively(true);
		UpdatePainelMobile ();
		
		ResolveCheckBoxColors();
		ResolveCheckBoxTops();
		ResolveCheckBoxDoorSide();
		ResolveCheckBoxTextures();
		ReplaceCheckBoxes();			
	}
	
	public void UpdateInfo(InformacoesMovel furnitureData) {
		SetInfo(furnitureData);
		item = furnitureData.gameObject;
		UpdatePainelMobile ();
		ResolveCheckBoxColors();
		ResolveCheckBoxTops();
		ResolveCheckBoxDoorSide();
		ResolveCheckBoxTextures();
		TweenPlayerButton btn = panelInfo.GetComponentInChildren<TweenPlayerButton>();
		btn.SendMessage("OnClick");
		ReplaceCheckBoxes();			
		btn.SendMessage("OnClick");
	}
	
	public void Close (){
		
		TweenPlayerButton btn = panelInfo.GetComponentInChildren<TweenPlayerButton>();
		if (btn == null)
		{
			return;
		}
		
		if (btn.IsActive)
		{
			btn.SendMessage("OnClick");
		}
		else
		{
			btn.SendMessage("OnClick");
			btn.SendMessage("OnClick");
		}
		
		NullObject ();
	}
	
	public void NullObject (){
		this.furnitureData = null;
		item = null;
		DrawEmptyWindow();
		isOpen = false;
	}
	
	private void HideInfoController ()
	{
		if (!isOpen) {
			panelInfo.SetActiveRecursively(false);
			panelMobile.SetActiveRecursively(false);
			UITooltip.Close();
		}
	}
	#endregion
	
	#region get/set furniture data
	public void UpdateItem (GameObject item) {
		this.item = item;
	}
	
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
	
	private void SetInfo (InformacoesMovel furnitureData) {
		this.furnitureData = furnitureData;
		infoLabels[0].SetLabels(I18n.t("Código"), furnitureData.Codigo);
		infoLabels[1].SetLabels(I18n.t("LXPXA"), furnitureData.Medidas);
		infoLabels[2].SetLabels(I18n.t("Descrição"), furnitureData.Nome);
		infoLabels[3].SetLabels(I18n.t("Linha"), Line.CurrentLine.Name);
		infoLabels[4].SetLabels(I18n.t("Categoria"), furnitureData.Categoria);
	}
	#endregion
		
	private void ResolveCheckBoxColors ()
	{
		Transform checkBox;
		Vector3 rootColorGuiPosition = new Vector3 (-54f, 2.4f, 0f);
		float xOffset = 37f;
		
		//levar as checkboxes far, far away
		foreach (Transform check in checkBoxCores.transform)
		{
			check.gameObject.SetActiveRecursively (false);
			
			if (check.gameObject.GetComponent<UICheckbox>() != null)
				check.gameObject.GetComponent<UICheckbox>().isChecked = false;
		}
		
		BrandColorEnum[] brandColors = Line.CurrentLine.colors;
		
		for (int i = 0; i != brandColors.Length; ++i)
		{
			checkBox = checkBoxCores.transform.Find ("Check " + BrandColor.GetColorName (brandColors [i]));
			if (checkBox == null)
			{
				Debug.LogError ("Verifique os nomes das checkbox de cores. " + 
								"\n Cor pedida e não encontrada: " + BrandColor.GetColorName (brandColors [i]));
				Debug.Break ();
				return;
			}
//			//colocando checkbox para o lado
//			checkBox.transform.localPosition = rootColorGuiPosition + (Vector3.right * xOffset * i);
			checkBox.gameObject.SetActiveRecursively(true);

			if (i == selectedColorIndex) {
				checkBox.GetComponent<UICheckbox> ().isChecked = true;
			}				
		}
		
		//Reabilitando label
		checkBoxCores.transform.Find("Label").gameObject.active = true;
	}
	
	private void ResolveCheckBoxTops () {
		currentTop = "";
		
		//Quando termina de abrir a janela seleciona o objeto
		string regStrings = "(sem tampo|s tampo|com tampo|c tampo|com cooktop|com cook top|com pia|para pia)";
		if (!Regex.Match(item.name,regStrings).Success)
		{
			//Módulo não possui tampo
			checkBoxTops.SetActiveRecursively(false);
			return;
		}
		
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
		
		Transform checkBox;
		Vector3 rootTopGuiPosition = new Vector3 (-54f, 2.4f, 0f);
		float xOffset = 37f;
		
		//levar as checkboxes far, far away
		foreach (Transform check in checkBoxTops.transform)
		{
			check.gameObject.SetActiveRecursively (false);
			
			if (check.gameObject.GetComponent<UICheckbox>() != null)
				check.gameObject.GetComponent<UICheckbox>().isChecked = false;
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
				if (currentTampoTypeRegexPrefix.Equals(tampoTypeRegexPrefix)) {
					print("currentTampoTypeRegexPrefix: " + currentTampoTypeRegexPrefix + " - tampoTypeRegexPrefix: " + tampoTypeRegexPrefix);
					checkBox.GetComponent<UICheckbox>().isChecked = true;
				}
			}
		}

		//Religar label do tops
		checkBoxTops.transform.Find("Label").gameObject.active = true;
	}

	private void ResolveCheckBoxDoorSide ()
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
			checkBoxDoorSide.SetActiveRecursively(false);
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
	
	private void ResolveCheckBoxTextures () {
					
		checkBoxTextures.SetActiveRecursively(false);
		
		if( currentTop.Equals("com tampo") || currentTop.Equals("c tampo") || 
			currentTop.Equals("com cooktop") || currentTop.Equals("com cook top"))
		{
			checkBoxTextures.active = true;
			
			foreach (Transform check in checkBoxTextures.transform)
			{
				check.gameObject.SetActiveRecursively (true);
				
				if (check.gameObject.GetComponent<UICheckbox>() != null)
				{
					check.gameObject.GetComponent<UICheckbox>().isChecked = false;
				
					if (topMaterial.mainTexture == check.GetComponent<CheckBoxTextureHandler>().texture)
						check.GetComponent<UICheckbox>().isChecked = true;
				}
			}
		}
	}
	
	private void ReplaceCheckBoxes ()
	{
//		Transform checksTransform = checkBoxCores.transform.parent;
//		
//		int yOffset = -60;
//		Vector3 rootPosition = new Vector3(0,-50, 0);
//		int activeCheckGroups = 0;
//		foreach (Transform checkGroup in checksTransform)
//		{
//			if (checkGroup.gameObject.active == true)
//			{
//				checkGroup.localPosition = rootPosition + Vector3.up * yOffset * activeCheckGroups++;
//			}
//		}
//		
//		//Arrumando scala do tween do background do menu
//		Transform background = checksTransform.parent.FindChild ("_BGMobiles");
//		
//		if (background == null)
//		{
//			Debug.LogError ("O nome do background do menu de opções está com o nome errado! Nome esperado: " + "_BGMobiles");
//			Debug.Break ();
//			return;
//		}
//		
////		Debug.Log ("Para fazer o resize do background do menu opções funcionar deve-se se ter um iTweenMotion com o nome de \"descer\"");
//		
//		Vector3 backgroundScaleRoot = new Vector3(162, 70, 0);
//		foreach (iTweenMotion motion in background.gameObject.GetComponents<iTweenMotion>())
//		{
//			if (motion.Name == "descer")
//			{
////				motion.to = backgroundScaleRoot + Vector3.up * -yOffset * activeCheckGroups;
//			}
//		}
	}
	
	/*public void UpdateSetting () {
		if (checkBoxTextures.active) {
			if (check.gameObject.GetComponent<UICheckbox>() != null) {
				check.gameObject.GetComponent<UICheckbox>().isChecked = false;
				if (topMaterial.mainTexture == check.GetComponent<CheckBoxTextureHandler>().texture)
					check.GetComponent<UICheckbox>().isChecked = true;
			}
		}
		
		if (checkBoxTops.active) {
			if (Regex.Match(currentTop, "(sem tampo|s tampo|com tampo|c tampo|com cooktop|com cook top|com pia|para pia)", RegexOptions.IgnoreCase)) {
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
			
			Transform checkBox;
			Vector3 rootTopGuiPosition = new Vector3 (-54f, 2.4f, 0f);
			float xOffset = 37f;
			
			//levar as checkboxes far, far away
			foreach (Transform check in checkBoxTops.transform)
			{
				check.gameObject.SetActiveRecursively (false);
				
				if (check.gameObject.GetComponent<UICheckbox>() != null)
					check.gameObject.GetComponent<UICheckbox>().isChecked = false;
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
					if (currentTop.Equals("sem tampo") || currentTop.Equals("s tampo"))
						checkBox = checkBoxTops.transform.Find ("Check1");
					if (currentTop.Equals("com tampo") || currentTop.Equals("c tampo"))
						checkBox = checkBoxTops.transform.Find ("Check2");
					if (currentTop.Equals("com cooktop") || currentTop.Equals("com cook top"))
						checkBox = checkBoxTops.transform.Find ("Check3");
					if (currentTop.Equals("com pia") || currentTop.Equals("para pia"))
						checkBox = checkBoxTops.transform.Find ("Check4");
					
					if (checkBox == null) {
						Debug.LogError ("WTF? Ele nao achou nenhum check dos TOPS o.o");
						Debug.Break ();
						return;
					}
					
					checkBox.gameObject.SetActiveRecursively(true);
					checkBox.GetComponent<CheckBoxTopHandler>().item = mobile;
					
					//Checks if the this "Tampo" has the same name as current "Tampo" a little while ago
					if (currentTop.Equals(tampoTypeRegexPrefix))
						checkBox.GetComponent<UICheckbox>().isChecked = true;
				}
			}
			
			//Religar label do tops
			checkBoxTops.transform.Find("Label").gameObject.active = true;
		}
	}*/
}