// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.IDAttributesNodes
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class IDAttributesNodes
{
  private ObjectIDAttributesNode _objectIDAttributesNode;
  private RelationIDAttributesNode _relationIDAttributesNode;

  public IDAttributesNodes(ICategoryTypeIconService iconService)
  {
    this._objectIDAttributesNode = new ObjectIDAttributesNode(iconService);
    this._relationIDAttributesNode = new RelationIDAttributesNode(iconService);
  }

  public void RefreshNode(TreeNode node, CompoitionSettings settings)
  {
    switch ((RootNodeTypes) Convert.ToInt32(node.Tag))
    {
      case RootNodeTypes.ObjectTypesList:
        this._objectIDAttributesNode.RefreshNode(node, settings.ObjectIDAttributes);
        break;
      case RootNodeTypes.RelationTypesList:
        this._relationIDAttributesNode.RefreshNode(node, settings.RelationIDAttributes);
        break;
    }
  }

  public bool IsSettingsNode(TreeNode node)
  {
    return this._objectIDAttributesNode.IsChildNode(node) || this._relationIDAttributesNode.IsChildNode(node);
  }

  public List<int> GetAttributesForNode(TreeNode node)
  {
    if (this._objectIDAttributesNode.IsChildNode(node))
      return this._objectIDAttributesNode.GetAttributes(node);
    return this._relationIDAttributesNode.IsChildNode(node) ? this._relationIDAttributesNode.GetAttributes(node) : (List<int>) null;
  }

  public List<int> AddAttribute(TreeNode node, CompoitionSettings settings)
  {
    List<int> intList1 = new List<int>();
    if (this._objectIDAttributesNode.IsChildNode(node))
    {
      List<int> intList2 = this._objectIDAttributesNode.AddAttribute(node);
      if (intList2 == null)
        return (List<int>) null;
      int objectType = this._objectIDAttributesNode.GetObjectType(node);
      foreach (int attributeID in intList2)
      {
        if (settings.AddObjectIDAttribute(objectType, attributeID))
          intList1.Add(attributeID);
      }
    }
    else if (this._relationIDAttributesNode.IsChildNode(node))
    {
      List<int> intList3 = this._relationIDAttributesNode.AddAttribute(node);
      if (intList3 == null)
        return (List<int>) null;
      int parentTypeID;
      int relationTypeID;
      this._relationIDAttributesNode.GetIDs(node, out parentTypeID, out relationTypeID);
      foreach (int attributeID in intList3)
      {
        if (settings.AddRelationIDAttribute(parentTypeID, relationTypeID, attributeID))
          intList1.Add(attributeID);
      }
    }
    return intList1;
  }

  public bool RemoveAttribute(TreeNode node, int attributeID, CompoitionSettings settings)
  {
    if (this._objectIDAttributesNode.IsChildNode(node))
    {
      int objectType = this._objectIDAttributesNode.GetObjectType(node);
      settings.RemoveObjectIDAttribute(objectType, attributeID);
      return true;
    }
    if (!this._relationIDAttributesNode.IsChildNode(node))
      return false;
    int parentTypeID;
    int relationTypeID;
    this._relationIDAttributesNode.GetIDs(node, out parentTypeID, out relationTypeID);
    settings.RemoveRelationIDAttribute(parentTypeID, relationTypeID, attributeID);
    return true;
  }
}
