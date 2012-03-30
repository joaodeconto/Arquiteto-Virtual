using UnityEngine;
using System.Collections;

public class ColliderControl : MonoBehaviour {
	
	private CameraController cameraController;
	
	private GameObject lastSelectedMobile;
	public bool IsPanelFloor {get; set;}

	void Start () {
		cameraController = GameObject.FindWithTag("GameController").GetComponentInChildren<CameraController>();
	}
	
	public void Enable () {
		
		Screen.lockCursor = true;
		
		if (GameObject.FindGameObjectsWithTag("MovelSelecionado").Length == 1) {
			lastSelectedMobile = GameObject.FindWithTag("MovelSelecionado");
			lastSelectedMobile.tag = "Movel";
		}
		GameObject[] moveis = GameObject.FindGameObjectsWithTag("Movel");
		if (moveis.Length != 0) {
			foreach (GameObject movel in moveis) {
				movel.collider.isTrigger = true;
			}
		}
		
		GameObject teto = GameObject.Find("ParentTeto");
		if  (teto != null && teto.renderer != null)
			teto.renderer.enabled = teto.collider.enabled = true;
		
		int i = 0;
		foreach (WallColor wallColor in cameraController.walls) {
			if (wallColor != null)
			{
				wallColor.wall.renderer.material = cameraController.wallMaterial;
				wallColor.wall.renderer.material.color = wallColor.color;
				wallColor.wall.collider.enabled = true;
				wallColor.wall.collider.isTrigger = false;
				++i;
			}
		}
		
		cameraController.interfaceGUI.uiRootFPS.SetActiveRecursively (true);
	}
	
	// Update is called once per frame
	public void Disable () {
		cameraController.interfaceGUI.uiRootFPS.SetActiveRecursively (false);
		
		GameObject[] moveis = GameObject.FindGameObjectsWithTag("Movel");
		if (moveis.Length != 0) {
			foreach (GameObject movel in moveis) {
				movel.collider.isTrigger = false;
				if (movel.GetComponent<InformacoesMovel>().portas != Portas.FECHADAS) {
					Animation[] animacoes = movel.GetComponentsInChildren<Animation>();
					foreach (Animation animacao in animacoes) {
						if (animacao.clip != null) {
							animacao[animacao.clip.name].speed = -1;
							animacao[animacao.clip.name].time = animacao[animacao.clip.name].length;
							animacao.Play();
						}
					}
					movel.GetComponent<InformacoesMovel>().portas = Portas.FECHADAS;
				}
			}
			if (lastSelectedMobile != null) {
				lastSelectedMobile.tag = "MovelSelecionado";
				lastSelectedMobile = null;
			}
		}
		
		if (cameraController.walls != null)
		{
			foreach (WallColor wallColor in cameraController.walls)
			{
				wallColor.wall.collider.isTrigger = true;
			}
		}
		GameObject teto = GameObject.Find("ParentTeto");
		if  (teto != null && teto.renderer != null)
			teto.renderer.enabled = teto.collider.enabled = true;
					
	}
	
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape)) {
			SnapBehaviour.ActivateAll();
			cameraController.mainCamera.gameObject.SetActiveRecursively(true);
			cameraController.setFirstPerson = false;
			cameraController.interfaceGUI.main.SetActiveRecursively(true);
			cameraController.interfaceGUI.uiRootFPS.SetActiveRecursively (false);
			
			Disable();
			if (!IsPanelFloor) {
				cameraController.interfaceGUI.viewUIPiso.SetActiveRecursively(false);
				cameraController.interfaceGUI.panelFloor.SetActiveRecursively(false);
			}
			if (lastSelectedMobile == null) {
				if (cameraController.interfaceGUI.panelInfo.active) {
					cameraController.interfaceGUI.panelInfo.SetActiveRecursively(false);
					cameraController.interfaceGUI.panelMobile.SetActiveRecursively(false);
				}
			}
			
			gameObject.SetActiveRecursively(false);
		}
	}
}
