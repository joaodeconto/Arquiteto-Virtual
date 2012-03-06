using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions; 
using System;

public enum Portas { 
	FECHADAS,
	ABERTAS,
}

public enum Top { 
	NENHUM, 
	AMBOS, 
	TAMPOePIA, 
	TAMPOeCOOKTOP, 
	PIAeCOOKTOP, 
	TAMPO, 
	PIA, 
	COOKTOP,
}

public enum TipoMovel {
	FIXO,
	MOVEL
}

public class InformacoesMovel : MonoBehaviour {
	
	public string[] 	Names { get; set; }
	public string 		Nome { get { return Names[I18n.GetCurrentLanguageIndex()]; } }
	
	/* Setted In Visual */
	public string 		Medidas;
	public string 		NomeP;
	public string 		NomeI;
	public string 		NomeE;	
	public string 		Codigo;
	public Texture2D 	Imagem;
	public Top			top;
	public TipoMovel	tipoMovel;
	/* End Setted In Visual  */ 
	
	public string		Categoria { get; set; }
	
	public string		Altura { get; private set; }
	public string		Largura { get; private set; }
	public string		Comprimento { get; private set; }
	
	internal Portas		portas;
	internal uint DrawersAndGlassDoorColorIndex;
	internal uint StructureAndCommonDoorColorIndex;
	
	private bool wasInitialized;
	
	public void Initialize(){
	
		if(wasInitialized){
			return;
		}
		
		wasInitialized = true;
		
		Names = new string[I18n.GetAvailableLanguages().Length];
		Names[0] = NomeP;
		Names[1] = NomeI;
		Names[2] = NomeE;
		
		string[] splitedStr = Medidas.Split('x');
		Largura 	= splitedStr[0];
		Altura 		= splitedStr[1];
		Comprimento = splitedStr[2];
		
		portas = Portas.FECHADAS;
		
		DrawersAndGlassDoorColorIndex 	 = Line.CurrentLine.GlobalDetailColorIndex;
		StructureAndCommonDoorColorIndex = Line.CurrentLine.GlobalBaseColorIndex;
		
		ReColorize();
	}
	
	public void ChangeTexture (Texture2D texture, string type){
		
		Renderer[] renders = GetComponentsInChildren<Renderer>();
		
		Regex regexType = new Regex(@""+type+".+", RegexOptions.IgnoreCase);
		
		foreach (Renderer r in renders) {
			if(regexType.Match(r.material.name).Success) {
				r.material.mainTexture = texture;
			}
		}
	}
	
	public void ChangeTop (GameObject top){
		
		Regex regexType = new Regex(@"posicaoTop.+", RegexOptions.IgnoreCase);

		foreach(Transform child in transform){
			if(regexType.Match(child.name).Success){
				GameObject newTop = Instantiate(top, child.position, child.rotation) as GameObject;
				newTop.transform.parent = child.transform;
			}
		}
	}
	
	public void ToogleDoorSide (){
		
		string nameRegexPrefix = Regex.Match(this.name,".*(?=esquerda|direita)").Value;
		string patternToFind = "";
		
		if(Regex.Match(this.name,"direita").Success){
//			Debug.LogError("Turn Left");
			patternToFind = nameRegexPrefix + "esquerda.*";
		} else if(Regex.Match(this.name,"esquerda").Success){
//			Debug.LogError("Turn Right");
			patternToFind = nameRegexPrefix + "direita.*";
		} else {
			Debug.LogError("Whata?!");
			return;
		}
		
//		Debug.LogError("patternToFind: " + patternToFind);

		Transform savedTransform = this.transform;
		int categoryIndex;
				
		//Find index of mobile's category 
		List<Category> categories = Line.CurrentLine.categories;
		for( categoryIndex = Line.CurrentLine.categories.Count - 1; categoryIndex != -1; --categoryIndex ){
			if(categories[categoryIndex].Name.Equals(this.Categoria)){
				break;		
			}
		}
		
		//Find the "Brother" of the current mobile
		List<GameObject> furniture  = categories[categoryIndex].Furniture;
		foreach(GameObject mobile in furniture){
			if(Regex.Match(mobile.name, patternToFind).Success){
				Clone(mobile,this.GetComponent<InformacoesMovel>(), "Movel");
				Destroy(this.gameObject);
//				Debug.LogError("Chegou");
				break;
			}
		}
	}
	
	public void Colorize(Color color){
		
		Renderer[] renders = this.GetComponentsInChildren<Renderer>();
		
		Regex regexVidro = new Regex(@".+vidro.+", RegexOptions.IgnoreCase);
		Regex regexGaveta = new Regex(@"gaveta.+", RegexOptions.IgnoreCase);
		Regex regexFruteira = new Regex(@".+fruteira.+", RegexOptions.IgnoreCase);
		
		Regex regexEstrutura = new Regex(@"estrutura.+", RegexOptions.IgnoreCase);
		Regex regexPorta = new Regex(@"portas [PMG]", RegexOptions.IgnoreCase);
		
		foreach (Renderer r in renders) 
		{
			foreach (Material rm in r.materials) 
			{
				if( regexVidro.Match(rm.name).Success ||
					regexGaveta.Match(rm.name).Success ||
					regexFruteira.Match(rm.name).Success ||
					regexEstrutura.Match(rm.name).Success ||
					regexPorta.Match(rm.name).Success)
				{
					rm.color = color;
				}
			}
		}
	}
	
	public void Colorize(uint drawersAndGlassDoorColorIndex, uint structureAndCommonDoorColorIndex){
		
		this.DrawersAndGlassDoorColorIndex 	  = drawersAndGlassDoorColorIndex;
		this.StructureAndCommonDoorColorIndex = structureAndCommonDoorColorIndex;
		
		ReColorize();
	}
	
	public void ChangeBaseColor(uint selectedColor ){
		
		this.StructureAndCommonDoorColorIndex = selectedColor;
		
		ReColorize();
	}
	
	public void ChangeDetailColor(uint selectedColor){
		
		this.DrawersAndGlassDoorColorIndex = selectedColor;
		
		ReColorize();
	}
	
	public void ReColorize(){
		
		Renderer[] renders = this.GetComponentsInChildren<Renderer>();
		
		Regex regexVidro  = new Regex(@".+vidro.+",RegexOptions.IgnoreCase);
		Regex regexGaveta = new Regex(@"gaveta.+", RegexOptions.IgnoreCase);
		Regex regexFruteira = new Regex(@".+fruteira.+", RegexOptions.IgnoreCase);

		Regex regexEstrutura  = new Regex(@"estrutura.+", RegexOptions.IgnoreCase);
		Regex regexPorta	  = new Regex(@"portas [PMG]", RegexOptions.IgnoreCase);

		foreach (Renderer r in renders) {

			foreach (Material rm in r.materials) {
				if( regexVidro.Match(rm.name).Success ||
				    regexGaveta.Match(rm.name).Success ||
					regexFruteira.Match(rm.name).Success){

					rm.color = Line.CurrentLine.colors[DrawersAndGlassDoorColorIndex];

				} else if(regexEstrutura.Match(rm.name).Success ||
							  regexPorta.Match(rm.name).Success){

					rm.color = Line.CurrentLine.colors[StructureAndCommonDoorColorIndex];
				}
			}
		}
	}
	
	private void Clone(GameObject cloned, InformacoesMovel info, string tag)		{
	
		GameObject newFurniture = Instantiate(cloned,this.transform.position,this.transform.rotation) as GameObject;
							
		newFurniture.tag = tag;
		newFurniture.layer = LayerMask.NameToLayer("Moveis");
		
		foreach (Animation anim in newFurniture.GetComponentsInChildren<Animation>()) {
			anim.Stop();
			anim.playAutomatically = false;
		}
		
		newFurniture.AddComponent<SnapBehaviour>();
		newFurniture.AddComponent<CalculateBounds>();
		newFurniture.GetComponent<InformacoesMovel>().Initialize();
		newFurniture.GetComponent<InformacoesMovel>().CloneInfo(info);
		if (tipoMovel == TipoMovel.FIXO) {
			newFurniture.rigidbody.constraints = RigidbodyConstraints.FreezePositionY | 
												 RigidbodyConstraints.FreezeRotation;
		}
		else {
			newFurniture.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		}
		newFurniture.transform.parent = GameObject.Find("Moveis GO").transform;
	}
	
	public void CloneInfo(InformacoesMovel info){
	
		Names = new string[info.Names.Length];
		Names[0] = info.Names[0];
		Names[1] = info.Names[1];
		Names[2] = info.Names[2];
		
		string[] splitedStr = info.Medidas.Split('x');
		Largura 	= splitedStr[0];
		Altura 		= splitedStr[1];
		Comprimento = splitedStr[2];
		
		portas = info.portas;
		
		DrawersAndGlassDoorColorIndex 	 = info.DrawersAndGlassDoorColorIndex;
		StructureAndCommonDoorColorIndex = info.StructureAndCommonDoorColorIndex;
		
		Categoria = info.Categoria;
		
		portas = Portas.FECHADAS;
	}
}
