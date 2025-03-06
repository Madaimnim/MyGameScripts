using UnityEngine;

public class SkillSlotManager : MonoBehaviour
{
    private PlayerStateManager.PlayerStats currentPlayer;

    private void OnEnable() {
        EventBus.Listen<UICurrentPlayerChangEvent>(OnCurrentPlayerChanged);
        EventBus.Listen<EquipSkillEvent>(OnEquipSkill);
    }

    private void OnDisable() {
        EventBus.StopListen<UICurrentPlayerChangEvent>(OnCurrentPlayerChanged);
        EventBus.StopListen<EquipSkillEvent>(OnEquipSkill);
    }

    private void OnCurrentPlayerChanged(UICurrentPlayerChangEvent eventData) {
        currentPlayer = eventData.currentPlayer;

        Debug.Log($"🟢 [SkillSlotManager] 角色變更: {currentPlayer.playerName} (ID: {currentPlayer.playerID})");

        LoadEquippedSkills();
    }


    private void LoadEquippedSkills() {
        if (currentPlayer == null)
        {
            Debug.LogError("❌ [SkillSlotManager] currentPlayer 為 null，無法加載技能！");
            return;
        }

        Debug.Log($"🟠 [SkillSlotManager] 加載 {currentPlayer.playerName} 的技能");

        Debug.Log("🔍 [SkillSlotManager] equippedSkillIDList: " + string.Join(", ", currentPlayer.equippedSkillIDList));

        for (int i = 0; i < currentPlayer.equippedSkillIDList.Count; i++)
        {
            int skillID = currentPlayer.equippedSkillIDList[i];
            Debug.Log($"  🔹 嘗試讀取技能槽 {i}，對應的技能 ID: {skillID}");

            var skill = currentPlayer.GetSkillAtSlot(i);
            Debug.Log($"  🔹 技能槽 {i}: {(skill != null ? skill.skillName : "空")}");

            EventBus.Trigger(new UISkillChangeEvent(i, skill));
        }
    }



    private void OnEquipSkill(EquipSkillEvent eventData) {
        if (currentPlayer == null) return;

        currentPlayer.SetSkillAtSlot(eventData.slotIndex, eventData.skillID);
        var newSkill = currentPlayer.GetSkillAtSlot(eventData.slotIndex);

        EventBus.Trigger(new UISkillChangeEvent(eventData.slotIndex, newSkill));
    }
}
