// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpAVSSettings
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки настроек спецификаций", "Перекачка настроек спецификаций")]
public class PumpAVSSettings : PumpClass
{
  protected SearchDataPlugin plugin;
  private CacheCategory _AVSSettingsCache;
  private int _settingsAttrID;
  private int _settingsFileTypesID;
  private int _defIniFileID;
  private int _oldAvsIniListAttr;
  private int _vedomostiTypeID;

  protected override Guid GUID => new Guid("{D6FA0522-0B0D-4548-AC9E-1BB2CDEED73A}");

  public PumpAVSSettings(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  protected CacheCategory AVSSettingsCache
  {
    get
    {
      if (this._AVSSettingsCache == null)
        this._AVSSettingsCache = PumpCache.Category[ImportingCategory.AVSSettings];
      return this._AVSSettingsCache;
    }
  }

  private void CopyAVSSettings(
    string registryPath,
    Guid dstObject,
    SpecificationSections sections,
    string iniSection,
    int spSectionTypeID)
  {
    RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey(registryPath);
    if (registryKey1 == null)
      return;
    IDBObject dbObject1 = BasePumpHelper.Session.GetObject(dstObject, false);
    if (dbObject1 == null)
      return;
    long objectId = dbObject1.ObjectID;
    IDBAttribute dbAttribute1 = dbObject1.GetAttributeByID(this._settingsAttrID);
    if (dbAttribute1 != null)
      dbAttribute1.ClearValues();
    else
      dbAttribute1 = dbObject1.Attributes.AddAttribute(this._settingsAttrID, false);
    IDBAttribute dbAttribute2 = dbObject1.GetAttributeByID(this._settingsFileTypesID);
    if (dbAttribute2 != null)
      dbAttribute2.ClearValues();
    else
      dbAttribute2 = dbObject1.Attributes.AddAttribute(this._settingsFileTypesID, false);
    IBlobWriter blobWriter = (IBlobWriter) null;
    try
    {
      string str1 = registryKey1.GetValue("CURRSP", (object) "").ToString();
      dbObject1.Attributes.AddAttribute(this._defIniFileID, false, new object[1]
      {
        (object) str1
      });
      List<string> stringList1 = new List<string>();
      foreach (string subKeyName in registryKey1.GetSubKeyNames())
      {
        RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyName);
        if (registryKey2 != null)
        {
          string str2 = registryKey2.GetValue("INIFILE", (object) "").ToString().Trim();
          string newValue = registryKey2.GetValue("FTYPE", (object) "").ToString();
          if (!stringList1.Contains(str2))
          {
            stringList1.Add(str2);
            byte[] numArray = new byte[0];
            if (File.Exists(str2))
            {
              FileStream fileStream = new FileStream(str2, FileMode.Open);
              try
              {
                numArray = new byte[fileStream.Length];
                fileStream.Read(numArray, 0, (int) fileStream.Length);
              }
              finally
              {
                fileStream.Close();
              }
              IniFile iniFile = new IniFile(str2);
              List<string> stringList2 = iniFile.ReadSection(iniSection);
              int result = 0;
              foreach (string str3 in stringList2)
              {
                if (int.TryParse(str3, out result))
                {
                  string caption = iniFile.IniReadValue(iniSection, str3).Trim();
                  SpecificationSection specificationSection = (SpecificationSection) null;
                  IDBObject dbObject2;
                  if (!sections.TryGetValue(result, out specificationSection))
                  {
                    dbObject2 = BasePumpHelper.Session.GetObjectCollection(spSectionTypeID).Create();
                    dbObject2.Caption = caption;
                    dbObject2.Attributes.AddAttribute(PumpHelper.AttrSPSectionNumID, false, new object[1]
                    {
                      (object) result
                    });
                    dbObject2.CommitCreation(false);
                    specificationSection = new SpecificationSection(dbObject2.ObjectID, caption);
                    if (sections == PumpHelper.SpecificationSections)
                      PumpHelper.SpecificationSections.Add(result, specificationSection);
                  }
                  else
                    dbObject2 = BasePumpHelper.Session.GetObject(specificationSection.ObjectID, false);
                  IDBAttribute dbAttribute3 = dbObject2.GetAttributeByID(this._oldAvsIniListAttr);
                  if (dbAttribute3 != null)
                  {
                    if (specificationSection.Tag == null)
                      dbAttribute3.ClearValues();
                  }
                  else
                    dbAttribute3 = dbObject2.Attributes.AddAttribute(this._oldAvsIniListAttr, false);
                  string fileName = Path.GetFileName(str2);
                  if (specificationSection.Tag == null)
                    dbAttribute3.AsString = fileName;
                  else
                    dbAttribute3.AddValue((object) fileName);
                  specificationSection.Tag = (object) 1;
                }
              }
            }
            if (blobWriter == null)
            {
              blobWriter = dbAttribute1 as IBlobWriter;
              dbAttribute2.AsString = newValue;
            }
            else
            {
              dbAttribute1.AddValue((object) null);
              dbAttribute2.AddValue((object) newValue);
            }
            BlobInformation blobInfo = new BlobInformation((long) numArray.Length, (long) numArray.Length, DateTime.Now, Path.GetFileName(str2), ArcMethods.NotPacked, subKeyName);
            if (blobWriter.OpenBlob(blobInfo, false))
              blobWriter.WriteDataBlock(numArray);
          }
        }
      }
    }
    finally
    {
      registryKey1.Close();
    }
  }

  public override void Exam() => this.ExamCheckPoint("Проверка данных успешно завершена", 100);

  private void doPump()
  {
    this._settingsAttrID = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad002a1-306c-11d8-b4e9-00304f19f545")).ID;
    this._settingsFileTypesID = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad002a3-306c-11d8-b4e9-00304f19f545")).ID;
    this._defIniFileID = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad002a4-306c-11d8-b4e9-00304f19f545")).ID;
    this._oldAvsIniListAttr = this.plugin.Imdi.AttributeTypes.GetByGuid(new Guid("cad002a8-306c-11d8-b4e9-00304f19f545")).ID;
    this._vedomostiTypeID = this.plugin.Imdi.ObjectTypes.GetByGuid(new Guid("cad002a7-306c-11d8-b4e9-00304f19f545")).ID;
    try
    {
      this.PumpCheckPoint("Перекачка настроек спецификаций", 0);
      this.CopyAVSSettings("Software\\Intermech\\AVS5\\DOC", new Guid("cad002a2-306c-11d8-b4e9-00304f19f545"), PumpHelper.SpecificationSections, "S4PRJ_SECTION", PumpHelper.ObjTypeSpecificationSectionID);
      int key = 0;
      SpecificationSections sections = new SpecificationSections();
      IDBObjectCollection objectCollection = BasePumpHelper.Session.GetObjectCollection(this._vedomostiTypeID);
      DBRecordSetParams paramSet = new DBRecordSetParams((ConditionStructure[]) null, new object[2]
      {
        (object) -2,
        (object) -50
      }, 0L, (object) null, -1);
      foreach (DataRow row in (InternalDataCollectionBase) objectCollection.Select(paramSet).Rows)
      {
        SpecificationSection specificationSection = new SpecificationSection((long) Convert.ToInt32(row[0]), row[1].ToString());
        sections.Add(key, specificationSection);
        ++key;
      }
      this.PumpCheckPoint("Перекачка настроек ведомостей", 50);
      this.CopyAVSSettings("Software\\Intermech\\AVS5\\VED", new Guid("cad002a6-306c-11d8-b4e9-00304f19f545"), sections, "RazdelsListV", this._vedomostiTypeID);
      this.PumpCheckPoint("Перекачка настроек спецификаций успешно завершена", 100);
    }
    catch (Exception ex)
    {
      BasePumpHelper.AppManager.AddWarningMessage(ex.Message);
    }
  }

  public override void Pump() => this.doPump();
}
