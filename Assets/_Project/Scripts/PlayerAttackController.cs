using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float MaxGapBetweenAttacks = 1f;
    [SerializeField] private float currentGapBetweenAttacks = 0f;

    [SerializeField] private Transform spellShootTransform;
    [SerializeField] private Spell spellPrefab;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask hittableLayerMask;
    

    public AudioClip[] spellCastSFX;
    public AudioSource as_;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        HandleAttack();
    }

    private void HandleAttack()
    {
        currentGapBetweenAttacks -= Time.deltaTime;
        currentGapBetweenAttacks = Mathf.Clamp(currentGapBetweenAttacks, 0, MaxGapBetweenAttacks);
        if (!playerInput.GetDidAttack() || !CanPerformAttack())
        {
            return;
        }

        ResetAttackDelays();
        PerformAttack();
    }

    private void PerformAttack()
    {
        Debug.Log("Pew!");
        as_.PlayOneShot(spellCastSFX[UnityEngine.Random.Range(0, spellCastSFX.Length)]);
        Vector3 spellDirection = cameraTransform.forward;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
            out RaycastHit hitInfo, 1000f, hittableLayerMask))
        {
            spellDirection = (hitInfo.point - spellShootTransform.position).normalized;
        }
        Spell spell = Instantiate(spellPrefab, spellShootTransform.position, Quaternion.identity);
        spell.SetVelocity(spellDirection * 15f);
        //spell.GetComponent<Rigidbody>().velocity = spellDirection * 15f;
        Destroy(spell.gameObject, 2f);
    }

    private bool CanPerformAttack()
    {
        return (currentGapBetweenAttacks <= 0);
    }

    private void ResetAttackDelays()
    {
        currentGapBetweenAttacks = MaxGapBetweenAttacks;
    }
}
