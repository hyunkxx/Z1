using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField] private Texture2D[] textures;
    private int MaxTargetCount = 10;
    private int FrameIndex = 0;
    private float fps = 30f;
    private float fpsCounter = 0;

    public List<GameObject> targetArray = new List<GameObject>();

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
    }

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Damageable>() == null) return;

        if (targetArray.Count < MaxTargetCount)
        {
            if (collision.GetComponent<Damageable>().TeamID == ETeam.EnemyAI)
                targetArray.Add(collision.gameObject);
        }
        else
        {
            // 공격 호출
        }
    }

    void Update()
    {
        AssignTarget();

        fpsCounter += Time.deltaTime;

        if(fpsCounter >= 1f/ fps)
        {
            FrameIndex++;

            if (FrameIndex == textures.Length)
                FrameIndex = 0;

            lineRenderer.material.SetTexture("_MainTex", textures[FrameIndex]);

            fpsCounter = 0;
        }
    }

    public void AssignTarget()
    {
        lineRenderer.positionCount = targetArray.Count;

        for(int i = 0; i < targetArray.Count; ++i)
        {
            lineRenderer.SetPosition(i, targetArray[i].transform.position);
        }
    }
}