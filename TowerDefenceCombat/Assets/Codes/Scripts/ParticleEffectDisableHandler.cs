using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectDisableHandler : MonoBehaviour
{
    [SerializeField] private ParticleEffectType visualEffectType;

    private void OnParticleSystemStopped()
    {
        ParticleEffectObjectPool.Instance.ReturnToPool(visualEffectType, GetComponent<ParticleSystem>());
    }
}
