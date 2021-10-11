using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class SpiritBehaviour : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private bool isDead;
    [SerializeField] private bool restedInPeace;
    [SerializeField] private Transform targetShrine;
    [SerializeField] private Material targetShrineMat;
    [SerializeField] private Light targetShrineLight;
    [SerializeField] private Color targetColor;

    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private bool makeAttack;
    [SerializeField] private float currentGapBetweenAttacks;
    [SerializeField] private float MaxGapBetweenAttacks = 1;
    [SerializeField] private EnemySpell spellPrefab;
    [SerializeField] private float shootRange = 10f;

    public static Action onSpiritDie;

    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource _as;

    private void Start()
    {
        health = 100;
        healthBarFillImage.fillAmount = health / 100;
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void Update()
    {
        if (isDead || playerHealth.isDead)
        {
            return;
        }
        if (Vector3.Distance(playerHealth.transform.position, transform.position) <= shootRange)
        {
            makeAttack = true;
        }

        HandleAttack();
    }
    
    private void HandleAttack()
    {
        currentGapBetweenAttacks -= Time.deltaTime;
        currentGapBetweenAttacks = Mathf.Clamp(currentGapBetweenAttacks, 0, MaxGapBetweenAttacks);
        if (!makeAttack || !CanPerformAttack())
        {
            return;
        }

        ResetAttackDelays();
        PerformAttack();
    }

    private void PerformAttack()
    {
        makeAttack = false;
        Vector3 spellDirection = (playerHealth.transform.transform.position - transform.position).normalized;
        EnemySpell spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
        spell.SetVelocity(spellDirection * 15f);
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

    public void TakeDamage(float damageAmount)
    {
        if (isDead)
        {
            return;
        }
        health -= damageAmount;
        healthBarFillImage.fillAmount = health / 100;
        if (health <= 0)
        {
            isDead = true;
            PlayDieEffect();
        }
    }

    public void PlayDieEffect()
    {
        transform.LeanScale(new Vector3(0.05f, 0.05f, 0.05f), 2f).setOnComplete(() =>
        {
            transform.LeanMove(targetShrine.position, 2f).setOnComplete(() =>
            {
                targetShrineMat = targetShrine.GetComponent<Renderer>().material;
                targetShrineMat.SetColor("_EmissionColor", targetColor);
                targetShrineMat.SetColor("_Color", targetColor);
                targetShrineLight.color = targetColor;
                targetShrineLight.range = 120;
                targetShrineLight.intensity = 2;
                gameObject.SetActive(false);
                int clipIndex = playerHealth.spiritsCollected;
                clipIndex = Mathf.Clamp(clipIndex, 0, clips.Length);
                _as.clip = clips[clipIndex];
                _as.Play();
                onSpiritDie?.Invoke();
            });
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, shootRange);
    }
}
