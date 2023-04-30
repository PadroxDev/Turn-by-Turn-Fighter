using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Padrox
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float _startDelay = 5f;

        public GameObject GuardianPlaceholder;
        [SerializeField] private Transform _guardiansParent;
        [SerializeField] private Transform _foesParent;

        public Transform _guardianSlotsHolder;
        public Transform _foeSlotsHolder;

        private Transform[] _guardianSlots;
        private Transform[] _foeSlots;

        private List<IGuardian> _summonedGuardians;
        private bool _fighting = true;

        private void Awake()
        {
            _guardianSlots = _guardianSlotsHolder.GetComponentsInChildren<Transform>();
            _foeSlots = _foeSlotsHolder.GetComponentsInChildren<Transform>();

            _summonedGuardians = new List<IGuardian>();
        }

        void Start() {
            StartCoroutine(BeginFight());
        }

        private IEnumerator BeginFight()
        {
            // Wait before starting the fight !
            yield return new WaitForSeconds(_startDelay);

            // Spawn the guardians
            SummonGuardians();

            // Activate passive effects of every guardians
            EnableGuardiansPassiveEffects();

            int i = 0;
            while (_fighting)
            {
                i++;
                TriggerGuardiansPerform();
                if (i > 3)
                    _fighting = false;
            }

            _summonedGuardians[0].DisablePassiveEffects();
            _summonedGuardians[0].Die();
        }

        private void SummonGuardians()
        {
            Vector3 pos = _guardianSlots[0].position;
            Quaternion rot = _guardianSlots[0].rotation;
            var guardian = Instantiate(GuardianPlaceholder, pos, rot, _guardiansParent);
            
            if (guardian == null)
            {
                Debug.LogError("Encountered an error while instantiating the guardian!");
            }

            IGuardian i_guardian = guardian.GetComponent<IGuardian>();
            i_guardian.Init();

            _summonedGuardians.Add(i_guardian);
        }

        private void EnableGuardiansPassiveEffects()
        {
            foreach (IGuardian guardian in _summonedGuardians)
            {
                guardian.EnablePassiveEffects();
            }
        }

        private void TriggerGuardiansPerform()
        {
            foreach (IGuardian guardian in _summonedGuardians)
            {
                guardian.Perform(null);
            }
        }
    }
}