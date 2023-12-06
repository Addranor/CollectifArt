using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ExtraGallery : MonoBehaviour
{
    [Header("SceneLoader")]
    [SerializeField] private string raccoonSceneName = "Raccoon_Lvl_1";
    [SerializeField] private string rapunzelSceneName = "Rapunzel_Boss_Battle";
    
    [Header("Galleries")]
    [SerializeField] private ExtraGalleryScriptableObject racoonGallery;
    [SerializeField] private ExtraGalleryScriptableObject raiponceGallery;
    
    [Header("References")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject extraMenu;
    [SerializeField] private GameObject raccoonHeader;
    [SerializeField] private GameObject raiponceHeader;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip backSfx;
    [SerializeField] private AudioClip navigationSfx;
    
    [Header("Pagination")]
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private TextMeshProUGUI maximumAmount;
    [SerializeField] private Image spriteRenderer;
    
    private Animator animator;
    private float onNavigation = 0.0f;
    private float onBack = 0.0f;
    private int index = 0;
    private int maximumIndex = 0;
    private ExtraGalleryScriptableObject cachedGallery;
    
    private static readonly int DisplayGallery = Animator.StringToHash("DisplayGallery");
    private static readonly int HideGallery = Animator.StringToHash("HideGallery");

    private void Start()
    {
        TryGetComponent(out animator);
    }

    public void LoadRaccoon() => Bootstrap.instance.LoadScene(raccoonSceneName);
    public void LoadRapunzel() => Bootstrap.instance.LoadScene(rapunzelSceneName);

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void EnableRacconExtra()
    {
        LoadGallery(racoonGallery);
        ToggleGallery(true);
        raccoonHeader.SetActive(true);
    }

    public void EnableRaiponceExtra()
    {
        LoadGallery(raiponceGallery);
        ToggleGallery(true);
        raiponceHeader.SetActive(true);
    }

    private void LoadGallery(ExtraGalleryScriptableObject gallery)
    {
        cachedGallery = gallery;
        index = 0;
        maximumIndex = cachedGallery.galleryImages.Length;
        maximumAmount.text = maximumIndex.ToString();
        
        spriteRenderer.sprite = cachedGallery.galleryImages[0];
        
        UpdateSprite();
    }

    private void ToggleGallery(bool enable = false)
    {
        raiponceHeader.SetActive(!enable);
        raccoonHeader.SetActive(!enable);
        mainMenu.SetActive(!enable);
        extraMenu.SetActive(enable);
    }

    private void UpdateSprite()
    {
        if (index < 0) index = maximumIndex - 1;    // From 0 to the end
        else if (index >= maximumIndex) index = 0;  // From the end to 0
        
        amount.text = (index + 1).ToString();   // Update displayed index
        spriteRenderer.sprite = cachedGallery.galleryImages[index];  // Display picture
    }

    public void OnNavigation(InputValue value)
    {
        onNavigation = value.Get<float>();

        if (!spriteRenderer.gameObject.activeInHierarchy) return;

        if (onNavigation < 0) // Previous image
        {
            index--;
            UpdateSprite();
            audioSource.PlayOneShot(navigationSfx);
        }
        else if (onNavigation > 0) // Next image
        {
            index++;
            UpdateSprite();
            audioSource.PlayOneShot(navigationSfx);
        }
    }

    public void OnBack(InputValue value)
    {
        onBack = value.Get<float>();
        
        if (!spriteRenderer.gameObject.activeInHierarchy) return;
        
        if (onBack > 0) ToggleGallery(false); // Back to the main menu
        audioSource.PlayOneShot(backSfx);
    }
}
