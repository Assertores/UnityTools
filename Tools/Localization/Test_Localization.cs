using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

public class Test_Localization : MonoBehaviour {

	void Start() {
		StartCoroutine(Retest());
	}

	IEnumerator Retest() {
		print("i'm changing the Language to German on " + Localize_text.references.Count + " instances.");
		foreach(var it in Localize_text.references) {
			it.ChangeContent("German");
		}

		yield return new WaitForSeconds(3);

		print("i'm changing the Language to English on " + Localize_text.references.Count + " instances.");
		foreach(var it in Localize_text.references) {
			it.ChangeContent("English");
		}

		yield return new WaitForSeconds(3);

		print("i'm testing vor Latin whitch is not in the csv file");
		foreach(var it in Localize_text.references) {
			it.ChangeContent("Latin");
		}
	}
}
