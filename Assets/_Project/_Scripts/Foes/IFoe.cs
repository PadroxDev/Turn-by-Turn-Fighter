using UnityEngine;

namespace Padrox
{
    public interface IFoe
    {
        void Die();
        void DisablePassiveEffects();
        void EnablePassiveEffects();
        void Init();
        void Perform(Transform target);
    }
}