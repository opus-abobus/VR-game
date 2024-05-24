namespace AppManagement.FSM.States
{
    public interface IAppState
    {
        void Enter();
        void Update();
        void Exit();
    }
}
