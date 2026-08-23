// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.SignControlPropertyTypeEditor
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>Редактор для класса SignControlPropertyClass</summary>
internal class SignControlPropertyTypeEditor : UITypeEditor
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
    using (SignControlPropertyEditorForm propertyEditorForm = new SignControlPropertyEditorForm())
    {
      propertyEditorForm.SignControlPropertyClass = value as SignControlPropertyClass;
      return propertyEditorForm.ShowDialog().Equals((object) DialogResult.OK) ? (object) propertyEditorForm.SignControlPropertyClass : value;
    }
  }
}
