using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {
	public float maxDistanceInteract = 2f;

	// Start is called before the first frame update
	void Start() { }

	// Update is called once per frame
	private void Update() {
		if (Input.GetButtonDown("Fire1")) {
			int layerMask = 1 << 3;

			// This would cast rays only against colliders in layer 3.
			RaycastHit hit;
			// Does the ray intersect any objects excluding the player layer
			var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f));
			if (Physics.Raycast(ray, out hit, maxDistanceInteract, layerMask)) {
				Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow, 1);
				Debug.Log("Did Hit");
				Interactable ic = hit.collider.GetComponent<Interactable>();
				if (ic != null) {
					ic.Interact();
				}
			}
			else {
				Debug.DrawRay(ray.origin, ray.direction * 1000, Color.white, 0.1f);
				Debug.Log("Did not Hit");
			}
		}
	}
}