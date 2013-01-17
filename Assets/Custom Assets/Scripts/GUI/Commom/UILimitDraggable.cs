using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpringPosition))]
public class UILimitDraggable : MonoBehaviour {

	public float betweenSize;
	public Transform referenceItems;
	public Camera camera;
	public Collider tiledCollider;
	
	private float timerTouch = 0.3f;
	
	private float initial;
	private SpringPosition springPosition;
	private float timer;
	private Vector3 initialMousePosition;
	private float yHeight;
	private bool isClicked;

	public int index {get; set;}
	public int limitIndex {get; set;}
	
	public bool IsLimit {
		get {
			return (index >= limitIndex);
		}
	}

	public float PositionPage {
		get {
			return (initial + (betweenSize * index));
		}
	}
	
	void Start () {
		
		if (limitIndex == 0 && referenceItems != null ) limitIndex = referenceItems.GetChildCount()-1;
		
		initial = transform.localPosition.x;
		yHeight = transform.localPosition.y;
		transform.localPosition = new Vector3(	(initial + (betweenSize * index)),
												yHeight,
												transform.localPosition.z);

		springPosition = GetComponent<SpringPosition>();
		springPosition.enabled = false;
	}
	
	void Update () {
		
		springPosition.worldSpace = false;
		
		if (Input.GetMouseButtonDown(0)) {
			timer = 0f;
			initialMousePosition = Input.mousePosition;
			if (tiledCollider != null) {
				Ray ray = camera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (!Physics.Raycast(ray, out hit))
				{
					isClicked = false;
				}
				else
				{
					isClicked = (hit.transform == tiledCollider.transform);
				}
			}
		}
		
		if (!Input.GetMouseButton(0)) {
			if (index < 0) {
				index = 0;
			}
			else if (IsLimit) {
				index = limitIndex;
			}
			springPosition.enabled = true;
			springPosition.target.x = PositionPage;
			springPosition.target.y = yHeight;
		}
		else {
			float getLocation = transform.localPosition.x - initial;
			index = (int)Mathf.Round((getLocation / betweenSize));
			
			if (timer < timerTouch) {
				timer += Time.deltaTime;
			}
		}
		
		if (Input.GetMouseButtonUp(0)) {
			if (tiledCollider != null)
			{
				if (!isClicked) return;
			}
			
			if (timer < timerTouch) {
				if (Input.mousePosition.x < initialMousePosition.x) {
					if (!IsLimit) {
						index++;
					}
				}
				if (Input.mousePosition.x > initialMousePosition.x) {
					if (index > 0) {
						index--;
					}
				}
			}
		}
		
	}
	
	public void SetIndex (int number) {
		if (number > 0 && number <= limitIndex) {
			transform.localPosition = new Vector3(	(initial + (betweenSize * number)),
													yHeight,
													transform.localPosition.z);
			index = number;
		}
	}
	
	/// <summary>
	/// Pass the index with: "next" and "prev".
	/// </summary>
	/// <summary>
	/// "next" - add index
	/// </summary>
	/// <summary>
	/// "prev" - remove index
	/// </summary>
	public void SetIndex (TypeIndexHandler typeIndex) {
		switch (typeIndex) {
		case TypeIndexHandler.Next:
			if (!IsLimit) index++;
			break;
		case TypeIndexHandler.Prev:
			if (index > 0) index--;
			break;
		}
	}

}