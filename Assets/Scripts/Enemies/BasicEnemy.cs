using Data.Globals;
using Environment.Triggers;
using Shared;
using Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Basic enemy that can be damaged and pushed.
    /// Moves towards the player and damages them on collision.
    /// </summary>
    public class BasicEnemy : DamageableWithIFrames, IPushable
    {
        [SerializeField]
        private Rigidbody body;

        [SerializeField]
        private DetectionRadius detectionRadius;

        private bool loaded = false;

        [SerializeField, Tooltip("The room this enemy is in.")]
        private RoomTransition roomTransition;

        private Coroutine hitstun;

        private GameObject player;

        public void Push(Vector3 direction, float force)
        {
            body.velocity = Vector3.zero;
            Debug.Log("Pushed");
            body.AddForce(direction * force);

            if (hitstun != null)
            {
                StopCoroutine(hitstun);
            }
            hitstun = StartCoroutine(Hitstun());
        }

        // Start is called before the first frame update
        new void Start()
        {
            base.Start();

            if (detectionRadius != null)
            {
                detectionRadius.OnPlayerEnter += seePlayer;
                detectionRadius.OnPlayerExit += losePlayer;
            }

            if (roomTransition == null)
            {
                // This doesn't seem to reliably find the RoomTransition from the same scene
                roomTransition = GameObject.FindObjectOfType<RoomTransition>();
            }

            roomTransition.OnRoomEnter += Activate;
            roomTransition.OnRoomExit += Deactivate;
            OnDeath += Die;
        }

        public void FixedUpdate()
        {
            if (loaded && !stopAccelerating && player != null)
            {
                Vector3 direction = (player.transform.position - transform.position);
                direction.y = 0;
                direction.Normalize();

                body.velocity += direction * acceleration * Time.deltaTime;

                if (body.velocity.magnitude > maxSpeed)
                {
                    //Debug.Log("Clamping");
                    body.velocity = direction * maxSpeed;
                }

            }
        }

        private void Activate(GameObject _)
        {
            loaded = true;
        }

        private void Deactivate()
        {
            loaded = false;
        }

        private void Die()
        {
            roomTransition.OnRoomEnter -= Activate;
            roomTransition.OnRoomExit -= Deactivate;

            if (detectionRadius != null)
            {
                detectionRadius.OnPlayerEnter -= seePlayer;
                detectionRadius.OnPlayerExit -= losePlayer;
            }

            Destroy(gameObject);
        }

        [SerializeField]
        private float maxSpeed = 3f;

        [SerializeField]
        private float acceleration = 2f;

        private bool stopAccelerating = false;

        public void seePlayer(GameObject player)
        {
            this.player = player;
        }

        public void losePlayer()
        {
            this.player = null;
        }

        /// <summary>
        /// When pushed into a wall, take damage
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            if (stopAccelerating && !collision.collider.CompareTag(StaticTagStrings.PLAYER))
            {
                DoDamage(Mathf.RoundToInt(collision.relativeVelocity.magnitude));
            }
        }

        private IEnumerator Hitstun()
        {
            Debug.Log("Hitstun");
            stopAccelerating = true;

            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitUntil(() => body.velocity.magnitude < 0.3f);

            Debug.Log("Hitstun over");

            stopAccelerating = false;
        }
    }
}