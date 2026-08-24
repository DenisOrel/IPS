// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.FileStore
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public class FileStore
{
  protected IDbConnection _connection;
  protected string _alias = "";
  private static byte[] _emptyArray = new byte[0];
  public const int BufLen = 131072 /*0x020000*/;

  public FileStore(AliasInfo ai)
  {
    this._alias = ai[AliasData.Alias];
    this.CreateConnection(ai);
  }

  protected virtual void CreateConnection(AliasInfo ai)
  {
    this._connection = PumpHelper.Plugin.OpenDbConnection(this._alias, ai[AliasData.DBName], ai[AliasData.DBString], ai[AliasData.Type]);
    if (this._connection.State == ConnectionState.Closed)
      throw new Exception($"Не удалось подключиться к файловому шкафу '{this._alias}={ai[AliasData.DBString]}' ({ai[AliasData.Type]}), смотрите журнал ошибок.");
  }

  public IDbConnection Connection => this._connection;

  public virtual string TableName => this._alias;

  public virtual string LinkedTableName => "S4LINKED";

  public virtual string AddColumns => "";

  public virtual void WriteFileBody(IDataReader reader, string fileName)
  {
    if (this.InternalWriteFileBody(reader, fileName))
      return;
    using (File.Create(fileName))
      ;
  }

  protected virtual bool InternalWriteFileBody(IDataReader reader, string fileName)
  {
    if (reader.IsDBNull(6))
      return false;
    FileStream fileStream = new FileStream(fileName, FileMode.Create);
    try
    {
      long fieldOffset = 0;
      byte[] buffer = new byte[131072 /*0x020000*/];
      long bytes;
      do
      {
        bytes = reader.GetBytes(6, fieldOffset, buffer, 0, 131072 /*0x020000*/);
        fieldOffset += bytes;
        fileStream.Write(buffer, 0, (int) bytes);
      }
      while (bytes == 131072L /*0x020000*/);
    }
    finally
    {
      fileStream.Close();
    }
    return true;
  }
}
