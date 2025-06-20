using System;
using UnityEngine;


[System.Serializable]
public struct ProjectileProperty
{
    [SerializeField] public float speed;
    [SerializeField] public float lifeTime;
    [NonSerialized] public float activateTime;
}

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile2D : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private ProjectileProperty projectileProperty;

    public void Initialize(Vector3 direction)
    {
        rigid.linearVelocity = direction * projectileProperty.speed;
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.linearVelocity = transform.right * projectileProperty.speed;
    }

    private void Update()
    {
        projectileProperty.activateTime += Time.deltaTime;
        if (projectileProperty.lifeTime <= projectileProperty.activateTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable script = collision.gameObject.GetComponent<Damageable>();
        //Debug.Log(collision.gameObject.name);
    }
}