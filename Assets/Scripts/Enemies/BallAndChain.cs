using Environment.Triggers;
using Shared;
using Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class BallAndChain : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private int health;

        [SerializeField]
        private float moveSpeed = 2f;

        [SerializeField]
        private GameObject ballTarget;

        [SerializeField]
        private BallAndChain_Ball ball;

        [SerializeField]
        private float ballSpeed = 5f;

        [SerializeField]
        private float ballDistance = 3f;

        [SerializeField]
        private float attackIntervalMin = 3f;

        [SerializeField]
        private float attackIntervalMax = 5f;

        [SerializeField]
        private float attackRangeMax = 5f;

        private float angle;
        private bool isAttacking = false;

        [SerializeField]
        private RoomTransition room;

        private GameObject player;

        public event System.Action OnDeath;

        private bool loaded = false;

        // Start is called before the first frame update
        void Start()
        {
            OnDeath += Die;
            StartCoroutine(AttackRoutine());

            if (room != null)
            {
                room.OnRoomEnter += Activate;
                room.OnRoomExit += Deactivate;
            }
        }

        public void DoDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                OnDeath?.Invoke();
            }
        }

        private void Activate(GameObject player)
        {
            this.player = player;
            loaded = true;
        }

        private void Deactivate() {
            this.player = null;
            loaded = false;
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void FixedUpdate()
        {
            if (!isAttacking && loaded)
            {
                RevolveBall();
                Move();
            }
        }

        private void Move()
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;

            float distance = direction.magnitude;
            float movement = moveSpeed * Time.fixedDeltaTime;
            float delta = distance - ballDistance;

            direction.Normalize();

            transform.Rotate(Vector3.up, Vector3.SignedAngle(transform.forward, direction, Vector3.up));

            if (Mathf.Abs(delta) < movement)
            {
                return;
            }
            else if (delta > 0)
            {
                transform.position += direction * movement;
            }
            else
            {
                transform.position -= direction * movement;
            }
        }

        private void RevolveBall()
        {
            angle += ballSpeed * Time.fixedDeltaTime;
            float x = Mathf.Cos(angle) * ballDistance;
            float z = Mathf.Sin(angle) * ballDistance;
            ballTarget.transform.position = transform.position + new Vector3(x, 0, z);
        }

        private IEnumerator AttackRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(attackIntervalMin, attackIntervalMax));
                // Only attack if the player is in the room
                if (player != null)
                    yield return StartCoroutine(PerformAttack());
                yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator PerformAttack()
        {
            Vector3 playerRelativePosition = transform.InverseTransformPoint(player.transform.position);
            if (playerRelativePosition.magnitude > attackRangeMax)
            {
                yield break;
            }

            isAttacking = true;

            // Move the ball behind the enemy
            Vector3 behindPosition = new Vector3(0, 0, -ballDistance * 0.5f);
            yield return MoveBallToPosition(behindPosition, 0.3f);

            // Swing the ball overhead
            Vector3 overheadPosition = new Vector3(0, ballDistance, 0);
            yield return MoveBallToPosition(overheadPosition, 0.3f);

            // Stop the ball on the ground in front
            Vector3 frontPosition = new Vector3(playerRelativePosition.x, 0, playerRelativePosition.z);
            yield return MoveBallToPosition(frontPosition, 0.4f);

            // After the attack, allow the player time to push the ball
            ball.enablePush();

            yield return new WaitForSeconds(3f);

            // Disable pushing after the attack
            ball.disablePush();

            // Set the angle to the current world angle of the ball
            Vector3 ballOffset = ballTarget.transform.position - transform.position;
            angle = Mathf.Atan2(ballOffset.z, ballOffset.x);

            isAttacking = false;
        }

        private IEnumerator MoveBallToPosition(Vector3 targetPosition, float duration)
        {
            Vector3 startPosition = ballTarget.transform.localPosition;
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                ballTarget.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            ballTarget.transform.localPosition = targetPosition;
        }
    }
}
