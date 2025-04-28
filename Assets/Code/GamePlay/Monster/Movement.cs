using UnityEngine;

public class Movement : MonoBehaviour
{
    public float MoveSpeed = 1.5f;

    void Move()
    {
        this.gameObject.transform.position += new Vector3(1, 0, 0) * Time.deltaTime;
    }

    void MoveDesPos(Vector3 _destination)
    {
        if (Vector2.Distance(_destination, transform.position) < 0.5f) return;

        Vector3 direction = _destination - this.transform.position;
        direction = direction.normalized;
        this.gameObject.transform.position += direction * MoveSpeed * Time.deltaTime;
    }
}
