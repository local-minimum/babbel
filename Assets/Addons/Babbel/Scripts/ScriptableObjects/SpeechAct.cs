using UnityEngine;
using System.Collections.Generic;

namespace Babbel {

public class SpeechAct : ScriptableObject {

	[Tooltip("Leave empty if in game's native language")]
	public SpeechAct translationSource;

	[Tooltip("Only for editor purposes")]
	public string name;

	public string title;

	public string text;

	public AudioClip sound;

	public List<Tag> tags = new List<Tag> ();
	
	public string Name {
		get {
			return string.IsNullOrEmpty (name) ? title : name; 
		}
	}
}

}