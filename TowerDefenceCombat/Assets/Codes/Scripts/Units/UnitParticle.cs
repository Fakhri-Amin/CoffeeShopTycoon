using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitParticle : MonoBehaviour
{
    public void PlayHitParticle()
    {
        PlayParticle(ParticleEffectType.Hit, transform.position);
    }

    public void PlayDeadParticle()
    {
        PlayParticle(ParticleEffectType.Dead, transform.position);
    }

    private void PlayParticle(ParticleEffectType vfxType, Vector3 position)
    {
        var vfx = ParticleEffectObjectPool.Instance.GetPooledObject(vfxType);
        if (vfx)
        {
            vfx.transform.position = position;
            vfx.Play();
        }
    }
}
