using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

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
	public Object heldObject;
	public bool IsHoldingObject {
		get => heldObject != null;
	}
	public bool YoungActive {
		get => !oldActive;
	}

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

	public void ForceLookAt() {
		currentFPC.enabled = false;
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
			heldObject.GameObject().transform.localPosition = Vector3.zero;
		}
		currentFPC = oldActive ? oldFPC : youngFPC;
	}

	[Button("Test Look At")]
	private void LookAtOrigin(Vector3 target) {
		currentFPC.externalRotationLookAt = true;
		currentFPC.externalRotationLookAtPosition = Vector3.zero;
	}

	[Button("Release Look At")]
	private void UnlockFPC() {
		currentFPC.externalRotation = false;
		currentFPC.externalRotationLookAt = false;
	}
}