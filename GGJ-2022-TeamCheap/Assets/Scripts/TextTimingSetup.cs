using UnityEngine;

namespace DefaultNamespace {
	[CreateAssetMenu(fileName = "TextTimingSetup", menuName = "Text Timing Setup", order = 0)]
	public class TextTimingSetup : ScriptableObject {
		public float fadeInTime = 0.5f;
		public float minDisplayedTime = 1f;
		public float perLetterDelay = 0.001f;
		public float fadeOutTime = 0.5f;
	}
}