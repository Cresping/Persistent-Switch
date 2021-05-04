using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Utils;

namespace UI
{
    /// <summary>
    /// Clase encargada de controlar la pantalla de fin de juego
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class EndGameController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI endGameText;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private Color gameOverColor;
        [SerializeField] private Color winGameColor;
        [SerializeField] private float fadeOutTime=15f;
        private GameManager _gameManager;
        private void Start()
        {
            this.gameObject.SetActive(false);
            _gameManager = FindObjectOfType<GameManager>();
        }
        /// <summary>
        /// Módulo encargado de iniciar la secuencia de GameOver
        /// </summary>
        /// <param name="characterName">Nombre del personaje</param>
        public void StartSequenceGameOver (string characterName)
        {
            this.gameObject.SetActive(true);
            endGameText.text = characterName + " is dead";
            GetComponent<Image>().DOColor(gameOverColor,fadeOutTime);
            musicSource.DOFade(0, fadeOutTime);
        }
        /// <summary>
        /// Módulo encargado de iniciar la secuencia de ganar
        /// </summary>
        /// <param name="characterName">Nombre del personaje</param>
        public void StartSequenceWinGame(string characterName)
        {
            this.gameObject.SetActive(true);
            endGameText.text = characterName + " has escaped";
            GetComponent<Image>().DOColor(winGameColor, fadeOutTime);
            musicSource.DOFade(0, fadeOutTime);
        }
        /// <summary>
        /// Módulo accedido por un boton que cierra el juego
        /// </summary>
        public void ExitGame()
        {
            _gameManager.ExitGame();
        }
        
    }

}
