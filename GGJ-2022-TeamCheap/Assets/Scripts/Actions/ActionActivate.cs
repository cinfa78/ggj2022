using System.Collections;
using UnityEngine;

namespace TheDay {
	public class ActionActivate : Action {
		public GameObject[] objectsToActivate;

		private void Awake() {
			foreach (var o in objectsToActivate) {
				o.SetActive(false);
			}
		}

		public override void Execute(GameObject caller) {
			StartCoroutine(DelayedExecution());
		}

		private IEnumerator DelayedExecution() {
			if(delay>0)
			yield return new WaitForSeconds(delay);
			foreach (var obj in objectsToActivate) {
				obj.SetActive(true);
			}
			yield break;
		}
	}
}