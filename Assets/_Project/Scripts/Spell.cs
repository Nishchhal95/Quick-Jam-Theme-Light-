using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject spellBreakEffect;

    public void SetVelocity(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            return;
        }
        GameObject spellBreak = Instantiate(spellBreakEffect, transform.position, Quaternion.identity);
        Destroy(spellBreak, 1f);
        Destroy(gameObject);
    }
}
