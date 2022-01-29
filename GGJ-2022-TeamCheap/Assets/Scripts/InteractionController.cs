using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {
	public float maxDistanceInteract = 2f;
	public int layerMask;
	private RaycastHit hit;
	private CharacterSwitcher characterSwitcher;

	private void Awake() {
		characterSwitcher = FindObjectOfType<CharacterSwitcher>();
	}

	private void Start() {
		layerMask = 1 << layerMask;
	}

	private void Update() {
		var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		if (Physics.Raycast(ray, out hit, maxDistanceInteract, layerMask)) {
			Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow, 1);
			Interactable ic = hit.collider.GetComponent<Interactable>();
			if (ic != null) {
				ic.ShowInteractionAvailable();
				//InteractionAvailable?.Invoke();
			}
		}

		if (Input.GetButtonDown("Fire1")) {
			Debug.Log($"Button down!");
			// This would cast rays only against colliders in layer 3.

			// Does the ray intersect any objects excluding the player layer

			if (Physics.Raycast(ray, out hit, maxDistanceInteract, layerMask)) {
				Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow, 1);
				Interactable ic = hit.collider.GetComponent<Interactable>();
				if (ic != null) {
					ic.Interact();
				}
			}
			else {
				Debug.DrawRay(ray.origin, ray.direction * 1000, Color.white, 0.1f);
				//Debug.Log("Did not Hit");
			}
		}
	}
}