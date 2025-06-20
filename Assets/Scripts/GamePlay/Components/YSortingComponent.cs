using System.Collections;
using UnityEngine;

public class YSortingComponent : MonoBehaviour
{
    private float minY = -5f;
    private float maxY = 6f;
    private float minScale = 1.0f;
    private float maxScale = 0.7f;

    Coroutine YSortingCoroutine;
    SpriteRenderer[] spriteRenderers;

    private void Awake()
    {
        spriteRenderers = gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnBecameVisible()
    {
        if (YSortingCoroutine != null) return;

        YSortingCoroutine = StartCoroutine(YSorting());
    }

    private void OnBecameInvisible()
    {
        if (YSortingCoroutine == null) return;

        StopCoroutine(YSortingCoroutine);
    }

    private IEnumerator YSorting()
    {
        while (gameObject.activeSelf)
        {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
                spriteRenderer.sortingOrder = (int)(transform.parent.position.y * -100);

            float t = Mathf.InverseLerp(maxY, minY, transform.parent.position.y);
            float scale = Mathf.Lerp(maxScale, minScale, t);

            transform.parent.localScale = new Vector3(scale, scale, 1f);

            yield return new WaitForSeconds(0.25f);
        }
    }
}
