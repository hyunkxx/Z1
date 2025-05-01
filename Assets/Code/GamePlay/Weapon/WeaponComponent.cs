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

    [SerializeField] private Transform weaponSocket;
    [SerializeField] private SpriteRenderer weaponSprite;
    [SerializeField] private DefaultSlashEffect slashEffect;

    private Character character;
    private MovementComponent movement;

    private float pivotAngle;
    private float offsetAngle;
    private bool isWeaponLowered;

    private Vector3 weaponDirection;
    private EWeaponComponentState weaponState;

    private Coroutine weaponRotateCoroutine;
    private Coroutine weaponResetCoroutine;

    public void Awake()
    {
        GameObject holder = new GameObject("EffectHolder");
        holder.transform.SetParent(transform.parent);
        slashEffect = Instantiate(slashEffect, holder.transform);

        movement = GetComponentInParent<MovementComponent>();
        character = GetComponentInParent<Character>();
    }

    public void ChangeWeaponState(EWeaponComponentState state)
    {
        if (weaponState == state)
            return;

        weaponState = state;
    }

    public void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(weaponRotateCoroutine == null)
                Swing();
        }

        float angle;
        float targetAngle;
        switch (weaponState)
        {
            case EWeaponComponentState.Idle:
            case EWeaponComponentState.Move:
                pivotAngle = character.IsRight() ? 0f : 180f;

                if (movement.IsMove())
                {
                    weaponDirection = movement.MoveDirection;
                }
                else
                {
                    weaponDirection = character.IsRight() ? Vector2.right : -Vector2.right;
                }

                targetAngle = Mathf.Atan2(weaponDirection.y, weaponDirection.x) * Mathf.Rad2Deg + pivotAngle + offsetAngle;
                angle = Mathf.LerpAngle(weaponSocket.transform.eulerAngles.z, targetAngle, Time.deltaTime * 15.0f);
                weaponSocket.transform.rotation = Quaternion.Euler(0, 0, angle);

                break;
            case EWeaponComponentState.Swing:
                break;
        }
    }

    public void Swing()
    {
        TransformData trans = new TransformData();
        trans.position = transform.position + weaponDirection * 0.1f;
        trans.localPosition = weaponSocket.localPosition;

        float angle = Mathf.Atan2(weaponDirection.y, weaponDirection.x) * Mathf.Rad2Deg;
        angle += character.IsRight() ? 0f : 180f;

        trans.rotation = Quaternion.Euler(0f, 0f, angle);

        slashEffect.ActivateSlash(trans, isWeaponLowered);
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
        GameManager.Instance.GameMode.playerController.SetInputLock(true);

        if(weaponResetCoroutine != null)
        {
            StopCoroutine(weaponResetCoroutine);
            weaponResetCoroutine = null;
        }

        float swingAngle = 160f;
        const float swingSpeed = 35f;

        float currentAngle = weaponSocket.transform.eulerAngles.z * Mathf.Rad2Deg;
        float targetAngle;

        if (character.IsRight())
        {
            swingAngle = isWeaponLowered ? -swingAngle : swingAngle;
            targetAngle = weaponSocket.transform.eulerAngles.z - swingAngle;
        }
        else
        {
            swingAngle = isWeaponLowered ? -swingAngle : swingAngle;
            targetAngle = weaponSocket.transform.eulerAngles.z + swingAngle;
        }

        offsetAngle += 180f;
        offsetAngle %= 360f;

        while (true)
        {
            float angle = Mathf.LerpAngle(weaponSocket.transform.eulerAngles.z, targetAngle, Time.deltaTime * swingSpeed);
            weaponSocket.transform.rotation = Quaternion.Euler(0, 0, angle);

            float delta = Mathf.DeltaAngle(angle, targetAngle);
            if (Mathf.Abs(delta) < 0.1f)
            {
                weaponSocket.transform.rotation = Quaternion.Euler(0, 0, targetAngle);
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
        GameManager.Instance.GameMode.playerController.SetInputLock(false);

        weaponRotateCoroutine = null;
    }
}
