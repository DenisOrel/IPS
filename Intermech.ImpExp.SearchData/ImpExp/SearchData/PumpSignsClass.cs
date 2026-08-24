// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpSignsClass
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.Interfaces;
using Intermech.Signs;
using Intermech.Signs.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки подписей", "Перекачка подписей")]
public class PumpSignsClass : PumpClass
{
  protected SearchDataPlugin plugin;
  private CacheCategory _docsCache;
  private CacheCategory _signsCache;
  private IImportedObjectList _iol;
  private Dictionary<string, string> _graphIDs;
  private List<SignInfo> _signsToCache = new List<SignInfo>();

  protected override Guid GUID => new Guid("{184DA464-4A5A-4c09-9618-67DEC788DB24}");

  public PumpSignsClass(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  public override void Exam() => this.ExamCheckPoint("Проверка данных успешно завершена", 100);

  protected IImportedObjectList Iol
  {
    get
    {
      if (this._iol == null)
        this._iol = this.plugin.Idw.CreateImportedObjectList(0);
      return this._iol;
    }
  }

  private void CheckDataPacket(bool ForcePump)
  {
    if (!ForcePump && this.Iol.Items.Count < BasePumpHelper.PacketSize || this._iol == null)
      return;
    this._iol.Import();
    int count = this._iol.Items.Count;
    for (int index = 0; index < count; ++index)
    {
      ImportingObject importingObject = this._iol.Items[index];
      SignInfo signInfo = this._signsToCache[index];
      this._signsCache.AddValue((object) signInfo.SignID, importingObject.Object.Object_id, (ITagImportObject) new SignTag(signInfo.ObjectID));
    }
    this._iol.Items.Clear();
    this._signsToCache.Clear();
    BlobHelper.Reset();
  }

  public string GraphToID(string graph)
  {
    if (this._graphIDs == null)
    {
      this._graphIDs = new Dictionary<string, string>();
      foreach (IAttributePossibleValue possibleValue in this.plugin.Imdi.AttributeTypes.GetByID(SignsHolder.GraphAttrTypeID).GetPossibleValues())
        this._graphIDs[possibleValue.Description] = possibleValue.ValueString;
    }
    string id = "";
    this._graphIDs.TryGetValue(graph, out id);
    return id;
  }

  public override void Pump()
  {
    SimpleLogger logger = BasePumpHelper.Logger;
    this._docsCache = PumpCache.Category[ImportingCategory.Documents];
    this._signsCache = PumpCache.Category[ImportingCategory.Signs];
    try
    {
      using (IDbCommand command = this.plugin.idb2.CreateCommand())
      {
        this.PumpCheckPoint("Определение количества подписей для перекачки", 0);
        string str = "where old=0 and sign_type = 'A'";
        command.CommandText = "select count(*) from signlist " + str;
        int int32_1 = Convert.ToInt32(command.ExecuteScalar());
        logger.Write($"{command.CommandText}: {int32_1} result(s)");
        command.CommandText = "select sign_id, docsgn_id, version_id, usersgn_id, sign_date, sign_as, file_size, file_date, filesize2, checksum, note, addinfo from signlist " + str;
        IDataReader reader = command.ExecuteReader();
        try
        {
          int index = 1;
          string format = "Перекачка подписей ({0} из {1})";
          while (reader.Read())
          {
            this.PumpCheckPoint(string.Format(format, (object) index, (object) int32_1), this.CalculatePercent(int32_1, index, 1, 99));
            logger.Flush();
            try
            {
              int int32_2 = BasePumpHelper.ToInt32(reader[0]);
              if (this._signsCache.GetNewKey((object) int32_2) == 0L)
              {
                int int32_3 = BasePumpHelper.ToInt32(reader[1]);
                int int32_4 = BasePumpHelper.ToInt32(reader[2]);
                DictionaryValue dictionaryValue1 = this._docsCache.GetValue((object) int32_3);
                if (dictionaryValue1 != null)
                {
                  long num1 = 0;
                  DocumentTag tag = dictionaryValue1.Tag as DocumentTag;
                  if (tag.Versions.TryGetValue(int32_4, out num1))
                  {
                    this.CheckDataPacket(false);
                    SignInfo signInfo = new SignInfo();
                    signInfo.SignID = int32_2;
                    signInfo.ObjectID = num1;
                    signInfo.UserID = BasePumpHelper.ToInt32(reader[3]);
                    object fldvalue = reader[4];
                    BasePumpHelper.FixDateTimeField(ref fldvalue);
                    if (fldvalue != null)
                      signInfo.SignDT = (DateTime) fldvalue;
                    DictionaryValue dictionaryValue2 = BasePumpHelper.RanksCache.GetValue((object) Convert.ToChar(reader[5]));
                    if (dictionaryValue2 != null)
                    {
                      string caption = dictionaryValue2.Caption;
                      long newObjectId = dictionaryValue2.NewObjectID;
                      if (caption == "")
                      {
                        BasePumpHelper.AppManager.AddWarningMessage($"Пустая графа для подписи не допускается, невозможно восстановить подпись (SIGN_ID={int32_2}).");
                      }
                      else
                      {
                        string id = this.GraphToID(caption);
                        if (id == null)
                        {
                          BasePumpHelper.AppManager.AddWarningMessage($"Недопустимое значение графы для подписи ({caption}), невозможно восстановить подпись (SIGN_ID={int32_2}).");
                        }
                        else
                        {
                          SearchSign searchSign = new SearchSign(reader);
                          AddVersionInfo addVersionInfo = tag.AddVersionInfo[int32_4];
                          DateTime modifDate = addVersionInfo.ContentModifiedDate;
                          DateTime signDt = signInfo.SignDT;
                          int fileSize = addVersionInfo.FileSize;
                          DateTime fileDate = addVersionInfo.FileDate;
                          long advanFilesDate = addVersionInfo.AdvanFilesDate;
                          int num2 = searchSign.Validate(fileSize, fileDate, advanFilesDate) ? 1 : 0;
                          this._signsToCache.Add(signInfo);
                          this.Iol.AddObject(SignsHolder.SignObjectTypeID, 0, "Электронная подпись объекта").ObjCreate = signDt;
                          this._iol.AddAttributeInt(SignsHolder.InArchiveAttrTypeID, 1L);
                          this._iol.AddAttributeInt(SignsHolder.SignVersionAttrTypeID, 1L);
                          this._iol.AddAttributeDate(SignsHolder.DateOfSignatureID, signDt);
                          this._iol.AddAttributeStr(SignsHolder.GraphAttrTypeID, id);
                          if (num2 == 0)
                            modifDate = modifDate.AddDays(-1.0);
                          this._iol.AddAttributeDate(SignsHolder.ModifyDateAttrTypeID, modifDate);
                          this._iol.AddAttributeLink(SignsHolder.RankAttrTypeID, newObjectId, BasePumpHelper.GetNewRankCaption(newObjectId));
                          PumpHelper.AddUserLink(this._iol, SignsHolder.SignUpAttrTypeID, signInfo.UserID);
                          string resolution = "";
                          if (!reader.IsDBNull(10))
                            resolution = reader.GetString(10);
                          this._iol.AddAttributeStr(SignsHolder.ResolutionAttrTypeID, resolution);
                          byte[] inArray = HashPack.CalcHash(new HashPack(id, this.plugin.Imdi.ImportedUsers.GetGUID(signInfo.UserID), modifDate, signDt, resolution).Pack());
                          this._iol.AddAttributeStr(SignsHolder.HashProtectionAttrTypeID, Convert.ToBase64String(inArray));
                          AttributesHelper.AddObligatoryObjectAttributes(BasePumpHelper.Session, this._iol);
                        }
                      }
                    }
                  }
                }
              }
            }
            finally
            {
              ++index;
            }
          }
          this.CheckDataPacket(true);
        }
        finally
        {
          reader.Close();
          BlobHelper.Clear();
        }
      }
      string format1 = "Привязка подписей к объектам ({0} из {1})";
      Dictionary<object, DictionaryValue> items = this._signsCache.Items;
      int count = items.Count;
      int index1 = 1;
      IImportedRelationList importedRelationList = this.plugin.Idw.CreateImportedRelationList();
      foreach (KeyValuePair<object, DictionaryValue> keyValuePair in items)
      {
        long newObjectId = keyValuePair.Value.NewObjectID;
        if (newObjectId != -1L)
        {
          SignTag tag = keyValuePair.Value.Tag as SignTag;
          this.PumpCheckPoint(string.Format(format1, (object) index1, (object) count), this.CalculatePercent(count, index1, 1, 99));
          importedRelationList.AddRelation(tag.SignedObjectID, newObjectId, SignsHolder.SignRelationTypeID);
          this._signsCache.SetNewKey(keyValuePair.Key, -1L);
        }
        ++index1;
      }
      importedRelationList.Import();
      this.PumpCheckPoint("Перекачка подписей успешно завершена", 100);
      logger.Write("=========Pump end\r\n\r\n");
    }
    catch (Exception ex)
    {
      logger.Write($"=========Pump abort ({ex.Message})\r\n\r\n");
      throw;
    }
    finally
    {
      this._docsCache.Release();
      this._signsCache.Release();
    }
  }
}
