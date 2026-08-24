// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.ImdocsStore
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public class ImdocsStore : FileStore
{
  private string _filePath = "";

  public ImdocsStore(AliasInfo ai)
    : base(ai)
  {
    this._filePath = ai[AliasData.FilePath];
  }

  protected override void CreateConnection(AliasInfo ai)
  {
    this._connection = FileStores.MainDBConnection;
  }

  protected override bool InternalWriteFileBody(IDataReader reader, string fileName)
  {
    if (reader.IsDBNull(7))
      return false;
    string str1 = reader.GetString(7);
    if (str1.Trim() == "")
      return false;
    string str2 = this._filePath + str1;
    if (!File.Exists(str2))
    {
      BasePumpHelper.AppManager.AddWarningMessage($"Файл \"{str2}\" сервера документов \"{this._alias}\" не найден");
      return false;
    }
    File.Copy(str2, fileName, true);
    return true;
  }

  public override string TableName => this._alias;

  public override string LinkedTableName => this.TableName + "lnk";

  public override string AddColumns => ", REALFN ";
}
