using UnityEngine;
using System.Collections.Generic;

public class UIInputController : MonoBehaviour
{
    public static UIInputController Instance { get; private set; }
    [Header("#Esc鍵、I鍵開關的對象--------------------------------------------------------------------")]
    public GameObject menuUI; // 主選單 UI
    private Stack<GameObject> activeUIPanels = new Stack<GameObject>(); // 儲存開啟中的 UI 面板

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleMenuUI();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activeUIPanels.Count > 0)
            {
                CloseTopUIPanel();
            }
            else
            {
                ToggleMenuUI();
            }
        }
    }

    #region **🔹 UI 開啟與關閉邏輯**
    public void OpenUIPanel(GameObject panel) {
        if (panel == null)
        {
            Debug.LogError("❌ OpenUIPanel: panel 為 null，請確認 Inspector 設定！");
            return;
        }

        if (!activeUIPanels.Contains(panel)) // 避免重複加入相同 UI
        {
            activeUIPanels.Push(panel);
            panel.SetActive(true);
        }
    }

    public void CloseTopUIPanel() {
        if (activeUIPanels.Count > 0)
        {
            GameObject topPanel = activeUIPanels.Pop();
            if (topPanel != null)
            {
                topPanel.SetActive(false);
            }
        }
    }

    public void CloseAllUIPanels() {
        while (activeUIPanels.Count > 0)
        {
            GameObject panel = activeUIPanels.Pop();
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }

    private void ToggleMenuUI() {
        if (menuUI == null)
        {
            Debug.LogError("❌ ToggleMenuUI: `menuUI` 未綁定，請確認 Inspector 內有設定！");
            return;
        }

        if (activeUIPanels.Count > 0 || menuUI.activeSelf)
        {
            CloseAllUIPanels();
            menuUI.SetActive(false);
        }
        else
        {
            menuUI.SetActive(true);
        }
    }
    #endregion
}
