// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.SearchScheme.RolePropertyEditor
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.Pdm.Compositions.SearchScheme;

internal sealed class RolePropertyEditor : UITypeEditor
{
  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider sp,
    object value)
  {
    long[] numArray = SelectionWindow.SelectObjects(LocalizationHolder.rm.GetString("Pdm_67"), LocalizationHolder.rm.GetString("Pdm_68"), MetaDataHelper.GetObjectTypeID("cad00007-306c-11d8-b4e9-00304f19f545"), SelectionOptions.Default | SelectionOptions.DisableMultiselect);
    if (numArray != null && numArray.Length == 1)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(numArray[0]);
        value = (object) new RoleAttProxy(objectInfo.VersionGuid, objectInfo.Caption);
      }
    }
    return value;
  }
}
