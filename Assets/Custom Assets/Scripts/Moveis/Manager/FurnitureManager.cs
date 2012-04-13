using UnityEngine;
using System.Collections;
using System;

public class FurnitureManager : MonoBehaviour {

    private Camera mainCamera;
    internal int select = -1;
    private int lastSelect = -1;
	private GameObject newFurniture;

    [HideInInspector]
    public enum TipoDeObjeto {
        NORMAL,
        PAREDE,
    };
	
	void Start(){
		
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		
		GameObject[] chaos = GameObject.FindGameObjectsWithTag("Chao");
		for(int i = 0; i != chaos.Length; ++i){
			chaos[i].collider.enabled = true;
		}
		
		GameObject[] paredes = GameObject.FindGameObjectsWithTag("Parede");
			
		for(int i = 0; i != paredes.Length; ++i){
			paredes[i].AddComponent<Rigidbody>();
			paredes[i].rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			paredes[i].rigidbody.isKinematic = true;
			paredes[i].collider.enabled = true;
		}
	}
	
	public bool isNewFurnitureActive(){
		return select != -1;
	}

	public GameObject GetActiveNewFurniture(){
		return newFurniture;
	}
	
	public void FreeActiveNewFurniture(){
		select = -1;
		newFurniture.tag = "Movel";
	}
}
