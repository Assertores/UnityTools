/// @author J-D Vbk
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A simple tool to check which UI Raycast Target is hit by a Mouseclick on a certain Position
/// </summary>
public class UIRayCastCheck : MonoBehaviour {
	enum MouseButton {
		LeftButton,
		RightButton,
		Middle
	}

	[SerializeField]
	MouseButton trigger = MouseButton.RightButton;

	[Header("Last Result")]
	public Vector3 clickedPosition;
	public GameObject target;
	public string layer;
	public int order;
	private string output;
	public string position;

	[Space(5)]
	[SerializeField]
	private UnityEngine.UI.GraphicRaycaster[] canvasActive;

	EventSystem eventSystem;
	PointerEventData pointerData;
	List<RaycastResult> results = new List<RaycastResult>();

	private void Start() {
		Debug.Log("UIRayCastCheck: " + gameObject.name + "(" + gameObject.scene.name + ")");
		DontDestroyOnLoad(this);
	}

	// Update is called once per frame
	void Update() {
		if(Input.GetMouseButtonDown((int)trigger)) {
			pointerData = new PointerEventData(EventSystem.current);
			clickedPosition = Input.mousePosition;
			pointerData.position = clickedPosition;


			// currently active canvas
			canvasActive = FindObjectsOfType<UnityEngine.UI.GraphicRaycaster>();

			output = "_____" + canvasActive.Length + " Canvas active!_____";
			Transform next;
			for(int i = 0; i < canvasActive.Length; i++) {
				next = canvasActive[i].transform;
				output += "\n" + canvasActive[i].gameObject.name;
				while(next.parent) {
					next = next.parent;
					output += "\n\t" + next.gameObject.name;
				}
				output += "\n\t(" + canvasActive[i].gameObject.scene.name + ")";
			}
			Debug.Log(output);


			// hits
			target = null;
			int previous = 0;
			for(int ci = 0; ci < canvasActive.Length; ci++) {
				// append hits to results
				canvasActive[ci].Raycast(pointerData, results);

				if(results.Count > previous) {
					for(int ri = previous; ri < results.Count; ri++) {
						target = results[ri].gameObject;
						layer = SortingLayer.IDToName(results[ri].sortingLayer);
						order = results[ri].sortingOrder;
						output = string.Format("{0}, {1}, {2}\n",
										target.name, layer, order);
						next = target.transform;
						position = " ";
						while(next.parent) {
							next = next.parent;
							position += next.gameObject.name + " \n ";
						}
						output += position + "(" + target.scene.name + ")\n" + results[ri];
						Debug.Log(output);
					}
					previous = results.Count;
				}
			}
			results.Clear();
		}
	}
}
