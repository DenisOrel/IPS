// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump.TechExpBasePump
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Intermech.Expert;
using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.DataWriter;
using Intermech.ImpExp.TechCard.Common;
using Intermech.ImpExp.TechCard.Common.LoadCache;
using Intermech.ImpExp.TechCard.TechExpPump.Common;
using Intermech.ImpExp.TechCard.TechExpPump.FormulaPump;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TechExpPump;

[TaskType(PumperType.MetaData)]
internal abstract class TechExpBasePump(PluginClass plugin) : PumpClass(plugin)
{
  protected int _impExpObjType;
  protected long _lastObjId;
  protected internal IImportingData _importingData;
  protected IImportedObjectList _impObjList;
  protected IImportedRelationList _impRelList;
  protected int _atNaimAttrTypeId;
  protected int _atObozAttrTypeId;
  protected int _atCommentTextAtrId;

  protected virtual void AnalyzeStoppedData()
  {
    this._lastObjId = 0L;
    if (!(ServicesManager.GetService(typeof (ICache)) is ICache service))
      return;
    IImportingData cache = service.GetCache(ImportingCategory.TechExpObjStruct);
    if (cache == null)
      return;
    try
    {
      DictionaryValue dictionaryValue = cache.GetValue(ImportingCategory.TechExpObjStruct, (object) this._impExpObjType);
      this._lastObjId = dictionaryValue != null ? dictionaryValue.NewObjectID : 0L;
      if (dictionaryValue != null)
        return;
      cache.AddValue(ImportingCategory.TechExpObjStruct, (object) this._impExpObjType, 0L);
    }
    finally
    {
      service.ReleaseCache(ImportingCategory.TechExpObjStruct);
    }
  }

  private string GetTempFileName() => TechUtils.File.GetTmpFileName(this.GUID);

  protected abstract bool NeedPumpExpData();

  protected virtual void LoadMetaData4Pump()
  {
    IMetadataInfo imdi = this.plugin.Imdi;
    if (imdi == null)
    {
      this.plugin.appManager.AddErrorMessage("Ошибка получения кэша метаданных");
    }
    else
    {
      IAttributeTypeItem byGuid1 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atCommentTextAtrGuid);
      if (byGuid1 != null)
        this._atCommentTextAtrId = byGuid1.ID;
      IAttributeTypeItem byGuid2 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atNaimAttrTypeGuid);
      if (byGuid2 != null)
        this._atNaimAttrTypeId = byGuid2.ID;
      IAttributeTypeItem byGuid3 = imdi.AttributeTypes.GetByGuid(TechcardConsts.TypeConsts.atObozAttrTypeGuid);
      if (byGuid3 == null)
        return;
      this._atObozAttrTypeId = byGuid3.ID;
    }
  }

  protected abstract void LoadExpertObjData();

  protected abstract void PumpExpertObjData();

  public void ConvertExpertData(
    short resType,
    FormulaList tcFormulaList,
    List<string> tcIdList,
    out TempFormula ipsFormulaData)
  {
    this.ConvertExpertData(resType, string.Empty, tcFormulaList, tcIdList, out ipsFormulaData);
  }

  public void ConvertExpertData(
    short resType,
    string resEntCode,
    FormulaList tcFormulaList,
    List<string> tcIdList,
    out TempFormula ipsFormulaData)
  {
    ipsFormulaData = (TempFormula) null;
    IAttributeTypeItem attrTypeItem = (IAttributeTypeItem) null;
    if (!resEntCode.Equals(string.Empty))
      attrTypeItem = TechExpert.TypeConverter.GetAttributeItemByCode(resEntCode, this.plugin, out string _);
    DataType resType1 = TechExpert.TypeConverter.ConvertFormulaResultType(resType, resEntCode, attrTypeItem);
    ipsFormulaData = resType1 != DataType.RelType ? new TempFormula(resType1) : throw new CommonDataTypeConvertException($"Ошибка приведения типа = \"{resType}\" (Код понятия = \"{resEntCode}\") к типу данных IPS");
    ipsFormulaData.Init();
    new ExpTokenConverter(this).ConvertTokens(resEntCode, tcFormulaList, tcIdList, ref ipsFormulaData);
    ipsFormulaData.AutoConvert = false;
    ipsFormulaData.DropMeasure = true;
    int BadToken;
    string errorMsg;
    if (!ipsFormulaData.Compile(out BadToken, out errorMsg))
    {
      string str = BadToken != -1 ? $"({ipsFormulaData[BadToken].text})" : string.Empty;
      throw new FormulaCompileException($"Ошибка компиляции формулы \"{ipsFormulaData.Text}\": {errorMsg} {str}");
    }
  }

  internal TempFormula CombineCondData(List<TempFormula> condList)
  {
    if (condList == null || condList.Count == 0)
      return (TempFormula) null;
    if (condList.Count == 1)
      return condList[0];
    TempFormula ipsFormula = new TempFormula();
    ipsFormula.Copy(condList[0]);
    ipsFormula.InsertToken(0, new Token(Intermech.Expert.TokenType.OpeningBrace, "("));
    ipsFormula.AddToken(new Token(Intermech.Expert.TokenType.OpeningBrace, "("));
    for (int index = 1; index < condList.Count; ++index)
    {
      ipsFormula.AddToken(new Token(Intermech.Expert.TokenType.BinaryOper, "И"));
      ipsFormula.AddToken(new Token(Intermech.Expert.TokenType.OpeningBrace, "("));
      foreach (Token t in condList[index].infixForm)
      {
        if (t.type == Intermech.Expert.TokenType.Attribute)
        {
          AttribPair usedAttr = condList[index].usedAttrs[t.info];
          if (usedAttr != null)
            ExpTokenConverter.CreateTokenAttribute(usedAttr.attribID, usedAttr.objTypeID, ref ipsFormula);
        }
        else
          ipsFormula.AddToken(new Token(t));
      }
      ipsFormula.AddToken(new Token(Intermech.Expert.TokenType.OpeningBrace, ")"));
    }
    ipsFormula.AutoConvert = false;
    ipsFormula.DropMeasure = true;
    int BadToken;
    string errorMsg;
    if (!ipsFormula.Compile(out BadToken, out errorMsg))
    {
      string str = BadToken != -1 ? $"({ipsFormula[BadToken].text})" : string.Empty;
      throw new FormulaCompileException($"Ошибка компиляции формулы \"{ipsFormula.Text}\": {errorMsg} ({str})");
    }
    return ipsFormula;
  }

  protected virtual AttributeRecord AddAttributeCondition(int attrTypeId, TempFormula condition)
  {
    if (condition == null)
      return (AttributeRecord) null;
    ImChunkedStream w = new ImChunkedStream();
    XmlTextWriter writer = new XmlTextWriter((Stream) w, Encoding.UTF8);
    condition.WriteToXML(ref writer);
    writer.Flush();
    ImChunkedStream baseOutputStream = new ImChunkedStream();
    Deflater deflater = new Deflater(3);
    DeflaterOutputStream destination1 = new DeflaterOutputStream((Stream) baseOutputStream, deflater);
    w.Position = 0L;
    w.CopyTo((Stream) destination1);
    destination1.Flush();
    destination1.Finish();
    string tempFileName = this.GetTempFileName();
    FileStream destination2 = new FileStream(tempFileName, FileMode.OpenOrCreate);
    long length;
    try
    {
      baseOutputStream.Position = 0L;
      baseOutputStream.CopyTo((Stream) destination2);
      destination2.Flush();
      length = destination2.Length;
    }
    finally
    {
      destination2.Close();
    }
    return this._impObjList.AddAttributeBlob(attrTypeId, tempFileName, length, string.Empty, ArcMethods.NotPacked);
  }

  protected virtual AttributeRecord AddAttributeComments(string comments)
  {
    if (string.IsNullOrEmpty(comments) || this._atCommentTextAtrId == 0)
      return (AttributeRecord) null;
    string tempFileName = this.GetTempFileName();
    try
    {
      System.IO.File.WriteAllText(tempFileName, comments);
      return this._impObjList.AddAttributeBlob(this._atCommentTextAtrId, tempFileName, (long) comments.Length, "Text", ArcMethods.NotPacked);
    }
    catch (Exception ex)
    {
      this.plugin.appManager.AddWarningMessage($"Ошибка создания записи атрибута комментария {this._atCommentTextAtrId}: {ex.Message}");
      if (ex is OutOfMemoryException)
        throw;
    }
    return (AttributeRecord) null;
  }

  protected virtual void ReleasePumpData()
  {
    this._importingData = (IImportingData) null;
    this._impObjList = (IImportedObjectList) null;
    this._impRelList = (IImportedRelationList) null;
    TechUtils.File.DeleteTmpFiles(this.GUID);
  }

  public override void Pump()
  {
    if (!this.NeedPumpExpData())
      return;
    try
    {
      if (TechCache.isResumeMode || this.IsMetadataPumper)
      {
        SavePoint savePoint = TechCache.SavePoint;
        if (savePoint != null && savePoint.PumpGuid == this.GUID && !savePoint.RePumpMode)
          this.AnalyzeStoppedData();
      }
      this.LoadMetaData4Pump();
      this.LoadExpertObjData();
      this.PumpExpertObjData();
    }
    finally
    {
      this.ReleasePumpData();
    }
  }
}
