using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private int baseSortingOrder = 1000; // ��¦�ƧǭȡA�T�O���|�ܦ��t��
    private float lastYPosition = 0f; // �O���W�@���� Y �y��

    #region Awake()��k
    void Awake() {
        UpdateSortingOrder();
    }
    #endregion
    #region Start()��k
    void Start() {
        UpdateSortingOrder();
    }
    #endregion
    #region LateUpdate()��k
    void LateUpdate() {
        if (Mathf.Abs(transform.position.y - lastYPosition) > 0.01f)    //�� Y �y���ܤƤ~��sOrder in Layer�A���ɮį�
        {
            UpdateSortingOrder();
        }
    }
    #endregion

    #region �p��UpdateSortingOrder()��k
    private void UpdateSortingOrder() {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = baseSortingOrder - Mathf.RoundToInt(transform.position.y * 100);
        }
        lastYPosition = transform.position.y;
    }
    #endregion
}
