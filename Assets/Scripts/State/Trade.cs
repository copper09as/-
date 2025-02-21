public class Trade : State
{
    public override UIManager.SceneState sceneState
    {
        get
        {
            return UIManager.SceneState.Trade;


        }
    }

    public override void TransEffect()
    {
        throw new System.NotImplementedException();
    }
}
