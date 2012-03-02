using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WallsParents
{
	public Transform parentWallFront;
	public Transform parentWallBack;
	public Transform parentWallLeft;
	public Transform parentWallRight;
	public Color colorWallFront;
	public Color colorWallBack;
	public Color colorWallLeft;
	public Color colorWallRight;
	
	public Transform[] GetWalls ()
	{
		Transform[] walls = new Transform[4];
		walls [0] = parentWallFront;
		walls [1] = parentWallBack;
		walls [2] = parentWallLeft;
		walls [3] = parentWallRight;
		return walls;
	}
	
	public Color GetWallColor (int index)
	{
		switch (index) {
		case 0 :
			return colorWallFront;
			break;
		case 1 :
			return colorWallBack;
			break;
		case 2 :
			return colorWallLeft;
			break;
		case 3 :
			return colorWallRight;
			break;
		default :
			return Color.white;
			break;
		}
	}	
}

public class WallBuilder : MonoBehaviour {
	
	#region Prefabs references
	public Camera 	mainCamera;
	public GameObject 	grid;
	public GameObject 	floor;
	public GameObject 	wall, wall95, wall90, corner;
	public GameObject 	ceil;
	#endregion
	
	#region Parents
	public WallsParents parentWalls;
	public Transform 	parentFloor;
	public Transform 	parentCeil;
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
	private Vector3 root;
	#endregion
	
	#region Unty Methods
	void Start(){
	
		WallWidth = 5;
		WallDepth = 5;
		
		MaxWallWidth = 10;
		MaxWallDepth = 10;
		
		MinWallWidth = 2;
		MinWallDepth = 2;
		
		root = new Vector3(1000,0,1000);
		
		mov = transform.position;
		zoom = GameObject.Find("Main Camera").camera.orthographicSize;
	}
	
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
	}
	#endregion
	
	public void CreateTile (){
		
		Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
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
					firstPosition = posicaoCalibrada;
			}
		}
	}

	public void BuildGround (){
		RemoveGround();
		for (int z = 0; z != WallDepth; ++z) {
			for (int x = 0; x != WallWidth; ++x) {
				GameObject newTile = Instantiate (floor, Vector3.zero, floor.transform.rotation) as GameObject;
				newTile.transform.position = root + new Vector3 ((int)(x - (WallWidth / 2)), 0.01f, (int)(z - (WallDepth / 2)));
				newTile.transform.parent = parentFloor;
			}
		}
	}
	
	public void BuildWalls (){
		RemoveRoof();
		RemoveWalls();
		
		CreateRoof();
		
		GameObject[] pisos = GameObject.FindGameObjectsWithTag ("Chao");
		if (pisos.Length > 0) {
			foreach (GameObject piso in pisos) {
				#region Form New
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
					Debug.DrawRay(piso.transform.position, 
					              direction, Color.blue);
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
							Debug.DrawRay(piso.transform.position, 
							              direction, Color.red);
							if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
							                    direction - new Vector3(0, 0.25f, 0),
							                    out hit)) {
								if (hit.transform.tag != "Chao") {
									if (direction == Vector3.right) {
										CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
														new Vector3 (0.5f, 0, 0.5f),
							               				parentWalls.parentWallRight,
							               				new Vector3 (0, 90, 0),
														true);
									}
									if (direction == Vector3.left) {
										CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
														new Vector3 (-0.5f, 0, 0.5f),
														parentWalls.parentWallLeft,
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
							Debug.DrawRay(piso.transform.position,
							              sides[0], Color.cyan);
							RaycastHit hit;
							if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
								                    sides[0] - new Vector3(0, 0.25f, 0),
								                    out hit)) {
								if (hit.transform.tag != "Chao") {
									CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
													new Vector3 (-0.5f, 0, -0.5f),
													parentWalls.parentWallRight,
													new Vector3 (0, 270, 0),
													false);
									CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
													new Vector3 (0.5f, 0, 0.5f),
						               				parentWalls.parentWallRight,
						               				new Vector3 (0, (int)90, 0),
													true);
								}
								else {
									CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
													new Vector3 (-0.5f, 0, -0.5f),
													parentWalls.parentWallRight,
													new Vector3 (0, 270, 0),
													false);
								}
							}
						}
						else if (frontalDirectionsAccepted[0] == (Vector3.forward + Vector3.left) && 
						    	 frontalDirectionsAccepted[1] == Vector3.forward) {
							Debug.DrawRay(piso.transform.position,
							              sides[1], Color.cyan);
							RaycastHit hit;
							if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
								                    sides[1] - new Vector3(0, 0.25f, 0),
								                    out hit)) {
								if (hit.transform.tag != "Chao") {
									CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
													new Vector3 (0.5f, 0, -0.5f),
						               				parentWalls.parentWallLeft,
						               				new Vector3 (0, 180, 0),
													false);
									CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
													new Vector3 (-0.5f, 0, 0.5f),
													parentWalls.parentWallLeft,
													new Vector3 (0, 0, 0),
													true);
								}
								else {
									CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
													new Vector3 (0.5f, 0, -0.5f),
						               				parentWalls.parentWallLeft,
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
							               parentWalls.parentWallFront,
							               new Vector3 (0, 180, 0),
										   false);
							CreateCorner(piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
										   new Vector3 (-0.5f, 0, -0.5f),
							               parentWalls.parentWallFront,
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
					Debug.DrawRay(piso.transform.position, 
					              direction, Color.blue);
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
							Debug.DrawRay(piso.transform.position, 
							              direction, Color.red);
							if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
							                    direction - new Vector3(0, 0.25f, 0),
							                    out hit)) {
								if (hit.transform.tag != "Chao") {
									if (direction == Vector3.right) {
										CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
														new Vector3 (0.5f, 0, -0.5f),
							               				parentWalls.parentWallRight,
							               				new Vector3 (0, 180, 0),
														true);
									}
									if (direction == Vector3.left) {
										CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
														new Vector3 (-0.5f, 0, -0.5f),
														parentWalls.parentWallLeft,
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
							Debug.DrawRay(piso.transform.position,
							              sides[0], Color.cyan);
							RaycastHit hit;
							if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
								                    sides[0] - new Vector3(0, 0.25f, 0),
								                    out hit)) {
								if (hit.transform.tag != "Chao") {
									CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
													new Vector3 (-0.5f, 0, 0.5f),
													parentWalls.parentWallRight,
													new Vector3 (0, 0, 0),
													false);
									CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
													new Vector3 (0.5f, 0, -0.5f),
						               				parentWalls.parentWallRight,
						               				new Vector3 (0, 180, 0),
													true);
								}
								else {
									CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
													new Vector3 (-0.5f, 0, 0.5f),
													parentWalls.parentWallRight,
													new Vector3 (0, 0, 0),
													false);
								}
							}
						}
						else if (backwardDirectionsAccepted[0] == (Vector3.back + Vector3.left) && 
						    	 backwardDirectionsAccepted[1] == Vector3.back) {
							Debug.DrawRay(piso.transform.position,
							              sides[1], Color.cyan);
							RaycastHit hit;
							if (Physics.Raycast(piso.transform.position + new Vector3(0, 0.25f, 0),
								                    sides[1] - new Vector3(0, 0.25f, 0),
								                    out hit)) {
								if (hit.transform.tag != "Chao") {
									CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
													new Vector3 (0.5f, 0, 0.5f),
						               				parentWalls.parentWallLeft,
						               				new Vector3 (0, 90, 0),
													false);
									CreateCorner(	piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
													new Vector3 (-0.5f, 0, -0.5f),
													parentWalls.parentWallLeft,
													new Vector3 (0, 270, 0),
													true);
								}
								else {
									CreateCorner(	piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
													new Vector3 (0.5f, 0, 0.5f),
						               				parentWalls.parentWallLeft,
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
							               parentWalls.parentWallBack,
							               new Vector3 (0, 90, 0),
										   false);
							CreateCorner(piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
							               new Vector3 (-0.5f, 0, 0.5f),
							               parentWalls.parentWallBack,
							               new Vector3 (0, 0, 0),
										   false);
						}
					}
				}
				#endregion
				#endregion
				
				#region Form Old
				/*Vector3[] direcoes = new Vector3[] { 
					Vector3.forward, Vector3.right, Vector3.left, Vector3.back
				};
				bool[] direcoesAceitas = new bool[] { 
					false, false, false, false
				};
				bool[] diagonaisAceitas = new bool[] { 
					false, false, false, false
				};
				foreach (Vector3 direcao in direcoes) {
					RaycastHit hit;
					//Debug.DrawLine(piso.transform.position, piso.transform.position + direcao, Color.red);
					if (!Physics.Linecast (piso.transform.position, piso.transform.position + direcao, out hit)) {
						
						Vector3 posicao = piso.transform.position;
						if (direcao == Vector3.forward) {
							direcoesAceitas[0] = true;
							Vector3[] diagonais = new Vector3[] { 
								Vector3.left, Vector3.right
							};
							posicao += Vector3.forward;
							RaycastHit hitChao;
							foreach (Vector3 diagonal in diagonais) {
								//Debug.DrawLine(posicao, posicao + diagonal);
								if (Physics.Linecast (posicao, posicao + diagonal, out hitChao)) {
									if (hitChao.transform.tag == "Chao")
										if (diagonal  == Vector3.right)
											diagonaisAceitas[1] = true;
										else
											diagonaisAceitas[2] = true;
								}
							}
						}
						if (direcao == Vector3.right) {
							direcoesAceitas[1] = true;
							Vector3[] diagonais = new Vector3[] { 
								Vector3.forward, Vector3.back
							};
							posicao += Vector3.right;
							RaycastHit hitChao;
							foreach (Vector3 diagonal in diagonais) {
								//Debug.DrawLine(posicao, posicao + diagonal);
								if (Physics.Linecast (posicao, posicao + diagonal, out hitChao)) {
									if (hitChao.transform.tag == "Chao")
										if (diagonal  == Vector3.forward)
											diagonaisAceitas[0] = true;
										else
											diagonaisAceitas[3] = true;
								}
							}
						}
						if (direcao == Vector3.left) {
							direcoesAceitas[2] = true;
							Vector3[] diagonais = new Vector3[] { 
								Vector3.forward, Vector3.back
							};
							posicao += Vector3.left;
							RaycastHit hitChao;
							foreach (Vector3 diagonal in diagonais) {
								//Debug.DrawLine(posicao, posicao + diagonal);
								if (Physics.Linecast (posicao, posicao + diagonal, out hitChao)) {
									if (hitChao.transform.tag == "Chao")
										if (diagonal  == Vector3.forward)
											diagonaisAceitas[0] = true;
										else
											diagonaisAceitas[3] = true;
								}
							}
						}
						if (direcao	== Vector3.back) {
							direcoesAceitas[3] = true;
							Vector3[] diagonais = new Vector3[] { 
								Vector3.left, Vector3.right
							};
							posicao += Vector3.back;
							RaycastHit hitChao;
							foreach (Vector3 diagonal in diagonais) {
								Debug.DrawLine(posicao, posicao + diagonal);
								if (Physics.Linecast (posicao, posicao + diagonal, out hitChao)) {
									if (hitChao.transform.tag == "Chao") {
										if (diagonal  == Vector3.right)
											diagonaisAceitas[1] = true;
										else
											diagonaisAceitas[2] = true;
									}
								}
							}
						}
					}
				}
				
				if ((direcoesAceitas[0] && direcoesAceitas[1]) || 
				    (direcoesAceitas[0] && diagonaisAceitas[1]) || 
				    (diagonaisAceitas[0] && direcoesAceitas[1])) {
					Vector3 posicaoQuina;
					GameObject novaQuina;
					posicaoQuina = piso.transform.position + new Vector3 (0.5f, 0, 0.5f);
					if (!SamePosition("Quina", posicaoQuina)) {
						novaQuina = Instantiate (quina, posicaoQuina, quina.transform.rotation) as GameObject;
						if (diagonaisAceitas[0] && direcoesAceitas[1]) {
							novaQuina.transform.parent = parentWalls.parentWallRight;
							novaQuina.transform.eulerAngles += new Vector3 (0, 270.0f, 0);
						}
						else {
							novaQuina.transform.parent = parentWalls.parentWallFront;
							novaQuina.transform.eulerAngles += new Vector3 (0, 180.0f, 0);
						}
					}
				}
				if ((direcoesAceitas[0] && direcoesAceitas[2]) || 
				    (direcoesAceitas[0] && diagonaisAceitas[2]) || 
				    (diagonaisAceitas[0] && direcoesAceitas[2])) {
					Vector3 posicaoQuina;
					GameObject novaQuina;
					posicaoQuina = piso.transform.position + new Vector3 (-0.5f, 0, 0.5f);
					if (!SamePosition("Quina", posicaoQuina)) {
						novaQuina = Instantiate (quina, posicaoQuina, quina.transform.rotation) as GameObject;
						if (diagonaisAceitas[0] && direcoesAceitas[2]) {
							novaQuina.transform.parent = parentWalls.parentWallLeft;
							novaQuina.transform.eulerAngles += new Vector3 (0, 90.0f, 0);
						}
						else {
							novaQuina.transform.parent = parentWalls.parentWallFront;
							novaQuina.transform.eulerAngles += new Vector3 (0, 180.0f, 0);
						}
					}
				}
				if ((direcoesAceitas[3] && direcoesAceitas[1]) || 
				    (direcoesAceitas[3] && diagonaisAceitas[1]) || 
				    (diagonaisAceitas[3] && direcoesAceitas[1])) {
					Vector3 posicaoQuina;
					GameObject novaQuina;
					posicaoQuina = piso.transform.position + new Vector3 (0.5f, 0, -0.5f);
					if (!SamePosition("Quina", posicaoQuina)) {
						novaQuina = Instantiate (quina, posicaoQuina, quina.transform.rotation) as GameObject;
						if (diagonaisAceitas[3] && direcoesAceitas[1]) {
							novaQuina.transform.parent = parentWalls.parentWallRight;
							novaQuina.transform.eulerAngles += new Vector3 (0, 270.0f, 0);
						}
						else {
							novaQuina.transform.parent = parentWalls.parentWallBack;
							novaQuina.transform.eulerAngles += new Vector3 (0, 0, 0);
						}
					}
				}
				if ((direcoesAceitas[3] && direcoesAceitas[2]) || 
				    (direcoesAceitas[3] && diagonaisAceitas[2]) || 
				    (diagonaisAceitas[3] && direcoesAceitas[2])) {
					Vector3 posicaoQuina;
					GameObject novaQuina;
					posicaoQuina = piso.transform.position + new Vector3 (-0.5f, 0, -0.5f);
					if (!SamePosition("Quina", posicaoQuina)) {
						novaQuina = Instantiate (quina, posicaoQuina, quina.transform.rotation) as GameObject;
						if (diagonaisAceitas[3] && direcoesAceitas[2]) {
							novaQuina.transform.parent = parentWalls.parentWallLeft;
							novaQuina.transform.eulerAngles += new Vector3 (0, 90.0f, 0);
						}
						else {
							novaQuina.transform.parent = parentWalls.parentWallBack;
							novaQuina.transform.eulerAngles += new Vector3 (0, 0, 0);
						}
					}
				}*/
				#endregion
				
				GameObject cf = new GameObject("ChaoFantasma");
				cf.transform.position = piso.transform.position;
				cf.tag = "ChaoVazio";
				cf.transform.parent = parentFloor;
			}
			foreach (GameObject piso in pisos) {
				Vector3[] direcoes = new Vector3[] { 
					Vector3.forward, Vector3.right, Vector3.left, Vector3.back
				};
				bool[] direcoesAceitas = new bool[] { 
					false, false, false, false
				};
				foreach (Vector3 direcao in direcoes) {
					RaycastHit hit;
					if (!Physics.Linecast (piso.transform.position, piso.transform.position + direcao, out hit)) {
						if (direcao == Vector3.forward) {
							direcoesAceitas[0] = true;
						}
						if (direcao == Vector3.right) {
							direcoesAceitas[1] = true;
						}
						if (direcao == Vector3.left) {
							direcoesAceitas[2] = true;
						}
						if (direcao == Vector3.back) {
							direcoesAceitas[3] = true;
						}
					}
				}
				Vector3 posicaoParede;
				GameObject novaParede;
				Vector3[] laterais;
				bool[] lateraisAceitas = new bool[] { false, false };
				Vector3 posicao;
				Transform rotacao = wall.transform;
				int jParede;
				int j;
				//Front
				if (direcoesAceitas[0]) {
					posicaoParede = piso.transform.position;
					jParede = 0;
					j = 0;
					posicaoParede += new Vector3 (0, 0, 0.5f);
					posicao = posicaoParede + (Vector3.up);
					rotacao.eulerAngles = new Vector3 (0, 180.0f, 0);
					laterais = new Vector3[] { rotacao.right / 2, -rotacao.right / 2};
					foreach(Vector3 lateral in laterais) {
						Debug.DrawLine(posicao, posicao + lateral, Color.green);
						RaycastHit hit;
						lateraisAceitas[j] = false;
						if (Physics.Linecast (posicao, posicao + lateral, out hit)) {
							if (hit.transform.tag == "Quina") {
								lateraisAceitas[j] = true;
								++jParede;
							}
						}
						++j;
					}
					if (jParede == 2) {
						novaParede = Instantiate (wall90, posicaoParede, wall.transform.rotation) as GameObject;
					}
					else if (jParede == 1) {
						if (lateraisAceitas[0])
							posicaoParede += new Vector3 (0.025f, 0, 0);
						else if (lateraisAceitas[1])
							posicaoParede += new Vector3 (-0.025f, 0, 0);
						novaParede = Instantiate (wall95, posicaoParede, wall.transform.rotation) as GameObject;
					}
					else {
						novaParede = Instantiate (wall, posicaoParede, wall.transform.rotation) as GameObject;
					}
					novaParede.transform.parent = parentWalls.parentWallFront;
					novaParede.transform.eulerAngles = rotacao.eulerAngles;
				}
				//Right
				if (direcoesAceitas[1]) {
					posicaoParede = piso.transform.position;
					jParede = 0;
					j = 0;
					posicaoParede += new Vector3 (0.5f, 0, 0);
					posicao = posicaoParede + (Vector3.up);
					rotacao.eulerAngles = new Vector3 (0, 270.0f, 0);
					laterais = new Vector3[] { rotacao.right / 2, -rotacao.right / 2};
					foreach(Vector3 lateral in laterais) {
						Debug.DrawLine(posicao, posicao + lateral, Color.green);
						RaycastHit hit;
						lateraisAceitas[j] = false;
						if (Physics.Linecast (posicao, posicao + lateral, out hit)) {
							if (hit.transform.tag == "Quina") {
								lateraisAceitas[j] = true;
								++jParede;
							}
						}
						++j;
					}
					if (jParede == 2) {
						novaParede = Instantiate (wall90, posicaoParede, wall.transform.rotation) as GameObject;
					}
					else if (jParede == 1) {
						if (lateraisAceitas[0])
							posicaoParede += new Vector3 (0, 0, -0.025f);
						else if (lateraisAceitas[1])
							posicaoParede += new Vector3 (0, 0, 0.025f);
						novaParede = Instantiate (wall95, posicaoParede, wall.transform.rotation) as GameObject;
					}
					else {
						novaParede = Instantiate (wall, posicaoParede, wall.transform.rotation) as GameObject;
					}
					novaParede.transform.parent = parentWalls.parentWallRight;
					novaParede.transform.eulerAngles = rotacao.eulerAngles;
				}
				//Left
				if (direcoesAceitas[2]) {
					posicaoParede = piso.transform.position;
					jParede = 0;
					j = 0;
					posicaoParede += new Vector3 (-0.5f, 0, 0);
					posicao = posicaoParede + (Vector3.up);
					rotacao.eulerAngles = new Vector3 (0, 90.0f, 0);
					laterais = new Vector3[] { rotacao.right / 2, -rotacao.right / 2};
					foreach(Vector3 lateral in laterais) {
						Debug.DrawLine(posicao, posicao + lateral, Color.green);
						RaycastHit hit;
						lateraisAceitas[j] = false;
						if (Physics.Linecast (posicao, posicao + lateral, out hit)) {
							if (hit.transform.tag == "Quina") {
								lateraisAceitas[j] = true;
								++jParede;
							}
						}
						++j;
					}
					if (jParede == 2) {
						novaParede = Instantiate (wall90, posicaoParede, wall.transform.rotation) as GameObject;
					}
					else if (jParede == 1) {
						if (lateraisAceitas[0])
							posicaoParede += new Vector3 (0, 0, 0.025f);
						else if (lateraisAceitas[1])
							posicaoParede += new Vector3 (0, 0, -0.025f);
						novaParede = Instantiate (wall95, posicaoParede, wall.transform.rotation) as GameObject;
					}
					else {
						novaParede = Instantiate (wall, posicaoParede, wall.transform.rotation) as GameObject;
					}
					novaParede.transform.parent = parentWalls.parentWallLeft;
					novaParede.transform.eulerAngles = rotacao.eulerAngles;
				}
				//Back
				if (direcoesAceitas[3]) {
					posicaoParede = piso.transform.position;
					jParede = 0;
					j = 0;
					posicaoParede += new Vector3 (0, 0, -0.5f);
					posicao = posicaoParede + (Vector3.up);
					rotacao.eulerAngles = new Vector3 (0, 0, 0);
					laterais = new Vector3[] { rotacao.right / 2, -rotacao.right / 2};
					foreach(Vector3 lateral in laterais) {
						Debug.DrawLine(posicao, posicao + lateral, Color.green);
						RaycastHit hit;
						lateraisAceitas[j] = false;
						if (Physics.Linecast (posicao, posicao + lateral, out hit)) {
							if (hit.transform.tag == "Quina") {
								lateraisAceitas[j] = true;
								++jParede;
							}
						}
						++j;
					}
					if (jParede == 2) {
						novaParede = Instantiate (wall90, posicaoParede, wall.transform.rotation) as GameObject;
					}
					else if (jParede == 1) {
						if (lateraisAceitas[0])
							posicaoParede += new Vector3 (-0.025f, 0, 0);
						else if (lateraisAceitas[1])
							posicaoParede += new Vector3 (0.025f, 0, 0);
						novaParede = Instantiate (wall95, posicaoParede, wall.transform.rotation) as GameObject;
					}
					else {
						novaParede = Instantiate (wall, posicaoParede, wall.transform.rotation) as GameObject;
					}
					novaParede.transform.parent = parentWalls.parentWallBack;
					novaParede.transform.eulerAngles = rotacao.eulerAngles;
				}
				else {continue;}
			}
			foreach (GameObject q in GameObject.FindGameObjectsWithTag("Quina")) {
				q.tag = "Parede";
			}
			CombineMesh(parentFloor, true);
			CombineMesh(parentWalls.parentWallFront, true);
			CombineMesh(parentWalls.parentWallLeft, true);
			CombineMesh(parentWalls.parentWallRight, true);
			CombineMesh(parentWalls.parentWallBack, true);
			CombineMesh(parentCeil, true);
			RemoveGround();
			RemoveWalls();
			RemoveRoof();
			
			//region desativando teto, paredes e chão
//			GameObject[] walls = GameObject.FindGameObjectsWithTag("ParedeParent");
//			foreach(GameObject wall in walls){
//				wall.GetComponent<MeshRenderer>().enabled = false;	
//			}			
//			GameObject[] grounds = GameObject.FindGameObjectsWithTag("ChaoParent");
//			foreach(GameObject ground in grounds){
//				ground.GetComponent<MeshRenderer>().enabled = false;
//			}		
//			GameObject[] ceilings = GameObject.FindGameObjectsWithTag("TetoParent");
//			foreach(GameObject ceiling in ceilings){
//				ceiling.GetComponent<MeshRenderer>().enabled = false;
//			}
//			
//			GetComponent<GuiSelecaoMarca>().enabled = true;
//			enabled = false;
//			grid.renderer.enabled = false;
			
		} else {
			Debug.LogWarning ("Não existe chão! Por isso não pode ser criado paredes.");
			return;
		}
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
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
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
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
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
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
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
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
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
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
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
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
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
	
	private void DestroyOneBlockGround ()
	{
		if (Input.GetMouseButtonUp (1)) {
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			int chaoSemColisoes = 0;
			if (Physics.Raycast (ray, out hit)) {
				if (hit.transform.tag == "Chao") {
					Transform chaoDestruido = hit.transform;
					Destroy (hit.transform.gameObject);
					Vector3[] direcoes = new Vector3[] { Vector3.forward, Vector3.right, Vector3.left, Vector3.back };
					foreach (Vector3 direcao in direcoes) {
						RaycastHit hit2;
						if (Physics.Raycast (hit.transform.position, direcao, out hit2, 1.0f)) {
							if (hit2.transform.tag == "Chao") {
								Vector3[] direcoes2 = new Vector3[] { Vector3.forward, Vector3.right, Vector3.left, Vector3.back };
								int colisores = 0;
								foreach (Vector3 direcao2 in direcoes2) {
									RaycastHit hit3;
									Debug.DrawRay (hit2.transform.position, direcao2 * 1.0f, Color.blue);
									if (Physics.Raycast (hit2.transform.position, direcao2, out hit3, 1.0f)) {
										if (hit3.transform.tag == "Chao")
											colisores++;
									}
								}
								Debug.Log (colisores);
								if (colisores < 2)
									chaoSemColisoes++;
							}
						}
					}
					Debug.Log (chaoSemColisoes);
					if (chaoSemColisoes > 0) {
						GameObject novoChao = Instantiate (floor, chaoDestruido.position, chaoDestruido.rotation) as GameObject;
						novoChao.transform.parent = parentFloor;
					}
				}
			}
		}
	}
	
	private void RemoveGround() {
		GameObject[] pisos = GameObject.FindGameObjectsWithTag ("Chao");
		if (pisos.Length > 0) {
			foreach (GameObject piso in pisos)
				Destroy (piso,1);
		}
	}
	
	private void RemoveRoof () {
		GameObject[] ceil = GameObject.FindGameObjectsWithTag ("Teto");
		if (ceil.Length > 0) {
			foreach (GameObject t in ceil)
				Destroy (t);
		}
	}
	
	private void RemoveWalls () {
		GameObject[] paredes = GameObject.FindGameObjectsWithTag ("Parede");
		if (paredes.Length > 0) {
			foreach (GameObject wall in paredes)
				Destroy (wall);
		}
		GameObject[] quinas = GameObject.FindGameObjectsWithTag ("Quina");
		if (quinas.Length > 0) {
			foreach (GameObject corner in quinas)
				Destroy (corner);
		}
	}

	void MethodEditionGUI ()
	{/*
		GUI.BeginGroup(wndBackground, bgTexture);
			GUI.Label(ScreenUtils.ScaledRect(15f, 44f, 25f, 22f), I18n.t("Lar"), measuresLabelsFont);
			largura = (int)GUI.HorizontalSlider(ScreenUtils.ScaledRect(45f, 48f, 67f, 6f), (float)largura, 5, 25, sliderStyle, thumbSliderStyle);
			//GUI.Label(ScreenUtils.ScaledRect(95f, 44f, 12f, 22f), "m", measuresTextFieldsFont);
			GUI.Label(ScreenUtils.ScaledRect(45f, 34f, 67f, 22f), largura+"m", measuresTextFieldsFont);
		
			GUI.Label(ScreenUtils.ScaledRect(15f, 71f, 25f, 22f), I18n.t("Alt"), measuresLabelsFont);
			comprimento = (int)GUI.HorizontalSlider(ScreenUtils.ScaledRect(45f, 75f, 67f, 6f), (float)comprimento, 5, 25, sliderStyle, thumbSliderStyle);
			//GUI.Label(ScreenUtils.ScaledRect(95f, 71f, 12f, 22f), "m", measuresTextFieldsFont);
			GUI.Label(ScreenUtils.ScaledRect(45f, 61f, 67f, 22f), comprimento+"m", measuresTextFieldsFont);
		
			if (GUI.Button(wndButtons[0], new GUIContent(I18n.t("preencher área"), I18n.t("tip-construcao-paredes-preenche")), buttonsStyle)) {
				GetGround (comprimento, largura);
			}
			if (GUI.Button(wndButtons[1], new GUIContent(I18n.t("colocar wall"), I18n.t("tip-construcao-paredes-coloca")), buttonsStyle)) {
				GetWalls ();
			}
			if (GUI.Button(wndButtons[2], new GUIContent(I18n.t("reiniciar"), I18n.t("tip-construcao-paredes-reiniciar")), buttonsStyle)) {
				Restart ();
			}
		GUI.EndGroup();
		
//		activeGrid = GUI.Toggle (new Rect (10, 60, 120, 20), activeGrid, "Grid Chao");*/
	}
	
	private void CombineMesh (Transform target, bool createCollider) {
		MeshFilter[] meshFilters = target.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		for (int i = 0; i != meshFilters.Length; i++){
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
		}
		target.GetComponent<MeshFilter>().mesh = new Mesh();
		target.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		if (createCollider) {
			MeshCollider mc = target.gameObject.AddComponent("MeshCollider") as MeshCollider;
		}
	}
	
	private void CreateRoof () {
		GameObject[] pisos = GameObject.FindGameObjectsWithTag ("Chao");
		if (pisos.Length > 0) {
			Vector3 posicao;
			Quaternion rotacao;
			foreach (GameObject piso in pisos) {
				posicao = piso.transform.position;
				rotacao = Quaternion.identity;
				posicao.y = 2.5f;
				GameObject newTeto = Instantiate(ceil, posicao, rotacao) as GameObject;
				newTeto.transform.localEulerAngles = new Vector3(90, 0, 0);
				newTeto.transform.parent = parentCeil;
			}
		}
		else {
			Debug.LogWarning ("Não existe chão! Por isso não pode ser criado ceil.");
			return;
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
