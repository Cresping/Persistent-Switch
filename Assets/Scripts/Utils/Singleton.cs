using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Clase Singleton para asegurar que solo exista un GameManager
    /// </summary>
    public class Singleton : MonoBehaviour
    {
        private static Singleton _instance;

        public static Singleton Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
