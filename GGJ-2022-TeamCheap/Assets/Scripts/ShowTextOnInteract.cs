using System.Collections;
using DefaultNamespace;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ShowTextOnInteract : Interactable {
	public TextTimingSetup textTimingSetup;
	public Canvas canvasText;
	[TextArea] [OnValueChanged("UpdateLabel")]
	public string message;
	public AudioClip audioClip;

	public CanvasIconManager canvasIconManager;
	public bool showTextMultipleTimes;
	[ReadOnly] public bool storyAdvanced;
	public GameObject[] objectsToActivate;
	public GameObject[] objectsToDeactivate;

	private CanvasGroup textCanvasGroup;
	private TMP_Text label;
	private int textLength;
	private Collider collider;

	private bool skip;

	private Coroutine iconCoroutine;
	private Coroutine textCoroutine;
	private bool delayedDeactivation;

	private void Awake() {
		textCanvasGroup = canvasText.GetComponent<CanvasGroup>();
		textCanvasGroup.alpha = 0;
		label = canvasText.GetComponentInChildren<TMP_Text>();
		textLength = label.text.Length;
		foreach (GameObject o in objectsToActivate) {
			o.SetActive(false);
		}
		collider = GetComponent<Collider>();
	}

	private void Start() {
		label.text = message;
	}

	private void UpdateLabel() {
		label = canvasText.GetComponentInChildren<TMP_Text>();
		if (label != null)
			label.text = message;
	}

	public override void Interact() {
		if (showTextMultipleTimes) {
			if (textCoroutine != null) StopCoroutine(textCoroutine);
			textCoroutine = StartCoroutine(FadeOutText(false));
		}
		else {
			if (!storyAdvanced) {
				if (textCoroutine != null) StopCoroutine(textCoroutine);
				textCoroutine = StartCoroutine(FadeOutText(true));
				storyAdvanced = true;
			}
		}
	}

	public override void SecondaryInteract() {
		skip = true;
	}

	public override void ShowInteractionAvailable() {
		canvasIconManager?.Show();
	}

	private IEnumerator FadeOutText(bool once) {
		collider.enabled = false;
		textCanvasGroup.alpha = 0;
		float timer = textTimingSetup.fadeInTime;
		while (timer > 0 && !skip) {
			float t = timer / textTimingSetup.fadeInTime;
			textCanvasGroup.alpha = 1 - t;
			timer -= Time.deltaTime;
			yield return null;
		}
		textCanvasGroup.alpha = 1;
		float timeout = textTimingSetup.minDisplayedTime + textLength * textTimingSetup.perLetterDelay;
		timer = timeout;
		while (timer > 0 && !skip) {
			timer -= Time.deltaTime;
			yield return null;
		}
		timer = textTimingSetup.fadeOutTime;
		while (timer > 0 && !skip) {
			float t = timer / textTimingSetup.fadeOutTime;
			textCanvasGroup.alpha = t;
			timer -= Time.deltaTime;
			yield return null;
		}
		textCanvasGroup.alpha = 0;
		skip = false;
		if (once) {
			if (audioClip != null) {
				AudioSource.PlayClipAtPoint(audioClip, transform.position);
			}
			foreach (GameObject o in objectsToActivate) {
				o.SetActive(true);
			}
			foreach (GameObject o in objectsToDeactivate) {
				if (o == gameObject) {
					delayedDeactivation = true;
				}
				else {
					o.SetActive(false);
				}
			}
			collider.enabled = false;
			StopAllCoroutines();
			canvasIconManager.Hide();
			gameObject.SetActive(false);
		}
		else {
			collider.enabled = true;
		}
	}
	

	// private IEnumerator FadeOutTextOnce() {
	// 	textCanvasGroup.alpha = 0;
	// 	float timer = fadeDuration / 2;
	// 	while (timer > 0 && !skip) {
	// 		float t = timer / fadeDuration;
	// 		textCanvasGroup.alpha = 1 - t;
	// 		timer -= Time.deltaTime;
	// 		yield return null;
	// 	}
	// 	textCanvasGroup.alpha = 1;
	// 	float timeout = minTimeout - fadeDuration / 2 + textLength * 0.1f;
	// 	timer = timeout;
	// 	while (timer > 0 && !skip) {
	// 		timer -= Time.deltaTime;
	// 		yield return null;
	// 	}
	// 	timer = fadeDuration;
	// 	while (timer > 0 && !skip) {
	// 		float t = timer / fadeDuration;
	// 		textCanvasGroup.alpha = t;
	// 		timer -= Time.deltaTime;
	// 		yield return null;
	// 	}
	// 	textCanvasGroup.alpha = 0;
	// 	foreach (GameObject o in objectsToActivate) {
	// 		o.SetActive(true);
	// 	}
	// 	foreach (GameObject o in objectsToDeactivate) {
	// 		o.SetActive(false);
	// 	}
	// 	skip = false;
	// 	Destroy(collider);
	// }
}