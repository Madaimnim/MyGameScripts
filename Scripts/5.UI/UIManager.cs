using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }      //單例
    public UISkillController uiSkillController;

    #region 角色管理
    private List<int> uiPlayerIDsList; // 角色 ID 順序列表
    private int currentIndex = 0;   // 貫穿整個 UI 的核心變數
    private PlayerStateManager.PlayerStats currentPlayer; // UI中當前角色
    #endregion

    #region 生命週期
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Start() {
        yield return StartCoroutine(GameManager.Instance.WaitForDataReady());
        StartCoroutine(InitializeUIPlayerIDsList());
    }

    private IEnumerator InitializeUIPlayerIDsList() {
        if (PlayerStateManager.Instance == null)
        {
            Debug.LogError("❌ [UIManager] 找不到 PlayerStateManager！");
            yield break;
        }

        if (PlayerStateManager.Instance.playerStatesDtny== null || PlayerStateManager.Instance.playerStatesDtny.Count == 0)
        {
            Debug.LogError("⚠️ [UIManager] playerStatesDtny 為空！");
            yield break;
        }

        // ✅ 只加入已解鎖的角色
        uiPlayerIDsList = PlayerStateManager.Instance.unlockedPlayerIDsList
            .Where(id => PlayerStateManager.Instance.playerStatesDtny.ContainsKey(id)) // 確保角色存在於 playerStatesDtny
            .ToList();

        Debug.Log($"🔍 [UIManager]顯示unlockedPlayerIDsList: {string.Join(", ", uiPlayerIDsList)}");

        if (uiPlayerIDsList.Count == 0)
        {
            Debug.LogError("❌ [UIManager] 沒有已解鎖的角色可顯示！");
            yield break;
        }

        UpdateAllUI(); // ✅ 初始化 UI
    }
    #endregion


    public void UpdateAllUI() {
        if (uiPlayerIDsList.Count == 0)
        {
            Debug.LogError("❌ [UIManager] uiPlayerIDsList 為空，無法更新 UI！");
            return;
        }
        int playerID = uiPlayerIDsList[currentIndex];
        PlayerStateManager.PlayerStats newPlayer = PlayerStateManager.Instance.playerStatesDtny[playerID];
        currentPlayer = newPlayer;
        EventBus.Trigger(new UICurrentPlayerChangEvent(currentPlayer));
    }




    #region 全域方法GetCurrentPlayer()
    public static PlayerStateManager.PlayerStats GetCurrentPlayer() {
        return Instance?.currentPlayer;
    }
    #endregion

    #region 提供外部方法變更currentIndex
    public void ChangLastPlayer() {
        currentIndex = (currentIndex - 1 + uiPlayerIDsList.Count) % uiPlayerIDsList.Count;
        UpdateAllUI();

        // 关闭技能选择面板
        uiSkillController.skillSelectionPanel.SetActive(false);
    }

    public void ChangNextPlayer() {
        currentIndex = (currentIndex + 1) % uiPlayerIDsList.Count;
        UpdateAllUI();

        // 关闭技能选择面板
        uiSkillController.skillSelectionPanel.SetActive(false);
    }

        public void SetCurrentCharacter(int newIndex) {
        if (newIndex < 0 || newIndex >= uiPlayerIDsList.Count) return;
        currentIndex = newIndex;
        UpdateAllUI();
    }
    #endregion
}
