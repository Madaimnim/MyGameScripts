using UnityEngine;
using System.Collections.Generic;

public class AttackSpawner : MonoBehaviour
{
    #region 公開變數
    public SkillController skillController;
    public LayerMask targetLayers; // ✅ 允許多選 Layer

    public float InstantiatOffsetX = 0.0f;
    public float InstantiatOffsetY = 0.0f;
    #endregion
    #region 私有變數
    private GameObject currentAttackObject;
    private int attackPower = 0;
    private AttackForLayer attackForLayer;
    #endregion

    #region AnimationEvent方法()

    #region 公開 SpawnAttack() 方法
    //public void SpawnAttack() {
    //    GameObject attackObjectPrefab = skillController.GetCurrentAttackObjectPrefab();
    //    if (attackObjectPrefab == null)
    //    {+
    //        Debug.LogWarning("❌ AttackSpawner: attackObjectPrefab 未設定！");
    //        return;
    //    }
    //
    //    Vector2 spawnPosition = new Vector2(transform.position.x + InstantiatOffsetX, transform.position.y + InstantiatOffsetY);
    //    currentAttackObject = Instantiate(attackObjectPrefab, spawnPosition, Quaternion.identity);
    //    currentAttackObject.SetActive(false);
    //
    //    SetAttackObjectTargetLayers();
    //    SetAttackObjectPower();
    //    SetSkillStrategy();
    //
    //    currentAttackObject.SetActive(true);
    //}
    #endregion

    #endregion

    #region 私有SetAttackObjectTargetLayers()方法
    private void SetAttackObjectTargetLayers() {
        attackForLayer = currentAttackObject.GetComponent<AttackForLayer>();
        if (attackForLayer == null)
        {
            Debug.LogError("❌ 沒有找到 AttackForLayer 腳本");
            return;
        }
        attackForLayer.SetTargetLayers(targetLayers); // 直接傳遞 LayerMask
    }
    #endregion
    #region 私有SetAttackObjectPower()方法
    private void SetAttackObjectPower() {
        //attackPower = skillController.GetCurrentAttackPower();
        attackForLayer.SetAttackPower(attackPower);
    }
    #endregion
    #region 私有SetSkillStrategy()方法
    private void SetSkillStrategy() {
        //SkillStrategyBase strategy = skillController.GetSkillStrategy();
        //strategy.ExecuteAttack(currentAttackObject, skillController.GetNextAvailableSkill()?.attackObjectSpeed ?? 0f);
    }
    #endregion
}
