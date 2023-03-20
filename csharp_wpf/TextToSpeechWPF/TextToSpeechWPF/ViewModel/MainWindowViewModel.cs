using System;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Input;
using Microsoft.Win32;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TextToSpeechWPF.Model;

namespace TextToSpeechWPF;

internal class MainWindowViewModel : IDisposable
{
    private readonly CompositeDisposable _disposables = new();
    private readonly MainWindowModel _model;

    public MainWindowViewModel()
    {
        _model = new MainWindowModel();
        IsGenerating = _model.IsGenerating;
        InputText = _model.Text;
        SpeakingRate = _model.Rate;
        Pitch = _model.Pitch;
        SpeechCommand = IsGenerating.Inverse().ToReactiveCommand();
        SpeechCommand.Subscribe(Speech).AddTo(_disposables);
        RegisterKeyCommand.Subscribe(RegisterKey).AddTo(_disposables);

        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var sourceDir = Path.Combine(localAppData, "Text2Speech");
        Directory.CreateDirectory(sourceDir);

        Directory.GetFiles(sourceDir, "*.mp3", SearchOption.TopDirectoryOnly)
            .ToList()
            .ForEach(x => AddFilePath(x));
    }

    public ReactivePropertySlim<bool> IsGenerating { get; }
    public ReactivePropertySlim<string> InputText { get; }
    public ReactivePropertySlim<double> SpeakingRate { get; }
    public ReactivePropertySlim<double> Pitch { get; }
    public ReactiveCommand SpeechCommand { get; } = new();
    public ReactiveCommand RegisterKeyCommand { get; } = new();

    public ReactiveCollection<AudioOperationViewModel> AudioOperations { get; } = new();

    public void Dispose()
    {
        _disposables.Dispose();
    }

    private void Speech()
    {
        Mouse.OverrideCursor = Cursors.Wait;
        try
        {
            var savePath = _model.Speech();
            AddFilePath(savePath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            Mouse.OverrideCursor = default;
        }
    }

    private void RegisterKey()
    {
        var sfd = new OpenFileDialog
        {
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            Title = "GCPのText to Speech APIの権限を付与したサービスアカウントキーを選択してください。",
            RestoreDirectory = true,
            CheckPathExists = true
        };

        if (sfd.ShowDialog() ?? false)
        {
            var fileName = sfd.FileName;
            _model.SetCredentials(fileName);
        }
    }

    private void AddFilePath(string path)
    {
        AudioOperations.Add(new AudioOperationViewModel(path));
    }
}