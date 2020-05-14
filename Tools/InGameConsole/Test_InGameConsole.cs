using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AsserTOOLres;

public class Test_InGameConsole : MonoBehaviour {

	[SerializeField] Text _textField;
	[SerializeField] InputField _inputField;

	void Start() {
		Retest();
	}

	void Retest() {
		InGameConsole.WriteToConsole("This Message is printed befor the textfield is added");

		InGameConsole.Inizialice(_textField);

		InGameConsole.WriteToConsole("This Message is printed alfter the textfield is added");

		if(InGameConsole.InterpreteCommand("/test")) {
			InGameConsole.WriteToConsole("/test command was found");
		} else {
			InGameConsole.WriteToConsole("/test command was not found");
		}

		InGameConsole.AddCommand("/test");

		if(InGameConsole.InterpreteCommand("/test")) {
			InGameConsole.WriteToConsole("/test command was found");
		} else {
			InGameConsole.WriteToConsole("/test command was not found");
		}

		InGameConsole.AddListener("/test", OnTestCommand);

		if(InGameConsole.InterpreteCommand("/test")) {
			InGameConsole.WriteToConsole("/test command was found");
		} else {
			InGameConsole.WriteToConsole("/test command was not found");
		}

		InGameConsole.AddListener("/Retest", RetestL);

		if(InGameConsole.InterpreteCommand("/Retest")) {
			InGameConsole.WriteToConsole("/Retest command was found");
		} else {
			InGameConsole.WriteToConsole("/Retest command was not found");
		}

		InGameConsole.RemoveListener("/Retest", RetestL);

		if(InGameConsole.InterpreteCommand("/Retest")) {
			InGameConsole.WriteToConsole("/Retest command was found");
		} else {
			InGameConsole.WriteToConsole("/Retest command was not found");
		}

		InGameConsole.AddListener("/Retest", RetestL);

		InGameConsole.AddListener("/Close", OnCloseConsole);

		InGameConsole.CreateAlias("/Close", "/close");
		InGameConsole.CreateAlias("/close", new[] { "/Quit", "/quit" });

		InGameConsole.Inizialice(_textField, _inputField);

		InGameConsole.WriteToConsole("try typing ->/test<- or ->/Retest<- to the InputField");
		InGameConsole.WriteToConsole("/Close will close the console");

	}

	void RetestL(string message) {
		InGameConsole.WriteToConsole("this would crash the programm sorry");
		//Retest();
	}

	void OnTestCommand(string message) {
		InGameConsole.WriteToConsole("/test command found with ->" + message + "<- added alfter it");
	}

	void OnCloseConsole(string message) {
		_inputField.transform.parent.gameObject.SetActive(false);
	}
}
