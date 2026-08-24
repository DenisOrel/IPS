// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords.TechObjectRecordDynamic
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.RecordParser;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.Common.TechBaseObjPump.ObjectRecords;

internal class TechObjectRecordDynamic : TechObjectRecord
{
  private TechRecordParser _parser;

  public TechObjectRecordDynamic(string tableName = "", TechRecordParser parser = null)
  {
    this.TableName = tableName;
    this._parser = parser != null ? parser : (TechRecordParser) TechRecordParserMemo.GetInstance();
  }

  public override void Clear()
  {
    base.Clear();
    this._parser = (TechRecordParser) null;
  }

  public override void Assign(object source)
  {
    base.Assign(source);
    if (!(source is TechObjectRecordDynamic objectRecordDynamic))
      return;
    this._parser = objectRecordDynamic._parser;
  }

  protected virtual void AddFields(string fieldName, object value)
  {
    this._fields.Add(fieldName, value);
  }

  public override void Parse(IDataReader dataReader)
  {
    base.Parse(dataReader);
    for (int index = 0; index < dataReader.FieldCount; ++index)
    {
      object obj = this._parser.Parse(dataReader, index);
      if (obj != null)
        this.AddFields(dataReader.GetName(index), obj);
    }
  }
}
