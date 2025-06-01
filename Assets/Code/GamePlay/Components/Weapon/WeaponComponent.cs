using System;
using System.Collections;
using UnityEngine;


public class WeaponComponent : MonoBehaviour
{
    private Character character;
    private SpriteRenderer weaponSprite;

    private Vector3 weaponDirection;

    public void Start()
    {
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
        float angle;
        float targetAngle;

        if (character.TargetingComponent.HasNearTarget())
        {
            weaponDirection = character.TargetingComponent.GetTargetDirection();
        }
        else
        {
            weaponDirection = character.GetCharacterDirection();
        }

        targetAngle = Mathf.Repeat(Mathf.Atan2(weaponDirection.y, weaponDirection.x) * Mathf.Rad2Deg, 360f);
        angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * 15.0f);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}