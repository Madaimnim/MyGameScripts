using UnityEngine;
using System.Collections.Generic;

public class AttackForLayer : MonoBehaviour
{
    #region 公開變數
    public float destroyDelay = 0.1f; // 碰撞後延遲消失
    #endregion

    #region 私有變數
    private int attackPower = 0; // 火球的攻擊力
    private LayerMask targetLayers; // ✅ 支援多選 Layer
    private HashSet<IDamageable> hitTargets = new HashSet<IDamageable>(); // 確保每個敵人只觸發一次
    #endregion

    #region Awake()方法
    private void Awake() { }
    #endregion

    #region OnTriggerEnter2D
    private void OnTriggerEnter2D(Collider2D collision) {
        // ✅ 使用 LayerMask 來檢查是否在攻擊目標內
        if (((1 << collision.gameObject.layer) & targetLayers) != 0)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null && !hitTargets.Contains(damageable))
            {
                hitTargets.Add(damageable);
                damageable.TakeDamage(attackPower);
                Destroy(gameObject, destroyDelay);
            }
        }
    }
    #endregion

    #region 設定攻擊目標 LayerMask
    public void SetTargetLayers(LayerMask layers) {
        targetLayers = layers; // ✅ 傳遞 LayerMask
    }
    #endregion

    #region 設定攻擊力
    public void SetAttackPower(int power) {
        attackPower = power;
    }
    #endregion
}
