// Decompiled with JetBrains decompiler
// Type: Intermech.MG.Integrator.MGSettingsSurrogate
// Assembly: Intermech.MG.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DC8032C5-2D09-47AD-9096-064F93238E19
// Assembly location: D:\IPS\Client\Intermech.MG.Integrator.dll

using Intermech.Interfaces;
using Intermech.PropertyEditors.ChangeHighlighting;
using Intermech.Tools.Integrators.Electrical;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;

#nullable disable
namespace Intermech.MG.Integrator;

internal class MGSettingsSurrogate : ECADSettingsSurrogate<MGIntegratorSettings>
{
  protected ChangeTrackingListAdapter<ElementListItemSurrogate> elementListTypes;
  protected ChangeTrackingListAdapter<FilterItemSurrogate> componentsFilterSettings;
  protected string filterParameterName;
  protected string partPosDesignationAttribute;
  protected string fgPosDesignation;

  public MGSettingsSurrogate(MGIntegratorSettings settings)
    : base(settings)
  {
    this.notImportingDir = new ChangeTrackingListAdapter<FolderNameSurrogate>((IEnumerable<FolderNameSurrogate>) settings.NotImportingDir.ConvertAll<FolderNameSurrogate>((Converter<string, FolderNameSurrogate>) (folder => new FolderNameSurrogate()
    {
      FolderName = folder
    })));
    this.partPosDesignationAttribute = settings.PartPosDesignationAttribute;
    this.filterParameterName = settings.FilterParameterName;
    using (SessionKeeper keeper = new SessionKeeper())
      this.elementListTypes = new ChangeTrackingListAdapter<ElementListItemSurrogate>((IEnumerable<ElementListItemSurrogate>) settings.ElementListTypes.ConvertAll<ElementListItemSurrogate>((Converter<Tuple<Guid, string>, ElementListItemSurrogate>) (item =>
      {
        ElementListItemSurrogate listItemSurrogate = new ElementListItemSurrogate();
        IDBObjectType objectType = keeper.Session.GetObjectType(item.Item1, true);
        listItemSurrogate.ObjectType = new GlobalId<int>(item.Item1, objectType.ObjectType, objectType.ObjectTypeName);
        listItemSurrogate.Suffix = item.Item2;
        return listItemSurrogate;
      })));
    this.componentsFilterSettings = settings.ComponentsFilter == null ? new ChangeTrackingListAdapter<FilterItemSurrogate>() : new ChangeTrackingListAdapter<FilterItemSurrogate>((IEnumerable<FilterItemSurrogate>) settings.ComponentsFilter.ConvertAll<FilterItemSurrogate>((Converter<Tuple<StringKey, CompositionVariants>, FilterItemSurrogate>) (item => new FilterItemSurrogate()
    {
      ParameterValue = (string) item.Item1,
      Variant = item.Item2
    })));
    this.fgPosDesignation = settings.FGPosDesignation;
  }

  protected override void SaveSettings(MGIntegratorSettings settings)
  {
    base.SaveSettings(settings);
    settings.NotImportingDir = new List<string>(this.NotImportingDir.Count<FolderNameSurrogate>());
    foreach (FolderNameSurrogate folderNameSurrogate in this.NotImportingDir)
      settings.NotImportingDir.Add(folderNameSurrogate.FolderName);
    settings.PartPosDesignationAttribute = this.PartPostDesignationAttribute;
    settings.ElementListTypes = new List<Tuple<Guid, string>>(this.ElementListTypes != null ? this.ElementListTypes.Count<ElementListItemSurrogate>() : 0);
    if (this.ElementListTypes != null)
    {
      foreach (ElementListItemSurrogate elementListType in this.ElementListTypes)
        settings.ElementListTypes.Add(new Tuple<Guid, string>(elementListType.ObjectType.Guid, elementListType.Suffix));
    }
    settings.FilterParameterName = this.FilterParameterName;
    settings.ComponentsFilter = new List<Tuple<StringKey, CompositionVariants>>();
    if (this.ComponentsFilter != null)
    {
      foreach (FilterItemSurrogate filterItemSurrogate in this.ComponentsFilter)
        settings.ComponentsFilter.Add(new Tuple<StringKey, CompositionVariants>((StringKey) filterItemSurrogate.ParameterValue, filterItemSurrogate.Variant));
    }
    settings.FGPosDesignation = this.fgPosDesignation;
  }

  public override object Clone() => (object) new MGSettingsSurrogate(this.Settings);

  [Category("Настройки идентифицирующих атрибутов")]
  [DisplayName("Позиционное обозначение прочего изделия")]
  [Description("Это свойство содержит наименование параметра элемента схемы, значение которого соответствует позиционному обозначению в применяемости прочего изделия.")]
  public string PartPostDesignationAttribute
  {
    get => this.partPosDesignationAttribute;
    set => this.partPosDesignationAttribute = value;
  }

  [Category("Настройки типов перечней элементов")]
  [DisplayName("Типы перечней элементов")]
  [Description("В этом списке описаны настройки типов перечней элементов")]
  [Editor(typeof (ElementListsUIEditor), typeof (UITypeEditor))]
  public ChangeTrackingListAdapter<ElementListItemSurrogate> ElementListTypes
  {
    get => this.elementListTypes;
    set => this.elementListTypes = value;
  }

  [Category("Настройки фильтрации составов")]
  [DisplayName("Настройки фильтрации составов")]
  [Description("Содержит настройки фильтрации списка компонентов схемы для состава изделия и перечня элементов.")]
  [Editor(typeof (ComponentsFilterUIEditor), typeof (UITypeEditor))]
  public ChangeTrackingListAdapter<FilterItemSurrogate> ComponentsFilter
  {
    get => this.componentsFilterSettings;
    set => this.componentsFilterSettings = value;
  }

  [Category("Настройки фильтрации составов")]
  [DisplayName("Имя параметра")]
  [Description("Имя параметра компонента, по значению которого определяется принадлежность компонента к соотвествующему варианту состава")]
  public string FilterParameterName
  {
    get => this.filterParameterName;
    set => this.filterParameterName = value;
  }

  [Category("Настройки функциональных групп")]
  [DisplayName("Позиционное обозначение функциональной группы")]
  [Description("Наименование параметра штампа в котором указано позиционное обозначение функциональной группы")]
  public string FGPosDesignation
  {
    get => this.fgPosDesignation;
    set => this.fgPosDesignation = value;
  }
}
