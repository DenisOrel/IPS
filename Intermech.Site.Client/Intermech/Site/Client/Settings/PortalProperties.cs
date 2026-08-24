// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.PortalProperties
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using Intermech.PropertyEditors;
using Intermech.PropertyEditors.ChangeHighlighting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;

#nullable disable
namespace Intermech.Site.Client.Settings;

internal class PortalProperties : ICloneable
{
  private long _countRecordsInPackage = PortalConsts.DefaultCountRecordsInPackage;
  private long _receiptTemplateID;
  private ChangeTrackingListAdapter<LoggingObjectTypeItem> _loggingTransferObjectTypes;
  public bool Inited;

  public void ApplyUpdates()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBConfigurations configurations = sessionKeeper.Session.Configurations;
      configurations.WriteInteger(PortalConsts.PortalClientModuleName, "GENERAL_SETTINGS", "RECORD_COUNT", this._countRecordsInPackage);
      configurations.WriteInteger(PortalConsts.PortalClientModuleName, "GENERAL_SETTINGS", "RECEIPT_TEMPL_ID", this._receiptTemplateID);
      List<int> intList = new List<int>();
      foreach (LoggingObjectTypeItem transferObjectType in this._loggingTransferObjectTypes)
        intList.Add(transferObjectType.TypeId.Id);
      (sessionKeeper.Session.GetCustomService(typeof (IPublishRulesService)) as IPublishRulesService).LoggingTransferObjectTypes = intList;
    }
  }

  private void CheckInited()
  {
    if (this.Inited)
      return;
    this.LoadCurrentValues();
    this.Inited = true;
  }

  public void LoadCurrentValues()
  {
    IDBConfigurations service = ServicesManager.GetService(typeof (IDBConfigurations)) as IDBConfigurations;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      sessionKeeper.Session.GetObjectInfo(new Guid("cad0028f-306c-11d8-b4e9-00304f19f545"));
      this._countRecordsInPackage = service.ReadInteger(PortalConsts.PortalClientModuleName, "GENERAL_SETTINGS", "RECORD_COUNT", PortalConsts.DefaultCountRecordsInPackage, DBConfigMode.UserAndGlobal);
      this._receiptTemplateID = SettingsHelper.GetReceiptTemplateID(sessionKeeper.Session, service);
      IPublishRulesService customService = sessionKeeper.Session.GetCustomService(typeof (IPublishRulesService)) as IPublishRulesService;
      this._loggingTransferObjectTypes = new ChangeTrackingListAdapter<LoggingObjectTypeItem>();
      List<int> transferObjectTypes = customService.LoggingTransferObjectTypes;
      if (transferObjectTypes == null)
        return;
      foreach (int objTypeID in transferObjectTypes)
      {
        IMSObjectType objectType = MetaDataHelper.GetObjectType(objTypeID);
        if (objectType != null)
          this._loggingTransferObjectTypes.Items.Add(new LoggingObjectTypeItem(objectType.ObjectTypeID, objectType.ObjectTypeName));
      }
    }
  }

  public object Clone()
  {
    return (object) new PortalProperties()
    {
      LoggingTransferObjectTypes = this.LoggingTransferObjectTypes.Clone(),
      CountRecordsInPackage = this.CountRecordsInPackage,
      ReceiptTemplate = this.ReceiptTemplate
    };
  }

  [Description("Типы объектов, при импорте и публикации объектов которых необходимо делать соответствующие записи в журнал")]
  [DisplayName("Запись в журнал")]
  [Editor(typeof (LoggingObjectTypesEditor), typeof (UITypeEditor))]
  public ChangeTrackingListAdapter<LoggingObjectTypeItem> LoggingTransferObjectTypes
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._loggingTransferObjectTypes;
    }
    set => this._loggingTransferObjectTypes = value;
  }

  [CustomDescription("Attribute.Site.Client_5")]
  [CustomDisplayName("Attribute.Site.Client_6")]
  public long CountRecordsInPackage
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._countRecordsInPackage;
    }
    set => this._countRecordsInPackage = value;
  }

  [DisplayName("Шаблон для квитанции")]
  [Description("Шаблон документа для отображения квитанции")]
  [Editor(typeof (ReceiptTemplateEditor), typeof (UITypeEditor))]
  public ObjectPropertyClass ReceiptTemplate
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._receiptTemplateID != 0L ? new ObjectPropertyClass(this._receiptTemplateID) : (ObjectPropertyClass) null;
    }
    set => this._receiptTemplateID = value.ObjectID;
  }
}
