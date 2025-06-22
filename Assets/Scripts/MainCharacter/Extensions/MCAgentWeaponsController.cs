using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace MainCharacter.Extensions
{
    public class MCAgentWeaponsController : MonoBehaviour
    {
        [SerializeField]
        private GameObject SwordPrefab;

        [SerializeField]
        private GameObject HammerPrefab;

        private InputAction meleeAction;

        // Start is called before the first frame update
        void Start()
        {
            meleeAction = InputSystem.actions.FindAction("Melee");
            meleeAction.performed += MeleeAttack;
        }

        void MeleeAttack(InputAction.CallbackContext context)
        {
            if (context.interaction is SlowTapInteraction)
            {
                Instantiate(HammerPrefab, transform.position, transform.rotation);
            }
            else if (context.interaction is PressInteraction)
            {
                Instantiate(SwordPrefab, transform.position, transform.rotation);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}