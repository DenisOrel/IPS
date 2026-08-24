// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpFinalizer
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для привязки файлов документов", "Привязка файлов документов")]
public class PumpFinalizer : PumpClass
{
  protected SearchDataPlugin plugin;

  protected override Guid GUID => new Guid("{012307F9-D831-46BE-8B12-4ACC10255F47}");

  public PumpFinalizer(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  public override void Pump()
  {
    if (this.plugin.BlobThread != null)
    {
      int progress = 0;
      while (this.plugin.BlobThread.IsAlive)
      {
        ++progress;
        if (progress >= 50)
          progress = 0;
        this.PumpCheckPoint($"Ожидание завершения фоновой закачки файлов документов (текущая скорость {this.plugin.BlobThread.BlobsPerSecond:0.0} ф/с, {this.plugin.BlobThread.MBPerSecond:0.0} МБ/с)", progress);
        Thread.Sleep(1000);
      }
    }
    if (BlobThread.ErrorMessage != "")
      throw new Exception($"Продолжение закачки невозможно, т.к. фоновая закачка документов была завершена с ошибкой: {BlobThread.ErrorMessage}.\r\nУстраните причину ошибки и перезапустите программу перекачки.");
    IDBImporter importer = this.plugin.Imdi.dbImporter;
    CacheCategory _docFiles = PumpCache.Category[ImportingCategory.DocFiles];
    try
    {
      int index = 0;
      long currentObjectID = 0;
      Dictionary<int, List<BlobAttributeValue>> attrblobs = (Dictionary<int, List<BlobAttributeValue>>) null;
      List<object> processedKeys = new List<object>();
      Action action = (Action) (() =>
      {
        if (attrblobs == null)
          return;
        foreach (KeyValuePair<int, List<BlobAttributeValue>> keyValuePair in attrblobs)
          importer.AddBlobAttribute(keyValuePair.Key, new Dictionary<long, BlobAttributeValue[]>()
          {
            {
              currentObjectID,
              keyValuePair.Value.ToArray()
            }
          });
        foreach (object oldKey in processedKeys)
          _docFiles.SetNewKey(oldKey, 1L);
        processedKeys.Clear();
      });
      int count = _docFiles.Items.Count;
      foreach (KeyValuePair<object, DictionaryValue> keyValuePair in _docFiles.Items)
      {
        if (keyValuePair.Value.NewObjectID == 0L)
        {
          BlobTag tag = keyValuePair.Value.Tag as BlobTag;
          if (currentObjectID != tag.ObjectID)
          {
            action();
            attrblobs = new Dictionary<int, List<BlobAttributeValue>>();
          }
          currentObjectID = tag.ObjectID;
          BlobAttributeValue blobAttributeValue = new BlobAttributeValue(tag.BlobID, keyValuePair.Value.Caption, tag.ModifyDate);
          List<BlobAttributeValue> blobAttributeValueList = (List<BlobAttributeValue>) null;
          if (!attrblobs.TryGetValue(tag.AttrID, out blobAttributeValueList))
          {
            blobAttributeValueList = new List<BlobAttributeValue>();
            attrblobs.Add(tag.AttrID, blobAttributeValueList);
          }
          blobAttributeValueList.Add(blobAttributeValue);
          ++index;
          this.PumpCheckPoint($"Привязка файлов документов ({index} из {count})", this.CalculatePercent(count, index, 1, 99));
          processedKeys.Add(keyValuePair.Key);
        }
      }
      action();
    }
    finally
    {
      _docFiles.Release();
    }
  }
}
