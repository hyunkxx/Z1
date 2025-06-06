using UnityEngine;

[CreateAssetMenu(fileName = "ActionRadialSpawn", menuName = "Scriptable Objects/Actions/ActionRadialSpawn")]
public class ActionRadialSpawn : BaseAction
{
    [SerializeField]
    protected GameObject m_SourcePrefab;

    [SerializeField]
    protected int m_spawnCount = 1;

    [SerializeField]
    protected string m_animationTrigger;

    public int SpawnCount => m_spawnCount;
    public string AnimationTrigger => m_animationTrigger;

    private Character character;
    protected override bool InternalExecuteAction(ICharacterQueryable InQueryable)
    {
        character = InQueryable.GetCharacter();
        if (character == null)
            return false;

        if (m_SourcePrefab == null)
            return false;

        GameObject target = character.TargetingComponent.GetNearestTarget();
        if (target == null)
            return false;

        Vector3 throwDirection = (target.transform.position - character.transform.position).normalized;
        RadialSpawn(throwDirection, character.transform.position);

        if (!string.IsNullOrEmpty(m_animationTrigger))
        {
            character.Animator.SetTrigger(m_animationTrigger);
        }

        return true;
    }

    private void RadialSpawn(Vector3 forward, Vector3 position, float positionSpacing = 0.5f, float angleSpacing = 10f)
    {
        Vector3 right = new Vector2(-forward.y, forward.x).normalized;

        int mid = m_spawnCount / 2;
        bool isEven = m_spawnCount % 2 == 0;

        for (int i = 0; i < m_spawnCount; i++)
        {
            int offsetIndex = i - mid;
            if (isEven && offsetIndex >= 0)
                offsetIndex += 1;

            Vector3 spawnPos = position + right * (offsetIndex * positionSpacing);

            float angleOffset = offsetIndex * angleSpacing;
            float baseAngle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
            float finalAngle = baseAngle + angleOffset;

            Quaternion rotation = Quaternion.Euler(0, 0, finalAngle);

            GameObject obj = Instantiate(m_SourcePrefab, spawnPos, rotation);
            Ability ability = obj.GetComponent<Ability>();
            if (ability)
            {
                ability.Activate(character.gameObject, null);
            }
        }
    }
}
