using UnityEngine;
using System.Collections;

public class ClickWindowItem : MonoBehaviour {

	public const float IPSLON = 0.0001f;
	public const float monkeyPatchUpperWallYScale = 0.0647338f; //I hate to do a shit like this...

	public GameObject item;
	public Camera camera;

	private GameObject wallParent;
	private float WindowPivotY;

	void Start ()
	{
		wallParent = GameObject.Find ("ParentParede");

		WindowPivotY = 0.7f;
	}

	void OnClick ()
	{
		RaycastHit hit;
		Ray ray = camera.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2, 0));
				
		if (!Physics.Raycast (ray, out hit))
		{
			Debug.Log ("Não bateu em nada.");
			Debug.DrawRay (ray.origin, ray.direction * 1000);
			Debug.Break ();
		}
		else if (hit.transform.tag == "Parede")
		{
			CreateWindow (ray, hit);
		}
	}
	
	private void CreateWindow (Ray ray, RaycastHit hit)
	{
		if (hit.transform.name.Equals ("Upper Wall") ||
			hit.transform.name.Equals ("Lower Wall"))
		{
			Debug.LogWarning ("Não foi criada a janela, pois foi selecionada a paredes de cima e de baixo de uma janela pré-existente.");
			return;
		}

		Vector3 hitPoint = hit.point;
		Transform wallTrans = hit.transform;
		Vector3 wallPosition = wallTrans.position;
		Vector3 wallRotation = wallTrans.eulerAngles;
		Vector3 cWallSize 	 = item.transform.GetChild (0).localScale;
		Vector3 maxWallPosition = wallPosition + (wallTrans.transform.right.normalized * cWallSize.x);
		
		hitPoint.y = 0.0f;
		
		cWallSize = MeshUtils.FixZYFromBlender (cWallSize);
			
		if ( ( cWallSize.x / 2.0f ) > Vector3.Distance (hitPoint, wallPosition) ||
			 ( cWallSize.x / 2.0f ) > Vector3.Distance (hitPoint, maxWallPosition))
		{
			Debug.LogWarning ("Não foi criada a janela por que foi clicado muito perto do canto da parede.");
			return;
		}
			
		InfoWall infoWall = wallTrans.GetComponent<InfoWall> ();
        	
		float cRightScaleX = Vector3.Distance (wallPosition, hitPoint) - (cWallSize.x / 2.0f); 
		float cLeftScaleX = (wallTrans.localScale.x - cRightScaleX) - (cWallSize.x);
		float cMiddleScaleX = wallTrans.localScale.x - cRightScaleX - cLeftScaleX;
		
		float cMiddleUpperScaleY = wallTrans.localScale.y - cWallSize.y - WindowPivotY - monkeyPatchUpperWallYScale;
		float cMiddleLowerScaleY = WindowPivotY;

			
		Vector3 leftWallPosition = wallPosition - 
									   	(wallTrans.transform.right.normalized *
										(cRightScaleX + cWallSize.x));
		Vector3 middleWallPosition = wallPosition - 
									   	(wallTrans.transform.right.normalized * cRightScaleX);
		leftWallPosition.y = 0.0f;

		GameObject rightWall = Instantiate (wallTrans.gameObject,
    										wallTrans.position,
    										wallTrans.rotation) as GameObject;
		rightWall.name = "Right Wall";
		rightWall.transform.localScale = new Vector3 (cRightScaleX,
    												  wallTrans.localScale.y,
    												  wallTrans.localScale.z);
		MaterialUtils.ResizeMaterial (rightWall, cRightScaleX, wallTrans.localScale.y, 0, 0);

		middleWallPosition.y = 0.0f;
		GameObject lowerWall = Instantiate (wallTrans.gameObject,
    									    middleWallPosition,
    									    wallTrans.rotation) as GameObject;
		lowerWall.name = "Lower Wall";
		lowerWall.transform.localScale = new Vector3 (cMiddleScaleX,
    												  cMiddleLowerScaleY,
    												  wallTrans.localScale.z);
		MaterialUtils.ResizeMaterial (lowerWall,
											cMiddleScaleX,
											cMiddleLowerScaleY,
											cRightScaleX,
											0.003f);

		middleWallPosition.y = cWallSize.y + WindowPivotY;
		GameObject upperWall = Instantiate (wallTrans.gameObject,
    									    middleWallPosition,
    									    wallTrans.rotation) as GameObject;
		upperWall.name = "Upper Wall";
		upperWall.transform.localScale = new Vector3 (cMiddleScaleX,
    												  cMiddleUpperScaleY,
    												  wallTrans.localScale.z);
		MaterialUtils.ResizeMaterial (upperWall,
											cMiddleScaleX,
											cMiddleUpperScaleY,
											cRightScaleX,
											cMiddleLowerScaleY + cWallSize.y + 0.01f);

		GameObject leftWall = Instantiate (wallTrans.gameObject,
    									   leftWallPosition,
    									   wallTrans.rotation) as GameObject;
		leftWall.name = "Left Wall";
		leftWall.transform.localScale = new Vector3 (cLeftScaleX,
    												 wallTrans.localScale.y,
    												 wallTrans.localScale.z);
		MaterialUtils.ResizeMaterial (leftWall,
											cLeftScaleX,
											wallTrans.localScale.y,
											cRightScaleX + cMiddleScaleX,
											0);

		GameObject wnd = Instantiate (item,
										new Vector3 (middleWallPosition.x,
													WindowPivotY + cWallSize.y / 2.0f,
													middleWallPosition.z) 
											- wallTrans.right.normalized * cWallSize.x / 2.0f,
										wallTrans.rotation) as GameObject;
			
		//Rotacionando para o lado correto (ao contrário da parede)
		wnd.transform.Rotate (Vector3.up * 180);
			
		leftWall.GetComponent<InfoWall> ().CopyFrom (wallTrans.GetComponent<InfoWall> ());
		rightWall.GetComponent<InfoWall> ().CopyFrom (wallTrans.GetComponent<InfoWall> ());
		lowerWall.GetComponent<InfoWall> ().CopyFrom (wallTrans.GetComponent<InfoWall> ());
		upperWall.GetComponent<InfoWall> ().CopyFrom (wallTrans.GetComponent<InfoWall> ());
			
		GameObject packWall = new GameObject ("PackSculptWall");

		packWall.AddComponent<SculptedWall> ().sculptedWindow = wnd;
		wnd.AddComponent<SculptedWindow> ().sculptedWall = packWall;
					
		packWall.transform.parent = wallTrans.parent;
						   
		leftWall.transform.parent = packWall.transform;
		rightWall.transform.parent = packWall.transform;
		lowerWall.transform.parent = packWall.transform;
		upperWall.transform.parent = packWall.transform;
			
		GameObject wnds = GameObject.Find ("Windows");
		if (wnds == null) {
			wnds = new GameObject ("Windows");
			wnds.transform.parent = wallParent.transform.parent;
		}
			
		wnd.transform.parent = wnds.transform;

		Destroy (wallTrans.gameObject);
	}

	void InstanceNewObject (GameObject gameObject) 
	{
       	Ray ray = camera.ScreenPointToRay(new Vector2(Screen.width / 2,Screen.height / 2));
		RaycastHit hit;
		
        if (!Physics.Raycast(ray, out hit))
        	return;
			
		#region travando a posição de todos os móveis
		GameObject[] furniture = GameObject.FindGameObjectsWithTag("Movel");
		for(int i = 0; i != furniture.Length; ++i)
		{
			furniture[i].rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		}
		#endregion
		
		GameObject newFurniture = Instantiate(gameObject) as GameObject;
							
		newFurniture.tag   = "Movel";
		
		GameObject MoveisGO = GameObject.Find("Moveis GO");
	
		if(MoveisGO == null)
		{
			MoveisGO = new GameObject("Moveis GO");
		}
	
		#region Setando a posicao inicial do móvel se não estiver sobre um "Chao"
		GameObject[] ground = GameObject.FindGameObjectsWithTag("Chao");
		Vector3 nearestAvailableGround = Vector3.zero;
		float shortestDistance = float.MaxValue;
		float distance;
		foreach(GameObject groundPiece in ground)
		{
			if (groundPiece.collider.bounds.Contains (hit.point))
			{
				nearestAvailableGround = hit.point;
				break;
			}
		}
		if (nearestAvailableGround == Vector3.zero)
		{
			nearestAvailableGround = hit.point;
			nearestAvailableGround.y = 0.0f;
		}
	
		newFurniture.transform.position = nearestAvailableGround;
		#endregion
	}
}
