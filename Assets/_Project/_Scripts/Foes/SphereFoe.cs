using UnityEngine;

namespace Padrox
{
    public class SphereFoe : BaseFoe
    {
        public override void Init()
        {
            base.Init();
            Damage _initDamage = new Damage();
            _initDamage.amount = 200;
            _initDamage.type = DamageType.Physical;
            stats.ReceiveDamage(_initDamage);
        }

        public override void EnablePassiveEffects()
        {
        }

        public override void Perform(Transform target)
        {
        }

        public override void DisablePassiveEffects()
        {
        }

        public override void Die()
        {
            base.Die();
        }
    }
}