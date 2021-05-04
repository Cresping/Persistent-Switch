using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Clase encargada de controlar los sonidos del juego
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SoundController : MonoBehaviour
    {
        /// <summary>
        /// Para ejecutar el sonido es necesario hacer referencia al enum que lo representa
        /// </summary>
        public enum SoundName
        {
            Heartbeat, Scare, Violin, ButtonMouseEnter, ButtonMouseClick, JumpPlayer, HurtPlayer, DeadPlayer, AttackPlayer, DeadScorpio, DeadVulture
        };
        [SerializeField] private AudioClip hearbeatSound;
        [SerializeField] private AudioClip scareSound;
        [SerializeField] private AudioClip violinSound;
        [SerializeField] private AudioClip buttonMouseEnterSound;
        [SerializeField] private AudioClip buttonMouseClickSound;
        [SerializeField] private AudioClip jumpPlayerSound;
        [SerializeField] private AudioClip hurtPlayerSound;
        [SerializeField] private AudioClip deadPlayerSound;
        [SerializeField] private AudioClip attackPlayerSound;
        [SerializeField] private AudioClip deadScorpioSound;
        [SerializeField] private AudioClip deadVultureSound;

        private AudioSource _soundEffects;

        private void Awake()
        {
            _soundEffects = GetComponent<AudioSource>();
        }
        /// <summary>
        /// Módulo encargado de ejecutar el sonido y devolver su duración
        /// </summary>
        /// <param name="soundName">Sonido seleccionado</param>
        /// <returns></returns>
        public float PlaySound(SoundName soundName)
        {
            AudioClip aux;
            switch (soundName)
            {
                case SoundName.Heartbeat:
                    aux = hearbeatSound;
                    break;
                case SoundName.Scare:
                    aux = scareSound;
                    break;
                case SoundName.Violin:
                    aux = violinSound;
                    break;
                case SoundName.ButtonMouseEnter:
                    aux = buttonMouseEnterSound;
                    break;
                case SoundName.ButtonMouseClick:
                    aux = buttonMouseClickSound;
                    break;
                case SoundName.JumpPlayer:
                    aux = jumpPlayerSound;
                    break;
                case SoundName.HurtPlayer:
                    aux = hurtPlayerSound;
                    break;
                case SoundName.DeadPlayer:
                    aux = deadPlayerSound;
                    break;
                case SoundName.AttackPlayer:
                    aux = attackPlayerSound;
                    break;
                case SoundName.DeadScorpio:
                    aux = deadScorpioSound;
                    break;
                case SoundName.DeadVulture:
                    aux = deadVultureSound;
                    break;
                default:
                    aux = null;
                    break;
            }
            _soundEffects.PlayOneShot(aux);
            return aux.length;
        }
    }
}

