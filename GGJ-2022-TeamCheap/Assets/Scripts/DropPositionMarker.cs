using System.Collections;
using UnityEditor;
using UnityEngine;

public class DropPositionMarker : Interactable {
	public float radius = 0.5f;
	public Pickable droppableObject;
	private SphereCollider sphereCollider;
	public CanvasGroup iconCanvasGroup;
	private Coroutine iconCoroutine;
	public float duration = 0.3f;
	private CharacterSwitcher characterSwitcher;
	public bool storyAdvanced;
	public AudioClip audioClip;
	public GameObject[] objectsToActivate;
	public GameObject[] objectsToDeactivate;

	private void Awake() {
		characterSwitcher = FindObjectOfType<CharacterSwitcher>();
		gameObject.layer = 3;
		sphereCollider = gameObject.AddComponent<SphereCollider>();
		sphereCollider.isTrigger = true;
		sphereCollider.radius = radius;
		iconCanvasGroup.alpha = 0;
	}

	private void Start() {
		foreach (var o in objectsToActivate) {
			o.SetActive(false);
		}
	}

	public override void ShowInteractionAvailable() {
		Debug.Log($"{characterSwitcher.IsHoldingObject} {characterSwitcher.heldObject} {droppableObject}");
		if (characterSwitcher.IsHoldingObject && characterSwitcher.heldObject.name == droppableObject.name) {
			iconCanvasGroup.alpha = 1;
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

	public override void Interact() {
		if (characterSwitcher.IsHoldingObject && characterSwitcher.heldObject.name == droppableObject.name) {
			Debug.Log($"Dropping {characterSwitcher.heldObject.name}");
			droppableObject.transform.parent = null;
			droppableObject.transform.position = transform.position;
			droppableObject.transform.rotation = transform.rotation;
			characterSwitcher.heldObject = null;

			MirrorTransform mirroredMirrorTransform = droppableObject.GetComponent<MirrorTransform>().mirroredObject.GetComponent<MirrorTransform>();
			if (mirroredMirrorTransform != null) {
				mirroredMirrorTransform.enabled = true;
			}
			var mirroredPickable = droppableObject.mirroredObject.GetComponent<Pickable>();
			if (mirroredPickable != null) {
				droppableObject.mirroredObject.GetComponent<Pickable>().PutDown();
			}
			else {
				droppableObject.mirroredObject.transform.parent = null;
			}
			
			droppableObject.GetComponent<Pickable>().PutDown();
			
			droppableObject.GetComponent<MirrorTransform>().enabled = true;
			
			if (audioClip != null) {
				AudioSource.PlayClipAtPoint(audioClip, transform.position);
			}
			if (!storyAdvanced) {
				storyAdvanced = true;
				foreach (var o in objectsToActivate) {
					o.SetActive(true);
				}
				foreach (var o in objectsToDeactivate) {
					o.SetActive(false);
				}
			}
		}
	}

	private void OnDrawGizmos() {
		Vector3 origin = transform.position;
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(origin + Vector3.left * 0.25f, origin + Vector3.right * 0.25f);
		Gizmos.DrawLine(origin + Vector3.forward * 0.25f, origin + Vector3.back * 0.25f);
		Gizmos.DrawWireSphere(origin, radius);
		Gizmos.color = Color.magenta;
		if (droppableObject != null) {
			Gizmos.DrawLine(origin, droppableObject.transform.position);
#if UNITY_EDITOR
			Handles.Label(origin, droppableObject.name);
#endif
		}
	}
}