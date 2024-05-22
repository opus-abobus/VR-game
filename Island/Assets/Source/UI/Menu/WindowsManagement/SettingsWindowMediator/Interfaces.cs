namespace UI.WindowsManagement
{
    public interface IMediator
    {
        void Notyfy(object sender, string eventValue);
    }

    public interface IColleague
    {
        void SetMediator(IMediator mediator);
    }
}
