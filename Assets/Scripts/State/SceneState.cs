
using UnityEngine;

public abstract class SceneState :MonoBehaviour
{
    public static SceneState Instance;
    public UICanvas uICanvas;
    [SerializeField]protected bool isLoad;
    public virtual void Enter()
    {
        uICanvas.canvasObject.SetActive(true);
        UIManager.Instance.CurrentState = this;
    }
    public virtual void Exit()
    {
        uICanvas.canvasObject.SetActive(false);
        if(UIManager.Instance.CurrentState != null)
        UIManager.Instance.CurrentState = null;
    }
    private void SwitchSceneState()
    {
        UIManager.Instance.SwitchSceneState(this);
    }
    private void Awake()
    {
        Instance = this;
        uICanvas.canvasObject = gameObject;
        uICanvas.canvasName = gameObject.name;
        uICanvas.canvasObject.transform.SetAsLastSibling();
    }
    private void Start()
    {
        uICanvas.button.onClick.AddListener(SwitchSceneState);
        gameObject.SetActive(false);
    }
}
