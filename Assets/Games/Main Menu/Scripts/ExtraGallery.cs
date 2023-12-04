using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ExtraGallery : MonoBehaviour
{
    [SerializeField] private ExtraGalleryScriptableObject extraGalleryScriptableObject;

    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private TextMeshProUGUI maximumAmount;
    [SerializeField] private Image spriteRenderer;
    
    private Animator animator;
    private float onNavigation = 0.0f;
    private float onBack = 0.0f;
    private int index = 0;
    private int maximumIndex = 0;
    
    private static readonly int DisplayGallery = Animator.StringToHash("DisplayGallery");
    private static readonly int HideGallery = Animator.StringToHash("HideGallery");

    private void Start()
    {
        TryGetComponent(out animator);
        EnableGallery(extraGalleryScriptableObject);
    }

    private void EnableGallery(ExtraGalleryScriptableObject gallery)
    {
        extraGalleryScriptableObject = gallery;
        index = 0;
        maximumIndex = extraGalleryScriptableObject.galleryImages.Length;
        maximumAmount.text = maximumIndex.ToString();
        
        spriteRenderer.sprite = extraGalleryScriptableObject.galleryImages[0];
        
        UpdateSprite();
        
        // "Désactiver" l'interaction avec le menu principal
        // Activer l'interaction après la fin de l'animation
    }

    private void DisableGallery()
    {
        Debug.Log("Retour au menu");
        
        // "Désactiver" l'interaction avec la galerie
        // Réactiver l'interaction avec le menu principal après la fin de l'animation
    }

    private void UpdateSprite()
    {
        if (index < 0) index = maximumIndex - 1;    // Boucle de 0 vers la fin
        else if (index >= maximumIndex) index = 0;  // Boucle de la fin vers 0
        
        amount.text = (index + 1).ToString();   // On met à jour l'index d'image affiché
        spriteRenderer.sprite = extraGalleryScriptableObject.galleryImages[index];  // On affiche l'image correspondante
    }

    public void OnNavigation(InputValue value)
    {
        onNavigation = value.Get<float>();

        if (onNavigation < 0) // Image précédente
        {
            index--;
            UpdateSprite();
        }
        else if (onNavigation > 0) // Image suivante
        {
            index++;
            UpdateSprite();            
        }
    }

    public void OnBack(InputValue value)
    {
        onBack = value.Get<float>();
        if (onBack > 0) DisableGallery(); // Retour au menu principal
    }
}
