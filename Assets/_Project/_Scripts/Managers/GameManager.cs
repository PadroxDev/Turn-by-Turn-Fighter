using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Padrox.Helpers;

namespace Padrox {
    public class GameManager : Singleton<GameManager> {
        [SerializeField] private float _startDelay = 5f;

        public GameObject GuardianPlaceholder;
        public GameObject FoePlaceholder;
        [SerializeField] private Transform _guardiansParent;
        [SerializeField] private Transform _foesParent;

        public Transform _guardianSlotsHolder;
        public Transform _foeSlotsHolder;

        private Transform[] _guardianSlots;
        private Transform[] _foeSlots;

        private List<BaseGuardian> _summonedGuardians;
        private List<BaseFoe> _summonedFoes;
        private bool _fighting = true;

        // Getters
        public BaseGuardian[] Guardians => _summonedGuardians.ToArray();
        public BaseFoe[] Foes => _summonedFoes.ToArray();

        protected override void Awake() {
            base.Awake();
            _guardianSlots = _guardianSlotsHolder.GetComponentsInChildren<Transform>().Where(slot => slot != _guardianSlotsHolder).ToArray();
            _foeSlots = _foeSlotsHolder.GetComponentsInChildren<Transform>().Where(slot => slot != _foeSlotsHolder).ToArray();

            _summonedGuardians = new List<BaseGuardian>();
            _summonedFoes = new List<BaseFoe>();
        }

        void Start() {
            StartCoroutine(BeginFight());
        }

        private void IncreaseOverallElapsedSpeed(float actionSpeed) {
            foreach (BaseGuardian guardian in _summonedGuardians) {
                guardian.stats.ElapsedSpeed += actionSpeed;
            }

            foreach (BaseFoe foe in _summonedFoes) {
                foe.stats.ElapsedSpeed += actionSpeed;
            }
        }

        private IEnumerator BeginFight() {
            // Wait before starting the fight !
            yield return new WaitForSeconds(_startDelay);

            // Spawn the guardians
            SummonGuardians();

            // Spawn the foes
            for (int i = 0; i < 1; i++) {
                SummonFoes(i);
            }

            // Activate passive effects of every guardians
            EnableGuardiansPassiveEffects();

            // Activate passive effects of every foes
            EnableFoesPassiveEffects();

            int j = 0;
            while (_fighting) {
                j++;
                if (j == 10)
                    _fighting = false;

                // Get the fastest guardian
                float performerAS = Mathf.Infinity;

                List<BaseGuardian> guardianPerformers = new List<BaseGuardian>();
                foreach (BaseGuardian guardian in _summonedGuardians) {
                    float guardianAS = guardian.stats.ActionSpeed - guardian.stats.ElapsedSpeed;
                    if (guardianPerformers.Count.Equals(0)) {
                        guardianPerformers.Add(guardian);
                        performerAS = guardianAS;
                    } else if (guardianAS == performerAS) {
                        guardianPerformers.Add(guardian);
                    } else if (guardianAS < performerAS) {
                        guardianPerformers.Clear();
                        guardianPerformers.Add(guardian);
                        performerAS = guardianAS;
                    }
                }

                List<BaseFoe> foePerformers = new List<BaseFoe>();
                foreach (BaseFoe foe in _summonedFoes) {
                    float foeAS = foe.stats.ActionSpeed - foe.stats.ElapsedSpeed;
                    if (foeAS == performerAS) {
                        foePerformers.Add(foe);
                    } else if (foeAS < performerAS) {
                        guardianPerformers.Clear();
                        foePerformers.Clear();
                        foePerformers.Add(foe);
                        performerAS = foeAS;
                    }
                }

                BaseGuardian guardianPerformer = null;
                BaseFoe foePerformer = null;
                if (!guardianPerformers.Count.Equals(0) && !foePerformers.Count.Equals(0)) {
                    int n = Random.Range(0, guardianPerformers.Count + foePerformers.Count);
                    if (n < guardianPerformers.Count) {
                        guardianPerformer = guardianPerformers[n];
                    } else {
                        foePerformer = foePerformers[n - guardianPerformers.Count];
                    }
                } else if (!guardianPerformers.Count.Equals(0)) {
                    int n = Random.Range(0, guardianPerformers.Count);
                    guardianPerformer = guardianPerformers[n];
                } else if (!foePerformers.Count.Equals(0)) {
                    int n = Random.Range(0, foePerformers.Count);
                    foePerformer = foePerformers[n];
                }
                if (guardianPerformer) {
                    guardianPerformer.Perform();
                    IncreaseOverallElapsedSpeed(guardianPerformer.stats.ActionSpeed);
                    guardianPerformer.stats.ElapsedSpeed = 0;
                } else if (foePerformer) {
                    foePerformer.Perform();
                    IncreaseOverallElapsedSpeed(foePerformer.stats.ActionSpeed);
                    foePerformer.stats.ElapsedSpeed = 0;
                }

                //yield return new WaitForSeconds(2);
            }

            _summonedGuardians[0].DisablePassiveEffects();
            _summonedFoes[0].DisablePassiveEffects();
        }

        private void SummonGuardians() {
            Vector3 pos = _guardianSlots[0].position;
            Quaternion rot = _guardianSlots[0].rotation;
            var guardian = Instantiate(GuardianPlaceholder, pos, rot, _guardiansParent);

            if (guardian == null) {
                Debug.LogError("Encountered an error while instantiating the guardian!");
            }

            BaseGuardian i_guardian = guardian.GetComponent<BaseGuardian>();
            i_guardian.Init();

            _summonedGuardians.Add(i_guardian);
        }

        private void SummonFoes(int i) {
            Vector3 pos = _foeSlots[i].position;
            Quaternion rot = _foeSlots[i].rotation;
            var foe = Instantiate(FoePlaceholder, pos, rot, _foesParent);

            if (foe == null) {
                Debug.Log("Encountered an error while instantiating the foe!");
            }

            BaseFoe i_foe = foe.GetComponent<BaseFoe>();
            i_foe.Init();

            _summonedFoes.Add(i_foe);
        }

        private void EnableGuardiansPassiveEffects() {
            foreach (BaseGuardian guardian in _summonedGuardians) {
                guardian.EnablePassiveEffects();
            }
        }

        private void EnableFoesPassiveEffects() {
            foreach (BaseFoe foe in _summonedFoes) {
                foe.EnablePassiveEffects();
            }
        }

        private void TriggerFoesPerform() {
            foreach (BaseFoe foe in _summonedFoes) {
                foe.Perform();
            }
        }
    }
}