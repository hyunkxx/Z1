using System.Collections.Generic;
using UnityEngine;

public class DefencePlayerController : PlayerController
{
    private CameraDragHandlerer CameraHandlerer;
    public GameObject Skill_Prefab;

    protected override void Start()
    {
        base.Awake();
        CameraHandlerer = Camera.main.GetComponent<CameraDragHandlerer>();
    }
    protected override void Update()
    {

    }

    public void ActioveSkill_0()
    {
        GameObject obj = Instantiate(Skill_Prefab);
        CameraHandlerer.ChangeState(CameraDragHandlerer.ECameraMoveState.Targeting, obj);
    }
}