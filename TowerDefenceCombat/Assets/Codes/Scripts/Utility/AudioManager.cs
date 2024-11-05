using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farou.Utility;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip clickFeedbacks;
    [SerializeField] private AudioClip levelStartFeedbacks;
    [SerializeField] private AudioClip coinFeedbacks;
    [SerializeField] private AudioClip coinSpawnFeedbacks;
    [SerializeField] private AudioClip coinAddedFeedbacks;
    [SerializeField] private AudioClip unitHitFeedbacks;
    [SerializeField] private AudioClip unitDeadFeedbacks;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayUnitHitFeedbacks()
    {

    }

    public void PlayLevelStartFeedbacks()
    {

    }

    public void PlayCoinFeedbacks()
    {

    }

    public void PlayCoinSpawnFeedbacks()
    {

    }

    public void PlayCoinAddedFeedbacks()
    {

    }

    public void PlayUnitDeadFeedbacks()
    {

    }

    public void PlayClickFeedbacks()
    {

    }
}