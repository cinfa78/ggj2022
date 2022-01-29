using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class Pickable : Interactable {
	public string objectName;
	[ReadOnly] public GameObject mirroredObject;
	public float duration = 1;
	private MirrorTransform mirrorTransform;
	private Rigidbody rigidbody;
	public CanvasGroup canvasGroup;
	private GameObject activePlayer;
	private CharacterSwitcher characterSwitcher;
	public GameObject canvasOrienter;
	private Collider collider;

	private void Awake() {
		characterSwitcher = FindObjectOfType<CharacterSwitcher>();
		mirrorTransform = GetComponent<MirrorTransform>();
		rigidbody = GetComponent<Rigidbody>();
		canvasGroup.alpha = 0;
		collider = GetComponent<Collider>();
	}

	private void Start() {
		mirroredObject = mirrorTransform.mirroredObject;
	}

	public override void Interact() {
		if (characterSwitcher.IsHoldingObject == false) {
			rigidbody.isKinematic = true;
			canvasGroup.GetComponent<Canvas>().enabled = false;
			if (characterSwitcher.oldActive) {
				transform.parent = characterSwitcher.oldFPC.heldObjectPos.transform;
				mirroredObject.transform.parent = characterSwitcher.youngFPC.heldObjectPos.transform;
			}
			else {
				transform.parent = characterSwitcher.youngFPC.heldObjectPos.transform;
				mirroredObject.transform.parent = characterSwitcher.oldFPC.heldObjectPos.transform;
			}
			transform.localPosition = Vector3.zero;
			mirroredObject.transform.localPosition = Vector3.zero;
			characterSwitcher.heldObject = gameObject;
			collider.enabled = false;
		}
	}

	public void Drop() {
		collider.enabled = true;
		transform.parent = null;
		rigidbody.isKinematic = false;
		mirrorTransform.enabled = false;
	}

	public void PutDown() {
		transform.parent = null;
		rigidbody.isKinematic = true;
		mirrorTransform.enabled = false;
		collider.enabled = true;
		canvasGroup.GetComponent<Canvas>().enabled = true;
	}

	public override void ShowInteractionAvailable() {
		if (characterSwitcher.IsHoldingObject == false) {
			canvasOrienter.transform.LookAt(Camera.main.transform);
			canvasGroup.alpha = 1;
			StopAllCoroutines();
			StartCoroutine(FadeOut());
		}
	}

	private IEnumerator FadeOut() {
		float timer = duration;
		while (timer > 0) {
			float t = timer / duration;
			canvasGroup.alpha = t;
			timer -= Time.deltaTime;
			yield return null;
		}
		canvasGroup.alpha = 0;
	}
}