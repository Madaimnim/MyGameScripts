using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIStatusController : MonoBehaviour
{
    public Image playerIconSprite; // 顯示角色圖片
    public TextMeshProUGUI[] statusTextsArray; // 角色基本狀態顯示（前 6 個）
    public Button leftButton, rightButton;

    private void Start() {
    }

    private void OnEnable() {
        EventBus.Listen<UICurrentPlayerChangEvent>(UpdatePlayerStatusUI);

        if (leftButton != null && rightButton != null)
        {
            Debug.Log("✅ [UIStatusController] 左右切換按鈕成功綁定事件");
            leftButton.onClick.AddListener(UIManager.Instance.ChangLastPlayer);
            rightButton.onClick.AddListener(UIManager.Instance.ChangNextPlayer);
        }
        else
        {
            Debug.LogError("❌ [UIStatusController] 左右按鈕未綁定！");
        }
    }

    private void OnDisable() {
        EventBus.StopListen<UICurrentPlayerChangEvent>(UpdatePlayerStatusUI);
        leftButton.onClick.RemoveListener(UIManager.Instance.ChangLastPlayer);
        rightButton.onClick.RemoveListener(UIManager.Instance.ChangNextPlayer);
    }

    private void UpdatePlayerStatusUI(UICurrentPlayerChangEvent eventData) {
        PlayerStateManager.PlayerStats currentPlayer = eventData.currentPlayer;
        Debug.Log($"🟢 [UIStatusController] 更新 UI，角色: {currentPlayer.playerName}");

        playerIconSprite.sprite = currentPlayer.spriteIcon;

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

}
