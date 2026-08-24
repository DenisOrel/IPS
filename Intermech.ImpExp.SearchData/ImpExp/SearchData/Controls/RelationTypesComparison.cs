// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.Controls.RelationTypesComparison
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.ImpExp.SearchData.Controls;

internal sealed class RelationTypesComparison
{
  private const string _relTypeComparisonSettingsName = "PLLIST_RT";
  private const string _relTypeSimlpe = "RT_SIMPLE";
  private const string _relTypeTech = "RT_TECH";
  private const string _relTypeProd = "RT_PROD";
  private const string _relTypePLPL = "RT_PLPL";
  private const string _relTypeF5 = "RT_F5";
  private static readonly Guid _defaultSimpleType = new Guid("cadd9a57-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid _defaultTechType = new Guid("cad0019f-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid _defaultProdType = new Guid("cad00151-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid _defaultPLPLType = new Guid("cadd9a57-306c-11d8-b4e9-00304f19f545");
  private static readonly Guid _defaultF5Type = new Guid("cad00154-306c-11d8-b4e9-00304f19f545");

  public RelationTypesComparison()
    : this(RelationTypesComparison._defaultSimpleType, RelationTypesComparison._defaultTechType, RelationTypesComparison._defaultProdType, RelationTypesComparison._defaultPLPLType, RelationTypesComparison._defaultF5Type)
  {
  }

  public RelationTypesComparison(
    Guid simpleTypeGuid,
    Guid techTypeGuid,
    Guid prodTypeGuid,
    Guid plplTypeGuid,
    Guid f5TypeGuid)
  {
    IUserSession userSession = (ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter).GetUserSession();
    this.SimpleType = this.GetFromGuid(userSession, simpleTypeGuid, RelationTypesComparison._defaultSimpleType);
    this.TechType = this.GetFromGuid(userSession, techTypeGuid, RelationTypesComparison._defaultTechType);
    this.ProdType = this.GetFromGuid(userSession, prodTypeGuid, RelationTypesComparison._defaultProdType);
    this.PLPLType = this.GetFromGuid(userSession, plplTypeGuid, RelationTypesComparison._defaultPLPLType);
    this.F5Type = this.GetFromGuid(userSession, f5TypeGuid, RelationTypesComparison._defaultF5Type);
  }

  public static RelationTypesComparison Instance
  {
    get
    {
      Dictionary<string, SaveSettingsAttribute[]> settings = (ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings).GetSettings("PLLIST_RT");
      return settings != null ? new RelationTypesComparison(RelationTypesComparison.GetRelationTypeGuidFromSettings(settings, "RT_SIMPLE"), RelationTypesComparison.GetRelationTypeGuidFromSettings(settings, "RT_TECH"), RelationTypesComparison.GetRelationTypeGuidFromSettings(settings, "RT_PROD"), RelationTypesComparison.GetRelationTypeGuidFromSettings(settings, "RT_PLPL"), RelationTypesComparison.GetRelationTypeGuidFromSettings(settings, "RT_F5")) : new RelationTypesComparison();
    }
  }

  public void Save()
  {
    ISaveSettings service = ServicesManager.ServiceContainer.GetService(typeof (ISaveSettings)) as ISaveSettings;
    service.ClearSettings("PLLIST_RT");
    service.SetSettings("PLLIST_RT", new Dictionary<string, SaveSettingsAttribute[]>()
    {
      {
        "RT_SIMPLE",
        new SaveSettingsAttribute[1]
        {
          new SaveSettingsAttribute("GUID", this.SimpleType.RelationType.ToString())
        }
      },
      {
        "RT_TECH",
        new SaveSettingsAttribute[1]
        {
          new SaveSettingsAttribute("GUID", this.TechType.RelationType.ToString())
        }
      },
      {
        "RT_PROD",
        new SaveSettingsAttribute[1]
        {
          new SaveSettingsAttribute("GUID", this.ProdType.RelationType.ToString())
        }
      },
      {
        "RT_PLPL",
        new SaveSettingsAttribute[1]
        {
          new SaveSettingsAttribute("GUID", this.PLPLType.RelationType.ToString())
        }
      },
      {
        "RT_F5",
        new SaveSettingsAttribute[1]
        {
          new SaveSettingsAttribute("GUID", this.F5Type.RelationType.ToString())
        }
      }
    });
  }

  private static Guid GetRelationTypeGuidFromSettings(
    Dictionary<string, SaveSettingsAttribute[]> settings,
    string name)
  {
    SaveSettingsAttribute[] settingsAttributeArray;
    return !settings.TryGetValue(name, out settingsAttributeArray) || !GuidHelper.IsGuid(settingsAttributeArray[0].AttributeValue) ? Guid.Empty : new Guid(settingsAttributeArray[0].AttributeValue);
  }

  private RelationTypeAttProxy GetFromGuid(IUserSession session, Guid guid, Guid defaultGuid)
  {
    IDBRelationType relationType = session.GetRelationType(guid != Guid.Empty ? guid : defaultGuid);
    return new RelationTypeAttProxy(guid, relationType.Description);
  }

  [DisplayName("Обычная связь")]
  [Editor(typeof (RelationTypeEditor), typeof (UITypeEditor))]
  public RelationTypeAttProxy SimpleType { get; }

  [DisplayName("Технологическая ручная связь")]
  [Editor(typeof (RelationTypeEditor), typeof (UITypeEditor))]
  public RelationTypeAttProxy TechType { get; set; }

  [DisplayName("Производственная ручная связь")]
  [Editor(typeof (RelationTypeEditor), typeof (UITypeEditor))]
  public RelationTypeAttProxy ProdType { get; set; }

  [DisplayName("Тип связи для ведомости «И»")]
  [Editor(typeof (RelationTypeEditor), typeof (UITypeEditor))]
  public RelationTypeAttProxy PLPLType { get; set; }

  [Browsable(false)]
  public RelationTypeAttProxy F5Type { get; set; }
}
