using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace TheDay {
	public class CharacterSwitcher : MonoBehaviour {
		public FirstPersonController oldFPC;
		public FirstPersonController youngFPC;
		private FirstPersonController currentFPC;

		private CharacterController oldCC;
		private CharacterController youngCC;

		[ReadOnly] public MirrorTransform oldMT;
		[ReadOnly] public MirrorTransform youngMT;

		public bool oldActive = true;

		private GlobalSwitcher globalSwitcher;
		public GameObject heldObject;
		public float zoomFieldOfView;
		public bool IsHoldingObject {
			get => heldObject != null;
		}
		public bool YoungActive {
			get => !oldActive;
		}
		public event System.Action ZoomIn;
		public event System.Action ZoomReset;

		private void Start() {
			globalSwitcher = FindObjectOfType<GlobalSwitcher>();
			globalSwitcher.Switch += Switch;

			youngCC = youngFPC.GetComponent<CharacterController>();
			oldCC = oldFPC.GetComponent<CharacterController>();
			youngMT = youngFPC.GetComponent<MirrorTransform>();
			oldMT = oldFPC.GetComponent<MirrorTransform>();


			youngFPC.enabled = false;
			youngCC.enabled = false;
			youngMT.enabled = true;
			youngFPC.cameraGameObject.GetComponent<AudioListener>().enabled = false;
			youngFPC.cameraGameObject.GetComponent<Camera>().enabled = false;

			oldFPC.enabled = true;
			oldCC.enabled = true;
			oldMT.enabled = false;
			oldFPC.cameraGameObject.GetComponent<AudioListener>().enabled = true;
			oldFPC.cameraGameObject.GetComponent<Camera>().enabled = true;
			currentFPC = oldFPC;
		}

		private void Update() {
			if (Input.GetButtonDown("Fire2")) {
				ZoomIn?.Invoke();
				currentFPC.ZoomIn(zoomFieldOfView);
			}
			if (Input.GetButtonUp("Fire2")) {
				ZoomReset?.Invoke();
				currentFPC.ZoomReset();
			}
		}

		private void Switch() {
			oldActive = !oldActive;

			youngFPC.enabled = YoungActive;
			youngCC.enabled = YoungActive;
			youngMT.enabled = !YoungActive;
			youngFPC.cameraGameObject.GetComponent<Camera>().enabled = YoungActive;
			youngFPC.cameraGameObject.GetComponent<AudioListener>().enabled = YoungActive;


			oldFPC.enabled = oldActive;
			oldCC.enabled = oldActive;
			oldMT.enabled = !oldActive;
			oldFPC.cameraGameObject.GetComponent<Camera>().enabled = oldActive;
			oldFPC.cameraGameObject.GetComponent<AudioListener>().enabled = oldActive;

			if (IsHoldingObject) {
				var mirroredObject = heldObject.GetComponent<MirrorTransform>().mirroredObject;
				heldObject.GetComponent<MirrorTransform>().enabled = true;
				mirroredObject.GetComponent<MirrorTransform>().enabled = false;
				heldObject = mirroredObject;
				heldObject.transform.localPosition = Vector3.zero;
			}
			currentFPC = oldActive ? oldFPC : youngFPC;
		}

		public void ForceLookAt(GameObject targetGameObject) {
			//Debug.Log($"Look At");
			ForceLookAt(targetGameObject.transform.position);
		}

		public void ForceLookAt(Vector3 targetWorldPosition) {
			currentFPC.externalRotationLookAt = true;
			currentFPC.externalRotationLookAtPosition = targetWorldPosition;
		}

		[Button("Test Look At")]
		private void LookAtOrigin(Vector3 target) {
			currentFPC.externalRotationLookAt = true;
			currentFPC.externalRotationLookAtPosition = Vector3.zero;
		}

		[Button("Release Look At")]
		public void ReleaseLookAt(float delay = 0) {
			StartCoroutine(DelayedRelease(delay));
		}

		private IEnumerator DelayedRelease(float delay) {
			if (delay > 0) {
				yield return new WaitForSeconds(delay);
			}
			//Debug.Log($"Look At Released");
			currentFPC.externalRotation = false;
			currentFPC.externalRotationLookAt = false;
			yield break;
		}
	}
}