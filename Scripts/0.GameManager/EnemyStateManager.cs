using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public static EnemyStateManager Instance { get; private set; }
    public readonly Dictionary<int, EnemyStats> EnemyStatesDtny = new Dictionary<int, EnemyStats>();

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

    public void SetEnemyStatesDtny(EnemyStatData EnemyStatData) {
        EnemyStatesDtny.Clear();
        foreach (var stat in EnemyStatData.EnemyStatsList)
        {
            EnemyStatesDtny[stat.EnemyID] = new EnemyStats(stat);
        }
    }

    public EnemyStats GetEnemyState(int EnemyID) {
        return EnemyStatesDtny.TryGetValue(EnemyID, out var state) ? state : null;
    }

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

        public List<SkillData> skillPoolList = new List<SkillData>();
        public List<int> unlockedSkillList = new List<int>();
        public List<int> equippedSkillList = new List<int>(new int[4] { -1, -1, -1, -1 });

        public EnemyStats(EnemyStatData.EnemyStats original) {
            EnemyID = original.EnemyID;
            EnemyName = original.EnemyName;
            level = original.level;
            maxHealth = original.maxHealth;
            attackPower = original.attackPower;
            moveSpeed = original.moveSpeed;
            moveStrategyType = original.moveStrategyType;

            EnemyPrefab = original.EnemyPrefab;
            damageTextPrefab = original.damageTextPrefab;

            // ✅ 獨立複製 skillPoolList
            skillPoolList = new List<SkillData>();
            foreach (var skill in original.skillPoolList)
            {
                skillPoolList.Add(new SkillData(skill));
            }

            unlockedSkillList = new List<int>(original.unlockedSkillList);
            equippedSkillList = new List<int>(original.equippedSkillList);
        }

        [System.Serializable]
        public class SkillData
        {
            public int skillID;
            public int currentLevel = 1;
            public List<SkillLevelData> skillLevelsDataList = new List<SkillLevelData>();

            public SkillData(EnemyStatData.EnemyStats.SkillData original) {
                skillID = original.skillID;
                currentLevel = original.currentLevel;
                skillLevelsDataList = new List<SkillLevelData>();
                foreach (var levelData in original.skillLevelsDataList)
                {
                    skillLevelsDataList.Add(new SkillLevelData(levelData));
                }
            }
        }

        [System.Serializable]
        public class SkillLevelData
        {
            public int level;
            public int attackPower;
            public float cooldownTime;
            public GameObject attackPrefab;
            public GameObject attackDetectPrefab;

            public SkillLevelData(EnemyStatData.EnemyStats.SkillLevelData original) {
                level = original.level;
                attackPower = original.attackPower;
                cooldownTime = original.cooldownTime;
                attackPrefab = original.attackPrefab;
                attackDetectPrefab = original.attackDetectPrefab;
            }
        }
    }
}

