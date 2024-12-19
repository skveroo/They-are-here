using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;

    private Health healthComponent;
    private Coroutine fadeCoroutine;

    private void Start()
    {

        if (gameObject.CompareTag("Player"))
        {
            healthComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
        else
        {

            healthComponent = GetComponentInParent<Health>();
        }

        if (healthComponent != null && healthBar != null)
        {
            healthBar.maxValue = healthComponent.health;
            healthBar.value = healthComponent.health;
        }
        else
        {
            Debug.LogError("Health component or HealthBar Slider not found.");
        }

        SetChildrenAlpha(0f);
    }

    private void Update()
    {
        transform.rotation = Quaternion.identity;
    }

    public void SetHealth(float hp)
    {
        if (healthBar != null)
        {
            if (hp < 0) hp = 0;
            if (hp > healthBar.maxValue) hp = healthBar.maxValue;

            healthBar.value = hp;


            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }


            fadeCoroutine = StartCoroutine(FadeHealthBar(1f, 0f, 5f));
        }
    }

    private IEnumerator FadeHealthBar(float targetAlphaIn, float targetAlphaOut, float delay)
    {

        yield return StartCoroutine(FadeToAlpha(targetAlphaIn, 0.5f));


        yield return new WaitForSeconds(delay);


        yield return StartCoroutine(FadeToAlpha(targetAlphaOut, 0.5f));
    }

    private IEnumerator FadeToAlpha(float targetAlpha, float duration)
    {
        float startAlpha = GetCurrentAlpha();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            SetChildrenAlpha(newAlpha);
            yield return null;
        }

        SetChildrenAlpha(targetAlpha);
    }

    private float GetCurrentAlpha()
    {
        Image[] childImages = GetComponentsInChildren<Image>();
        if (childImages.Length > 0)
        {
            return childImages[0].color.a;
        }
        return 1f;
    }

    private void SetChildrenAlpha(float alpha)
    {
        foreach (Image childImage in GetComponentsInChildren<Image>())
        {
            Color color = childImage.color;
            color.a = alpha;
            childImage.color = color;
        }
    }
}
