using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

public class Test_WaightedAI : MonoBehaviour {

	[SerializeField] float _reevaluateTime = 2;

	float _lastReevaluate = 0;

	void Start() {

	}

	void Update() {

		if(Time.timeSinceLevelLoad - _lastReevaluate >= 2) {
			_lastReevaluate = Time.timeSinceLevelLoad;
			foreach(var it in AIBehavior._aIReverences) {
				it.Reevaluate();
			}
		}

	}
}
