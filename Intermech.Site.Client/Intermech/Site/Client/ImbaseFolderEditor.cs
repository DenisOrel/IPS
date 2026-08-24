// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.ImbaseFolderEditor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.DataFormats;
using Intermech.PropertyEditors;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.Site.Client;

public class ImbaseFolderEditor : UITypeEditor
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
    long[] numArray;
    if (value == null || !(value is ImbaseFolderPropertyClass))
      numArray = (long[]) null;
    else
      numArray = new long[1]
      {
        ((ObjectPropertyClass) value).ObjectID
      };
    long[] objects = numArray;
    IDBObjectID[] dbObjectIdArray = SelectorForm.SelectObjects(new int[2]
    {
      Intermech.Imbase.Consts.ImbaseFoldersID,
      Intermech.Imbase.Consts.ImbaseCatalogTypeID
    }, objects, false, false);
    if (dbObjectIdArray != null)
      value = (object) new ImbaseFolderPropertyClass(dbObjectIdArray[0].Value);
    return value;
  }
}
