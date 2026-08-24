// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.AllCountItemsForPV
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#nullable disable
namespace Intermech.MRP2;

/// <summary>
/// Чтение и запись данных по суммарному количеству на ПВ для каждой ПК ДСЕ состава
/// </summary>
public static class AllCountItemsForPV
{
  /// <summary>Guid типа атрибута для хранения данных</summary>
  private static readonly Guid _dataAttrTypeGuid = new Guid("cadd9c70-306c-11d8-b4e9-00304f19f545");
  /// <summary>Разделитель внутри строки данных одной ПК ДСЕ</summary>
  private static readonly char _separator = '|';

  /// <summary>
  /// Прочитать сохраненные данные по объему количеству на ПВ
  /// </summary>
  /// <param name="pvId">Id версии ПВ</param>
  /// <returns>Словарь данных по суммарному количеству ПК ДСЕ на всю ПВ</returns>
  public static Dictionary<long, object[]> ReadAllCount(long pvId)
  {
    Dictionary<long, object[]> dictionary = new Dictionary<long, object[]>();
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      if (!(sessionKeeper.Session.GetObjectAttributeByGuid(pvId, AllCountItemsForPV._dataAttrTypeGuid) is IDBShortBlobAttribute objectAttributeByGuid))
        return dictionary;
      ShortBlobValue blobValue = objectAttributeByGuid.GetBlobValue();
      if (blobValue.RealFileSize == 0L)
        return dictionary;
      MemoryStream inStream = new MemoryStream(blobValue.Value);
      MemoryStream outStream = blobValue.ArcMethod == ArcMethods.ZLibPacked ? new MemoryStream(Convert.ToInt32(blobValue.RealFileSize)) : inStream;
      try
      {
        if (blobValue.ArcMethod == ArcMethods.ZLibPacked)
          ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true).UnpackStream((Stream) outStream, (Stream) inStream);
        outStream.Position = 0L;
        foreach (string str1 in Encoding.UTF8.GetString(outStream.ToArray()).Split(Environment.NewLine.ToCharArray()))
        {
          char[] chArray = new char[1]
          {
            AllCountItemsForPV._separator
          };
          string[] source = str1.Split(chArray);
          foreach (string str2 in source)
          {
            string countItem = str2;
            long result;
            if (long.TryParse(countItem, out result))
            {
              object[] array = ((IEnumerable<string>) source).Where<string>((Func<string, bool>) (a => a != countItem)).Select<string, object>((Func<string, object>) (a => (object) a)).ToArray<object>();
              dictionary[result] = array;
              break;
            }
          }
        }
      }
      finally
      {
        inStream.Close();
        if (blobValue.ArcMethod == ArcMethods.ZLibPacked)
          outStream.Close();
      }
    }
    return dictionary;
  }

  /// <summary>Записать данные по объему количеству на ПВ</summary>
  /// <param name="pvId">Id версии ПВ</param>
  /// <param name="allCount">Словарь данных по суммарному количеству ПК ДСЕ на всю ПВ</param>
  /// <returns></returns>
  public static bool WriteAllCount(long pvId, Dictionary<long, object[]> allCount)
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (KeyValuePair<long, object[]> keyValuePair in allCount)
    {
      stringBuilder.Append(keyValuePair.Key);
      foreach (object obj in keyValuePair.Value)
      {
        if (!string.IsNullOrEmpty(obj.ToString()))
        {
          stringBuilder.Append(AllCountItemsForPV._separator);
          stringBuilder.Append(obj);
        }
      }
      stringBuilder.Append(Environment.NewLine);
    }
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBAttribute dbAttribute = sessionKeeper.Session.GetObject(pvId, false)?.Attributes.AddAttribute(AllCountItemsForPV._dataAttrTypeGuid, false);
      if (dbAttribute == null)
        return false;
      using (MemoryStream inStream = new MemoryStream(Encoding.UTF8.GetBytes(stringBuilder.ToString())))
      {
        using (MemoryStream outStream = new MemoryStream())
        {
          ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true).PackStream((Stream) outStream, (Stream) inStream, 9);
          if (!(dbAttribute is IBlobWriter blobWriter))
            return false;
          blobWriter.OpenBlob(new BlobInformation(inStream.Length, outStream.Length, DateTime.Now, string.Empty, ArcMethods.ZLibPacked, string.Empty), false);
          blobWriter.WriteDataBlock(outStream.ToArray());
          return true;
        }
      }
    }
  }
}
