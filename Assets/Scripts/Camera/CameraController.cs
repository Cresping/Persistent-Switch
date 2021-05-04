using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cameras
{
    /// <summary>
    /// Clase encargada de controlar la cámara en el nivel
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private GameObject lookAt;
        [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private float minPositionX;
        [SerializeField] private float maxPositionX;

        private void LateUpdate()
        {
            MoveCamera();
        }
        /// <summary>
        /// Mueve la cámara hacia el objeto que esta mirando con una cierta velocidad, para que tenga un movimiento suave
        /// </summary>
        private void MoveCamera()
        {
            Vector3 desiredPosition = new Vector3(
            Mathf.Clamp(lookAt.transform.position.x, minPositionX, maxPositionX),
            transform.position.y,
            transform.position.z);
            Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);
            transform.position = smoothPosition;
        }
        /// <summary>
        /// Dada una posición en X, mueve la cámara hacia dicha posición. Este módulo se debe de ejecutar al incializar la escena
        /// </summary>
        /// <param name="positionX">Posición donde se debe de mover la cámara </param>
        public void PrepareCamera(float positionX)
        {
            transform.position = new Vector3(positionX, transform.position.y,transform.position.z);
        }
    }
}

