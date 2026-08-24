// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.HideDeletePositionsCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Bars;
using Intermech.Interfaces.Client;
using Intermech.Navigator.Interfaces;
using System;

#nullable disable
namespace Intermech.MRP2;

/// <summary>команда меню скрыть исключенны позиции в составе</summary>
internal class HideDeletePositionsCommand
{
  internal static bool checkedState;

  internal static void Handler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    HideDeletePositionsCommand.checkedState = !HideDeletePositionsCommand.checkedState;
    IFiltrationService service = ServicesManager.GetService(typeof (IFiltrationService)) as IFiltrationService;
    if (HideDeletePositionsCommand.checkedState)
      service.Filtration.Tags[(object) "9854400D-D3EB-4A82-ADD3-00163FB748FC"] = (object) true;
    else
      service.Filtration.Tags[(object) "9854400D-D3EB-4A82-ADD3-00163FB748FC"] = (object) null;
    service.FiltrationApplyUpdates(true);
    ServicesManager.GetService<ICommandManager>().QueryStatus();
  }
}
