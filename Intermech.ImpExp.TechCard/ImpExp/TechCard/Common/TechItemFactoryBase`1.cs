// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Common.TechItemFactoryBase`1
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.Interface;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.TechCard.Common;

internal abstract class TechItemFactoryBase<T> : PumpItemFactory
{
  public TechItemFactoryBase(string tableName, IDataReader dataReader)
    : base(tableName, dataReader, TechcardConsts.Plugin.appManager)
  {
  }

  public abstract T CreateItem(IDataReader dataReader);

  public override object NewItem(IDataReader dataReader) => (object) this.CreateItem(dataReader);
}
