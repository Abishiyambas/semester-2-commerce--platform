using System;
using System.Collections.Generic;
using DAM.Models;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using DAM.Services;
using ReactiveUI;

namespace DAM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IList<Node> _nodes;

        private readonly DirectoryTree _directoryTree = new();
        
        private string _inputText;

        private readonly Uri _mp4Icon = new("avares://DAM/Assets/mp4.png");
        private readonly Uri _documentIcon = new("avares://DAM/Assets/document.png");
        
        
        public static ReactiveCommand<string, Unit>? LoadAssetsCommand { get; set; }
        
        private List<ImageNode> _imagePaths = new();
        public List<ImageNode> ImagePaths { 
            get => _imagePaths; 
            set => this.RaiseAndSetIfChanged(ref _imagePaths, value);
        }
        
        public IList<Node> Nodes
        {
            get => _nodes;
            set => this.RaiseAndSetIfChanged(ref _nodes, value);
        }
        
        public string InputText
        {
            get => _inputText;
            set
            {
                if (_inputText != value)
                {
                    _inputText = value;
                    OnPropertyChanged(nameof(InputText));
                    if (InputText.Length <= 0)
                    {
                        DirectorySearch.Clear();
                        Nodes = _directoryTree.GetFileTreeDirectories();
                    }
                    else
                    {
                        Nodes = DirectorySearch.Search(InputText);
                    }   
                }
            }
        }

        public MainViewModel()
        {
            Nodes = _directoryTree.GetFileTreeDirectories();
            LoadAssets("");
            LoadAssetsCommand = ReactiveCommand.Create<string>(param =>
            {
                LoadAssets($"{param}");
            });
        }
        
        public void LoadAssets(string path)
        {
            var folder = Path.Combine(AppContext.BaseDirectory, $"assets/{path}");
            
            if (Directory.Exists(folder))
            {
                var files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(f => 
                        f.EndsWith(".jpg") ||
                        f.EndsWith(".png") ||
                        f.EndsWith(".pdf") ||
                        f.EndsWith(".mp4"));

                List<ImageNode> imageList = new();

                foreach (string file in files)
                {
                    try
                    {
                        string name = Path.GetFileNameWithoutExtension(file);
                        switch (file)
                        {
                            case { } s when s.EndsWith(".pdf"):
                                imageList.Add(new ImageNode(name, new Bitmap(AssetLoader.Open(_documentIcon))));
                                break;
                            case { } s when s.EndsWith(".mp4"):
                                imageList.Add(new ImageNode(name, new Bitmap(AssetLoader.Open(_mp4Icon))));
                                break;
                            default:
                                imageList.Add(new ImageNode(name, new Bitmap(file)));
                                break;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading {file}: {ex.Message}");
                    }
                }
                ImagePaths = imageList;
            }
        }
        
        public new event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}