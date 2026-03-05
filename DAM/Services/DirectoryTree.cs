using System;
using System.Collections.Generic;
using DAM.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;


namespace DAM.Services;

public class DirectoryTree
{
    private readonly string _rootPath = $"{Directory.GetCurrentDirectory()}/assets";
    public ObservableCollection<Node> GetFileTreeDirectories()
    {
         //TODO Change on windows to \\

        string[] directories = Directory.GetDirectories(_rootPath, "*", SearchOption.TopDirectoryOnly);
            
        ObservableCollection<Node> directoryCollection = new ObservableCollection<Node>();
        
        foreach (string path in directories)
        {
            directoryCollection.Add(new Node(path, Path.GetFileName(path)));
        }

        return directoryCollection;
    }
}