using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    //[Header("玩家 ID")]
    //public Player player;
    //
    //public event Action<int, PlayerSkillData.PlayerSkillStats> OnSkillSlotUpdated;
    //
    //private void Start() {
    //    InitializeSkills();
    //}
    //
    //private void InitializeSkills() {
    //    for (int i = 0; i < 4; i++)
    //    {
    //        PlayerSkillData.PlayerSkillStats skillStats = PlayerStateManager.Instance.GetSkillStats(player.playerID, i);
    //        OnSkillSlotUpdated?.Invoke(i, skillStats);
    //    }
    //}
    //
    //public void SetSkillSlot(int slotIndex, int skillID) {
    //    PlayerStateManager.Instance.SetSkillSlot(player.playerID, slotIndex, skillID);
    //    PlayerSkillData.PlayerSkillStats skillStats = PlayerStateManager.Instance.GetSkillStats(player.playerID, slotIndex);
    //    OnSkillSlotUpdated?.Invoke(slotIndex, skillStats);
    //}
    ////Todo
    ////public int GetSkillID(int slotIndex) {
    ////    return PlayerStateManager.Instance.GetSkillID(player.playerID, slotIndex);
    ////}
    //
    //public PlayerSkillData.PlayerSkillStats GetSkillStats(int slotIndex) {
    //    return PlayerStateManager.Instance.GetSkillStats(player.playerID, slotIndex);
    //}
    ////Todo
    ////public List<PlayerSkillData.PlayerSkillStats> GetAllEquippedSkills() {
    ////    return PlayerStateManager.Instance.GetAllEquippedSkills(player.playerID);
    ////}
}
