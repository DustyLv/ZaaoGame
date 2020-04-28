using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreLoad : MonoBehaviour
{
    public string m_SceneName = "";
    public Slider m_LoadingSlider;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_SceneName);

        while (!asyncLoad.isDone)
        {
            m_LoadingSlider.value = asyncLoad.progress;
            yield return null;
        }
    }
}
