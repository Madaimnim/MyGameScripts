using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class PlayerStateManager : MonoBehaviour
{
    #region 定義
    public static PlayerStateManager Instance { get; private set; }
    public Dictionary<int, PlayerStats> playerStatesDtny = new Dictionary<int, PlayerStats>();
    public List<int> unlockedPlayerIDsList = new List<int>();
    public List<Vector2> spawnPositionsList= new List<Vector2>();

    #endregion
    #region 生命週期
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
    #endregion

    private IEnumerator Start() {
        yield return StartCoroutine(GameManager.Instance.WaitForDataReady());
        UnlockPlayer(1);
        UnlockPlayer(2);

    }

    public void UnlockPlayer(int playerID) {
        if (!unlockedPlayerIDsList.Contains(playerID))
        {
            unlockedPlayerIDsList.Add(playerID);
        }
    }

    #region SpawnPlayer(int playerID, Vector3 position, Quaternion rotation)
    public GameObject SpawnPlayer(int playerID, Vector3 position, Quaternion rotation) {
        if (!playerStatesDtny.TryGetValue(playerID, out var playerStats) || playerStats.playerPrefab == null)
        {
            Debug.LogError($"[PlayerStateManager] 無法生成玩家 {playerID}，可能是 playerPrefab 為 null");
            return null;
        }

        // 生成角色
        GameObject playerPrefab = Instantiate(playerStats.playerPrefab, position, rotation);

        // 確保角色能讀取自身的 PlayerStats
        Player player = playerPrefab.GetComponent<Player>();
        if (player != null)
        {
            player.Initialize(playerStats);
        }
        else
        {
            Debug.LogWarning($"[PlayerStateManager] 玩家 {playerID} 沒有 PlayerController，無法初始化屬性");
        }

        return playerPrefab;
    }
    #endregion

    #region SetPlayerStatesDtny(PlayerStatData playerStatData)方法
    public void SetPlayerStatesDtny(PlayerStatData playerStatData) {

        playerStatesDtny.Clear();
        foreach (var stat in playerStatData.playerStatsList)
        {
            playerStatesDtny[stat.playerID] = new PlayerStats(stat);
        }
    }
    #endregion
    #region 建構
    [System.Serializable]
    public class PlayerStats
    {
        public int playerID;
        public string playerName;
        public int level;
        public int maxHealth;
        public int attackPower;
        public float moveSpeed;
        public int currentEXP;
        public MoveStrategyType moveStrategyType;

        public int currentHealth;

        public Sprite spriteIcon;
        public GameObject playerPrefab;
        public GameObject damageTextPrefab;

        public List<SkillData> skillPoolList = new List<SkillData>();
        public List<int> unlockedSkillIDList = new List<int>();
        public List<int> equippedSkillIDList = new List<int>();

        public SkillData GetSkillAtSlot(int slotIndex) {
            if (slotIndex < 0 || slotIndex >= equippedSkillIDList.Count)
            {
                Debug.LogError($"[PlayerStateManager] 嘗試讀取未裝備的技能槽: {slotIndex}");
                return null;
            }

            int skillID = equippedSkillIDList[slotIndex];
            if (skillID == -1) return null; // -1 代表該技能槽未裝備技能

            SkillData foundSkill = skillPoolList.Find(skill => skill.skillID == skillID);

            return foundSkill;
        }


        public void SetSkillAtSlot(int slotIndex, int skillID) {
            if (slotIndex < 0 || slotIndex >= equippedSkillIDList.Count) return;

            // 確保新技能存在於技能池
            if (skillID != -1 && !skillPoolList.Exists(skill => skill.skillID == skillID))
            {
                Debug.LogWarning($"[PlayerStats] 試圖裝備未解鎖的技能 ID: {skillID}");
                return;
            }
            equippedSkillIDList[slotIndex] = skillID;
        }

        public PlayerStats(PlayerStatData.PlayerStats original) {
            playerID = original.playerID;
            playerName = original.playerName;
            level = original.level;
            maxHealth = original.maxHealth;
            attackPower = original.attackPower;
            moveSpeed = original.moveSpeed;
            moveStrategyType = original.moveStrategyType;

            spriteIcon = original.spriteIcon;
            playerPrefab = original.playerPrefab;
            damageTextPrefab = original.damageTextPrefab;

            currentHealth = maxHealth;

            skillPoolList = new List<SkillData>();
            foreach (var skill in original.skillPoolList)
            {
                skillPoolList.Add(new SkillData(skill));
            }

            unlockedSkillIDList = new List<int>(original.unlockedSkillIDList);
            equippedSkillIDList = new List<int>(original.equippedSkillIDList);
        }

        [System.Serializable]
        public class SkillData
        {
            public int skillID;
            public string skillName;
            public int currentLevel = 1;
            public List<SkillLevelData> skillLevelsDataList = new List<SkillLevelData>();

            public SkillData(PlayerStatData.PlayerStats.SkillData original) {
                skillID = original.skillID;
                skillName = original.skillName;
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

            public SkillLevelData(PlayerStatData.PlayerStats.SkillLevelData original) {
                level = original.level;
                attackPower = original.attackPower;
                cooldownTime = original.cooldownTime;
                attackPrefab = original.attackPrefab;
                attackDetectPrefab = original.attackDetectPrefab;
            }


        }
    }
    #endregion
}

