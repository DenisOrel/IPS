// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.Controls.MaterialProperties.TableRefTypeEditor
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

using Intermech.Imbase.Selection;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Imbase;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.MaterialsHandbook.Controls.MaterialProperties;

internal class TableRefTypeEditor : UITypeEditor
{
  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider provider,
    object value)
  {
    object obj = value;
    if (!(ServicesManager.GetService(typeof (IImbaseSelector)) is ImbaseSelector service) || value == null)
      return obj;
    string strImbaseKey = value.ToString();
    string str = service.SelectRecord(strImbaseKey, false);
    return !string.IsNullOrEmpty(str) ? (object) str : obj;
  }
}
