using UnityEngine;
using System.Collections;
using Visiorama;

public class CameraController : MonoBehaviour {
	private GameObject movelSelecionado;
	private FurnitureManager furnitureManager;
	private InfoController infoController;
	public bool CanMoveCamera {get; private set;}
	
	public static Camera sCamera;
	public static float SpeedZoom;
	public static float StepZoom;
	
	private GameObject[] cameras;
	public FreeCamera3d freeCamera { get; private set; }
	void Start () {
		
		CanMoveCamera = true;
		
		furnitureManager = GetComponent<FurnitureManager> ();
	
		sCamera = GetComponent<Camera>();
		SpeedZoom = 1.5f;
		StepZoom  = 8f;
		
		infoController = GameObject.FindWithTag("GameController").GetComponentInChildren<InfoController>();
		
		CanMoveCamera = true;
		
		cameras = GameObject.FindGameObjectsWithTag("GUICamera");
		freeCamera = gameObject.AddComponent<FreeCamera3d>()
									.Initialize(this.camera,1.0f,0.3f, true, false);
	}

	// Update is called once per frame
	void Update () {
#if !UNITY_ANDROID && !UNITY_IPHONE
		freeCamera.DoCamera ();
#endif
		SelectMobile ();
	}
	
	private void SelectMobile () {
		if (Input.GetMouseButtonDown(0))
		{
			if (NGUIUtils.ClickedInGUI (cameras,"GUI")) 
				return;
			
			if(!furnitureManager.isNewFurnitureActive()) 
				CheckActiveFurniture();
			else
				MoveActiveNewFurniture ();
		}
	}
	
	private void CheckActiveFurniture (){
		
		movelSelecionado = GameObject.FindGameObjectWithTag("MovelSelecionado");
		
		//Botão esquerdo
		if (Input.GetMouseButtonDown(0)) {
			
			//Deselecionar móvel selecionado se clicar com o botão esquerdo
			if (movelSelecionado != null) {
				movelSelecionado.GetComponentInChildren<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
				movelSelecionado.tag = "Movel";
				movelSelecionado.GetComponentInChildren<SnapBehaviour>().Select = false;					
				GetComponent<RenderBounds>().Display = false;
				infoController.Close();
			}
			
			RaycastHit hit = new RaycastHit ();
			Ray ray = camera.ScreenPointToRay (Input.mousePosition);
			GameObject[] moveis = GameObject.FindGameObjectsWithTag ("Movel");
			
			//Só continua se pegar algum móvel no ray cast do mouse
			//Se houverem moveis na cena
			if (moveis.Length > 0) {
				foreach (GameObject movel in moveis) {
					if (Physics.Raycast (ray, out hit, Mathf.Infinity) && hit.transform.tag != "Movel")
						continue;
					if (hit.transform == movel.transform) {
						movel.tag = "MovelSelecionado";
						movel.GetComponentInChildren<SnapBehaviour>().Select = true;
						movel.GetComponentInChildren<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
						GetComponent<RenderBounds>().Display = true;
						GetComponent<RenderBounds>().SetBox(movel);
						infoController.Open(movel.GetComponent<InformacoesMovel>());
					}
				}
			} else {
				print ("Não há mais objetos deselecionados.");
			}
		}
	}

	private void MoveActiveNewFurniture ()
	{
		
		if (!furnitureManager.isNewFurnitureActive ())
			return;
		
		GameObject activeFurniture = furnitureManager.GetActiveNewFurniture ();
		
		if (activeFurniture.tag != "Movel") {
			if (!Input.GetMouseButtonDown (0)) {
				RaycastHit hit;
				Ray ray = camera.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit)) {
					if (hit.transform.gameObject.layer == LayerMask.NameToLayer ("Chao")) {
						activeFurniture.transform.position = hit.point;
					}
				}
			} else {
				furnitureManager.FreeActiveNewFurniture ();
			}
		}
	}
}
