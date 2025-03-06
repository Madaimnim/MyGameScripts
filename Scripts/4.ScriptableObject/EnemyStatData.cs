using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyStatData", menuName = "GameData/EnemyStatData")]
public class EnemyStatData : ScriptableObject
{
    #region 🔹 角色數據列表
    [Header("所有玩家數據列表")]
    public List<EnemyStats> EnemyStatsList = new List<EnemyStats>();
    #endregion

    [System.Serializable]
    public class EnemyStats
    {
        public int EnemyID;
        public string EnemyName;
        public int level;
        public int maxHealth;
        public int attackPower;
        public float moveSpeed;
        public MoveStrategyType moveStrategyType;

        public GameObject EnemyPrefab;
        public GameObject damageTextPrefab;

        public List<SkillData> skillPoolList = new List<SkillData>();  //  存放角色的技能數據
        public List<int> unlockedSkillList = new List<int>();          //  已解鎖的技能 ID
        public List<int> equippedSkillList = new List<int>(new int[4] { -1, -1, -1, -1 }); // ✅ 技能槽

        // **技能數據類**
        [System.Serializable]
        public class SkillData
        {
            public int skillID;
            public int currentLevel = 1;
            public List<SkillLevelData> skillLevelsDataList = new List<SkillLevelData>();
        }

        // ✅ **技能等級數據類**
        [System.Serializable]
        public class SkillLevelData
        {
            public int level;
            public int attackPower;
            public float cooldownTime;
            public GameObject attackPrefab;
            public GameObject attackDetectPrefab;
        }
    }
}