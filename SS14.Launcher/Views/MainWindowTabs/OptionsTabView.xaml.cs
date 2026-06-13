using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using ReactiveUI;
using Splat;
using SS14.Launcher.Localization;
using SS14.Launcher.Utility;
using SS14.Launcher.ViewModels.MainWindowTabs;

namespace SS14.Launcher.Views.MainWindowTabs;

public partial class OptionsTabView : UserControl
{
    public OptionsTabView()
    {
        InitializeComponent();

        Flip.Command = ReactiveCommand.Create(() =>
        {
            var window = (Window?) VisualRoot;
            if (window == null)
                return;

            window.Classes.Add("DoAFlip");

            DispatcherTimer.RunOnce(() => { window.Classes.Remove("DoAFlip"); }, TimeSpan.FromSeconds(1));
        });
    }

    public async void ClearEnginesPressed(object? _1, RoutedEventArgs _2)
    {
        ((OptionsTabViewModel)DataContext!).ClearEngines();
        await ClearEnginesButton.DisplayDoneMessage();
    }

    public async void ClearServerContentPressed(object? _1, RoutedEventArgs _2)
    {
        var blocked = !await ((OptionsTabViewModel)DataContext!).ClearServerContent();
        var locMgr = Locator.Current.GetService<LocalizationManager>()!;

        await ClearServerContentButton.DisplayDoneMessage(
            blocked ? locMgr.GetString("tab-options-clear-content-close-client") : null);
    }

    private async void OpenHubSettings(object? sender, RoutedEventArgs args)
    {
        await new HubSettingsDialog().ShowDialog((Window)this.GetVisualRoot()!);
    }

    private void UpdateUrl(string url)
    {
        AuthUrlBox.Text = url;
    }

    private void SetAuthUrl(object? sender, RoutedEventArgs e)
    {
        ((OptionsTabViewModel)DataContext!).SetAuthUrl();
    }

    private void SetFunkyAuth(object? sender, RoutedEventArgs e)
    {
        var context = (OptionsTabViewModel)DataContext!;
        context.SetAuthUrl(context.FunkyAuthUrl);
    }

    private void SetWizDenAuth(object? sender, RoutedEventArgs e)
    {
        var context = (OptionsTabViewModel)DataContext!;
        context.SetAuthUrl(context.WizDenAuthUrl);
    }
}
