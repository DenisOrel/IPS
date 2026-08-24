// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.Configurations.ControlHelper
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Interfaces;
using Intermech.Statistics.Interfaces;
using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Statistics.Configurations;

internal class ControlHelper
{
  public static void AutoResizeColumns(ListView lv)
  {
    lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
    ListView.ColumnHeaderCollection columns = lv.Columns;
    for (int index = 0; index < columns.Count; ++index)
    {
      int num = TextRenderer.MeasureText(columns[index].Text, lv.Font).Width + 10;
      if (num > columns[index].Width)
        columns[index].Width = num;
    }
  }

  public static string GetActivityTemplateCaption(long activityObjId, IUserSession session)
  {
    IDBAttribute objectAttributeById = session.GetObjectAttributeByID(activityObjId, wfConsts.AttrProcessID);
    if (objectAttributeById == null)
      return string.Empty;
    long asInteger = objectAttributeById.AsInteger;
    IDBObject dbObject = session.GetObject(asInteger, false);
    if (dbObject == null)
      return string.Empty;
    if (dbObject.VersionID == 0)
      return dbObject.Caption;
    return $"{dbObject.Caption} [{(object) dbObject.VersionID}]";
  }

  public static bool CanRemoveItems(int removingItemsAmount, string singular, string plural)
  {
    if (removingItemsAmount == 0)
      return false;
    string empty = string.Empty;
    return MessageBox.Show(removingItemsAmount != 1 ? $"Вы действительно хотите удалить {plural} из списка?" : $"Вы действительно хотите удалить {singular} из списка?", "Внимание!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK;
  }

  public static List<ItemWithDescription> GetPeriodEnumValueList()
  {
    return Enum.GetValues(typeof (CollectPeriodsEnum)).Cast<CollectPeriodsEnum>().Select<CollectPeriodsEnum, ItemWithDescription>((Func<CollectPeriodsEnum, ItemWithDescription>) (value => new ItemWithDescription(value))).OrderBy<ItemWithDescription, CollectPeriodsEnum>((Func<ItemWithDescription, CollectPeriodsEnum>) (item => item.Value)).ToList<ItemWithDescription>();
  }
}
