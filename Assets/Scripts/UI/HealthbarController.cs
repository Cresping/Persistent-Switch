using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;
using Persistence;

namespace UI
{
    /// <summary>
    /// Clase encargada de controlar la barra de vida del juego
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public class HealthbarController : MonoBehaviour
    {
        [SerializeField] private Image fillDisplay;
        [SerializeField] private Slider healthbarDisplay;
        [SerializeField] private Image portrait;
        private PlayerController _player;
        private int _actualHealth;
        private void Awake()
        {
            if (healthbarDisplay == null)
            {
                healthbarDisplay = GetComponent<Slider>();
            }
            _player = FindObjectOfType<PlayerController>();
        }
        /// <summary>
        /// Módulo encargado de restar tanta vida a la barra como la indicada por el parametro
        /// </summary>
        /// <param name="amount">Cantidad de vida que se restará</param>
        public void TakeHealth(int amount)
        {
            _actualHealth -= amount;
            if (_actualHealth <= 0)
            {
                fillDisplay.enabled = false;
            }
            if (_actualHealth > healthbarDisplay.maxValue)
            {
                _actualHealth = (int) healthbarDisplay.maxValue;
            }
            healthbarDisplay.value = _actualHealth;
        }
        /// <summary>
        /// Módulo encargado de preparar la barra de vida y la foto del personaje
        /// </summary>
        /// <param name="playerData">Datos del jugador</param>
        public void PrepareHealthBar(CharacterData playerData)
        {
            Sprite changePortrait = Resources.Load<Sprite>("Portraits/" + playerData.CharacterName.ToString());
            portrait.sprite = changePortrait;
            healthbarDisplay.minValue = 0;
            healthbarDisplay.maxValue = playerData.MaximumHealthPoints;
            healthbarDisplay.value = playerData.CurrentHealthPoints;
            _actualHealth = playerData.CurrentHealthPoints;

        }
    }
}

