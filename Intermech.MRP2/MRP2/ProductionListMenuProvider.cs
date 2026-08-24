// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ProductionListMenuProvider
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Navigator.ContextMenu;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.MRP2;

internal class ProductionListMenuProvider : ICommandsProvider
{
  public CommandsInfo GetMergedCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    CommandsInfo mergedCommands = new CommandsInfo();
    mergedCommands.Add("CreateProto", new CommandInfo(0, new ClickEventHandler(CreateProtoCommand.Handler)));
    return mergedCommands;
  }

  public CommandsInfo GetGroupCommands(ISelectedItems items, IServiceProvider viewServices)
  {
    return CommandsInfo.Empty;
  }
}
