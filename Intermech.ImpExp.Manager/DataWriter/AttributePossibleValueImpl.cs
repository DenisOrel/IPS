// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.DataWriter.AttributePossibleValueImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.DataWriter;
using System;

#nullable disable
namespace Intermech.ImpExp.Manager.DataWriter;

internal sealed class AttributePossibleValueImpl : IAttributePossibleValue
{
  public AttributePossibleValueImpl(
    int inList,
    string descr,
    string valueStr,
    int valueInt,
    double valueDbl,
    DateTime valueDat)
  {
    this.InListId = inList;
    this.Description = descr;
    this.ValueString = valueStr;
    this.ValueInteger = valueInt;
    this.ValueDouble = valueDbl;
    this.ValueDateTime = valueDat;
  }

  public AttributePossibleValueImpl(int inList, string descr, string valueStr)
    : this(inList, descr, valueStr, int.MinValue, double.MinValue, DateTime.MinValue)
  {
  }

  public AttributePossibleValueImpl(int inList, string descr, int valueInt)
    : this(inList, descr, (string) null, valueInt, double.MinValue, DateTime.MinValue)
  {
  }

  public AttributePossibleValueImpl(int inList, string descr, double valueDbl)
    : this(inList, descr, (string) null, int.MinValue, valueDbl, DateTime.MinValue)
  {
  }

  public AttributePossibleValueImpl(int inList, string descr, DateTime valueDat)
    : this(inList, descr, (string) null, int.MinValue, double.MinValue, valueDat)
  {
  }

  public int InListId { get; }

  public string Description { get; } = string.Empty;

  public string ValueString { get; }

  public int ValueInteger { get; } = int.MinValue;

  public double ValueDouble { get; } = double.MinValue;

  public DateTime ValueDateTime { get; } = DateTime.MinValue;
}
