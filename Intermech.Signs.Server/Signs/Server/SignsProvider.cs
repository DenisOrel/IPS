// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Server.SignsProvider
// Assembly: Intermech.Signs.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3ABC2C25-9F6B-4AA7-A176-FCB28F816B8C
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Signs.Server.dll

using System;

#nullable disable
namespace Intermech.Signs.Server;

internal class SignsProvider
{
  private SignsErrors _error;
  private long _signObjectID = -1;
  private string _graphValue = string.Empty;
  private DateTime _date = DateTime.Now;

  public SignsProvider(long signObjectID, string graphValue, DateTime modifyDate)
  {
    this._signObjectID = signObjectID;
    this._graphValue = graphValue;
    this._date = modifyDate;
  }

  public long SignObjectID => this._signObjectID;

  public string GraphValue => this._graphValue;

  public DateTime ModifyDate => this._date;

  public SignsErrors ErrorCode
  {
    get => this._error;
    set => this._error = value;
  }

  public override bool Equals(object obj)
  {
    if (!(obj is SignsProvider))
      return base.Equals(obj);
    SignsProvider signsProvider = obj as SignsProvider;
    return signsProvider._signObjectID.Equals(this._signObjectID) && signsProvider._graphValue.Equals(this._graphValue);
  }

  public override int GetHashCode() => base.GetHashCode();
}
