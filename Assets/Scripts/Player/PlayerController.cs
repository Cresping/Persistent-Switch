using System.Collections;
using UnityEngine;
using Utils;
using Enemy;
using UI;
using System.Collections.Generic;
using Persistence;

namespace Player
{
    /// <summary>
    /// Clase encargada de controlar el personaje principal del juego
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private const float INITIAL_SPEED_Y_ADDFORCE = 0.1F;
        private const float LOST_SPEED_X_ADDFORCE = 0.05f;
        private const float LOST_SPEED_Y_ADDFORCE = 0.012f;

        [SerializeField] private InputController inputController;
        [SerializeField] private string pitTag = "Pit";
        [SerializeField] private string endTag = "End";
        [SerializeField] private AnimatorController animatorController;
        [SerializeField] private HealthbarController healthbarController;
        [SerializeField] private float speedMovement=3f;
        [SerializeField] private float jumpForce=5f;
        [SerializeField] private float recoilForce = 2f;
        [SerializeField] private float fallForce = 6f;
        [SerializeField] private GameObject attackCheck;
        [SerializeField] private PlayerTriggerControler groundCheck;

        private SceneManagement _sceneManager;
        private EndGameController _endGameController;
        private SoundController _soundController;
        private Rigidbody2D _myRb;
        private CapsuleCollider2D _myCollider;
        private CharacterData _playerData;
        private int _whereIsLooking;
        private bool _isFalling;
        private bool _canAttack;
        private bool _isReceivingDamage;
        private IEnumerator _coroutineActiveAttack;
        private IEnumerator _coroutineActiveRBAddForce;

        private GameManager _gameManager;


        private void Awake()
        {
            _playerData = new CharacterData();
            _playerData.CurrentPosition = transform.position;
            _isReceivingDamage = false;
            _isFalling = false;
            _canAttack = true;
            _whereIsLooking = 1;
            _gameManager = null;
            attackCheck.SetActive(false);
            _coroutineActiveAttack = CoroutineActiveAttack();
            _coroutineActiveRBAddForce = CoroutineActiveRBAddForce(0,0,0);
            _myRb = GetComponentInChildren<Rigidbody2D>();
            _myCollider = GetComponent<CapsuleCollider2D>();
            animatorController = new AnimatorController(GetComponentInChildren<Animator>());
            _sceneManager = FindObjectOfType<SceneManagement>();
            _gameManager = FindObjectOfType<GameManager>();
            _endGameController = FindObjectOfType<EndGameController>();
            _soundController = FindObjectOfType<SoundController>();
            PreparePlayer(_playerData);
        }
        private void Update()
        {
            if (!_playerData.IsDead && !_playerData.HasWon)
            {
                TryJump();
                TryAttack();
                TryFall();
                ExitGame();
                //Cada vez que el jugador pisa el suelo, guarda la posición de su posible tumba
                if (groundCheck.IsGround)
                {
                    UpdateGravePosition();
                }
            }
        }
        private void FixedUpdate()
        {
            if (!_playerData.IsDead && !_playerData.HasWon)
            {
                TryMove();
            }
        }
        /// <summary>
        /// Cada vez que el juego se minimiza, guarda los datos del jugador
        /// </summary>
        /// <param name="focus">Estado del juego</param>
        private void OnApplicationFocus(bool focus)
        {
            _playerData.CurrentPosition = (Vector2)transform.position;
            if (_gameManager!=null)
            {
                _gameManager.SaveGameSaveData();
            }
        }
        /// <summary>
        /// Al salir del juego, guarda la información del jugador
        /// </summary>
        private void ExitGame()
        {
            if (inputController.ExitButton)
            {
                _playerData.CurrentPosition = (Vector2)transform.position;
                if (_gameManager != null)
                {
                    _gameManager.SaveGameSaveData();
                    _gameManager.ExitGame();
                }
            }
        }
        #region Try
        /// <summary>
        /// Módulo encargado de controlar el movimiento del jugador
        /// </summary>
        private void TryMove()
        {
            //Si esta en el suelo, no esta cayendo y puede atacar, el jugador puede moverse
            if (CheckIsGroundOrMovingPlataform() && !_isFalling && !_isReceivingDamage && _canAttack)
            {
                //Cada vez que el jugador se mueve, la velocidad de su RB se pone a 0
                _myRb.velocity = Vector2.zero;
                if (inputController.HorizontalAxis != 0)
                {
                    //Si esta en tierra, se mueve por RB
                    if (groundCheck.IsGround)
                    {
                        _myRb.MovePosition((Vector2)transform.position + Vector2.right * inputController.HorizontalAxis * speedMovement * Time.deltaTime);
                    }
                    //Si esta en una plataforma, se mueve por Transform
                    if (groundCheck.IsMovingPlataform)
                    {

                        transform.position = (Vector2)transform.position + Vector2.right * inputController.HorizontalAxis * speedMovement * Time.deltaTime;
                    }
                    //Si mira hacía otro lado, se cambia la dirección donde mira
                    transform.localScale = new Vector3(inputController.HorizontalAxis * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    _whereIsLooking = (int)inputController.HorizontalAxis * Mathf.Abs(_whereIsLooking);
                }
            }
            animatorController.SetWalk(inputController.HorizontalAxis != 0);
        }
        /// <summary>
        /// Módulo encargado de ejecutar el salto del jugador
        /// </summary>
        private void TryJump()
        {
            //El jugador puede saltar cuando esta en el suelo, se ha pulsado el boton de salto, no esta cayendo y no esta recibiendo daño
            if(CheckIsGroundOrMovingPlataform() && inputController.JumpButton && !_isFalling && !_isReceivingDamage)
            {
                _coroutineActiveRBAddForce = CoroutineActiveRBAddForce(jumpForce,jumpForce,inputController.HorizontalAxis);
                StartCoroutine(_coroutineActiveRBAddForce);
            }
        }
        /// <summary>
        /// Módulo encargado de ejecutar el ataque del jugador
        /// </summary>
        private void TryAttack()
        {
            //El jugador puede atacar si se le permite atacar, esta pulsando el boton de ataque y no esta recibiendo daño
            if(inputController.AttackButton && _canAttack && !_isReceivingDamage)
            {
                _canAttack = false;
                animatorController.SetAttack(true);
            }
        }
        /// <summary>
        /// Módulo encargado de permitir al jugador recibir daño
        /// </summary>
        /// <param name="damage">Daño recibido</param>
        public void TryReceiveDamage(int damage)
        {
            if (!_isReceivingDamage)
            {
                DisableAttack();
                DisableRBAddForce();
                _playerData.CurrentHealthPoints -= damage;
                healthbarController.TakeHealth(damage);
                _isReceivingDamage = true;
                if (_playerData.CurrentHealthPoints <= 0)
                {
                    _soundController.PlaySound(SoundController.SoundName.DeadPlayer);
                    _myCollider.direction = CapsuleDirection2D.Horizontal;
                    _myCollider.size = new Vector2(0.67f, 0.14f);
                    animatorController.SetDeath();
                }
                else
                {
                    _soundController.PlaySound(SoundController.SoundName.HurtPlayer);
                    DoRecoilMovement();
                    animatorController.SetHurt(true);
                }
            }
        }
        /// <summary>
        /// Módulo encargado de permitir al jugador caerse de plataformas
        /// </summary>
        private void TryFall()
        {
            if (!_isFalling && !CheckIsGroundOrMovingPlataform() && !_isReceivingDamage)
            {
                _coroutineActiveRBAddForce = CoroutineActiveRBAddForce(0, -fallForce, 0);
                StartCoroutine(_coroutineActiveRBAddForce);
            }
        }
        #endregion
        #region EndGame
        /// <summary>
        /// Finaliza la partida matando al jugador
        /// </summary>
        private void Die()
        {
            _playerData.IsDead = true;
            animatorController.DisableAnimator();
            DisableRBAddForce();
            DisableAttack();
            _playerData.CurrentPosition = (Vector2)transform.position;
            if (_gameManager != null)
            {
                _gameManager.SaveGameSaveData();
            }
            _endGameController.StartSequenceGameOver(_playerData.CharacterName.ToString());
        }
        /// <summary>
        /// Finaliza la partida haciendo que el jugador gane
        /// </summary>
        private void Win()
        {
            _playerData.HasWon = true;
            DisableRBAddForce();
            DisableAttack();
            _playerData.CurrentPosition = (Vector2)transform.position;
            if (_gameManager != null)
            {
                _gameManager.SaveGameSaveData();
            }
            _endGameController.StartSequenceWinGame(_playerData.CharacterName.ToString());

        }
        #endregion
        #region AnimatorEvent
        /// <summary>
        /// Activado por evento de animacion, módulo encargado de activar el collider encargado de registrar que el ataque ha tenido exito
        /// </summary>
        public void EnableAttackDamage()
        {
            attackCheck.SetActive(true);
            _soundController.PlaySound(SoundController.SoundName.AttackPlayer);
            StartCoroutine(_coroutineActiveAttack);
        }
        /// <summary>
        /// Activado por evento de animacion, módulo encargado de desactivar el collider de ataque y desactivar el ataque
        /// </summary>
        public void DisableAttack()
        {
            StopCoroutine(_coroutineActiveAttack);
            attackCheck.SetActive(false);
            animatorController.SetAttack(false);
            _canAttack = true;
        }
        /// <summary>
        /// Activado por evento de animacion , módulo encargado de desactivar la inmunidad del jugador
        /// </summary>
        public void DisableImmunity()
        {
            _isReceivingDamage = false;
            animatorController.SetHurt(false);
        }

        #endregion
        #region Attack
        /// <summary>
        /// Corrutina encargada de intentar matar a un enemigo cuando el jugador ataca haciendo uso de un Collider hijo
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoroutineActiveAttack()
        {
            while (true)
            {
                if (attackCheck.GetComponentInChildren<TriggerCheck>().GetIsTriggered())
                {
                    attackCheck.GetComponent<TriggerCheck>().GetObjectTriggered().GetComponent<EnemyController>().StartDying();
                }
                yield return new WaitForFixedUpdate();
            }
        }
        #endregion
        #region Physics
        /// <summary>
        /// Móduloe encargado de tirar al jugador en la dirección contraria donde esta mirando
        /// </summary>
        private void DoRecoilMovement()
        {
            _coroutineActiveRBAddForce = CoroutineActiveRBAddForce(recoilForce, recoilForce, _whereIsLooking * (-1));
            StartCoroutine(_coroutineActiveRBAddForce);
        }
        /// <summary>
        /// Corrutina encargada de agregar velocidad al jugador. Funciona de forma parecida a la función AddForce de Unity, pero con parametros personalizados.
        /// </summary>
        /// <param name="velocityInX"> Velocidad que se agregará en X</param>
        /// <param name="velocityInY"> Velocidad que se agregará en Y </param>
        /// <param name="horizontal"> Dirección horizontal donde se agregará la velocidad</param>
        /// <returns></returns>
        private IEnumerator CoroutineActiveRBAddForce(float velocityInX, float velocityInY, float horizontal)
        {
            float velocityX = velocityInX; 
            float velocityY = velocityInY; 
            float removeSpeedY = INITIAL_SPEED_Y_ADDFORCE;
            animatorController.SetJump(true);
            _myRb.velocity = Vector2.zero;
            _isFalling = true;
            //Mientras que este en el suelo, esperará hasta que el personaje salga de este
            while (CheckIsGroundOrMovingPlataform())
            {
                _myRb.velocity = new Vector2(horizontal * velocityX, velocityY);
                yield return new WaitForFixedUpdate();
            }
            //Cuando esta fuera del suelo, esperará hasta que el personaje vuelva a caer al suelo
            while (!CheckIsGroundOrMovingPlataform())
            {
                //Si la velocidad de X es cercana a 0 se pone a 0
                if (Mathf.Round(velocityX) != 0)
                {
                    velocityX -= LOST_SPEED_X_ADDFORCE;
                }
                else
                {
                    velocityX = 0;
                } 
                removeSpeedY += LOST_SPEED_Y_ADDFORCE;
                velocityY -= removeSpeedY;
                _myRb.velocity = new Vector2(horizontal * velocityX, velocityY);
                yield return new WaitForFixedUpdate();
            }
            //Al llegar al suelo se ejecuta el sonido de salto y se anulan las velocidades
            _soundController.PlaySound(SoundController.SoundName.JumpPlayer);
            _myRb.velocity = Vector2.zero;
            _isFalling = false;
            animatorController.SetJump(false);
        }
        /// <summary>
        /// Desactiva el módulo anterior
        /// </summary>
        public void DisableRBAddForce()
        {
            StopCoroutine(_coroutineActiveRBAddForce);
            _myRb.velocity = Vector2.zero;
            _isFalling = false;
            animatorController.SetJump(false);
        }
        #endregion
        #region PlayerData
        /// <summary>
        /// Se ejecuta al iniciar el juego, prepará al jugador con los datos cargados
        /// </summary>
        /// <param name="playerData">Datos del jugador</param>
        public void PreparePlayer(CharacterData playerData)
        {
            this._playerData = playerData;
            transform.position = PlayerData.CurrentPosition;
            healthbarController.PrepareHealthBar(_playerData);
            animatorController.ChangeAnimator(_playerData.CharacterName.ToString());
        }
        /// <summary>
        /// Actualiza la posición de la tumba
        /// </summary>
        private void UpdateGravePosition()
        {
            _playerData.GravePosition = (Vector2)transform.position;
        }
        #endregion
        /// <summary>
        /// Comprueba que este en tierra firme o en una plataforma movil
        /// </summary>
        /// <returns></returns>
        private bool CheckIsGroundOrMovingPlataform()
        {
            if (groundCheck.IsMovingPlataform || groundCheck.IsGround)
            {
                return true;
            }
            return false;
        }
        public CharacterData PlayerData
        {
            get
            {
                return _playerData;
            }
            set
            {
                _playerData = value;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(pitTag))
            {
                _soundController.PlaySound(SoundController.SoundName.DeadPlayer);
                this.gameObject.SetActive(false);
                Die();
            }
            if (collision.CompareTag(endTag))
            {
                Win();
            }
        }
    }
}

