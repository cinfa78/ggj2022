using UnityEngine;

public class SwitchInteraction : Interactable {
	public CanvasIconManager canvasIconManager;
	private GlobalSwitcher gameObjectSwitcher;
	public AudioClip audioClip;
	private Coroutine iconCoroutine;

	private void Awake() {
		gameObjectSwitcher = FindObjectOfType<GlobalSwitcher>();
	}

	public override void Interact() {
		gameObjectSwitcher.CallSwitch();
		if (audioClip != null) {
			AudioSource.PlayClipAtPoint(audioClip, transform.position);
		}
	}

	public override void ShowInteractionAvailable() {
		canvasIconManager.Show();
	}
}