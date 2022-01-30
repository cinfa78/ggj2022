using UnityEngine;

public class AnimationInteraction : Interactable {
	public CanvasIconManager canvasIconManager;
	private Animator animator;
	private Coroutine iconCoroutine;
	private bool animationOver;
	public AudioClip audioClip;

	private void Awake() {
		animator = GetComponent<Animator>();
	}

	public override void Interact() {
		if (!animationOver) {
			animator.SetTrigger("start");
			animationOver = true;
			if (audioClip != null) {
				AudioSource.PlayClipAtPoint(audioClip, transform.position);
			}
		}
	}

	public override void ShowInteractionAvailable() {
		if (!animationOver) {
			canvasIconManager.Show();
		}
	}
}