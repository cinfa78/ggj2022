using UnityEngine;

namespace DefaultNamespace {
	[CreateAssetMenu(fileName = "Fade Timing Setup", menuName = "Fade Timing", order = 0)]
	public class FadeTimingSetup : ScriptableObject {
		public float fadeInSpeed;
		public float fadeOutDuration;
	}
}