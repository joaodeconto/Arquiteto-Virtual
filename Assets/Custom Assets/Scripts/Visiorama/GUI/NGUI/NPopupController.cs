using UnityEngine;
using System.Collections.Generic;

public class NPopupController : MonoBehaviour {

	private Transform NPopup;
	private Queue<string> messages;
	private bool IsShowingMessage;

	private float timeBetweenCalls;

	private float movementTime;

	void Start ()
	{
		if (GameObject.Find("NPopup") == null)
		{
			Debug.LogError ("Não há NPopup na cena!");
			return;
		}

		this.NPopup = GameObject.Find("NPopup").transform;

		if (!NPopup.transform.parent.name.Equals ("Panel") ||
			!NPopup.transform.parent.parent.name.Equals ("Anchor") ||
			!NPopup.transform.parent.parent.parent.name.Equals ("Camera Popup"))
		{
			Debug.LogError ("NPopup não está na estrutura certa!");
		}
		else
		{
			Debug.Log ("tud certs!");
		}

		messages = new Queue<string>();

		IsShowingMessage = false;

		timeBetweenCalls = 5.0f;

		movementTime = 1.0f;
	}

	public void ShowMessage (string message)
	{
		messages.Enqueue(message);

		if (!IsShowingMessage)
		{
			InvokeRepeating("ShowMessage",timeBetweenCalls, timeBetweenCalls);

			IsShowingMessage = true;
		}
	}

	private void ShowMessage ()
	{
		if (messages.Count == 0)
		{
			CancelInvoke("ShowMessage");
			IsShowingMessage = false;
			return;
		}

		string cMessage = messages.Dequeue();

//		iTween.MoveTo(NPopup.transform.
	}
}
