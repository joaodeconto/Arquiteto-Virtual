using UnityEngine;
using System.Collections.Generic;

public class Category {
	
	#region array singleton
	//Essa classe serve somente para guardar os valores de categorias
	static public List<Category> Categories { get; private set; }
	
	static public void Initialize(List<Category> categories){
		WasInitialized = true;
	}
	
	static public bool WasInitialized { get; private set; }
	static public Category Get(int index){
		if(index > -1 && index < Categories.Count)
			return Categories[index];
		else {
			Debug.LogError("Indice selecionado de categorias nao existe: " + index);
			return null;
		}
	}
	static public bool Contains(string name){
		
		foreach(Category category in Categories){
			if(category.Name.Equals(name)){
				return true;
			}
		}
		
		return false;
	}
	#endregion
	
	public string Name { get; private set; }
	public Texture2D Image { get; private set; }
	public Texture2D SelectedImage { get; private set; }
	public List<GameObject> Furniture { get; private set; }
	public void SetImages(Texture2D image, Texture2D selectedImage){
		Image = image;
		SelectedImage = selectedImage;
	}
	public Category(string name, List<GameObject> furniture, Texture2D image){
		Name  	 = name;
		Image	 = image;
		Furniture= furniture;
	}
			
}
