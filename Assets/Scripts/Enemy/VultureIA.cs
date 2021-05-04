using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Clase encargada de controlar el moviento del enemigo Vulture
    /// </summary>
    public class VultureIA : MonoBehaviour
    {
        [SerializeField] private float speedMovement = 5f;
        [SerializeField] private float magnitudeMovement = 2f;
        [SerializeField] private float frequencyMovement = 20f;
        private EnemyController _enemyController;
        private Rigidbody2D _myRb;

        private void Awake()
        {
            _enemyController = GetComponentInChildren<EnemyController>();
            _myRb = GetComponentInChildren<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            if (_enemyController.IsActive)
            {
                TryMove();
            }
        }
        /// <summary>
        /// Vulture se mueve en forma de seno, con su determinada frecuencia y magnitud. Al mismo tiempo se actualiza su posición en X para su desplazamiento
        /// </summary>
        private void TryMove()
        {
            Vector2 pos = transform.position -transform.right *speedMovement* Time.deltaTime;
            transform.position = pos + (Vector2) transform.up * Mathf.Sin(Time.time * frequencyMovement) * magnitudeMovement;
        }

    }
}

