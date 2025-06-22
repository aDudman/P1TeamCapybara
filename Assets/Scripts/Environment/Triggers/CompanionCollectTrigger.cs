using Data.Globals;
using MainCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Triggers
{
    public class CompanionCollectTrigger : MonoBehaviour
    {
        [SerializeField, Tooltip("The spell that will be collected when the player enters the trigger.")]
        private GameObject pushSpellPrefab;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == StaticTagStrings.PLAYER)
            {
                MCAgent player = other.GetComponent<MCAgent>();
                if (player != null)
                {
                    player.AddSpell(pushSpellPrefab);
                    Destroy(transform.parent.gameObject); // Destroy the trigger after collecting the spell. May change later.
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
