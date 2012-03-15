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
	
	// Use this for initialization
	void Start () {
		Close ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Panel info controller
	public void Open (InformacoesMovel furnitureData){
		this.furnitureData = furnitureData;
		panelInfo.SetActiveRecursively(true);
		GetInfo ();
	}
	
	public void Close (){
		this.furnitureData = null;
		//panelInfo.SetActiveRecursively(false);
		DrawEmptyWindow();
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
