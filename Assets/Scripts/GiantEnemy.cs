using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using static BlankStudio.Constants.Constants;
using Random = UnityEngine.Random;
using TMPro;
using DG.Tweening;
using BlankStudio.Constants;

[Serializable]
public class GiantEnemy : EnemyAI
{
    [Serializable]
    public class AttacksFXData
    {
        public AttackType _AttackType
        { 
            get { return m_AttackType; }
            private set { m_AttackType = value; }
        }

        [SerializeField]
        private AttackType m_AttackType = AttackType.MultiJumpAttack;

        public GameObject _AttackFX
        {
            get { return m_AttackFX; }
            private set { m_AttackFX = value; }
        }

        [SerializeField]
        private GameObject m_AttackFX = null;
    }

    [SerializeField]
    List<AttacksFXData> m_AttackFXData = new List<AttacksFXData>();

    [SerializeField]
    private Animator m_Animator = null;

    [SerializeField]
    private int m_TotalHalth = 100;

    [SerializeField]
    private TextMeshProUGUI m_HealthText = null;

    [SerializeField]
    private LayerMask m_PlayerLayerMask = default;

    [SerializeField]
    private LayerMask m_NonPlayerLayerMask = default;

    [SerializeField]
    private float m_MaxDistance = 10f;

    [SerializeField]
    private float m_MinDistance = 2f;

    [SerializeField]
    private float m_DetectionRadius = 30f;

    [SerializeField]
    private int m_CurrentHealth = 0;

    [SerializeField]
    private int m_MaxHealth = 100;
   
    [SerializeField]
    private EnemyState m_CurrentEnemyState = EnemyState.Idle;

    [SerializeField]
    private Material m_EnemyMaterial = null;

    public override Constants.EnemyType Type => Constants.EnemyType.BOSS;

    protected override void Start()
    {
        base.Start();
        m_CurrentHealth = Mathf.RoundToInt(Stats.Health);
        SetHealthText();
        m_Animator = GetComponent<Animator>();
        m_CurrentEnemyState = EnemyState.Idle;

    }

    protected override void Update()
    {
        RaycastIn180DegreeRange();
    }  

    public void RaycastIn180DegreeRange()
    {
        if (m_CurrentEnemyState == EnemyState.Death)
        {
            return;
        }
        RaycastHit hit;
        float startAngle = 0, endAngle = 360;
        int rayCount = 10;
        float angleStep = Mathf.Abs(startAngle - endAngle) / rayCount;
        float angle = startAngle;

        for (int i = 0; i < rayCount; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle, transform.up);
            Vector3 direction = rotation * transform.forward;
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
            Debug.DrawRay(newPosition, direction, Color.green); 
            if (Physics.Raycast(newPosition, direction, out hit, m_MinDistance, m_PlayerLayerMask))
            {
                StartCoroutine(SetEnemyState(EnemyState.Attack));
            }
            else if (Physics.Raycast(newPosition, direction, out hit, m_MaxDistance, m_PlayerLayerMask))
            {
                gameObject.transform.LookAt(hit.transform.position); 
                if (m_CurrentEnemyState != EnemyState.Run)
                {
                    StartCoroutine(SetEnemyState(EnemyState.Run));
                }
            }

          angle += angleStep; 
        }
    }
/*    void OnDrawGizmos() 
    {  
        //float m_ScaleFactor = 2f;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one * 1);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, m_DetectionRadius);
        Gizmos.matrix = Matrix4x4.identity; 
    }*/
    private IEnumerator SetEnemyState(EnemyState enemyState) 
    {
        if (enemyState == m_CurrentEnemyState)
            yield break;

        Debug.Log((enemyState == m_CurrentEnemyState) + " | " + enemyState + " " + m_CurrentEnemyState);//run Attack

        m_CurrentEnemyState = enemyState;
        float waitingTime = SetAnimationTrigger(enemyState.ToString());

        yield return new WaitForSeconds(waitingTime);

        switch (enemyState)
        {
            case EnemyState.Attack:
                SetEnemyState(EnemyState.Idle);
                break;
            case EnemyState.Hurt:
                SetEnemyState(EnemyState.Idle);
                break;
            case EnemyState.Death:
                Die(); 
                break;  
        }

    }

    protected override void Attack()
    {
        base.Attack();  
        PlayerStats.Instance.Health -= Stats.Damage;
    }

    private float SetAnimationTrigger(string animationName)
    {
        if (animationName == EnemyState.Attack.ToString())
        {
            m_Animator.SetTrigger(((AttackType)Random.Range(0, 4)).ToString());
            return m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
        
        m_Animator.SetTrigger(animationName);
        return m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    private void SetFXOnAnimation(AttackType attackType)
    {
        GameObject fx = m_AttackFXData[0]._AttackFX;
        for (int i = 0; i < m_AttackFXData.Count; i++)
        {
            if (m_AttackFXData[i]._AttackType == attackType)
            {
                fx = m_AttackFXData[i]._AttackFX;
            }
        }

        StartCoroutine(SetFX(fx));
    }

    private IEnumerator SetFX(GameObject fx)
    {
        fx.SetActive(true);
        Attack();
        yield return new WaitForSeconds(1f);
        fx.SetActive(false);
    }

    public override void TakeDamage(float damage)
    {

        if (m_CurrentEnemyState == EnemyState.Attack || m_CurrentEnemyState == EnemyState.Death)
        {
            return;
        }
        
        m_CurrentHealth-= Mathf.RoundToInt(damage);

        if (m_CurrentHealth <= 0 && m_CurrentEnemyState != EnemyState.Death)
        {
            StartCoroutine(SetEnemyState(EnemyState.Death));
            m_CurrentHealth = 0;
            
        } 
        SetHealthText();

    }

    private void SetHealthText()
    {
        m_HealthText.text = m_CurrentHealth.ToString();
    }

    public override void Die()
    {
        transform.DOMoveY(-2f, 1f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            base.Die();

        }
        );

    }
}


