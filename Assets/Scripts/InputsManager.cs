using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerController.InputsManager
{
    public class InputsManager : MonoBehaviour
    {
        private PlayerControls m_PlayerControls;

        [SerializeField]
        private Vector2 m_Movement = Vector2.zero;
         
        [SerializeField]
        private Vector2 m_MousePosition = Vector2.zero;

        public Vector3 _MousePosition
        {
            get { return m_MousePosition; }
            private set { m_MousePosition = value; }
        }

        public Vector2 _MovementInput
        {
            get { return m_MovementInput; }
            private set { m_MovementInput = value; }
        }

        public float _HorizontalInput
        {
            get { return m_HorizontalInput; }
            private set { m_HorizontalInput = value; }
        }

        public float _VerticalInput
        {
            get { return m_VerticalInput; }
            private set { m_VerticalInput = value;}
        }   
        
        private Vector2 m_MovementInput = Vector2.zero;
        private float m_HorizontalInput = 0f;
        private float m_VerticalInput = 0f;

        private void OnEnable()
        {
            if(m_PlayerControls == null)
            { 
                m_PlayerControls = new PlayerControls();
                m_PlayerControls.PlayerController.Movement.performed += i => m_Movement = i.ReadValue<Vector2>();
                m_PlayerControls.PlayerController.LookAt.performed += i => m_MousePosition = i.ReadValue<Vector2>();
            }
            m_PlayerControls.Enable(); 
        }

        private void OnDisable()
        {
            m_PlayerControls.Disable();    
        }

        public void HandleInputs()
        {
            m_VerticalInput = m_Movement.y;
            m_HorizontalInput= m_Movement.x;
        }
    }
}