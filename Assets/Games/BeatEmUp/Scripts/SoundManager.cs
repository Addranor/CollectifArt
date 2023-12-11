using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    void Start() => TryGetComponent(out _audioSource);

    public void PlayAudio(AudioClip audioClip) => _audioSource.PlayOneShot(audioClip);
}
