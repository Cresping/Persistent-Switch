using Cameras;
using Enemy;
using Persistence;
using Player;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Clase encargada de cargar y controlar el juego
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private FileManager fileManager;
        [SerializeField] private GameObject playerGrave;
        private CameraController _cameraPlayer;
        private PlayerController _player;
        private EnemyController[] _arrayEnemies;
        private GameData _gameData;
        private bool _isFirstTime;
        private void Awake()
        {
            if (!fileManager.CheckGameDataExists())
            {
                _gameData = new GameData();
                _isFirstTime = true;
            }
            else
            {
                _isFirstTime = false;
                LoadGameSaveData();
            }
        }
        /// <summary>
        /// Sale del juego
        /// </summary>
        public void ExitGame()
        {
            Application.Quit();
        }
        #region LoadGame
        /// <summary>
        /// Carga la partida con los datos almacenados y el personaje pasado por parametros
        /// </summary>
        /// <param name="characterNameSelected">Personaje seleccionado</param>
        public void LoadData(PlayerCharacters.CharacterNames characterNameSelected)
        {
            LoadCharacters(characterNameSelected);
            LoadEnemies();
            LoadGraves();
            SaveGameSaveData();
            _isFirstTime = false;
        }
        /// <summary>
        /// Carga el personaje pasado por parametros en la partida
        /// </summary>
        /// <param name="characterNameSelected">Personaje seleccionado</param>
        public void LoadCharacters(PlayerCharacters.CharacterNames characterNameSelected)
        {
            _player = FindObjectOfType<PlayerController>();
            _cameraPlayer = FindObjectOfType<CameraController>();
            if (_isFirstTime)
            {
                //Si es la primera vez que se juega, crea los datos
                foreach (PlayerCharacters.CharacterNames characterName in System.Enum.GetValues(typeof(PlayerCharacters.CharacterNames)))
                {
                    CharacterData aux = new CharacterData();
                    aux.CurrentPosition = _player.transform.position;
                    aux.CharacterName = characterName;
                    _gameData.CharacterData.Add(aux);
                }
            }
            //Si no es la primera vez, prepara los jugadores con los datos guardados
            for (int i = 0; i < _gameData.CharacterData.Count; i++)
            {
                if (_gameData.CharacterData[i].CharacterName == characterNameSelected)
                {

                    _player.PreparePlayer(_gameData.CharacterData[i]);

                    if (_gameData.CharacterData[i].CurrentPosition.x > 1)
                    {
                        _cameraPlayer.PrepareCamera(_gameData.CharacterData[i].CurrentPosition.x);
                    }

                }
            }
        }
        /// <summary>
        /// Carga los enemigos
        /// </summary>
        public void LoadEnemies()
        {
            _arrayEnemies = FindObjectsOfType<EnemyController>();
            if (_isFirstTime)
            {
                for (int i = 0; i < _arrayEnemies.Length; i++)
                {
                    _gameData.EnemyData.Add(new EnemyData());
                    _arrayEnemies[i].PrepareEnemy(_gameData.EnemyData[i]);
                }
            }
            else
            {
                for (int i = 0; i < _gameData.EnemyData.Count; i++)
                {

                    _arrayEnemies[i].PrepareEnemy(_gameData.EnemyData[i]);
                }
            }
        }
        /// <summary>
        /// Carga las tumbas de los personajes
        /// </summary>
        public void LoadGraves()
        {
            if (!_isFirstTime)
            {
                for (int i = 0; i < _gameData.CharacterData.Count; i++)
                {
                    if (_gameData.CharacterData[i].IsDead)
                    {
                        Instantiate(playerGrave, _gameData.CharacterData[i].GravePosition, Quaternion.identity);
                    }
                }
            }
        }
        #endregion
        #region Load&SaveData
        /// <summary>
        /// Carga los datos almacenados del juego
        /// </summary>
        public void LoadGameSaveData()
        {
            _gameData = fileManager.DeSereializeGameData();
        }
        /// <summary>
        /// Guarda los datos almacenados del juego
        /// </summary>
        [ContextMenu("SaveFile")]
        public void SaveGameSaveData()
        {
            fileManager.SerializeGameData(_gameData);
        }
        #endregion
        #region Properties
        public GameData GetGameData
        {
            get
            {
                return _gameData;
            }
        }
        public FileManager FileManager
        {
            get
            {
                return fileManager;
            }
        }
        #endregion
    }
}

