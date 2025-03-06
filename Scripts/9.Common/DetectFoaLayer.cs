using UnityEngine;
using System.Collections.Generic;

public class DetectForLayer : MonoBehaviour
{
    #region 公開變數

    #endregion

    #region 私有變數
    private LayerMask targetLayers; // ✅ 支援多選 Layer
    private bool hasDetectedTarget;
    #endregion

    #region Awake()方法
    private void Awake() { }
    #endregion

    #region OnTriggerEnter2D(Collider2D collision)
    private void OnTriggerEnter2D(Collider2D collision) {
        if (((1 << collision.gameObject.layer) & targetLayers) != 0)
        {
            hasDetectedTarget = true; // ✅ 設為 true，表示偵測到目標
            Debug.Log($"偵測到目標: {collision.gameObject.name}");
        }
    }
    #endregion
    #region OnTriggerExit2D (Collider2D collision)
    private void OnTriggerExit2D(Collider2D collision) {
        // ✅ 只檢測 Layer，不用處理 IDamageable
        if (((1 << collision.gameObject.layer) & targetLayers) != 0)
        {
            hasDetectedTarget = false;
            Debug.Log("所有目標已離開，重置 hasDetectedTarget = false");
        }
    }
    #endregion

    #region 設定攻擊目標 LayerMask
    public void SetTargetLayers(LayerMask layers) {
        targetLayers = layers; // ✅ 傳遞 LayerMask
    }
    #endregion
}
