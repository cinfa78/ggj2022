using System.Collections;
using UnityEngine;

namespace TheDay {
	public class ActionPlaySound : Action {
		public AudioClip clip;
		private GameObject caller;
		[Range(0, 1)] public float volume = 1;

		public override void Execute(GameObject caller) {
			this.caller = caller;
			StartCoroutine(DelayedExecution());
		}

		private IEnumerator DelayedExecution() {
			if (delay > 0)
				yield return new WaitForSeconds(delay);
			AudioSource.PlayClipAtPoint(clip, caller.transform.position, volume);
			yield break;
		}
	}
}