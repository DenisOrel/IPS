// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.UserRankInformation
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Interfaces;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Signs.Client;

public class UserRankInformation
{
  private long _rankID;
  private string _rankCaption = string.Empty;
  private List<string> _graphs;

  private UserRankInformation()
  {
  }

  public UserRankInformation(long rankID)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      QuickObjectInfo objectInfo = sessionKeeper.Session.GetObjectInfo(rankID);
      this._rankID = rankID;
      this._rankCaption = objectInfo.Caption;
      this._graphs = new List<string>();
    }
  }

  public long RankID => this._rankID;

  public string RankCaption => this._rankCaption;

  public List<string> Graphs => this._graphs;

  public static UserRankInformation Clone(UserRankInformation info)
  {
    return new UserRankInformation()
    {
      _rankID = info._rankID,
      _rankCaption = info._rankCaption,
      _graphs = new List<string>((IEnumerable<string>) info._graphs)
    };
  }

  public override int GetHashCode() => base.GetHashCode();

  public override bool Equals(object obj)
  {
    return obj is UserRankInformation ? (obj as UserRankInformation)._rankID.Equals(this._rankID) : base.Equals(obj);
  }

  public override string ToString() => this._rankCaption;
}
