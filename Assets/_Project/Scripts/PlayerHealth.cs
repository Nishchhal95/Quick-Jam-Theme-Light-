using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100;
    public Image healthBarFillImage;
    public bool isDead;
    public int spiritsCollected = 0;

    [SerializeField] private GameObject gameOverScreen, gameFinishedScreen;
    [SerializeField] private TextMeshProUGUI spiritsCountText;

    [SerializeField] private Behaviour originalFogEffect, finishFogImageEffect;
    [SerializeField] private GameObject finishParticleEffect;

    private void Start()
    {
        health = 100;
        healthBarFillImage.fillAmount = health / 100;
    }

    private void OnEnable()
    {
        SpiritBehaviour.onSpiritDie += SpiritCollected;
    }

    private void OnDisable()
    {
        SpiritBehaviour.onSpiritDie -= SpiritCollected;
    }

    private void SpiritCollected()
    {
        spiritsCollected++;
        spiritsCountText.SetText("Spirits Collected : " + spiritsCollected + " / 3");

        if (spiritsCollected >= 3)
        {
            GameFinished();
        }
    }

    private void GameFinished()
    {
        gameFinishedScreen.SetActive(true);
        originalFogEffect.enabled = false;
        finishFogImageEffect.enabled = true;
        finishParticleEffect.SetActive(true);
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerAttackController>().enabled = false;
        GetComponent<PlayerInput>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void TakeDamage(float amount)
    {
        if (isDead)
        {
            return;
        }
        health -= amount;
        healthBarFillImage.fillAmount = health / 100;
        if (health <= 0)
        {
            isDead = true;
            PlayDieEffect();
        }
    }

    private void PlayDieEffect()
    {
        gameOverScreen.SetActive(true);
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerAttackController>().enabled = false;
        GetComponent<PlayerInput>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
