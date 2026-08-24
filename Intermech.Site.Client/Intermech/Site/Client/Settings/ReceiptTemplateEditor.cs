// Decompiled with JetBrains decompiler
// Type: Intermech.Site.Client.Settings.ReceiptTemplateEditor
// Assembly: Intermech.Site.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 45B3D0A4-42A5-477F-95CF-CC2F5C39B360
// Assembly location: D:\IPS\Client\Intermech.Site.Client.dll

using Intermech.Holders;
using Intermech.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.Collections;

#nullable disable
namespace Intermech.Site.Client.Settings;

internal sealed class ReceiptTemplateEditor : ObjectEditor
{
  private static int blanksObjTypeId;

  public ReceiptTemplateEditor()
    : base(new EventsHolder.GetListDelegate(ReceiptTemplateEditor.GetObjTypeList))
  {
  }

  private static ArrayList GetObjTypeList(object s, params object[] values)
  {
    if (ReceiptTemplateEditor.blanksObjTypeId == 0)
      ReceiptTemplateEditor.blanksObjTypeId = MetaDataHelper.GetObjectTypeID(new Guid("cad00134-306c-11d8-b4e9-00304f19f545"));
    return new ArrayList((ICollection) new int[1]
    {
      ReceiptTemplateEditor.blanksObjTypeId
    });
  }
}
