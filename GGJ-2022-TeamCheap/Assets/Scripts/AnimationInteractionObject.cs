using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationInteractionObject : Interactable {
	public Pickable objectNeeded;

	public CanvasIconManager canvasIconManager;
	public AudioClip audioClip;
	public GameObject[] objectsToActivate;
	public GameObject[] objectsToDeactivate;
	public float activationDelay = 0f;

	private Animator animator;
	private Collider collider;
	private CharacterSwitcher characterSwitcher;
	private Coroutine iconCoroutine;

	private void Awake() {
		characterSwitcher = FindObjectOfType<CharacterSwitcher>();
		animator = GetComponent<Animator>();
		collider = GetComponent<Collider>();
		foreach (GameObject o in objectsToActivate) {
			o.SetActive(false);
		}
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
			if (audioClip != null) {
				AudioSource.PlayClipAtPoint(audioClip, transform.position);
			}
			StartCoroutine(DelayedActivation());
		}
	}

	private IEnumerator DelayedActivation() {
		yield return new WaitForSeconds(activationDelay);
		foreach (GameObject o in objectsToActivate) {
			o.SetActive(true);
		}
		foreach (GameObject o in objectsToDeactivate) {
			o.SetActive(false);
		}
	}
}