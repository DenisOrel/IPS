// Decompiled with JetBrains decompiler
// Type: Intermech.Statistics.StatisticNodeItem
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using Intermech.Statistics.Interfaces;
using System;

#nullable disable
namespace Intermech.Statistics;

public class StatisticNodeItem : IComparable<StatisticNodeItem>
{
  private long _objectID;
  private int _objectTypeID;
  private string _caption;
  private long _id;
  private CommandStatisticsTypesEnum _commandType;
  private string _guid;

  public long ObjectID
  {
    get => this._objectID;
    set => this._objectID = value;
  }

  public int ObjectTypeID
  {
    get => this._objectTypeID;
    set => this._objectTypeID = value;
  }

  public string Caption
  {
    get => this._caption;
    set => this._caption = value;
  }

  public long ID
  {
    get => this._id;
    set => this._id = value;
  }

  public CommandStatisticsTypesEnum CommandType
  {
    get => this._commandType;
    set => this._commandType = value;
  }

  public string StatObjectGuid
  {
    get => this._guid;
    set => this._guid = value;
  }

  public StatisticNodeItem(
    long objectID,
    long ID,
    string caption,
    int objectTypeID,
    CommandStatisticsTypesEnum commandType,
    string guid)
  {
    this._objectID = objectID;
    this._caption = caption;
    this._objectTypeID = objectTypeID;
    this._id = ID;
    this._commandType = commandType;
    this._guid = guid;
  }

  public StatisticNodeItem(string caption, int objectTypeID, long objectID, long ID)
  {
    this._objectID = objectID;
    this._caption = caption;
    this._objectTypeID = objectTypeID;
    this._id = ID;
  }

  public int CompareTo(StatisticNodeItem other)
  {
    return string.Compare(this.Caption, other.Caption, StringComparison.Ordinal);
  }
}
