using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farou.Utility;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip levelStartSound;
    [SerializeField] private AudioClip coinSound;
    [SerializeField] private AudioClip coinSpawnSound;
    [SerializeField] private AudioClip coinAddedSound;
    [SerializeField] private AudioClip unitSpawnSound;
    [SerializeField] private AudioClip[] unitHitSound;
    [SerializeField] private AudioClip[] unitDeadSound;

    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayUnitHitSound()
    {
        PlaySound(unitHitSound);
    }

    public void PlayLevelStartSound()
    {
        PlaySound(levelStartSound);
    }

    public void PlayCoinSound()
    {
        PlaySound(coinSound);
    }

    public void PlayCoinSpawnSound()
    {
        PlaySound(coinSpawnSound);
    }

    public void PlayCoinAddedSound()
    {
        PlaySound(coinAddedSound);
    }

    public void PlayUnitDeadSound()
    {
        PlaySound(unitDeadSound);
    }

    public void PlayClickSound()
    {
        PlaySound(clickSound);
    }

    public void PlayUnitSpawnSound()
    {
        PlaySound(unitSpawnSound);
    }

    private void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void PlaySound(AudioClip[] audioClips)
    {
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
    }
}