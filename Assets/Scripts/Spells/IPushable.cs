

using UnityEngine;

namespace Spells
{
    public interface IPushable
    {
        void Push(Vector3 direction, float force);
    }
}