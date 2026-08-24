// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.NodeTreeFromWord
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Requirement;

public class NodeTreeFromWord : IComparable<NodeTreeFromWord>
{
  public bool IsDeleted;

  public NodeTreeFromWord() => this.IsChecked = true;

  public bool IsChecked { get; set; }

  public string Name { get; set; }

  public string ParentName => this.Parent != null ? this.Parent.Name : "TZ";

  public bool IsHaveChild => this.Child.Count > 0;

  public int IconIndex { get; set; }

  public string TTLines { get; set; }

  public string TTDescription { get; set; }

  public string TTParentID { get; set; }

  public string TTLevel { get; set; }

  public int TTLCStep { get; set; }

  public string TTObjectID { get; set; }

  public string TTLevelHierarhi { get; set; }

  public string TTIndexInDocument { get; set; }

  public List<NodeTreeFromWord> Child { get; set; }

  public bool IsNew => this.OldNode == null;

  public NodeTreeFromWord OldNode { get; set; }

  public NodeTreeFromWord Parent { get; set; }

  public NodeTreeFromWord Find(string ttName, string parentName)
  {
    if (this.Name == ttName && this.ParentName == parentName)
      return this;
    return this.Child.Count > 0 ? this.FindInChild(this.Child, ttName, parentName) : (NodeTreeFromWord) null;
  }

  public NodeTreeFromWord Find(string ttName, int ttEntry, int srch = 0)
  {
    if (this.Name == ttName)
    {
      if (srch == ttEntry || ttEntry == -1)
        return this;
      ++srch;
    }
    return this.Child.Count > 0 ? this.FindInChild(this.Child, ttName, ttEntry, ref srch) : (NodeTreeFromWord) null;
  }

  private NodeTreeFromWord FindInChild(
    List<NodeTreeFromWord> child,
    string ttName,
    string parentName)
  {
    foreach (NodeTreeFromWord inChild1 in child)
    {
      if (inChild1.Name == ttName && inChild1.ParentName == parentName)
        return inChild1;
      if (inChild1.Child.Count > 0)
      {
        NodeTreeFromWord inChild2 = this.FindInChild(inChild1.Child, ttName, parentName);
        if (inChild2 != null)
          return inChild2;
      }
    }
    return (NodeTreeFromWord) null;
  }

  private NodeTreeFromWord FindInChild(
    List<NodeTreeFromWord> child,
    string ttName,
    int ttEntry,
    ref int srch)
  {
    for (int index = 0; index < child.Count; ++index)
    {
      if (child[index].Name == ttName)
      {
        if (srch == ttEntry)
          return child[index];
        ++srch;
      }
      if (child[index].Child.Count > 0)
      {
        NodeTreeFromWord inChild = this.FindInChild(child[index].Child, ttName, ttEntry, ref srch);
        if (inChild != null)
          return inChild;
      }
    }
    return (NodeTreeFromWord) null;
  }

  private List<NodeTreeFromWord> FindAndReplaceInChild(
    List<NodeTreeFromWord> child,
    string ttName,
    int ttEntry,
    string newName,
    ref int searchs,
    string indexHierarhi = "")
  {
    for (int index = 0; index < child.Count; ++index)
    {
      if (child[index].Name == ttName)
      {
        if (searchs == ttEntry)
        {
          child[index].Name = newName;
          if (!string.IsNullOrEmpty(indexHierarhi))
            child[index].TTLevelHierarhi = indexHierarhi;
          return child;
        }
        ++searchs;
      }
      if (child[index].Child.Count > 0)
      {
        child[index].Child = this.FindAndReplaceInChild(child[index].Child, ttName, ttEntry, newName, ref searchs, indexHierarhi);
        return child;
      }
    }
    return child;
  }

  public NodeTreeFromWord FindAndReplace(
    string ttName,
    int ttEntry,
    string newName,
    string indexHierarhi = "",
    int srch = 0)
  {
    if (this.Name == ttName)
    {
      if (srch == ttEntry || ttEntry == -1)
      {
        this.Name = newName;
        if (!string.IsNullOrEmpty(indexHierarhi))
          this.TTLevelHierarhi = indexHierarhi;
        return this;
      }
      ++srch;
    }
    if (this.Child.Count <= 0)
      return (NodeTreeFromWord) null;
    this.Child = this.FindAndReplaceInChild(this.Child, ttName, ttEntry, newName, ref srch, indexHierarhi);
    return this;
  }

  public int CompareTo(NodeTreeFromWord other)
  {
    string[] strArray1 = this.TTLevelHierarhi.Split(new string[1]
    {
      "."
    }, StringSplitOptions.RemoveEmptyEntries);
    string[] strArray2 = other.TTLevelHierarhi.Split(new string[1]
    {
      "."
    }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray1.Length != strArray2.Length)
    {
      if (strArray1.Length > strArray2.Length)
        return -1;
      return strArray1.Length < strArray2.Length ? 1 : 0;
    }
    int int32_1 = Convert.ToInt32(strArray1[strArray1.Length - 1]);
    int int32_2 = Convert.ToInt32(strArray2[strArray2.Length - 1]);
    if (int32_1 > int32_2)
      return 1;
    return int32_1 < int32_2 ? -1 : 0;
  }
}
