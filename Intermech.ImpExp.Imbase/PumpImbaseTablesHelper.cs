// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpImbaseTablesHelper
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.IO;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace Intermech.ImpExp.Imbase;

internal static class PumpImbaseTablesHelper
{
  private static string _tableIM_M_OBJS_PROPS = "IM_M_OBJS_PROPS";

  public static List<ImbaseGroup> GetTablesList(
    IImportingData cacheData,
    ImportingCategory filterCategory,
    int openmode)
  {
    Dictionary<object, DictionaryValue> category = cacheData.GetCategory(ImportingCategory.ImbaseGroups);
    List<ImbaseGroup> tablesList = new List<ImbaseGroup>(category.Count);
    foreach (DictionaryValue dictionaryValue in category.Values)
    {
      if (dictionaryValue.Tag is ImbaseGroup tag && PumpImbaseTablesHelper.FilterTableType(tag, openmode) && PumpImbaseTablesHelper.FilterCategory(tag, cacheData, filterCategory) && tag.TableName != PumpImbaseTablesHelper._tableIM_M_OBJS_PROPS)
        tablesList.Add(tag);
    }
    return tablesList;
  }

  private static bool FilterTableType(ImbaseGroup group, int openmode)
  {
    return group.TableType == 4 && group.Openmode == openmode && ImbasePlugin.IsTableToPump(group.TableName);
  }

  private static bool FilterCategory(
    ImbaseGroup group,
    IImportingData cacheData,
    ImportingCategory filterCategory)
  {
    return filterCategory == ImportingCategory.None || cacheData.GetNewKey(filterCategory, (object) group.TableName) == 0L;
  }

  public static IImportedObjectList GetImportedTableObjectList(
    ImbasePlugin plugin,
    ImbaseGroup table,
    IImportingData cacheData,
    int ownerUser,
    int objectType)
  {
    IImportedObjectList importedObjectList = plugin.Idw.CreateImportedObjectList(0);
    importedObjectList.AddObject(objectType, ownerUser, table.Description);
    importedObjectList.AddAttribute(ImbaseIDHelper.AttrIdName, AttrValueType.stringVal, (object) table.Description, 0);
    importedObjectList.AddAttribute(ImbaseIDHelper.AttrIdDescription, AttrValueType.stringVal, (object) table.Description, 0);
    importedObjectList.AddAttributeStr(ImbaseIDHelper.AttrIdTableName, table.TableName);
    importedObjectList.AddAttributeInt(ImbaseIDHelper.AttrIdImCode, (long) table.Key);
    if (plugin.imbaseBlobs != null)
    {
      DictionaryValue dictionaryValue = cacheData.GetValue(ImportingCategory.ImbaseBlobs, (object) table.GraphID);
      if (dictionaryValue != null)
        importedObjectList.AddAttributeLink(ImbaseIDHelper.AttrIdPicture, dictionaryValue.NewObjectID, dictionaryValue.Caption);
    }
    return importedObjectList;
  }

  public static void AddTableBlobAttribute(
    ImbaseGroup table,
    DataSet dataSet,
    IImportedObjectList iol,
    out string fileFullName)
  {
    fileFullName = Path.Combine(Path.GetTempPath(), $"{table.TableName}.tmp");
    long fileSize = 0;
    using (ImChunkedStream imChunkedStream = new ImChunkedStream())
    {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      dataSet.RemotingFormat = SerializationFormat.Binary;
      FileStream outStream = new FileStream(fileFullName, FileMode.Create, FileAccess.Write);
      try
      {
        binaryFormatter.Serialize((Stream) imChunkedStream, (object) dataSet);
        imChunkedStream.Position = 0L;
        ((IPackedStream) ServicesManager.ServiceContainer.GetService(typeof (IPackedStream))).PackStream((Stream) outStream, (Stream) imChunkedStream, 9);
      }
      finally
      {
        outStream.Flush();
        fileSize = outStream.Length;
        outStream.Close();
      }
    }
    iol.AddAttributeBlob(ImbaseIDHelper.AttrTableDataLength < fileSize ? ImbaseIDHelper.AttrLongTableData : ImbaseIDHelper.AttrTableData, fileFullName, fileSize, $"Записи таблицы IMBASE {table.TableName}", ArcMethods.ZLibPacked);
  }
}
