using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

namespace AsserTOOLres_std {
	public class CN_Fade : MonoBehaviour, ICameraNode {

		[Tooltip("The time the camera takes to komplitly fade out")]
		[SerializeField]
		float _fadeOutTime;
		[Tooltip("The time the camera stays out faded")]
		[SerializeField]
		float _stayFadedOut;
		[Tooltip("The time the camera takes to komplitly fade in")]
		[SerializeField]
		float _fadeInTime;
		[Tooltip("The time the camera waits alfter the fade has completed")]
		[SerializeField]
		float _waitTime;
		[Tooltip("The Field of View after the fade")]
		[SerializeField]
		float _targetFOV;

		[Tooltip("the color the camera fades in to and out of")]
		[SerializeField]
		Color _fadeColor;
		[Tooltip("the ratio of elapst time between 0 and 1 at x and the ratio of the alpha value fade in between 0 and 1 at y")]
		[SerializeField]
		AnimationCurve _lerpOutCurve;
		[Tooltip("the ratio of elapst time between 0 and 1 at x and the ratio of the alpha value fade out between 0 and 1 at y")]
		[SerializeField]
		AnimationCurve _lerpInCurve;

		bool _camErrorFlag;
		Transform _cam;
		float _startTime = 0;
		float _collapsedTime = 0;
		bool _changedPosition;
		bool _startWaiting;

		Color _currentColor;

		Texture2D _fadePixel = null;

		void Start() {
			_fadePixel = new Texture2D(1, 1);
			_currentColor = _fadeColor;
			_currentColor.a = 0.0f;
			_fadePixel.SetPixel(0, 0, _currentColor);
			_fadePixel.Apply();
		}

		public bool NextTick() {
			if(_camErrorFlag) {
				print("Errorflag is set");
				return true;
			}

			if(_startTime == 0) {
				_fadePixel.SetPixel(0, 0, _currentColor);
				_fadePixel.Apply();

				_changedPosition = false;
				_startWaiting = false;
				_startTime = Time.timeSinceLevelLoad;

			}
			if(Time.timeSinceLevelLoad > _startTime + _fadeOutTime + _stayFadedOut + _fadeInTime + _waitTime) {

				_startTime = 0;
				return true;
			}

			float temp = 0;
			_collapsedTime = Time.timeSinceLevelLoad - _startTime;
			if(_collapsedTime < _fadeOutTime) {
				temp = _lerpOutCurve.Evaluate(_collapsedTime / _fadeOutTime);
				_currentColor.a = Mathf.Lerp(0.0f, _fadeColor.a, temp);
				_fadePixel.SetPixel(0, 0, _currentColor);
				_fadePixel.Apply();

				return false;
			}

			_collapsedTime -= _fadeOutTime;
			if(_collapsedTime < _stayFadedOut) {
				if(!_changedPosition) {
					_cam.transform.position = transform.position;
					_cam.transform.rotation = transform.rotation;
					_currentColor.a = _fadeColor.a * _lerpOutCurve.Evaluate(1);
					_fadePixel.SetPixel(0, 0, _currentColor);
					_fadePixel.Apply();

					_cam.GetComponent<Camera>().fieldOfView = _targetFOV;

					_changedPosition = true;
				}
				return false;
			}

			_collapsedTime -= _stayFadedOut;

			if(_collapsedTime < _fadeOutTime) {
				temp = _lerpInCurve.Evaluate(_collapsedTime / _fadeInTime);
				_currentColor.a = Mathf.Lerp(_fadeColor.a, 0.0f, temp);
				_fadePixel.SetPixel(0, 0, _currentColor);
				_fadePixel.Apply();
				return false;
			}

			if(!_startWaiting) {
				_fadePixel.SetPixel(0, 0, new Color(0, 0, 0, 0));
				_fadePixel.Apply();
				_startWaiting = true;
			}

			return false;
		}

		void OnGUI() {
			if(_startTime != 0) {
				GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _fadePixel);
			}
		}

		public bool NextTick(float timeDelta) {
			return NextTick();
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