using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image backImage;
    [SerializeField] private Image frontImage;
    [SerializeField] private TextMeshProUGUI healthText;

    private Coroutine reduceHealthCoroutine;

    public void Initialize(Character charcater)
    {
        if (charcater == null)
            return;


        float currentHP = charcater.Stats.GetStat(EStatType.CurHealth);
        float maxHP = charcater.Stats.GetStat(EStatType.MaxHealth);

        healthText.text = $"{currentHP}/{maxHP}";
    }

    public void SetHealth(CharacterStats stat)
    {
        if(reduceHealthCoroutine != null)
        {
            StopCoroutine(reduceHealthCoroutine);
        }

        float currentHP = stat.GetStat(EStatType.CurHealth);
        float maxHP = stat.GetStat(EStatType.MaxHealth);

        healthText.text = $"{currentHP}/{maxHP}";

        float ratio = currentHP / maxHP;
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
