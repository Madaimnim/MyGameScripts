using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }      //單例
    public GameObject menuUIPanel;
    public GameObject statusUIPanel;
    public GameObject jobUIPanel;
    public GameObject equipmentUIPanel;
    public GameObject skillsUIPanel;
    public GameObject formationUIPanel;


    public UISkillController uiSkillController;
    public Dictionary<int, GameObject> activeUIPlayersDtny = new Dictionary<int, GameObject>();
    public Stack<GameObject> activeUIPanelsStack = new Stack<GameObject>(); // 儲存開啟中的 UI 面板

    #region 角色管理
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

    private void OnEnable() {
        CloseAllUIPanels();
    }

    private IEnumerator Start() {
        yield return StartCoroutine(GameManager.Instance.WaitForDataReady());
        UpdateAllUI(); // ✅ 初始化 UI
    }

    //供Button訂閱傳入「string 動畫名稱」，執行當前角色的動畫撥放
    public void PlayUIAttackAnimation(string animationName) {
        activeUIPlayersDtny[currentPlayer.playerID].GetComponent<Animator>().Play(Animator.StringToHash(animationName));
    }


    public void OpenUIPanel(GameObject panel) {
        if (panel == null)
        {
            Debug.LogError("❌ OpenUIPanel: panel 為 null，請確認 Inspector 設定！");
            return;
        }

        if (activeUIPanelsStack.Count > 0 && activeUIPanelsStack.Contains(panel))
        {
            Debug.LogWarning($"⚠️ {panel.name} 已經在堆疊中，不重複添加！");
            return;
        }
        activeUIPanelsStack.Push(panel);
        panel.SetActive(true);
    }

    public void CloseTopUIPanel() {
        if (activeUIPanelsStack.Count == 0)
        {
            Debug.LogWarning("⚠️ 嘗試關閉 UI 但 Stack 為空！");
            return;
        }
        GameObject topPanel = activeUIPanelsStack.Pop();
        if (topPanel != null)
            topPanel.SetActive(false);
    }

    public void CloseAllUIPanels() {
        menuUIPanel.SetActive(false);
        statusUIPanel.SetActive(false);
        //jobUIPanel.SetActive(false);
        //equipmentUIPanel.SetActive(false);
        //skillsUIPanel.SetActive(false);
        //formationUIPanel.SetActive(false);
        activeUIPanelsStack.Clear();
    }
    #endregion
    public void UpdateAllUI() {
        var unlockedPlayerIDs = PlayerStateManager.Instance.unlockedPlayerIDsHashSet;

        if (unlockedPlayerIDs.Count == 0)
        {
            Debug.LogError("❌ [UIManager] 沒有已解鎖的角色");
            return;
        }

        // 確保 currentIndex 在合理範圍
        currentIndex = Mathf.Clamp(currentIndex, 0, unlockedPlayerIDs.Count - 1);

        int playerID = unlockedPlayerIDs.ElementAt(currentIndex);
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
        var unlockedPlayerIDs = PlayerStateManager.Instance.unlockedPlayerIDsHashSet;

        if (unlockedPlayerIDs.Count == 0) return;

        currentIndex = (currentIndex - 1 + unlockedPlayerIDs.Count) % unlockedPlayerIDs.Count;
        UpdateAllUI();

        uiSkillController.skillSelectionPanel.SetActive(false);
    }

    public void ChangNextPlayer() {
        var unlockedPlayerIDs = PlayerStateManager.Instance.unlockedPlayerIDsHashSet;

        if (unlockedPlayerIDs.Count == 0) return;

        currentIndex = (currentIndex + 1) % unlockedPlayerIDs.Count;
        UpdateAllUI();

        uiSkillController.skillSelectionPanel.SetActive(false);
    }

    public void SetCurrentCharacter(int newIndex) {
        var unlockedPlayerIDs = PlayerStateManager.Instance.unlockedPlayerIDsHashSet;

        if (unlockedPlayerIDs.Count == 0) return;
        if (newIndex < 0 || newIndex >= unlockedPlayerIDs.Count) return;

        currentIndex = newIndex;
        UpdateAllUI();
    }

    #endregion


}
