using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Clase encargada de controlar el moviento del enemigo Scorpio
    /// </summary>
    public class ScorpioIA : MonoBehaviour
    {
        [SerializeField] private string groundTag = "Ground";
        [SerializeField] private float speedMovement = 5f;
        [SerializeField] private int direction= 1;
        [SerializeField] private float distanceRayCast = 1;
        [SerializeField] private LayerMask layerMask;
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
        /// El enemigo se moverá hacia la dirección actual, hasta que no detecte tierra donde se pueda mover o detecte tierra justo delante de el, en ambos caso
        /// se da la vuelta y continua moviendose
        /// </summary>
        private void TryMove()
        {
            if (!CheckGround((Vector2.right * direction) - Vector2.up) || CheckGround(Vector2.right*direction))
            {
                direction = -direction;
                transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
            }
            transform.position += transform.right * direction * speedMovement * Time.deltaTime;
        }
        /// <summary>
        /// Dada una dirección, tira un RayCast para comprobar si hay tierra
        /// </summary>
        /// <param name="direction"> Dirección donde tirará el RayCast </param>
        /// <returns>True si es tierra, false si no </returns>
        private bool CheckGround(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distanceRayCast, layerMask);
            if (hit.collider!=null)
            {
                return hit.collider.CompareTag(groundTag);
            }
            else
            {
                return false;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay((Vector2)transform.position, ((Vector2.right * this.direction) - Vector2.up) * distanceRayCast);
            Gizmos.DrawRay((Vector2)transform.position, Vector2.right*direction * distanceRayCast);
        }
    }

}
