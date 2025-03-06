using UnityEngine;
using UnityEngine.UI;

public class MouseIconFollow : MonoBehaviour
{
    public RectTransform iconRectTransform; // ��J UI �Ϲ�
    public Canvas canvas; // ��J Canvas
    public Vector2 offset = new Vector2(10f, -10f); // �L�հ����q

    void Update() {
        if (iconRectTransform != null)
        {
            Vector2 mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out mousePosition
            );

            iconRectTransform.anchoredPosition = mousePosition + offset;
        }
    }
}
