using UnityEngine;
using System.Collections;

public class InfoController : MonoBehaviour {
	
	// Data furniture selected in camera
	private InformacoesMovel furnitureData;
	
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
	
	private bool isSemTampo, isComTampo, isCooktop, isPia;
	bool wasInitialized;
	bool isOpen;
	// Use this for initialization
	void Awake ()
	{
		Close ();
	}
	
	void OnEnabled ()
	{
		if (!wasInitialized) {
			Invoke ("ResolveCheckBoxColors", 1f);
			wasInitialized = true;
		}
	}
	
	void ResolveCheckBoxColors ()
	{
		if (GameObject.Find ("CheckBox Cores") == null) {
			Debug.LogError ("O nome do checkbox das cores deve ser \"CheckBox Cores\" renome-o por favor.");
			Debug.Break ();
		} else {
			Transform checkBoxCores = GameObject.Find ("CheckBox Cores").transform;
			Transform checkBox;
			Vector3 rootColorGuiPosition = new Vector3 (-54f, 2.4f, 0f);
			float xOffset = 37f;
			
			BrandColorEnum[] brandColors = Line.CurrentLine.colors;
			
			for (int i = 0; i != brandColors.Length; ++i) {
				
				checkBox = checkBoxCores.Find ("Check " + BrandColor.GetColorName (brandColors [i]));//.gameObject;
				if (checkBox == null) {
					Debug.LogError ("Verifique os nomes das checkbox de cores. " + 
									"\n Cor pedida e nã encontrada: " + BrandColor.GetColorName (brandColors [i]));
					Debug.Break ();
					return;
				}
				//colocando checkbox para o lado
				checkBox.transform.localPosition = rootColorGuiPosition + (Vector3.right * xOffset * i);
				checkBox.gameObject.SetActiveRecursively(true);
				
				if (i == 0) {
					checkBox.GetComponent<UICheckbox> ().startsChecked = true;
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
		panelInfo.SetActiveRecursively(true);
		GetInfo ();
		isOpen = true;

	}
	
	public void Close (){
		this.furnitureData = null;
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
	
	private void VerifyTops () {
		isSemTampo = isComTampo = isCooktop = isPia = false;
	}
	
}
