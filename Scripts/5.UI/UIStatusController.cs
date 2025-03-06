using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIStatusController : MonoBehaviour
{
    public Image playerIconSprite; // 顯示角色圖片
    public TextMeshProUGUI[] statusTextsArray; // 角色基本狀態顯示（前 6 個）
    public Button leftButton, rightButton;

    private PlayerStateManager.PlayerStats currentPlayer;

    private void Start() {
    }

    private void OnEnable() {
        EventBus.Listen<UICurrentPlayerChangEvent>(OnUICurrentPlayerChanged);

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
        RefreshPlayerStatusUI();
    }

    private void OnDisable() {
        EventBus.StopListen<UICurrentPlayerChangEvent>(OnUICurrentPlayerChanged);

        leftButton.onClick.RemoveListener(UIManager.Instance.ChangLastPlayer);
        rightButton.onClick.RemoveListener(UIManager.Instance.ChangNextPlayer);
    }


    private void OnUICurrentPlayerChanged(UICurrentPlayerChangEvent eventData) {
        currentPlayer = eventData.currentPlayer;
        RefreshPlayerStatusUI();
    }
    private void RefreshPlayerStatusUI() {
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
