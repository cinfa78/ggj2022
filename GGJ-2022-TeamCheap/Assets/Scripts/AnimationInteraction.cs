using UnityEngine;

public class AnimationInteraction : Interactable {
	public CanvasIconManager canvasIconManager;
	private Animator animator;
	private Coroutine iconCoroutine;
	private TheDay.Action[] actions;
	private bool animationOver;
	
	private void Awake() {
		animator = GetComponent<Animator>();
		actions = GetComponents<TheDay.Action>();
	}

	public override void Interact() {
		if (!animationOver) {
			animator.SetTrigger("start");
			animationOver = true;
			foreach (var action in actions) {
				action.Execute(gameObject);
			}
		}
	}

	public override void ShowInteractionAvailable() {
		if (!animationOver) {
			canvasIconManager.Show();
		}
	}
}