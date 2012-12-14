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
		rigidbody.isKinematic = true;

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
	
	List<GameObject> furniture = new List<GameObject>();
	
	void SelectObject ()
	{
		if (!enabled)
			return;
		
		furniture.Clear ();
		
		foreach (GameObject f in GameObject.FindGameObjectsWithTag ("Movel"))
		{
			furniture.Add (f);
		}
		
		if (furniture.Count == 0)
			return;
		
		GameObject activeFurniture = GameObject.FindGameObjectWithTag ("MovelSelecionado");
		if (activeFurniture)
		{
			furniture.Remove(activeFurniture);
		}
	}
	
	float vertical, horizontal;
	RaycastHit hit;
	Vector2 mousePosition;
	Vector3 lastPosition;

	void DragObject ()
	{
		if (!isSelected || !enabled || !enableDrag)
			return;

		Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
		
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer ("Cenario")))
		{

			if (hit.transform.tag == "Parede" ||
			    hit.transform.tag == "Chao" ||
				hit.transform.tag == "Teto") {
				
				if (GetComponent<InformacoesMovel> ().tipoMovel != TipoMovel.MOVEL) {
					/*if (furniture.Count != 0)
					{
						foreach (GameObject f in furniture)
						{
							transform.position = (Mathf.Clamp(	hit.point.x, 
															  	f.collider.bounds.min.x,
															  	f.collider.bounds.max.x) * transform.parent.right) + 
												 (Mathf.Clamp(	hit.point.z, 
																f.collider.bounds.min.z,
																f.collider.bounds.max.z) * transform.parent.forward);
						}
					}
					else
					{*/
					bool colliding = false;
					Vector3 collisionPrecision = Vector3.zero;
					if (furniture.Count != 0)
					{
						Vector3 hitPoint = hit.point;
						hitPoint.y = transform.localPosition.y;
						SnapFunction (hitPoint, ref collisionPrecision, ref colliding);
					}
					
					if (!colliding)
					{
						transform.localPosition = (hit.point.x * transform.parent.right) + 
										 	 (hit.point.z * transform.parent.forward);
					}
					else
					{
						transform.localPosition = collisionPrecision;
						lastPosition = transform.localPosition;
					}
					//}
				}
				else
				{
					transform.position = (hit.point.x * transform.parent.right) + 
										 (hit.point.y * transform.parent.up) + 
										 (hit.point.z * transform.parent.forward);
				}
			}
			 
			wasDragged = true;
		}
	}

	void DropObject ()
	{
		if (!isSelected || !enabled)
			return;
			
		enableDrag = false;
		
		/*		
		if(wasDragged){
			transform.position -= new Vector3(0,0.2f,0);
		}
		*/
		//need to refresh wasDragged 
		wasDragged = false;
		
		//Invoke ("VerifyGround", 0.2f);
	}
	
	void SnapFunction (Vector3 point, ref Vector3 collisionPrecision, ref bool colliding)
	{
		Bounds bounds = new Bounds(point + (collider.bounds.center - transform.localPosition), collider.bounds.size);
		foreach (GameObject f in furniture)
		{
			if (transform != f.transform)
			{
				Vector3 tempCenter = f.collider.bounds.center;
				Bounds bf = new Bounds(tempCenter, f.collider.bounds.size);
				if (bf.Intersects (bounds))
				{
					colliding = true;
					collisionPrecision = hit.point;
					collisionPrecision.y = transform.position.y;
					
//					BoxDebug(bounds.center, bounds.size, Color.magenta);
					
					if (Mathf.Abs(hit.point.x - f.transform.position.x) >= Mathf.Abs(hit.point.z - f.transform.position.z))
					{
						float temp = f.collider.bounds.min.x - (f.collider.bounds.min.x - f.transform.position.x);
						if (hit.point.x < f.transform.position.x)
						{
							collisionPrecision.x = (temp - f.collider.bounds.size.x - ((collider.bounds.size.x - f.collider.bounds.size.x) / 2));
							GameObject otherFurnitureCollider = VerifyCollisionInFurniture (collisionPrecision);
							if (otherFurnitureCollider != null)
							{
//								collisionPrecision.x = transform.localPosition.x;
								SnapFunction (otherFurnitureCollider.transform.localPosition, ref collisionPrecision, ref colliding);
							}
						}
						else
						{
							collisionPrecision.x = (temp + f.collider.bounds.size.x + ((collider.bounds.size.x - f.collider.bounds.size.x) / 2));
							GameObject otherFurnitureCollider = VerifyCollisionInFurniture (collisionPrecision);
							if (otherFurnitureCollider != null)
							{
//								collisionPrecision.x = transform.localPosition.x;
								SnapFunction (otherFurnitureCollider.transform.localPosition, ref collisionPrecision, ref colliding);
							}
						}
					}
					else
					{
						float temp = f.collider.bounds.min.z - (f.collider.bounds.min.z - f.transform.position.z);
						if (hit.point.z < f.transform.position.z)
						{
							collisionPrecision.z = (temp - collider.bounds.size.z);
							if (VerifyCollisionInFurniture (collisionPrecision) != null)
							{
//								collisionPrecision.z = (temp + f.collider.bounds.size.z);
							}
						}
						else
						{
							collisionPrecision.z = (temp + collider.bounds.size.z);
							if (VerifyCollisionInFurniture (collisionPrecision) != null)
							{
//								collisionPrecision.z = (temp - f.collider.bounds.size.z);
							}
						}
					}
				}
			}
		}
	}
	
	GameObject VerifyCollisionInFurniture (Vector3 position)
	{
		Bounds newBoundPosition = new Bounds(position + (collider.bounds.center - transform.localPosition), collider.bounds.size - (Vector3.one * 0.001f));
//		BoxDebug(newBoundPosition.center, newBoundPosition.size, Color.red);
		foreach (GameObject newTest in furniture)
		{
			if (transform != newTest.transform)
			{
				Vector3 newTestTempCenter = newTest.collider.bounds.center;
				Bounds newTestBounds = new Bounds(newTestTempCenter, newTest.collider.bounds.size);
//				BoxDebug(newTestBounds.center, newTestBounds.size, Color.blue);
				if (newTestBounds.Intersects (newBoundPosition))
				{
					return newTest;
				}
			}
		}
		return null;
	}
	
	/*void BoxDebug (Vector3 positionMax, Vector3 postionMin, Color color)
	{
		Debug.DrawLine((Vector3.right * postionMin.x) + (Vector3.up * postionMin.y) + (Vector3.forward * postionMin.z), (Vector3.right * positionMax.x) + (Vector3.up * postionMin.y) + (Vector3.forward * postionMin.z), color);
		Debug.DrawLine((Vector3.right * postionMin.x) + (Vector3.up * postionMin.y) + (Vector3.forward * positionMax.z), (Vector3.right * positionMax.x) + (Vector3.up * postionMin.y) + (Vector3.forward * positionMax.z), color);
		Debug.DrawLine((Vector3.right * postionMin.x) + (Vector3.up * postionMin.y) + (Vector3.forward * postionMin.z), (Vector3.right * postionMin.x) + (Vector3.up * postionMin.y) + (Vector3.forward * positionMax.z), color);
		Debug.DrawLine((Vector3.right * positionMax.x) + (Vector3.up * postionMin.y) + (Vector3.forward * postionMin.z), (Vector3.right * positionMax.x) + (Vector3.up * postionMin.y) + (Vector3.forward * positionMax.z), color);
		Debug.DrawLine((Vector3.right * postionMin.x) + (Vector3.up * positionMax.y) + (Vector3.forward * postionMin.z), (Vector3.right * positionMax.x) + (Vector3.up * positionMax.y) + (Vector3.forward * postionMin.z), color);
		Debug.DrawLine((Vector3.right * postionMin.x) + (Vector3.up * positionMax.y) + (Vector3.forward * positionMax.z), (Vector3.right * positionMax.x) + (Vector3.up * positionMax.y) + (Vector3.forward * positionMax.z), color);
		Debug.DrawLine((Vector3.right * postionMin.x) + (Vector3.up * positionMax.y) + (Vector3.forward * postionMin.z), (Vector3.right * postionMin.x) + (Vector3.up * positionMax.y) + (Vector3.forward * positionMax.z), color);
		Debug.DrawLine((Vector3.right * positionMax.x) + (Vector3.up * positionMax.y) + (Vector3.forward * postionMin.z), (Vector3.right * positionMax.x) + (Vector3.up * positionMax.y) + (Vector3.forward * positionMax.z), color);
		Debug.DrawLine((Vector3.right * postionMin.x) + (Vector3.up * postionMin.y) + (Vector3.forward * positionMax.z), (Vector3.right * postionMin.x) + (Vector3.up * positionMax.y) + (Vector3.forward * positionMax.z), color);
		Debug.DrawLine((Vector3.right * postionMin.x) + (Vector3.up * postionMin.y) + (Vector3.forward * postionMin.z), (Vector3.right * postionMin.x) + (Vector3.up * positionMax.y) + (Vector3.forward * postionMin.z), color);
		Debug.DrawLine((Vector3.right * positionMax.x) + (Vector3.up * postionMin.y) + (Vector3.forward * positionMax.z), (Vector3.right * positionMax.x) + (Vector3.up * positionMax.y) + (Vector3.forward * positionMax.z), color);
		Debug.DrawLine((Vector3.right * positionMax.x) + (Vector3.up * postionMin.y) + (Vector3.forward * postionMin.z), (Vector3.right * positionMax.x) + (Vector3.up * positionMax.y) + (Vector3.forward * postionMin.z), color);
	}*/
	
	/*void BoxDebug (Vector3 center, Vector3 positionMin, Vector3 positionMax, Color color)
	{
		Debug.DrawLine((Vector3.right * (center.x - positionMin.x)) + (Vector3.up * (center.y - positionMin.y)) + (Vector3.forward * (center.z - positionMin.z)), (Vector3.right * (center.x + positionMax.x)) + (Vector3.up * (center.y - positionMin.y)) + (Vector3.forward * (center.z - positionMin.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - positionMin.x)) + (Vector3.up * (center.y - positionMin.y)) + (Vector3.forward * (center.z + positionMax.z)), (Vector3.right * (center.x + positionMax.x)) + (Vector3.up * (center.y - positionMin.y)) + (Vector3.forward * (center.z + positionMax.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - positionMin.x)) + (Vector3.up * (center.y - positionMin.y)) + (Vector3.forward * (center.z - positionMin.z)), (Vector3.right * (center.x - positionMin.x)) + (Vector3.up * (center.y - positionMin.y)) + (Vector3.forward * (center.z + positionMax.z)), color);
		Debug.DrawLine((Vector3.right * (center.x + positionMax.x)) + (Vector3.up * (center.y - positionMin.y)) + (Vector3.forward * (center.z - positionMin.z)), (Vector3.right * (center.x + positionMax.x)) + (Vector3.up * (center.y - positionMin.y)) + (Vector3.forward * (center.z + positionMax.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - positionMin.x)) + (Vector3.up * (center.y + positionMax.y)) + (Vector3.forward * (center.z - positionMin.z)), (Vector3.right * (center.x + positionMax.x)) + (Vector3.up * (center.y + positionMax.y)) + (Vector3.forward * (center.z - positionMin.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - positionMin.x)) + (Vector3.up * (center.y + positionMax.y)) + (Vector3.forward * (center.z + positionMax.z)), (Vector3.right * (center.x + positionMax.x)) + (Vector3.up * (center.y + positionMax.y)) + (Vector3.forward * (center.z + positionMax.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - positionMin.x)) + (Vector3.up * (center.y + positionMax.y)) + (Vector3.forward * (center.z - positionMin.z)), (Vector3.right * (center.x - positionMin.x)) + (Vector3.up * (center.y + positionMax.y)) + (Vector3.forward * (center.z + positionMax.z)), color);
		Debug.DrawLine((Vector3.right * (center.x + positionMax.x)) + (Vector3.up * (center.y + positionMax.y)) + (Vector3.forward * (center.z - positionMin.z)), (Vector3.right * (center.x + positionMax.x)) + (Vector3.up * (center.y + positionMax.y)) + (Vector3.forward * (center.z + positionMax.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - positionMin.x)) + (Vector3.up * (center.y - positionMin.y)) + (Vector3.forward * (center.z + positionMax.z)), (Vector3.right * (center.x - positionMin.x)) + (Vector3.up * (center.y + positionMax.y)) + (Vector3.forward * (center.z + positionMax.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - positionMin.x)) + (Vector3.up * (center.y - positionMin.y)) + (Vector3.forward * (center.z - positionMin.z)), (Vector3.right * (center.x - positionMin.x)) + (Vector3.up * (center.y + positionMax.y)) + (Vector3.forward * (center.z - positionMin.z)), color);
		Debug.DrawLine((Vector3.right * (center.x + positionMax.x)) + (Vector3.up * (center.y - positionMin.y)) + (Vector3.forward * (center.z + positionMax.z)), (Vector3.right * (center.x + positionMax.x)) + (Vector3.up * (center.y + positionMax.y)) + (Vector3.forward * (center.z + positionMax.z)), color);
		Debug.DrawLine((Vector3.right * (center.x + positionMax.x)) + (Vector3.up * (center.y - positionMin.y)) + (Vector3.forward * (center.z - positionMin.z)), (Vector3.right * (center.x + positionMax.x)) + (Vector3.up * (center.y + positionMax.y)) + (Vector3.forward * (center.z - positionMin.z)), color);
	}*/
	
	void BoxDebug (Vector3 center, Vector3 size, Color color)
	{
		size /= 2;
		Debug.DrawLine((Vector3.right * (center.x - size.x)) + (Vector3.up * (center.y - size.y)) + (Vector3.forward * (center.z - size.z)), (Vector3.right * (center.x + size.x)) + (Vector3.up * (center.y - size.y)) + (Vector3.forward * (center.z - size.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - size.x)) + (Vector3.up * (center.y - size.y)) + (Vector3.forward * (center.z + size.z)), (Vector3.right * (center.x + size.x)) + (Vector3.up * (center.y - size.y)) + (Vector3.forward * (center.z + size.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - size.x)) + (Vector3.up * (center.y - size.y)) + (Vector3.forward * (center.z - size.z)), (Vector3.right * (center.x - size.x)) + (Vector3.up * (center.y - size.y)) + (Vector3.forward * (center.z + size.z)), color);
		Debug.DrawLine((Vector3.right * (center.x + size.x)) + (Vector3.up * (center.y - size.y)) + (Vector3.forward * (center.z - size.z)), (Vector3.right * (center.x + size.x)) + (Vector3.up * (center.y - size.y)) + (Vector3.forward * (center.z + size.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - size.x)) + (Vector3.up * (center.y + size.y)) + (Vector3.forward * (center.z - size.z)), (Vector3.right * (center.x + size.x)) + (Vector3.up * (center.y + size.y)) + (Vector3.forward * (center.z - size.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - size.x)) + (Vector3.up * (center.y + size.y)) + (Vector3.forward * (center.z + size.z)), (Vector3.right * (center.x + size.x)) + (Vector3.up * (center.y + size.y)) + (Vector3.forward * (center.z + size.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - size.x)) + (Vector3.up * (center.y + size.y)) + (Vector3.forward * (center.z - size.z)), (Vector3.right * (center.x - size.x)) + (Vector3.up * (center.y + size.y)) + (Vector3.forward * (center.z + size.z)), color);
		Debug.DrawLine((Vector3.right * (center.x + size.x)) + (Vector3.up * (center.y + size.y)) + (Vector3.forward * (center.z - size.z)), (Vector3.right * (center.x + size.x)) + (Vector3.up * (center.y + size.y)) + (Vector3.forward * (center.z + size.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - size.x)) + (Vector3.up * (center.y - size.y)) + (Vector3.forward * (center.z + size.z)), (Vector3.right * (center.x - size.x)) + (Vector3.up * (center.y + size.y)) + (Vector3.forward * (center.z + size.z)), color);
		Debug.DrawLine((Vector3.right * (center.x - size.x)) + (Vector3.up * (center.y - size.y)) + (Vector3.forward * (center.z - size.z)), (Vector3.right * (center.x - size.x)) + (Vector3.up * (center.y + size.y)) + (Vector3.forward * (center.z - size.z)), color);
		Debug.DrawLine((Vector3.right * (center.x + size.x)) + (Vector3.up * (center.y - size.y)) + (Vector3.forward * (center.z + size.z)), (Vector3.right * (center.x + size.x)) + (Vector3.up * (center.y + size.y)) + (Vector3.forward * (center.z + size.z)), color);
		Debug.DrawLine((Vector3.right * (center.x + size.x)) + (Vector3.up * (center.y - size.y)) + (Vector3.forward * (center.z - size.z)), (Vector3.right * (center.x + size.x)) + (Vector3.up * (center.y + size.y)) + (Vector3.forward * (center.z - size.z)), color);
	}
	
	public bool AABBContains (Vector3 min, Vector3 max)
	{
		Vector3 _tempVec;
		
		_tempVec = collider.bounds.min;
		if (min.x < _tempVec.x || min.y < _tempVec.y || min.z < _tempVec.z)
			return false;

		_tempVec = collider.bounds.max;
		if (max.x > _tempVec.x || max.y > _tempVec.y || max.z > _tempVec.z)
			return false;

		return true;
	}
}