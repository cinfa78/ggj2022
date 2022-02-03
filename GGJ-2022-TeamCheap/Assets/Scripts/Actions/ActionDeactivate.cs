using System;
using System.Collections;
using UnityEngine;

namespace TheDay {
	public class ActionDeactivate : Action {
		public GameObject[] objectsToDeactivate;
		private GameObject caller;
		private bool delayedDeactivation = false;

		public override void Execute(GameObject caller) {
			this.caller = caller;
			StartCoroutine(DelayedExecution());
		}

		private IEnumerator DelayedExecution() {
			if (delay > 0)
				yield return new WaitForSeconds(delay);
			foreach (var obj in objectsToDeactivate) {
				if (obj != caller) {
					obj.SetActive(false);
				}
				else {
					delayedDeactivation = true;
				}
			}
			var interactable = caller.GetComponent<Interactable>();
			if (interactable != null) {
				if (delayedDeactivation) interactable.StartCoroutine(Deactivate());
				else {
					Destroy(caller, 0.1f);
				}
			}
			yield break;
		}

		private IEnumerator Deactivate() {
			yield return null;
			Destroy(caller);
		}
	}
}