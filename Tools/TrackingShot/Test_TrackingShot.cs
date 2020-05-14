using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

public class Test_TrackingShot : MonoBehaviour {

	[SerializeField] Transform Camera;
	[SerializeField] Transform Cutsceen;

	void Start() {
		Restart();
	}

	void Restart() {
		TrackingShot.StartTrackingShot(Cutsceen, Camera, true);
		TrackingShot._onFinish += DoStuffWhenFinished;
	}

	void DoStuffWhenFinished() {
		TrackingShot.StartTrackingShot(Cutsceen, Camera);
		TrackingShot._onFinish -= DoStuffWhenFinished;
	}
}
