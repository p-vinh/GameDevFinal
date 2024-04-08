using PlayerController.InputsManager;
using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputsManager m_InputsManager;
    private PlayerLocomotion m_PlayerLocomotion;
    private Animator m_Animator;

    int m_Horizontal = 0;
    int m_Vertical = 0;

    private void Awake()
    {
        m_InputsManager = GetComponent<InputsManager>();
        m_PlayerLocomotion = GetComponent<PlayerLocomotion>();
        m_Animator = GetComponent<Animator>();
    }
     
    void Start()
    {
        m_Horizontal = Animator.StringToHash("Horizontal");
        m_Vertical = Animator.StringToHash("Vertical");
    }

    void Update()
    {
        m_InputsManager.HandleInputs(); 
        m_PlayerLocomotion.UpdateCrosshairPosition();
        UpdateAnimationValue(0, Mathf.Clamp01(Mathf.Abs(m_InputsManager._HorizontalInput)) + 
                            Mathf.Clamp01(Mathf.Abs(m_InputsManager._VerticalInput)));
    }

    private void LateUpdate()
    {
        m_PlayerLocomotion.RotateTowardsMouse();
    }

    private void FixedUpdate() 
    {
        m_PlayerLocomotion.PlayerMovement();
    }

    private void UpdateAnimationValue(float horizntalValue, float verticalValue)
    {
        float time = 0.1f;
        m_Animator.SetFloat(m_Horizontal, horizntalValue, time, Time.deltaTime);
        m_Animator.SetFloat(m_Vertical, verticalValue, time, Time.deltaTime);
    }



}

