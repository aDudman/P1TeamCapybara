using Environment.Triggers;
using Spells;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class BallAndChain_Ball : MonoBehaviour, IPushable
    {
        private bool isPushable = false;

        [SerializeField, Tooltip("This object's Rigidbody")]
        private Rigidbody body;

        [SerializeField, Tooltip("The desired position when controlled by the boss. Ignored when pushed by the player.")]
        private GameObject target;

        [SerializeField, Tooltip("The main body of the boss.")]
        private GameObject boss;

        [SerializeField, Tooltip("The speed at which the ball moves towards the target when controlled by the boss.")]
        private float speed;

        [SerializeField, Tooltip("The GameObject which holds the damage information.")]
        private DamageSource damage;

        [SerializeField, Tooltip("Strength of aim correction, between 0-1")]
        private float nudgeRatio;

        [SerializeField, Tooltip("The maximum amount of nudge, between 0-2")]
        private float nudgeCap;

        public void Push(Vector3 direction, float force)
        {
            if (isPushable)
            {
                var finalDirection = direction;

                if (boss != null)
                {
                    var bossDirection = (boss.transform.position - transform.position).normalized;
                    var aimNudge = (bossDirection - direction) * nudgeRatio;

                    if (aimNudge.magnitude < nudgeCap)
                        finalDirection = (direction + aimNudge).normalized;

                    Debug.Log(direction);
                    Debug.Log(bossDirection);
                    Debug.Log(aimNudge);
                    Debug.Log(finalDirection);
                }

                body.AddForce((finalDirection * force));
            }
        }

        public void setPushState(bool state)
        {
            isPushable = state;
            if (damage != null)
            {
                damage.ModifyAlignment(!state, state);
            }
        }

        public void enablePush() => setPushState(true);
        public void disablePush() => setPushState(false);

        // Use this for initialization
        void Start()
        {
            if (body == null)
            {
                body = GetComponent<Rigidbody>();
            }
            if (speed <= 0)
            {
                speed = 5f; // Default speed if not set
            }
        }

        private void OnDrawGizmos()
        {
            Vector3 offset = target.transform.position - transform.position;
            float distance = offset.magnitude;

            Vector3 destination = target.transform.position;

            if (distance > speed)
            {
                // Move towards the target at the specified speed
                destination = offset * (speed / distance) + transform.position;
            }

            Gizmos.DrawLine(transform.position, destination);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!isPushable)
            {
                if (target != null)
                {
                    Vector3 offset = target.transform.position - transform.position;
                    float distance = offset.magnitude;

                    Vector3 destination = target.transform.position;
                    float frameSpeed = Time.fixedDeltaTime * speed;

                    if (distance > frameSpeed)
                    {
                        // Move towards the target at the specified speed
                        destination = offset * (frameSpeed / distance) + transform.position;
                    }

                    body.MovePosition(destination);
                }
            }
            else
            {
                // If the ball is pushable, it will be controlled by the player or other forces.
                // No additional movement logic needed here.
            }

        }
    }
}