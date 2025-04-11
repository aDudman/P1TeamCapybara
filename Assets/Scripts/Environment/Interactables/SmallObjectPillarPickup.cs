using System.Collections;
using System.Collections.Generic;
using Environment.Interactables;
using UnityEngine;

namespace Environment.Interactables
{
    public class SmallObjectPillarPickup : SmallObjectPickupable
    {
        [SerializeField, Tooltip("Pillar object that is connected")]
        private BrokenPillar pillar;

        public override void PickupObject(GameObject agent)
        {
            base.PickupObject(agent);
            pillar?.EnablePushing();
            pillar = null;

        }
    }
}

