// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.Materials
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Kernel.Search;
using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class Materials : IMaterials
{
  private IImportingData _cacheData;
  private int _materialTypeID = -1;
  private int _attrName;
  private int _attrDes;
  private int _attrAutoAdded;
  private int _attrImbaseKey;
  private IUserSession _session;

  public Materials(IUserSession session)
  {
    this._cacheData = (ServicesManager.GetService(typeof (ICache)) as ICache).GetCache(ImportingCategory.Materials, ImportingCategory.ImbaseMaterials);
    this._session = session;
    this._materialTypeID = session.GetObjectType(new Guid("cad00172-306c-11d8-b4e9-00304f19f545")).ObjectType;
    this._attrName = session.GetAttributeType(new Guid("cad00020-306c-11d8-b4e9-00304f19f545")).AttributeID;
    this._attrDes = session.GetAttributeType(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545")).AttributeID;
    this._attrAutoAdded = session.GetAttributeType(new Guid("cad00797-306c-11d8-b4e9-00304f19f545")).AttributeID;
    this._attrImbaseKey = session.GetAttributeType(new Guid("cad00162-306c-11d8-b4e9-00304f19f545")).AttributeID;
  }

  public MaterialInfo GetMaterial(string materialName) => this.GetMaterial(materialName, -1);

  public MaterialInfo GetMaterial(string materialName, int createType)
  {
    string materialName1 = "";
    string secondPart = "";
    int num = createType != -1 ? createType : this._materialTypeID;
    MaterialHelper.TrimMaterialNameString(materialName, ref materialName1, ref secondPart);
    if (materialName1 == "")
      materialName1 = materialName;
    long objectID = 0;
    if (secondPart != "")
    {
      DictionaryValue dictionaryValue = this._cacheData.GetValue(ImportingCategory.ImbaseMaterials, (object) secondPart);
      if (dictionaryValue != null)
        objectID = dictionaryValue.NewObjectID;
    }
    if (objectID == 0L)
      objectID = this._cacheData.GetNewKey(ImportingCategory.Materials, (object) this.CreateCacheKey(materialName1, num));
    if (objectID != 0L)
      return new MaterialInfo(objectID, materialName1);
    return createType == -1 ? (MaterialInfo) null : this.CreateNewMaterial(materialName1, secondPart, num);
  }

  public void LoadMaterialsFromBase(IUserSession session)
  {
    DataTable dataTable = session.GetObjectCollection(this._materialTypeID).Select(new DBRecordSetParams((ConditionStructure[]) null, new object[7]
    {
      (object) -2,
      (object) -50,
      (object) this._attrImbaseKey,
      (object) -3,
      (object) -7,
      (object) -12,
      (object) -18
    }));
    IMetadataInfo service = ServicesManager.GetService(typeof (IMetadataInfo)) as IMetadataInfo;
    for (int index = 0; index < dataTable.Rows.Count; ++index)
    {
      long int64 = Convert.ToInt64(dataTable.Rows[index][0]);
      string name = Convert.ToString(dataTable.Rows[index][1]);
      string oldKey = Convert.ToString(dataTable.Rows[index][2]);
      string cacheKey = this.CreateCacheKey(name, this._materialTypeID);
      if (this._cacheData.GetNewKey(ImportingCategory.Materials, (object) cacheKey) == 0L)
        this._cacheData.AddValue(ImportingCategory.Materials, (object) cacheKey, int64);
      if (oldKey != string.Empty && this._cacheData.GetNewKey(ImportingCategory.ImbaseMaterials, (object) oldKey) == 0L)
        this._cacheData.AddValue(ImportingCategory.ImbaseMaterials, (object) oldKey, int64);
      if (service != null && service.ImportedObjects.GetInfo(int64) == null)
      {
        string str1 = Convert.ToString(dataTable.Rows[index][5]);
        string str2 = Convert.ToString(dataTable.Rows[index][6]);
        service.ImportedObjects.AddValue(int64, Convert.ToInt64(dataTable.Rows[index][3]), Convert.ToInt32(dataTable.Rows[index][4]), GuidHelper.IsGuid(str1) ? new Guid(str1) : Guid.Empty, GuidHelper.IsGuid(str2) ? new Guid(str2) : Guid.Empty);
      }
    }
  }

  private string CreateCacheKey(string name, int type) => name.ToUpper();

  private MaterialInfo CreateNewMaterial(string name, string imbaseKey, int typeID)
  {
    IImportedObjectList importedObjectList = (ServicesManager.GetService(typeof (IDataWriter)) as IDataWriter).CreateImportedObjectList(0);
    importedObjectList.AddObject(typeID, 0, name);
    importedObjectList.AddAttributeStr(this._attrName, name);
    if (imbaseKey != string.Empty)
      importedObjectList.AddAttributeStr(this._attrImbaseKey, imbaseKey);
    AttributesHelper.AddObligatoryObjectAttributes(this._session, importedObjectList);
    importedObjectList.Import();
    if (importedObjectList.Items[0].Object == null || importedObjectList.Items[0].Object.Object_id <= 0L)
      return (MaterialInfo) null;
    long objectId = importedObjectList.Items[0].Object.Object_id;
    this._cacheData.AddValue(ImportingCategory.Materials, (object) this.CreateCacheKey(name, typeID), objectId);
    if (imbaseKey != "")
      this._cacheData.AddValue(ImportingCategory.ImbaseMaterials, (object) imbaseKey, objectId);
    return new MaterialInfo(objectId, name);
  }

  public void AddToCache(string materialName, string imbaseKey, int typeID, long objectID)
  {
    string cacheKey = this.CreateCacheKey(materialName, typeID);
    if (this._cacheData.GetNewKey(ImportingCategory.Materials, (object) cacheKey) == 0L)
      this._cacheData.AddValue(ImportingCategory.Materials, (object) cacheKey, objectID);
    if (string.IsNullOrEmpty(imbaseKey) || this._cacheData.GetNewKey(ImportingCategory.ImbaseMaterials, (object) imbaseKey) != 0L)
      return;
    this._cacheData.AddValue(ImportingCategory.ImbaseMaterials, (object) imbaseKey, objectID, (ITagImportObject) new MaterialTag(materialName));
  }
}
