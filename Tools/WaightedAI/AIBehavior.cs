using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsserTOOLres {
	public class AIBehavior : MonoBehaviour {

		public static List<AIBehavior> _aIReverences = new List<AIBehavior>();

		IWaightedAIDesition[] _behaviors;

		int _currentBehavior = -1;

		void Awake() {
			_aIReverences.Add(this);
		}
		void OnDestroy() {
			_aIReverences.Add(this);
		}

		void Start() {
			_behaviors = GetComponents<IWaightedAIDesition>();

			foreach(var it in _behaviors) {
				it.Initialice(this);
			}

			Reevaluate();
		}

		public void Reevaluate() {
			int tempBehavior = -1;
			float currentWaight = float.MinValue;
			float tmp;
			for(int i = 0; i < _behaviors.Length; i++) {
				tmp = _behaviors[i].Evaluate();
				if(tmp > currentWaight) {
					tempBehavior = i;
					currentWaight = tmp;
				}
			}

			if(_currentBehavior != tempBehavior) {
				if(_currentBehavior >= 0)
					_behaviors[_currentBehavior].StopExecution();
				_currentBehavior = tempBehavior;
				_behaviors[_currentBehavior].StartExecution();
			}
		}

		void Update() {
			if(_currentBehavior < 0 || _currentBehavior >= _behaviors.Length)
				return;

			_behaviors[_currentBehavior].Execute();
		}
	}
}