using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Map
{
    /// <summary>
    /// Clase encargada de controlar la lógica de movimiento de las plataformas
    /// </summary>
    public class PlataformerController : MonoBehaviour
    {
        /// <summary>
        /// Tipo de movimiento de la plataforma
        /// </summary>
        private enum PlataformMovement
        {
            Vertical, Horizontal
        }
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private PlataformMovement plataformMovement = PlataformMovement.Horizontal;
        [SerializeField] private float speedMovement = 2f;
        [SerializeField] private float maxDistance = 2f;
        [SerializeField] private int direction = 1;
        private float _actualDistance;
        private PlayerController _player;

        private void Awake()
        {
            _actualDistance = 0;
            _player = FindObjectOfType<PlayerController>();
        }
        private void Update()
        {
            if (!_player.PlayerData.IsDead)
            {
                MovePlataform();
            }
        }
        /// <summary>
        /// La plataforma se moverá hacia arriba o hacia abajo dependiendo del tipo de plataforma que sea, con una velocidad y dirección determinadas.
        /// Al llegar a la máxima distancia de desplazamiento, cambia la dirección
        /// </summary>
        private void MovePlataform()
        {
            Vector2 newPosition;
            switch (plataformMovement)
            {
                case PlataformMovement.Horizontal:
                    newPosition = (Vector2)transform.position + Vector2.right * direction * speedMovement * Time.deltaTime;
                    _actualDistance += Vector2.Distance(transform.position, newPosition);
                    transform.position = newPosition;
                    break;
                case PlataformMovement.Vertical:
                    newPosition = (Vector2)transform.position + Vector2.up * direction * speedMovement * Time.deltaTime;
                    _actualDistance += Vector2.Distance(transform.position, newPosition);
                    transform.position = newPosition;
                    break;
            }
            if (_actualDistance >= maxDistance)
            {
                direction *= -1;
                _actualDistance = 0;
            }
        }
        /// <summary>
        /// Cuando la plataforma colisiona con el jugador, cambia la posición jerarquica del jugador para que este se pueda desplzar con la plataforma
        /// </summary>
        /// <param name="collision">Jugador </param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(playerTag))
            {
                if (_player.isActiveAndEnabled)
                {
                    _player.transform.SetParent(transform);
                }

            }
        }
        /// <summary>
        /// Cuando sale de la colisión el jugador, restablece la posición jerarquica del jugador
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(playerTag))
            {
                if (_player.isActiveAndEnabled)
                {
                    _player.transform.SetParent(null);
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 direction = transform.TransformDirection(Vector3.right) * maxDistance;
            switch (plataformMovement)
            {
                case PlataformMovement.Horizontal:
                    direction = Vector3.right * maxDistance;
                    break;
                case PlataformMovement.Vertical:
                    direction = Vector3.up * maxDistance;
                    break;
            }
            Gizmos.DrawRay(transform.position, direction * this.direction);
        }
    }

}
