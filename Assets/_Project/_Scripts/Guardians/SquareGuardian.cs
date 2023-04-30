using UnityEngine;

namespace Padrox
{
    public class SquareGuardian : MonoBehaviour, IGuardian
    {
        public void Init()
        {
            print("Initialize SquareGuardian");
        }
        public void EnablePassiveEffects()
        {
            print("SquareGuardian's passives enabled!");
        }

        public void Perform(Transform target)
        {
            print("SquareGuardian Performing");
        }

        public void Die()
        {
            print("SquareGuardian died!");
        }

        public void DisablePassiveEffects()
        {
            print("SquareGuardian's passives disabled!");
        }
    }
}