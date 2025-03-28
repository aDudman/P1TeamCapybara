using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared
{
    public abstract class DamageableWithIFrames : MonoBehaviour, IDamageable
    {
        [SerializeField, Tooltip("Time in seconds that entity will be invulnerable after taking damage")]
        protected float invulnTimer;

        protected bool isInvuln;

        [SerializeField]
        protected int maxHealth = 100;

        [SerializeField, Tooltip("Set to -1 to start with max health")]
        protected int currentHealth = -1;

        public delegate void HealthChange(int change, int remaining);
        public event HealthChange OnHealthChanged;

        public event Action OnDeath;

        /// <summary>
        /// Gets a value indicating whether the entity is invulnerable.
        /// </summary>
        public bool IsInvuln => isInvuln;

        /// <summary>
        /// Gets the current health of the entity.
        /// </summary>
        public int CurrentHealth => currentHealth;

        /// <summary>
        /// Gets the maximum health of the entity.
        /// </summary>
        public int MaxHealth => maxHealth;

        /// <summary>
        /// Applies damage to the entity.
        /// </summary>
        /// <param name="amount">The amount of damage to apply.</param>
        public void DoDamage(int amount)
        {
            if (amount < 0)
            {
                Debug.LogWarning("Negative damage not allowed");
                return;
            }

            if (isInvuln) { return; }

            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath?.Invoke();
            }

            OnHealthChanged?.Invoke(amount, currentHealth);

            StartCoroutine(StartInvulnTimer(invulnTimer));
        }

        protected IEnumerator StartInvulnTimer(float duration)
        {
            if (duration < 0)
            {
                Debug.LogWarning("Invulnerability timer cannot be negative");
                yield break;
            }

            isInvuln = true;
            yield return new WaitForSeconds(duration);
            isInvuln = false;
        }

        // Start is called before the first frame update
        protected void Start()
        {
            if (currentHealth < 0)
            {
                currentHealth = maxHealth;
            }

            OnHealthChanged += (int change, int remaining) => Debug.Log($"change: {change}, remaining: {remaining}");
        }

        protected void OnDestroy()
        {
            OnHealthChanged = null;
            OnDeath = null;
        }
    }
}
