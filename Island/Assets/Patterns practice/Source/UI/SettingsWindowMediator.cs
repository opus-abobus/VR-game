using UI.SettingsManagement;
using UI.WindowsManagement;

public interface IMediator
{
    void Notyfy(object sender, string eventValue);
}

public interface IColleague
{
    void SetMediator(IMediator mediator);
}

public class SettingsWindowMediator : IMediator
{
    private SettingsController _settingsController;
    private WindowsManager _windowsManager;

    public SettingsWindowMediator(SettingsController settingsController, WindowsManager windowsManager)
    {
        _settingsController = settingsController;
        _settingsController.SetMediator(this);
        _windowsManager = windowsManager;
        _windowsManager.SetMediator(this);
    }

#pragma warning disable CS0252
    void IMediator.Notyfy(object sender, string eventValue)
    {
        switch (eventValue)
        {
            case "CloseSettingsAttempt":
                {
                    if (sender == _windowsManager && _settingsController.HasUnsavedData)
                    {
                        _windowsManager.OpenUnsavedChangesInSettings();

                        _settingsController.ShowCommandPanel(false);
                    }
                    break;
                }
            case "SaveSettingsDataInModal":
                {
                    if (sender == _windowsManager)
                    {
                        _settingsController.ApplySettings();

                        _windowsManager.GoHome();
                    }
                    break;
                }
            case "DiscardSettingsDataInModal":
                {
                    if (sender == _windowsManager)
                    {
                        _settingsController.DiscardSettings();

                        _windowsManager.GoHome();
                    }
                    break;
                }
        }
    }
}
