using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    public GameState currentState = GameState.GameStart;
    #region Enum �w�q
    public enum GameState
    {
        GameStart,       // �T�ΩҦ�����]�C���}�l�ʵe�ιL���^
        Battle,
        BattlePause,       // �Ȱ����q
        BattleResult,    // �԰��������q
        Preparation,     // �ǳƶ��q
        EndGame          // �C���������q
    }
    #endregion
    #region ��ҼҦ�
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �T�O������s��
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region �]�w���A
    public void SetState(GameState newState) {
        if (currentState == newState) return; // �קK���Ƴ]�m�ۦP���A

        Debug.Log($"[GameStateManager] ���A�ܧ�: {currentState} -> {newState}");

        //�����}��e���A�A�A�i�J�U�Ӫ��A
        ExitState(currentState);
        EnterState(newState);
        currentState = newState;
    }
    #endregion

    #region �U���A��������k
    //�i�X�C���}�l
    private void HandleGameStart() {
        GameSceneManager.Instance.LoadSceneGameStart();
        GameSceneManager.Instance.GameStartButton.gameObject.SetActive(true);   
        UIInputController.Instance.isInputEnabled = false;
    }
    private void ExitGameStart() {
        GameSceneManager.Instance.GameStartButton.interactable=false;
    }

    //�i�X�ǳ�
    private void HandlePreparation() {
        PlayerStateManager.Instance.UnlockAndSpawnPlayer(1);
        PlayerStateManager.Instance.UnlockAndSpawnPlayer(2);

        GameSceneManager.Instance.LoadScenePreparation();
        GameSceneManager.Instance.GameStartButton.gameObject.SetActive(false);
        PlayerStateManager.Instance.DeactivateAllPlayer();

        UIInputController.Instance.isInputEnabled = true;
    }
    private void ExitPreparation() {
        UIInputController.Instance.isInputEnabled = false;
        UIManager.Instance.CloseAllUIPanels();
    }

    //�i�X�԰�
    private void HandleBattle() {
        GameSceneManager.Instance.LoadSceneBattle();

    }
    private void ExitBattle() {

    }

    //�i�X�԰��Ȱ�
    private void HandleBattlePause() {
        Time.timeScale = 0f;
    }
    private void ExitBattlePause() {
        Time.timeScale = 1f;
    }

    //�i�X�԰����G
    private void HandleBattleResult() {
    }
    private void ExitBattleResult() {
    }

    //�i�X�C������
    private void HandleEndGame() {
        GameSceneManager.Instance.LoadSceneGameStart();
    }
    private void ExitEndGame() {
    }
    #endregion

    #region ���A�B�z�޿�
    private void EnterState(GameState state) {
        switch (state)
        {
            case GameState.GameStart:
                HandleGameStart();
                break;

            case GameState.Preparation:
                HandlePreparation();
                break;

            case GameState.Battle:
                HandleBattle();
                break;

            case GameState.BattlePause:
                HandleBattlePause();
                break;

            case GameState.BattleResult:
                HandleBattleResult();
                break;

            case GameState.EndGame:
                HandleEndGame();
                break;
        }
    }
    private void ExitState(GameState state) {
        switch (state)
        {
            case GameState.GameStart:
                ExitGameStart();
                break;

            case GameState.Preparation:
                ExitPreparation();
                break;

            case GameState.Battle:
                ExitBattle();
                break;

            case GameState.BattlePause:
                ExitBattlePause();
                break;

            case GameState.BattleResult:
                ExitBattleResult();
                break;

            case GameState.EndGame:
                ExitEndGame();
                break;
        }
    }
    #endregion
}
