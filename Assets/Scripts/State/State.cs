using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{

    public abstract UIManager.SceneState sceneState { get; }
    public abstract void TransEffect();

}
