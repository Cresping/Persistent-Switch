using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Utils;
using Persistence;

namespace Enemy
{
    /// <summary>
    /// Clase encarga de realizar operaciones que los enemigos en general tienen en común
    /// </summary>
    public class EnemyController : MonoBehaviour
    {
        /// <summary>
        /// Cada nivel representa la dificultad de dicho enemigo, los enemigos de mayor nivel solo se activarán si 
        /// el jugador ha ganado tantas veces como nivel tenga el enemigo
        /// </summary>
        private enum EnemyDifficulty
        {
            Level0, Level1, Level2
        }
        /// <summary>
        /// Es el tipo de enemigo
        /// </summary>
        private enum EnemyType{
            Vulture, Scorpio
        };
        [SerializeField] private string playerTag="Player";
        [SerializeField] private string groundTag = "Ground";
        [SerializeField] private string cameraTag = "MainCamera";
        [SerializeField] private string pitTag = "Pit";
        [SerializeField] private int damageContact=10;
        [SerializeField] private Sprite deadSprite;
        [SerializeField] private EnemyType enemyType = EnemyType.Vulture;
        [Tooltip("La dificultad hará que aparezca o no el enemigo, es decir, si el jugador no ha ganado nunca y el enemigo es de nivel 1, no aparecerá, sin " +
            "embargo, si ha ganado una vez y el enemigo es de nivel 1, aparecerá")]
        [SerializeField] private EnemyDifficulty enemyDifficulty = EnemyDifficulty.Level0;
        private EnemyData _enemyData;
        private Rigidbody2D _myRb;
        private AnimatorController _animatorController;
        private PlayerController _player;
        private SoundController _soundController;
        private GameManager _gameManager;
        private Vector3 _originalPosition;
        private bool _isActive;
        private bool _isDying;
        private bool _isInGround;
        private void Awake()
        {
            _isInGround = false;
            _enemyData = new EnemyData();
            _isActive = false;
            _isDying = false;
            _originalPosition = transform.position;
            _myRb = GetComponentInChildren<Rigidbody2D>();
            _animatorController = new AnimatorController(GetComponentInChildren<Animator>());
            _soundController = FindObjectOfType<SoundController>();
            _player = FindObjectOfType<PlayerController>();
            _gameManager = FindObjectOfType<GameManager>();
        }
        private void Update()
        {
            //Si el enemigo esta activa o se esta muriendo, almacenará su posición actual para un posible uso en el futuro
            if (_isActive || _isDying)
            {
                this._enemyData.CurrentPosition = transform.position;
            }
            //Si se esta muriendno y esta en el suelo, el enemigo será completamente deshabilitado
            if (_isDying && _isInGround)
            {
                ChangeStateToDead();
            }
        }
        /// <summary>
        /// Módulo encargado de realizar las operaciones iniciales sobre el enemigo como revisar si esta muerto o si puede activarse
        /// </summary>
        /// <param name="enemyData"> Datos almacenados del enemigo </param>
        public void PrepareEnemy(EnemyData enemyData)
        {
            this._enemyData = enemyData;
            if (_enemyData.IsDead)
            {
                transform.position = enemyData.CurrentPosition;
                ChangeStateToDead();
            }
            else
            {
                switch (_gameManager.GetGameData.NumberOfCharactersWon())
                {
                    case 0:
                        if (enemyDifficulty != EnemyDifficulty.Level0)
                        {
                            this.gameObject.SetActive(false);
                        }
                        break;
                    case 1:
                        if (enemyDifficulty == EnemyDifficulty.Level2)
                        {
                            this.gameObject.SetActive(false);
                        }
                        break;
                }
            }
        }
        #region EnemyDie
        /// <summary>
        /// Si el enemigo recibe un impacto del jugador, este comenzará a morir
        /// </summary>
        public void StartDying()
        {
            if (_isActive)
            {
                //Empezará realizando su animación de muerto
                _animatorController.SetDeath();
                //Se desactivarán sus funciones de movimiento y ataque
                _isActive = false;
                //Dependiendo del tipo de enemigo, morirá de una forma diferente
                switch (enemyType)
                {
                    //Si es un Vulture (vuela), caerá al suelo y realizará su sonido de muerte
                    case EnemyType.Vulture:
                        _soundController.PlaySound(SoundController.SoundName.DeadVulture);
                        _isDying = true;
                        if (_myRb != null)
                        {
                            _myRb.isKinematic = false;
                        }
                        break;
                    //Si es un Scorpio solo realizará su sonido de muerte
                    case EnemyType.Scorpio:
                        _soundController.PlaySound(SoundController.SoundName.DeadScorpio);
                        break;
                }
            }

        }
        /// <summary>
        /// Cuando el enemigo muere del todo, al terminar su animación de muerte o al impactar contra el suelo, se cambiará su estado actual a muerto
        /// y todos sus Scripts serán eliminado para no realizar operaciones adicionales
        /// </summary>
        public void ChangeStateToDead()
        {
            var components = GetComponents(typeof(Component));
            _enemyData.IsDead = true;
            _isDying = false;
            foreach (var comp in components)
            {
                if (!(comp is SpriteRenderer) && !(comp is Transform) && !(comp is EnemyController))
                {

                    Destroy(comp);
                }
            }
            GetComponent<SpriteRenderer>().sprite = deadSprite;
            enabled = false;
        }
        /// <summary>
        /// Módulo llamado por el Animator del enemigo cuando finaliza la animación de muerte
        /// </summary>
        public void AnimatorEventDead()
        {
            _isDying = true;
            _animatorController.DisableAnimator();
        }
        #endregion

        #region Collision&Trigger
        private void OnTriggerEnter2D(Collider2D collision)
        { 
            //Si choca contra el enemigo, le realiza daño por impaco
            if (collision.CompareTag(playerTag) && _isActive)
            {
                _player.TryReceiveDamage(damageContact);
            }
            if (collision.CompareTag(groundTag))
            {
                _isInGround = true;
            }
            //Los enemigos solo se activan cuando la camará colisiona con ellos
            if (collision.CompareTag(cameraTag))
            {
                _isActive = true;
            }
            //Si caen al vacio, su estado cambian directamente a muerto
            if (collision.CompareTag(pitTag))
            {
                ChangeStateToDead();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(groundTag))
            {
                _isInGround = false;
            }
        }
        #endregion
        #region Properties 
        public EnemyData EnemyData
        {
            get
            {
                return _enemyData;
            }
        }
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
        }
        public Vector3 OriginalPosition
        {
            get
            {
                return _originalPosition;
            }
        }
        #endregion
    }
}
