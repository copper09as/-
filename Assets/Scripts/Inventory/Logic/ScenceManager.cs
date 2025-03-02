using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    public SceneState CurrentState;
    public SceneState defaultState; // ���һ��Ĭ��״̬

    private void Start()
    {
        // ��Awakeʱ��ʼ��Ĭ��״̬
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

        CurrentState = enterState; // ���µ�ǰ״̬
        enterState.Enter(); // �����µ�״̬
    }
}
