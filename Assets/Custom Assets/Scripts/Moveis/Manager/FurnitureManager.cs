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
	
//	private bool isCreating = false;
//	public void Create(int index){
//		return;
//		select = index;
//		isCreating = true;
//		
//        RaycastHit hit;
//        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//		
//        if (Physics.Raycast(ray, out hit)){
//            if (GetComponent<FurnitureStore>().get(index)){
//				
//				newFurniture = Instantiate(GetComponent<FurnitureStore>().get(index), hit.point,Quaternion.identity) as GameObject;// new Quaternion(objetos[numObjeto].transform.rotation.x, objetos[numObjeto].transform.rotation.y, objetos[numObjeto].transform.rotation.z, objetos[numObjeto].transform.rotation.w)) as GameObject;
//				newFurniture.layer = LayerMask.NameToLayer("Moveis");
//			
//				/* Por enquanto */
//				FurnitureData fd = newFurniture.AddComponent<Furniture>().Data;
//			
//				fd.Name 	= "Nome de um móvel";
//				fd.Position = newFurniture.transform.position;
//				fd.Size		= new int[3];
//				fd.Line		= "";
//			
//				/* Só por enquanrto */
//			
//				newFurniture.tag = "MovelSelecionado";
//			}
//            else
//                return;
//			
//            newFurniture.transform.parent = GameObject.Find("Moveis GO").transform;
//        }
//    }
	
	public bool isNewFurnitureActive(){
		return select != -1;
	}

	public GameObject GetActiveNewFurniture(){
		return newFurniture;
	}
	
	public void FreeActiveNewFurniture(){
		select = -1;
//		isCreating = false;
		newFurniture.tag = "Movel";
	}
}
