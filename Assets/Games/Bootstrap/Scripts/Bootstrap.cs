using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    public static Bootstrap instance;

    [SerializeField] private Animator _animator;
    [SerializeField] private AudioListener _audioListener;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private bool _debug;

    private string currentScene;
    private static readonly int IsLoading = Animator.StringToHash("isLoading");
    private static readonly int IsLoadingLocal = Animator.StringToHash("isLoadingLocal");
    private static string _currentMusic;

    private void Start()
    {
        instance = this;
        if (_debug)
        {
            _animator.SetBool(IsLoading, false);
            _audioListener.enabled = true;
            return;
        }
        LoadScene("Main_Menu", 1, "Raccoon_Menu");        
    }
    
    public void LoadScene(string sceneName, int delay, string musicToLoad) => StartCoroutine(StartLoading(sceneName, delay, musicToLoad));
    public void LoadLocal(bool state) => StartLocalLoading(state);

    private IEnumerator StartLoading(string sceneName, int delay, string musicToLoad)
    {
        // Animator
        _animator.SetBool(IsLoading, true);
        
        // Wait
        yield return new WaitForSeconds(1);

        if (!string.IsNullOrEmpty(currentScene))
        {
            AsyncOperation sceneToUnload = SceneManager.UnloadSceneAsync(currentScene);
            yield return new WaitUntil(() => sceneToUnload.isDone); // Wait for unloaded
        }

        AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive); // Load
        yield return new WaitUntil(() => sceneToLoad.isDone); // Wait for loaded
        currentScene = sceneName;
        
        yield return new WaitForSeconds(delay);
        if (musicToLoad != _currentMusic) FadeMusic(musicToLoad);
        
        // Remove loading screen
        _animator.SetBool(IsLoading, false);

        yield return null;
    }
    
    private void StartLocalLoading(bool state)
    {
        // Animator
        _animator.SetBool(IsLoadingLocal, state);
    }

    public void FadeMusic(string musicName)
    {
        if (_currentMusic != null) StartCoroutine(StartFade(_audioMixer, _currentMusic, 2, 0));
        StartCoroutine(StartFade(_audioMixer, musicName, 2, 0.04f));
        _currentMusic = musicName;
    }
    
    public void FadeMusicQuick(string musicName)
    {
        if (_currentMusic != null) StartCoroutine(StartFade(_audioMixer, _currentMusic, 0.5f, 0));
        StartCoroutine(StartFade(_audioMixer, musicName, 0.5f, 0.06f));
        _currentMusic = musicName;
    }
    
    private IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        float currentTime = 0;
        audioMixer.GetFloat(exposedParam, out var currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
    }
}
