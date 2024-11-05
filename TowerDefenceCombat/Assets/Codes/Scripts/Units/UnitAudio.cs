using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAudio : MonoBehaviour
{
    public void PlayHitSound()
    {
        AudioManager.Instance.PlayUnitHitFeedbacks();
    }

    public void PlayDeadSound()
    {
        AudioManager.Instance.PlayUnitDeadFeedbacks();
    }
}
