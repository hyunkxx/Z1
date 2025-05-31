using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


[System.Serializable]
public struct GhostEffectParam
{
    public int count;
    public float activeInterval;
    public float duration;
    public Color beginColor;
    public Color endColor;
}

public class GhostEffect : MonoBehaviour
{
    [SerializeField] private GhostEffectParam ghostParam;
    [SerializeField] private ObjectPool effectPool;

    private SpriteRenderer sourceSprite;
    private Character character;

    Material tintMaterial;

    public void Initialize(SpriteRenderer source)
    {
        if(tintMaterial == null)
        {
            tintMaterial = Resources.Load<Material>("Common/Materials/MAT_Tint");
        }

        sourceSprite = source;
        character = source.gameObject.transform.root.GetComponent<Character>();

        effectPool = gameObject.AddComponent<ObjectPool>();
        GameObject Ghost = new GameObject("Ghost");
        SpriteRenderer renderer = Ghost.AddComponent<SpriteRenderer>();
        renderer.sprite = source.sprite;
        renderer.material = tintMaterial;
        renderer.color = ghostParam.beginColor;

        effectPool.InitializePool(Ghost, 20);
        Destroy(Ghost);
    }
    public void ActivateEffect()
    {
        StartCoroutine(CoroutineActivateGhost());
    }

    IEnumerator CoroutineActivateGhost()
    {
        int spawnCount = 0;
        while (spawnCount < ghostParam.count)
        {
            GameObject obj = effectPool.GetObject(transform.position, transform.rotation);
            if(obj)
            {
                SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
                sprite.color = ghostParam.beginColor;
                sprite.flipX = character.IsRight() ? false : true;

                StartCoroutine(CoroutineChangeColor(obj, sprite));
                spawnCount++;
            }

            yield return new WaitForSeconds(ghostParam.activeInterval);
        }
    }

    IEnumerator CoroutineChangeColor(GameObject obj, SpriteRenderer sprite)
    {
        float elapsedTime = 0f;
        Color originColor = sprite.color;
        while(elapsedTime < ghostParam.duration)
        {
            elapsedTime += Time.deltaTime;
            sprite.material.color = Color.Lerp(originColor, ghostParam.endColor, elapsedTime / ghostParam.duration);
            yield return null;
        }

        effectPool.ReturnObject(obj);
    }
}
