using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsserTOOLres {
	public class TrackingShot : MonoBehaviour {

		#region ===== ===== CALLBACK ===== =====

		/// <summary>
		/// callback funktion when Cutsceen is finished.
		/// </summary>
		public static System.Action _onFinish;

		#endregion
		#region ===== ===== DATA ===== =====

		Transform _rootNode;
		ICameraNode[] _nodes;

		Vector3 _cashNodePos;
		Quaternion _cashNodeRot;
		int _cashNodeIndex;
		int _currentNode = 0;
		bool _hasFinished = true;

		#endregion
		#region ===== ===== SINGELTON ===== =====

		static TrackingShot Singelton = null;
		void Awake() {
			if(Singelton == null) {
				Singelton = this;
			} else {
				Destroy(this);
			}
		}
		void OnDestroy() {
			if(Singelton = this) {
				Singelton = null;
			}
		}

		#endregion
		#region ===== ===== CORE ===== =====

		void Update() {
			if(!_hasFinished) {
				if(_currentNode >= _nodes.Length) {
					_rootNode.GetChild(_cashNodeIndex).transform.position = _cashNodePos;
					_rootNode.GetChild(_cashNodeIndex).transform.rotation = _cashNodeRot;
					_onFinish?.Invoke();
					return;
				}
				if(_nodes[_currentNode].NextTick()) {
					_currentNode++;
				}
			}
		}

		#endregion
		#region ===== ===== API ===== =====

		/// <summary>
		/// Starts the Tracking shot.
		/// _onFinish will be called when the Tracking shot has finished.
		/// </summary>
		/// <param name="rootTransform">the root transform inwitch all the cutsceens nodes are as first childs. node scripts have to inharent ICameraNode</param>
		/// <param name="camera">the camera with witch the cuttscene should be played</param>
		/// <param name="cykleToStart">if true last node gets set to startingpoint of the camera</param>
		public static void StartTrackingShot(Transform rootTransform, Transform camera, bool cykleToStart = false) {

			if(Singelton == null) {
				GameObject tmp = new GameObject();
				tmp.AddComponent<TrackingShot>();
				tmp.name = "TrackingShotHandler";
			}

			Singelton._rootNode = rootTransform;

			List<ICameraNode> tempList = new List<ICameraNode>();
			ICameraNode temp;
			Singelton._cashNodeIndex = -1;
			for(int i = 0; i < Singelton._rootNode.childCount; i++) {
				temp = Singelton._rootNode.GetChild(i).GetComponent<ICameraNode>();
				if(temp != null) {
					tempList.Add(temp);
					temp.SetCamera(camera);
					Singelton._cashNodeIndex = i;
				}
			}
			Singelton._nodes = tempList.ToArray();

			Transform Tmp = Singelton._rootNode.GetChild(Singelton._cashNodeIndex).transform;
			Singelton._cashNodePos = Tmp.position;
			Singelton._cashNodeRot = Tmp.rotation;
			if(cykleToStart) {
				Tmp.position = camera.transform.position;
				Tmp.rotation = camera.transform.rotation;
			}

			Singelton._currentNode = 0;
			Singelton._hasFinished = false;
		}

		#endregion
	}
}