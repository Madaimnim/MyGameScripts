using UnityEngine;
using UnityEngine.UI;

public class UIMenuController : MonoBehaviour
{
    public Button statusButton;
    public Button jobButton;
    public Button equipmentButton;
    public Button skillsButton;
    public Button formationButton;

    public GameObject statusPanel;
    public GameObject jobPanel;
    public GameObject equipmentPanel;
    public GameObject skillsPanel;
    public GameObject formationPanel;

    private void Start() {
        statusButton.onClick.AddListener(() => UIInputController.Instance.OpenUIPanel(statusPanel));
        jobButton.onClick.AddListener(() => UIInputController.Instance.OpenUIPanel(jobPanel));
        equipmentButton.onClick.AddListener(() => UIInputController.Instance.OpenUIPanel(equipmentPanel));
        skillsButton.onClick.AddListener(() => UIInputController.Instance.OpenUIPanel(skillsPanel));
        formationButton.onClick.AddListener(() => UIInputController.Instance.OpenUIPanel(formationPanel));
    }
}
