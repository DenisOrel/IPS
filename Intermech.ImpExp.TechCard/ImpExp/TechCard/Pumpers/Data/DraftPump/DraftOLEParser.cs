// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Data.DraftPump.DraftOLEParser
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.RecordParser;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.IO;
using System;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Data.DraftPump;

internal class DraftOLEParser : TechRecordParserSimple
{
  private Guid _pumperGuid;
  private string _tableName;
  private const int BufferSize = 262144 /*0x040000*/;
  private static readonly byte[] Buffer = new byte[262144 /*0x040000*/];
  private static DraftOLEParser _instance;

  private object Parse_ByteArray(IDataReader dataReader, int fieldIndex)
  {
    if (dataReader.IsDBNull(fieldIndex))
      return (object) null;
    ImChunkedStream inStream = new ImChunkedStream();
    int fieldOffset = 0;
    bool flag = false;
    try
    {
      while (true)
      {
        int bytes = (int) dataReader.GetBytes(fieldIndex, (long) fieldOffset, DraftOLEParser.Buffer, 0, 262144 /*0x040000*/);
        if (bytes > 0)
        {
          int offset = 0;
          if (fieldOffset == 0 && bytes >= 4 && DraftOLEParser.Buffer[0] == (byte) 90 && DraftOLEParser.Buffer[1] == (byte) 76 && DraftOLEParser.Buffer[2] == (byte) 73 && DraftOLEParser.Buffer[3] == (byte) 66)
          {
            offset = 4;
            flag = true;
          }
          fieldOffset += bytes;
          inStream.Write(DraftOLEParser.Buffer, offset, bytes - offset);
        }
        else
          break;
      }
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Ошибка при чтении BLOB (key={dataReader.GetValue(0)}, tablename={this.TableName}): {ex.Message}");
      if (!(ex is OutOfMemoryException))
        return (object) null;
      throw;
    }
    if (inStream.Length == 0L)
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Данные не найдены в BLOB (key={dataReader.GetValue(0)}, tablename={this.TableName})");
      return (object) null;
    }
    IPackedStream service = (IPackedStream) ServicesManager.ServiceContainer.GetService(typeof (IPackedStream));
    try
    {
      if (flag)
      {
        ImChunkedStream outStream = new ImChunkedStream();
        try
        {
          service.UnpackStream((Stream) outStream, (Stream) inStream);
        }
        catch (Exception ex)
        {
          TechcardConsts.Plugin.appManager.AddWarningMessage($"Ошибка при распаковке BLOB (key={dataReader.GetValue(0)}, tablename={this.TableName}): {ex.Message}");
          if (!(ex is OutOfMemoryException))
            return (object) null;
          throw;
        }
        inStream.Close();
        outStream.Flush();
        inStream = outStream;
        inStream.Position = 0L;
      }
      int num = 0;
      byte[] buffer = new byte[8];
      inStream.Position = 0L;
      if (inStream.Read(buffer, 0, buffer.Length) == buffer.Length)
      {
        if (buffer[0] == (byte) 66 && buffer[1] == (byte) 68 && buffer[2] == (byte) 79 && buffer[3] == (byte) 67)
          num = 12;
        else if (flag)
          num = 8;
      }
      string tmpFileName = TechUtils.File.GetTmpFileName(this._pumperGuid);
      using (FileStream destination = new FileStream(tmpFileName, FileMode.OpenOrCreate, FileAccess.Write))
      {
        inStream.Position = (long) num;
        inStream.CopyTo((Stream) destination);
      }
      FileInfo fileInfo = new FileInfo(tmpFileName);
      return fileInfo.Exists ? (object) fileInfo : throw new Exception($"Файл {tmpFileName} не найден");
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage($"Ошибка при обработке BLOB (key={dataReader.GetValue(0)}, tablename={this.TableName}): {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    finally
    {
      inStream.Close();
    }
    return (object) null;
  }

  public override object Parse(IDataReader dataReader, int fieldIndex)
  {
    return dataReader.GetFieldType(fieldIndex).FullName == "System.Byte[]" ? this.Parse_ByteArray(dataReader, fieldIndex) : base.Parse(dataReader, fieldIndex);
  }

  public Guid PumperGuid
  {
    get => this._pumperGuid;
    set => this._pumperGuid = value;
  }

  public string TableName
  {
    get => this._tableName;
    set => this._tableName = value;
  }

  public static DraftOLEParser GetInstance(Guid pumperGuid, string tableName)
  {
    if (DraftOLEParser._instance == null)
    {
      DraftOLEParser draftOleParser = new DraftOLEParser();
      draftOleParser.ShowWarnings = true;
      DraftOLEParser._instance = draftOleParser;
    }
    DraftOLEParser._instance.PumperGuid = pumperGuid;
    DraftOLEParser._instance.TableName = tableName;
    return DraftOLEParser._instance;
  }
}
