using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace AsserTOOLres_std {
	public class CN_Rotate : MonoBehaviour, ICameraNode {

		[Tooltip("the time it takes to rotate to the new position")]
		[SerializeField] float _rotateTime = 2;
		[Tooltip("the time the kamera will wait on the new position")]
		[SerializeField] float _waitingTime = 2;
		[Tooltip("The new Field of View")]
		[SerializeField] float _targetFOV = 60;
		[Tooltip("the ratio of elapst time between 0 and 1 at x and the ratio of elapst position between 0 and 1 of y")]
		[SerializeField]
		AnimationCurve _lerpCurve;

		[Tooltip("determins if the camera will look towords the center or away from the center")]
		[SerializeField] bool _LockCameraInward = true;
		[Tooltip("stops the Camera to clip throw walls")]
		[SerializeField] bool _doCameraCollition = true;
		[Tooltip("the target distence from the center point. stays the same as the start distance if it is negative")]
		[SerializeField] float _targetDistance = -1;

		float _collapsedTime = 0;

		bool _camErrorFlag;
		Transform _cam;
		Vector3 _startPos;
		float _startDistance;
		Quaternion _startRot;
		float _startFOV;
		bool _finishedRotatoin = false;

		public bool NextTick() {
			return NextTick(Time.deltaTime);
		}

		public bool NextTick(float timeDelta) {
			if(_camErrorFlag) {
				return true;
			}

			if(_collapsedTime == 0) {
				_startPos = _cam.position;
				_startRot = _cam.rotation;
				_startFOV = _cam.GetComponent<Camera>().fieldOfView;
				_startDistance = Vector3.Distance(_cam.position, transform.position);
				_finishedRotatoin = false;
				if(_targetDistance < 0) {
					_targetDistance = _startDistance;
				}
			}
			_collapsedTime += timeDelta;

			if(_collapsedTime >= _rotateTime + _waitingTime) {
				_collapsedTime = 0;
				return true;
			}

			float temp = _lerpCurve.Evaluate(_collapsedTime < _rotateTime ? _collapsedTime / _rotateTime : 1);
			if(!_finishedRotatoin) {
				_cam.rotation = Quaternion.Slerp(Quaternion.LookRotation((transform.position - _startPos).normalized), Quaternion.LookRotation(transform.forward), temp);
				_cam.position = transform.position;
				float dist = Mathf.Lerp(_startDistance, _targetDistance, temp);
				if(_doCameraCollition) {
					Ray ray = new Ray(transform.position, -_cam.forward);
					RaycastHit hit;
					if(Physics.Raycast(ray, out hit, dist)) {
						dist = Vector3.Distance(transform.position, hit.point) - 1f;
					}
				}
				_cam.Translate(-Vector3.forward * dist);
				_cam.rotation = Quaternion.Slerp(_startRot, Quaternion.LookRotation(transform.forward * ((_LockCameraInward) ? 1 : -1)), temp);
				_cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(_startFOV, _targetFOV, temp);
				if(_collapsedTime >= _rotateTime) {
					_finishedRotatoin = true;
				}
			}

			return false;
		}

		public void SetCamera(Transform cam) {
			if(!cam.GetComponent<Camera>()) {
				_camErrorFlag = true;
				return;
			}
			_camErrorFlag = false;
			_cam = cam;
		}
	}
}