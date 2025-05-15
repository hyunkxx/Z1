using System;
using System.Collections;
using UnityEngine;


struct WeaponSwingParameter
{
    AnimationCurve curve;
    float swingAngle;
}

public class WeaponComponent : MonoBehaviour
{
    public enum EWeaponComponentState
    {
        Idle,
        Move,
        Swing
    }

    [SerializeField] private DefaultSlashEffect slashEffect;

    private Character character;
    private MovementComponent movement;
    private SpriteRenderer weaponSprite;

    private float pivotAngle;
    private float offsetAngle;
    private bool isWeaponLowered;

    private Vector3 weaponDirection;
    private EWeaponComponentState weaponState;

    private Coroutine weaponRotateCoroutine;
    private Coroutine weaponResetCoroutine;

    public void Start()
    {
        GameObject holder = new GameObject("EffectHolder");
        holder.transform.SetParent(transform.parent);
        slashEffect = Instantiate(slashEffect, holder.transform);

        weaponSprite = GetComponent<SpriteRenderer>();
        character = GetComponentInParent<Character>();

        movement = character.Movement;

        character.OnChangedFlip += FlipX;
    }
    public void OnDestroy()
    {
        if(character)
        {
            character.OnChangedFlip -= FlipX;
        }
    }

    public void FlipX(bool bFlip)
    {
        weaponSprite.flipX = bFlip;
        Vector3 localTransform = transform.localPosition;
        localTransform.x *= -1f;
        transform.localPosition = localTransform;
    }

    public void ChangeWeaponState(EWeaponComponentState state)
    {
        if (weaponState == state)
            return;

        weaponState = state;
    }

    public void LateUpdate()
    {
        /* temp */
        if (Input.GetMouseButtonDown(0))
        {
            if (weaponRotateCoroutine == null)
                Swing();
        }

        float angle;
        float targetAngle;
        switch (weaponState)
        {
            case EWeaponComponentState.Idle:
            case EWeaponComponentState.Move:
                pivotAngle = character.IsRight() ? 0f : 180f;

                if(character.TargetingComponent.HasNearTarget())
                {
                    weaponDirection = character.TargetingComponent.GetTargetDirection();
                }
                else
                {
                    weaponDirection = character.GetCharacterDirection();
                }

                targetAngle = Mathf.Repeat(Mathf.Atan2(weaponDirection.y, weaponDirection.x) * Mathf.Rad2Deg + pivotAngle + offsetAngle, 360f);
                angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * 15.0f);
                transform.rotation = Quaternion.Euler(0, 0, angle);

                break;
            case EWeaponComponentState.Swing:
                break;
        }
    }

    public void Swing()
    {
        TransformData trans = new TransformData();
        trans.position = transform.position + weaponDirection * 0.05f;
        trans.localPosition = transform.localPosition;

        float angle = Mathf.Atan2(weaponDirection.y, weaponDirection.x) * Mathf.Rad2Deg;
        trans.rotation = Quaternion.Euler(0f, 0f, angle);

        slashEffect.ActivateEffect(character, trans, isWeaponLowered);
        weaponRotateCoroutine = StartCoroutine(CoroutineRotateWeapon());
    }

    public void ResetWeaponState()
    {
        offsetAngle = 0f;
        isWeaponLowered = false;
        weaponSprite.sortingOrder = 0;
    }

    IEnumerator CoroutineResetWeapon(float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ResetWeaponState();
        weaponResetCoroutine = null;
    }

    IEnumerator CoroutineRotateWeapon()
    {
        weaponState = EWeaponComponentState.Swing;
        GameManager.Instance.GameMode.PlayerController.SetInputLock(true);

        if(weaponResetCoroutine != null)
        {
            StopCoroutine(weaponResetCoroutine);
            weaponResetCoroutine = null;
        }

        float swingAngle = 175f;
        const float swingSpeed = 35f;

        float currentAngle = transform.eulerAngles.z;
        float targetAngle;

        if (character.IsRight())
        {
            swingAngle = isWeaponLowered ? -swingAngle : swingAngle;
            targetAngle = transform.eulerAngles.z - swingAngle;
        }
        else
        {
            swingAngle = isWeaponLowered ? -swingAngle : swingAngle;
            targetAngle = transform.eulerAngles.z + swingAngle;
        }

        offsetAngle += 180f;
        offsetAngle %= 360f;

        while (true)
        {
            float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * swingSpeed);
            transform.rotation = Quaternion.Euler(0, 0, angle);

            float delta = Mathf.DeltaAngle(angle, targetAngle);
            if (Mathf.Abs(delta) < 0.1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, targetAngle);
                break;
            }

            yield return null;
        }

        isWeaponLowered = !isWeaponLowered;
        if (isWeaponLowered)
        {
            weaponSprite.sortingOrder = 2;

            if (weaponResetCoroutine == null)
            {
                weaponResetCoroutine = StartCoroutine(CoroutineResetWeapon(2f));
            }
        }
        else
        {
            weaponSprite.sortingOrder = 0;
        }

        weaponState = EWeaponComponentState.Idle;
        GameManager.Instance.GameMode.PlayerController.SetInputLock(false);

        weaponRotateCoroutine = null;
    }
}
