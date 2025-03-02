using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    public SceneState CurrentState;
    public SceneState defaultState; // 添加一个默认状态

    private void Start()
    {
        // 在Awake时初始化默认状态
        if (defaultState != null)
        {
            SwitchSceneState(defaultState);
        }
    }

    public void SwitchSceneState(SceneState enterState)
    {
        Debug.Log("transSuccess");
        if (CurrentState != null)
            CurrentState.Exit();

        CurrentState = enterState; // 更新当前状态
        enterState.Enter(); // 进入新的状态
    }
}
