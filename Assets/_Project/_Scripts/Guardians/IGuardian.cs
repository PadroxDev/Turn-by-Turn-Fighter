using UnityEngine;

namespace Padrox
{
    public interface IGuardian
    {
        void Init();
        void EnablePassiveEffects();
        void Perform(Transform target);
        void Die();
        void DisablePassiveEffects();
    }
}