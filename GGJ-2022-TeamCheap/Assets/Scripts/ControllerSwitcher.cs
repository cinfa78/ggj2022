using UnityEngine;
using UnityEngine.Serialization;
using UnityStandardAssets.Characters.FirstPerson;

public class ControllerSwitcher : MonoBehaviour {
	public FirstPersonController youngFPC;
	public FirstPersonController oldFPC;
	private CharacterController youngCC;
	private CharacterController oldCC;
	public MirrorTransform youngMT;
	public MirrorTransform oldMT;
	private GlobalSwitcher globalSwitcher;

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
		youngFPC.cameraGameObject.SetActive(false);

		oldFPC.enabled = true;
		oldFPC.cameraGameObject.SetActive(true);
		oldCC.enabled = true;
		oldMT.enabled = false;
	}

	private void Switch() {
		youngFPC.enabled = !youngFPC.enabled;
		youngCC.enabled = youngFPC.enabled;
		youngFPC.cameraGameObject.SetActive(youngFPC.enabled);
		youngMT.enabled = !youngMT.enabled;
		
		oldFPC.enabled = !oldFPC.enabled;
		oldCC.enabled = oldFPC.enabled;
		oldFPC.cameraGameObject.SetActive(oldFPC.enabled);
		oldMT.enabled = !oldMT.enabled;
	}
}