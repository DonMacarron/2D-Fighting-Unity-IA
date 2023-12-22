using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public Animator transition;
    public float transitionTime = 1f;

    public void LoadScene(string scene) {
        StartCoroutine(LoadXScene(scene));
        transition.SetTrigger("nextTrans");
    }
    public void LoadSceneInstant(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    IEnumerator LoadXScene(string scene) {
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(scene);
    }
    public void Salir()
    {
        Application.Quit();
    }
}
