using UnityEngine;

public class DefenceCastle : MonoBehaviour
{
    private Damageable damageable = null;
    [SerializeField] private float hP;
    [SerializeField] private HPBar hPBar;
    DefenceGameRule gameRule;

    ETeam team;

    public float HP => hP;

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        gameRule = (DefenceGameRule)GameManager.Instance.GameMode.Rule;
    }

    void Start()
    {
        Initialize();
        hPBar.Initialize(hP);
        damageable.OnDamageTaken += OnHit;

    }

    void Update()
    {
        
    }

    public void Initialize()
    {
        team = GetComponent<Damageable>().TeamID;
    }

    public void OnHit(DamageEvent damageEvent)
    {
        hP -= damageEvent.damage;
        hPBar.ShowInfo(hP);

        if (HP > 0) return;

        if (team == ETeam.Player)
            gameRule.LoseGame();
        else
            gameRule.WinGame();
    }
}
