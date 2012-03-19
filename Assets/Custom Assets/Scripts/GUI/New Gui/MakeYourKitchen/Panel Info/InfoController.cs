using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class InfoController : MonoBehaviour {
	
	// Data furniture selected in camera
	private InformacoesMovel furnitureData;
	// Item selected
	private GameObject item;
	
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
	
	#region vars Choose Tampo Type
	private string currentTop = "";
	private bool isWithoutTop, isWithTampo, isCooktop, isSink;
	#endregion
	
	bool isOpen;
	// Use this for initialization
	void Awake ()
	{
		Close ();
	}
	
	private void ResolveCheckBoxColors ()
	{
		GameObject checkBoxCores = GameObject.Find ("CheckBox Cores");
		if (checkBoxCores == null) {
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
		item = GameObject.FindGameObjectWithTag("MovelSelecionado");
		panelInfo.SetActiveRecursively(true);
		GetInfo ();
		isOpen = true;
		Invoke ("ResolveCheckBoxColors", 0.5f);
		Invoke ("ResolveCheckBoxTops", 0.5f);
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
			
			Debug.LogWarning("Line.CurrentLine.categories = " + furnitureData.Nome);
			
			//Find index of mobile's category 
			List<Category> categories = Line.CurrentLine.categories;
			for( categoryIndex = Line.CurrentLine.categories.Count - 1; categoryIndex != -1; --categoryIndex ){
				Debug.Log(categories[categoryIndex].Name + " : " + furnitureData.Categoria);
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
						
						//Checks if the this "Tampo" has the same name as current "Tampo" a little while ago
						if (currentTampoTypeRegexPrefix.Equals(tampoTypeRegexPrefix)) checkBox.GetComponent<UICheckbox>().isChecked = true;
					}
				}
			}
		}
	}
}
