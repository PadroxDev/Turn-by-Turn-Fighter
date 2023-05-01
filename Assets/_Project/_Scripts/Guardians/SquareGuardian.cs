using UnityEngine;

namespace Padrox
{
    public class SquareGuardian : BaseGuardian
    {
        [SerializeField, Range(100, 2000)] private int attackPhcFactor = 500;

        public override void Init()
        {
            base.Init();
        }
        public override void EnablePassiveEffects()
        {

        }

        public override void Perform()
        {
            // GetRandomTarget
            BaseFoe[] foes = GameManager.Instance.Foes;
            if (foes.Length == 0) return;
            BaseFoe target = foes[Random.Range(0, foes.Length)];

            Damage damageInstance = new Damage();
            damageInstance.amount = (int)(attackPhcFactor / 100 * stats.Power);
            damageInstance.type = DamageType.Physical;

            target.stats.ReceiveDamage(damageInstance);
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