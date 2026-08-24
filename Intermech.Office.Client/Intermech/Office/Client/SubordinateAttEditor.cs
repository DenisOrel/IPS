// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.SubordinateAttEditor
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using Intermech.DataFormats;
using Intermech.Diagnostics;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Client.PropertyEditors;
using Intermech.Kernel.Search;
using Intermech.Navigator;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Office.Interfaces;
using Intermech.PropertyEditors;
using Intermech.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;

#nullable disable
namespace Intermech.Office.Client;

internal sealed class SubordinateAttEditor : UITypeEditor
{
  private bool _multiValues;

  public SubordinateAttEditor(int attributeID)
  {
    IMSAttributeType attributeType = MetaDataHelper.GetAttributeType(attributeID);
    this._multiValues = attributeType.MultiValueMode == MultiValueModes.MultiValues || attributeType.MultiValueMode == MultiValueModes.MultiValuesFromList;
  }

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider sp,
    object value)
  {
    if (sp.GetService(typeof (IEditorDialogStyle)) is IEditorDialogStyle service)
      this._multiValues = service.MultiValues;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      List<long> objectIDs = (List<long>) null;
      DBRecordSetParams paramSet = new DBRecordSetParams(new ConditionStructure[1]
      {
        new ConditionStructure(OfficeConsts.AttrDirectorID, RelationalOperators.Equal, (object) sessionKeeper.Session.UserID, LogicalOperators.AND, 0, false)
      }, new object[2]{ (object) -2, (object) -7 });
      int num = OfficeConsts.ObjtypeOrganizationID;
      DataTable table1 = sessionKeeper.Session.GetObjectCollection(num).Select(paramSet);
      if (table1.Rows.Count > 0)
      {
        objectIDs = SubordinateAttEditor.GetListObjects(table1);
      }
      else
      {
        num = OfficeConsts.ObjtypeDepartmentID;
        DataTable table2 = sessionKeeper.Session.GetObjectCollection(num).Select(paramSet);
        if (table2.Rows.Count > 0)
          objectIDs = SubordinateAttEditor.GetListObjects(table2);
      }
      IDescriptor rootDescriptor = objectIDs == null || objectIDs.Count <= 0 ? (IDescriptor) new UsersGroupsDescriptor() : (IDescriptor) new ListDescriptor(OfficeClientConsts.CategorySubordinateRoot, num, Localization.GetString("Office.Client_77"), (IList) objectIDs);
      SelectionOptions options = SelectionOptions.SelectObjects;
      if (!this._multiValues)
        options |= SelectionOptions.DisableMultiselect;
      if (SelectionWindow.Select(Localization.GetString("Office.Client_15"), rootDescriptor, typeof (IDBTypedObjectID), options, MetaDataHelper.GetObjectTypeChildrenIDRecursive(OfficeConsts.ObjtypeUsersID).ToArray()) is IDBTypedObjectID[] source && source.Length != 0)
      {
        List<ObjectPropertyClass> list = ((IEnumerable<IDBTypedObjectID>) source).Select<IDBTypedObjectID, ObjectPropertyClass>((System.Func<IDBTypedObjectID, ObjectPropertyClass>) (x => new ObjectPropertyClass(x.ObjectID, x.Caption))).ToList<ObjectPropertyClass>();
        if (list.Count == 1)
          return (object) list[0];
        if (list.Count > 1)
          return (object) list.ToArray();
      }
      return value;
    }
  }

  [NotNull]
  private static List<long> GetListObjects([NotNull] DataTable table)
  {
    List<long> listObjects = new List<long>(table.Rows.Count);
    for (int index = 0; index < table.Rows.Count; ++index)
      listObjects.Add(Convert.ToInt64(table.Rows[index][0]));
    return listObjects;
  }
}
