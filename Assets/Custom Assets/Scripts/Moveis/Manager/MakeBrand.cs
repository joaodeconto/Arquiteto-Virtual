using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

[AddComponentMenu("Arquiteto Virtual/Moveis/Make Brand")]
public class MakeBrand : MonoBehaviour {
	public Texture2D ico; 
	public Material materialPortaP;
	public Texture2D normalMapPortaP;
	public Material materialPortaM;
	public Texture2D normalMapPortaM;
	public Material materialPortaG;
	public Texture2D normalMapPortaG;
	public Material materialPortaVidro;
	public Texture2D texturaVidro;
	
	public void ChangeDoor () {
//		Regex regexPorta = new Regex(@"Porta.+", RegexOptions.IgnoreCase);
//		Regex regexPuxador = new Regex(@"Acrilico.+", RegexOptions.IgnoreCase);
//		
//		Transform[] allChildrens = this.GetComponentsInChildren<Transform>();
//		foreach(Transform child in allChildrens){
//			if(regexPorta.Match(child.name).Success){
//				foreach (Renderer r in child.GetComponentsInChildren<Renderer>()) {
//					if (!regexPuxador.Match(r.material.name).Success) {
//						r.material = DoorMaterial;
//						if (DoorBrandTexture != null) {
//							r.material.SetTexture("_BumpMap", DoorBrandTexture);
//						}
//						else {
//							r.material.SetTexture("_BumpMap", null);
//						}
//					}
//				}
//			}
//		}
		
		if (materialPortaP != null && normalMapPortaP != null) {
			materialPortaP.SetTexture("_BumpMap", normalMapPortaP);
		}
		if (materialPortaM != null && normalMapPortaM != null) {
			materialPortaM.SetTexture("_BumpMap", normalMapPortaM);
		}
		if (materialPortaG != null && normalMapPortaG != null) {
			materialPortaG.SetTexture("_BumpMap", normalMapPortaG);
		}
		if (materialPortaVidro != null && texturaVidro != null) {
			materialPortaVidro.mainTexture = texturaVidro;
		}
	}
}
