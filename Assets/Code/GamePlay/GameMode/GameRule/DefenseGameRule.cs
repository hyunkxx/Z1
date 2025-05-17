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
        yield return new WaitForSeconds(WaitTime);          //������ð�

        if(CurLife <= 0)
        {
            LoseGame();
            yield break;
        }

        ++Round;
        //Spawner.Spawn(Round) // �̺κп� ���� �޾Ƽ� ���� ����

        yield return new WaitForSeconds(RoundTime);     //���潺�ð�

        if(Round > EndRound)
        {
            WinGame();
            yield break;
        }
    }

    private void SetLife()
    {
        //CurLife = FullLife - Spawner.GetMonsterSize(); //�����ʿ��� ���ͻ�����޾ƿͼ� ����
    }
}
