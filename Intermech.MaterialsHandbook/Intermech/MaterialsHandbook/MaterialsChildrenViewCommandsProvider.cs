// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.MaterialsChildrenViewCommandsProvider
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Interfaces.MaterialsHandbook;
using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Controls;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.MaterialsHandbook;

public sealed class MaterialsChildrenViewCommandsProvider : ICommandsProvider
{
  private ChildrenViewCommandsProvider _provider;

  public MaterialsChildrenViewCommandsProvider(ChildrenView childrenView)
  {
    this._provider = childrenView != null ? new ChildrenViewCommandsProvider(childrenView) : throw new ArgumentNullException(nameof (childrenView));
  }

  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return this._provider.GetMergedCommands(items, viewServices);
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo groupCommands = this._provider.GetGroupCommands(items, viewServices);
    if (items.GetItemData(0, typeof (IIMHNode)) is IIMHNode itemData)
    {
      bool flag = false;
      int parentCategoryId = itemData.ParentCategoryID;
      if (IMHHelper.ChildNodesColl.ContainsKey(parentCategoryId))
        flag = true;
      else if (parentCategoryId == Consts.IMHMaterialsNodeCategoryID || parentCategoryId == Consts.IMHAssortmentNodeCategoryID)
      {
        if (items.GetParentData(0, typeof (IIMHNode)) is IIMHNode parentData)
          flag = parentData.ParentCategoryID == Consts.IMHStandardNodeCategoryID;
      }
      else if (parentCategoryId == Consts.IMHDetailsMaterialNodeCategoryID)
        flag = true;
      if (flag)
        groupCommands.Remove("SetupColumns");
    }
    return groupCommands;
  }
}
