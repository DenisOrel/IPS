// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.ImbaseKeyConvertor
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.TechCard;
using System;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal class ImbaseKeyConvertor
{
  private readonly int _imbaseKeyAttrId;
  private readonly object _syncObject = new object();
  private readonly IImportedObjectList _importedObjectList;
  private readonly SimpleLogger _logger;
  private readonly ImbaseKeyObjectCache _cache;
  private static ImbaseKeyConvertor _instance;

  private bool CanObjectCreate(Entity entity, string imbaseKey)
  {
    return entity != null && entity.RecordID == 23 && !string.IsNullOrEmpty(imbaseKey);
  }

  private DictionaryValue CreateObject(
    Entity entity,
    string imbaseKey,
    string caption,
    IImportingData importingData)
  {
    Guid objTypeGuid = Guid.Empty;
    switch (entity.Code)
    {
      case "%МТР":
        objTypeGuid = TechCardConsts.ObjectTypes.MarkaGUID;
        break;
      case "%ZAG":
        objTypeGuid = TechCardConsts.ObjectTypes.MaterialGUID;
        break;
    }
    if (objTypeGuid == Guid.Empty)
      return (DictionaryValue) null;
    if (string.IsNullOrEmpty(caption))
      caption = imbaseKey;
    ObjectRecord objectRecord = this._importedObjectList.AddObject(MetaDataHelper.GetObjectTypeID(objTypeGuid), 0, caption);
    objectRecord.ObjCreate = DateTime.Now.ToUniversalTime();
    objectRecord.ModifyDate = DateTime.Now;
    objectRecord.IsBaseVersion = true;
    this._importedObjectList.AddAttributeStr(this._imbaseKeyAttrId, imbaseKey);
    if (!ImbaseImportHelper.ImbaseKeyHandler(this._importedObjectList, MetaDataHelper.GetAttributeID((object) TechCardConsts.AttributeTypes.ImbaseObjectAttrGuid), MetaDataHelper.GetAttributeID((object) TechCardConsts.AttributeTypes.ImbaseCodeAttrGuid), imbaseKey, importingData))
    {
      string message = $"Код Imbase (Code = \"{imbaseKey}\") не найден в кэше Imbase";
      TechcardConsts.Plugin.appManager.AddNewWarningMessage(message);
    }
    ImportingObject importingObject = this._importedObjectList.Items[this._importedObjectList.Items.Count - 1];
    this._importedObjectList.Import();
    this._logger.Write($"Конвертация кода Imbase (Code={imbaseKey}) -> IPS (ObjectId = {importingObject.Object.Object_id} Caption = '{caption}')");
    return new DictionaryValue(importingObject.Object.Object_id, importingObject.Object.Caption, (ITagImportObject) null);
  }

  public ImbaseKeyConvertor(ImbaseKeyObjectCache cache)
  {
    this._cache = cache != null ? cache : throw new ArgumentNullException(nameof (cache));
    this._importedObjectList = TechcardConsts.Plugin.Idw.CreateImportedObjectList();
    this._importedObjectList.PacketSize = 1;
    this._imbaseKeyAttrId = MetaDataHelper.GetAttributeID((object) TechCardConsts.AttributeTypes.ImbaseKeyAttrGuid);
    this._logger = new SimpleLogger(Path.Combine(Application.StartupPath, "ImbaseKeyConvertion.log"));
  }

  public DictionaryValue ConvertValue(
    Entity entity,
    string imbaseKey,
    IImportingData importingData)
  {
    return this.ConvertValue(entity, imbaseKey, string.Empty, importingData);
  }

  public DictionaryValue ConvertValue(
    Entity entity,
    string imbaseKey,
    string caption,
    IImportingData importingData)
  {
    if (entity == null || entity.EntityReference == null || entity.EntityReference.Field != -2)
      return (DictionaryValue) null;
    if (string.IsNullOrEmpty(imbaseKey))
      return (DictionaryValue) null;
    lock (this._syncObject)
    {
      DictionaryValue objectInfo = this._cache.GetObjectInfo(imbaseKey);
      if (objectInfo != null)
        return objectInfo;
      if (this.CanObjectCreate(entity, imbaseKey))
        objectInfo = this.CreateObject(entity, imbaseKey, caption, importingData);
      if (objectInfo == null)
        return (DictionaryValue) null;
      this._cache.Add(imbaseKey, objectInfo);
      return objectInfo;
    }
  }

  public static ImbaseKeyConvertor Instance
  {
    get
    {
      if (ImbaseKeyConvertor._instance == null)
        ImbaseKeyConvertor._instance = new ImbaseKeyConvertor(new ImbaseKeyObjectCache());
      return ImbaseKeyConvertor._instance;
    }
    internal set => ImbaseKeyConvertor._instance = value;
  }
}
