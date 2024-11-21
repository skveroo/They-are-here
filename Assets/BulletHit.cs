using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public TrailRenderer trailRenderer; 

    private void OnCollisionEnter(Collision collision)
    {
        if (trailRenderer != null)
        {
            Destroy(trailRenderer.gameObject, 0); 
        }
        Destroy(gameObject);
    }
}
