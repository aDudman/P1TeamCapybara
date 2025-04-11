using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared
{
    public interface IDamageable
    {
        public void DoDamage(int amount);
    }
}
