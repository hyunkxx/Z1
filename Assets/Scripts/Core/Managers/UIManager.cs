using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;


public class UIManager : Singleton<UIManager>
{
    private AsyncOperationHandle<GameObject> MessagePopup_Handle;
    public MessagePopup MessagePopup { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        MessagePopup_Handle = Addressables.InstantiateAsync("MessagePopup");
        MessagePopup_Handle.Completed += handle => 
        {
            MessagePopup = handle.Result.gameObject.GetComponent<MessagePopup>();
            MessagePopup.transform.SetParent(gameObject.transform);
        };
    }

    protected override void OnDestroy()
    {
        if(MessagePopup_Handle.IsValid() || MessagePopup_Handle.Result != null)
        {
            Addressables.ReleaseInstance(MessagePopup_Handle);
        }

        base.OnDestroy();
    }
}
