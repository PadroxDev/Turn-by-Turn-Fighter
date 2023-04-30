using System.Collections;
using UnityEngine;

namespace Padrox
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float _startDelay = 5f;

        public Transform _guardianSlotsHolder;
        public Transform _foeSlotsHolder;

        private Transform[] _guardianSlots;
        private Transform[] _foeSlots;

        private void Awake()
        {
            _guardianSlots = _guardianSlotsHolder.GetComponentsInChildren<Transform>();
            _foeSlots = _foeSlotsHolder.GetComponentsInChildren<Transform>();
        }

        void Start() {
            StartCoroutine(BeginFight());
        }

        private IEnumerator BeginFight()
        {
            yield return new WaitForSeconds(_startDelay);
            print("Start");
        }

        private void SummonGuardians()
        {

        }
    }
}