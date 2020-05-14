using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AsserTOOLres;

namespace AsserTOOLres_std {
	[RequireComponent(typeof(AIBehavior))]
	[RequireComponent(typeof(NavMeshAgent))]
	public class WAI_Flee : MonoBehaviour, IWaightedAIDesition {

		[Header("References")]
		[SerializeField] Transform _fleeFrom;

		[Header("Behavior")]
		[SerializeField] AnimationCurve _fleeDistanceCurve;

		[Header("Debug")]
		public float DBValue;

		AIBehavior _parent;
		NavMeshAgent _nMA;

		public float Evaluate() {
			float value;

			value = _fleeDistanceCurve.Evaluate(Vector3.Distance(transform.position, _fleeFrom.position));

			DBValue = value;
			return value;
		}

		public void Execute() {
			_nMA.SetDestination(transform.position + (transform.position - _fleeFrom.position));
		}

		public void Initialice(AIBehavior aI) {
			_parent = aI;
			_nMA = GetComponent<NavMeshAgent>();
		}

		public void StartExecution() {
		}

		void FixedUpdate() {
			if(_fleeDistanceCurve.Evaluate(Vector3.Distance(transform.position, _fleeFrom.position)) > 0.8f) {
				_parent.Reevaluate();
			}
		}

		public void StopExecution() {

		}
	}
}