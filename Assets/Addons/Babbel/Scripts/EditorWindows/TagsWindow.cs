using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Babbel {
	public class TagsWindow : EditorWindow {

		[MenuItem("Window/Babbel/Tags...")]
		public static void ShowWindow() {


			EditorWindow window = EditorWindow.GetWindow (typeof(TagsWindow));
			window.titleContent = new GUIContent ("Babbel:Tags");
		}

		void OnGUI() {
			// Show all the tags...
		}
	}

}