using UnityEngine;
using System.Collections;

public class InfoController : MonoBehaviour {
	
	[System.Serializable]
	public class InfoLabels {
		public string name;
		public UILabel itemName;
		public UILabel itemValue;
		
		public InfoLabels (string itemName, string itemValue) {
			this.itemName.text = itemName;
			this.itemValue.text = itemValue;
		}
	}
	
	private InformacoesMovel furnitureData;
	
	public InfoLabels[] infoLabels;
	
	// Use this for initialization
	void Start () {
		DrawEmptyWindow ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region get/set furniture data
	public void UpdateData(InformacoesMovel furnitureData){
		this.furnitureData = furnitureData;
	}
	
	public InformacoesMovel GetData(){
		return furnitureData;		
	}
	
	private void DrawEmptyWindow (){
		infoLabels[0] = new InfoLabels(I18n.t("Código"), "");
		infoLabels[1] = new InfoLabels(I18n.t("LXPXA"), "");
		infoLabels[2] = new InfoLabels(I18n.t("Descrição"), "");
		infoLabels[3] = new InfoLabels(I18n.t("Linha"), "");
		infoLabels[4] = new InfoLabels(I18n.t("Categoria"), "");
	}
	#endregion
	
	void GetInfo () {
		infoLabels[0] = new InfoLabels(I18n.t("Código"), furnitureData.Codigo);
		infoLabels[1] = new InfoLabels(I18n.t("LXPXA"), furnitureData.Medidas);
		infoLabels[2] = new InfoLabels(I18n.t("Descrição"), furnitureData.Nome);
		infoLabels[3] = new InfoLabels(I18n.t("Linha"), Line.CurrentLine.Name);
		infoLabels[4] = new InfoLabels(I18n.t("Categoria"), furnitureData.Categoria);
	}
}
