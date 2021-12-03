using System;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public interface IServiceLocator
    {
        T GetService<T>();
    }
    
    public class ServiceLocator : IServiceLocator
    {
        private readonly IDictionary<object, object> _services;

        public ServiceLocator(MonoBehaviour monoBehaviour)
        {
            _services = new Dictionary<object, object>
            {
                {typeof(IDailyBonusService), new DailyBonusService()},
                {typeof(ICoroutineService), new CoroutineService(monoBehaviour)}
            };
        }

        public T GetService<T>()
        {
            try
            {
                return (T)_services[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                throw new ApplicationException("The requested service is not registered");
            }
        }
    }
}