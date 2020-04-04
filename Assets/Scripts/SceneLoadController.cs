using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoadController : MonoBehaviour
{
	public CanvasGroup cg;


	public static SceneLoadController instance;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;//Avoid doing anything else
		}

		instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
	{
		cg.alpha = 1f;
		cg.DOFade(0f, 0.5f);
	}

	void Update()
	{

	}

	public void LoadScene(string _sceneName)
	{
		cg.alpha = 0f;
		cg.DOFade(1f, 0.5f).OnComplete(() => {
			SceneManager.LoadScene(_sceneName);
		});

	}
}
