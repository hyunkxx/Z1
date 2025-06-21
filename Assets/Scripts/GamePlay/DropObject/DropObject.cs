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
    private GameObject _target;
    private Collider2D _collision;

    [SerializeField]
    private DropProperty _dropConfig;
    private Vector3 _invertedDirection;
    private Vector3 _cachedPosition;
    private float   _elapsedTime;

    public void Awake()
    {
        _collision = GetComponent<Collider2D>();
        _collision.isTrigger = true;
    }

    public void OnEnable()
    {
        _target = null;
        _elapsedTime = 0f;
        _collision.enabled = false;
        _dropConfig._turn = false;
        _cachedPosition = Vector3.zero;
    }

    public void OnDisable()
    {
        _target = null;
        _elapsedTime = 0f;
        _collision.enabled = false;
        _dropConfig._turn = false;
        _cachedPosition = Vector3.zero;
    }

    public void Update()
    {
        if (_target == null)
            return;

        _elapsedTime += Time.deltaTime;
        if (_elapsedTime < _dropConfig._turnDelay)
        {
            transform.position = Vector3.Lerp(transform.position, (_cachedPosition + _invertedDirection), _dropConfig._moveSpeed * Time.deltaTime);
        }
        else
        {
            _dropConfig._turn = true;
            _collision.enabled = true;
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
            _collision.enabled = false;
            _target = collision.gameObject;
            _cachedPosition = transform.position;
            _invertedDirection = (transform.position - _target.transform.position).normalized;
        }
    }

    public void AnimationFinished()
    {
        _collision.enabled = true;
    }
}
