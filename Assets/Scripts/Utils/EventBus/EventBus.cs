using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.EventBus
{
    
    public static class EventBus<T> where T : IEvent
    {
        static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();
        
        public static void AddListener(EventBinding<T> binding) => bindings.Add(binding);
        public static void RemoveListener(EventBinding<T> binding) => bindings.Remove(binding);
        
        public static void Raise(T @event)
        {
            foreach (var binding in bindings.ToList())
            {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }

        static void Clear()
        {
            Debug.Log($"Clearing {typeof(T).Name} bindings");
            bindings.Clear();
        }
    }
}