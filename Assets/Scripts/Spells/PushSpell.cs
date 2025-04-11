using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spells
{
    public class PushSpell : MonoBehaviour
    {

        [SerializeField, Tooltip("The width of the push effect.")]
        private float width;

        [SerializeField, Tooltip("The length of the push effect.")]
        private float length;

        [SerializeField, Tooltip("The force of the push effect.")]
        private float force;

        [SerializeField, Tooltip("A volume representing the shape of the area to be pushed.")]
        private Collider effectTrigger;

        [SerializeField, Tooltip("The number of seconds that the spell should remain in effect.")]
        private float duration;

        // Start is called before the first frame update
        void Start()
        {
            effectTrigger.transform.localScale = new Vector3(width, 1, length);
            effectTrigger.transform.localPosition += transform.forward * length/2f;

            Destroy(gameObject, duration);
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Applies force to pushable objects within the spell area
        /// </summary>
        /// <param name="other">An object within the area</param>
        private void OnTriggerEnter(Collider other)
        {
            IPushable pushable = other.GetComponent<IPushable>();
            if(pushable != null)
            {
                pushable.Push(transform.forward, force);
            }

        }
    }
}

