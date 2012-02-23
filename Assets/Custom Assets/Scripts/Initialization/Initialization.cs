using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Arquiteto Virtual/Initialization")]
public class Initialization : MonoBehaviour {
	
	public GameObject[] everything;
	
	// Use this for initialization of the static classes
	void Start () {
		
		//Inicializar classe ScreenUtils
		ScreenUtils.Initialize(1024,640);
		I18n.Initialize();
	}
	
	public void LoadObjects (int id) {
		
		#region load objects
		GameObject root = everything[id];
		
		List<InformacoesMovel> listInfoMoveis = new List<InformacoesMovel>();
		
		int i = 0;
		Color[] colors;
		Texture[] colorsTextures;
		Line line;
		List<Line> lines = new List<Line>();
		List<Category> categories = new List<Category>();
		List<GameObject> furniture = new List<GameObject>();
		
		colors 			= root.GetComponent<LineColorsPrefab>().colors;		
		colorsTextures 	= root.GetComponent<LineColorsPrefab>().colorTextures;		
		
		foreach (Transform categoriesTransform in root.transform) {
			furniture = new List<GameObject>();
			foreach (Transform mobile in categoriesTransform.transform) {
				furniture.Add(mobile.gameObject);
			}
			categories.Add(new Category(categoriesTransform.name,furniture,categoriesTransform.GetComponent<MakeCategory>().ico));
		}
		
		print("categories.Count: " + categories.Count);
		lines.Add(new Line(colors,colorsTextures,categories, root.name));
		Line.Initialize(lines);
		#endregion	
	}
}
