using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsserTOOLres {
	[System.Serializable]
	public class Audio {
		public string name;
		public AudioSource referece;
	}

	/// <summary>
	/// make shure that all added audioSources are not PlayOnAwake
	/// requiers Singleton
	/// </summary>
	public sealed class AudioManager : Singleton<AudioManager> {

		#region ===== ===== DATA ===== =====

		[Tooltip("make shure that all added audioSources are not PlayOnAwake")]
		[SerializeField] Audio[] r_clips;

		Dictionary<string, AudioSource> m_clipRefs = new Dictionary<string, AudioSource>();

		#endregion
		#region ===== ===== API ===== =====

		/// <summary>
		/// Plays an audio by referenced name
		/// </summary>
		/// <param name="name">the name as key</param>
		public void PlayAudioByName(string name) {
			m_clipRefs[name]?.Play();
		}

		/// <summary>
		/// adds a AudioSource component to the GameObject, sets it to the referenced audio and plays it
		/// </summary>
		/// <param name="name">the name as key</param>
		/// <param name="reference">the referenced GameObject</param>
		public AudioSource PlayAudioOnRef(string name, GameObject reference) {
			if(!m_clipRefs.ContainsKey(name))
				return null;

			AudioSource tmp = reference.AddComponent<AudioSource>();

			return PlayAudioOnRef(name, tmp);
		}

		/// <summary>
		/// sets the referenced AudioSource to the Rreferenced audio and plays it
		/// </summary>
		/// <param name="name">the name as key</param>
		/// <param name="reference">the referenced AudioSource</param>
		public AudioSource PlayAudioOnRef(string name, AudioSource reference) {
			if(!m_clipRefs.ContainsKey(name))
				return null;

			DeepCopyAudioSource(m_clipRefs[name], reference);

			reference.Play();

			return reference;
		}

		#endregion
		#region ===== ===== CORE ===== =====

		protected override void OnMyAwake() {
			foreach(var it in r_clips) {
				if(!it.referece) {
					Debug.LogWarning("reference for audio " + it.name + " not set.");
					continue;
				}
				if(it.name == "") {
					it.name = it.referece.clip.name;
				}
				if(m_clipRefs.ContainsKey(it.name)) {
					continue;
				}

				m_clipRefs[it.name] = it.referece;
			}
		}

		// TODO: move To own Element
		void DeepCopyAudioSource(AudioSource origin, AudioSource target) {
			System.Reflection.PropertyInfo[] fields = typeof(AudioSource).GetProperties();
			foreach(var field in fields) {
				if(!field.CanWrite || field.Name == "name")
					continue;
				if(isPropertyObsolete(field)) {
					continue;
				}
				try {
					field.SetValue(target, field.GetValue(origin));
				} catch { }
			}
		}

		bool isPropertyObsolete(System.Reflection.PropertyInfo property) {
			var attrData = property.CustomAttributes;
			foreach(var it in attrData) {
				if(it.AttributeType == typeof(System.ObsoleteAttribute)) {
					return true;
				}
			}
			return false;
		}

		#endregion
	}
}