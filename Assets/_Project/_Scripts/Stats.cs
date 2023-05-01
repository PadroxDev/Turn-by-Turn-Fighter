using System;
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

        // Getters
        public float ActionSpeed => _actionSpeed;
        public float Power => _power;
        public float Armor => _armor;
        public float MagicCloak => _magicCloak;

        // Events
        public Action OnDeath;
        
        private int _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        private void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        public int GetMitigatedDamage(int dmg, int resistance)
        {
            float mitigatedRate = 0f;
            if (resistance >= 0)
            {
                mitigatedRate = 100 / (100 + (float)resistance);
            } else
            {
                mitigatedRate = 2 - 100 / (100 - (float)resistance);
            }

            int damage = Mathf.RoundToInt(dmg * mitigatedRate);
            return damage;
        }

        public int ReceiveDamage(Damage damage)
        {
            int mitigatedDamage = 0;
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

            int finalDamage = Mathf.Min(_currentHealth, mitigatedDamage);
            TakeDamage(finalDamage);

            return finalDamage;
        }

        private void Die()
        {
            OnDeath?.Invoke();
        }
    }

    public struct Damage
    {
        public DamageType type;
        public int amount;
    }
}
