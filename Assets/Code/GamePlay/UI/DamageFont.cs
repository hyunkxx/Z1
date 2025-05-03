using System;
using System.Collections;
using UnityEngine;
using TMPro;


public class DamageFont : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    private TextMeshProUGUI text;

    public void Show(DamageEvent damage, Color color)
    {
        Vector2 offset = UnityEngine.Random.insideUnitCircle * 0.2f;
        transform.position += (Vector3)offset;

        text.text = damage.damage.ToString();
        text.color = color;
        StartCoroutine(CoroutineTimer());
    }
    public void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void OnDisable()
    {
        text.text = "";
        text.color = Color.white;
    }

    IEnumerator CoroutineTimer()
    {
        float elapsed = 0f;
        float duration = curve.keys[curve.length - 1].time;
        Vector3 origin = transform.position;
        Color color = text.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float delta = Mathf.Clamp01(elapsed / duration);
            float value = curve.Evaluate(delta);

            text.color = Color.Lerp(color, Color.gray, delta);

            Vector3 position = origin;
            position.x += delta * 0.25f;
            position.y += value;

            transform.position = position;

            yield return null;
        }

        ObjectPool pool = PoolManager.Instance.GetPool("DamageFont");
        pool.ReturnObject(gameObject);
    }
}
