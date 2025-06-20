using System.Collections;
using UnityEngine;


[System.Serializable]
struct DropProperty
{
    public float _moveSpeed;
    public float _turnDelay;
    
    [HideInInspector]
    public bool  _turn;
}

public class DropObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;

    [SerializeField]
    private DropProperty _dropConfig;
    private Vector3 _invertedDirection;
    private float   _elapsedTime;

    public void OnEnable()
    {
        _elapsedTime = 0f;
    }

    public void OnDisable()
    {
        _target = null;
        _elapsedTime = 0f;
    }

    public void LateUpdate()
    {
        if (_target == null)
            return;

        _elapsedTime += Time.deltaTime;
        if (_elapsedTime < _dropConfig._turnDelay)
        {
            transform.position = Vector3.Lerp(transform.position, (transform.position + _invertedDirection), _dropConfig._moveSpeed * Time.deltaTime);
        }
        else
        {
            _dropConfig._turn = true;
            transform.position = Vector3.Lerp(transform.position, _target.transform.position, _dropConfig._moveSpeed * 2f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        GameMode mode = GameManager.Instance.GameMode;
        GameRule rule = mode.Rule;

        if (_dropConfig._turn)
        {
            ObjectPool pool = rule.PoolContainer.GetPool("DropObject_XP");
            pool.ReturnObject(gameObject);

            var survivalRule = rule as SurvivalGameRule;
            survivalRule.AddJam(1);
        }
        else
        {
            _target = collision.gameObject;
            _invertedDirection = (transform.position - _target.transform.position).normalized;
        }
    }

}
