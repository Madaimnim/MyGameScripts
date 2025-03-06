using System;
using System.Collections;
using TMPro.Examples;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class Enemy : MonoBehaviour, IDamageable
{
    #region 公開變數
    public int enemyID;// 敵人 ID，由Inspector設定
    public Animator animator;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public BehaviorTree behaviorTree;
    public AttackSpawner attackSpawner;
    public EnemyStateManager.EnemyStats States { get; private set; }
    #endregion
    #region 私有變數
    private float lastActionTime;
    private int currentHealth;
    #endregion

    #region Awake()方法
    private void Awake() {
        lastActionTime = -Mathf.Infinity;
        SetEnemyData();
    }
    #endregion
    #region OnEnable()方法
    private void OnEnable() {
    }
    #endregion
    #region OnDisable()方法
    private void OnDisable() {
    }
    #endregion
    #region Start()方法
    private void Start() {
        //Todo Event_HpChanged?.Invoke(currentHealth, Stats.maxHealth);// 觸發事件，通知 UI 初始血量
    }
    #endregion

    #region Update()方法
    private void Update() {
    }
    #endregion

    #region 公開TakeDamage()方法
    public void TakeDamage(int takeDamage) {
        currentHealth -= takeDamage;
        //Todo currentHealth = Mathf.Clamp(currentHealth, 0, Stats.maxHealth);
        Debug.Log($"敵人 {enemyID} 受傷！當前血量: {currentHealth}");


        //Todo Event_HpChanged?.Invoke(currentHealth, Stats.maxHealth);//觸發事件，通知 UI 更新血量

        StartCoroutine(FlashWhite(0.1f));//執行閃白協程，替換材質
        ShowDamageText(takeDamage);//顯示damage數字TEXT
    }
    #endregion

    #region 公開StartCooldown()方法
    public void StartCooldown() {
        lastActionTime = Time.time;
        behaviorTree.isCooldownComplete = false; // 進入冷卻狀態
    }
    #endregion

    #region 私有ShowDamageText(int damage)
    private void ShowDamageText(int damage) {
        //Todo if (Stats.damageTextPrefab == null)
        //{
        //    Debug.LogError("傷害預製體未設置，請在EnemyStatData裡設置");
        //    return;
        //}
        //Vector3 spawnPosition = transform.position + new Vector3(0, 1f, 0); // 讓數字浮動在敵人上方
        //
        //GameObject damageTextObj = Instantiate(Stats.damageTextPrefab, spawnPosition, Quaternion.identity, transform);
        //damageTextObj.GetComponent<DamageTextController>().Setup(damage);
    }
    #endregion
    #region 私有SetEnemyData()
    private void SetEnemyData() {
        //Todo Stats = EnemyStateManager.Instance.GetEnemyState(enemyID);
        spriteRenderer.material = GameManager.Instance.normalMaterial;
    }
    #endregion
    #region 私有協程：FlashWhite(float duration)，受擊閃白效果
    private IEnumerator FlashWhite(float duration) {
        if (spriteRenderer != null)
        {
            spriteRenderer.material = GameManager.Instance.flashMaterial;
            yield return new WaitForSeconds(duration);
            spriteRenderer.material = GameManager.Instance.normalMaterial;
        }
    }
    #endregion
}
