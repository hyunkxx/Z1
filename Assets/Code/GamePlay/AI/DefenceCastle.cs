using UnityEngine;

public class DefenceCastle : MonoBehaviour
{
    private Damageable damageable = null;
    //[SerializeField] private float HP;

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        //HP = 100f;

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Initialize(ETeam team)
    {

    }
}
