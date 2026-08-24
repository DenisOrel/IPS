// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.PumpImbaseBlobs
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Imbase.ItemFactories;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.Imbase;

[TaskDescription("Получение информации о изображениях Imbase", "Перекачка изображений Imbase")]
[TaskType(PumperType.MetaData)]
internal sealed class PumpImbaseBlobs(ImbasePlugin plugin) : PumpImbaseClass(plugin)
{
  internal static Guid guid = new Guid("{F756D940-892F-4c7b-9104-86A4BF331962}");

  protected override Guid GUID => PumpImbaseBlobs.guid;

  public override void Pump()
  {
    ICache service = ServicesManager.GetService(typeof (ICache)) as ICache;
    IImportingData cacheData = service.GetCache(ImportingCategory.ImbaseBlobs, ImportingCategory.ImbaseBlobToLibrary);
    try
    {
      int owner = 0;
      IUserSession userSession = this.plugin.Idw.GetUserSession();
      long libFolderObjectID = 0;
      IDBObject dbObject = userSession.GetObject(new Guid("cad0146e-306c-11d8-b4e9-00304f19f545"), false);
      libFolderObjectID = dbObject == null ? this.plugin.Idw.AddObject(ImbaseIDHelper.ObjTypeIdImageFolder, owner, "Импортированные из IMBASE", new Guid("cad0146e-306c-11d8-b4e9-00304f19f545")) : dbObject.ObjectID;
      this.PumpCheckPoint("Определение количества записей", 0);
      string format1 = "SELECT {0} FROM IM_BLOBS WHERE F_SOURCE NOT LIKE 'OLE Sketch%' AND F_SOURCE NOT LIKE 'SKETCH%'";
      int int32 = Convert.ToInt32(this.GetCustomExecuteScalar(string.Format(format1, (object) "COUNT(*)")));
      this.SetCountPumpRecords(int32);
      int index1 = 0;
      List<string> stringList = new List<string>();
      this.PumpCheckPoint("Получение данных из таблицы " + ImBlobsItemFactory.TableName, 1);
      IDataReader dataReader = this.GetDataReader(string.Format(format1, (object) "F_KEY,F_USED,F_SOURCE,F_HASH,F_BLOB"));
      try
      {
        string format2 = $"Импорт записи из таблицы {ImBlobsItemFactory.TableName} ({{0}} из {{1}})";
        ImBlobsItemFactory blobsItemFactory = new ImBlobsItemFactory(dataReader, this.plugin.Idw.AppManager);
        IImportedObjectList ioL = this.plugin.Idw.CreateImportedObjectList();
        List<IImBlobsItem> blobItems = new List<IImBlobsItem>((ServicesManager.GetService(typeof (IConfigurationService)) as IConfigurationService).Configuration.PacketSize);
        ioL.AfterImportEvent += (AfterImportEventDelegate) ((_param1, _param2) =>
        {
          for (int index2 = 0; index2 < blobItems.Count; ++index2)
          {
            if (ioL.Items[index2].Object.Object_id != 0L && ioL.Items[index2].Object.Object_id != -1L)
            {
              blobItems[index2].ObjectID = ioL.Items[index2].Object.Object_id;
              cacheData.AddValue(ImportingCategory.ImbaseBlobs, (object) blobItems[index2].Key, blobItems[index2].ObjectID, blobItems[index2].Source);
              if (cacheData.GetNewKey(ImportingCategory.ImbaseBlobToLibrary, (object) blobItems[index2].Key) == 0L)
              {
                IImportedRelationList importedRelationList = this.plugin.Idw.CreateImportedRelationList(0);
                importedRelationList.AddRelation(libFolderObjectID, blobItems[index2].ObjectID, ImbaseIDHelper.RelTypeIDImSimple);
                importedRelationList.Import();
                if (importedRelationList.Items[0].Relation.PrjLinkId != 0L && importedRelationList.Items[0].Relation.PrjLinkId != -1L)
                  cacheData.AddValue(ImportingCategory.ImbaseBlobToLibrary, (object) blobItems[index2].Key, importedRelationList.Items[0].Relation.PrjLinkId);
                else
                  this.plugin.appManager.AddWarningMessage($"Блоб Imbase {blobItems[index2].Key} не включен в библиотеку изображений");
              }
            }
            else
              this.plugin.appManager.AddWarningMessage($"Блоб Imbase {blobItems[index2].Key} не импортирован");
          }
          blobItems.Clear();
        });
        while (dataReader.Read())
        {
          ++index1;
          this.PumpCheckPoint(string.Format(format2, (object) index1, (object) int32), this.CalculatePercent(int32, index1, 2, 99));
          IImBlobsItem imBlobsItem = blobsItemFactory.NewItem(dataReader);
          try
          {
            if (cacheData.GetNewKey(ImportingCategory.ImbaseBlobs, (object) imBlobsItem.Key) == 0L)
            {
              if (ImbasePlugin.IsBlobToPump(imBlobsItem.Key))
              {
                FileInfo fileInfo = new FileInfo(imBlobsItem.TmpFileName);
                string caption = fileInfo.Extension != string.Empty ? fileInfo.Name.Replace(fileInfo.Extension, string.Empty) : fileInfo.Name;
                if (imBlobsItem.BlobType == BlobType.Picture)
                {
                  ioL.AddObject(ImbaseIDHelper.ObjTypeIdImLibImage, owner, imBlobsItem.Source);
                  ioL.AddAttributeStr(ImbaseIDHelper.AttrIdName, imBlobsItem.Source);
                  ioL.AddAttributeBlob(ImbaseIDHelper.AttrIdLibraryImage, imBlobsItem.TmpFileName, imBlobsItem.FileSize, imBlobsItem.Source, imBlobsItem.IsZipped ? ArcMethods.ZLibPacked : ArcMethods.NotPacked);
                  ioL.AddAttributeInt(ImbaseIDHelper.AttrIdImCode, (long) imBlobsItem.Key);
                  AttributesHelper.AddObligatoryObjectAttributes(userSession, ioL);
                  blobItems.Add(imBlobsItem);
                }
                else if (imBlobsItem.BlobType == BlobType.Template)
                {
                  ioL.AddObject(ImbaseIDHelper.ObjTypeIdImTemplate, owner, caption);
                  ioL.AddAttributeStr(ImbaseIDHelper.AttrIdName, caption);
                  ioL.AddAttributeBlob(ImbaseIDHelper.AttrIdTemplateData, imBlobsItem.TmpFileName, imBlobsItem.FileSize, imBlobsItem.Source, imBlobsItem.IsZipped ? ArcMethods.ZLibPacked : ArcMethods.NotPacked);
                  ioL.AddAttributeInt(ImbaseIDHelper.AttrIdImCode, (long) imBlobsItem.Key);
                  AttributesHelper.AddObligatoryObjectAttributes(userSession, ioL);
                  blobItems.Add(imBlobsItem);
                }
                else
                {
                  ioL.AddObject(ImbaseIDHelper.ObjTypeIdBloвImbase, owner, caption);
                  ioL.AddAttributeStr(ImbaseIDHelper.AttrIdName, caption);
                  ioL.AddAttributeBlob(ImbaseIDHelper.AttrComentText, imBlobsItem.TmpFileName, imBlobsItem.FileSize, imBlobsItem.Source, imBlobsItem.IsZipped ? ArcMethods.ZLibPacked : ArcMethods.NotPacked);
                  ioL.AddAttributeInt(ImbaseIDHelper.AttrIdImCode, (long) imBlobsItem.Key);
                  AttributesHelper.AddObligatoryObjectAttributes(userSession, ioL);
                  blobItems.Add(imBlobsItem);
                }
              }
            }
          }
          finally
          {
            if (imBlobsItem.TmpFileName != string.Empty && new FileInfo(imBlobsItem.TmpFileName).Exists)
              stringList.Add(imBlobsItem.TmpFileName);
          }
        }
        ioL.Import();
      }
      finally
      {
        dataReader.Close();
        foreach (string str in stringList)
        {
          if (new FileInfo(str).Exists)
            File.Delete(str);
        }
      }
    }
    finally
    {
      service?.ReleaseCache(ImportingCategory.ImbaseBlobs, ImportingCategory.ImbaseBlobToLibrary);
    }
    this.PumpCheckPoint($"Обработка данных из таблицы {ImBlobsItemFactory.TableName} успешно завершена", 100);
  }
}
