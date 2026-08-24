// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Manager.CommonData.UsersImpl
// Assembly: Intermech.ImpExp.Manager, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 837A17E0-5EE6-46DB-9571-5E7918B22E69
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Manager.exe

using Intermech.ImpExp.Interface.CommonData;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Manager.CommonData;

internal class UsersImpl : IUsers
{
  private Dictionary<int, long> usersHT = new Dictionary<int, long>();

  public void AddUserIntoCache(int searchUserID, long newUserID)
  {
    if (this.usersHT.ContainsKey(searchUserID))
      return;
    this.usersHT.Add(searchUserID, newUserID);
  }

  public long GetNewUserID(int searchUserID)
  {
    return this.usersHT.ContainsKey(searchUserID) ? this.usersHT[searchUserID] : 0L;
  }

  public bool Reload() => true;
}
