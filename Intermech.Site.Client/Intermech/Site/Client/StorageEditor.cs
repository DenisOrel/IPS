// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.StorageEditor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace Intermech.Site.Client;

internal sealed class StorageEditor : UITypeEditor
{
  private int? storageObjTypeID;

  public int StorageObjTypeID
  {
    get
    {
      if (!this.storageObjTypeID.HasValue)
        this.storageObjTypeID = new int?(MetaDataHelper.GetObjectTypeID("cad00014-306c-11d8-b4e9-00304f19f545"));
      return this.storageObjTypeID.Value;
    }
  }

  public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
  {
    return context != null && context.PropertyDescriptor.IsReadOnly ? UITypeEditorEditStyle.None : UITypeEditorEditStyle.Modal;
  }

  public override object EditValue(
    ITypeDescriptorContext context,
    IServiceProvider sp,
    object value)
  {
    long[] objects;
    if (value == null || !(value is StoragePropertyClass))
      objects = (long[]) null;
    else
      objects = new long[1]
      {
        ((StoragePropertyClass) value).Storage
      };
    IDBObjectID[] dbObjectIdArray = SelectorForm.SelectObjects(new int[1]
    {
      this.StorageObjTypeID
    }, objects, false, false);
    if (dbObjectIdArray != null)
      value = (object) new StoragePropertyClass(dbObjectIdArray[0].Value, dbObjectIdArray[0].Caption);
    return value;
  }
}
