using System;
using System.Collections;
using UnityEngine;


public sealed class WeaponComponent : MonoBehaviour
{
    private Character character;
    private SpriteRenderer weaponSprite;
    private Animator animator;

    public Vector3 WeaponDirection { get; private set; }

    public void Awake()
    {
        animator = GetComponent<Animator>();
        weaponSprite = GetComponent<SpriteRenderer>();
        character = GetComponentInParent<Character>();

        character.OnChangedFlip += FlipX;
    }
    public void OnDestroy()
    {
        if (character)
        {
            character.OnChangedFlip -= FlipX;
        }
    }

    public void FlipX(bool bFlip)
    {
        weaponSprite.flipY = bFlip;
        Vector3 localTransform = transform.localPosition;
        localTransform.x *= -1f;
        transform.localPosition = localTransform;
    }

    public void LateUpdate()
    {
        AlignToWeapon();
    }

    private void AlignToWeapon()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Character_Weapon_Attack"))
            return;

        float angle;
        float targetAngle;

        if (character.TargetingComponent.HasNearTarget())
        {
            WeaponDirection = character.TargetingComponent.GetTargetDirection();

            targetAngle = Mathf.Repeat(Mathf.Atan2(WeaponDirection.y, WeaponDirection.x) * Mathf.Rad2Deg, 360f);
            angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * 30.0f);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            WeaponDirection = character.GetCharacterDirection();

            targetAngle = Mathf.Repeat(Mathf.Atan2(WeaponDirection.y, WeaponDirection.x) * Mathf.Rad2Deg, 360f);
            angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * 15.0f);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}