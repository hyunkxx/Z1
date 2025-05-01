using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class CameraMovement : MonoBehaviour
{
    //[SerializeField] private float smoothMovementFactor = 0.01f;

    [SerializeField]
    private float zoomFactor = 1f;
    private const float cameraDistance = -10;

    private UnityEngine.Rendering.Universal.PixelPerfectCamera pixelPerfectCamera;

    private GameObject viewTarget;
    
    private Rigidbody2D targetRg2d;
    private MovementComponent targetMovement;

    public Action<GameObject> OnChangeViewTarget;

    public GameObject ViewTarget => viewTarget;

    public void Awake()
    {
        pixelPerfectCamera = GetComponent<UnityEngine.Rendering.Universal.PixelPerfectCamera>();
        pixelPerfectCamera.assetsPPU = 32;
    }

    public void SetViewTarget(GameObject target)
    {
        if (!target || viewTarget == target)
            return;

        viewTarget = target;
        targetRg2d = viewTarget.GetComponent<Rigidbody2D>();
        targetMovement = viewTarget.GetComponent<MovementComponent>();

        Vector3 position = viewTarget.transform.position;
        position.z = cameraDistance;
        transform.position = position;

        OnChangeViewTarget?.Invoke(viewTarget);
    }

    private void LateUpdate()
    {
        if (!viewTarget)
            return;

        float ratio = targetRg2d.linearVelocity.magnitude / targetMovement.MaxVelocity;
        Camera.main.orthographicSize = Mathf.Lerp(3, 4, ratio * zoomFactor);

        Vector3 position = viewTarget.transform.position;
        position.z = cameraDistance;
        transform.position = position;

        //pixelPerfectCamera.assetsPPU = (int)Mathf.Lerp(3, 4, ratio);
    }
}
