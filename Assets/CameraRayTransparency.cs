using System.Collections.Generic;
using UnityEngine;

public class RoomTransparencyManager : MonoBehaviour
{
    public Transform player;
    public float transparencyTransitionSpeed = 2f;

    private Dictionary<string, List<Renderer>> roomRenderers = new Dictionary<string, List<Renderer>>();
    private HashSet<Renderer> transparentRenderers = new HashSet<Renderer>();
    private Dictionary<Renderer, float> currentAlpha = new Dictionary<Renderer, float>();
    private Dictionary<Renderer, string> originalTags = new Dictionary<Renderer, string>();


    void Start()
    {
        if (player == null)
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player object has the tag 'Player'.");
        }
    }
        // Optional: Pre-populate rooms and renderers programmatically or through Unity Inspector
              InitializeRooms();  // Inicjalizacja pokoi 
    }

    void Update()
    {
        UpdateTransparency();
        
    }

public void InitializeRooms()
{
    roomRenderers.Clear();
    foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
    {
        string roomName = room.name;
        if (!roomRenderers.ContainsKey(roomName))
            roomRenderers[roomName] = new List<Renderer>();

        Renderer[] renderers = room.GetComponentsInChildren<Renderer>();
        roomRenderers[roomName].AddRange(renderers);
    }
}



    void UpdateTransparency()
    {
        List<Renderer> toRemove = new List<Renderer>();

        foreach (Renderer renderer in transparentRenderers)
        {
            if (renderer != null)
            {
                Debug.Log("Updating transparency for: " + renderer.gameObject.name);
                // Get current alpha and target alpha
                float targetAlpha = currentAlpha.ContainsKey(renderer) ? currentAlpha[renderer] : 1f;

                // Calculate the new alpha using smooth interpolation
                float current = GetRendererAlpha(renderer);
                float newAlpha = Mathf.Lerp(current, targetAlpha, Time.deltaTime * transparencyTransitionSpeed);

                // Apply the new alpha
                SetTransparency(renderer, newAlpha);

                // Check if the target alpha is nearly reached
                if (Mathf.Abs(newAlpha - targetAlpha) < 0.01f)
                {
                    SetTransparency(renderer, targetAlpha); // Snap to target alpha
                    if (Mathf.Approximately(targetAlpha, 1f)) // Fully opaque
                    {
                        RestoreOriginalTag(renderer);
                        toRemove.Add(renderer); // Remove from transparency tracking
                    }
                }
            }
        }

        // Clean up renderers that have fully transitioned
        foreach (var renderer in toRemove)
        {
            transparentRenderers.Remove(renderer);
            currentAlpha.Remove(renderer);
        }
    }

    float GetRendererAlpha(Renderer renderer)
    {
        // Get the current alpha from the renderer's material
        if (renderer != null && renderer.material.HasProperty("_Color"))
        {
            return renderer.material.color.a;
        }
        return 1f; // Default to opaque if no alpha found
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
                material.SetInt("_ZWrite", 0);
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.DisableKeyword("_ALPHATEST_ON");
                material.renderQueue = 3000;
            }
        }

        // Change tag to "Transparent" if not already done
        if (!originalTags.ContainsKey(renderer))
        {
            originalTags[renderer] = renderer.gameObject.tag; // Store original tag
            renderer.gameObject.tag = "Transparent";
        }
    }

    void RestoreOriginalTag(Renderer renderer)
    {
        if (originalTags.ContainsKey(renderer))
        {
            renderer.gameObject.tag = originalTags[renderer]; // Restore original tag
            originalTags.Remove(renderer); // Remove from dictionary
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            string roomName = other.name;
            if (roomRenderers.ContainsKey(roomName))
            {
                foreach (Renderer renderer in roomRenderers[roomName])
                {
                    if (!transparentRenderers.Contains(renderer))
                    {
                        transparentRenderers.Add(renderer);
                        currentAlpha[renderer] = 0.2f; // Target transparency
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            string roomName = other.name;
            if (roomRenderers.ContainsKey(roomName))
            {
                foreach (Renderer renderer in roomRenderers[roomName])
                {
                    if (transparentRenderers.Contains(renderer))
                    {
                        currentAlpha[renderer] = 1f; // Restore transparency
                    }
                }
            }
        }
    }
}
