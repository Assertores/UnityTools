using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AsserTOOLres;

namespace AsserTOOLres_std {
	[RequireComponent(typeof(AIBehavior))]
	[RequireComponent(typeof(NavMeshAgent))]
	public class WAI_Wait : MonoBehaviour, IWaightedAIDesition {

		[Header("Behavior")]
		[SerializeField] AnimationCurve _waitTimeCurve;
		float _startTime = 0;

		[Header("Debug")]
		public float DBValue;

		NavMeshAgent _nMA;

		public void Initialice(AIBehavior aI) {
			_nMA = GetComponent<NavMeshAgent>();
		}

		public float Evaluate() {
			float value;

			value = _waitTimeCurve.Evaluate(Time.timeSinceLevelLoad - _startTime);

			DBValue = value;
			return value;
		}

		public void StartExecution() {
			_startTime = Time.timeSinceLevelLoad;
		}

		public void Execute() {

		}

		public void StopExecution() {

		}
	}
}