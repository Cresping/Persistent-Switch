using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Persistence
{
    /// <summary>
    /// Clase encargada de almacenar los datos de los enemigos
    /// </summary>
    [Serializable]
    public class EnemyData
    {
        private float _positionX;
        private float _positionY;
        private bool _isDead;

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public EnemyData()
        {
            _positionX = 0;
            _positionY = 0;
            _isDead = false;
        }
        #region Properties
        public bool IsDead
        {
            get
            {
                return _isDead;
            }
            set
            {
                _isDead = value;
            }
        }
        public Vector2 CurrentPosition
        {
            get
            {
                return new Vector2(_positionX, _positionY);
            }
            set
            {
                _positionX = value.x;
                _positionY = value.y;
            }
        }
        #endregion
    }

}
