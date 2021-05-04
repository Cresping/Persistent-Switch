using Player;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;
using Utils;

namespace UI
{
    /// <summary>
    /// Clase encargada de controlar todo lo que ocurra en el menú inicial
    /// </summary>
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private InputController inputController;
        [SerializeField] private string levelSceneName = "Level";
        [SerializeField] private Button lucasButton;
        [SerializeField] private Button ghastButton;
        [SerializeField] private Button kimButton;
        [SerializeField] private GameObject lucasPortrait;
        [SerializeField] private GameObject ghastPortrait;
        [SerializeField] private GameObject kimPortrait;
        [SerializeField] private GameObject exitButton;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI choseYourCharacterText;
        [SerializeField] private GameObject characterSelector;
        [SerializeField] private GameObject tutorialScreen;
        [SerializeField] private TextMeshProUGUI tutorialText;
        [SerializeField] private float fadeInTutorialTime = 5f;
        [SerializeField] private List<GameObject> listScares;
        private SceneManagement _sceneManagement;
        private GameManager _gameManager;
        private SoundController _soundController;
        private bool _isMinimised;
        private bool _isScareEnabled;
        private PlayerCharacters.CharacterNames _selectedCharacter;
        private void Awake()
        {
            _isMinimised = false;
            _isScareEnabled = false;
            for (int i = 0; i < listScares.Count; i++)
            {
                listScares[i].SetActive(false);
            }
            _sceneManagement = FindObjectOfType<SceneManagement>();
            _gameManager = FindObjectOfType<GameManager>();
            _soundController = FindObjectOfType<SoundController>();
            tutorialScreen.SetActive(false);
        }
        private void Start()
        {
            CheckCharacters();
        }
        private void Update()
        {
            if (!_isMinimised && _isScareEnabled)
            {
                if (!_gameManager.FileManager.CheckScareExists() && _gameManager.FileManager.CheckGameDataExists())
                {
                    DoScare();
                    _isScareEnabled = false;
                }
            }
            if (tutorialScreen.activeSelf && inputController.JumpButton)
            {
                _sceneManagement.LoadScene(levelSceneName, _selectedCharacter);
            }
        }
        private void OnApplicationFocus(bool focus)
        {
            _isMinimised = !focus;
        }
        #region Buttons
        /// <summary>
        /// Cierra el juego
        /// </summary>
        public void ExitGame()
        {
            _gameManager.ExitGame();
        }
        /// <summary>
        /// Carga al personaje 'Lucas' y pasa al tutorial
        /// </summary>
        public void LoadCharacterLucas()
        {
            DoFadeInTutorial();
            _selectedCharacter = PlayerCharacters.CharacterNames.Lucas;
        }
        /// <summary>
        /// Carga al personaje 'Ghast' y pasa al tutorial
        /// </summary>
        public void LoadCharacterGhast()
        {
            DoFadeInTutorial();
            _selectedCharacter = PlayerCharacters.CharacterNames.Ghast;
        }
        /// <summary>
        /// Carga al personaje 'Kim' y pasa al tutorial
        /// </summary>
        public void LoadCharacterKim()
        {
            DoFadeInTutorial();
            _selectedCharacter = PlayerCharacters.CharacterNames.Kim;
        }
        /// <summary>
        /// Si el raton pasa por encima de un botón, ejecuta el sonido asignado
        /// </summary>
        public void PlaySoundOnPointerEnterButton()
        {
            _soundController.PlaySound(SoundController.SoundName.ButtonMouseEnter);
        }
        /// <summary>
        /// Si el raton selecciona el botón, ejecuta el sonido asignado
        /// </summary>
        public void PlaySoundOnClickButton()
        {
            _soundController.PlaySound(SoundController.SoundName.ButtonMouseClick);
        }
        #endregion
        #region MenuChanges
        /// <summary>
        /// Comprueba que personajes esten disponibles, si el personaje ha muerto, se desactiva su boton y se pone en rojo su foto. Si el personaje ha escapado
        /// se quita su foto
        /// </summary>
        private void CheckCharacters()
        {
            PlayerCharacters.CharacterNames lastCharacterAlive = PlayerCharacters.CharacterNames.Lucas;
            PlayerCharacters.CharacterNames firstCharacterDead = PlayerCharacters.CharacterNames.Lucas;
            PlayerCharacters.CharacterNames lastCharacterWon = PlayerCharacters.CharacterNames.Lucas;
            for (int i = 0; i < _gameManager.GetGameData.CharacterData.Count; i++)
            {
                if (_gameManager.GetGameData.CharacterData[i].IsDead)
                {
                    DisableCharacterButton(_gameManager.GetGameData.CharacterData[i].CharacterName);
                    firstCharacterDead = _gameManager.GetGameData.CharacterData[i].CharacterName;
                }
                else if (!_gameManager.GetGameData.CharacterData[i].HasWon)
                {
                    lastCharacterAlive = _gameManager.GetGameData.CharacterData[i].CharacterName;
                }
                if (_gameManager.GetGameData.CharacterData[i].HasWon)
                {
                    DisableCharacterPortrait(_gameManager.GetGameData.CharacterData[i].CharacterName);
                    lastCharacterWon = _gameManager.GetGameData.CharacterData[i].CharacterName;
                }
            }
            ChangeMenu(lastCharacterAlive, firstCharacterDead, lastCharacterWon);
        }
        /// <summary>
        /// Módulo encargado de cambiar el menú dependiendo de lo que haya pasado en la partida anterior
        /// </summary>
        /// <param name="lastCharacterAlive"> Último personaje que esta vivo</param>
        /// <param name="firstCharacterDead"> Primero personaje que ha muerto</param>
        /// <param name="lastCharacterWon">Último personaje que ha ganado</param>
        private void ChangeMenu(PlayerCharacters.CharacterNames lastCharacterAlive, PlayerCharacters.CharacterNames firstCharacterDead, PlayerCharacters.CharacterNames lastCharacterWon)
        {
            switch (_gameManager.GetGameData.NumberOfCharactersWon())
            {
                case 0:
                    switch (_gameManager.GetGameData.NumberOfCharactersDead())
                    {
                        case 1:
                            titleText.text = firstCharacterDead.ToString() + "IsDead";
                            break;
                        case 2:
                            choseYourCharacterText.text = "Choose " + lastCharacterAlive.ToString();
                            break;
                        case 3:
                            titleText.text = _gameManager.FileManager.FileNameScare;
                            choseYourCharacterText.text = "Everyone is dead";
                            //El Scare es solo activable si todos los personajes disponibles han muerto
                            _isScareEnabled = true;
                            break;
                    }
                    break;
                case 1:
                    switch (_gameManager.GetGameData.NumberOfCharactersDead())
                    {
                        case 0:
                            titleText.text = lastCharacterWon.ToString() + "HasEscaped";
                            break;
                        case 1:
                            choseYourCharacterText.text = firstCharacterDead.ToString() + " has not escaped";
                            break;
                        case 2:
                            titleText.text = _gameManager.FileManager.FileNameScare;
                            choseYourCharacterText.text = "Everyone is dead except " + lastCharacterWon.ToString();
                            _isScareEnabled = true;
                            break;
                    }
                    break;
                case 2:
                    switch (_gameManager.GetGameData.NumberOfCharactersDead())
                    {
                        case 0:
                            choseYourCharacterText.text = "Choose " + lastCharacterAlive.ToString();
                            break;
                        case 1:
                            titleText.text = _gameManager.FileManager.FileNameScare;
                            choseYourCharacterText.text = firstCharacterDead.ToString() + " is alone and dead";
                            _isScareEnabled = true;
                            break;
                    }
                    break;
                case 3:
                    choseYourCharacterText.text = "Everyone has escaped";
                    break;
            }

        }
        /// <summary>
        /// Desactiva el botón del personaje dado por parametros
        /// </summary>
        /// <param name="character">Personaje</param>
        private void DisableCharacterButton(PlayerCharacters.CharacterNames character)
        {
            switch (character)
            {
                case PlayerCharacters.CharacterNames.Lucas:
                    lucasButton.interactable = false;
                    lucasButton.gameObject.GetComponent<EventTrigger>().enabled = false;
                    break;
                case PlayerCharacters.CharacterNames.Ghast:
                    ghastButton.interactable = false;
                    ghastButton.gameObject.GetComponent<EventTrigger>().enabled = false;
                    break;
                case PlayerCharacters.CharacterNames.Kim:
                    kimButton.interactable = false;
                    kimButton.gameObject.GetComponent<EventTrigger>().enabled = false;
                    break;
            }
        }
        /// <summary>
        /// Desactiva la foto del personaje dado por parametros
        /// </summary>
        /// <param name="character">Personaje</param>
        private void DisableCharacterPortrait(PlayerCharacters.CharacterNames character)
        {
            switch (character)
            {
                case PlayerCharacters.CharacterNames.Lucas:
                    lucasPortrait.SetActive(false);
                    break;
                case PlayerCharacters.CharacterNames.Ghast:
                    ghastPortrait.SetActive(false);
                    break;
                case PlayerCharacters.CharacterNames.Kim:
                    kimPortrait.SetActive(false);
                    break;
            }
        }
        #endregion
        #region Scare
        /// <summary>
        /// Ejecuta el Scare
        /// </summary>
        [ContextMenu("Scare")]
        private void DoScare()
        {
            IEnumerator coroutineDoWriteText = CoroutineDoWriteTextScare("YOU HAVE DESTROYED OUR", 1f);
            IEnumerator coroutineViolinSound = CoroutineViolinSound();
            titleText.text = "";
            exitButton.SetActive(false);
            choseYourCharacterText.gameObject.SetActive(false);
            for (int i = 0; i < _gameManager.GetGameData.CharacterData.Count; i++)
            {
                DisableCharacterButton(_gameManager.GetGameData.CharacterData[i].CharacterName);
            }
            StartCoroutine(coroutineViolinSound);
            StartCoroutine(coroutineDoWriteText);
        }
        /// <summary>
        /// Corrutina que cierra el juego cuando el sonido del violín termina
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoroutineViolinSound()
        {
            yield return new WaitForSeconds(_soundController.PlaySound(SoundController.SoundName.Violin));
            ExitGame();
        }
        /// <summary>
        /// Corrutina encargada de escribir un texto determinado del Scare en un tiempo dado, letra por letra
        /// </summary>
        /// <param name="text">Texto que se tiene que escribir</param>
        /// <param name="timePerLetter">Tiempo por letra</param>
        /// <returns></returns>
        private IEnumerator CoroutineDoWriteTextScare(string text, float timePerLetter)
        {
            foreach (char letter in text.ToCharArray())
            {
                titleText.text += letter;
                if (!char.IsWhiteSpace(letter))
                {
                    _soundController.PlaySound(SoundController.SoundName.Heartbeat);
                    yield return new WaitForSeconds(timePerLetter);
                }
            }
            _soundController.PlaySound(SoundController.SoundName.Scare);
            titleText.text += " GRAVES";
            for (int i = 0; i < listScares.Count; i++)
            {
                listScares[i].SetActive(true);
            }
            DoShakePortraits();
        }
        /// <summary>
        /// Hace que las fotos de los personajes se agiten haciendo uso de DOTween
        /// </summary>
        private void DoShakePortraits()
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(characterSelector.transform.DOShakePosition(10, 5, 10, 90, false, false));
            seq.OnComplete(DoShakePortraits);
            seq.Play();
        }
        #endregion
        #region Tutorial
        /// <summary>
        /// Pasa al panel del tútorial con un Fade In
        /// </summary>
        private void DoFadeInTutorial()
        {
            tutorialScreen.SetActive(true);
            tutorialText.GetComponent<Image>().DOFade(1, fadeInTutorialTime);
            StartCoroutine(FadeTextToFullAlpha(fadeInTutorialTime, tutorialText));
        }
        /// <summary>
        /// Corrutina que cambia el alpha del texto a 1 en un tiempo dado 
        /// </summary>
        /// <param name="fadeInTime">Tiempo que tardará en cambiarse</param>
        /// <param name="texto">Texto que se cambiará</param>
        /// <returns></returns>
        public IEnumerator FadeTextToFullAlpha(float fadeInTime, TextMeshProUGUI texto)
        {
            texto.color = new Color(texto.color.r, texto.color.g, texto.color.b, 0);
            while (texto.color.a < 1.0f)
            {
                texto.color = new Color(texto.color.r, texto.color.g, texto.color.b, texto.color.a + (Time.deltaTime / fadeInTime));
                yield return null;
            }
        }
        #endregion
    }
}
