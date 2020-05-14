using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsserTOOLres {
	/// <summary>
	/// Localisation scripts must always have a public get list of Scriptreferences named references
	/// Localisation scripts must always have a public void function called ChangeContent(string Language)
	/// </summary>
	public class Localize_text : MonoBehaviour {

		#region ===== ===== DATA ===== =====

		/// <summary>
		/// a list of all referenced instances of this script
		/// </summary>
		public static List<Localize_text> references { get; private set; } = new List<Localize_text>();

		[SerializeField] Text[] _textField; //reference to the Textfield
		[SerializeField] string _id; //the id of the string

		#endregion
		#region ===== ===== REFRENCING ===== =====

		/// <summary>
		/// registers this script to the static List if it is valid
		/// if _textField is not set, it tries to find a text component in the gameobject
		/// </summary>
		private void Awake() {
			if(_textField.Length == 0) {
				_textField = new Text[1] { GetComponent<Text>() };

				if(_textField.Length == 0) {
					return;
				}
			}
			references.Add(this);
		}

		/// <summary>
		/// unregisters this script
		/// </summary>
		private void OnDestroy() {
			references.Remove(this);
		}

		#endregion
		#region ===== ===== API ===== =====

		/// <summary>
		/// call this to get the id set in the Inspektor
		/// </summary>
		/// <returns>the ID set in the Inspektor</returns>
		public string GetID() {
			return _id;
		}

		/// <summary>
		/// changes all referenced texts.
		/// no parameter will clear the texts.
		/// </summary>
		/// <param name="Content">the text that will be displayed</param>
		public void ChangeContent(string text = "") {
			foreach(var it in _textField) {
				it.text = text;
			}
		}

		#endregion
	}
}