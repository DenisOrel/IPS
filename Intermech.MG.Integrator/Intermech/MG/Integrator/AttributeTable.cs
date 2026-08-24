// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.AttributeTable
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Memoization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class AttributeTable
{
  private readonly MGSettingsService settingsSvc;
  private readonly AttributeTable.TableKind tableKind;
  private readonly StateMonitorCacheGuard renameTableGuard;
  private List<Tuple<StringKey, StringKey, bool>> renameTableCache;

  public AttributeTable(MGSettingsService settingsSvc, AttributeTable.TableKind tableKind)
  {
    this.settingsSvc = settingsSvc != null ? settingsSvc : throw new ArgumentNullException(nameof (settingsSvc));
    this.tableKind = tableKind;
    this.renameTableGuard = new StateMonitorCacheGuard(settingsSvc.GetSettingsStateMonitor());
    this.renameTableGuard.ResetCache += new EventHandler(this.OnRebuildRenameTableCache);
  }

  private void OnRebuildRenameTableCache(object sender, EventArgs e)
  {
    this.renameTableCache = AttributeTable.GetTable(this.settingsSvc.GetSettings(), this.tableKind);
  }

  private static List<Tuple<StringKey, StringKey, bool>> GetTable(
    MGIntegratorSettings settings,
    AttributeTable.TableKind tableKind)
  {
    switch (tableKind)
    {
      case AttributeTable.TableKind.DocumentAttributes:
        return settings.DocumentAttributesTable;
      case AttributeTable.TableKind.AssemblyAttributes:
        return settings.AssemblyAttributesTable;
      case AttributeTable.TableKind.PartAttributes:
        return settings.PartAttributesTable;
      default:
        throw new NotImplementedException();
    }
  }

  public AttributeTable.TableKind Kind => this.tableKind;

  public List<Tuple<StringKey, StringKey, bool>> Rows
  {
    get
    {
      lock (this)
      {
        this.renameTableGuard.CheckCache();
        return this.renameTableCache;
      }
    }
  }

  public StringKey GetFormatterValueKey(StringKey attributeKey, StringKey defaultValueKey)
  {
    if (attributeKey == (StringKey) null)
      throw new ArgumentNullException(nameof (attributeKey));
    if (defaultValueKey == (StringKey) null)
      throw new ArgumentNullException(nameof (defaultValueKey));
    if (this.Rows != null)
    {
      Tuple<StringKey, StringKey, bool> tuple = this.Rows.Find((Predicate<Tuple<StringKey, StringKey, bool>>) (item => item.Item1 == attributeKey));
      if (tuple != null)
        return tuple.Item2;
    }
    return defaultValueKey;
  }

  public enum TableKind
  {
    DocumentAttributes,
    AssemblyAttributes,
    PartAttributes,
  }
}
