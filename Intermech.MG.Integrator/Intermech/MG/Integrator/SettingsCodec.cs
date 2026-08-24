// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.SettingsCodec
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.Electrical;
using Intermech.Tools.Settings;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Intermech.MG.Integrator;

internal sealed class SettingsCodec : ECADSettingsCodec<MGIntegratorSettings>
{
  private IIntegrator _integrator;

  public SettingsCodec(IIntegrator integrator)
    : base(integrator.DisplayName)
  {
    this._integrator = integrator;
  }

  protected override void OnCreateEmptySettings(MGIntegratorSettings settings)
  {
    MGSettingsService service = this._integrator.GetService(typeof (MGSettingsService)) as MGSettingsService;
    settings.AssemblyDocumentType = service.ProjectDocumentType;
  }

  protected override int GetEncoderFormatVersion() => 3;

  protected override void EncodeSettings(
    ISettingsObject settingsObject,
    SettingsXmlBuilder settingsBuilder)
  {
    base.EncodeSettings(settingsObject, settingsBuilder);
    MGIntegratorSettings settings = (MGIntegratorSettings) settingsObject;
    settingsBuilder.AppendElement(this.EncodePartIDAttributes(settingsBuilder, settings));
    settingsBuilder.AppendElement(this.EncodeElementListTypes(settingsBuilder, settings));
    settingsBuilder.AppendElement(this.EncodeStamp(settingsBuilder, settings));
    settingsBuilder.AppendElement(this.EncodeStampAttributes(settingsBuilder, settings));
    settingsBuilder.AppendElement(this.EncodeCompositionFilter(settingsBuilder, settings));
    settingsBuilder.AppendElement(this.EncodeNotImportedBoardSettings(settingsBuilder, settings));
    settingsBuilder.AppendElement(this.EncodeFuncGroupAttributes(settingsBuilder, settings));
  }

  private XmlNode EncodePartIDAttributes(
    SettingsXmlBuilder settingsBuilder,
    MGIntegratorSettings settings)
  {
    XmlElement element = settingsBuilder.CreateElement("PartIDAttributes");
    element.AppendChild((XmlNode) settingsBuilder.EncodeText("PosDesignation", settings.PartPosDesignationAttribute));
    return (XmlNode) element;
  }

  private XmlNode EncodeElementListTypes(
    SettingsXmlBuilder settingsBuilder,
    MGIntegratorSettings settings)
  {
    XmlElement element = settingsBuilder.CreateElement("ElementLists");
    if (settings.ElementListTypes != null)
    {
      foreach (Tuple<Guid, string> elementListType in settings.ElementListTypes)
      {
        XmlElement xmlElement = settingsBuilder.EncodeText("ElementList", elementListType.Item2);
        settingsBuilder.AppendAttribute((XmlNode) xmlElement, "type", (object) elementListType.Item1);
        element.AppendChild((XmlNode) xmlElement);
      }
    }
    return (XmlNode) element;
  }

  private XmlNode EncodeNotImportedBoardSettings(
    SettingsXmlBuilder settingsBuilder,
    MGIntegratorSettings settings)
  {
    XmlElement element = settingsBuilder.CreateElement("NotImportetBoardSettings");
    if (settings.NotImportetBoardSettings != null)
    {
      foreach (Tuple<StringKey, StringKey> importetBoardSetting in settings.NotImportetBoardSettings)
      {
        XmlElement xmlElement = settingsBuilder.EncodeText("Item", (string) importetBoardSetting.Item2);
        settingsBuilder.AppendAttribute((XmlNode) xmlElement, "value", (object) importetBoardSetting.Item1.ToString());
        element.AppendChild((XmlNode) xmlElement);
      }
    }
    return (XmlNode) element;
  }

  private XmlNode EncodeCompositionFilter(
    SettingsXmlBuilder settingsBuilder,
    MGIntegratorSettings settings)
  {
    XmlElement element = settingsBuilder.CreateElement("CompositionFilter");
    if (settings.ComponentsFilter != null)
    {
      element.AppendChild((XmlNode) settingsBuilder.EncodeText("ParameterName", settings.FilterParameterName));
      foreach (Tuple<StringKey, CompositionVariants> tuple in settings.ComponentsFilter)
      {
        XmlElement xmlElement = settingsBuilder.EncodeText("CompositionVariants", Convert.ToString((int) tuple.Item2));
        settingsBuilder.AppendAttribute((XmlNode) xmlElement, "value", (object) tuple.Item1.ToString());
        element.AppendChild((XmlNode) xmlElement);
      }
    }
    return (XmlNode) element;
  }

  private XmlNode EncodeStampAttributes(
    SettingsXmlBuilder settingsBuilder,
    MGIntegratorSettings settings)
  {
    XmlElement element = settingsBuilder.CreateElement("StampAttributes");
    element.AppendChild((XmlNode) settingsBuilder.EncodeText("MainSchemeId", settings.MainSchemeId));
    return (XmlNode) element;
  }

  private XmlNode EncodeStamp(SettingsXmlBuilder settingsBuilder, MGIntegratorSettings settings)
  {
    XmlElement element = settingsBuilder.CreateElement("Stamp");
    element.AppendChild((XmlNode) settingsBuilder.EncodeText("StampSign", settings.Sheet));
    return (XmlNode) element;
  }

  protected override void EncodeServerData(
    ISettingsObject settingsObject,
    IntegratorServerDataBuilder serverData)
  {
    base.EncodeServerData(settingsObject, serverData);
    MGSettingsService service = this._integrator.GetService(typeof (MGSettingsService)) as MGSettingsService;
    serverData.AddObjectType(service.ProjectDocumentType.Guid);
  }

  private XmlNode EncodeFuncGroupAttributes(
    SettingsXmlBuilder settingsBuilder,
    MGIntegratorSettings settings)
  {
    XmlElement element = settingsBuilder.CreateElement("AdditionalFuncGroupAttributes");
    element.AppendChild((XmlNode) settingsBuilder.EncodeText("PosDesignation", settings.FGPosDesignation));
    return (XmlNode) element;
  }

  private void DecodeFuncGroupAttributes(
    SettingsXmlBuilder settingsBuilder,
    MGIntegratorSettings settings)
  {
    settings.FGPosDesignation = this.TrimStringValue(settingsBuilder.DecodeText("AdditionalFuncGroupAttributes/PosDesignation", (string) null));
  }

  protected override void DecodeSettings(
    int formatVersion,
    SettingsXmlBuilder settingsBuilder,
    ISettingsObject settingsObject)
  {
    base.DecodeSettings(formatVersion, settingsBuilder, settingsObject);
    switch (formatVersion)
    {
      case 1:
        this.DecodeV1(settingsBuilder, (MGIntegratorSettings) settingsObject);
        break;
      case 2:
        this.DecodeV2(settingsBuilder, (MGIntegratorSettings) settingsObject);
        break;
      case 3:
        this.DecodeV3(settingsBuilder, (MGIntegratorSettings) settingsObject);
        break;
    }
  }

  private void DecodeV3(SettingsXmlBuilder settingsBuilder, MGIntegratorSettings integratorSettings)
  {
    this.DecodeV2(settingsBuilder, integratorSettings);
    this.DecodeFuncGroupAttributes(settingsBuilder, integratorSettings);
  }

  private void DecodeV2(SettingsXmlBuilder settingsBuilder, MGIntegratorSettings integratorSettings)
  {
    this.DecodeV1(settingsBuilder, integratorSettings);
    this.DecodeCompositionFilter(settingsBuilder, integratorSettings);
    this.DecodeNotImportedBoardSettings(settingsBuilder, integratorSettings);
  }

  private void DecodeV1(SettingsXmlBuilder settingsBuilder, MGIntegratorSettings settings)
  {
    this.DecodePartIDAttributes(settingsBuilder, settings);
    this.DecodeElementListTypes(settingsBuilder, settings);
    this.DecodeStamp(settingsBuilder, settings);
    this.DecodeStampAttributes(settingsBuilder, settings);
  }

  private void DecodePartIDAttributes(
    SettingsXmlBuilder settingsBuilder,
    MGIntegratorSettings settings)
  {
    settings.PartPosDesignationAttribute = SettingsUtils.TrimStringValue(settingsBuilder.DecodeText("PartIDAttributes/PosDesignation", (string) null));
  }

  private void DecodeElementListTypes(
    SettingsXmlBuilder settingsBuilder,
    MGIntegratorSettings settings)
  {
    XmlNodeList xmlNodeList = settingsBuilder.SelectNodes("ElementLists/ElementList[@type]");
    settings.ElementListTypes = new List<Tuple<Guid, string>>(xmlNodeList.Count);
    foreach (XmlElement xmlElement in xmlNodeList)
    {
      Guid objTypeGuid = settingsBuilder.ReadAttribute<Guid>((XmlNode) xmlElement, "type", Guid.Empty);
      if (!(objTypeGuid == Guid.Empty) && MetaDataHelper.GetObjectTypeID(objTypeGuid) != -1)
      {
        string str = SettingsUtils.TrimStringValue(settingsBuilder.ReadText((XmlNode) xmlElement, string.Empty));
        settings.ElementListTypes.Add(new Tuple<Guid, string>(objTypeGuid, str));
      }
    }
  }

  private void DecodeStampAttributes(
    SettingsXmlBuilder settingsBuilder,
    MGIntegratorSettings settings)
  {
    settings.MainSchemeId = SettingsUtils.TrimStringValue(settingsBuilder.DecodeText("StampAttributes/MainSchemeId", (string) null));
  }

  private void DecodeStamp(SettingsXmlBuilder settingsBuilder, MGIntegratorSettings settings)
  {
    settings.Sheet = SettingsUtils.TrimStringValue(settingsBuilder.DecodeText("Stamp/StampSign", (string) null));
  }

  private void DecodeNotImportedBoardSettings(
    SettingsXmlBuilder settingsBuilder,
    MGIntegratorSettings settings)
  {
    settings.NotImportetBoardSettings = new List<Tuple<StringKey, StringKey>>();
    foreach (XmlElement selectNode in settingsBuilder.SelectNodes("NotImportetBoardSettings/Item[@value]"))
    {
      string str1 = settingsBuilder.ReadAttribute((XmlNode) selectNode, "value", string.Empty);
      if (!(str1 == string.Empty))
      {
        string str2 = SettingsUtils.TrimStringValue(settingsBuilder.ReadText((XmlNode) selectNode, string.Empty));
        settings.NotImportetBoardSettings.Add(new Tuple<StringKey, StringKey>((StringKey) str1, (StringKey) str2));
      }
    }
  }

  private void DecodeCompositionFilter(
    SettingsXmlBuilder settingsBuilder,
    MGIntegratorSettings settings)
  {
    settings.FilterParameterName = SettingsUtils.TrimStringValue(settingsBuilder.DecodeText("CompositionFilter/ParameterName", string.Empty));
    settings.ComponentsFilter = new List<Tuple<StringKey, CompositionVariants>>();
    foreach (XmlElement selectNode in settingsBuilder.SelectNodes("CompositionFilter/CompositionVariants[@value]"))
    {
      string str1 = settingsBuilder.ReadAttribute((XmlNode) selectNode, "value", string.Empty);
      if (!(str1 == string.Empty))
      {
        string str2 = SettingsUtils.TrimStringValue(settingsBuilder.ReadText((XmlNode) selectNode, string.Empty));
        if (!(str2 == string.Empty))
          settings.ComponentsFilter.Add(new Tuple<StringKey, CompositionVariants>((StringKey) str1, (CompositionVariants) Convert.ToInt32(str2)));
      }
    }
  }
}
