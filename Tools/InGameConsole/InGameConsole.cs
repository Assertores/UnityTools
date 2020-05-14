using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsserTOOLres {
	public static class InGameConsole {

		#region ===== ===== DATA ===== =====

		static Dictionary<string, string> _aliasList = new Dictionary<string, string>();

		static Dictionary<string, System.Action<string>> _commandList = new Dictionary<string, System.Action<string>>();

		static Text _text = null;

		static string _buffer = "";

		#endregion
		#region ===== ===== CORE ===== =====

		static void InputFieldCommand(InputField field) {
			string temp = field.text;
			field.text = "";
			InterpreteCommand(temp);
		}

		#endregion
		#region ===== ===== API ===== =====

		/// <summary>
		/// setzt up the Console, sothat it automaticly reads in the commands.
		/// </summary>
		/// <param name="textField">reference to the Textfeald to write to</param>
		/// <param name="mainInputField">reference to the InputField to get commands from</param>
		public static void Inizialice(Text textField, InputField mainInputField) {
			mainInputField.onEndEdit.AddListener(delegate { InputFieldCommand(mainInputField); });
			Inizialice(textField);
		}

		/// <summary>
		/// Sets up the console
		/// </summary>
		/// <param name="textField">reference to the Textfield to write to</param>
		public static void Inizialice(Text textField) {
			_text = textField;
			_text.text += _buffer;
			_buffer = "";
		}

		/// <summary>
		/// writes to the referenced Textfield.
		/// if there isn't any it will write to a buffer.
		/// </summary>
		/// <param name="message">the message to write</param>
		public static void WriteToConsole(string message) {
			if(_text == null) {
				if(_buffer != "") {
					_buffer += System.Environment.NewLine;
				}
				_buffer += message;
			} else {
				if(_text.text != "") {
					_text.text += System.Environment.NewLine;
				}
				_text.text += message;
				_text.rectTransform.sizeDelta = new Vector2(_text.rectTransform.sizeDelta.x, _text.preferredHeight);
			}
			Debug.Log(message);
		}

		/// <summary>
		/// adds a Lisender Funktion to a command.
		/// if the comand dosn't exist it will create it.
		/// </summary>
		/// <param name="command">the command</param>
		/// <param name="Lisener">funktion pointer to the Lisener funktion that will be added</param>
		public static void AddListener(string command, System.Action<string> Lisener) {
			if(_aliasList.ContainsKey(command)) {
				command = _aliasList[command];
			}

			if(_commandList.ContainsKey(command)) {
				_commandList[command] += Lisener;
			} else {
				_commandList.Add(command, Lisener);
			}
		}

		/// <summary>
		/// Removes the Lisener from the command
		/// if there are no mor Liseners left the comand will be deleted
		/// </summary>
		/// <param name="command">the command</param>
		/// <param name="Lisener">funktion pointer to the Lisener funktion that should be removed</param>
		public static void RemoveListener(string command, System.Action<string> Lisener) {
			if(_aliasList.ContainsKey(command)) {
				command = _aliasList[command];
			}

			if(_commandList.ContainsKey(command)) {
				_commandList[command] -= Lisener;
				if(_commandList[command] == null) {
					_commandList.Remove(command);
				}
			}
		}

		/// <summary>
		/// adds an command with no Liseners atached
		/// </summary>
		/// <param name="command">the command to add</param>
		public static void AddCommand(string command) {
			if(_commandList.ContainsKey(command) || _aliasList.ContainsKey(command))
				return;
			_commandList.Add(command, WriteToConsole);
			_commandList[command] -= WriteToConsole;
		}

		/// <summary>
		/// compares the start of the command string to the registered commands
		/// </summary>
		/// <param name="command">the command to be interpretad</param>
		/// <returns>true if the command was successfully interpreted</returns>
		public static bool InterpreteCommand(string command) {
			foreach(var it in _commandList) {
				if(command.StartsWith(it.Key)) {
					it.Value?.Invoke(command.Substring(it.Key.Length).Trim());
					return true;
				}
			}
			foreach(var it in _aliasList) {
				Debug.Log("im here");
				if(command.StartsWith(it.Key)) {

					_commandList[it.Value]?.Invoke(command.Substring(it.Key.Length).Trim());
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// create one alias for one key
		/// </summary>
		/// <param name="key">the key the alias should be redirekted</param>
		/// <param name="alias">the alias command</param>
		public static void CreateAlias(string key, string alias) {
			if(_aliasList.ContainsKey(alias)) {
				return;
			}
			if(_aliasList.ContainsKey(key)) {
				key = _aliasList[key];
			}
			if(_commandList.ContainsKey(key)) {
				_aliasList.Add(alias, key);
			}
		}

		/// <summary>
		/// creates aliases for one key
		/// </summary>
		/// <param name="key">the key all the aliases should be redirekted</param>
		/// <param name="alias">the alias commands</param>
		public static void CreateAlias(string key, string[] alias) {
			if(_aliasList.ContainsKey(key)) {
				key = _aliasList[key];
			}
			foreach(var it in alias) {
				if(_aliasList.ContainsKey(it)) {
					continue;
				}
				if(_commandList.ContainsKey(key)) {
					_aliasList.Add(it, key);
				}
			}

		}

		#endregion
	}
}