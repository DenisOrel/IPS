// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.CertSheetProperties
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.PropertyEditors;
using Intermech.Search.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.Signs.Client;

internal class CertSheetProperties
{
  private long _BlankObjectId = -1;
  private int _g09AttributeId;
  private int _g10AttributeId;
  private CertSheetGraphSortMethod _CertSheetGraphSortMethod;
  private string _CertSheetCommonFolder = LocalizationHolder.rm.GetString(nameof (CertSheetCommonFolder));
  private bool _ActualSignsOnly;
  internal bool _inited;

  internal void ApplyUpdates()
  {
    IDBConfigurations service = ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations;
    string empty1 = string.Empty;
    if (this._BlankObjectId != -1L)
    {
      QuickObjectInfo objectInfo = ApplicationServices.Container.GetService<IObjectsInfoCache>().GetObjectInfo(this._BlankObjectId);
      if (!objectInfo.Empty)
        empty1 = objectInfo.VersionGuid.ToString();
    }
    service.WriteString("CLIENT", "CERTSHEETS", "BLANKGUID", empty1, 0L);
    this._CertSheetCommonFolder = OSHelper.ReplaceForbiddenSymbols(this._CertSheetCommonFolder, ' ');
    service.WriteString("CLIENT", "CERTSHEETS", "COMMONFOLDER", this._CertSheetCommonFolder, 0L);
    service.WriteBool("CLIENT", "CERTSHEETS", "ACTUALSIGNSONLY", this._ActualSignsOnly, 0L);
    string empty2 = string.Empty;
    Guid attributeGuid;
    if (this._g09AttributeId != 0)
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(this._g09AttributeId);
      if (attributeType != null)
      {
        attributeGuid = attributeType.AttributeGuid;
        empty2 = attributeGuid.ToString();
      }
    }
    service.WriteString("CLIENT", "CERTSHEETS", "G09ATTRIBUTEGUID", empty2, 0L);
    string empty3 = string.Empty;
    if (this._g10AttributeId != 0)
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(this._g10AttributeId);
      if (attributeType != null)
      {
        attributeGuid = attributeType.AttributeGuid;
        empty3 = attributeGuid.ToString();
      }
    }
    service.WriteString("CLIENT", "CERTSHEETS", "G10ATTRIBUTEGUID", empty3, 0L);
    service.WriteInteger("CLIENT", "CERTSHEETS", "CERTSHEETGRAPHSORT", Convert.ToInt64((object) this._CertSheetGraphSortMethod), 0L);
  }

  internal void LoadCurrentValues()
  {
    IDBConfigurations service = ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations;
    string g1 = service.ReadString("CLIENT", "CERTSHEETS", "BLANKGUID", CertSheetHolder.DefaultParamBlankGuid, DBConfigMode.GlobalOnly);
    this._BlankObjectId = -1L;
    if (g1.Trim() != string.Empty)
    {
      QuickObjectInfo objectInfo = ApplicationServices.Container.GetService<IObjectsInfoCache>().GetObjectInfo(new Guid(g1));
      if (!objectInfo.Empty)
        this._BlankObjectId = objectInfo.ObjectID;
    }
    this._CertSheetCommonFolder = service.ReadString("CLIENT", "CERTSHEETS", "COMMONFOLDER", CertSheetHolder.DefaultParamCertSheetCommonFolder, DBConfigMode.GlobalOnly);
    this._ActualSignsOnly = service.ReadBool("CLIENT", "CERTSHEETS", "ACTUALSIGNSONLY", CertSheetHolder.DefaultParamActualSignsOnly, DBConfigMode.GlobalOnly);
    string g2 = service.ReadString("CLIENT", "CERTSHEETS", "G09ATTRIBUTEGUID", CertSheetHolder.DefaultParamG09AttributeGuid, DBConfigMode.GlobalOnly);
    this._g09AttributeId = 0;
    if (g2 != string.Empty)
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(new Guid(g2));
      if (attributeType != null)
        this._g09AttributeId = attributeType.AttributeID;
    }
    string g3 = service.ReadString("CLIENT", "CERTSHEETS", "G10ATTRIBUTEGUID", CertSheetHolder.DefaultParamG10AttributeGuid, DBConfigMode.GlobalOnly);
    this._g10AttributeId = 0;
    if (g3 != string.Empty)
    {
      IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(new Guid(g3));
      if (attributeType != null)
        this._g10AttributeId = attributeType.AttributeID;
    }
    this._CertSheetGraphSortMethod = (CertSheetGraphSortMethod) service.ReadInteger("CLIENT", "CERTSHEETS", "CERTSHEETGRAPHSORT", Convert.ToInt64((object) CertSheetGraphSortMethod.ByDefault), DBConfigMode.GlobalOnly);
  }

  private void CheckInited()
  {
    if (this._inited)
      return;
    this.LoadCurrentValues();
    this._inited = true;
  }

  [CustomDescription("CertSheetGraphSortDescription")]
  [CustomDisplayName("CertSheetGraphSort")]
  [TypeConverter(typeof (CertSheetGraphSortConverter))]
  public CertSheetGraphSortClass CertSheetGraphSort
  {
    get
    {
      this.CheckInited();
      return new CertSheetGraphSortClass(this._CertSheetGraphSortMethod);
    }
    set => this._CertSheetGraphSortMethod = value.CertSheetGraphSortMethod;
  }

  [CustomDescription("CertSheetBlankDescription")]
  [CustomDisplayName("CertSheetBlank")]
  [TypeConverter(typeof (ObjectPropertyClass))]
  [Editor(typeof (BlankObjectEditor), typeof (UITypeEditor))]
  public ObjectPropertyClass CertSheetBlank
  {
    get
    {
      this.CheckInited();
      return this._BlankObjectId == -1L ? new ObjectPropertyClass(this._BlankObjectId, string.Empty, LocalizationHolder.rm.GetString("CertSheetProps_NotAssigned")) : new ObjectPropertyClass(this._BlankObjectId);
    }
    set => this._BlankObjectId = value.ObjectID;
  }

  [CustomDescription("CertSheetCommonFolderDescription")]
  [CustomDisplayName("CertSheetCommonFolderCaption")]
  [TypeConverter(typeof (string))]
  [DefaultValue("")]
  public string CertSheetCommonFolder
  {
    get
    {
      this.CheckInited();
      return this._CertSheetCommonFolder;
    }
    set => this._CertSheetCommonFolder = value;
  }

  [CustomDescription("ActualSignsOnlyDescription")]
  [CustomDisplayName("ActualSignsOnlyCaption")]
  [TypeConverter(typeof (YesNoBooleanConverter))]
  [DefaultValue(false)]
  public bool ActualSignsOnly
  {
    get
    {
      this.CheckInited();
      return this._ActualSignsOnly;
    }
    set => this._ActualSignsOnly = value;
  }

  [CustomDescription("CertSheetG09AttributeDescription")]
  [CustomDisplayName("CertSheetG09Attribute")]
  [TypeConverter(typeof (AttributePropertyClass))]
  [Editor(typeof (AttributeEditor), typeof (UITypeEditor))]
  [DefaultValue(null)]
  public AttributePropertyClass G09Attribute
  {
    get
    {
      this.CheckInited();
      return this._g09AttributeId == 0 ? (AttributePropertyClass) null : new AttributePropertyClass(this._g09AttributeId);
    }
    set => this._g09AttributeId = value == null ? 0 : value.Attribute;
  }

  [CustomDescription("CertSheetG10AttributeDescription")]
  [CustomDisplayName("CertSheetG10Attribute")]
  [TypeConverter(typeof (AttributePropertyClass))]
  [Editor(typeof (AttributeEditor), typeof (UITypeEditor))]
  [DefaultValue(null)]
  public AttributePropertyClass G10Attribute
  {
    get
    {
      this.CheckInited();
      return this._g10AttributeId == 0 ? (AttributePropertyClass) null : new AttributePropertyClass(this._g10AttributeId);
    }
    set => this._g10AttributeId = value == null ? 0 : value.Attribute;
  }
}
