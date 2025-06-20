using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image backImage;
    [SerializeField] private Image frontImage;

    private Coroutine reduceHealthCoroutine;

    public void SetHealth(float ratio)
    {
        if(reduceHealthCoroutine != null)
        {
            StopCoroutine(reduceHealthCoroutine);
        }

        reduceHealthCoroutine = StartCoroutine(UpdateHealthSmoothly(ratio));
    }

    private IEnumerator UpdateHealthSmoothly(float ratio)
    {
        while (!Mathf.Approximately(frontImage.fillAmount, ratio))
        {
            if(ratio == 0f)
                frontImage.fillAmount = Mathf.Lerp(frontImage.fillAmount, ratio, Time.deltaTime * 4f);
            else
                frontImage.fillAmount = Mathf.Lerp(frontImage.fillAmount, ratio, Time.deltaTime * 2f);
            
            yield return null;
        }

        frontImage.fillAmount = ratio;
        reduceHealthCoroutine = null;
    }
}
