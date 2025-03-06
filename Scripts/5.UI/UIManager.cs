using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }      //單例
    
    #region 角色管理
    private Dictionary<int, PlayerStateManager.PlayerStats> playerStatesDtny;
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

        playerStatesDtny = PlayerStateManager.Instance.playerStatesDtny;
        if (playerStatesDtny == null || playerStatesDtny.Count == 0)
        {
            Debug.LogError("⚠️ [UIManager] playerStatesDtny 為空！");
            yield break;
        }

        // ✅ 只加入已解鎖的角色
        uiPlayerIDsList = PlayerStateManager.Instance.unlockedPlayerIDsList
            .Where(id => playerStatesDtny.ContainsKey(id)) // 確保角色存在於 playerStatesDtny
            .ToList();

        Debug.Log($"🔍 [UIManager] 初始化角色 ID 列表: {string.Join(", ", uiPlayerIDsList)}");

        if (uiPlayerIDsList.Count == 0)
        {
            Debug.LogError("❌ [UIManager] 沒有已解鎖的角色可顯示！");
            yield break;
        }

        UpdateAllUI(); // ✅ 初始化 UI
    }


    #endregion


    private void UpdateAllUI() {
        if (uiPlayerIDsList.Count == 0)
        {
            Debug.LogError("❌ [UIManager] uiPlayerIDsList 為空，無法更新 UI！");
            return;
        }

        int playerID = uiPlayerIDsList[currentIndex];
        PlayerStateManager.PlayerStats newPlayer = playerStatesDtny[playerID];
        currentPlayer = newPlayer;

        Debug.Log($"🔹 [UIManager] 角色變更為: {currentPlayer.playerName} (ID: {playerID})");

        EventBus.Trigger(new UICurrentPlayerChangEvent(currentPlayer));
    }




    #region 全域方法GetCurrentPlayer()
    public static PlayerStateManager.PlayerStats GetCurrentPlayer() {
        return Instance?.currentPlayer;
    }
    #endregion

    #region 提供外部方法變更currentIndex
    public void ChangLastPlayer() {
        if (uiPlayerIDsList.Count <= 1)
        {
            Debug.LogWarning("❌ [UIManager] 無法切換角色，只有一個角色！");
            return;
        }

        currentIndex = (currentIndex - 1 + uiPlayerIDsList.Count) % uiPlayerIDsList.Count;
        Debug.Log($"⬅️ [UIManager] 切換到上一個角色，新的 Index: {currentIndex}");

        UpdateAllUI();
    }

    public void ChangNextPlayer() {
        if (uiPlayerIDsList.Count <= 1)
        {
            Debug.LogWarning("❌ [UIManager] 無法切換角色，只有一個角色！");
            return;
        }

        currentIndex = (currentIndex + 1) % uiPlayerIDsList.Count;
        Debug.Log($"➡️ [UIManager] 切換到下一個角色，新的 Index: {currentIndex}");

        UpdateAllUI();
    }



    public void SetCurrentCharacter(int newIndex) {
        if (newIndex < 0 || newIndex >= uiPlayerIDsList.Count) return;
        currentIndex = newIndex;
        UpdateAllUI();
    }
    #endregion

}
