using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Arquiteto Virtual/Initialization")]
public class Initialization : MonoBehaviour {
	
	public GameObject[] everything;
	public GameObject camera;
	
	// Use this for initialization of the static classes
	void Start () {
		
		//Inicializar classe ScreenUtils
		ScreenUtils.Initialize(1024,640);
		I18n.Initialize();
		I18n.ChangeLanguage(PlayerPrefs.GetInt("SelectedLanguage"));
		LoadObjects(PlayerPrefs.GetInt("SelectedKitchen"));

		/* Combine meshs for while */
		
		CombineMesh(GameObject.Find("ParentChao").transform, true);
		CombineMesh(GameObject.Find("ParedesBack").transform, true);
		CombineMesh(GameObject.Find("ParedesFront").transform, true);
		CombineMesh(GameObject.Find("ParedesLeft").transform, true);
		CombineMesh(GameObject.Find("ParedesRight").transform, true);
		CombineMesh(GameObject.Find("ParentTeto").transform, true);
		Destroy(GameObject.FindWithTag("Grid"));
		RemoveGround();
		RemoveWalls();
		RemoveRoof();

		camera.SetActiveRecursively(true);
	}
	
	public void LoadObjects (int id) {
		
		#region load objects
		GameObject root = everything[id];
		
		root.GetComponent<MakeBrand>().ChangeDoor();
		
		List<InformacoesMovel> listInfoMoveis = new List<InformacoesMovel>();
		
		int i = 0;
		BrandColorEnum[] colors;
		Texture[] colorsTextures;
		Line line;
		List<Line> lines = new List<Line>();
		List<Category> categories = new List<Category>();
		List<GameObject> furniture = new List<GameObject>();
		
		colors 			= root.GetComponent<BrandColor>().colors;		
		colorsTextures 	= root.GetComponent<LineColorsPrefab>().colorTextures;		
		
		foreach (Transform categoriesTransform in root.transform) {
			furniture = new List<GameObject>();
			foreach (Transform mobile in categoriesTransform.transform) {
				furniture.Add(mobile.gameObject);
			}
			categories.Add(new Category(categoriesTransform.name,furniture,categoriesTransform.GetComponent<MakeCategory>().imageReference));
		}
		
		print("categories.Count: " + categories.Count);
		lines.Add(new Line(colors,categories, root.name));
		Line.Initialize(lines);
		#endregion	
	}
	
	private void RemoveGround() {
		GameObject[] pisos = GameObject.FindGameObjectsWithTag ("Chao");
		if (pisos.Length > 0) {
			foreach (GameObject piso in pisos)
				Destroy (piso,1);
		}
	}

	private void RemoveRoof () {
		GameObject[] ceil = GameObject.FindGameObjectsWithTag ("Teto");
		if (ceil.Length > 0) {
			foreach (GameObject t in ceil)
				Destroy (t);
		}
	}

	private void RemoveWalls () {
		GameObject[] paredes = GameObject.FindGameObjectsWithTag ("Parede");
		if (paredes.Length > 0) {
			foreach (GameObject wall in paredes)
				Destroy (wall);
		}
		GameObject[] quinas = GameObject.FindGameObjectsWithTag ("Quina");
		if (quinas.Length > 0) {
			foreach (GameObject corner in quinas)
				Destroy (corner);
		}
	}

	private void CombineMesh (Transform target, bool createCollider) {
		MeshFilter[] meshFilters = target.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		for (int i = 0; i != meshFilters.Length; i++){
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
		}
		target.GetComponent<MeshFilter>().mesh = new Mesh();
		target.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		if (createCollider) {
			MeshCollider mc = target.gameObject.AddComponent("MeshCollider") as MeshCollider;
		}
	}
}
