// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.RecordParser.TechRecordParserMemo
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using System;
using System.Data;
using System.IO;
using System.Text;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.RecordParser;

internal class TechRecordParserMemo : TechRecordParserSimple
{
  private const int BufferSize = 1024 /*0x0400*/;
  private static readonly byte[] Buffer = new byte[1024 /*0x0400*/];
  private static TechRecordParserMemo _instance;

  private object Parse_ByteArray(IDataReader dataReader, int fieldIndex)
  {
    if (dataReader.IsDBNull(fieldIndex))
      return (object) null;
    string tmpFileName = TechUtils.File.GetTmpFileName(TechPumpBase.cnt_Common_Folder_Guid);
    using (FileStream output = new FileStream(tmpFileName, FileMode.OpenOrCreate, FileAccess.Write))
    {
      Encoding encoding = Encoding.GetEncoding(1251);
      BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
      int fieldOffset = 0;
      try
      {
        long bytes = dataReader.GetBytes(fieldIndex, (long) fieldOffset, TechRecordParserMemo.Buffer, 0, 1024 /*0x0400*/);
        byte[] numArray = new byte[bytes];
        Array.Copy((Array) TechRecordParserMemo.Buffer, (Array) numArray, bytes);
        while (bytes == 1024L /*0x0400*/)
        {
          char[] chars = encoding.GetChars(numArray);
          binaryWriter.Write(chars, 0, chars.Length);
          binaryWriter.Flush();
          fieldOffset += 1024 /*0x0400*/;
          bytes = dataReader.GetBytes(fieldIndex, (long) fieldOffset, TechRecordParserMemo.Buffer, 0, 1024 /*0x0400*/);
          numArray = new byte[bytes];
          Array.Copy((Array) TechRecordParserMemo.Buffer, (Array) numArray, bytes);
        }
        char[] chars1 = encoding.GetChars(numArray);
        binaryWriter.Write(chars1, 0, chars1.Length);
        binaryWriter.Flush();
      }
      catch (Exception ex)
      {
        TechcardConsts.Plugin.appManager.AddWarningMessage("Невозможно сохранить данные в файл." + ex.Message);
        if (ex is OutOfMemoryException)
          throw;
      }
      finally
      {
        binaryWriter.Close();
        output.Close();
      }
    }
    FileInfo fileInfo = new FileInfo(tmpFileName);
    return fileInfo.Exists ? (object) fileInfo : (object) null;
  }

  public override object Parse(IDataReader dataReader, int fieldIndex)
  {
    return dataReader.GetFieldType(fieldIndex).FullName == "System.Byte[]" ? this.Parse_ByteArray(dataReader, fieldIndex) : base.Parse(dataReader, fieldIndex);
  }

  public static TechRecordParserMemo GetInstance()
  {
    if (TechRecordParserMemo._instance == null)
    {
      TechRecordParserMemo recordParserMemo = new TechRecordParserMemo();
      recordParserMemo.ShowWarnings = true;
      TechRecordParserMemo._instance = recordParserMemo;
    }
    return TechRecordParserMemo._instance;
  }
}
