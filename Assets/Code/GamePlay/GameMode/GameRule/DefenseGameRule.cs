using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class DefenceGameRule
    : GameRule
{
    [SerializeField]
    GameObject Spawner;

    private float WaitTime = 10f;
    private float RoundTime = 30f;
    private int FullLife = 20;
    private int CurLife = 0;
    private int Round = 0;
    private int EndRound = 5;

    protected override void Awake()
    {
        base.Awake();

        CurLife = FullLife;
    }


    protected override void Start()
    {
        base.Start();

        StartCoroutine(RoundCheck());
    }

    protected override void Update()
    {
        base.Update();
        SetLife();
    }

    private void WinGame()
    {
        Debug.Log("Win");
    }

    private void LoseGame()
    {
        Debug.Log("Lose");
    }

    private IEnumerator RoundCheck()
    {
        yield return new WaitForSeconds(WaitTime);          //라운드대기시간

        if(CurLife <= 0)
        {
            LoseGame();
            yield break;
        }

        ++Round;
        //Spawner.Spawn(Round) // 이부분에 라운드 받아서 몬스터 스폰

        yield return new WaitForSeconds(RoundTime);     //디펜스시간

        if(Round > EndRound)
        {
            WinGame();
            yield break;
        }
    }

    private void SetLife()
    {
        //CurLife = FullLife - Spawner.GetMonsterSize(); //스포너에서 몬스터사이즈받아와서 빼기
    }
}
