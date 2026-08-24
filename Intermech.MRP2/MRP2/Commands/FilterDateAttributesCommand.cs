// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.Commands.FilterDateAttributesCommand
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Bars;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using Intermech.Search;
using System;
using System.Windows.Forms;

#nullable disable
namespace Intermech.MRP2.Commands;

/// <summary>
/// Обработчики команд добавления/удаления атрибутов фильтрации составов по дате
/// </summary>
internal sealed class FilterDateAttributesCommand
{
  /// <summary>
  /// Установить значение атрибута "Дата окончания действия" на связь с указанными версиями объектов
  /// </summary>
  /// <param name="items">Версии объектов, на связях с которыми нужно установить значение атрибута</param>
  /// <param name="value">Значение атрибута. DeleteModesEnum.None - удалит атрибут из связи при его наличии</param>
  private static void SetEndDateAttributeToObjects(ISelectedItems items, object value)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      INotificationService service = ApplicationServices.Container.GetService<INotificationService>();
      for (int index = 0; index < items.Count && items.GetItemID(index).CategoryID == 1; ++index)
      {
        if (items.GetItemData(0, typeof (IDBRelationID)) is IDBRelationID itemData && !Consts.IsUndefinedRelationId(itemData.Value))
        {
          IDBRelation relation = sessionKeeper.Session.GetRelation(itemData.Value);
          AttributeValues[] valuesList = new AttributeValues[1]
          {
            new AttributeValues(MRP2Consts.attrIdEndDate, value)
          };
          relation.SetAttributesValues(valuesList);
          service?.FireEvent((object) null, (NotificationEventArgs) new DBRelationsEventArgs("RelationsChanged", relation.RelationID, relation.ProjID, relation.RelationType));
        }
      }
    }
  }

  /// <summary>Включен ли фильтр по дате окончания действия связи</summary>
  public static bool FilterByDateInCompositionEnabled { get; set; }

  /// <summary>
  /// Выбранная дата в фильтре по дате окончания действия связи
  /// </summary>
  public static DateTime FilterByDateInComposition { get; set; } = DateTime.Now;

  /// <summary>
  /// Прочитать сохраненные настройки из серверной службы фильтрации
  /// </summary>
  public static void ReadFilterSettingsFromServerFilterService()
  {
    IFiltrationService service = ApplicationServices.Container.GetService<IFiltrationService>();
    object tag1 = service.Filtration.Tags[(object) "85357DBA-2685-4F94-8B40-7889D08B322A"];
    FilterDateAttributesCommand.FilterByDateInComposition = tag1 != null ? Convert.ToDateTime(tag1) : DateTime.Now;
    object tag2 = service.Filtration.Tags[(object) "CC4B5C20-3E62-4436-89E8-699262510FD5"];
    FilterDateAttributesCommand.FilterByDateInCompositionEnabled = tag2 != null && Convert.ToBoolean(tag2);
  }

  /// <summary>Команда меню "Установить сроки действия связи"</summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  public static void AddDateAttributesCommandHandler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    DateTime selectionStart;
    using (MonthCalendarWithButtonsForm calendarWithButtonsForm = new MonthCalendarWithButtonsForm())
    {
      calendarWithButtonsForm.Text = LocalizationHolder.rm.GetString("msgChooseEndLinkDate");
      if (calendarWithButtonsForm.ShowDialog() != DialogResult.OK)
        return;
      selectionStart = calendarWithButtonsForm.MonthCalendar.SelectionStart;
    }
    FilterDateAttributesCommand.SetEndDateAttributeToObjects(items, (object) selectionStart);
  }

  /// <summary>Команда меню "Удалить сроки действия связи"</summary>
  /// <param name="items"></param>
  /// <param name="viewServices"></param>
  /// <param name="additionalInfo"></param>
  public static void RemoveDateAttributesCommandHandler(
    ISelectedItems items,
    IServiceProvider viewServices,
    object additionalInfo)
  {
    FilterDateAttributesCommand.SetEndDateAttributeToObjects(items, (object) DeleteModesEnum.None);
  }

  /// <summary>Обработчик команды выбора даты для фильтрации</summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  public static void ChooseFilterDateCommandHandler(object sender, EventArgs e)
  {
    if (!(sender is MenuItemBase menuItemBase))
      return;
    IFiltrationService service = ApplicationServices.Container.GetService<IFiltrationService>();
    object tag = service.Filtration.Tags[(object) "85357DBA-2685-4F94-8B40-7889D08B322A"];
    FilterDateAttributesCommand.FilterByDateInComposition = tag != null ? Convert.ToDateTime(tag) : DateTime.Now;
    using (MonthCalendarWithButtonsForm calendarWithButtonsForm = new MonthCalendarWithButtonsForm())
    {
      calendarWithButtonsForm.Text = LocalizationHolder.rm.GetString("msgChooseDate");
      calendarWithButtonsForm.MonthCalendar.SetDate(FilterDateAttributesCommand.FilterByDateInComposition);
      if (calendarWithButtonsForm.ShowDialog() != DialogResult.OK)
        return;
      FilterDateAttributesCommand.FilterByDateInComposition = calendarWithButtonsForm.MonthCalendar.SelectionStart;
      service.Filtration.Tags[(object) "85357DBA-2685-4F94-8B40-7889D08B322A"] = (object) FilterDateAttributesCommand.FilterByDateInComposition;
    }
    if (!(menuItemBase.Parent is DropDownMenuItem parent))
      return;
    parent.ToolTipText = FilterDateAttributesCommand.FilterByDateInComposition.ToShortDateString();
    if (parent.Checked)
      FilterDateAttributesCommand.ApplyFilterDateCommandHandler((object) parent, e);
    else
      menuItemBase.Parent.PerformClick();
  }

  /// <summary>
  /// Обработчик команды включения/выключения фильтрации по дате
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  public static void ApplyFilterDateCommandHandler(object sender, EventArgs e)
  {
    FilterDateAttributesCommand.FilterByDateInCompositionEnabled = sender is DropDownMenuItem dropDownMenuItem && dropDownMenuItem.Checked;
    IFiltrationService service = ApplicationServices.Container.GetService<IFiltrationService>();
    service.Filtration.Tags[(object) "CC4B5C20-3E62-4436-89E8-699262510FD5"] = (object) FilterDateAttributesCommand.FilterByDateInCompositionEnabled;
    service.FiltrationApplyUpdates(true);
    ApplicationServices.Container.GetService<ICommandManager>().QueryStatus();
  }
}
