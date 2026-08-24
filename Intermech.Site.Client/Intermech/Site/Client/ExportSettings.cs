// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ExportSettings
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.PropertyEditors;
using Intermech.PropertyEditors.ChangeHighlighting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class ExportSettings : TransferSettings<IPublishRulesService>
{
  private int _maxAccessLevel;
  private bool _otdFiltering;
  private List<long> _beSurePublishForSites;
  private ChangeTrackingListAdapter<InseparableObjectTypesItem> _inseparableObjectTypes;
  private long _blobStorageID;
  private TaskPriority _receipt4packetTaskPriority = TaskPriority.Hight;
  private TaskPriority _answerTaskPriority = TaskPriority.Hight;
  private List<long> _enableTrueTaskForSites;

  public override void OnApply(IPublishRulesService service)
  {
    service.MaxAccessLevel = this._maxAccessLevel;
    service.OTDFiltering = this._otdFiltering;
    service.BeSurePublishForSites = this._beSurePublishForSites;
    service.BlobStorageID = this._blobStorageID;
    service.AnswerTaskPriority = this._answerTaskPriority;
    service.Receipt4packetTaskPriority = this._receipt4packetTaskPriority;
    service.EnableTrueTaskForSites = this._enableTrueTaskForSites;
    List<Tuple<int, int>> tupleList = new List<Tuple<int, int>>();
    if (this._inseparableObjectTypes != null)
    {
      foreach (InseparableObjectTypesItem inseparableObjectType in this._inseparableObjectTypes)
        tupleList.Add(new Tuple<int, int>(inseparableObjectType.LeftTypeId.Id, inseparableObjectType.RightTypeId.Id));
    }
    service.InseparableObjectTypes = tupleList;
  }

  public override void OnLoad(IPublishRulesService service)
  {
    this._maxAccessLevel = service.MaxAccessLevel;
    this._otdFiltering = service.OTDFiltering;
    this._beSurePublishForSites = service.BeSurePublishForSites;
    this._blobStorageID = service.BlobStorageID;
    this._inseparableObjectTypes = new ChangeTrackingListAdapter<InseparableObjectTypesItem>();
    if (service.InseparableObjectTypes != null)
    {
      foreach (Tuple<int, int> inseparableObjectType in service.InseparableObjectTypes)
        this._inseparableObjectTypes.Items.Add(new InseparableObjectTypesItem(new LocalId<int>(inseparableObjectType.Item1, MetaDataHelper.GetObjectTypeName(inseparableObjectType.Item1)), new LocalId<int>(inseparableObjectType.Item2, MetaDataHelper.GetObjectTypeName(inseparableObjectType.Item2))));
    }
    this._answerTaskPriority = service.AnswerTaskPriority;
    this._receipt4packetTaskPriority = service.Receipt4packetTaskPriority;
    this._enableTrueTaskForSites = service.EnableTrueTaskForSites;
  }

  [DisplayName("Приоритет публикации квитанций импорта")]
  [Description("Приоритет автоматически формируемых задач с публикацией квитанции импорта пакета")]
  [TypeConverter(typeof (TaskPriorityConverter))]
  public TaskPriority Receipt4packetTaskPriority
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._receipt4packetTaskPriority;
    }
    set => this._receipt4packetTaskPriority = value;
  }

  [DisplayName("Приоритет публикации отчетов импорта")]
  [Description("Приоритет автоматически формируемых задач с публикацией ответа об успешном импорте для узла инициатора импорта")]
  [TypeConverter(typeof (TaskPriorityConverter))]
  public TaskPriority AnswerTaskPriority
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._answerTaskPriority;
    }
    set => this._answerTaskPriority = value;
  }

  [DisplayName("Максимальный уровень доступа")]
  [Description("Максимальный уровень доступа объектов, разрешенных для публикации на портал")]
  [TypeConverter(typeof (SecurityLevelTypeConverter))]
  public SecurityLevelPropertyClass MaxAssessLevel
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return new SecurityLevelPropertyClass(this._maxAccessLevel);
    }
    set => this._maxAccessLevel = value != null ? value.SecurityLevel : 0;
  }

  [DisplayName("Фильтровать документы по абоненту ОТД")]
  [Description("При включенной настройке выполняется публикация только тех объектов типа «Документ» у которых в листе рассылки есть узлы из списка разрешенных узлов")]
  [TypeConverter(typeof (YesNoConverter))]
  public bool OTDFiltering
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._otdFiltering;
    }
    set => this._otdFiltering = value;
  }

  [DisplayName("Обязательно публиковать для узлов")]
  [Description("Список узлов, на которые документы будут публиковаться вне зависимости от наличия этих узлов в листах рассылки документов. Настройка действует если включена настройка \"Фильтровать документы по абоненту ОТД\".")]
  [TypeConverter(typeof (SitesListConverter))]
  public SitesListPropertyClass BeSurePublishForSites
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._beSurePublishForSites == null ? new SitesListPropertyClass(new List<long>(0)) : new SitesListPropertyClass(this._beSurePublishForSites);
    }
    set => this._beSurePublishForSites = value.ObjectIDList;
  }

  [DisplayName("Синхронно публикуемые типы объектов")]
  [Description("Синхронно публикуемые типы объектов")]
  [Editor(typeof (InseparableObjectTypesEditor), typeof (UITypeEditor))]
  public ChangeTrackingListAdapter<InseparableObjectTypesItem> InseparableObjectTypes
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._inseparableObjectTypes;
    }
    set => this._inseparableObjectTypes = value;
  }

  [DisplayName("Файловый шкаф для публикуемых данных")]
  [Description("Если назначен, то публикуемые данные будут храниться в указанном файловом шкафу. Данный режим рекомендован при очень больших объемах публикуемых данных.")]
  [Editor(typeof (StorageEditor), typeof (UITypeEditor))]
  [DefaultValue(null)]
  public StoragePropertyClass BlobStorage
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._blobStorageID != 0L ? new StoragePropertyClass(this._blobStorageID) : (StoragePropertyClass) null;
    }
    set => this._blobStorageID = value != null ? value.Storage : 0L;
  }

  [DisplayName("Всегда разрешать задачи для узлов")]
  [Description("Список узлов, при публикации на которые атрибут задачи \"Публикация разрешена\" всегда будет иметь значение ИСТИНА.")]
  [TypeConverter(typeof (SitesListConverter))]
  public SitesListPropertyClass EnableTrueTaskForSites
  {
    [DebuggerStepThrough] get
    {
      this.CheckInited();
      return this._enableTrueTaskForSites == null ? new SitesListPropertyClass(new List<long>(0)) : new SitesListPropertyClass(this._enableTrueTaskForSites);
    }
    set => this._enableTrueTaskForSites = value.ObjectIDList;
  }
}
