using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AsserTOOLres;

namespace AsserTOOLres_std {
	[RequireComponent(typeof(AIBehavior))]
	[RequireComponent(typeof(NavMeshAgent))]
	public class WAI_FolowPath : MonoBehaviour, IWaightedAIDesition {

		[Header("References")]
		[SerializeField] Transform _pathRootNode;

		[Header("Behavior")]
		[SerializeField] AnimationCurve _timeSinceLastNodeReachedCurve;
		float _timeSinceLastNodeReached = 0;
		[SerializeField] AnimationCurve _distanceToTargetCurve;

		[Header("Balancing")]
		[SerializeField] int _firstTarget = 0;

		[Header("Debug")]
		public float DBValue;

		AIBehavior _parent;
		NavMeshAgent _nMA;

		public void Initialice(AIBehavior aI) {
			_parent = aI;
			_nMA = GetComponent<NavMeshAgent>();
		}

		public float Evaluate() {
			float value;

			value = _timeSinceLastNodeReachedCurve.Evaluate(Time.timeSinceLevelLoad - _timeSinceLastNodeReached);
			value *= _distanceToTargetCurve.Evaluate(Vector3.Distance(_pathRootNode.GetChild(_firstTarget).position, transform.position));

			DBValue = value;
			return value;
		}

		public void StartExecution() {
			_nMA.SetDestination(_pathRootNode.GetChild(_firstTarget).position);
		}

		public void Execute() {

			if(_nMA.remainingDistance <= _nMA.stoppingDistance) {

				NextTaget();
				return;
			}

			_nMA.SetDestination(_pathRootNode.GetChild(_firstTarget).position);
		}

		void NextTaget() {
			_firstTarget++;
			_firstTarget %= _pathRootNode.childCount;

			_timeSinceLastNodeReached = Time.timeSinceLevelLoad;

			_parent.Reevaluate();
		}

		public void StopExecution() {
			_nMA.SetDestination(transform.position);
		}
	}
}