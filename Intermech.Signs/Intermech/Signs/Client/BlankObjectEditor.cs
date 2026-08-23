// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.BlankObjectEditor
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Holders;
using Intermech.Interfaces;
using Intermech.PropertyEditors;
using System;
using System.Collections;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>редактор выбора бланков</summary>
internal class BlankObjectEditor : ObjectEditor
{
  private static int blanksObjTypeId;

  public BlankObjectEditor()
    : base(new EventsHolder.GetListDelegate(BlankObjectEditor.GetObjTypeList))
  {
  }

  private static ArrayList GetObjTypeList(object s, params object[] values)
  {
    if (BlankObjectEditor.blanksObjTypeId == 0)
      BlankObjectEditor.blanksObjTypeId = MetaDataHelper.GetObjectTypeID(new Guid("cad00134-306c-11d8-b4e9-00304f19f545"));
    return new ArrayList((ICollection) new int[1]
    {
      BlankObjectEditor.blanksObjTypeId
    });
  }
}
