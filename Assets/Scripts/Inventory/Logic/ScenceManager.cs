using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : Singleton<UIManager>
{
    public SceneState CurrentState;
    public void SwitchSceneState(SceneState enterState)
    {
        Debug.Log("transSuccess");
        if(CurrentState !=null)
        CurrentState.Exit();
        enterState.Enter();
    }
}
