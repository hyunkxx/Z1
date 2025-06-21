using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;


class MessageInfo
{
    public string Message;
    public Action Complete;

    public MessageInfo(string text, Action onComplte)
    {
        Message = text;
        Complete = onComplte;
    }
};

public class MessagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI message;
    [SerializeField] private Animator animator;

    private Queue<MessageInfo> messageQueue = new();
    private MessageInfo currentMessage;

    public void Awake()
    {
        StartCoroutine(MessageMonitor());
    }

    public void Message(string text, Action onComplete = null)
    {
        MessageInfo message = new MessageInfo(text, onComplete);
        messageQueue.Enqueue(message);
    }

    public void AnimationFinished()
    {
        currentMessage.Complete?.Invoke();
        currentMessage = null;
    }

    IEnumerator MessageMonitor()
    {
        while(true)
        {
            if (messageQueue.Count > 0 && currentMessage == null)
            {
                currentMessage = messageQueue.Dequeue();
                message.text = currentMessage.Message;
                animator.SetTrigger("Pop");
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}
 