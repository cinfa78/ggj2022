using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {
	private bool isLoading = false;
	public CanvasGroup canvasGroup;

	public void LoadGameScene() {
		if (!isLoading) {
			isLoading = true;
			StartCoroutine(LoadYourAsyncScene());
		}
	}

	IEnumerator LoadYourAsyncScene() {
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");
		while (!asyncLoad.isDone) {
			canvasGroup.alpha += Time.deltaTime;
			yield return null;
		}
	}
}