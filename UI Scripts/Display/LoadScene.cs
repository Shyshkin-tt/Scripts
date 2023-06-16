using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    
    public SceneLoader _loader;

    public Slider _slider;

    private void Awake()
    {
        _loader = FindObjectOfType<SceneLoader>();
    }

    private void Start()
    {
        var scene = _loader.Location;

        StartCoroutine(LoadSceneScreen(scene));
    }
    IEnumerator LoadSceneScreen(string scene)
    {     

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);

        while (!asyncOperation.isDone)
        {
            float progressLoad = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            _slider.value = progressLoad;

            yield return null;
        }
    }
}
