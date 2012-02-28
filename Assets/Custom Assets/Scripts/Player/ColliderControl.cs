using UnityEngine;
using System.Collections;

public class ColliderControl : MonoBehaviour {
	
	public Camera3d camera3d;
	
	private GameObject lastSelectedMobile;
	void OnEnable () {
		if (GameObject.FindGameObjectsWithTag("MovelSelecionado").Length == 1) {
			lastSelectedMobile = GameObject.FindWithTag("MovelSelecionado");
			lastSelectedMobile.tag = "Movel";
		}
		GameObject[] moveis = GameObject.FindGameObjectsWithTag("Movel");
		foreach (GameObject movel in moveis) {
			movel.collider.isTrigger = true;
		}
		
		int i = 0;
		foreach (Transform parede in camera3d.paredesParents.GetWalls()) {
			parede.renderer.material = camera3d.paredeMaterial;
			parede.renderer.material.color = camera3d.paredesParents.GetWallColor(i);
			parede.collider.enabled = true;
			parede.collider.isTrigger = false;
			++i;
		}
		GameObject teto = GameObject.FindWithTag("TetoParent");
		if (teto.renderer != null && !teto.renderer.enabled)
			teto.renderer.enabled = true;
	}
	
	// Update is called once per frame
	void OnDisable () {
		GameObject[] moveis = GameObject.FindGameObjectsWithTag("Movel");
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
		
		foreach (Transform parede in camera3d.paredesParents.GetWalls()) {
			parede.collider.isTrigger = true;
		}
		foreach (Transform teto in GameObject.Find("ParentTeto").transform) {
			if (teto.renderer != null)
				teto.renderer.enabled = camera3d.AreWallsAlwaysVisible;
		}
	}
	
	void Update () {
		if (Input.GetKeyUp(KeyCode.Escape)) {
			SnapBehaviour.ActivateAll();
			gameObject.SetActiveRecursively(false);
			foreach (Transform child in GameObject.Find("RotacaoCubo").transform) {
				child.gameObject.SetActiveRecursively(true);
			}
			camera3d.gameObject.SetActiveRecursively(true); 
		}
	}
}
