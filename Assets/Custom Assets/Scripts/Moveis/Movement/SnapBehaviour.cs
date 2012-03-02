using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnapBehaviour : MonoBehaviour {
	
	#region public variables
	public bool canSnapTop;
	public bool canSnapBottom;
	#endregion
	
	public bool collidedWithWall = false;
	private bool isSelected = false;
	private bool wasDragged = false;
	
	private Camera mainCamera;
	
	#region temp variables
    private Vector3 direction;
	private GameObject collidingMobile;
	#endregion
	
	private float coolDownTime;
	private bool enableDrag;
		
	#region Unity Methods
	
	public static void ActivateAll(){
		SetAll(true);
	}
	
	public static void DeactivateAll(){
		SetAll(false);
	}
	
	private static void SetAll(bool value){
		GameObject[] furniture = GameObject.FindGameObjectsWithTag("Movel");
		foreach(GameObject mobile in furniture){
			mobile.GetComponent<SnapBehaviour>().enabled = value;
		}
		
		GameObject activeMobile = GameObject.FindGameObjectWithTag("MovelSelecionado");
		if(activeMobile != null){
			activeMobile.GetComponent<SnapBehaviour>().enabled = value;
		}
	}
	
	void Start(){
		coolDownTime = 0.2f;
		enableDrag = false;
        rigidbody.detectCollisions = true;
        rigidbody.mass = 1000.0f;
        rigidbody.drag = 100.0f;
        rigidbody.angularDrag = 100.0f;
        rigidbody.freezeRotation = true;
	}

	#endregion
		
    public bool Select {
        get { return isSelected; }
        set {
			//print("setado: " + this.name + " para " + (value == true? "selecionado" : "Não selecionado"));
		    isSelected = value;
		    if(isSelected){
		    	Invoke("EnableDrag",coolDownTime);
		    }
        }
    }
    
    private void EnableDrag(){
    	enableDrag = true;
    }
	
	void OnMouseDown(){
		
		if(!enabled)
			return;
		
		GameObject[] furniture = GameObject.FindGameObjectsWithTag("Movel");
		
		if(furniture.Length == 0)
			return;
		
		for(int i = 0; i != furniture.Length; ++i){
			furniture[i].rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
		
		GameObject activeFurniture = GameObject.FindGameObjectWithTag("MovelSelecionado");
		
		if(activeFurniture != null)
			activeFurniture.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}
	
	float vertical,horizontal;
	RaycastHit hit;
	Vector2 mousePosition;
	void OnMouseDrag(){
		
		if(!isSelected || !enabled || !enableDrag)
			return;
			
        mainCamera = GameObject.FindWithTag("Player").camera;
		
		mousePosition = Input.mousePosition;
		//mousePosition.x += Screen.width  * 0.0050f;
		//mousePosition.y -= Screen.height * 0.0515f;
		
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
		
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.NameToLayer("Cenario"))){
			
			if (hit.transform.tag == "Grid")
				return;//Ignore Grid			
				
			if (hit.transform.tag == "ParedeParent" ||
			    hit.transform.tag == "ChaoParent"){
				 					
           		transform.position = (hit.point.x * transform.parent.right)	+ 
									 (hit.point.z * transform.parent.forward);
				
			}
		
			//transform.position += new Vector3(0,0.2f,0);
			 
			wasDragged = true;
		}
	}
	
	void OnMouseUp(){
		
		if(!isSelected || !enabled)
			return;
			
		enableDrag = false;
		
		this.rigidbody.constraints = RigidbodyConstraints.FreezePositionY |
									 RigidbodyConstraints.FreezeRotation;
		/*		
		if(wasDragged){
			transform.position -= new Vector3(0,0.2f,0);
		}
		*/
		//need to refresh wasDragged 
		wasDragged = false;
		
		Invoke("VerifyGround",0.4f);
	}
	
	void VerifyGround(){
		
		uint foundGround = 0;
		
		Vector3[] origins =  new Vector3[4]{new Vector3(this.collider.bounds.center.x - 
			                                            this.collider.bounds.size.x / 4,
						                             	10,
			                                            this.collider.bounds.center.z - 
			                                            this.collider.bounds.size.z / 4),
											new Vector3(this.collider.bounds.center.x - 
			                                            this.collider.bounds.size.x / 4,
						                             	10,
			                                            this.collider.bounds.center.z + 
			                                            this.collider.bounds.size.z / 4),
											new Vector3(this.collider.bounds.center.x + 
			                                            this.collider.bounds.size.x / 4,
						                             	10,
			                                            this.collider.bounds.center.z - 
			                                            this.collider.bounds.size.z / 4),
											new Vector3(this.collider.bounds.center.x + 
			                                            this.collider.bounds.size.x / 4,
						                             	10,
			                                            this.collider.bounds.center.z + 
			                                            this.collider.bounds.size.z / 4)};
				
		Ray ray;
		foreach(Vector3 origin in origins){
			ray = new Ray(origin, Vector3.down);
//			Debug.DrawRay(ray.origin, ray.direction * 100, Color.cyan);
			if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.NameToLayer("Cenario"))){			
				if(hit.transform.tag == "ChaoParent" || hit.transform.tag == "ParedeParent"){
					++foundGround;
				} else {
					Debug.Log(hit.transform.tag);	
				}
			}
		}
//		Debug.Break();
//		Debug.DebugBreak();
		
		Debug.Log("foundGround: " + foundGround.ToString());
		if(foundGround != 4){
			
			GameObject[] ground = GameObject.FindGameObjectsWithTag("ChaoVazio");
			GameObject nearestAvailableGround = null;
			
			float shortestDistance = float.MaxValue;
			float distance;
			foreach(GameObject groundPiece in ground){
				distance = Vector3.Distance(groundPiece.transform.position,hit.point);
				if(distance < shortestDistance){
					shortestDistance = distance;
					nearestAvailableGround = groundPiece;
				}
			}
			this.transform.position  = nearestAvailableGround.transform.position;
		}
	}
}