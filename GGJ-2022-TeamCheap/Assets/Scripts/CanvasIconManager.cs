using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class CanvasIconManager : MonoBehaviour {
	public CanvasGroup canvasGroup;
	public FadeTimingSetup fadeTimingSetup;

	private void Awake() {
		canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.alpha = 0;
	}

	public void Show() {
		canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, fadeTimingSetup.fadeInSpeed);
		StopAllCoroutines();
		StartCoroutine(FadeOutIcon());
	}

	public void Hide() {
		StopAllCoroutines();
		StartCoroutine(FadeOutIcon());
	}

	private IEnumerator FadeOutIcon() {
		float timer = fadeTimingSetup.fadeOutDuration;
		while (timer > 0) {
			float t = timer / fadeTimingSetup.fadeOutDuration;
			canvasGroup.alpha = t;
			timer -= Time.deltaTime;
			yield return null;
		}
		canvasGroup.alpha = 0;
	}
}