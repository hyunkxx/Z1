using UnityEngine;
using TMPro;


public abstract class HUDBase : MonoBehaviour
{
    public abstract void Initialize();
}

public class SurvivalHUD : HUDBase
{
    [SerializeField]
    private HealthBar healthBar;
    public HealthBar HealthBar => healthBar;

    [SerializeField]
    private JamWidget jamWidget;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private TextMeshProUGUI waveText;

    PlayerController playerController;
    SurvivalGameRule rule;

    public override void Initialize()
    {
        playerController = GameManager.Instance.GameMode.PlayerController;
        rule = GameManager.Instance.GameMode.Rule as SurvivalGameRule;

        rule.OnChangedJamCount += OnChangedJam;
        rule.OnChangedWave += OnChangedWave;

        if (playerController.Character != null)
        {
            var damageable = playerController.Character.gameObject.GetComponent<Damageable>();
            damageable.OnDamageTaken += TakeDamage;

            healthBar.Initialize(playerController.Character);
        }
    }

    private void OnDestroy()
    {
        if(rule != null)
        {
            rule.OnChangedJamCount -= OnChangedJam;
            rule.OnChangedWave -= OnChangedWave;
        }

        if(playerController?.Character != null)
        {
            var damageable = playerController?.Character.GetComponent<Damageable>();
            if(damageable != null)
            {
                damageable.OnDamageTaken -= TakeDamage;
            }
        }
    }

    public void Update()
    {
        int waveTime = Mathf.FloorToInt(rule.currentWaveTime);
        int minutes = waveTime / 60;
        int seconds = waveTime % 60;

        timerText.text = $"{minutes:D2}:{seconds:D2}";
    }

    private void TakeDamage(DamageEvent info)
    {
        CharacterStats stat = playerController.Character.Stats;
        healthBar.SetHealth(stat);
    }

    private void OnChangedJam(int amount)
    {
        jamWidget.JamText.text = $"x{amount}";
        jamWidget.animator.SetTrigger("JamWidget_React");
    }

    private void OnChangedWave(int wave)
    {
        waveText.text = $"{wave + 1} Wave";
    }
}
