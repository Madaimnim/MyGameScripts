﻿using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIStatusController : MonoBehaviour
{
    public Image playerIconSprite; // 顯示角色圖片
    public TextMeshProUGUI[] statusTextsArray; // 角色基本狀態顯示（前 6 個）
    public Button leftButton, rightButton;

    private PlayerStateManager.PlayerStats currentPlayer;


    private void OnEnable() {
        EventBus.Listen<UICurrentPlayerChangEvent>(OnUICurrentPlayerChanged);
        if (UIManager.Instance == null)
        {
            Debug.LogError("❌ [UIManager]為空！");
            return;
        }


        if (leftButton != null && rightButton != null)
        {
            leftButton.onClick.AddListener(UIManager.Instance.ChangLastPlayer);
            rightButton.onClick.AddListener(UIManager.Instance.ChangNextPlayer);
        }
        else
        {
            Debug.LogError("❌ [UIStatusController] 左右按鈕未綁定！");
        }
        currentPlayer = UIManager.GetCurrentPlayer();
        RefreshPlayerStatusText();
    }

    private void OnDisable() {
        EventBus.StopListen<UICurrentPlayerChangEvent>(OnUICurrentPlayerChanged);

        leftButton.onClick.RemoveListener(UIManager.Instance.ChangLastPlayer);
        rightButton.onClick.RemoveListener(UIManager.Instance.ChangNextPlayer);
    }


    private void OnUICurrentPlayerChanged(UICurrentPlayerChangEvent eventData) {
        currentPlayer = eventData.currentPlayer;
        SetActiveUIPlayer(currentPlayer.playerID);
        RefreshPlayerStatusText();
    }

    #region RefreshPlayerStatusText()
    //更新狀態文字，執行RenderTexture撥放角色預覽動畫
    private void RefreshPlayerStatusText() {
        if (statusTextsArray.Length >= 6)
        {
            statusTextsArray[0].text = $"等級: {currentPlayer.level}";
            statusTextsArray[1].text = $"HP: {currentPlayer.maxHealth}/{currentPlayer.maxHealth}";
            statusTextsArray[2].text = $"名稱: {currentPlayer.playerName}";
            statusTextsArray[3].text = $"攻擊力: {currentPlayer.attackPower}";
            statusTextsArray[4].text = $"經驗值: {currentPlayer.currentEXP}/{currentPlayer.currentEXP}";
            statusTextsArray[5].text = $"速度: {currentPlayer.moveSpeed}";
        }
    }
    #endregion

    #region SetActiveUIPlayer(int playerID)
    //每切換當前UI角色執行，將activePlayerUIDtny中的角色物件Active
    public void SetActiveUIPlayer(int playerID) {
        foreach (var kvp in UIManager.Instance.activeUIPlayersDtny) // 遍歷所有角色 UI
        {
            bool isActive = (kvp.Key == playerID); // 只有當前角色 UI 設為 true
            kvp.Value.SetActive(isActive);
            Animator animator = kvp.Value.GetComponent<Animator>();
            if (isActive) // 如果是當前選擇的角色，播放動畫
                animator.Play(Animator.StringToHash("Attack")); // 播放指定動畫
        }
    }
    #endregion
}
