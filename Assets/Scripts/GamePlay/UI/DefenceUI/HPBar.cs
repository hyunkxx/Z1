using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider HPBarSlider;
    public TextMeshProUGUI HP_text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialize(float value)
    {
        HPBarSlider.maxValue = value;
        HPBarSlider.value = value;
        HP_text.text = $"{value}";
    }

    public void ShowInfo(float curHP)
    {
        HPBarSlider.value = curHP;
        HP_text.text = $"{curHP}";
    }
}
