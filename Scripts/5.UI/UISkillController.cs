using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISkillController : MonoBehaviour
{
    public Button[] skillButtons;  // 技能槽4按鈕
    public TextMeshProUGUI[] skillNames;  // 對應技能名稱的 Text
    public GameObject skillSelectionPanel;  // 已解鎖技能選擇面板
    public GameObject skillButtonPrefab;  // 技能選擇按鈕的預製體
    private PlayerStateManager.PlayerStats currentPlayer;
    private PlayerStateManager.PlayerStats.SkillData[] displayedSkills = new PlayerStateManager.PlayerStats.SkillData[4];

    private void OnEnable() {
        EventBus.Listen<UISkillChangeEvent>(OnSkillChanged);
        EventBus.Listen<UICurrentPlayerChangEvent>(OnUICurrentPlayerChanged);

        skillSelectionPanel.SetActive(false);
        // 監聽技能槽按鈕點擊事件
        for (int i = 0; i < skillButtons.Length; i++)
        {
            int slotIndex = i;
            skillButtons[i].onClick.AddListener(() => ShowAvailableSkills(slotIndex));
        }
        currentPlayer=UIManager.GetCurrentPlayer();
        RefreshSkillSlotUI(currentPlayer);

    }
    private void OnDisable() {
        EventBus.StopListen<UISkillChangeEvent>(OnSkillChanged);
        EventBus.StopListen<UICurrentPlayerChangEvent>(OnUICurrentPlayerChanged);

        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].onClick.RemoveAllListeners();
        }
    }

    private void OnSkillChanged(UISkillChangeEvent eventData) {
        displayedSkills[eventData.slotIndex] = eventData.newSkill;
        UpdateSkillButton(eventData.slotIndex, eventData.newSkill);
    }


    private void OnUICurrentPlayerChanged(UICurrentPlayerChangEvent eventData) {
        currentPlayer = eventData.currentPlayer;  
    }

    private void RefreshSkillSlotUI(PlayerStateManager.PlayerStats currentPlayer) {
        this.currentPlayer = currentPlayer;
        if (this.currentPlayer == null) return;
        for (int i = 0; i < displayedSkills.Length; i++)
        {
            displayedSkills[i] = this.currentPlayer.GetSkillAtSlot(i);
            UpdateSkillButton(i, displayedSkills[i]);
        }
    }

    private void UpdateSkillButton(int index, PlayerStateManager.PlayerStats.SkillData skill) {
        if (index < 0 || index >= skillButtons.Length || index >= skillNames.Length)
        {
            Debug.LogError($"❌ [UISkillController] 技能槽索引超出範圍: {index}");
            return;
        }
        if (skill != null)
        {
            skillNames[index].text = skill.skillName;
            skillButtons[index].interactable = true;
        }
        else
        {
            skillNames[index].text = "空";
            skillButtons[index].interactable = false;
        }
    }


    /// <summary>
    /// 顯示當前角色未裝備的可選技能
    /// </summary>
    private void ShowAvailableSkills(int slotIndex) {
        if (currentPlayer == null) return;

        // 清除舊的技能選擇按鈕
        foreach (Transform child in skillSelectionPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // 獲取當前角色的所有可用技能（已解鎖但未裝備的）
        List<PlayerStateManager.PlayerStats.SkillData> availableSkills = new List<PlayerStateManager.PlayerStats.SkillData>();

        foreach (var skillID in currentPlayer.unlockedSkillIDList)
        {
            // 確保該技能未被裝備
            if (!currentPlayer.equippedSkillIDList.Contains(skillID))
            {
                // 從 skillPoolList 中尋找對應的技能數據
                var skill = currentPlayer.skillPoolList.Find(s => s.skillID == skillID);
                if (skill != null)
                {
                    availableSkills.Add(skill);
                }
                else
                {
                    Debug.LogWarning($"[PlayerStateManager] 在 skillPoolList 找不到技能 ID: {skillID}，但它存在於 unlockedSkillIDList 中");
                }
            }
        }


        // 創建按鈕來選擇技能
        foreach (var skill in availableSkills)
        {
            GameObject skillButtonObj = Instantiate(skillButtonPrefab, skillSelectionPanel.transform);
            Button skillButton = skillButtonObj.GetComponent<Button>();
            TextMeshProUGUI skillText = skillButtonObj.GetComponentInChildren<TextMeshProUGUI>();

            if (skillText != null)
            {
                skillText.text = skill.skillName;
            }

            skillButton.onClick.AddListener(() => EquipSkill(slotIndex, skill.skillID));
        }
        if (currentPlayer.unlockedSkillIDList.Count >0)
        {
            skillSelectionPanel.SetActive(true);
        }
      

    }

    private void EquipSkill(int slotIndex, int skillID) {
        if (currentPlayer == null) return;

        EventBus.Trigger(new EquipSkillEvent(slotIndex, skillID));  // 觸發裝備事件
        skillSelectionPanel.SetActive(false);  // 關閉技能選擇面板
    }
}
