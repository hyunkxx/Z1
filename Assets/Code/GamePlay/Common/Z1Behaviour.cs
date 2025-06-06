using UnityEngine;


public abstract class Z1Behaviour
    : MonoBehaviour
{
    protected virtual void Awake() { }
    protected virtual void Start() { }
    protected virtual void Update() { }
    protected virtual void LateUpdate() { }
    protected virtual void FixedUpdate() { }

    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }
    protected virtual void OnDestroy() { }

    //public abstract void Initialize();
    //public abstract void Uninitialize();
}
