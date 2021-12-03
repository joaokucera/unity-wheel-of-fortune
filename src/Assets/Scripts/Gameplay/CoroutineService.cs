using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

namespace Test
{
    public interface ICoroutineService
    {
        void StartCoroutine(int identifier, IEnumerator coroutine);
        void StopCoroutine(int identifier);
        IPromise<long> ExecutePromise(IPromise<long> playerBalance);
        IPromise<int> ExecutePromise(IPromise<int> playerBalance);
    }

    /// <summary>
    /// Simplified service to handle coroutines (which could be implemented without calling StartCoroutine but Update method).
    /// </summary>
    public class CoroutineService : ICoroutineService
    {
        private readonly MonoBehaviour _monoBehaviour;
        private readonly IDictionary<int, Coroutine> _coroutinesByIdentifier;

        public CoroutineService(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
            _coroutinesByIdentifier = new Dictionary<int, Coroutine>();
        }

        public void StartCoroutine(int identifier, IEnumerator coroutine)
        {
            StopCoroutine(identifier);
            _coroutinesByIdentifier[identifier] = _monoBehaviour.StartCoroutine(coroutine);
        }

        public void StopCoroutine(int identifier)
        {
            if (!_coroutinesByIdentifier.TryGetValue(identifier, out Coroutine coroutine))
            {
                return;
            }
            
            _monoBehaviour.StopCoroutine(coroutine);
            _coroutinesByIdentifier.Remove(identifier);
        }

        public IPromise<long> ExecutePromise(IPromise<long> playerBalance)
        {
            _monoBehaviour.StartCoroutine(RunPromise(playerBalance));
            return playerBalance;
        }

        private IEnumerator RunPromise(IPromise<long> promise)
        {
            yield return promise;
        }

        public IPromise<int> ExecutePromise(IPromise<int> playerBalance)
        {
            _monoBehaviour.StartCoroutine(RunPromise(playerBalance));
            return playerBalance;
        }

        private IEnumerator RunPromise(IPromise<int> promise)
        {
            yield return promise;
        }
    }
}