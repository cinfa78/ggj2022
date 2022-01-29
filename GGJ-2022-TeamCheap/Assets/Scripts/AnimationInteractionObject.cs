using System.Collections;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationInteractionObject : Interactable {
	public Pickable objectNeeded;

	[ShowInInspector] private float duration = 0.3f;
	public CanvasGroup iconCanvasGroup;

	public GameObject[] objectsToActivate;
	public GameObject[] objectsToDeactivate;

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
		iconCanvasGroup.alpha = 0;
	}

	private void OnDrawGizmos() {
		if (objectNeeded != null) {
			Gizmos.color = Color.white;
			Gizmos.DrawLine(transform.position, objectNeeded.transform.position);
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
			foreach (GameObject o in objectsToActivate) {
				o.SetActive(true);
			}
			foreach (GameObject o in objectsToDeactivate) {
				o.SetActive(false);
			}
		}
	}

	public override void ShowInteractionAvailable() {
		if (characterSwitcher.IsHoldingObject && objectNeeded.name == characterSwitcher.heldObject.name) {
			iconCanvasGroup.alpha = Mathf.Lerp(iconCanvasGroup.alpha, 1, 0.9f);
			if (iconCoroutine != null) StopCoroutine(iconCoroutine);
			iconCoroutine = StartCoroutine(FadeOutIcon());
		}
	}

	private IEnumerator FadeOutIcon() {
		float timer = duration;
		while (timer > 0) {
			float t = timer / duration;
			iconCanvasGroup.alpha = t;
			timer -= Time.deltaTime;
			yield return null;
		}
		iconCanvasGroup.alpha = 0;
	}
}