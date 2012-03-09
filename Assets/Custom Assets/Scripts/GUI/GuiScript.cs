using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GuiScript : MonoBehaviour
{
	public static uint BORDER_MARGIN = 5;
	private static bool showGUI;
	public static bool ShowGUI {
		set { 
		
			Debug.LogWarning("Changing gui state to: " + value.ToString() );
			
			showGUI = value;
			
			//Esconder cubo das direções
			GameObject rotacaoCubo =  GameObject.Find("RotacaoCubo");
			if(rotacaoCubo != null){
				Renderer[] meshRenders = rotacaoCubo.GetComponentsInChildren<Renderer>();
			
				foreach(Renderer render in meshRenders){
					render.enabled = showGUI;
				}	
			}
		}
		get { return showGUI; }
	}
	
	private int activeTexture;

	public GUISkin guiSkin;

	private GameObject movelSelecionado;
	private Camera mainCamera;

	#region GUIs
	private GuiDescription guiDescription;
	private GuiCatalogo guiCatalogo;
	private GuiCamera guiCamera;
	private List<Rect> allGuiWindows;
	#endregion

	private FurnitureManager furnitureManager;

	void Start ()
	{
		furnitureManager = transform.parent.GetComponent<FurnitureManager> ();
		
		guiDescription = GetComponent<GuiDescription> ();
		guiCatalogo = GetComponent<GuiCatalogo> ();
		guiCamera = GetComponent<GuiCamera> ();
		
		mainCamera = GameObject.FindGameObjectWithTag("Player").GetComponent<Camera>();
		
		allGuiWindows = new List<Rect> ();
		
		showGUI = true;
	}

	void Update ()
	{
		//Se clicar fora do GUI da esquerda verificar o móvel selecionado
		if (Input.GetMouseButtonDown(0) && 
		    !IsClickedInsideWindows() && 
		    !furnitureManager.isNewFurnitureActive()) 
		{
			CheckActiveFurniture();
		}
		else 
		{
			MoveActiveNewFurniture ();
		}
	}
	
	bool IsClickedInsideWindows ()
	{
		//Monkey patch ¬¬
		Vector3 position = Input.mousePosition;
		position.y = Screen.height - position.y;
		
		allGuiWindows.Clear();
		allGuiWindows.AddRange(guiDescription.GetWindows ());
		allGuiWindows.AddRange(guiCatalogo.GetWindows ());
		allGuiWindows.AddRange(guiCamera.GetWindows ());
		
		foreach (Rect wnd in allGuiWindows) 
		{
			if (wnd.Contains (position)) 
			{
				return true;
			}
		}
		return false;
	}

	void OnGUI ()
	{
		if (!Line.WasInitialized || !showGUI)
			return;
		
		GUI.skin = guiSkin;
		
		guiDescription.Draw();
		guiCatalogo.Draw();
		//guiCamera.Draw();
	}

	private void CheckActiveFurniture (){
		
		movelSelecionado = GameObject.FindGameObjectWithTag("MovelSelecionado");
		
		//Botão esquerdo
		if (Input.GetMouseButtonDown(0)) {
			if (!MouseUtils.MouseClickedInArea(guiCamera.wndOpenMenu) &&
			    !MouseUtils.MouseClickedInArea(guiCatalogo.wndAccordMain) &&
			    !MouseUtils.MouseClickedInArea(guiDescription.window)) {
				//Deselecionar móvel selecionado se clicar com o botão esquerdo
				if (movelSelecionado != null) {
					movelSelecionado.GetComponentInChildren<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
					movelSelecionado.tag = "Movel";
					movelSelecionado.GetComponentInChildren<SnapBehaviour>().Select = false;					
					mainCamera.GetComponent<RenderBounds>().Display = false;
				}
				
				RaycastHit hit = new RaycastHit ();
				Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
				GameObject[] moveis = GameObject.FindGameObjectsWithTag ("Movel");
				
				//Só continua se pegar algum móvel no ray cast do mouse
				//Se houverem moveis na cena
				if (moveis.Length > 0) {
					//print("Input.GetMouseButtonDown(0)");
					foreach (GameObject movel in moveis) {
						if (Physics.Raycast (ray, out hit, Mathf.Infinity) && hit.transform.tag != "Movel")
							continue;
						if (hit.transform == movel.transform) {
							movel.tag = "MovelSelecionado";
							movel.GetComponentInChildren<SnapBehaviour>().Select = true;
							movel.GetComponentInChildren<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
							mainCamera.GetComponent<RenderBounds>().Display = true;
							mainCamera.GetComponent<RenderBounds>().SetBox(movel);
						}
					}
					
					movelSelecionado = GameObject.FindGameObjectWithTag ("MovelSelecionado");
					
					if (movelSelecionado != null) {
											
						if (movelSelecionado.GetComponent<InformacoesMovel>() == null) {
							Debug.LogError ("The select furniture doesn't have a furniture data");
						}
						
	//					furnitureData.Position = movelSelecionado.transform.position;
	//					movelSelecionado.layer = LayerMask.NameToLayer("Moveis");
	//					furnitureData.Position = movelSelecionado.transform.position;
						//furnitureData.Size = movelSelecionado.transform.collider.bounds.size;
						guiDescription.UpdateData (movelSelecionado.GetComponent<InformacoesMovel>());
						guiDescription.Show();
					} else {
						guiDescription.Hide();
					}
				} else {
					print ("Não há mais objetos deselecionados.");
				}
			}
		}
	}

	private void MoveActiveNewFurniture ()
	{
		
		if (!furnitureManager.isNewFurnitureActive ())
			return;
		
		GameObject activeFurniture = GetComponent<FurnitureManager> ().GetActiveNewFurniture ();
		
		if (activeFurniture.tag != "Movel") {
			if (!Input.GetMouseButtonDown (0)) {
				RaycastHit hit;
				Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
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

	private void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.gameObject.tag != "Chao" && hit.gameObject.tag != "Parede") {
			Physics.IgnoreCollision (this.collider, hit.collider, true);
		}
	}
	
}
