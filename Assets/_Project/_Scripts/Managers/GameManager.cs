using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Padrox.Helpers;

namespace Padrox
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private float _startDelay = 5f;

        public GameObject GuardianPlaceholder;
        public GameObject FoePlaceholder;
        [SerializeField] private Transform _guardiansParent;
        [SerializeField] private Transform _foesParent;

        public Transform _guardianSlotsHolder;
        public Transform _foeSlotsHolder;

        public Transform[] _guardianSlots;
        private Transform[] _foeSlots;

        private List<BaseGuardian> _summonedGuardians;
        private List<BaseFoe> _summonedFoes;
        private bool _fighting = true;

        // Getters
        public BaseGuardian[] Guardians => _summonedGuardians.ToArray();
        public BaseFoe[] Foes => _summonedFoes.ToArray();

        protected override void Awake()
        {
            base.Awake();
            _guardianSlots = _guardianSlotsHolder.GetComponentsInChildren<Transform>().Where(slot => slot != _guardianSlotsHolder).ToArray();
            _foeSlots = _foeSlotsHolder.GetComponentsInChildren<Transform>().Where(slot => slot != _foeSlotsHolder).ToArray();

            _summonedGuardians = new List<BaseGuardian>();
            _summonedFoes = new List<BaseFoe>();
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

            // Spawn the foes
            for (int i = 0; i < 2; i++)
            {
                SummonFoes(i);
            }

            // Activate passive effects of every guardians
            EnableGuardiansPassiveEffects();

            // Activate passive effects of every foes
            EnableFoesPassiveEffects();

            while (_fighting)
            {
                TriggerGuardiansPerform();
                TriggerGuardiansPerform();
                TriggerFoesPerform();
                _fighting = false;
            }

            _summonedGuardians[0].DisablePassiveEffects();
            _summonedFoes[0].DisablePassiveEffects();
            _summonedFoes[1].DisablePassiveEffects();
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

            BaseGuardian i_guardian = guardian.GetComponent<BaseGuardian>();
            i_guardian.Init();

            _summonedGuardians.Add(i_guardian);
        }

        private void SummonFoes(int i)
        {
            Vector3 pos = _foeSlots[i].position;
            Quaternion rot = _foeSlots[i].rotation;
            var foe = Instantiate(FoePlaceholder, pos, rot, _foesParent);

            if (foe == null)
            {
                Debug.Log("Encountered an error while instantiating the foe!");
            }

            BaseFoe i_foe = foe.GetComponent<BaseFoe>();
            i_foe.Init();

            _summonedFoes.Add(i_foe);
        }

        private void EnableGuardiansPassiveEffects()
        {
            foreach (BaseGuardian guardian in _summonedGuardians)
            {
                guardian.EnablePassiveEffects();
            }
        }

        private void EnableFoesPassiveEffects()
        {
            foreach (BaseFoe foe in _summonedFoes)
            {
                foe.EnablePassiveEffects();
            }
        }

        private void TriggerGuardiansPerform()
        {
            foreach (BaseGuardian guardian in _summonedGuardians)
            {
                guardian.Perform();
            }
        }

        private void TriggerFoesPerform()
        {
            foreach (BaseFoe foe in _summonedFoes)
            {
                foe.Perform();
            }
        }
    }
}