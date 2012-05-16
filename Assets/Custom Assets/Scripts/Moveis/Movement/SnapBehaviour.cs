using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnapBehaviour : MonoBehaviour
{
		
	#region public variables
	public bool canSnapTop;
	public bool canSnapBottom;
	#endregion
	
	public bool collidedWithWall = false;

	public bool wasDragged { get; private set; }

	private bool isSelected = false;
	private Camera mainCamera;
	
	#region temp variables
	private Vector3 direction;
	private GameObject collidingMobile;
	#endregion
	
	private float coolDownTime;
	private bool enableDrag;
	private Vector3 p; //Pivot value -1..1, calculated from Mesh bounds
	private Vector3 last_p; //Last used pivot
	
	#region Unity Methods
	
	public static void ActivateAll ()
	{
		SetAll (true);
	}
	
	public static void DeactivateAll ()
	{
		SetAll (false);
	}
	
	private static void SetAll (bool value)
	{
		GameObject[] furniture = GameObject.FindGameObjectsWithTag ("Movel");
		foreach (GameObject mobile in furniture) {
			mobile.GetComponent<SnapBehaviour> ().enabled = value;
		}
		
		GameObject activeMobile = GameObject.FindGameObjectWithTag ("MovelSelecionado");
		if (activeMobile != null) {
			activeMobile.GetComponent<SnapBehaviour> ().enabled = value;
		}
	}
	
	void Start ()
	{
		coolDownTime = 0.4f;
		enableDrag = false;
		rigidbody.detectCollisions = true;
		rigidbody.mass = 1000.0f;
		rigidbody.drag = 100.0f;
		rigidbody.angularDrag = 100.0f;
		rigidbody.freezeRotation = true;

		mainCamera = GameObject.FindWithTag ("MainCamera").camera;

	}

	#endregion
		
	public bool Select {
		get { return isSelected; }
		set {
			//print("setado: " + this.name + " para " + (value == true? "selecionado" : "Não selecionado"));
			isSelected = value;
			if (isSelected) {
				Invoke ("EnableDrag", coolDownTime);
			}
		}
	}
    
	private void EnableDrag ()
	{
		enableDrag = true;
	}
	
	#if (!UNITY_ANDROID && !UNITY_IPHONE) || UNITY_EDITOR
	void OnMouseDown ()
	{
		SelectObject ();
	}
	
	void OnMouseDrag ()
	{
		DragObject ();
	}
	
	void OnMouseUp ()
	{
		DropObject ();
	}
	#endif
	
	#if UNITY_ANDROID || UNITY_IPHONE
	// Android & Iphone Method to move object
	void Update () {
		if (Input.touchCount == 1) {
			Touch touch = Input.GetTouch(0);
			switch (touch.phase) {
				case TouchPhase.Began :
					SelectObject();
					break;
				case TouchPhase.Moved :
					DragObject();
					break;
				case TouchPhase.Canceled :
					DropObject();
					break;
				case TouchPhase.Ended :
					DropObject();
					break;
			}
		}
	}
	#endif
	
	void SelectObject ()
	{
		if (!enabled)
			return;
		
		GameObject[] furniture = GameObject.FindGameObjectsWithTag ("Movel");
		
		if (furniture.Length == 0)
			return;
		
		for (int i = 0; i != furniture.Length; ++i) {
			furniture [i].rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
		
		GameObject activeFurniture = GameObject.FindGameObjectWithTag ("MovelSelecionado");
		
		if (activeFurniture != null)
			activeFurniture.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}
	
	float vertical, horizontal;
	RaycastHit hit;
	Vector2 mousePosition;

	void DragObject ()
	{
		if (!isSelected || !enabled || !enableDrag)
			return;

		Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
		
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer ("Cenario"))) {

			if (hit.transform.tag == "Parede" ||
			    hit.transform.tag == "Chao" ||
				hit.transform.tag == "Teto") {

				if (GetComponent<InformacoesMovel> ().tipoMovel != TipoMovel.MOVEL) {
					transform.position = (hit.point.x * transform.parent.right) + 
										 (hit.point.z * transform.parent.forward);
				} else {
					transform.position = (hit.point.x * transform.parent.right) + 
										 (hit.point.y * transform.parent.up) + 
										 (hit.point.z * transform.parent.forward);
				}
			}
		
			//transform.position += new Vector3(0,0.2f,0);
			 
			wasDragged = true;
		}
	}

	void DropObject ()
	{
		if (!isSelected || !enabled)
			return;
			
		enableDrag = false;
		
		if (GetComponent<InformacoesMovel> ().tipoMovel != TipoMovel.MOVEL) {
			this.rigidbody.constraints = RigidbodyConstraints.FreezePositionY |
											RigidbodyConstraints.FreezeRotation;
		} else {
			this.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		}
		/*		
		if(wasDragged){
			transform.position -= new Vector3(0,0.2f,0);
		}
		*/
		//need to refresh wasDragged 
		wasDragged = false;
		
		Invoke ("VerifyGround", 0.2f);
	}
	
	void VerifyGround ()
	{
		uint foundGround = 0;
		
		Vector3[] origins = new Vector3[4]{new Vector3 (this.collider.bounds.center.x - 
			                                            this.collider.bounds.size.x / 4,
						                             	2,
			                                            this.collider.bounds.center.z - 
			                                            this.collider.bounds.size.z / 4),
											new Vector3 (this.collider.bounds.center.x - 
			                                            this.collider.bounds.size.x / 4,
						                             	2,
			                                            this.collider.bounds.center.z + 
			                                            this.collider.bounds.size.z / 4),
											new Vector3 (this.collider.bounds.center.x + 
			                                            this.collider.bounds.size.x / 4,
						                             	2,
			                                            this.collider.bounds.center.z - 
			                                            this.collider.bounds.size.z / 4),
											new Vector3 (this.collider.bounds.center.x + 
			                                            this.collider.bounds.size.x / 4,
						                             	2,
			                                            this.collider.bounds.center.z + 
			                                            this.collider.bounds.size.z / 4)};
				
		Ray ray;
		foreach (Vector3 origin in origins) {
			ray = new Ray (origin, Vector3.down);
//			Debug.DrawRay(ray.origin, ray.direction * 100, Color.cyan);
			if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer ("Cenario"))) {			
				if (hit.transform.tag == "Chao" || hit.transform.tag == "Parede") {
					++foundGround;
				} else {
//					Debug.Log(hit.transform.tag);	
				}
			} else {
//				Debug.LogError ("Whata? Não pegou em nada?");
			}
		}
//		Debug.Break();
//		Debug.DebugBreak();
		
		Debug.Log ("foundGround: " + foundGround.ToString ());
		if (foundGround != 4) {
			
			GameObject[] ground = GameObject.FindGameObjectsWithTag ("ChaoVazio");
			GameObject nearestAvailableGround = null;
			
			float shortestDistance = float.MaxValue;
			float distance;
			foreach (GameObject groundPiece in ground) {
				distance = Vector3.Distance (groundPiece.transform.position, hit.point);
				if (distance < shortestDistance) {
					shortestDistance = distance;
					nearestAvailableGround = groundPiece;
				}
			}
			
			if (GetComponent<InformacoesMovel> ().tipoMovel != TipoMovel.MOVEL)
			{
				this.transform.position  = nearestAvailableGround.transform.position;
			}
			else 
			{
				Vector3 positionMobile = nearestAvailableGround.transform.position;
				positionMobile.y = this.transform.position.y;
				this.transform.position  = positionMobile;
			}
		}
	}
}