// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.PumpFormsClass
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using System;
using System.Data;
using System.Text.RegularExpressions;

#nullable disable
namespace Intermech.ImpExp.SearchData;

[TaskDescription("Инициализация данных для перекачки форм", "Перекачка форм")]
[TaskType(PumperType.MetaData)]
public class PumpFormsClass : PumpClass
{
  protected SearchDataPlugin plugin;
  private CacheCategory _formsCache;
  private CacheCategory _artTypes;
  private CacheCategory _docTypes;

  protected override Guid GUID => new Guid("{6941BBCF-22FE-42CA-8CC8-8B2E228C393D}");

  public PumpFormsClass(SearchDataPlugin plugin)
    : base((PluginClass) plugin)
  {
    this.plugin = plugin;
  }

  protected CacheCategory FormsCache
  {
    get
    {
      if (this._formsCache == null)
        this._formsCache = PumpCache.Category[ImportingCategory.SearchForms];
      return this._formsCache;
    }
  }

  public override void Exam() => this.ExamCheckPoint("Проверка данных успешно завершена", 100);

  private void doPump()
  {
    this._artTypes = PumpCache.Category[ImportingCategory.ArticleTypes];
    this._docTypes = PumpCache.Category[ImportingCategory.DocTypes];
    try
    {
      this.PumpCheckPoint("Перекачка форм Search", 0);
      DataTable dataTable = new DataTable();
      using (IDbCommand command = this.plugin.idb2.CreateCommand())
      {
        command.CommandText = "select distinct formid, kind, id from formslinks";
        IDataReader reader = command.ExecuteReader(CommandBehavior.SequentialAccess);
        dataTable.Load(reader);
      }
      IMetadataInfo imdi = this.plugin.Imdi;
      using (IDbCommand command = this.plugin.idb2.CreateCommand())
      {
        command.CommandText = "select * from FORMSTABLE order by ID";
        IDataReader dataReader = command.ExecuteReader(CommandBehavior.SequentialAccess);
        try
        {
          IImportedObjectList importedObjectList = this.plugin.Idw.CreateImportedObjectList(0);
          while (dataReader.Read())
          {
            int int32 = BasePumpHelper.ToInt32(dataReader[0]);
            if (this.FormsCache.GetNewKey((object) int32) <= 0L)
            {
              string caption = dataReader.GetString(1);
              FormConverter formConverter = new FormConverter(this.ConvertCodes(BasePumpHelper.BlobToString(dataReader[2])));
              formConverter.OnConvertVarValue += new FormConverter.ConvertVarValue(this.FormConverter_OnConvertVarValue);
              try
              {
                BlobHelper.ReserveZBlob(new SaveToStreamDelegate(formConverter.SaveToStream));
              }
              catch (InvalidCastException ex)
              {
                BasePumpHelper.AppManager.AddErrorMessage($"Ошибка перекачки формы ({caption}): {ex.Message}");
              }
              importedObjectList.Items.Clear();
              importedObjectList.AddObject(PumpHelper.ObjTypeFormID, 0, caption);
              importedObjectList.AddAttributeStr(PumpHelper.AttrFormNameID, caption);
              int numInList = 0;
              foreach (DataRow dataRow in dataTable.Select($"[formid]={int32}"))
              {
                int newKey = (int) (!0.Equals(dataRow[1]) ? this._docTypes : this._artTypes).GetNewKey((object) Convert.ToInt32(dataRow[2]));
                if (newKey > 0)
                {
                  IObjectTypeItem byId = imdi.ObjectTypes.GetByID(newKey);
                  if (byId != null)
                  {
                    importedObjectList.AddAttribute(PumpHelper.AttrFormObjectTypesID, AttrValueType.stringVal, (object) byId.GUID.ToString(), numInList);
                    ++numInList;
                  }
                }
              }
              importedObjectList.AddAttributeBlob(PumpHelper.AttrFormBodyID, BlobHelper.TempFileName, BlobHelper.FileSize, "", ArcMethods.ZLibPacked);
              AttributesHelper.AddObligatoryObjectAttributes(BasePumpHelper.Session, importedObjectList);
              importedObjectList.Import();
              long objectId = importedObjectList.Items[0].Object.Object_id;
              this.FormsCache.AddValue((object) int32, objectId);
            }
          }
        }
        finally
        {
          dataReader.Close();
        }
      }
      this.PumpCheckPoint("Перекачка форм Search успешно завершена", 100);
    }
    catch (Exception ex)
    {
      BasePumpHelper.AppManager.AddWarningMessage(ex.Message);
      throw;
    }
    finally
    {
      this._docTypes.Release();
      this._artTypes.Release();
    }
  }

  public override void Pump() => this.doPump();

  private bool FormConverter_OnConvertVarValue(string Name, ref string Value, ref ComponentInfo ci)
  {
    int length = Value.IndexOf(';');
    if (length != -1)
    {
      Convert.ToInt32(Value.Substring(0, length));
      Value = Value.Substring(length + 1);
    }
    if (Value == "Наименование объекта")
      Value = "Наименование";
    else if (Value == "Обозначение объекта")
      Value = "Обозначение";
    IAttributeTypeItem byName = this.plugin.Imdi.AttributeTypes.GetByName(Value);
    if (byName == null)
      return false;
    if (byName.GetPossibleValues().Length != 0)
      ci = FormConverter.AttrComboBox;
    Value = byName.GUID.ToString();
    return true;
  }

  private string ConvertCodes(string str)
  {
    if (string.IsNullOrEmpty(str))
      return str;
    MatchCollection matchCollection = new Regex("#(?<code>\\d{4})").Matches(str);
    foreach (Match match in matchCollection)
    {
      int result;
      if (int.TryParse(match.Groups["code"].Value, out result))
        str = str.Replace(match.Value, Convert.ToString((char) result));
    }
    if (matchCollection.Count > 0)
      str = str.Replace("'", string.Empty);
    return str;
  }
}
