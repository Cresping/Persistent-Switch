using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Clase encargada de controlar las colisiones Trigger del jugador, más concretamente, el suelo y las plataformas moviles
    /// </summary>
    public class PlayerTriggerControler : MonoBehaviour
    {
        [SerializeField] private string groundTag;
        [SerializeField] private string movingPlataformTag;
        private bool _isGround;
        private bool _isMovingPlataform;
        private GameObject _objectTriggered;
        private void Awake()
        {
            _isGround = false;
            _objectTriggered = null;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(groundTag))
            {
                _isGround = true;
            }
            if (collision.CompareTag(movingPlataformTag))
            {
                _isMovingPlataform = true;

            }
            _objectTriggered = collision.gameObject;
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(groundTag))
            {
                _isGround = false;
            }
            if (collision.CompareTag(movingPlataformTag))
            {
                _isMovingPlataform = false;

            }
            _objectTriggered = null;
        }
        public bool IsGround
        {
            get
            {
                return _isGround;
            }
        }
        public bool IsMovingPlataform
        {
            get
            {
                return _isMovingPlataform;
            }
        }
    }
}

