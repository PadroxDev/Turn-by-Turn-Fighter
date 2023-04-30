using UnityEngine;

namespace Padrox
{
    public enum DamageType
    {
        Physical,
        Magical,
        True
    }

    public class Stats : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private float _actionSpeed = 10;
        [SerializeField] private DamageType _type = DamageType.Physical;
        [SerializeField] private int _power = 5; // Uses factor
        [SerializeField] private int _armor = 30;
        [SerializeField] private int _magicCloak = 30;

        private int _currentHealth;

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
        }

        public int GetMitigatedDamage(int dmg, int resistance)
        {
            float mitigatedRate = 0f;
            if (resistance >= 0)
            {
                mitigatedRate = 100 / (100 + resistance);
            } else
            {
                mitigatedRate = 2 - 100 / (100 - resistance);
            }

            int damage = Mathf.RoundToInt(dmg * mitigatedRate);
            return damage;
        }

        public int ReceiveDamage(Damage damage)
        {
            int mitigatedDamage = GetMitigatedDamage(damage.amount, _armor);
            switch (damage.type)
            {
                case DamageType.Physical:
                    mitigatedDamage = GetMitigatedDamage(damage.amount, _armor);
                    break;
                case DamageType.Magical:
                    mitigatedDamage = GetMitigatedDamage(damage.amount, _magicCloak);
                    break;
                case DamageType.True:
                    mitigatedDamage = damage.amount;
                    break;
            }

            int damageTaken = Mathf.Max(_currentHealth, mitigatedDamage);
            TakeDamage(damageTaken);

            return damageTaken;
        }
    }

    public struct Damage
    {
        public DamageType type;
        public int amount;
    }
}
