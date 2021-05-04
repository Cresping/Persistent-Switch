using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Clase encargada de controlar colisiones Trigger
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class TriggerCheck : MonoBehaviour
    {
        [SerializeField] private string triggerTag;
        private bool _isTriggered;
        private GameObject _objectTriggered;
        private void Awake()
        {
            _isTriggered = false;
            _objectTriggered = null;
        }
        private void OnDisable()
        {
            _isTriggered = false;
            _objectTriggered = null;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(triggerTag))
            {
                _isTriggered = true;
                _objectTriggered = collision.gameObject;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(triggerTag))
            {
                _isTriggered = false;
                _objectTriggered = null;
            }
        }
        /// <summary>
        /// Devuelve si ha chocado con algo
        /// </summary>
        /// <returns>True si ha chocado, False si no</returns>
        public bool GetIsTriggered()
        {
            return _isTriggered;
        }
        /// <summary>
        /// Devuelve el objeto con el que ha chocado
        /// </summary>
        /// <returns></returns>
        public GameObject GetObjectTriggered()
        {
            return _objectTriggered;
        }
    }
}

