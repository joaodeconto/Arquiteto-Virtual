//#define DRAW_RAY
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallBuilder : MonoBehaviour {
	
	public const  float IPSLON 		= 0.0001f;
	public static float WALL_HEIGHT = 2.6f;

	public float WallHeight;

	#region Parents
	public Transform 	parentFloor;
	public Transform 	parentCeil;
	public Transform	parentWalls;
	#endregion
	
	#region Prefabs references
	public Camera 		mainCamera;
	public GameObject 	grid;
	public GameObject 	floor;
	public GameObject 	wall;
	public GameObject 	ceil;
	#endregion
	
	#region Rulers
	public LineRenderer lineDepth;
	public LineRenderer lineWidth;
	#endregion
	
	#region Camera Data
	private GameObject 	scenario;
	
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
	public float 	RealWallWidth { get; set; }
	public float 	RealWallDepth  { get; set; }
	
	public int 		MaxWallWidth { get; private set; }
	public int 		MaxWallDepth  { get; private set; }
	public int 		MinWallWidth { get; private set; }
	public int 		MinWallDepth  { get; private set; }
	#endregion
	
	#region WorkAround to resolve NGUI bug: Move object to negative positions crashes NGUI functionality
	public static Vector3 ROOT { get; private set; }
	#endregion
	
	// AsyncOperation to loading a scene
	private AsyncOperation ao;
	
	#region Unty Methods
	void Start()
	{
		if (Mathf.Approximately(WallHeight,0) || WallHeight < 0)
		{
			Debug.LogError ("WallHeight não é um número positivo.");
			WallHeight = WALL_HEIGHT;
		}
		else
		{
			WALL_HEIGHT = WallHeight;
		}

		WallWidth = 5;
		WallDepth = 5;
		
		MaxWallWidth = 100;
		MaxWallDepth = 100;
		
		MinWallWidth = 30;
		MinWallDepth = 30;
		
		//ROOT = new Vector3(1000,0,1000);
		
		scenario = GameObject.Find ("Scenario");
//		ROOT = scenario.transform.root != null ? scenario.transform.root.position : scenario.transform.position;
		ROOT = scenario.transform.position;
		
		mov = transform.position;
		zoom = GameObject.FindWithTag("MainCamera").camera.orthographicSize;
	}
	
	public float percentageLoaded = 0;
	
#if UNITY_WEBPLAYER	&& !UNITY_EDITOR
    void Update()
	{
        if (Application.GetStreamProgressForLevel(1) != 1)
		{
            percentageLoaded = Application.GetStreamProgressForLevel(1);
        }
    }
#endif
#if !UNITY_WEBPLAYER || UNITY_EDITOR
	
	void OnGUI ()
	{
		if (ao != null)
		{
			GUI.Box(new Rect((Screen.width / 2) - 100f, (Screen.height / 2) - 12.5f, 200f, 25f), "Carregando: " + (int)(ao.progress * 100f) + "%");
		}
	}
	
#endif
	#endregion
		
	public void BuildGround (){
	
		RemoveGround();

		GameObject newTile = Instantiate (floor, Vector3.zero, Quaternion.identity) as GameObject;
		newTile.transform.parent   = parentFloor;
//		newTile.transform.localPosition = scenario.transform.position + new Vector3 (- (RealWallDepth / 2) , 0.0f, (RealWallWidth / 2));/*+ new Vector3 ( (int)(- ( RealWallWidth / 2) ) - 0.5f ,0.0f, (int)( RealWallDepth / 2) + 0.5f );*/
		newTile.transform.localPosition = (transform.right * (- (RealWallWidth / 2)) + (transform.forward * (RealWallDepth / 2)));
		newTile.transform.localEulerAngles = transform.eulerAngles;
		newTile.transform.localEulerAngles += floor.transform.localEulerAngles;
		newTile.transform.localScale = new Vector3(RealWallWidth, RealWallDepth, 1);
		foreach (Material cMaterial in newTile.renderer.materials)
		{
//			Debug.LogWarning ("materials - cMaterial.name: " + cMaterial.name);
			float a = Mathf.CeilToInt(RealWallWidth) - RealWallWidth;
			float b = Mathf.CeilToInt(RealWallDepth) - RealWallDepth;

			cMaterial.mainTextureScale  = new Vector2 (RealWallWidth, RealWallDepth);
			cMaterial.mainTextureOffset = new Vector2 (a, 0);
			cMaterial.SetTextureScale  ("_BumpMap", new Vector2 (RealWallWidth, RealWallDepth));
			cMaterial.SetTextureOffset ("_BumpMap", new Vector2 (a, 0));
		}
		
		#region Line Renderer Ruler
		
		float offset = 1f;
		
		lineDepth.SetPosition (0, (Vector3.forward * -(RealWallDepth - offset) / 2) + (Vector3.right * (RealWallWidth / 2 + 1f)));
		lineDepth.SetPosition (1, (Vector3.forward * (RealWallDepth - offset) / 2) + (Vector3.right * (RealWallWidth / 2 + 1f)));
		lineDepth.transform.GetChild(0).localPosition = new Vector3((RealWallWidth / 2) + 1f, 0, -RealWallDepth / 2);
		lineDepth.transform.GetChild(1).localPosition = new Vector3((RealWallWidth / 2) + 1f, 0, RealWallDepth / 2);
		
		lineWidth.SetPosition (0, (Vector3.forward * (RealWallDepth / 2 + 1f)) + (Vector3.right * -(RealWallWidth - offset) / 2));
		lineWidth.SetPosition (1, (Vector3.forward * (RealWallDepth / 2 + 1f)) + (Vector3.right * (RealWallWidth - offset) / 2));
		lineWidth.transform.GetChild(0).localPosition = new Vector3(RealWallWidth / 2, 0, (RealWallDepth / 2) + 1f);
		lineWidth.transform.GetChild(1).localPosition = new Vector3(-RealWallWidth / 2, 0, (RealWallDepth / 2) + 1f);
		#endregion
	}
	
	public void BuildWalls (){
		
		RemoveRoof();
		RemoveWalls();
		
		if (scenario.transform.parent != null)
		{
			scenario.transform.parent = null;
			scenario.transform.localPosition = Vector3.zero;
			scenario.transform.localRotation = Quaternion.identity;
			scenario.transform.localScale = Vector3.one;
		}
		
		//TODO Por enquanto só fazendo inversão dos valores
		float temp = RealWallWidth;
		RealWallWidth = RealWallDepth;
		RealWallDepth = temp;
		
		CreateRoof();
		
		GameObject floor = GameObject.FindWithTag ("Chao");
		Vector3 floorSizeOffset = floor.transform.localScale;
		Vector3 floorPosition   = floor.transform.position;
		floorSizeOffset.z = floorSizeOffset.y;
		floorSizeOffset.y = 0;

		GameObject newWall;
		float factor = 0.1f;
		float semifactor = factor / 2.0f;

		//0
		newWall = Instantiate (wall,
								floorPosition
									+ (Vector3.right * floorSizeOffset.x)
									- (Vector3.forward * (floorSizeOffset.z + semifactor)),
								Quaternion.Euler (Vector3.up * 0.0f)) as GameObject;
		ChangeWalMaterialAndScale (newWall, RealWallWidth + factor);
		newWall.name = "Front Wall";

		//90
		newWall = Instantiate (wall,
								floorPosition
									- (Vector3.right * semifactor)
									- (Vector3.forward * floorSizeOffset.z),
								Quaternion.Euler (Vector3.up * 90.0f)) as GameObject;
		ChangeWalMaterialAndScale (newWall, RealWallDepth + factor);
		newWall.name = "Left Wall";

		//180
		newWall = Instantiate (	wall,
								floorPosition
									+ (Vector3.forward * semifactor),
								Quaternion.Euler (Vector3.up * 180.0f)) as GameObject;
		ChangeWalMaterialAndScale (newWall, RealWallWidth + factor);
		newWall.name = "Back Wall";

		//270
		newWall = Instantiate (	wall,
								floorPosition
									+ (Vector3.right * (floorSizeOffset.x + semifactor)),
								Quaternion.Euler (Vector3.up * 270.0f)) as GameObject;
		ChangeWalMaterialAndScale (newWall, RealWallDepth + factor);
		newWall.name = "Right Wall";

		//Inicializando InfoWall em cada parede
		foreach (GameObject cWall in GameObject.FindGameObjectsWithTag("Parede"))
		{
			cWall.GetComponent<InfoWall> ().color  	= Color.white;
			cWall.GetComponent<InfoWall> ().wall  	= cWall.transform;
			cWall.GetComponent<InfoWall> ().texture = cWall.transform.
																GetChild(0).
																	renderer.
																		materials[0].mainTexture;
		}

		int depth = (int)RealWallDepth;
		int width = (int)RealWallWidth;
		
		for (int i = 0; i != depth; ++i)
		{
			for (int j = 0; j != width; ++j)
			{
				GameObject emptyFloor = new GameObject();//GameObject.CreatePrimitive (PrimitiveType.Sphere);
				emptyFloor.name = "EmptyFloor";
				emptyFloor.transform.position = WallBuilder.ROOT
											 + Vector3.forward * Mathf.Ceil(i - depth / 2.0f)
											 + Vector3.right   * Mathf.Ceil(j - width / 2.0f);

				emptyFloor.tag = "ChaoVazio";
				emptyFloor.transform.parent = parentFloor;
			}
		}
		
#if UNITY_WEBPLAYER	&& !UNITY_EDITOR
		if (Application.CanStreamedLevelBeLoaded(1))
		{
			Application.LoadLevel(1);
	    }
#endif
#if !UNITY_WEBPLAYER || UNITY_EDITOR
		ao = Application.LoadLevelAsync(1);
#endif		
		
	}
	
	private void ChangeWalMaterialAndScale (GameObject newWall, float wallScaleX)
	{
		newWall.transform.localScale = new Vector3 (wallScaleX, WallBuilder.WALL_HEIGHT, 1);

		Vector2 textScale = new Vector2 (wallScaleX, WallBuilder.WALL_HEIGHT);
		foreach (Material cMaterial in newWall.transform.GetChild(0).renderer.materials)
		{
			cMaterial.mainTextureScale = textScale;
			cMaterial.SetTextureScale ("_BumpMap", textScale);
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
	
	private void CreateRoof ()
	{
		GameObject newCeil = Instantiate(ceil, Vector3.zero, ceil.transform.rotation) as GameObject;	
		/*newCeil.transform.position = WallBuilder.ROOT + new Vector3 ( (int)(- ( RealWallWidth / 2) ) - 0.5f ,
																				WallBuilder.WALL_HEIGHT ,
																	  (int)(  RealWallDepth / 2)   + 0.5f );*/
		newCeil.transform.parent = parentCeil;
		newCeil.transform.localPosition = new Vector3 (- (RealWallWidth / 2) , 
													WallBuilder.WALL_HEIGHT, 
													(RealWallDepth / 2));
		newCeil.transform.localScale = new Vector3(RealWallWidth, RealWallDepth, 1);	
		foreach (Material cMaterial in newCeil.renderer.materials)
		{
			Debug.LogWarning ("materials - cMaterial.name: " + cMaterial.name);
			cMaterial.mainTextureScale = new Vector2 (RealWallWidth, RealWallDepth);
			cMaterial.SetTextureScale ("_BumpMap", new Vector2 (RealWallWidth, RealWallDepth));
		}
	}
}
