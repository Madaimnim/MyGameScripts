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
    public EnemySkillSpawner skillSpawner;
    public EnemyStateManager.EnemyStats enemyStats { get; private set; }

    public Action<int,int> Event_HpChanged;
    #endregion
    #region 私有變數
    private float lastActionTime;
    private int currentHealth;
    #endregion

    #region 生命週期
    private void Awake() {
        lastActionTime = -Mathf.Infinity;
        SetEnemyData();
    }

    private void OnEnable() {    }
    private void OnDisable() {    }
    private IEnumerable Start() {
        yield return StartCoroutine(GameManager.Instance.WaitForDataReady());
        enemyStats = EnemyStateManager.Instance.enemyStatesDtny[enemyID];
        //Todo Event_HpChanged?.Invoke(currentHealth, Stats.maxHealth);// 觸發事件，通知 UI 初始血量
    }
    private void Update() {}
    #endregion

    #region 公開方法Initialize(EnemyStateManager.EnemyStats stats)
    //提供EnemyStateManager呼叫初始化使用
    public void Initialize(EnemyStateManager.EnemyStats stats) {
        enemyStats = stats;
        transform.name = $"Enemy_{enemyStats.enemyID} ({enemyStats.enemyName})";
        Debug.Log($"[EnemyController] {enemyStats.enemyName} 已初始化，等級 {enemyStats.level}，血量 {enemyStats.maxHealth}");
    }
    #endregion

    #region 公開TakeDamage()方法
    public void TakeDamage(int takeDamage,float knockedForce) {
        currentHealth -= takeDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, enemyStats.maxHealth); 
        Event_HpChanged?.Invoke(currentHealth, enemyStats.maxHealth);//觸發事件，通知 UI 更新血量

        ApplyKnockback(knockedForce);

        StartCoroutine(FlashWhite(0.1f));//執行閃白協程，替換材質

        ShowDamageText(takeDamage);//顯示damage數字TEXT
    }
    #endregion

    #region 私有ApplyKnockback()方法
    private void ApplyKnockback(float force) {
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // ✅ 先清除當前速度，避免擊退力疊加
            rb.AddForce(new Vector2(1,0) * force, ForceMode2D.Impulse); // ✅ 添加瞬間衝擊力
        }
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
