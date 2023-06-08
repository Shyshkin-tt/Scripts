using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject _loadScreen;
    public Slider _slider;

    public void LoadScene(int level)
    {
        StartCoroutine(LoadSceneAsynch(level));

    }

    IEnumerator LoadSceneAsynch(int level)
    {
        AsyncOperation aop = SceneManager.LoadSceneAsync(level);
        _loadScreen.SetActive(true);
        while (!aop.isDone)
        {
            _slider.value = aop.progress;
            yield return null;
        }
    }
}
