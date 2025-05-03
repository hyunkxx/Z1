using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class TargetElement
{
    public float weight;
    public GameObject target;

    public TargetElement(GameObject obj)
    {
        this.weight = 0f;
        this.target = obj;
    }
    public TargetElement(GameObject obj, float weight)
    {
        this.weight = 0f;
        this.target = obj;
    }
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        TargetElement other = (TargetElement)obj;
        return this.target == other.target;
    }
    public override int GetHashCode()
    {
        return target != null ? target.GetHashCode() : 0;
    }
}
public class TargetingComponent : MonoBehaviour
{
    //[SerializeField] private float scanRadius;
    //[SerializeField] private float scanInterval;
    //[SerializeField] private int maxTargetCount;
    private List<TargetElement> targetList = new List<TargetElement>();
    private CircleCollider2D targetingCollider;
    private Character character;

    public void Awake()
    {
        character = transform.root.GetComponent<Character>();
        targetingCollider = GetComponent<CircleCollider2D>();
    }
    public void Start()
    {
        StartCoroutine(UpdatePriorityWeights());
    }
    public bool HasNearTarget()
    {
        return targetList.Count > 0;
    }
    public GameObject GetNearestTarget()
    {
        return HasNearTarget() ? targetList[0].target : null;
    }
    public Vector3 GetTargetDirection()
    {
        return (targetList[0].target.transform.position - transform.position).normalized;
    }
    private IEnumerator UpdatePriorityWeights()
    {
        while(true)
        {
            if(targetList.Count > 0)
            {
                for(int i = 0; i < targetList.Count; ++i)
                {
                    Vector3 targetPosition = targetList[i].target.transform.position;
                    float distance = Vector2.Distance(targetPosition, transform.position);
                    float distanceWeight = 1f - Mathf.Clamp01(distance / targetingCollider.radius);

                    Vector2 characterDir = character.GetCharacterDirection();
                    Vector2 targetDirection = targetPosition - transform.position.normalized;
                    float directionWeight = Vector2.Dot(characterDir, targetDirection);
                    directionWeight = Mathf.Clamp01((directionWeight + 1f / 2f));

                    targetList[i].weight = distanceWeight + directionWeight;
                }

                SortByPriorityWeights();
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void SortByPriorityWeights()
    {
        targetList.Sort((a, b) =>
        {
            return a.weight.CompareTo(b.weight);
        });

        for (int i = 0; i < targetList.Count; ++i)
        {
            SpriteRenderer sprite = targetList[i].target.GetComponent<SpriteRenderer>();
            sprite.color = new Color(targetList[i].weight, 0f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable owner = transform.root.GetComponent<Damageable>();
        Damageable other = collision.GetComponent<Damageable>();
        if (!owner || !other)
            return;

        if(owner.IsEnemy(other))
        {
            TargetElement element = new TargetElement(collision.gameObject);

            targetList.Add(element);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Damageable owner = transform.root.GetComponent<Damageable>();
        Damageable other = collision.GetComponent<Damageable>();
        if (!owner || !other)
            return;

        if (owner.IsEnemy(other))
        {
            TargetElement find = targetList.Find(e => e.target == collision.gameObject);
            targetList.Remove(find);

            SpriteRenderer sprite = collision.gameObject.GetComponent<SpriteRenderer>();
            sprite.color = Color.white;
        }
    }
}
