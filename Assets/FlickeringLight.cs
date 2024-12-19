using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public bool isFlickering = false;
    public float timeDelay;

    void Update()
    {
        if (!isFlickering)
        {
            StartCoroutine(FlickeringLightCoroutine());
        }
    }

    IEnumerator FlickeringLightCoroutine()
    {
        isFlickering = true;
        Light lightComponent = this.gameObject.GetComponent<Light>();

        if (lightComponent != null)
        {
            lightComponent.enabled = false;
            timeDelay = Random.Range(1f, 5f);
            yield return new WaitForSeconds(timeDelay);

            lightComponent.enabled = true;
            timeDelay = Random.Range(0.2f, 1f);
            yield return new WaitForSeconds(timeDelay);
        }

        isFlickering = false;
    }
}
