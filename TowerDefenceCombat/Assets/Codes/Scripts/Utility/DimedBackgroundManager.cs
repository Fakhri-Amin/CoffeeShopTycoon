using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farou.Utility;
using DG.Tweening;

public class DimedBackgroundManager : Singleton<DimedBackgroundManager>
{
    [SerializeField] private CanvasGroup dimedBackground;
    [SerializeField] private CanvasGroup whiteBackground;
    public float FadeTime = 0.3f;

    private void Start()
    {
        // SetActiveDimedBackground(false);
        dimedBackground.alpha = 0;
        dimedBackground.blocksRaycasts = false;

        if (whiteBackground == null) return;
        whiteBackground.alpha = 0;
        whiteBackground.blocksRaycasts = false;
    }

    public void SetActiveDimedBackground(bool active)
    {
        // dimedBackground.SetActive(active);
        if (active)
        {
            dimedBackground.DOFade(1, FadeTime);
            dimedBackground.blocksRaycasts = true;
        }
        else
        {
            dimedBackground.DOFade(0, FadeTime);
            dimedBackground.blocksRaycasts = false;
        }
    }

    public void SetActiveWhiteBackground(bool active)
    {
        // dimedBackground.SetActive(active);
        if (active)
        {
            whiteBackground.DOFade(1, FadeTime);
            whiteBackground.blocksRaycasts = true;
        }
        else
        {
            whiteBackground.DOFade(0, FadeTime);
            whiteBackground.blocksRaycasts = false;
        }
    }
}
