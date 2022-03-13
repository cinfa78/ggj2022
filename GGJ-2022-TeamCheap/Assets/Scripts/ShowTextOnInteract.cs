using System;
using System.Collections;
using DefaultNamespace;
using Sirenix.OdinInspector;
using TheDay;
using TMPro;
using UnityEngine;
using Action = TheDay.Action;

public class ShowTextOnInteract : Interactable {
	public TextTimingSetup textTimingSetup;
	public Canvas canvasText;
	[TextArea(5, 10)] [OnValueChanged("UpdateLabel")]
	public string message;

	public CanvasIconManager canvasIconManager;
	public bool forceLookAt;
	public bool showTextMultipleTimes;
	[ReadOnly] public bool storyAdvanced;
	private TheDay.Action[] actions;

	private CharacterSwitcher characterSwitcher;
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
		collider = GetComponent<Collider>();
		actions = GetComponents<Action>();
		characterSwitcher = FindObjectOfType<CharacterSwitcher>();
	}

	private void Start() {
		label.text = message;
	}
#if UNITY_EDITOR
	private void Update() {
		if (Input.GetButtonDown("Fire2")) {
			SecondaryInteract();
		}
	}
	#endif

	private void UpdateLabel() {
		label = canvasText.GetComponentInChildren<TMP_Text>();
		if (label != null)
			label.text = message;
	}

	public override void Interact() {
		skip = false;
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
		if (forceLookAt) {
			characterSwitcher.ForceLookAt(textCanvasGroup.gameObject);
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
			if (forceLookAt) {
				characterSwitcher.ReleaseLookAt();
			}
			foreach (var action in actions) {
				action.Execute(gameObject);
			}
			collider.enabled = false;
			StopAllCoroutines();
			canvasIconManager.Hide();
			gameObject.SetActive(false);
		}
		else {
			if (forceLookAt) {
				characterSwitcher.ReleaseLookAt();
			}
			collider.enabled = true;
		}

		
	}
}