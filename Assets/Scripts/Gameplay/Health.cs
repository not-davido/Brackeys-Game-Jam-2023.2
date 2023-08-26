using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Tooltip("Maximum amount of health")] public float MaxHealth = 10f;

    [Tooltip("Health ratio at which the critical health vignette starts appearing")]
    public float CriticalHealthRatio = 0.3f;

    public float InvincibleDuration = 0.8f;

    public UnityAction<float, GameObject, Transform> OnDamaged;
    public UnityAction<float> OnHealed;
    public UnityAction OnDie;

    public float CurrentHealth { get; set; }
    public bool Invincible { get; set; }
    public bool CanPickup() => CurrentHealth < MaxHealth;

    public float GetRatio() => CurrentHealth / MaxHealth;
    public bool IsCritical() => GetRatio() <= CriticalHealthRatio;

    bool m_IsDead;
    float invincibleTimer;

    void Awake() {
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (Invincible) {
            if (Time.time > invincibleTimer + InvincibleDuration) {
                Invincible = false;
            }
        }
    }

    public bool Heal(float healAmount) {
        if (CurrentHealth == MaxHealth) return false;

        float healthBefore = CurrentHealth;
        CurrentHealth += healAmount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        // call OnHeal action
        float trueHealAmount = CurrentHealth - healthBefore;
        if (trueHealAmount > 0f) {
            OnHealed?.Invoke(trueHealAmount);
        }

        return true;
    }

    public bool TakeDamage(float damage, GameObject damageSource, Transform positionAfterHit = null) {
        if (Invincible)
            return false;

        float healthBefore = CurrentHealth;
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        Invincible = true;
        invincibleTimer = Time.time;
        // call OnDamage action
        float trueDamageAmount = healthBefore - CurrentHealth;
        if (trueDamageAmount > 0f) {
            OnDamaged?.Invoke(trueDamageAmount, damageSource, positionAfterHit);
        }

        HandleDeath();

        return true;
    }

    public void Kill() {
        CurrentHealth = 0f;

        // call OnDamage action
        OnDamaged?.Invoke(MaxHealth, null, null);

        HandleDeath();
    }

    void HandleDeath() {
        if (m_IsDead)
            return;

        // call OnDie action
        if (CurrentHealth <= 0f) {
            m_IsDead = true;
            OnDie?.Invoke();
        }
    }
}
