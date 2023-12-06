using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    public static Bootstrap instance;

    [SerializeField] private Animator animator;
    [SerializeField] private AudioListener audioListener;
    [SerializeField] private bool debug = false;

    private string currentScene;
    private static readonly int IsLoading = Animator.StringToHash("isLoading");
    private static readonly int IsLoadingLocal = Animator.StringToHash("isLoadingLocal");

    private void Start()
    {
        instance = this;
        if (debug)
        {
            animator.SetBool(IsLoading, false);
            audioListener.enabled = true;
            return;
        }
        LoadScene("Main_Menu", 1);        
    }
    
    public void LoadScene(string sceneName, int delay = 0) => StartCoroutine(StartLoading(sceneName, delay));
    public void LoadLocal(bool state) => StartLocalLoading(state);

    private IEnumerator StartLoading(string sceneName, int delay)
    {
        // Animator
        animator.SetBool(IsLoading, true);
        
        // Wait
        yield return new WaitForSeconds(1);
        audioListener.enabled = false;

        if (!string.IsNullOrEmpty(currentScene))
        {
            AsyncOperation sceneToUnload = SceneManager.UnloadSceneAsync(currentScene);
            yield return new WaitUntil(() => sceneToUnload.isDone); // Wait for unloaded
        }

        AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive); // Load
        yield return new WaitUntil(() => sceneToLoad.isDone); // Wait for loaded
        currentScene = sceneName;
        
        yield return new WaitForSeconds(delay);
        audioListener.enabled = true;
        
        // Remove loading screen
        animator.SetBool(IsLoading, false);

        yield return null;
    }
    
    private void StartLocalLoading(bool state)
    {
        // Animator
        animator.SetBool(IsLoadingLocal, state);
    }
}
