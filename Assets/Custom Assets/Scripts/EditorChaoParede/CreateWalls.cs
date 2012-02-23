using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class WallsParents {
	public Transform parentWallFront;
	public Transform parentWallBack;
	public Transform parentWallLeft;
	public Transform parentWallRight;
	
	public Color colorWallFront;
	public Color colorWallBack;
	public Color colorWallLeft;
	public Color colorWallRight;
	
	public Transform[] GetWalls(){
		Transform[] walls = new Transform[4];
		walls[0] = parentWallFront;
		walls[1] = parentWallBack;
		walls[2] = parentWallLeft;
		walls[3] = parentWallRight;
		return walls;
	}
}

public class CreateWalls : MonoBehaviour
{
	/* Editor */
	public GameObject 	chao;
	public Transform 	parentPiso;
	public GameObject 	parede, parede95, parede90, quina;
	public WallsParents parentParedes;
	public GameObject 	teto;
	public Transform 	parentTeto;
	public GameObject 	grid;
	public Texture2D 	bgTexture;
	public GUIStyle		buttonsStyle;
	public GUIStyle		sliderStyle;
	public GUIStyle		thumbSliderStyle;
	/* End Editor */
	
	
	private Rect 		wndBackground;
	private Rect[] 		wndButtons;
	private GUIStyle	measuresLabelsFont;
	private GUIStyle	measuresTextFieldsFont;
	
	private Vector3		mov;
	private float 		speedCam = 5.0f;
	private int 		largura = 5, comprimento = 5;
	private bool 		ativarGridChao = false;
	private Vector3 	primeiroPos;
	private float 		zoom;
	private int 		zoomGrau = 40;
	
	private bool 		wasInitialized = false;
	
	private delegate void GUIMenu();
	private GUIMenu currentGuiMenu;
	
	void Start ()
	{
		mov = transform.position;
		zoom = camera.orthographicSize;
		currentGuiMenu = MethodEditionGUI;
		
		wndBackground = ScreenUtils.ScaledRect(	10f, 10f,
											 	bgTexture.width,
											 	bgTexture.height);
		
		GuiFont.ChangeFont(buttonsStyle, "Trebuchet14");
		buttonsStyle.contentOffset = ScreenUtils.ScaledVector2(-2f, -3f);
		
		measuresTextFieldsFont = GuiFont.GetFont("Trebuchet14");
		measuresTextFieldsFont.alignment = TextAnchor.UpperCenter;
		measuresLabelsFont = GuiFont.GetFont("Trebuchet14");
		measuresLabelsFont.normal.textColor = Color.gray;
		
		wndButtons = new Rect[3];
		wndButtons[0] = ScreenUtils.ScaledRect(	10f, 140f,
		 										buttonsStyle.normal.background.width,
		 										buttonsStyle.normal.background.height);
		
		wndButtons[1] = ScreenUtils.ScaledRect(	10f, 170f,
		 										buttonsStyle.normal.background.width,
		 										buttonsStyle.normal.background.height);
		
		wndButtons[2] = ScreenUtils.ScaledRect(	10f, 200f,
		 										buttonsStyle.normal.background.width,
		 										buttonsStyle.normal.background.height);
		
		Tooltip.AddDynamicTip(I18n.t("tip-construcao-paredes-preenche"));
		Tooltip.AddDynamicTip(I18n.t("tip-construcao-paredes-coloca"));
		Tooltip.AddDynamicTip(I18n.t("tip-construcao-paredes-reiniciar"));
		
	}
	
	void OnEnable () {
		parentPiso.GetComponent<MeshFilter>().sharedMesh = null;
		foreach (GameObject chaoVazio in GameObject.FindGameObjectsWithTag("ChaoVazio")) {
			Destroy(chaoVazio);
		}
		parentTeto.GetComponent<MeshFilter>().sharedMesh = null;
		foreach (Transform paredes in parentParedes.GetWalls()) {
			paredes.GetComponent<MeshFilter>().sharedMesh = null;
		}
		grid.renderer.enabled = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentGuiMenu == MethodEditionGUI) {
			CreateGround ();
			DestroyOneBlockGround ();
			MovCamera ();
		}
	}

	void MovCamera ()
	{
		mov += new Vector3 ((Input.GetAxis ("Horizontal") * Time.deltaTime) * speedCam, 0, (Input.GetAxis ("Vertical") * Time.deltaTime) * speedCam);
		mov.x = Mathf.Clamp(mov.x, -20.5f, 20.5f);
		mov.z = Mathf.Clamp(mov.z, -15.5f, 15.5f);		
		transform.position = mov;
		zoom -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomGrau * Mathf.Abs(zoom);
		zoom = Mathf.Clamp(zoom, 2.5f, 15f);
		camera.orthographicSize = zoom;
	}

	void CreateGround ()
	{
//		if (ativarGridChao) {
//			if (Input.GetMouseButtonUp (0)) {
//				primeiroPos = Vector3.zero;
//				GameObject[] pisos = GameObject.FindGameObjectsWithTag ("Chao");
//				foreach (GameObject piso in pisos)
//					piso.renderer.material.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
//			} else if (Input.GetMouseButtonDown (0)) {
//				VerifyGround ();
//			} else if (Input.GetMouseButton (0)) {
//				VerifyGroundShift ();
//			}
//		} else {
			if (Input.GetMouseButton (0)) {
				VerifyGround ();
			}
//		}
	}

	void VerifyGround () {
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
				GameObject[] pisos = GameObject.FindGameObjectsWithTag ("Chao");
				if (pisos.Length > 0) {
					Vector3[] direcoes = new Vector3[] { Vector3.forward, Vector3.right, Vector3.left, Vector3.back };
					int countDirecoes = 0;
					foreach (Vector3 direcao in direcoes) {
						RaycastHit hit2;
						if (Physics.Raycast (posicaoCalibrada, direcao, out hit2, 1.0f)) {
							Debug.DrawRay (posicaoCalibrada, direcao * 1.0f, Color.blue);
							if (hit2.transform.tag == "Chao" && countDirecoes != 1) {
								GameObject novoChao = Instantiate (chao, posicaoCalibrada, chao.transform.rotation) as GameObject;
								novoChao.transform.parent = parentPiso;
								countDirecoes++;
							}
						}
					}
				} 
//				else {
//					GameObject novoChao = Instantiate (chao, posicaoCalibrada, Quaternion.identity) as GameObject;
//					novoChao.transform.parent = parentPiso;
//				}
				if (ativarGridChao)
					primeiroPos = posicaoCalibrada;
			}
		}
	}

	void VerifyGroundShift ()
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
				float xs = primeiroPos.x, zs = primeiroPos.z;
				if (xs > posicaoCalibrada.x)
					xs = posicaoCalibrada.x;
				if (xs < posicaoCalibrada.x)
					xs = posicaoCalibrada.x;
				if (zs > posicaoCalibrada.z)
					zs = posicaoCalibrada.z;
				if (zs < posicaoCalibrada.z)
					zs = posicaoCalibrada.z;
				
				//Debug.Log(xs + " : " + posicaoCalibrada.x + " --- "+ zs + " : " + posicaoCalibrada.z);
				
				for (int newX = (int)primeiroPos.x, newZ = (int)primeiroPos.z; newX < (int)xs && newZ < (int)zs; ++newX, ++newZ) {
					Vector3 posicaoArray = new Vector3 (newX, y, newZ);
					RaycastHit hit2;
					if (Physics.Raycast (posicaoArray + new Vector3 (0, 0.01f, 0), Vector3.down, out hit2, 1.0f)) {
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
						if (hit2.transform.tag != "Chao") {
							GameObject novoChao = Instantiate (chao, posicaoArray, chao.transform.rotation) as GameObject;
							novoChao.transform.parent = parentPiso;
							novoChao.renderer.material.color = new Color (0, 1, 0, 1);
						}
					}
				}
				for (int newX = (int)primeiroPos.x, newZ = (int)primeiroPos.z; newX > (int)xs && newZ > (int)zs; --newX, --newZ) {
					Vector3 posicaoArray = new Vector3 (newX, y, newZ);
					RaycastHit hit2;
					if (Physics.Raycast (posicaoArray + new Vector3 (0, 0.01f, 0), Vector3.down, out hit2, 1.0f)) {
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
						if (hit2.transform.tag != "Chao") {
							GameObject novoChao = Instantiate (chao, posicaoArray, chao.transform.rotation) as GameObject;
							novoChao.transform.parent = parentPiso;
							novoChao.renderer.material.color = new Color (0, 1, 0, 1);
						}
					}
				}
				for (int newX = (int)primeiroPos.x; newX < (int)xs; ++newX) {
					Vector3 posicaoArray = new Vector3 (newX, y, zs);
					RaycastHit hit2;
					if (Physics.Raycast (posicaoArray + new Vector3 (0, 0.01f, 0), Vector3.down, out hit2, 1.0f)) {
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
						if (hit2.transform.tag != "Chao") {
							GameObject novoChao = Instantiate (chao, posicaoArray, chao.transform.rotation) as GameObject;
							novoChao.transform.parent = parentPiso;
							novoChao.renderer.material.color = new Color (0, 1, 0, 1);
						}
					}
				}
				for (int newX = (int)primeiroPos.x; newX > (int)xs; --newX) {
					Vector3 posicaoArray = new Vector3 (newX, y, zs);
					RaycastHit hit2;
					if (Physics.Raycast (posicaoArray + new Vector3 (0, 0.01f, 0), Vector3.down, out hit2, 1.0f)) {
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
						if (hit2.transform.tag != "Chao") {
							GameObject novoChao = Instantiate (chao, posicaoArray, chao.transform.rotation) as GameObject;
							novoChao.transform.parent = parentPiso;
							novoChao.renderer.material.color = new Color (0, 1, 0, 1);
						}
					}
				}
				for (int newZ = (int)primeiroPos.z; newZ < (int)zs; ++newZ) {
					Vector3 posicaoArray = new Vector3 (xs, y, newZ);
					RaycastHit hit2;
					if (Physics.Raycast (posicaoArray + new Vector3 (0, 0.01f, 0), Vector3.down, out hit2, 1.0f)) {
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
						if (hit2.transform.tag != "Chao") {
							GameObject novoChao = Instantiate (chao, posicaoArray, chao.transform.rotation) as GameObject;
							novoChao.transform.parent = parentPiso;
							novoChao.renderer.material.color = new Color (0, 1, 0, 1);
						}
					}
				}
				for (int newZ = (int)primeiroPos.z; newZ > (int)zs; --newZ) {
					Vector3 posicaoArray = new Vector3 (xs, y, newZ);
					RaycastHit hit2;
					if (Physics.Raycast (posicaoArray + new Vector3 (0, 0.01f, 0), Vector3.down, out hit2, 1.0f)) {
						Debug.DrawRay (posicaoArray, Vector3.down * 1.0f, Color.blue);
						if (hit2.transform.tag != "Chao") {
							GameObject novoChao = Instantiate (chao, posicaoArray, chao.transform.rotation) as GameObject;
							novoChao.transform.parent = parentPiso;
							novoChao.renderer.material.color = new Color (0, 1, 0, 1);
						}
					}
				}
			}
		}
	}

	void DestroyOneBlockGround ()
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
						GameObject novoChao = Instantiate (chao, chaoDestruido.position, chaoDestruido.rotation) as GameObject;
						novoChao.transform.parent = parentPiso;
					}
				}
			}
		}
	}

	void OnGUI ()
	{
	
		if (!wasInitialized) {
			GUI.skin.settings.selectionColor = new Color(0,486f, 0.525f, 0.560f);
			wasInitialized = true;
		}
		currentGuiMenu();
		
		Tooltip.DoTips();
	}
	
	void Restart () {
		RemoveWalls();
		RemoveGround();
		RemoveRoof();
	}
	
	void RemoveGround() {
		GameObject[] pisos = GameObject.FindGameObjectsWithTag ("Chao");
		if (pisos.Length > 0) {
			foreach (GameObject piso in pisos)
				Destroy (piso);
		}
	}
	
	void RemoveRoof () {
		GameObject[] teto = GameObject.FindGameObjectsWithTag ("Teto");
		if (teto.Length > 0) {
			foreach (GameObject t in teto)
				Destroy (t);
		}
	}
	
	void RemoveWalls () {
		GameObject[] paredes = GameObject.FindGameObjectsWithTag ("Parede");
		if (paredes.Length > 0) {
			foreach (GameObject parede in paredes)
				Destroy (parede);
		}
		GameObject[] quinas = GameObject.FindGameObjectsWithTag ("Quina");
		if (quinas.Length > 0) {
			foreach (GameObject quina in quinas)
				Destroy (quina);
		}
	}

	void MethodEditionGUI ()
	{
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
			if (GUI.Button(wndButtons[1], new GUIContent(I18n.t("colocar parede"), I18n.t("tip-construcao-paredes-coloca")), buttonsStyle)) {
				GetWalls ();
			}
			if (GUI.Button(wndButtons[2], new GUIContent(I18n.t("reiniciar"), I18n.t("tip-construcao-paredes-reiniciar")), buttonsStyle)) {
				Restart ();
			}
		GUI.EndGroup();
		
//		ativarGridChao = GUI.Toggle (new Rect (10, 60, 120, 20), ativarGridChao, "Grid Chao");
	}

	void GetGround (int comprimento, int largura)
	{
		RemoveGround();
		
		for (int z = 0; z < comprimento; z++) {
			for (int x = 0; x < largura; x++) {
				GameObject novoChao = Instantiate (chao, Vector3.zero, chao.transform.rotation) as GameObject;
				novoChao.transform.parent = parentPiso;
				novoChao.transform.position = new Vector3 (x - (int)(largura / 2), 0.01f, z - (int)(comprimento / 2));
			}
		}
	}

	void GetParedes () {
		GameObject[] antigasParedes = GameObject.FindGameObjectsWithTag ("Parede");
		if (antigasParedes.Length > 0) {
			foreach (GameObject antigaParede in antigasParedes)
				Destroy (antigaParede);
		}
		
		CreateRoof();
		
		GameObject[] pisos = GameObject.FindGameObjectsWithTag ("Chao");
		if (pisos.Length > 0) {
			foreach (GameObject piso in pisos) {
				Vector3[] direcoes = new Vector3[] { 
					Vector3.forward, Vector3.right, Vector3.left, Vector3.back
				};
				bool[] direcoesAceitas = new bool[] { 
					false, false, false, false
				};
				bool[] diagonaisAceitas = new bool[] { 
					false, false, false, false
				};
				bool bParede90 = false;
				bool bParede95 = false;
				foreach (Vector3 direcao in direcoes) {
					RaycastHit hit;
					if (!Physics.Raycast (piso.transform.position, direcao, out hit, 1.0f)) {
						
						Vector3 posicao = piso.transform.position;
						if (direcao == Vector3.forward) {
							direcoesAceitas[0] = true;
							Vector3[] diagonais = new Vector3[] { 
								Vector3.left, Vector3.right
							};
							posicao += Vector3.forward;
							RaycastHit hitChao;
							foreach (Vector3 diagonal in diagonais) {
								Debug.DrawRay(posicao, diagonal);
								if (Physics.Raycast (posicao, diagonal, out hitChao, 1.0f)) {
									if (hitChao.transform.tag == "Chao")
										if (diagonal  == Vector3.right)
											diagonaisAceitas[1] = true;
										else
											diagonaisAceitas[2] = true;
								}
							}
						}
						else if (direcao == Vector3.right) {
							direcoesAceitas[1] = true;
							Vector3[] diagonais = new Vector3[] { 
								Vector3.forward, Vector3.back
							};
							posicao += Vector3.right;
							RaycastHit hitChao;
							foreach (Vector3 diagonal in diagonais) {
								Debug.DrawRay(posicao, diagonal);
								if (Physics.Raycast (posicao, diagonal, out hitChao, 1.0f)) {
									if (hitChao.transform.tag == "Chao")
										if (diagonal  == Vector3.forward)
											diagonaisAceitas[0] = true;
										else
											diagonaisAceitas[3] = true;
								}
							}
						}
						else if (direcao == Vector3.left) {
							direcoesAceitas[2] = true;
							Vector3[] diagonais = new Vector3[] { 
								Vector3.forward, Vector3.back
							};
							posicao += Vector3.left;
							RaycastHit hitChao;
							foreach (Vector3 diagonal in diagonais) {
								Debug.DrawRay(posicao, diagonal);
								if (Physics.Raycast (posicao, diagonal, out hitChao, 1.0f)) {
									if (hitChao.transform.tag == "Chao")
										if (diagonal  == Vector3.forward)
											diagonaisAceitas[0] = true;
										else
											diagonaisAceitas[3] = true;
								}
							}
						}
						else {
							direcoesAceitas[3] = true;
							Vector3[] diagonais = new Vector3[] { 
								Vector3.left, Vector3.right
							};
							posicao += Vector3.back;
							RaycastHit hitChao;
							foreach (Vector3 diagonal in diagonais) {
								Debug.DrawRay(posicao, diagonal);
								if (Physics.Raycast (posicao, diagonal, out hitChao, 1.0f)) {
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
				
				if ((diagonaisAceitas[1] && diagonaisAceitas[2]) || 
				    (diagonaisAceitas[0] && diagonaisAceitas[3])) {
					bParede90 = true;
				}
				
				if ((direcoesAceitas[0] && direcoesAceitas[1]) || 
				    (direcoesAceitas[0] && diagonaisAceitas[1]) || 
				    (diagonaisAceitas[0] && direcoesAceitas[1])) {
					bParede95 = true;
					Vector3 posicaoQuina;
					GameObject novaQuina;
					posicaoQuina = piso.transform.position + new Vector3 (0.5f, 0, 0.5f);
					if (!SamePosition("Parede", posicaoQuina)) {
						novaQuina = Instantiate (quina, posicaoQuina, quina.transform.rotation) as GameObject;
						if (diagonaisAceitas[0] && direcoesAceitas[1]) {
							novaQuina.transform.parent = parentParedes.parentWallRight;
							novaQuina.transform.eulerAngles += new Vector3 (0, 270.0f, 0);
						}
						else {
							novaQuina.transform.parent = parentParedes.parentWallFront;
							novaQuina.transform.eulerAngles += new Vector3 (0, 180.0f, 0);
						}
					}
					else {
						bParede90 = true;
					}
				}
				if ((direcoesAceitas[0] && direcoesAceitas[2]) || 
				    (direcoesAceitas[0] && diagonaisAceitas[2]) || 
				    (diagonaisAceitas[0] && direcoesAceitas[2])) {
					bParede95 = true;
					Vector3 posicaoQuina;
					GameObject novaQuina;
					posicaoQuina = piso.transform.position + new Vector3 (-0.5f, 0, 0.5f);
					if (!SamePosition("Parede", posicaoQuina)) {
						novaQuina = Instantiate (quina, posicaoQuina, quina.transform.rotation) as GameObject;
						if (diagonaisAceitas[0] && direcoesAceitas[2]) {
							novaQuina.transform.parent = parentParedes.parentWallLeft;
							novaQuina.transform.eulerAngles += new Vector3 (0, 90.0f, 0);
						}
						else {
							novaQuina.transform.parent = parentParedes.parentWallFront;
							novaQuina.transform.eulerAngles += new Vector3 (0, 180.0f, 0);
						}
					}
					else {
						bParede90 = true;
					}
				}
				if ((direcoesAceitas[3] && direcoesAceitas[1]) || 
				    (direcoesAceitas[3] && diagonaisAceitas[1]) || 
				    (diagonaisAceitas[3] && direcoesAceitas[1])) {
					bParede95 = true;
					Vector3 posicaoQuina;
					GameObject novaQuina;
					posicaoQuina = piso.transform.position + new Vector3 (0.5f, 0, -0.5f);
					if (!SamePosition("Parede", posicaoQuina)) {
						novaQuina = Instantiate (quina, posicaoQuina, quina.transform.rotation) as GameObject;
						if (diagonaisAceitas[3] && direcoesAceitas[1]) {
							novaQuina.transform.parent = parentParedes.parentWallRight;
							novaQuina.transform.eulerAngles += new Vector3 (0, 270.0f, 0);
						}
						else {
							novaQuina.transform.parent = parentParedes.parentWallBack;
							novaQuina.transform.eulerAngles = new Vector3 (0, 0, 0);
						}
					}
					else {
						bParede90 = true;
					}
				}
				if ((direcoesAceitas[3] && direcoesAceitas[2]) || 
				    (direcoesAceitas[3] && diagonaisAceitas[2]) || 
				    (diagonaisAceitas[3] && direcoesAceitas[2])) {
					bParede95 = true;
					Vector3 posicaoQuina;
					GameObject novaQuina;
					posicaoQuina = piso.transform.position + new Vector3 (-0.5f, 0, -0.5f);
					if (!SamePosition("Parede", posicaoQuina)) {
						novaQuina = Instantiate (quina, posicaoQuina, quina.transform.rotation) as GameObject;
						if (diagonaisAceitas[3] && direcoesAceitas[2]) {
							novaQuina.transform.parent = parentParedes.parentWallLeft;
							novaQuina.transform.eulerAngles += new Vector3 (0, 90.0f, 0);
						}
						else {
							novaQuina.transform.parent = parentParedes.parentWallBack;
							novaQuina.transform.eulerAngles = new Vector3 (0, 0, 0);
						}
					}
					else {
						bParede90 = true;
					}
				}
				
				//Front
				if (direcoesAceitas[0]) {
					Vector3 posicaoParede = piso.transform.position;
					GameObject novaParede;
					if (bParede90) {
						posicaoParede += new Vector3 (0, 0, 0.5f);
						novaParede = Instantiate (parede90, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else if (bParede95) {
						if (direcoesAceitas[1] || diagonaisAceitas[1])
							posicaoParede += new Vector3 (-0.025f, 0, 0.5f);
						else if (direcoesAceitas[2] || diagonaisAceitas[2])
							posicaoParede += new Vector3 (0.025f, 0, 0.5f);
						novaParede = Instantiate (parede95, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else {
						posicaoParede += new Vector3 (0, 0, 0.5f);
						novaParede = Instantiate (parede, posicaoParede, parede.transform.rotation) as GameObject;
					}
					novaParede.transform.parent = parentParedes.parentWallFront;
					novaParede.transform.eulerAngles += new Vector3 (0, 180.0f, 0);
				}
				//Right
				if (direcoesAceitas[1]) {
					Vector3 posicaoParede = piso.transform.position;
					GameObject novaParede;
					if (bParede90) {
						posicaoParede += new Vector3 (0.5f, 0, 0);
						novaParede = Instantiate (parede90, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else if (bParede95) {
						if (direcoesAceitas[0] || diagonaisAceitas[0])
							posicaoParede += new Vector3 (0.5f, 0, -0.025f);
						else if (direcoesAceitas[3] || diagonaisAceitas[3])
							posicaoParede += new Vector3 (0.5f, 0, 0.025f);
						novaParede = Instantiate (parede95, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else {
						posicaoParede += new Vector3 (0.5f, 0, 0);
						novaParede = Instantiate (parede, posicaoParede, parede.transform.rotation) as GameObject;
					}
					novaParede.transform.parent = parentParedes.parentWallRight;
					novaParede.transform.eulerAngles += new Vector3 (0, 270.0f, 0);
				}
				//Left
				if (direcoesAceitas[2]) {
					Vector3 posicaoParede = piso.transform.position;
					GameObject novaParede;
					if (bParede90) {
						posicaoParede += new Vector3 (-0.5f, 0, 0);
						novaParede = Instantiate (parede90, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else if (bParede95) {
						if (direcoesAceitas[0] || diagonaisAceitas[0])
							posicaoParede += new Vector3 (-0.5f, 0, -0.025f);
						else if (direcoesAceitas[3] || diagonaisAceitas[3])
							posicaoParede += new Vector3 (-0.5f, 0, 0.025f);
						novaParede = Instantiate (parede95, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else {
						posicaoParede += new Vector3 (-0.5f, 0, 0);
						novaParede = Instantiate (parede, posicaoParede, parede.transform.rotation) as GameObject;
					}
					novaParede.transform.parent = parentParedes.parentWallLeft;
					novaParede.transform.eulerAngles += new Vector3 (0, 90.0f, 0);
				}
				//Back
				if (direcoesAceitas[3]) {
					Vector3 posicaoParede = piso.transform.position;
					GameObject novaParede;
					if (bParede90) {
						posicaoParede += new Vector3 (0, 0, -0.5f);
						novaParede = Instantiate (parede90, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else if (bParede95) {
						if (direcoesAceitas[1] || diagonaisAceitas[1])
							posicaoParede += new Vector3 (-0.025f, 0, -0.5f);
						else if (direcoesAceitas[2] || diagonaisAceitas[2])
							posicaoParede += new Vector3 (0.025f, 0, -0.5f);
						novaParede = Instantiate (parede95, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else {
						posicaoParede += new Vector3 (0, 0, -0.5f);
						novaParede = Instantiate (parede, posicaoParede, parede.transform.rotation) as GameObject;
					}
					novaParede.transform.parent = parentParedes.parentWallBack;
					novaParede.transform.eulerAngles = new Vector3 (0, 0, 0);
				}
				GameObject cf = new GameObject("ChaoFantasma");
				cf.transform.position = piso.transform.position;
				cf.tag = "ChaoVazio";
				cf.transform.parent = parentPiso;
			}
//			CombineMesh(parentPiso, true);
//			CombineMesh(parentParedes.parentWallFront, true);
//			CombineMesh(parentParedes.parentWallLeft, true);
//			CombineMesh(parentParedes.parentWallRight, true);
//			CombineMesh(parentParedes.parentWallBack, true);
//			CombineMesh(parentTeto, false);
//			RemoveGround();
//			RemoveWalls();
//			RemoveRoof();
			
			//region desativando teto, paredes e chão
			GameObject[] walls = GameObject.FindGameObjectsWithTag("ParedeParent");
			foreach(GameObject wall in walls){
				wall.GetComponent<MeshRenderer>().enabled = false;	
			}			
			GameObject[] grounds = GameObject.FindGameObjectsWithTag("ChaoParent");
			foreach(GameObject ground in grounds){
				ground.GetComponent<MeshRenderer>().enabled = false;
			}		
			GameObject[] ceilings = GameObject.FindGameObjectsWithTag("TetoParent");
			foreach(GameObject ceiling in ceilings){
				ceiling.GetComponent<MeshRenderer>().enabled = false;
			}
			
			GetComponent<GuiSelecaoMarca>().enabled = true;
			enabled = false;
			grid.renderer.enabled = false;
			
		} else {
			Debug.LogWarning ("Não existe chão! Por isso não pode ser criado paredes.");
			return;
		}
	}
	
	void GetWalls () {
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
										CreateCinchona(	piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
							               				parentParedes.parentWallRight,
							               				new Vector3 (0, 90, 0));
									}
									if (direction == Vector3.left) {
										CreateCinchona(	piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
														parentParedes.parentWallLeft,
														new Vector3 (0, 0, 0));
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
									CreateCinchona(	piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
													parentParedes.parentWallRight,
													new Vector3 (0, 270, 0));
									CreateCinchona(	piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
							               				parentParedes.parentWallRight,
							               				new Vector3 (0, (int)90, 0));
								}
								else {
									CreateCinchona(	piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
													parentParedes.parentWallRight,
													new Vector3 (0, 270, 0));
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
									CreateCinchona(	piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
						               				parentParedes.parentWallLeft,
						               				new Vector3 (0, 180, 0));
									CreateCinchona(	piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
													parentParedes.parentWallLeft,
													new Vector3 (0, 0, 0));
								}
								else {
									CreateCinchona(	piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
						               				parentParedes.parentWallLeft,
						               				new Vector3 (0, 180, 0));
								}
							}
						}
					}
					else if (frontalDirectionsAccepted.Count == 1) {
						if (frontalDirectionsAccepted[0] == Vector3.forward) {
							CreateCinchona(piso.transform.position + new Vector3 (0.5f, 0, 0.5f),
							               parentParedes.parentWallFront,
							               new Vector3 (0, 180, 0));
							CreateCinchona(piso.transform.position + new Vector3 (-0.5f, 0, 0.5f),
							               parentParedes.parentWallFront,
							               new Vector3 (0, 270, 0));
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
										CreateCinchona(	piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
							               				parentParedes.parentWallRight,
							               				new Vector3 (0, 180, 0));
									}
									if (direction == Vector3.left) {
										CreateCinchona(	piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
														parentParedes.parentWallLeft,
														new Vector3 (0, 270, 0));
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
									CreateCinchona(	piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
													parentParedes.parentWallRight,
													new Vector3 (0, 0, 0));
									CreateCinchona(	piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
							               				parentParedes.parentWallRight,
							               				new Vector3 (0, 180, 0));
								}
								else {
									CreateCinchona(	piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
													parentParedes.parentWallRight,
													new Vector3 (0, 0, 0));
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
									CreateCinchona(	piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
						               				parentParedes.parentWallLeft,
						               				new Vector3 (0, 90, 0));
									CreateCinchona(	piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
													parentParedes.parentWallLeft,
													new Vector3 (0, 270, 0));
								}
								else {
									CreateCinchona(	piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
						               				parentParedes.parentWallLeft,
						               				new Vector3 (0, 90, 0));
								}
							}
						}
					}
					else if (backwardDirectionsAccepted.Count == 1) {
						if (backwardDirectionsAccepted[0] == Vector3.back) {
							CreateCinchona(piso.transform.position + new Vector3 (0.5f, 0, -0.5f),
							               parentParedes.parentWallBack,
							               new Vector3 (0, 90, 0));
							CreateCinchona(piso.transform.position + new Vector3 (-0.5f, 0, -0.5f),
							               parentParedes.parentWallBack,
							               new Vector3 (0, 0, 0));
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
							novaQuina.transform.parent = parentParedes.parentWallRight;
							novaQuina.transform.eulerAngles += new Vector3 (0, 270.0f, 0);
						}
						else {
							novaQuina.transform.parent = parentParedes.parentWallFront;
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
							novaQuina.transform.parent = parentParedes.parentWallLeft;
							novaQuina.transform.eulerAngles += new Vector3 (0, 90.0f, 0);
						}
						else {
							novaQuina.transform.parent = parentParedes.parentWallFront;
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
							novaQuina.transform.parent = parentParedes.parentWallRight;
							novaQuina.transform.eulerAngles += new Vector3 (0, 270.0f, 0);
						}
						else {
							novaQuina.transform.parent = parentParedes.parentWallBack;
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
							novaQuina.transform.parent = parentParedes.parentWallLeft;
							novaQuina.transform.eulerAngles += new Vector3 (0, 90.0f, 0);
						}
						else {
							novaQuina.transform.parent = parentParedes.parentWallBack;
							novaQuina.transform.eulerAngles += new Vector3 (0, 0, 0);
						}
					}
				}*/
				#endregion
				
				GameObject cf = new GameObject("ChaoFantasma");
				cf.transform.position = piso.transform.position;
				cf.tag = "ChaoVazio";
				cf.transform.parent = parentPiso;
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
				Transform rotacao = parede.transform;
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
						novaParede = Instantiate (parede90, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else if (jParede == 1) {
						if (lateraisAceitas[0])
							posicaoParede += new Vector3 (0.025f, 0, 0);
						else if (lateraisAceitas[1])
							posicaoParede += new Vector3 (-0.025f, 0, 0);
						novaParede = Instantiate (parede95, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else {
						novaParede = Instantiate (parede, posicaoParede, parede.transform.rotation) as GameObject;
					}
					novaParede.transform.parent = parentParedes.parentWallFront;
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
						novaParede = Instantiate (parede90, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else if (jParede == 1) {
						if (lateraisAceitas[0])
							posicaoParede += new Vector3 (0, 0, -0.025f);
						else if (lateraisAceitas[1])
							posicaoParede += new Vector3 (0, 0, 0.025f);
						novaParede = Instantiate (parede95, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else {
						novaParede = Instantiate (parede, posicaoParede, parede.transform.rotation) as GameObject;
					}
					novaParede.transform.parent = parentParedes.parentWallRight;
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
						novaParede = Instantiate (parede90, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else if (jParede == 1) {
						if (lateraisAceitas[0])
							posicaoParede += new Vector3 (0, 0, 0.025f);
						else if (lateraisAceitas[1])
							posicaoParede += new Vector3 (0, 0, -0.025f);
						novaParede = Instantiate (parede95, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else {
						novaParede = Instantiate (parede, posicaoParede, parede.transform.rotation) as GameObject;
					}
					novaParede.transform.parent = parentParedes.parentWallLeft;
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
						novaParede = Instantiate (parede90, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else if (jParede == 1) {
						if (lateraisAceitas[0])
							posicaoParede += new Vector3 (-0.025f, 0, 0);
						else if (lateraisAceitas[1])
							posicaoParede += new Vector3 (0.025f, 0, 0);
						novaParede = Instantiate (parede95, posicaoParede, parede.transform.rotation) as GameObject;
					}
					else {
						novaParede = Instantiate (parede, posicaoParede, parede.transform.rotation) as GameObject;
					}
					novaParede.transform.parent = parentParedes.parentWallBack;
					novaParede.transform.eulerAngles = rotacao.eulerAngles;
				}
				else {continue;}
			}
			foreach (GameObject q in GameObject.FindGameObjectsWithTag("Quina")) {
				q.tag = "Parede";
			}
			CombineMesh(parentPiso, true);
			CombineMesh(parentParedes.parentWallFront, true);
			CombineMesh(parentParedes.parentWallLeft, true);
			CombineMesh(parentParedes.parentWallRight, true);
			CombineMesh(parentParedes.parentWallBack, true);
			CombineMesh(parentTeto, true);
			RemoveGround();
			RemoveWalls();
			RemoveRoof();
			
			//region desativando teto, paredes e chão
			GameObject[] walls = GameObject.FindGameObjectsWithTag("ParedeParent");
			foreach(GameObject wall in walls){
				wall.GetComponent<MeshRenderer>().enabled = false;	
			}			
			GameObject[] grounds = GameObject.FindGameObjectsWithTag("ChaoParent");
			foreach(GameObject ground in grounds){
				ground.GetComponent<MeshRenderer>().enabled = false;
			}		
			GameObject[] ceilings = GameObject.FindGameObjectsWithTag("TetoParent");
			foreach(GameObject ceiling in ceilings){
				ceiling.GetComponent<MeshRenderer>().enabled = false;
			}
			
			GetComponent<GuiSelecaoMarca>().enabled = true;
			enabled = false;
			grid.renderer.enabled = false;
			
		} else {
			Debug.LogWarning ("Não existe chão! Por isso não pode ser criado paredes.");
			return;
		}
	}
	
	void CombineMesh (Transform target, bool createCollider) {
		MeshFilter[] meshFilters = target.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		for (int i = 0; i < meshFilters.Length; i++){
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
		}
		target.GetComponent<MeshFilter>().mesh = new Mesh();
		target.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
		if (createCollider) {
			MeshCollider mc = target.gameObject.AddComponent("MeshCollider") as MeshCollider;
		}
	}
	
	void CreateRoof () {
		GameObject[] pisos = GameObject.FindGameObjectsWithTag ("Chao");
		if (pisos.Length > 0) {
			Vector3 posicao;
			Quaternion rotacao;
			foreach (GameObject piso in pisos) {
				posicao = piso.transform.position;
				rotacao = Quaternion.identity;
				posicao.y = 2.5f;
				GameObject newTeto = Instantiate(teto, posicao, rotacao) as GameObject;
				newTeto.transform.localEulerAngles = new Vector3(90, 0, 0);
				newTeto.transform.parent = parentTeto;
			}
		}
		else {
			Debug.LogWarning ("Não existe chão! Por isso não pode ser criado teto.");
			return;
		}
	}
	
	void CreateCinchona (Vector3 position, Transform side, Vector3 eulerAngles) {
		if (!SamePosition("Quina", position)) {
			GameObject novaQuina;
			novaQuina = Instantiate (quina, position, quina.transform.rotation) as GameObject;
			novaQuina.transform.parent = side;
			novaQuina.transform.eulerAngles = eulerAngles;
		}
	}
	
	bool SamePosition (string tag, Vector3 position) {
		GameObject[] novas = GameObject.FindGameObjectsWithTag (tag);
		foreach (GameObject n in novas) {
			if (position == n.transform.position) {
				return true;
			}
		}
		return false;
	}
}