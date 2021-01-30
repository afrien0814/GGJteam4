using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroySelf : MonoBehaviour
{
    private void Update()
    {
        if(!this.GetComponent<ParticleSystem>().isPlaying)
        {
            Destroy(this.gameObject);
        }    
    }
}
