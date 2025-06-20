using UnityEngine;


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

    PlayerController playerController;
    SurvivalGameRule rule;

    public override void Initialize()
    {
        playerController = GameManager.Instance.GameMode.PlayerController;
        rule = GameManager.Instance.GameMode.Rule as SurvivalGameRule;

        rule.OnChangedJamCount += OnChangedJam;

        if (playerController.Character != null)
        {
            var damageable = playerController.Character.gameObject.GetComponent<Damageable>();
            damageable.OnDamageTaken += TakeDamage;
        }
    }

    private void OnDestroy()
    {
        if(rule != null)
        {
            rule.OnChangedJamCount -= OnChangedJam;
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

    private void TakeDamage(DamageEvent info)
    {
        var stat = playerController.Character.Stats;
        float ratio = stat.GetStat(EStatType.CurHealth) / stat.GetStat(EStatType.MaxHealth);
        healthBar.SetHealth(ratio);
    }

    private void OnChangedJam(int amount)
    {
        jamWidget.JamText.text = $"x{amount}";
        jamWidget.animator.SetTrigger("JamWidget_React");
    }
}
