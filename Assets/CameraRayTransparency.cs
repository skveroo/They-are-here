using System.Collections.Generic;
using UnityEngine;

public class CameraRayTransparency : MonoBehaviour
{
    public Transform player;
    public LayerMask obstacleMask;
    public float transparencyTransitionSpeed = 2f;

    private List<Renderer> lastHitRenderers = new List<Renderer>();
    private Dictionary<Renderer, float> currentAlpha = new Dictionary<Renderer, float>();

    void Update()
    {
        if (player == null) return;
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, directionToPlayer.normalized, distanceToPlayer, obstacleMask);
        ResetTransparency();
        foreach (RaycastHit hit in hits)
        {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
            if (hitRenderer != null)
            {
                if (!currentAlpha.ContainsKey(hitRenderer))
                {
                    currentAlpha[hitRenderer] = 1f;
                }
                currentAlpha[hitRenderer] = Mathf.Lerp(currentAlpha[hitRenderer], 0.2f, Time.deltaTime * transparencyTransitionSpeed);
                SetTransparency(hitRenderer, currentAlpha[hitRenderer]);

                if (!lastHitRenderers.Contains(hitRenderer))
                {
                    lastHitRenderers.Add(hitRenderer);
                }
            }
        }
    }
    void SetTransparency(Renderer renderer, float alpha)
    {
        Material[] materials = renderer.materials;
        foreach (Material material in materials)
        {
            if (material.HasProperty("_Color"))
            {
                Color color = material.color;
                color.a = alpha;
                material.color = color;
                material.SetFloat("_Mode", 3);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.DisableKeyword("_ALPHATEST_ON");
                material.renderQueue = 3000;
                renderer.gameObject.tag = "Transparent";
            }
        }
    }
    void ResetTransparency()
    {
        List<Renderer> toRemove = new List<Renderer>();

        foreach (Renderer renderer in lastHitRenderers)
        {
            if (renderer != null && !IsInRaycast(renderer))
            {
                if (currentAlpha.ContainsKey(renderer))
                {
                    currentAlpha[renderer] = Mathf.Lerp(currentAlpha[renderer], 1f, Time.deltaTime * transparencyTransitionSpeed);
                    SetTransparency(renderer, currentAlpha[renderer]);
                    if (Mathf.Approximately(currentAlpha[renderer], 1f))
                    {
                        toRemove.Add(renderer);
                    }
                }
            }
        }
        foreach (var renderer in toRemove)
        {
            lastHitRenderers.Remove(renderer);
            currentAlpha.Remove(renderer);
        }
    }
    bool IsInRaycast(Renderer renderer)
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        Ray ray = new Ray(transform.position, directionToPlayer.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanceToPlayer, obstacleMask))
        {
            return hit.collider.GetComponent<Renderer>() == renderer;
        }

        return false;
    }
}
