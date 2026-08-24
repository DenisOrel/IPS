// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.SmdoDocKindsBook
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;
using System.Data;

#nullable disable
namespace Intermech.Office.Client;

public class SmdoDocKindsBook : SmdoBook
{
  public override void LoadBook() => this.LoadBook(new Guid(SmdoBookConsts.smdoDocKindsBook));

  public string IpsKindNameToSmdoKindName(string kindName)
  {
    foreach (DataRow row in (InternalDataCollectionBase) this.dataTable.Rows)
    {
      string smdoKindName = Convert.ToString(row[Tag.colName]);
      if (smdoKindName.Equals(kindName, StringComparison.InvariantCultureIgnoreCase))
        return smdoKindName;
    }
    return "ПИСЬМО";
  }
}
