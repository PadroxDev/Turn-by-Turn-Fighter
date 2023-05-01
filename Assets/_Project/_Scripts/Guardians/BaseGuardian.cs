using UnityEngine;

namespace Padrox
{
    [RequireComponent(typeof(Stats))]
    public abstract class BaseGuardian : MonoBehaviour
    {
        public Stats stats;

        public virtual void Init()
        {
            stats = GetComponent<Stats>();
            stats.OnDeath += Die;
        }
        public abstract void EnablePassiveEffects();
        public abstract void Perform(Transform target);
        public abstract void DisablePassiveEffects();
        public virtual void Die()
        {
            Destroy(gameObject);
        }
    }
}