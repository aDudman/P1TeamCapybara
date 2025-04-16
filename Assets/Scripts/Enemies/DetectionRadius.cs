using Data.Globals;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class DetectionRadius : MonoBehaviour
    {
        public event Action<GameObject> OnPlayerEnter;
        public event Action OnPlayerExit;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(StaticTagStrings.PLAYER))
            {
                OnPlayerEnter?.Invoke(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(StaticTagStrings.PLAYER))
            {
                    OnPlayerExit?.Invoke();
            }
        }
    }
}
