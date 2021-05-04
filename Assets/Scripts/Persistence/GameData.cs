using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System;

namespace Persistence
{
    /// <summary>
    /// Clase encarga de almacenar la información del juego
    /// </summary>
    [Serializable]
    public class GameData
    {
        //Datos de los personajes
        private List<CharacterData> _characterData;
        //Datos de los enemigos
        private List<EnemyData> _enemyData;

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public GameData()
        {
            _characterData = new List<CharacterData>();
            _enemyData = new List<EnemyData>();
        }
        /// <summary>
        /// Devuelve el número de personajes que han ganado
        /// </summary>
        /// <returns> Número de personajes que han ganado </returns>
        public int NumberOfCharactersWon()
        {
            int cont = 0;
            for (int i = 0; i < _characterData.Count; i++)
            {
                if (_characterData[i].HasWon)
                {
                    cont++;
                }
            }
            return cont;
        }
        /// <summary>
        /// Devuelve el número de personajes que han muerto
        /// </summary>
        /// <returns>Número de personajes que han muerto</returns>
        public int NumberOfCharactersDead()
        {
            int cont = 0;
            for (int i = 0; i < _characterData.Count; i++)
            {
                if (_characterData[i].IsDead)
                {
                    cont++;
                }
            }
            return cont;
        }
        #region Properties
        public List<CharacterData> CharacterData
        {
            get
            {
                return _characterData;
            }
            set
            {
                _characterData = value;
            }
        }
        public List<EnemyData> EnemyData
        {
            get
            {
                return _enemyData;
            }
            set
            {
                _enemyData = value;
            }
        }
        #endregion
    }
}

