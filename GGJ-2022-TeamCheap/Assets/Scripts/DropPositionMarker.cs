using System;
using System.Collections;
using System.Collections.Generic;
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

	private void Awake() {
		characterSwitcher = FindObjectOfType<CharacterSwitcher>();
		gameObject.layer = 3;
		sphereCollider = gameObject.AddComponent<SphereCollider>();
		sphereCollider.isTrigger = true;
		sphereCollider.radius = radius;
		iconCanvasGroup.alpha = 0;
	}

	public override void ShowInteractionAvailable() {
		Debug.Log($"{characterSwitcher.IsHoldingObject} {characterSwitcher.heldObject} {droppableObject}");
		if (characterSwitcher.IsHoldingObject && characterSwitcher.heldObject.name == droppableObject.name) {
			iconCanvasGroup.alpha = 1;
			if (iconCoroutine != null) StopCoroutine(iconCoroutine);
			iconCoroutine = StartCoroutine(FadeOutIcon());
		}
	}

	public override void Interact() {
		if (characterSwitcher.IsHoldingObject && characterSwitcher.heldObject.name == droppableObject.name) {
			droppableObject.transform.parent = null;
			droppableObject.transform.position = transform.position;
			droppableObject.transform.rotation = transform.rotation;
			characterSwitcher.heldObject = null;
			droppableObject.GetComponent<Pickable>().PutDown();
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