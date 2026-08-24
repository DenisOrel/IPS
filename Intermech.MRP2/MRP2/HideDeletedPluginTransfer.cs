// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.HideDeletedPluginTransfer
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.MRP2.Commands;
using Intermech.Navigator;
using System;
using System.Collections.Specialized;

#nullable disable
namespace Intermech.MRP2;

/// <summary>
/// плагин для передачи параметров запроса на сервер
/// для скрытия в составе исключенных позиций
/// </summary>
internal class HideDeletedPluginTransfer : ClientPluginsDataTransfer
{
  public override void GetPluginData(HybridDictionary PluginsData)
  {
    base.GetPluginData(PluginsData);
    if (HideDeletePositionsCommand.checkedState)
      PluginsData[(object) "9854400D-D3EB-4A82-ADD3-00163FB748FC"] = (object) true;
    else
      PluginsData[(object) "9854400D-D3EB-4A82-ADD3-00163FB748FC"] = (object) null;
    PluginsData[(object) "CC4B5C20-3E62-4436-89E8-699262510FD5"] = (object) FilterDateAttributesCommand.FilterByDateInCompositionEnabled;
    if (FilterDateAttributesCommand.FilterByDateInCompositionEnabled)
      PluginsData[(object) "85357DBA-2685-4F94-8B40-7889D08B322A"] = (object) FilterDateAttributesCommand.FilterByDateInComposition;
    else
      PluginsData[(object) "85357DBA-2685-4F94-8B40-7889D08B322A"] = (object) null;
  }

  public override void PutPluginData(HybridDictionary PluginsData)
  {
    base.PutPluginData(PluginsData);
    bool result1;
    HideDeletePositionsCommand.checkedState = PluginsData[(object) "9854400D-D3EB-4A82-ADD3-00163FB748FC"] != null && bool.TryParse(PluginsData[(object) "9854400D-D3EB-4A82-ADD3-00163FB748FC"].ToString(), out result1) && result1;
    IFiltrationService service = ApplicationServices.Container.GetService<IFiltrationService>();
    bool result2;
    if (PluginsData[(object) "CC4B5C20-3E62-4436-89E8-699262510FD5"] != null && bool.TryParse(PluginsData[(object) "9854400D-D3EB-4A82-ADD3-00163FB748FC"].ToString(), out result2))
    {
      FilterDateAttributesCommand.FilterByDateInCompositionEnabled = result2;
      service.Filtration.Tags[(object) "CC4B5C20-3E62-4436-89E8-699262510FD5"] = (object) result2;
    }
    else
    {
      FilterDateAttributesCommand.FilterByDateInCompositionEnabled = false;
      service.Filtration.Tags[(object) "CC4B5C20-3E62-4436-89E8-699262510FD5"] = (object) false;
    }
    DateTime result3;
    if (PluginsData[(object) "CC4B5C20-3E62-4436-89E8-699262510FD5"] != null && DateTime.TryParse(PluginsData[(object) "9854400D-D3EB-4A82-ADD3-00163FB748FC"].ToString(), out result3))
    {
      FilterDateAttributesCommand.FilterByDateInComposition = result3;
      service.Filtration.Tags[(object) "CC4B5C20-3E62-4436-89E8-699262510FD5"] = (object) result3;
    }
    else
    {
      FilterDateAttributesCommand.FilterByDateInComposition = DateTime.Now;
      service.Filtration.Tags[(object) "CC4B5C20-3E62-4436-89E8-699262510FD5"] = (object) null;
    }
  }
}
