#define DEEP_DEBUG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Rigidbody))]
public class SnapBehaviour : MonoBehaviour {

	const float IPSLON = 0.2f;

	public bool wasDragged { get; private set; }

	private Camera mainCamera;

	#region temp variables
	private Ray ray;
	private RaycastHit hit;
	private Vector3 lastMousePosition;
	private Vector3 mousePosition;
	#endregion

	private float coldDownTime;
	private bool enableDrag;
	private bool isSelected;

	private Vector3 referencePoint;
	private Transform referenceTransform;
	private float halfModuleWidth;
	private int scenarioLayer;
	#region Unity Methods

	private bool isOnTheWall;
	void Start()
	{
		coldDownTime = 0.4f;
		enableDrag = false;
        rigidbody.detectCollisions = true;
        rigidbody.mass = 1000.0f;
        rigidbody.drag = 100.0f;
        rigidbody.angularDrag = 100.0f;
        rigidbody.freezeRotation = true;

		mainCamera = GameObject.FindWithTag ("MainCamera").camera;
		halfModuleWidth = (collider as BoxCollider).size.x / 1.9f;
		scenarioLayer = 1 << LayerMask.NameToLayer("Cenario");
		isOnTheWall = false;
	}

	// Android & Iphone Method to move object
	void Update ()
	{
		//se nada aconteceu ou não foi clicado no objeto retorna
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_WEBPLAYER || UNITY_EDITOR
		if (Input.GetMouseButtonDown(0) ||
			Input.GetMouseButton(0) 	||
			Input.GetMouseButtonUp(0))
		{
			lastMousePosition = mousePosition;
			mousePosition 	  = Input.mousePosition;
#else
			if (Input.GetTouch(0).phase == )
			{
				Touch touch = Input.GetTouch(0);
				Vector2 mousePosition = touch.position;
				//TODO this works
#endif
	        ray = mainCamera.ScreenPointToRay(mousePosition);

	        Physics.Raycast(ray, out hit, Mathf.Infinity);

	        if (hit.transform != transform)
	        	return;
		}
		else
		{
			return;
		}

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_WEBPLAYER || UNITY_EDITOR

		if (Input.GetMouseButtonDown(0))
		{
			SelectObject();
		}
 		else if (Input.GetMouseButton(0) &&
				 Vector3.Distance(lastMousePosition, Input.mousePosition) > IPSLON)
		{
			DragObject();
		}
		else if (Input.GetMouseButtonUp(0))
		{
			DropObject();
		}
#else
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
#endif
	}

	#endregion

	void OnCollisionEnter(Collision collision)
	{
		if(!isOnTheWall)
		{
			if ( (collision.transform.tag == "Parede"))
			{
				transform.rotation = collision.transform.rotation;
			}
			else if ( hit.transform.tag == "Teto" &&
					 (GetComponent<InformacoesMovel> ().tipoMovel == TipoMovel.MOVEL))
			{
				Vector3 nEuler = hit.transform.eulerAngles;
				nEuler.x = - nEuler.x;
				transform.eulerAngles = nEuler;
			}

			isOnTheWall = true;
		}
	}

	public static void ActivateAll()
	{
		SetAll(true);
	}

	public static void DeactivateAll()
	{
		SetAll(false);
	}

	private static void SetAll(bool value)
	{
		GameObject[] furniture = GameObject.FindGameObjectsWithTag("Movel");
		foreach(GameObject mobile in furniture)
		{
			mobile.GetComponent<SnapBehaviour>().enabled = value;
		}

		GameObject activeMobile = GameObject.FindGameObjectWithTag("MovelSelecionado");
		if(activeMobile != null)
		{
			activeMobile.GetComponent<SnapBehaviour>().enabled = value;
		}
	}

    public bool Select {
        get { return isSelected; }
        set {
			//print("setado: " + this.name + " para " + (value == true? "selecionado" : "Não selecionado"));
		    isSelected = value;
		    if(isSelected)
		    {
		    	Invoke("EnableDrag", coldDownTime);
		    }
        }
    }

    private void EnableDrag()
    {
    	enableDrag = true;
    }

	void SelectObject ()
	{
		GameObject[] furniture 	   = GameObject.FindGameObjectsWithTag("Movel");
		GameObject activeFurniture = GameObject.FindGameObjectWithTag ("MovelSelecionado");

		if(furniture.Length == 0 && activeFurniture == null)
			return;

		for(int i = 0; i != furniture.Length; ++i)
		{
			furniture[i].rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}

		if(activeFurniture != null)
		{
			activeFurniture.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}

        ray = mainCamera.ScreenPointToRay(mousePosition);

        Physics.Raycast(ray, out hit, Mathf.Infinity, scenarioLayer);

		if (referenceTransform == hit.transform)
		{
			referencePoint = hit.point;
		}
		else
		{
			referencePoint = transform.position;
		}

		referenceTransform = hit.transform;

		if (hit.transform.tag == "Parede")
		{
			transform.rotation = hit.transform.rotation;
		}
		else if ( hit.transform.tag == "Teto" &&
				 (GetComponent<InformacoesMovel> ().tipoMovel == TipoMovel.MOVEL))
		{
			Vector3 nEuler = hit.transform.eulerAngles;
			nEuler.x = - nEuler.x;
			transform.eulerAngles = nEuler;
		}

		enableDrag = true;
	}

	bool wasTurn = false;
	void DragObject ()
	{
		if(!isSelected)
			return;

        ray = mainCamera.ScreenPointToRay (mousePosition);

        if(!Physics.Raycast (ray, out hit, Mathf.Infinity, scenarioLayer))
        	return;

		Vector3 moduleCenter = collider.bounds.center;

		Ray leftRay  = new Ray(moduleCenter, - transform.right.normalized);
		Ray rightRay = new Ray(moduleCenter,   transform.right.normalized);

//		Debug.DrawRay (leftRay.origin,  leftRay.direction,  Color.green);
//		Debug.DrawRay (rightRay.origin, rightRay.direction, Color.blue);
//		Debug.Break ();

		RaycastHit leftHit;
		RaycastHit rightHit;

		isOnTheWall = (referenceTransform.tag == "Parede");

		if (referenceTransform == hit.transform)
		{
			Vector3 vector = (hit.point - referencePoint);
			referencePoint =  hit.point;

			TipoMovel tipoMovel = GetComponent<InformacoesMovel>().tipoMovel;
			if (tipoMovel == TipoMovel.FIXO ||
				tipoMovel == TipoMovel.CANTOFIXO)
			{
				vector.y = 0;
			}

			if (halfModuleWidth < WallController.GetNearestWallDistance (transform))
			{
				transform.position += vector;
			}
			else
			{
				bool wasMouseMovedToRightSide = mousePosition.x > lastMousePosition.x;
				bool wasCollidedOnLeftSide 	  = Physics.Raycast(leftRay,  out leftHit,
																halfModuleWidth, scenarioLayer);
				bool wasCollidedOnRightSide   = Physics.Raycast(rightRay, out rightHit,
																halfModuleWidth, scenarioLayer);

				if ( ( wasMouseMovedToRightSide && !wasCollidedOnLeftSide)  ||
					 (!wasMouseMovedToRightSide && !wasCollidedOnRightSide) )
				{
					transform.position += vector;
				}
			}
		}
		else
		{
			Vector3 newPosition = hit.point;

			if (!isOnTheWall)
			{
				if ( hit.transform.tag == "Teto" &&
					(GetComponent<InformacoesMovel> ().tipoMovel == TipoMovel.MOVEL))
				{
					Vector3 nEuler = hit.transform.eulerAngles;
					nEuler.x = - nEuler.x;
					transform.eulerAngles = nEuler;
				}
			}
			if (isOnTheWall && hit.transform.tag == "Parede")
			{
				transform.rotation = hit.transform.rotation;

				//se não é direita é esquerda
				if (Physics.Raycast(rightRay, out rightHit, halfModuleWidth, scenarioLayer))
				{
					newPosition =  hit.transform.position
								- (transform.right.normalized * hit.transform.localScale.x)
								+ (transform.right.normalized * halfModuleWidth);
				}
				else
				{
					newPosition =  hit.transform.position
								- (transform.right.normalized * halfModuleWidth);
				}
			}

			TipoMovel tipoMovel = GetComponent<InformacoesMovel>().tipoMovel;
			if (tipoMovel == TipoMovel.FIXO ||
				tipoMovel == TipoMovel.CANTOFIXO)
			{
				newPosition.y = transform.position.y;
			}

			referenceTransform = hit.transform;
			transform.position = newPosition;
		}

		wasDragged = true;
	}

	void DropObject ()
	{
		enableDrag = false;

		if(!isSelected || !enabled)
			return;

		if (GetComponent<InformacoesMovel>().tipoMovel != TipoMovel.MOVEL)
		{
			this.rigidbody.constraints = RigidbodyConstraints.FreezePositionY |
										 RigidbodyConstraints.FreezeRotation;
		}
		else
		{
			this.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		}

		wasDragged = false;
		
//		Invoke("VerifyGround",0.2f);
	}
	
	void VerifyGround()
	{
		uint foundGround = 0;
		
		Vector3[] origins =  new Vector3[4]{new Vector3(this.collider.bounds.center.x - 
			                                            this.collider.bounds.size.x / 4,
						                             	2,
			                                            this.collider.bounds.center.z - 
			                                            this.collider.bounds.size.z / 4),
											new Vector3(this.collider.bounds.center.x - 
			                                            this.collider.bounds.size.x / 4,
						                             	2,
			                                            this.collider.bounds.center.z + 
			                                            this.collider.bounds.size.z / 4),
											new Vector3(this.collider.bounds.center.x + 
			                                            this.collider.bounds.size.x / 4,
						                             	2,
			                                            this.collider.bounds.center.z - 
			                                            this.collider.bounds.size.z / 4),
											new Vector3(this.collider.bounds.center.x + 
			                                            this.collider.bounds.size.x / 4,
						                             	2,
			                                            this.collider.bounds.center.z + 
			                                            this.collider.bounds.size.z / 4)};
				

		foreach(Vector3 origin in origins)
		{
			ray = new Ray(origin, Vector3.down);
			Debug.DrawRay(ray.origin, ray.direction * 100, Color.cyan,100);
			if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.NameToLayer("Cenario")))
			{
				if(hit.transform.tag == "Chao" || hit.transform.tag == "Parede")
				{
					++foundGround;
				} else {
//					Debug.Log(hit.transform.tag);	
				}
			}
			else
			{
//				Debug.LogError ("Whata? Não pegou em nada?");
			}
		}
		Debug.Log("foundGround: " + foundGround);
		Debug.Break();
		return;
		
//		Debug.Log("foundGround: " + foundGround.ToString());
		if(foundGround != 4)
		{
			GameObject[] ground = GameObject.FindGameObjectsWithTag("ChaoVazio");
			GameObject nearestAvailableGround = null;
			
			float shortestDistance = float.MaxValue;
			float distance;
			foreach(GameObject groundPiece in ground)
			{
				distance = Vector3.Distance(groundPiece.transform.position,hit.point);
				if(distance < shortestDistance)
				{
					shortestDistance = distance;
					nearestAvailableGround = groundPiece;
				}
			}
			
			if (GetComponent<InformacoesMovel>().tipoMovel != TipoMovel.MOVEL)
			{
				this.rigidbody.position = nearestAvailableGround.transform.position;
			} 
			else 
			{
				Vector3 positionMobile = nearestAvailableGround.transform.position;
				positionMobile.y = this.transform.position.y;
				this.rigidbody.position = positionMobile;
			}
		}
	}
}
