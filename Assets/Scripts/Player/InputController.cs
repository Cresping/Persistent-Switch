using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Clase encargada de controlar las entradas del jugador
    /// </summary>
    [Serializable]
    public class InputController
    {
        [SerializeField] private string Horizontal = "Horizontal";
        [SerializeField] private string Vertical = "Vertical";
        [SerializeField] private string Exit = "Exit";
        [SerializeField] private string Jump = "Jump";
        [SerializeField] private string Attack = "Fire3";
        public float HorizontalAxis => Input.GetAxisRaw(Horizontal);
        public float VerticalAxis => Input.GetAxisRaw(Vertical);
        public bool ExitButton => Input.GetButtonDown(Exit);
        public bool JumpButton => Input.GetButtonDown(Jump);
        public bool AttackButton => Input.GetButtonDown(Attack);
    }
}
