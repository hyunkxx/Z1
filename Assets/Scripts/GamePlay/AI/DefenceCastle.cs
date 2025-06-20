using UnityEngine;

public class DefenceCastle : MonoBehaviour
{
    private Damageable damageable = null;
    [SerializeField] private float hP;
    [SerializeField] private HPBar hPBar;

    public float HP => hP;

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        hPBar.Initialize(hP);
    }

    void Start()
    {
        damageable.OnDamageTaken += OnHit;
    }

    void Update()
    {
        
    }

    public void Initialize(ETeam team)
    {

    }

    public void OnHit(DamageEvent damageEvent)
    {
        hP -= damageEvent.damage;
        hPBar.ShowInfo(hP);
    }
}
