// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Search.UserInfo
// Assembly: Intermech.ImpExp.Search, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: DCC7C774-0788-47B1-BD86-E2BCE31689FD
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Search.dll

using Intermech.Interfaces;
using System;

#nullable disable
namespace Intermech.ImpExp.Search;

internal struct UserInfo(
  string firstName,
  string lastName,
  string fio,
  string workPhone,
  string homePhone,
  string roomNumber,
  string address,
  string email,
  string note,
  string guid)
{
  public string FirstName = firstName;
  public string LastName = lastName;
  public string Fio = fio;
  public string WorkPhone = workPhone;
  public string HomePhone = homePhone;
  public string RoomNumber = roomNumber;
  public string Address = address;
  public string Email = email;
  public string Note = note;
  public Guid Guid = GuidHelper.IsGuid(guid) ? new Guid(guid) : Guid.Empty;
}
