// Decompiled with JetBrains decompiler
// Type: Intermech.Requirement.ListExtensions
// Assembly: Intermech.Requirement, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: F81AA5A5-0C21-4456-88ED-807BD1BB2DA2
// Assembly location: D:\IPS\Client\Intermech.Requirement.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Requirement;

public static class ListExtensions
{
  public static void SortAllList(this List<NodeTreeFromWord> list)
  {
    list.Sort();
    ListExtensions.SortChild(list);
  }

  private static void SortChild(List<NodeTreeFromWord> list)
  {
    foreach (NodeTreeFromWord nodeTreeFromWord in list)
    {
      if (nodeTreeFromWord.Child.Count > 0)
      {
        nodeTreeFromWord.Child.Sort();
        ListExtensions.SortChild(nodeTreeFromWord.Child);
      }
    }
  }

  public static List<NodeTreeFromWord> RebuilList(
    this List<NodeTreeFromWord> list,
    List<NodeTreeFromWord> wordDict,
    Dictionary<NodeTreeFromWord, NodeTreeFromWord> compareNodes)
  {
    List<NodeTreeFromWord> nodeTreeFromWordList = new List<NodeTreeFromWord>();
    List<int> intList = new List<int>();
    for (int index = 0; index < list.Count; ++index)
      intList.Add(index);
    foreach (KeyValuePair<NodeTreeFromWord, NodeTreeFromWord> compareNode in compareNodes)
    {
      int num = list.IndexOf(compareNode.Value);
      intList.Remove(num);
    }
    for (int index = 0; index < list.Count; ++index)
    {
      if (!intList.Contains(index) && !list[index].IsDeleted)
        nodeTreeFromWordList.Add(list[index]);
      if (list[index].IsHaveChild)
        nodeTreeFromWordList.AddRange((IEnumerable<NodeTreeFromWord>) ListExtensions.GenerateListFromChild(list[index].Child, list[index].IsDeleted));
    }
    return nodeTreeFromWordList;
  }

  public static void RebuilList(this List<NodeTreeFromWord> list)
  {
    for (int index = 0; index < list.Count; ++index)
    {
      if (list[index].OldNode != null)
      {
        list[index].TTObjectID = list[index].OldNode.TTObjectID;
        if (list[index].Parent == null)
          list[index].TTParentID = RequirementConst.SpecificationID.ToString();
        else if (list[index].ParentName.Equals(list[index].OldNode.ParentName))
          list[index].TTParentID = list[index].OldNode.TTParentID;
      }
      if (list[index].IsHaveChild)
        ListExtensions.RebuildInChild(list[index].Child);
    }
  }

  private static void RebuildInChild(List<NodeTreeFromWord> child)
  {
    for (int index = 0; index < child.Count; ++index)
    {
      if (child[index].OldNode != null)
      {
        child[index].TTObjectID = child[index].OldNode.TTObjectID;
        if (child[index].Parent == null)
          child[index].TTParentID = RequirementConst.SpecificationID.ToString();
        else if (child[index].ParentName.Equals(child[index].OldNode.ParentName))
          child[index].TTParentID = child[index].OldNode.TTParentID;
        else if (child[index].ParentName == "TZ")
          child[index].TTParentID = child[index].Parent.TTObjectID;
        else if (child[index].Parent.TTObjectID != null)
          child[index].TTParentID = child[index].Parent.TTObjectID;
        else if (child[index].Parent.OldNode != null)
          child[index].TTParentID = child[index].Parent.OldNode.TTObjectID;
      }
      else if (child[index].Parent != null && child[index].Parent.TTObjectID != null)
        child[index].TTParentID = child[index].Parent.TTObjectID;
      if (child[index].IsHaveChild)
        ListExtensions.RebuildInChild(child[index].Child);
    }
  }

  private static List<NodeTreeFromWord> GenerateListFromChild(
    List<NodeTreeFromWord> child,
    bool parentIsDel)
  {
    List<NodeTreeFromWord> listFromChild = new List<NodeTreeFromWord>();
    for (int index = 0; index < child.Count; ++index)
    {
      if (!child[index].IsDeleted)
        listFromChild.Add(child[index]);
      if (child[index].IsHaveChild)
        listFromChild.AddRange((IEnumerable<NodeTreeFromWord>) ListExtensions.GenerateListFromChild(child[index].Child, child[index].IsDeleted));
    }
    return listFromChild;
  }

  public static string TruncateLongString(this string inputString, int maxChars, string postfix = "...")
  {
    if (maxChars - 3 <= 0)
      throw new ArgumentOutOfRangeException(nameof (maxChars));
    return inputString == null || inputString.Length < maxChars - 3 ? inputString : inputString.Substring(0, maxChars - 3) + postfix;
  }

  public static bool AnyInList(this List<NodeTreeFromWord> list, string searchName)
  {
    foreach (NodeTreeFromWord nodeTreeFromWord in list)
    {
      if (nodeTreeFromWord.Name == searchName || nodeTreeFromWord.Child.Count > 0 && ListExtensions.AnyInChildList(nodeTreeFromWord.Child, searchName))
        return true;
    }
    return false;
  }

  private static bool AnyInChildList(List<NodeTreeFromWord> list, string searchName)
  {
    foreach (NodeTreeFromWord nodeTreeFromWord in list)
    {
      if (nodeTreeFromWord.Name == searchName || nodeTreeFromWord.Child.Count > 0 && ListExtensions.AnyInChildList(nodeTreeFromWord.Child, searchName))
        return true;
    }
    return false;
  }
}
