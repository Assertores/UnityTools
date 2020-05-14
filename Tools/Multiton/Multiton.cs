using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsserTOOLres {
	public abstract class Multiton<T> : MonoBehaviour where T : MonoBehaviour {

		#region ===== ===== API ===== =====

		public static List<T> s_references { get; private set; } = new List<T>();

		#endregion
		#region ===== ===== VIRTUAL ===== =====

		protected virtual void OnMyAwake() { }

		protected virtual void OnMyDestroy() { }

		#endregion
		#region ===== ===== CORE ===== =====

		void Awake() {
			s_references.Add(this as T);

			OnMyAwake();
		}

		private void OnDestroy() {
			OnMyDestroy();

			s_references.Remove(this as T);
		}

		#endregion
	}
}
