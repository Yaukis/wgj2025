using UnityEngine;

namespace Utils
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                _instance = FindAnyObjectByType<T>();
                
                if (_instance != null) return _instance;
                var go = new GameObject
                {
                    name = typeof(T).Name
                };
                _instance = go.AddComponent<T>();

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}