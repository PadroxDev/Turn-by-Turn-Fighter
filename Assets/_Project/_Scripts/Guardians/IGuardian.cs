using UnityEngine;

namespace Padrox
{
    public interface IGuardian
    {
        void Die();
        void DisablePassiveEffects();
        void EnablePassiveEffects();
        void Init();
        void Perform(Transform target);
    }
}