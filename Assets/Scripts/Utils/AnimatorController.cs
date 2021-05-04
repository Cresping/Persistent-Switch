using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Utils
{
    [Serializable]
    /// <summary>
    /// Clase encargada de cambiar las animaciones del Animator
    /// </summary>
    public class AnimatorController 
    {
        [SerializeField] private String nameWalkParameter = "Walk";
        [SerializeField] private String nameJumpParameter = "Jump";
        [SerializeField] private String nameAttackParameter = "Attack";
        [SerializeField] private String nameHurtParameter = "Hurt";
        [SerializeField] private String nameDeadParameter = "Death";
        private Animator _animator;

        ///// <summary>
        ///// Constructor parametrizado
        ///// </summary>
        ///// <param name="animator">Animator del modelo</param>
        public AnimatorController(Animator animator)
        {
            this._animator = animator;
        }
        /// <summary>
        /// Cambia el estado de  la animación 'Walk'
        /// </summary>
        /// <param name="value">Estado</param>
        public void SetWalk(bool value)
        {
            _animator.SetBool(nameWalkParameter, value);
        }
        /// <summary>
        /// Cambia el estado de  la animación 'Jump'
        /// </summary>
        /// <param name="value">Estado</param>
        public void SetJump(bool value)
        {
            _animator.SetBool(nameJumpParameter,value);
        }
        /// <summary>
        /// Cambia el estado de  la animación 'Attack'
        /// </summary>
        /// <param name="value">Estado</param>
        public void SetAttack(bool value)
        {
            _animator.SetBool(nameAttackParameter, value);
        }
        /// <summary>
        /// Cambia el estado de  la animación 'Hurt'
        /// </summary>
        /// <param name="value">Estado</param>
        public void SetHurt(bool value)
        {
            _animator.SetBool(nameHurtParameter, value);
        }
        /// <summary>
        /// Cambia el estado de  la animación 'Hurt'
        /// </summary>
        /// <param name="value">Estado</param>
        public void SetDeath()
        {
            _animator.SetTrigger(nameDeadParameter);
        }
        /// <summary>
        /// Cambia el animator del modelo
        /// </summary>
        /// <param name="animator">Nombre del animator que se cambiará</param>
        public void ChangeAnimator(String animator)
        {
            _animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animators/Animator" + animator);
        }
        /// <summary>
        /// Desactiva el animator del modelo
        /// </summary>
        public void DisableAnimator()
        {
            _animator.enabled = false;
        }
    }

}

