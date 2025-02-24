using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingState : SceneState
{
    public override void Enter()
    {
        if (!isLoad)
        {
            SaveManager.Instance.LoadBuilding();
            isLoad = true;
        }
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

}
