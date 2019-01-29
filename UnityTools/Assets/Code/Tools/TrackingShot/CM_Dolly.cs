﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsserTOOLres {
    public class CM_Dolly : MonoBehaviour, ICameraNode {

        [Tooltip("the time it takes to dolly to the new position")]
        [SerializeField] float _dollyTime;
        [Tooltip("the time the kamera will wait on the new position")]
        [SerializeField] float _waitingTime;
        [Tooltip("The new Field of View")]
        [SerializeField] float _targetFOV;

        float _collapsedTime = 0;

        bool _camErrorFlag;
        Transform _cam;
        Vector3 _startPos;
        Quaternion _startRot;
        float _startFOV;
        bool _finishedDolly = false;

        public bool NextTick() {
            return NextTick(Time.deltaTime);
        }

        public bool NextTick(float timeDelta) {
            if (_camErrorFlag) {
                print("Errorflag is set");
                return true;
            }

            if (_collapsedTime == 0) {
                _startPos = _cam.position;
                _startRot = _cam.rotation;
                _startFOV = _cam.GetComponent<Camera>().fieldOfView;
                _finishedDolly = false;
            }
            _collapsedTime += timeDelta;


            if (_collapsedTime >= _dollyTime + _waitingTime) {
                _collapsedTime = 0;
                return true;
            }

            float temp = _collapsedTime / _dollyTime;
            if (_collapsedTime < _dollyTime) {
                _cam.position = Vector3.Lerp(_startPos, transform.position, temp);
                _cam.rotation = Quaternion.Lerp(_startRot, transform.rotation, temp);
                _cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(_startFOV, _targetFOV, temp);
            } else if(!_finishedDolly) {
                _cam.position = transform.position;
                _cam.rotation = transform.rotation;
                _cam.GetComponent<Camera>().fieldOfView = _targetFOV;
                _finishedDolly = true;
            }

            return false;
        }

        public void SetCamera(Transform cam) {
            if (!cam.GetComponent<Camera>()) {
                _camErrorFlag = true;
                return;
            }
            _camErrorFlag = false;
            _cam = cam;
        }
    }
}