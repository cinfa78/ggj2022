using System.Collections;
using UnityEngine;

namespace TheDay {
	public class ActionLookAt : Action {
		public GameObject objectToLookAt;
		private CharacterSwitcher characterSwitcher;
		public float lookForSeconds;

		private void Awake() {
			characterSwitcher = FindObjectOfType<CharacterSwitcher>();
		}

		public override void Execute(GameObject caller) {
			if (objectToLookAt != null) {
				StartCoroutine(DelayedExecution());
			}
		}

		private IEnumerator DelayedExecution() {
			if (delay > 0)
				yield return new WaitForSeconds(delay);
			Debug.Log($"Look At {objectToLookAt.name}");
			characterSwitcher.ForceLookAt(objectToLookAt);
			characterSwitcher.ReleaseLookAt(lookForSeconds);
			yield break;
		}
	}
}