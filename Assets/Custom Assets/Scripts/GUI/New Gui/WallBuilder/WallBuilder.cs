//#define DRAW_RAY
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class InfoWall : MonoBehaviour
{
	public Transform wall;
	public Transform rightWall;
	public Transform leftWall;
	public Color color;
	
	public void SetColor (Color color)
	{
		this.color = color;
	}
}

public class WallBuilder : MonoBehaviour {
	
	public const float IPSLON = 0.0001f;
	
	#region Parents
	public InfoWall[] 	walls;
	public Transform 	parentFloor;
	public Transform 	parentCeil;
	public Transform	parentWalls;
	#endregion
	
	#region Prefabs references
	public Camera 		mainCamera;
	public GameObject 	grid;
	public GameObject 	floor;
	public GameObject 	wall, wall95, wall90, corner;
	public GameObject 	ceil;
	#endregion
	
	#region Camera Data
	private bool 		wasInitialized = false;
	private bool 		activeGrid = false;
	
	private Vector3		mov;
	private float 		speedCam = 5.0f;
		
	private Vector3 	firstPosition;
	
	private float 		zoom;
	private int 		zoomAngle = 40;
	#endregion
	
	#region Wall Size
	public int 		WallWidth { get; set; }
	public int 		WallDepth  { get; set; }
	
	public int 		MaxWallWidth { get; private set; }
	public int 		MaxWallDepth  { get; private set; }
	public int 		MinWallWidth { get; private set; }
	public int 		MinWallDepth  { get; private set; }
	#endregion
	
	#region WorkAround to resolve NGUI bug: Move object to negative positions crashes NGUI functionality
	public static Vector3 ROOT { get; private set; }
	#endregion
	
	#region Unty Methods
	void Start(){
	
		WallWidth = 5;
		WallDepth = 5;
		
		MaxWallWidth = 10;
		MaxWallDepth = 10;
		
		MinWallWidth = 3;
		MinWallDepth = 3;
		
		ROOT = new Vector3(1000,0,1000);
		
		mov = transform.position;
		zoom = GameObject.FindWithTag("MainCamera").camera.orthographicSize;
		
		lastCreatedWalls = new Stack<GameObject> ();
		redoCreatedWalls = new Stack<GameObject> ();
	}
	/*
	private void OnEnable () {
		parentFloor.GetComponent<MeshFilter>().sharedMesh = null;
		foreach (GameObject chaoVazio in GameObject.FindGameObjectsWithTag("ChaoVazio")) {
			Destroy(chaoVazio);
		}
		parentCeil.GetComponent<MeshFilter>().sharedMesh = null;
		foreach (Transform walls in parentWalls.GetWalls()) {
			walls.GetComponent<MeshFilter>().sharedMesh = null;
		}
		grid.renderer.enabled = true;
	}*/
	#endregion
	
	private Ray ray;
	private RaycastHit hit;
	private bool IsFirstClick = true;
	private Vector3 secondPosition;
	
	private Stack<GameObject> lastCreatedWalls;
	private Stack<GameObject> redoCreatedWalls;
	
	public void CreateWall ()
	{
		ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast (ray, out hit);
			
		GameObject box = GameObject.CreatePrimitive (PrimitiveType.Cube);
		box.transform.position  = new Vector3 (ray.origin.x, 0 , ray.origin.z);
		box.transform.localScale = Vector3.one * 0.25f;
		  
		if (IsFirstClick)
		{
			firstPosition = box.transform.position;
			IsFirstClick = !IsFirstClick;
		}
		else
		{
			//Limpar redo
			redoCreatedWalls.Clear ();
		
			secondPosition = box.transform.position;
			
			GameObject newWall = Instantiate (wall, firstPosition, Quaternion.identity) as GameObject;
			lastCreatedWalls.Push (newWall);
			
			float wallScaleX = Vector3.Distance (secondPosition, firstPosition);
			newWall.transform.localScale = new Vector3 ( wallScaleX, 1, 1 );
			
			float wallAngle = Vector3.Angle(Vector3.forward, (secondPosition - firstPosition).normalized);
				
			if (firstPosition.x < secondPosition.x)
				wallAngle += 90f;
			else
				wallAngle = 90f - wallAngle;
			
			newWall.transform.localEulerAngles = Vector3.up * wallAngle;
			
			foreach (Material cMaterial in newWall.transform.GetChild(0).renderer.materials)
			{
				Debug.LogWarning ("materials - cMaterial.name: " + cMaterial.name );
				cMaterial.mainTextureScale = new Vector2 (wallScaleX, 1);
				cMaterial.SetTextureScale ("_BumpMap", new Vector2 (wallScaleX, 1));
			}
			
			firstPosition = secondPosition;
		}
	
	/*
	
		float x, y, z;
								
		x = ray.origin.x;
		y = 0.01f;
		z = ray.origin.z;
		x = x - (int)x > 0.5f ? (int)x + 1 : x - (int)x < -0.5f ? (int)x - 1 : (int)x;
		z = z - (int)z > 0.5f ? (int)z + 1 : z - (int)z < -0.5f ? (int)z - 1 : (int)z;
		
		Vector3 posicaoCalibrada = new Vector3 (x, y, z);
		GameObject[] pisos = GameObject.FindGameObjectsWithTag ("Chao");
		if (pisos.Length > 0) {
			Vector3[] direcoes = new Vector3[] { Vector3.forward, Vector3.right, Vector3.left, Vector3.back };
			int countDirecoes = 0;
			foreach (Vector3 direcao in direcoes) {
				RaycastHit hit2;
				if (Physics.Raycast (posicaoCalibrada, direcao, out hit2, 1.0f)) {
					Debug.DrawRay (posicaoCalibrada, direcao * 1.0f, Color.blue);
					if (hit2.transform.tag == "Chao" && countDirecoes != 1) {
						GameObject novoChao = Instantiate (floor, posicaoCalibrada, floor.transform.rotation) as GameObject;
						novoChao.transform.parent = parentFloor;
						countDirecoes++;
					}
				}
			}
		}
		if (activeGrid)
			firstPosition = posicaoCalibrada;*/
	}
	
	public void Redo ()
	{
		if (redoCreatedWalls.Count == 0)
			return;
			
		GameObject redoWall = redoCreatedWalls.Pop ();
		lastCreatedWalls.Push (redoWall);
		
		redoWall.SetActiveRecursively(true);
		firstPosition = redoWall.transform.position + Vector3.RotateTowards((Vector3.forward * redoWall.transform.localScale.x),Vector3.up,redoWall.transform.eulerAngles.y,100);
		firstPosition.y = 0; 
	}
	
	public void Undo ()
	{
		if (lastCreatedWalls.Count == 0)
			return;
			
		GameObject lastWall = lastCreatedWalls.Pop ();
		redoCreatedWalls.Push (lastWall);
		lastWall.SetActiveRecursively(false);
		lastWall = lastCreatedWalls.Peek ();
		firstPosition = lastWall.transform.position + Vector3.RotateTowards ((Vector3.forward * lastWall.transform.localScale.x), Vector3.up, lastWall.transform.eulerAngles.y, 100);
		firstPosition.y = 0; 
	}
	
	public void ClearCaches ()
	{
		GameObject cWall = null;
		while (lastCreatedWalls.Count != 0)
		{
			cWall = lastCreatedWalls.Pop();
			DestroyImmediate (cWall);
		}
		while (redoCreatedWalls.Count != 0)
		{
			cWall = redoCreatedWalls.Pop();
			DestroyImmediate (cWall);
		}
		
		Debug.LogWarning ("lastCreatedWalls.Count: " + lastCreatedWalls.Count);
		Debug.LogWarning ("redoCreatedWalls.Count: " + redoCreatedWalls.Count);
	}
	
	public void DestroyTile ()
	{
		Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.transform.tag == "Chao") {
				Destroy (hit.transform.gameObject);
			}
		}
	}
	
	public void BuildGround (){
	/*
		Vector3[] vertices = new Vector3[lastCreatedWalls.Count];
		Vector3[] normals  = new Vector3[lastCreatedWalls.Count];
		int[] triangles    = new int[lastCreatedWalls.Count * 3];
		Vector2[] uvs	   = new Vector2[lastCreatedWalls.Count * 3];
		
		GameObject cWall = null;
		for (int i = 0, j = 3; lastCreatedWalls.Count != 0; ++i, j += 3 )
		{
			cWall = lastCreatedWalls.Pop ();
			vertices[i] = cWall.transform.position;
			normals[i]  = Vector3.up;
			DestroyImmediate (cWall);
			
			if ( lastCreatedWalls.Count > 2)
			{
				Debug.LogWarning ("triangles.Length: " + triangles.Length);
				Debug.LogWarning ("j : " + j);
			
				triangles [j] = 0;
				triangles [j + 1] = i + 1;
				triangles [j + 2] = i + 2;
			}
		}
		
		
//		Debug.LogWarning ("triangles[triangles.Length - 1]: " + triangles [triangles.Length - 1]);
		Mesh groundMesh = new Mesh();
		groundMesh.vertices = vertices;
		groundMesh.normals  = normals;
		groundMesh.triangles= triangles;
		
		GameObject a = (new GameObject("teste-chao") as GameObject);
		a.transform.position = WallBuilder.ROOT;
		a.AddComponent<MeshFilter>().mesh = groundMesh;
		a.AddComponent<MeshRenderer>().material = asa;*/
		
		RemoveGround();
/*
		for (int z = 0; z != WallDepth; ++z)
		{
			for (int x = 0; x != WallWidth; ++x)
			{
				GameObject newTile = Instantiate (floor, Vector3.zero, floor.transform.rotation) as GameObject;
				newTile.transform.position = ROOT + new Vector3 ((int)(x - (WallWidth / 2)), 0.01f, (int)(z - (WallDepth / 2)));
				newTile.transform.parent = parentFloor;
			}
		}
*/		
		GameObject newTile = Instantiate (floor, Vector3.zero, floor.transform.rotation) as GameObject;
		newTile.transform.position = WallBuilder.ROOT + new Vector3 ( (int)(- ( WallWidth / 2) ) - 0.5f ,0.1f, (int)( WallDepth / 2) + 0.5f );// + new Vector3 (- (WallWidth / 2 + 0.5f) , 0.01f, - (WallDepth / 2 + 0.5f));
		newTile.transform.parent = parentFloor;
		newTile.transform.localScale = new Vector3(WallWidth, WallDepth, 1);	
		foreach (Material cMaterial in newTile.renderer.materials)
		{
			Debug.LogWarning ("materials - cMaterial.name: " + cMaterial.name);
			cMaterial.mainTextureScale = new Vector2 (WallWidth, WallDepth);
			cMaterial.SetTextureScale ("_BumpMap", new Vector2 (WallWidth, WallDepth));
		}
	}
	
	public void BuildWalls (){
		
		RemoveRoof();
		RemoveWalls();
		
		CreateRoof();
		
		GameObject floor = GameObject.FindWithTag ("Chao");
		Vector3 floorSizeOffset = floor.transform.localScale;
		Vector3 floorPosition   = floor.transform.localPosition;
		floorSizeOffset.z = floorSizeOffset.y;
		floorSizeOffset.y = 0;
		Debug.LogWarning ("floorSizeOffset: " + floorSizeOffset);
		
		GameObject newWall;
		
		//0
		newWall = Instantiate (	wall,
								floorPosition + (Vector3.right * floorSizeOffset.x), 
								Quaternion.Euler (Vector3.up * 0.0f)) as GameObject;
		ChangeWalMaterialAndScale (newWall, WallWidth);
		
		//90
		newWall = Instantiate (	wall,
								floorPosition
									+ (Vector3.right * floorSizeOffset.x) 
										- (Vector3.forward * floorSizeOffset.z),
								Quaternion.Euler (Vector3.up * 90.0f)) as GameObject;
		ChangeWalMaterialAndScale (newWall, WallDepth);
		
		//180
		newWall = Instantiate (	wall,
								floorPosition - (Vector3.forward * floorSizeOffset.z),
								Quaternion.Euler (Vector3.up * 180.0f)) as GameObject;
		ChangeWalMaterialAndScale (newWall, WallWidth);
				
		//270
		newWall = Instantiate (	wall,
								floorPosition,
								Quaternion.Euler (Vector3.up * 270.0f)) as GameObject;
		ChangeWalMaterialAndScale (newWall, WallDepth);
		
		/*
		GameObject[] pisos = GameObject.FindGameObjectsWithTag ("Chao");
		if (pisos.Length == 0) 
		{
			Debug.LogWarning ("Não existe chão! Por isso não pode ser criado paredes.");
			return;
		}
		foreach (GameObject piso in pisos) {
			Vector3[] frontalDirections = new Vector3[] {
				Vector3.forward + Vector3.right, Vector3.forward + Vector3.left, Vector3.forward
			};
			Vector3[] backwardDirections = new Vector3[] {
				Vector3.back + Vector3.right, Vector3.back + Vector3.left, Vector3.back,
			};
			Vector3[] sides = new Vector3[] {
				Vector3.right, Vector3.left
			};
			
			#region Frontal Search
			List<Vector3> frontalDirectionsAccepted = new List<Vector3>();
			foreach (Vector3 direction in frontalDirections) {
				RaycastHit hit;
#if DRAW_RAY
				Debug.DrawRay(piso.transform.position, 
				              direction, Color.blue);
#endif          
				if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
				                    direction - new Vector3(0, 0.25f, 0),
				                    out hit)) {
					if (hit.transform.tag != "Chao") {
						frontalDirectionsAccepted.Add(direction);
					}
				}
			}
			if (frontalDirectionsAccepted.Count	!= 0) {
				if (frontalDirectionsAccepted.Count	== 3) {
					foreach (Vector3 direction in sides) {
						RaycastHit hit;
#if DRAW_RAY
						Debug.DrawRay(piso.transform.position, 
						              direction, Color.red);
#endif
						if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
						                    direction - new Vector3(0, 0.25f, 0),
						                    out hit)) {
							if (hit.transform.tag != "Chao") {
								if (direction == Vector3.right) {
									CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
													new Vector3 (0.5f, 0, 0.5f),
						               				parentWalls,
						               				new Vector3 (0, 90, 0),
													true);
								}
								if (direction == Vector3.left) {
									CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
													new Vector3 (-0.5f, 0, 0.5f),
													parentWalls,
													new Vector3 (0, 0, 0),
													true);
								}
							}
						}
					}
				}
				else if (frontalDirectionsAccepted.Count == 2) {
					if (frontalDirectionsAccepted[0] == (Vector3.forward + Vector3.right) && 
					    frontalDirectionsAccepted[1] == Vector3.forward) {
#if DRAW_RAY
						Debug.DrawRay(piso.transform.position,
						              sides[0], Color.cyan);
#endif
						RaycastHit hit;
						if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
							                    sides[0] - new Vector3(0, 0.25f, 0),
							                    out hit)) {
							if (hit.transform.tag != "Chao") {
								CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
												new Vector3 (-0.5f, 0, -0.5f),
												parentWalls,
												new Vector3 (0, 270, 0),
												false);
								CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
												new Vector3 (0.5f, 0, 0.5f),
					               				parentWalls,
					               				new Vector3 (0, (int)90, 0),
												true);
							}
							else {
								CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
												new Vector3 (-0.5f, 0, -0.5f),
												parentWalls,
												new Vector3 (0, 270, 0),
												false);
							}
						}
					}
					else if (frontalDirectionsAccepted[0] == (Vector3.forward + Vector3.left) && 
					    	 frontalDirectionsAccepted[1] == Vector3.forward) {
#if DRAW_RAY
						Debug.DrawRay(piso.transform.position,
						              sides[1], Color.cyan);
#endif
						RaycastHit hit;
						if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
							                    sides[1] - new Vector3(0, 0.25f, 0),
							                    out hit)) {
							if (hit.transform.tag != "Chao") {
								CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
												new Vector3 (0.5f, 0, -0.5f),
					               				parentWalls,
					               				new Vector3 (0, 180, 0),
												false);
								CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
												new Vector3 (-0.5f, 0, 0.5f),
												parentWalls,
												new Vector3 (0, 0, 0),
												true);
							}
							else {
								CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
												new Vector3 (0.5f, 0, -0.5f),
					               				parentWalls,
					               				new Vector3 (0, 180, 0),
												false);
							}
						}
					}
				}
				else if (frontalDirectionsAccepted.Count == 1) {
					if (frontalDirectionsAccepted[0] == Vector3.forward) {
						CreateCorner(piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
									   new Vector3 (0.5f, 0, -0.5f),
						               parentWalls,
						               new Vector3 (0, 180, 0),
									   false);
						CreateCorner(piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
									   new Vector3 (-0.5f, 0, -0.5f),
						               parentWalls,
						               new Vector3 (0, 270, 0),
									   false);
					}
				}
			}
			#endregion
			
			#region Backward Search
			List<Vector3> backwardDirectionsAccepted = new List<Vector3>();
			foreach (Vector3 direction in backwardDirections) {
				RaycastHit hit;
#if DRAW_RAY
				Debug.DrawRay(piso.transform.position, 
				              direction, Color.blue);
#endif
				if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
				                    direction - new Vector3(0, 0.25f, 0),
				                    out hit)) {
					if (hit.transform.tag != "Chao") {
						backwardDirectionsAccepted.Add(direction);
					}
				}
			}
			if (backwardDirectionsAccepted.Count != 0) {
				if (backwardDirectionsAccepted.Count == 3) {
					foreach (Vector3 direction in sides) {
						RaycastHit hit;
#if DRAW_RAY
						Debug.DrawRay(piso.transform.position, 
						              direction, Color.red);
#endif
						if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
						                    direction - new Vector3(0, 0.25f, 0),
						                    out hit)) {
							if (hit.transform.tag != "Chao") {
								if (direction == Vector3.right) {
									CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
													new Vector3 (0.5f, 0, -0.5f),
						               				parentWalls,
						               				new Vector3 (0, 180, 0),
													true);
								}
								if (direction == Vector3.left) {
									CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
													new Vector3 (-0.5f, 0, -0.5f),
													parentWalls,
													new Vector3 (0, 270, 0),
													true);
								}
							}
						}
					}
				}
				else if (backwardDirectionsAccepted.Count == 2) {
					if (backwardDirectionsAccepted[0] == (Vector3.back + Vector3.right) && 
					    backwardDirectionsAccepted[1] == Vector3.back) {
#if DRAW_RAY
						Debug.DrawRay(piso.transform.position,
						              sides[0], Color.cyan);
#endif
						RaycastHit hit;
						if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
							                    sides[0] - new Vector3(0, 0.25f, 0),
							                    out hit)) {
							if (hit.transform.tag != "Chao") {
								CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
												new Vector3 (-0.5f, 0, 0.5f),
												parentWalls,
												new Vector3 (0, 0, 0),
												false);
								CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
												new Vector3 (0.5f, 0, -0.5f),
					               				parentWalls,
					               				new Vector3 (0, 180, 0),
												true);
							}
							else {
								CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
												new Vector3 (-0.5f, 0, 0.5f),
												parentWalls,
												new Vector3 (0, 0, 0),
												false);
							}
						}
					}
					else if (backwardDirectionsAccepted[0] == (Vector3.back + Vector3.left) && 
					    	 backwardDirectionsAccepted[1] == Vector3.back) {
#if DRAW_RAY
						Debug.DrawRay(piso.transform.position,
						              sides[1], Color.cyan);
#endif
						RaycastHit hit;
						if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
							                    sides[1] - new Vector3(0, 0.25f, 0),
							                    out hit)) {
							if (hit.transform.tag != "Chao") {
								CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
												new Vector3 (0.5f, 0, 0.5f),
					               				parentWalls,
					               				new Vector3 (0, 90, 0),
												false);
								CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
												new Vector3 (-0.5f, 0, -0.5f),
												parentWalls,
												new Vector3 (0, 270, 0),
												true);
							}
							else {
								CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
												new Vector3 (0.5f, 0, 0.5f),
					               				parentWalls,
					               				new Vector3 (0, 90, 0),
												false);
							}
						}
					}
				}
				else if (backwardDirectionsAccepted.Count == 1) {
					if (backwardDirectionsAccepted[0] == Vector3.back) {
						CreateCorner(piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
						               new Vector3 (0.5f, 0, 0.5f),
						               parentWalls,
						               new Vector3 (0, 90, 0),
									   false);
						CreateCorner(piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
						               new Vector3 (-0.5f, 0, 0.5f),
						               parentWalls,
						               new Vector3 (0, 0, 0),
									   false);
					}
				}
			}
			#endregion
			
			GameObject cf = new GameObject("ChaoFantasma");
			cf.transform.position = piso.transform.position;
			cf.tag = "ChaoVazio";
			cf.transform.parent = parentFloor;
			
		}
		
		GameObject[] corners = GameObject.FindGameObjectsWithTag("Quina");
		if (corners.Length == 0)
		{
			Debug.LogError("Algo ocorreu de forma equivocada!");
		}
		
		bool[] alreadyPlaced = new bool[corners.Length];
		
		Debug.LogWarning ("corners.Length: " + corners.Length);
		for (int i = 0; i != corners.Length; ++i)
		{
//			corners[i].tag = "Parede";
			
			for (int j = 0; j != corners.Length; ++j)
			{
				if (i != j && !alreadyPlaced [j])
				{
					if (corners [i].transform.position.x == corners [j].transform.position.x ||
						corners [i].transform.position.z == corners [j].transform.position.z)
					{
						CreateWall (corners [i].transform.position, corners [j].transform.position);
					}
				}
			}
			
			alreadyPlaced [i] = true;
		}
		
		float angleBetweenWalls;
			
		*/
		//Inicializando InfoWall em cada parede
		foreach (GameObject cWall in GameObject.FindGameObjectsWithTag("Parede"))
		{
			if (cWall.GetComponent<InfoWall> () == null) {
				cWall.AddComponent<InfoWall> ();
			}
			cWall.GetComponent<InfoWall> ().color = Color.white;
			cWall.GetComponent<InfoWall> ().wall  = cWall.transform;
		
			/*
			foreach (GameObject cWall in GameObject.FindGameObjectsWithTag("Parede"))
			{
				if (wall.Equals (cWall))
					continue;
					
				angleBetweenWalls = (wall.transform.eulerAngles.y - cWall.transform.eulerAngles.y);	
				
				if ((angleBetweenWalls - 90f) <  IPSLON &&
					(angleBetweenWalls - 90f) > -IPSLON)
				{
					if (cWall.GetComponent<InfoWall> () == null){
						cWall.AddComponent<InfoWall> ();
					}
					
					wall.GetComponent<InfoWall> ().rightWall = cWall.transform;
					cWall.GetComponent<InfoWall> ().leftWall = wall.transform;
					break;
				}
			}	
		*/
		}
		/*
		//Preencher leftWall/rightWall que ficaram de fora
		foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Parede"))
		{
			if (wall.GetComponent<InfoWall> ().rightWall == null)
			{
				foreach (GameObject cWall in GameObject.FindGameObjectsWithTag("Parede"))
				{
					if (cWall.GetComponent<InfoWall> ().leftWall == null)
					{
						wall.GetComponent<InfoWall> ().rightWall = cWall.transform;
						cWall.GetComponent<InfoWall> ().leftWall = wall.transform;
						break;
					}
				}
				break;
			}
		}
		
		//removendo quinas fantasma
		GameObject[] ghostCorners = GameObject.FindGameObjectsWithTag("QuinaVazia");
		foreach (GameObject ghostCorner in ghostCorners)
		{
			Destroy (ghostCorner);
		}
			
		*/
		
		Application.LoadLevel(4);
		
	}
	
	private void ChangeWalMaterialAndScale (GameObject newWall, float wallScaleX)
	{
		newWall.transform.localScale = new Vector3 (wallScaleX, 1, 1);
				
		foreach (Material cMaterial in newWall.transform.GetChild(0).renderer.materials)
		{
			cMaterial.mainTextureScale = new Vector2 (wallScaleX, 1);
			cMaterial.SetTextureScale ("_BumpMap", new Vector2 (wallScaleX, 1));
		}
		
		newWall.transform.parent = parentWalls.transform;
	}
	
	private GameObject CreateWall ( Vector3 initialPosition, Vector3 finalPosition )
	{
		float wallAngle = Vector3.Angle (Vector3.right, (finalPosition - initialPosition));
		Debug.LogWarning ("wallAngle: " + wallAngle);
		
//		if (initialPosition.z > finalPosition.z)
//			wallAngle += 180f;
//		else 
//			wallAngle += 180f;
		
		Vector3 a;
		if (initialPosition.x > finalPosition.x)
		{
			a = initialPosition;
		}
		else
		{
			a = finalPosition;
		}
			
		GameObject newWall = Instantiate (wall, a, Quaternion.Euler(new Vector3(0,wallAngle,0))) as GameObject;
		 
		float wallScaleX = Vector3.Distance (finalPosition, initialPosition);
		newWall.transform.localScale = new Vector3 (wallScaleX, 1, 1);
				
		foreach (Material cMaterial in newWall.transform.GetChild(0).renderer.materials)
		{
			cMaterial.mainTextureScale = new Vector2 (wallScaleX, 1);
			cMaterial.SetTextureScale ("_BumpMap", new Vector2 (wallScaleX, 1));
		}
		
		newWall.transform.parent = parentWalls.transform;
		
		/*
		//Testando para ver se há uma parede exatamente sobre o mesmo lugar
		GameObject[] walls = GameObject.FindGameObjectsWithTag("Parede");
		foreach (GameObject cWall in walls)
		{
			if (cWall.Equals (newWall))
				continue;
			
			if (Vector3.Distance(cWall.transform.position, newWall.transform.position) < WallBuilder.IPSLON)
			{
				//achou parede coincidente
				newWall.transform.localEulerAngles = Vector3.up * (wallAngle + 180f);
				newWall.transform.position		   = newWall.transform.position + (newWall.transform.right.normalized * wallScaleX);
				break;
			}
		}*/
		
		return newWall;
	}
	
	public void Restart ()
	{
		RemoveWalls ();
		RemoveGround ();
		RemoveRoof ();
	}
	
	private void MovCamera ()
	{
		mov += new Vector3 ((Input.GetAxis ("Horizontal") * Time.deltaTime) * speedCam, 0, (Input.GetAxis ("Vertical") * Time.deltaTime) * speedCam);
		mov.x = Mathf.Clamp(mov.x, -20.5f, 20.5f);
		mov.z = Mathf.Clamp(mov.z, -15.5f, 15.5f);		
		transform.position = mov;
		zoom -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomAngle * Mathf.Abs(zoom);
		zoom = Mathf.Clamp(zoom, 2.5f, 15f);
		camera.orthographicSize = zoom;
	}
	
	private void VerifyGroundShift ()
	{
		Ray ray = camera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.transform.tag == "Grid") {
				float x, y, z;
				
				x = ray.origin.x;
				y = 0.01f;
				z = ray.origin.z;
				x = x - (int)x > 0.5f ? (int)x + 1 : x - (int)x < -0.5f ? (int)x - 1 : (int)x;
				z = z - (int)z > 0.5f ? (int)z + 1 : z - (int)z < -0.5f ? (int)z - 1 : (int)z;
				
				Vector3 posicaoCalibrada = new Vector3 (x, y, z);
				float xs = firstPosition.x, zs = firstPosition.z;
				if (xs > posicaoCalibrada.x)
					xs = posicaoCalibrada.x;
				if (xs < posicaoCalibrada.x)
					xs = posicaoCalibrada.x;
				if (zs > posicaoCalibrada.z)
					zs = posicaoCalibrada.z;
				if (zs < posicaoCalibrada.z)
					zs = posicaoCalibrada.z;
				
				for (int newX = (int)firstPosition.x, newZ = (int)firstPosition.z; newX < (int)xs && newZ < (int)zs; ++newX, ++newZ) {
					Vector3 posicaoArray = new Vector3 (newX, y, newZ);
					RaycastHit hit2;
					if (Physics.Raycast (posicaoArray + new Vector3 (0, 0.01f, 0), Vector3.down, out hit2, 1.0f)) {
#if DRAW_RAY
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
#endif
						if (hit2.transform.tag != "Chao") {
							GameObject novoChao = Instantiate (floor, posicaoArray, floor.transform.rotation) as GameObject;
							novoChao.transform.parent = parentFloor;
							novoChao.renderer.material.color = new Color (0, 1, 0, 1);
						}
					}
				}
				for (int newX = (int)firstPosition.x, newZ = (int)firstPosition.z; newX > (int)xs && newZ > (int)zs; --newX, --newZ) {
					Vector3 posicaoArray = new Vector3 (newX, y, newZ);
					RaycastHit hit2;
					if (Physics.Raycast (posicaoArray + new Vector3 (0, 0.01f, 0), Vector3.down, out hit2, 1.0f)) {
#if DRAW_RAY
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
#endif
						if (hit2.transform.tag != "Chao") {
							GameObject novoChao = Instantiate (floor, posicaoArray, floor.transform.rotation) as GameObject;
							novoChao.transform.parent = parentFloor;
							novoChao.renderer.material.color = new Color (0, 1, 0, 1);
						}
					}
				}
				for (int newX = (int)firstPosition.x; newX < (int)xs; ++newX) {
					Vector3 posicaoArray = new Vector3 (newX, y, zs);
					RaycastHit hit2;
					if (Physics.Raycast (posicaoArray + new Vector3 (0, 0.01f, 0), Vector3.down, out hit2, 1.0f)) {
#if DRAW_RAY
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
#endif
						if (hit2.transform.tag != "Chao") {
							GameObject novoChao = Instantiate (floor, posicaoArray, floor.transform.rotation) as GameObject;
							novoChao.transform.parent = parentFloor;
							novoChao.renderer.material.color = new Color (0, 1, 0, 1);
						}
					}
				}
				for (int newX = (int)firstPosition.x; newX > (int)xs; --newX) {
					Vector3 posicaoArray = new Vector3 (newX, y, zs);
					RaycastHit hit2;
					if (Physics.Raycast (posicaoArray + new Vector3 (0, 0.01f, 0), Vector3.down, out hit2, 1.0f)) {
#if DRAW_RAY
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
#endif
						if (hit2.transform.tag != "Chao") {
							GameObject novoChao = Instantiate (floor, posicaoArray, floor.transform.rotation) as GameObject;
							novoChao.transform.parent = parentFloor;
							novoChao.renderer.material.color = new Color (0, 1, 0, 1);
						}
					}
				}
				for (int newZ = (int)firstPosition.z; newZ < (int)zs; ++newZ) {
					Vector3 posicaoArray = new Vector3 (xs, y, newZ);
					RaycastHit hit2;
					if (Physics.Raycast (posicaoArray + new Vector3 (0, 0.01f, 0), Vector3.down, out hit2, 1.0f)) {
#if DRAW_RAY
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
#endif
						if (hit2.transform.tag != "Chao") {
							GameObject novoChao = Instantiate (floor, posicaoArray, floor.transform.rotation) as GameObject;
							novoChao.transform.parent = parentFloor;
							novoChao.renderer.material.color = new Color (0, 1, 0, 1);
						}
					}
				}
				for (int newZ = (int)firstPosition.z; newZ > (int)zs; --newZ) {
					Vector3 posicaoArray = new Vector3 (xs, y, newZ);
					RaycastHit hit2;
					if (Physics.Raycast (posicaoArray + new Vector3 (0, 0.01f, 0), Vector3.down, out hit2, 1.0f)) {
#if DRAW_RAY
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
#endif
						if (hit2.transform.tag != "Chao") {
							GameObject novoChao = Instantiate (floor, posicaoArray, floor.transform.rotation) as GameObject;
							novoChao.transform.parent = parentFloor;
							novoChao.renderer.material.color = new Color (0, 1, 0, 1);
						}
					}
				}
			}
		}
	}
	
	private void RemoveGround() {
		GameObject[] pisos = GameObject.FindGameObjectsWithTag ("Chao");
		if (pisos.Length > 0) {
			foreach (GameObject piso in pisos)
				DestroyImmediate (piso);
		}
	}
	
	private void RemoveRoof () {
		GameObject[] ceil = GameObject.FindGameObjectsWithTag ("Teto");
		if (ceil.Length > 0) {
			foreach (GameObject t in ceil)
				DestroyImmediate (t);
		}
	}
	
	private void RemoveWalls () {
		GameObject[] paredes = GameObject.FindGameObjectsWithTag ("Parede");
		if (paredes.Length > 0) {
			foreach (GameObject wall in paredes)
				DestroyImmediate (wall);
		}
		GameObject[] quinas = GameObject.FindGameObjectsWithTag ("Quina");
		if (quinas.Length > 0) {
			foreach (GameObject corner in quinas)
				DestroyImmediate (corner);
		}
	}
	
	private void CreateRoof () {
		GameObject newCeil = Instantiate(ceil, Vector3.zero, ceil.transform.rotation) as GameObject;	
		newCeil.transform.position = WallBuilder.ROOT + new Vector3 ( (int)(- ( WallWidth / 2) ) - 0.5f , 2.5f , (int)( WallDepth / 2) + 0.5f );// + new Vector3 (- (WallWidth / 2 + 0.5f) , 0.01f, - (WallDepth / 2 + 0.5f));
		newCeil.transform.parent = parentCeil;
		newCeil.transform.localScale = new Vector3(WallWidth, WallDepth, 1);	
		foreach (Material cMaterial in newCeil.renderer.materials)
		{
			Debug.LogWarning ("materials - cMaterial.name: " + cMaterial.name);
			cMaterial.mainTextureScale = new Vector2 (WallWidth, WallDepth);
			cMaterial.SetTextureScale ("_BumpMap", new Vector2 (WallWidth, WallDepth));
		}
	}
	
	private void CreateCorner (Vector3 position, Vector3 sum, Transform side, Vector3 eulerAngles, bool createEmptyCinchona) {
		if (!SamePosition("Quina", position)) {
			GameObject novaQuina;
			if (createEmptyCinchona) {
				GameObject qf = new GameObject("QuinaFantasma");
				qf.transform.position = position + (-(sum / 8));
				qf.tag = "QuinaVazia";
				qf.transform.parent = side;
			}
			novaQuina = Instantiate (corner, position, corner.transform.rotation) as GameObject;
			novaQuina.transform.parent = side;
			novaQuina.transform.eulerAngles = eulerAngles;
		}
	}
	
	private bool SamePosition (string tag, Vector3 position) {
		GameObject[] novas = GameObject.FindGameObjectsWithTag (tag);
		foreach (GameObject n in novas) {
			if (position == n.transform.position) {
				return true;
			}
		}
		return false;
	}
}
