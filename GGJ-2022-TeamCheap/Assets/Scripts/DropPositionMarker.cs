using System;
using System.Collections;
using Sirenix.OdinInspector;
using TheDay;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;
#endif
using UnityEngine;
using Action = TheDay.Action;

public class DropPositionMarker : Interactable {
	public float radius = 0.5f;
	public Pickable droppableObject;
	private SphereCollider sphereCollider;
	public CanvasIconManager canvasIconManager;

	[ReadOnly] public bool storyAdvanced;
	private TheDay.Action[] actions;
	private CharacterSwitcher characterSwitcher;

	private void Awake() {
		characterSwitcher = FindObjectOfType<CharacterSwitcher>();
		gameObject.layer = 3;
		sphereCollider = gameObject.AddComponent<SphereCollider>();
		sphereCollider.isTrigger = true;
		sphereCollider.radius = radius;
		actions = GetComponents<Action>();
	}

	public override void ShowInteractionAvailable() {
		if (characterSwitcher.IsHoldingObject && characterSwitcher.heldObject.name == droppableObject.name) {
			canvasIconManager.Show();
		}
	}

	public override void Interact() {
		if (characterSwitcher.IsHoldingObject && characterSwitcher.heldObject.name == droppableObject.name) {
			Debug.Log($"Dropping {characterSwitcher.heldObject.name}");
			var dropTransform = transform;

			droppableObject.transform.parent = null;

			droppableObject.transform.position = dropTransform.position;
			droppableObject.transform.rotation = dropTransform.rotation;
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

			if (!storyAdvanced) {
				storyAdvanced = true;
				foreach (var action in actions) {
					action.Execute(gameObject);
				}
			}
		}
	}
#if UNITY_EDITOR
	private void OnDrawGizmos() {
		Vector3 origin = transform.position;
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(origin + Vector3.left * 0.25f, origin + Vector3.right * 0.25f);
		Gizmos.DrawLine(origin + Vector3.forward * 0.25f, origin + Vector3.back * 0.25f);
		Gizmos.DrawWireSphere(origin, radius);
		Gizmos.color = Color.magenta;
		if (droppableObject != null) {
			Gizmos.DrawLine(origin, droppableObject.transform.position);
			Handles.Label(origin, droppableObject.name);
		}
	}
#endif
}