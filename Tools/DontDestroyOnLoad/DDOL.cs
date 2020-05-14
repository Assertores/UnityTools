using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsserTOOLres {
	public class DDOL : MonoBehaviour {

		#region ===== ===== CORE ===== =====

		private void Awake() {
			DontDestroyOnLoad(this.gameObject);
			Destroy(this);
		}

		#endregion
	}
}
