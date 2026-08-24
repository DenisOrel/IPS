// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ParentObjectTypeEditor
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Interface;

public class ParentObjectTypeEditor : ObjectTypeEditor
{
  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider sp,
    object value)
  {
    SelectionWindow selectionWindow = SelectionWindow.ShowWindow(4, SelectionWindowOptions.EmptyInclude);
    if (selectionWindow.ShowDialog() != DialogResult.OK)
      return value;
    return selectionWindow.SelectedGuid == SelectionWindow.SelectionWindowEmpty ? (object) new ParentObjectTypeAttProxy(Guid.Empty, "Не назначен") : (object) new ParentObjectTypeAttProxy(selectionWindow.SelectedGuid, selectionWindow.SelectedText);
  }
}
