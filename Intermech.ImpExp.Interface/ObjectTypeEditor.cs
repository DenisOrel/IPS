// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.ObjectTypeEditor
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Summary description for ObjectTypeEditor.</summary>
public class ObjectTypeEditor : UITypeEditor
{
  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return context != null && context.PropertyDescriptor.IsReadOnly ? UITypeEditorEditStyle.None : UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider sp,
    object value)
  {
    SelectionWindow selectionWindow = SelectionWindow.ShowWindow(4);
    return selectionWindow.ShowDialog() == DialogResult.OK && selectionWindow.SelectedGuid != Guid.Empty ? (object) new ObjectTypeAttProxy(selectionWindow.SelectedGuid, selectionWindow.SelectedText) : value;
  }
}
