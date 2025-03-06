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
        EventBus.Listen<UICurrentPlayerChangEvent>(OnCurrentPlayerChanged);

        // 監聽技能槽按鈕點擊事件
        for (int i = 0; i < skillButtons.Length; i++)
        {
            int slotIndex = i;
            skillButtons[i].onClick.AddListener(() => ShowAvailableSkills(slotIndex));
        }
    }

    private void OnDisable() {
        EventBus.StopListen<UISkillChangeEvent>(OnSkillChanged);
        EventBus.StopListen<UICurrentPlayerChangEvent>(OnCurrentPlayerChanged);

        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].onClick.RemoveAllListeners();
        }
    }

    private void OnCurrentPlayerChanged(UICurrentPlayerChangEvent e) {
        currentPlayer = e.currentPlayer;
        RefreshUI();
    }

    private void OnSkillChanged(UISkillChangeEvent eventData) {
        Debug.Log($"🔵 [UISkillController] 技能變更: 槽 {eventData.slotIndex} -> {(eventData.newSkill != null ? eventData.newSkill.skillName : "空")}");

        displayedSkills[eventData.slotIndex] = eventData.newSkill;
        UpdateSkillButton(eventData.slotIndex, eventData.newSkill);
    }


    private void RefreshUI() {
        if (currentPlayer == null) return;

        for (int i = 0; i < displayedSkills.Length; i++)
        {
            displayedSkills[i] = currentPlayer.GetSkillAtSlot(i);
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
            Debug.Log($"✅ [UISkillController] 更新技能槽 {index}: {skill.skillName}");
        }
        else
        {
            skillNames[index].text = "空";
            skillButtons[index].interactable = false;
            Debug.Log($"❌ [UISkillController] 技能槽 {index} 為空");
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

        // 獲取當前角色的所有可用技能（排除已裝備的）
        List<PlayerStateManager.PlayerStats.SkillData> availableSkills = new List<PlayerStateManager.PlayerStats.SkillData>();
        foreach (var skill in currentPlayer.skillPoolList)
        {
            if (!currentPlayer.equippedSkillIDList.Contains(skill.skillID))
            {
                availableSkills.Add(skill);
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

        skillSelectionPanel.SetActive(true);
    }

    private void EquipSkill(int slotIndex, int skillID) {
        if (currentPlayer == null) return;

        EventBus.Trigger(new EquipSkillEvent(slotIndex, skillID));  // 觸發裝備事件
        skillSelectionPanel.SetActive(false);  // 關閉技能選擇面板
    }
}
