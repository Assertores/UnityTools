using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsserTOOLres;

public class Test_AudioManager : MonoBehaviour {

	[SerializeField] GameObject _testObject;
	[SerializeField] AudioSource _testSource;

	void Start() {
		StartCoroutine(Retest());
	}

	IEnumerator Retest() {
		if(!AudioManager.Exists())
			yield break;

		AudioManager.s_instance.PlayAudioByName("Message");

		yield return new WaitForSeconds(2);

		AudioManager.s_instance.PlayAudioOnRef("Charge", _testSource);

		yield return new WaitForSeconds(2);

		AudioManager.s_instance.PlayAudioOnRef("Explosion", _testObject);
	}
}