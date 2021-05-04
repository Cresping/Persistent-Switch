using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using Player;

namespace Persistence
{
    /// <summary>
    /// Clase encargada de almacenar la información de los personajes
    /// </summary>
    [Serializable]
    public class CharacterData
    {
        private int _currentHealthPoints;
        private int _maximumHealthPoints;
        private float _characterPositionX;
        private float _characterPositionY;
        private bool _isDead;
        private PlayerCharacters.CharacterNames _characterName;
        //Posición donde está la tumba del personaje
        private float _gravePositionX;
        private float _gravePositionY;
        private bool _hasWon;

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public CharacterData()
        {
            _currentHealthPoints = 100;
            _maximumHealthPoints = 100;
            _characterPositionX = 0;
            _characterPositionY = 0;
            _gravePositionX = 0;
            _gravePositionY = 0;
            _isDead = false;
            _hasWon = false;
            _characterName = PlayerCharacters.CharacterNames.Lucas;
        }
        #region Properties
        public int CurrentHealthPoints
        {
            get
            {
                return _currentHealthPoints;
            }
            set
            {
                _currentHealthPoints = value;
            }
        }
        public Vector2 CurrentPosition
        {
            get
            {
                return new Vector2(_characterPositionX, _characterPositionY);
            }
            set
            {
                _characterPositionX = value.x;
                _characterPositionY = value.y;
            }
        }
        public Vector2 GravePosition
        {
            get
            {
                return new Vector2(_gravePositionX, _gravePositionY);
            }
            set
            {
                _gravePositionX = value.x;
                _gravePositionY = value.y;
            }
        }
        public int MaximumHealthPoints
        {
            get
            {
                return _maximumHealthPoints;
            }
            set
            {
                _maximumHealthPoints = value;
            }
        }
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
        public PlayerCharacters.CharacterNames CharacterName
        {
            get
            {
                return _characterName;
            }
            set
            {
                _characterName = value;
            }
        }
        public bool HasWon
        {
            get
            {
                return _hasWon;
            }
            set
            {
                _hasWon = value;
            }
        }
        #endregion


    }
}

