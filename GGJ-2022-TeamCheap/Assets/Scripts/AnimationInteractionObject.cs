using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace TheDay {
	public class AnimationInteractionObject : Interactable {
		public Pickable objectNeeded;

		public CanvasIconManager canvasIconManager;

		private TheDay.Action[] actions;
		private Animator animator;
		private Collider collider;
		private CharacterSwitcher characterSwitcher;
		private Coroutine iconCoroutine;

		private void Awake() {
			characterSwitcher = FindObjectOfType<CharacterSwitcher>();
			animator = GetComponent<Animator>();
			collider = GetComponent<Collider>();
			actions = GetComponents<TheDay.Action>();
		}

		private void OnDrawGizmos() {
			if (objectNeeded != null) {
				Gizmos.color = Color.white;
				Gizmos.DrawLine(transform.position, objectNeeded.transform.position);
			}
		}

		public override void ShowInteractionAvailable() {
			if (characterSwitcher.IsHoldingObject && objectNeeded.name == characterSwitcher.heldObject.name) {
				canvasIconManager.Show();
			}
		}

		public override void Interact() {
			if (characterSwitcher.IsHoldingObject && objectNeeded.name == characterSwitcher.heldObject.name) {
				var mirroredObject = characterSwitcher.heldObject.GameObject().GetComponent<MirrorTransform>().mirroredObject;
				Destroy(characterSwitcher.heldObject);
				Destroy(mirroredObject);
				characterSwitcher.heldObject = null;
				collider.enabled = false;
				Destroy(collider);
				if (animator != null) {
					animator.SetTrigger("start");
				}
				else {
					Debug.Log($"No animator in {name}");
				}
				foreach (var action in actions) {
					action.Execute(gameObject);
				}
			}
		}
	}
}