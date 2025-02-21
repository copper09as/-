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
    // 用于存储所有需要切换的UI画布
    [System.Serializable]
    public class UICanvas
    {
        public string canvasName;
        public GameObject canvasObject;
        public Button button; // 可以为每个UI画布设置对应的按钮
    }

    [Header("UI管理")]
    public List<UICanvas> uiCanvases = new List<UICanvas>();
 
    private void Start()
    {
        // 确保 MainUI 在其父物体中的最上层
        mainUI.transform.SetAsLastSibling();
        // 为每个按钮设置点击事件监听
        foreach (var uiCanvas in uiCanvases)
        {
            uiCanvas.button.onClick.AddListener(() => SwitchCanvas(uiCanvas));
        }

        // 初始时设置默认显示的UI画布
        SetActiveCanvas("Map"); // 比如默认为 Map 画布
    }

    /// <summary>
    /// 切换当前显示的UI画布
    /// </summary>
    /// <param name="newCanvas">新的UI画布</param>
    private void SwitchCanvas(UICanvas newCanvas)
    {
        foreach (var uiCanvas in uiCanvases)
        {
            // 只隐藏不需要显示的画布
            uiCanvas.canvasObject.SetActive(false);
        }

        // 启用新画布
        newCanvas.canvasObject.SetActive(true);
    }

    /// <summary>
    /// 根据名称设置当前显示的UI画布
    /// </summary>
    /// <param name="canvasName">画布名称</param>
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
