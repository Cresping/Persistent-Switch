using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player;
namespace Utils
{
    /// <summary>
    /// Clase encargada de controlar la carga de escenas
    /// </summary>
    public class SceneManagement : MonoBehaviour
    {
        [SerializeField] private string levelSceneName = "Level";
        private GameManager _gameManager;
        private PlayerCharacters.CharacterNames _playerSelected;
        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
        /// <summary>
        /// Se ejecuta cuando la escena ha sido cargada
        /// </summary>
        /// <param name="scene">Escena cargada</param>
        /// <param name="mode">Modo de carga de la escena</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (SceneManager.GetActiveScene().name.Equals(levelSceneName))
            {
                _gameManager.LoadData(_playerSelected);
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        /// <summary>
        /// Carga una escena con un nombre y un personaje determinado
        /// </summary>
        /// <param name="nameScene">Nombre de la escena</param>
        /// <param name="playerSelected">Jugador seleccionado</param>
        public void LoadScene(string nameScene, PlayerCharacters.CharacterNames playerSelected)
        {
            _playerSelected = playerSelected;
            SceneManager.LoadScene(nameScene, LoadSceneMode.Single);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        /// <summary>
        /// Carga una escena con un nombre determinado
        /// </summary>
        /// <param name="nameScene">Nombre de la escena </param>
        public void LoadScene(string nameScene)
        {
            SceneManager.LoadScene(nameScene, LoadSceneMode.Single);
        }
    }
}


