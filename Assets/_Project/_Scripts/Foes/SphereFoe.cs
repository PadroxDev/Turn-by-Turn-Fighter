using UnityEngine;

namespace Padrox
{
    public class SphereFoe : MonoBehaviour, IFoe
    {
        public void Init()
        {
            Debug.Log("Foe is initiating");
        }

        public void EnablePassiveEffects()
        {
            Debug.Log("Enabling every foe's passive effects");
        }

        public void Perform(Transform target)
        {
            Debug.Log("Foe's performing!");
        }

        public void DisablePassiveEffects()
        {
            Debug.Log("Enabling every foe's passive effects");
        }

        public void Die()
        {
            Debug.Log("Foe's dying");
        }
    }
}