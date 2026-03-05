using System;
using System.Collections.ObjectModel;
using System.Reactive;
using DAM.Services;
using DAM.ViewModels;
using ReactiveUI;

namespace DAM.Models
{
    public class Node : ReactiveObject
    {
        public string Path { get; }
        public string Name { get; }
        

        public Node(string path, string name)
        {
            DirectorySearch.Add(this);
            Path = path;
            Name = name;
        }
    }
}