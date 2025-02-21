using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    public enum SceneState
    { 
        Trade,
        Building,
        Map,
        DragBuilding,
        NextDayUi,

    }
    public GameObject mainUI;
    // ���ڴ洢������Ҫ�л���UI����
    [System.Serializable]
    public class UICanvas
    {
        public string canvasName;
        public GameObject canvasObject;
        public Button button; // ����Ϊÿ��UI�������ö�Ӧ�İ�ť
    }

    [Header("UI����")]
    public List<UICanvas> uiCanvases = new List<UICanvas>();
 
    private void Start()
    {
        // ȷ�� MainUI ���丸�����е����ϲ�
        mainUI.transform.SetAsLastSibling();
        // Ϊÿ����ť���õ���¼�����
        foreach (var uiCanvas in uiCanvases)
        {
            uiCanvas.button.onClick.AddListener(() => SwitchCanvas(uiCanvas));
        }

        // ��ʼʱ����Ĭ����ʾ��UI����
        SetActiveCanvas("Map"); // ����Ĭ��Ϊ Map ����
    }

    /// <summary>
    /// �л���ǰ��ʾ��UI����
    /// </summary>
    /// <param name="newCanvas">�µ�UI����</param>
    private void SwitchCanvas(UICanvas newCanvas)
    {
        foreach (var uiCanvas in uiCanvases)
        {
            // ֻ���ز���Ҫ��ʾ�Ļ���
            uiCanvas.canvasObject.SetActive(false);
        }

        // �����»���
        newCanvas.canvasObject.SetActive(true);
    }

    /// <summary>
    /// �����������õ�ǰ��ʾ��UI����
    /// </summary>
    /// <param name="canvasName">��������</param>
    public void SetActiveCanvas(string canvasName)
    {
        var selectedCanvas = uiCanvases.Find(canvas => canvas.canvasName == canvasName);
        if (selectedCanvas != null)
        {
            SwitchCanvas(selectedCanvas);
        }
        else
        {
            Debug.LogError($"Canvas with name '{canvasName}' not found.");
        }
    }
}
