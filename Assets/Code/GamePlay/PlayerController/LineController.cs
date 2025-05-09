using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField] private Texture2D[] textures;
    private int FrameIndex = 0;
    private float fps = 30f;
    private float fpsCounter = 0;

    Vector2 startPos = Vector2.zero;
    [SerializeField] Transform[] targetPosArray;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
    }

    // Update is called once per frame
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

    void AssignTarget()
    {
        startPos = transform.position;
        lineRenderer.positionCount = targetPosArray.Length + 1;
        lineRenderer.SetPosition(0, startPos);

        for(int i = 0; i < targetPosArray.Length; ++i)
        {
            lineRenderer.SetPosition(i + 1, targetPosArray[i].position);
        }
    }
}