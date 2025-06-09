using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UIParticleComponent : MaskableGraphic
{
    public bool fixedTime = true;
    private bool isInitialised = false;

    private ParticleSystem pSystem;
    private ParticleSystemRenderer particleRenderer;
    private ParticleSystem.Particle[] particleArray;

    private UIVertex[] UIVertexs = new UIVertex[4];
    private Vector4 imageUV = Vector4.zero;

    private Material currentMaterial;
    private Texture currentTexture;

    private ParticleSystem.TextureSheetAnimationModule textureSheetAnimation;
    private int textureSheetAnimationFrames;
    private Vector2 textureSheetAnimationFrameSize;

    private ParticleSystem.MainModule mainModule;

    public override Texture mainTexture
    {
        get
        {
            return currentTexture;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (!Initialize())
            enabled = false;
    }

    void Update()
    {
        if (!fixedTime && Application.isPlaying)
        {
            pSystem.Simulate(Time.unscaledDeltaTime, false, false, true);
            SetAllDirty();

            if ((currentMaterial != null && currentTexture != currentMaterial.mainTexture) ||
                (material != null && currentMaterial != null && material.shader != currentMaterial.shader))
            {
                pSystem = null;
                Initialize();
            }
        }
    }

    private void LateUpdate()
    {
        if (!Application.isPlaying)
        {
            SetAllDirty();
        }
        else
        {
            if (fixedTime)
            {
                pSystem.Simulate(Time.unscaledDeltaTime, false, false, true);
                SetAllDirty();
                if ((currentMaterial != null && currentTexture != currentMaterial.mainTexture) ||
                    (material != null && currentMaterial != null && material.shader != currentMaterial.shader))
                {
                    pSystem = null;
                    Initialize();
                }
            }
        }
        if (material == currentMaterial)
            return;
        pSystem = null;
        Initialize();
    }

    protected override void OnDestroy()
    {
        currentMaterial = null;
        currentTexture = null;
    }

    bool Initialize()
    {
        pSystem = GetComponent<ParticleSystem>();
        particleRenderer = pSystem.GetComponent<ParticleSystemRenderer>();
        mainModule = pSystem.main;

        if (pSystem == null)
            return false;

        if (particleRenderer != null)
            particleRenderer.enabled = false;

        if (pSystem.main.maxParticles > 14000)
            mainModule.maxParticles = 14000;

        if (material == null)
        {
            var foundShader = Shader.Find("UI/Additive");
            if (foundShader)
            {
                material = new Material(foundShader);
            }
        }

        currentMaterial = material;

        if (currentMaterial && currentMaterial.HasProperty("_MainTex"))
        {
            currentTexture = currentMaterial.mainTexture;
            if (currentTexture == null)
                currentTexture = Texture2D.whiteTexture;
        }

        if (particleArray == null)
            particleArray = new ParticleSystem.Particle[pSystem.main.maxParticles];

        textureSheetAnimation = pSystem.textureSheetAnimation;
        if (textureSheetAnimation.enabled)
        {
            textureSheetAnimationFrames = textureSheetAnimation.numTilesX * textureSheetAnimation.numTilesY;
            textureSheetAnimationFrameSize = new Vector2(1f / textureSheetAnimation.numTilesX, 1f / textureSheetAnimation.numTilesY);
        }
        else
        {
            textureSheetAnimationFrames = 0;
            textureSheetAnimationFrameSize = Vector2.zero;
        }

        mainModule.scalingMode = ParticleSystemScalingMode.Hierarchy;

        imageUV = new Vector4(0, 0, 1, 1);

        return true;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (!Initialize())
            {
                return;
            }
        }
#endif

        vh.Clear();

        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (!isInitialised && !pSystem.main.playOnAwake)
        {
            pSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            isInitialised = true;
        }


        Vector2 tempUV = Vector2.zero;
        Vector2 corner1 = Vector2.zero;
        Vector2 corner2 = Vector2.zero;

        int particleCount = pSystem.GetParticles(particleArray);

        for (int i = 0; i < particleCount; ++i)
        {
            ParticleSystem.Particle particle = particleArray[i];

            Vector2 position = (mainModule.simulationSpace == ParticleSystemSimulationSpace.Local ? particle.position : transform.InverseTransformPoint(particle.position));

            float rotation = -particle.rotation * Mathf.Deg2Rad;
            float rotation90 = rotation + Mathf.PI / 2;
            Color32 color = particle.GetCurrentColor(pSystem);
            float size = particle.GetCurrentSize(pSystem) * 0.5f;
            Vector4 particleUV = imageUV;

            if (mainModule.scalingMode == ParticleSystemScalingMode.Shape)
                position /= canvas.scaleFactor;

            if (textureSheetAnimation.enabled)
                tempUV = CalculateParticleUV(particle, textureSheetAnimation);
            else
                tempUV = imageUV;

            SetParticleUV(particleUV, color, ref UIVertexs);

            SetParticlePositions(ref UIVertexs, position, size, rotation);

            vh.AddUIVertexQuad(UIVertexs);
        }

    }

    public void StartParticleEmission()
    {
        //pSystem.Play();
        gameObject.SetActive(true);
        StartCoroutine(OnActiveFalse());

    }

    public void StopParticleEmission()
    {
        pSystem.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void PauseParticleEmission()
    {
        pSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }

    IEnumerator OnActiveFalse()
    {
        yield return new WaitForSeconds(mainModule.startLifetimeMultiplier);
        gameObject.SetActive(false);
        Debug.Log("Particle Active False");
    }

    Vector4 CalculateParticleUV(ParticleSystem.Particle particle, ParticleSystem.TextureSheetAnimationModule textureSheetAnimation)
    {
        Vector4 particleUV = Vector4.zero;

        float frameProgress = 1 - (particle.remainingLifetime / particle.startLifetime);

        if (textureSheetAnimation.frameOverTime.curveMin != null)
        {
            frameProgress = textureSheetAnimation.frameOverTime.curveMin.Evaluate(1 - (particle.remainingLifetime / particle.startLifetime));
        }
        else if (textureSheetAnimation.frameOverTime.curve != null)
        {
            frameProgress = textureSheetAnimation.frameOverTime.curve.Evaluate(1 - (particle.remainingLifetime / particle.startLifetime));
        }
        else if (textureSheetAnimation.frameOverTime.constant > 0)
        {
            frameProgress = textureSheetAnimation.frameOverTime.constant - (particle.remainingLifetime / particle.startLifetime);
        }

        frameProgress = Mathf.Repeat(frameProgress * textureSheetAnimation.cycleCount, 1);
        int frame = 0;

        switch (textureSheetAnimation.animation)
        {

            case ParticleSystemAnimationType.WholeSheet:
                frame = Mathf.FloorToInt(frameProgress * textureSheetAnimationFrames);
                break;

            case ParticleSystemAnimationType.SingleRow:
                frame = Mathf.FloorToInt(frameProgress * textureSheetAnimation.numTilesX);

                int row = textureSheetAnimation.rowIndex;
                frame += row * textureSheetAnimation.numTilesX;
                break;

        }

        frame %= textureSheetAnimationFrames;

        particleUV.x = (frame % textureSheetAnimation.numTilesX) * textureSheetAnimationFrameSize.x;
        particleUV.y = 1.0f - Mathf.FloorToInt(frame / textureSheetAnimation.numTilesX) * textureSheetAnimationFrameSize.y;
        particleUV.z = particleUV.x + textureSheetAnimationFrameSize.x;
        particleUV.w = particleUV.y + textureSheetAnimationFrameSize.y;

        return particleUV;
    }

    private void SetParticleUV(Vector4 particleUV, Color32 color, ref UIVertex[] _uiVertexs)
    {
        Vector2[] uvCorners = new Vector2[4]
        {
        new Vector2(particleUV.x, particleUV.y), // 왼쪽 아래
        new Vector2(particleUV.x, particleUV.w), // 왼쪽 위
        new Vector2(particleUV.z, particleUV.w), // 오른쪽 위
        new Vector2(particleUV.z, particleUV.y)  // 오른쪽 아래
        };

        for (int i = 0; i < 4; i++)
        {
            _uiVertexs[i] = UIVertex.simpleVert;
            _uiVertexs[i].color = color;
            _uiVertexs[i].uv0 = uvCorners[i];
        }
    }

    private void SetParticlePositions(ref UIVertex[] _uiVertexs, Vector2 position, float size, float rotation)
    {
        if (rotation == 0f)
        {
            Vector2 corner1 = new Vector2(position.x - size, position.y - size);
            Vector2 corner2 = new Vector2(position.x + size, position.y + size);

            _uiVertexs[0].position = corner1;
            _uiVertexs[1].position = new Vector2(corner1.x, corner2.y);
            _uiVertexs[2].position = corner2;
            _uiVertexs[3].position = new Vector2(corner2.x, corner1.y);
        }
        else
        {
            float cos = Mathf.Cos(rotation);
            float sin = Mathf.Sin(rotation);
            float cos90 = Mathf.Cos(rotation + Mathf.PI / 2);
            float sin90 = Mathf.Sin(rotation + Mathf.PI / 2);

            Vector2 right = new Vector2(cos, sin) * size;
            Vector2 up = new Vector2(cos90, sin90) * size;

            _uiVertexs[0].position = position - right - up;
            _uiVertexs[1].position = position - right + up;
            _uiVertexs[2].position = position + right + up;
            _uiVertexs[3].position = position + right - up;
        }
    }
}
