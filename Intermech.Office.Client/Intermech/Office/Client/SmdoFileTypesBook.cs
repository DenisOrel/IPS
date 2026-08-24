// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.SmdoFileTypesBook
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;
using System.Data;

#nullable disable
namespace Intermech.Office.Client;

public class SmdoFileTypesBook : SmdoBook
{
  public override void LoadBook() => this.LoadBook(new Guid(SmdoBookConsts.smdoFileTypesBook));

  public bool ExtensionToName(string ext, out string name)
  {
    name = string.Empty;
    ext = ext.ToLower();
    DataRow[] dataRowArray = this.dataTable.Select($"{Tag.colExtension}='{ext}'");
    if (dataRowArray == null || dataRowArray.Length == 0)
      return false;
    name = Convert.ToString(dataRowArray[0][Tag.colName]);
    return true;
  }
}
