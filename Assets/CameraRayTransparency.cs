using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyOnTrigger : MonoBehaviour
{
    public float transparentAlpha = 0.5f;
    public float transitionDuration = 1.0f;
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

    void Start()
    {
        Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in childRenderers)
        {
            if (renderer.material.HasProperty("_Color"))
            {
                originalColors[renderer] = renderer.material.color;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(ChangeTransparency(transparentAlpha, "Transparent"));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(ChangeTransparency(1.0f, "Untagged"));
        }
    }

    private IEnumerator ChangeTransparency(float targetAlpha, string newTag)
    {
        float elapsedTime = 0f;


        Dictionary<Renderer, Color> startingColors = new Dictionary<Renderer, Color>();
        foreach (var kvp in originalColors)
        {
            startingColors[kvp.Key] = kvp.Key.material.color;
        }

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            foreach (var kvp in startingColors)
            {
                Renderer renderer = kvp.Key;
                Color startColor = kvp.Value;
                Color targetColor = originalColors[renderer];
                targetColor.a = targetAlpha;


                Color newColor = Color.Lerp(startColor, targetColor, t);


                Material material = renderer.material;
                material.color = newColor;

                if (newColor.a < 1.0f)
                {
                    material.SetFloat("_Mode", 3);
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                }
            }

            yield return null;
        }


        foreach (var kvp in originalColors)
        {
            Renderer renderer = kvp.Key;
            Color finalColor = kvp.Value;
            finalColor.a = targetAlpha;

            Material material = renderer.material;
            material.color = finalColor;

            if (Mathf.Approximately(targetAlpha, 1.0f))
            {
                material.SetFloat("_Mode", 0);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
            }

            renderer.gameObject.tag = newTag;
        }
    }
}
