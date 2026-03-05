using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DAM.Models;

namespace DAM.Services;

public abstract class DirectorySearch
{
    private static readonly List<Node> Nodes = new();

    public static void Clear() => Nodes.Clear();

    public static void Add(Node node) => Nodes.Add(node);

    public static ObservableCollection<Node> Search(string searchText)
    {
        ObservableCollection<Node> sortedFiles = new ObservableCollection<Node>();
        
        foreach (Node node in Nodes)
        {
            if (node.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
            {
                sortedFiles.Add(node);
            }
        }
        return sortedFiles;
    }
}