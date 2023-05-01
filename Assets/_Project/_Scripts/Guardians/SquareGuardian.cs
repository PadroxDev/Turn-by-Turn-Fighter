using UnityEngine;

namespace Padrox
{
    public class SquareGuardian : BaseGuardian
    {
        public override void Init()
        {
            base.Init();
            print("Physical Damage: " + stats.ActionSpeed);
            Damage _initDamage = new Damage();
            _initDamage.amount = 100;
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