using System.Collections;
using System.Collections.Generic;

namespace AsserTOOLres {
	/// <summary>
	/// With this class you can store and access variables from anywhere in you project.
	/// note: this is not thread save yet.
	/// </summary>
	public static class Blackboard {

		#region ===== ===== DATA ===== =====

		///<summary>
		/// All Blackboards of all types that have already been initialized.
		/// note: the ID is always a string.
		/// </summary>
		static Dictionary<System.Type, Dictionary<string, List<object>>> m_library = new Dictionary<System.Type, Dictionary<string, List<object>>>();

		#endregion
		#region ===== ===== API ===== =====

		public static void ClearBlackboard() {
			m_library = new Dictionary<System.Type, Dictionary<string, List<object>>>();
		}

		/// <summary>
		/// API;
		/// Adds a new Variable to the Blackboard.
		/// It will overwrite the value if the id already exists.
		/// </summary>
		/// <typeparam name="T">the type of the variable (will be detected automaticly through the variable)</typeparam>
		/// <param name="id">the ID of the variable</param>
		/// <param name="value">the variable to store</param>
		/// <param name="pos">the position, at which the value should be written to. (if you want to save multible values for one id) default value = 0</param>
		public static void AddValueToBlackboard<T>(string id, T value, int pos = 0) {
			if(!m_library.ContainsKey(typeof(T))) {
				NewBlackboard<T>();
			}

			if(!m_library[typeof(T)].ContainsKey(id)) {
				m_library[typeof(T)].Add(id, new List<object>());
			}
			if(m_library[typeof(T)][id].Count <= pos) {
				for(int i = m_library[typeof(T)][id].Count; i <= pos; i++) {
					m_library[typeof(T)][id].Add(default);
				}
			}
			m_library[typeof(T)][id][pos] = value;
		}

		/// <summary>
		/// API;
		/// Accesses a variable that has already been set.
		/// </summary>
		/// <typeparam name="T">the type of the variable (will be detected automaticly through the variable)</typeparam>
		/// <param name="id">the ID of the variable</param>
		/// <param name="value">the variable the value will be stored at</param>
		/// <param name="pos">the position to the value to access (if you have multible values at one id) default value = 0</param>
		/// <returns>true, if the value was found and has been set</returns>
		public static bool GetValueFromBlackboard<T>(out T value, string id, int pos = 0) {
			if(!m_library.ContainsKey(typeof(T)) || !m_library[typeof(T)].ContainsKey(id) || m_library[typeof(T)][id].Count <= pos || m_library[typeof(T)][id][pos] == null) {
				value = default(T);
				return false;
			}

			value = (T)m_library[typeof(T)][id][pos];
			return true;
		}

		#endregion
		#region ===== ===== CORE ===== =====

		/// <summary>
		/// !!USE API!!
		/// Creates an new Blackboard.
		/// </summary>
		/// <typeparam name="T">the Type of the Blackboard (int, string, Transform ...)</typeparam>
		/// <returns>false, if this BlackboardType already existes.</returns>
		public static bool NewBlackboard<T>() {
			if(m_library.ContainsKey(typeof(T))) {
				return false;
			}

			m_library.Add(typeof(T), new Dictionary<string, List<object>>());
			return true;
		}

		#endregion
	}
}