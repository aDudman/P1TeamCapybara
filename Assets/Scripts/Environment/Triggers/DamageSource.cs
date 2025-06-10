using Data.Globals;
using Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Triggers
{
    public class DamageSource : MonoBehaviour
    {
        [SerializeField, Tooltip("The amount of damage dealt")]
        private int damage;

        [SerializeField, Tooltip("True if this can damage the player")]
        protected bool canHitPlayer;

        [SerializeField, Tooltip("True if this can damage enemies")]
        protected bool canHitEnemies;

        [SerializeField, Tooltip("True if this source continues doing damage to a target which stays in it.")]
        private bool isContinuous;

        [SerializeField, Tooltip("Leave blank to assign automatically")]
        private Collider hitBox;

        private void Start()
        {
            if (hitBox == null)
            {
                hitBox = GetComponent<Collider>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isContinuous) { return; }
            HandleDamage(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (!isContinuous) { return; }
            HandleDamage(other);
        }

        /// <summary>
        /// Modifies the alignment of this damage source, allowing it to hit either the player, enemies, or both.
        /// </summary>
        /// <param name="player">True if it can damage the player</param>
        /// <param name="enemies">True if it can damage enemies</param>
        public void ModifyAlignment(bool player, bool enemies)
        {
            canHitPlayer = player;
            canHitEnemies = enemies;
        }

        private void HandleDamage(Collider other)
        {
            if (other.CompareTag(StaticTagStrings.PLAYER) && canHitPlayer)
            {
                var damageable = other.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.DoDamage(damage);
                }
            }
            else if (other.CompareTag(StaticTagStrings.ENEMY) && canHitEnemies)
            {
                var damageable = other.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.DoDamage(damage);
                }
            }
        }
    }
}
