// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.HashPack
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.Interfaces;
using Intermech.Signs.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Intermech.Signs;

internal class HashPack
{
  private static readonly byte[] sek = new byte[64 /*0x40*/]
  {
    (byte) 107,
    (byte) 2,
    (byte) 124,
    (byte) 56,
    (byte) 24,
    (byte) 87,
    (byte) 45,
    (byte) 124,
    (byte) 98,
    (byte) 56,
    (byte) 153,
    (byte) 12,
    (byte) 56,
    (byte) 82,
    (byte) 254,
    (byte) 32 /*0x20*/,
    (byte) 98,
    (byte) 46,
    (byte) 86,
    (byte) 92,
    (byte) 97,
    (byte) 4,
    (byte) 65,
    (byte) 3,
    (byte) 87,
    (byte) 213,
    (byte) 234,
    (byte) 76,
    (byte) 2,
    (byte) 67,
    (byte) 153,
    (byte) 12,
    (byte) 56,
    (byte) 82,
    (byte) 254,
    (byte) 32 /*0x20*/,
    (byte) 98,
    (byte) 46,
    (byte) 86,
    (byte) 92,
    (byte) 4,
    (byte) 66,
    (byte) 234,
    (byte) 66,
    (byte) 52,
    (byte) 37,
    (byte) 246,
    (byte) 24,
    (byte) 88,
    (byte) 45,
    (byte) 2,
    (byte) 18,
    (byte) 20,
    (byte) 20,
    (byte) 20,
    (byte) 90,
    (byte) 84,
    (byte) 56,
    (byte) 90,
    (byte) 188,
    (byte) 1,
    (byte) 2,
    (byte) 3,
    (byte) 4
  };
  private string _graph = string.Empty;
  private long _rank = -1;
  private long _userID = -1;
  private long _ioUserID;
  private string _userGuid = string.Empty;
  private string _ioUserGuid = string.Empty;
  private long _version;
  private string _modifDate = string.Empty;
  private string _dateOfSign = string.Empty;
  private string _resolution = string.Empty;

  public HashPack(IDBObject signObject)
  {
    this._version = signObject.GetAttributeByID(SignsHolder.SignVersionAttrTypeID).AsInteger;
    this._graph = signObject.GetAttributeByID(SignsHolder.GraphAttrTypeID).AsString;
    DateTime dateTime1 = signObject.GetAttributeByID(SignsHolder.ModifyDateAttrTypeID).AsDateTime;
    if (this._version >= 2L)
      dateTime1 -= signObject.Session.TimeZoneOffset;
    else
      dateTime1 = dateTime1.ToUniversalTime();
    this._modifDate = SignProcs.DateTimeToString(dateTime1);
    DateTime dateTime2 = signObject.GetAttributeByID(SignsHolder.DateOfSignatureID).AsDateTime;
    if (this._version >= 2L)
      dateTime2 -= signObject.Session.TimeZoneOffset;
    else
      dateTime2 = dateTime2.ToUniversalTime();
    this._dateOfSign = SignProcs.DateTimeToString(dateTime2);
    IDBAttribute attributeById1 = signObject.GetAttributeByID(SignsHolder.ResolutionAttrTypeID);
    if (attributeById1 != null)
      this._resolution = attributeById1.AsString;
    if (this._version < 2L)
    {
      IDBAttribute attributeById2 = signObject.GetAttributeByID(SignsHolder.SignUpIOAttrTypeID);
      if (attributeById2 != null)
        this._ioUserID = attributeById2.AsInteger;
    }
    this._userID = signObject.GetAttributeByID(SignsHolder.SignUpAttrTypeID).AsInteger;
    if (this._version == 0L)
    {
      this._rank = signObject.GetAttributeByID(SignsHolder.RankAttrTypeID).AsInteger;
    }
    else
    {
      this._userGuid = this.GetUserGuid(this._userID, signObject);
      if (this._ioUserID == 0L)
        return;
      this._ioUserGuid = this.GetUserGuid(this._ioUserID, signObject);
    }
  }

  private string GetUserGuid(long userID, IDBObject signObject)
  {
    string userGuid = SignsServerUsersCache.GetUserGuid(userID);
    if (userGuid == string.Empty)
    {
      IUserSession session = signObject.Session;
      if (session != null)
      {
        IDBObject dbObject = session.GetObject(userID, false);
        if (dbObject != null)
          userGuid = dbObject.ObjectGUID.ToString();
      }
    }
    return userGuid;
  }

  public HashPack(
    string graph,
    Guid userGuid,
    DateTime modifDate,
    DateTime dateOfSign,
    string resolution)
  {
    this._version = 2L;
    this._graph = graph;
    this._userGuid = userGuid.ToString();
    this._modifDate = SignProcs.DateTimeToString(modifDate);
    this._dateOfSign = SignProcs.DateTimeToString(dateOfSign);
    this._resolution = resolution;
  }

  public byte[] Pack()
  {
    MemoryStream output = new MemoryStream();
    BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
    binaryWriter.Write(this._graph);
    if (this._version == 0L)
    {
      binaryWriter.Write(this._rank);
      binaryWriter.Write(this._userID);
    }
    else
      binaryWriter.Write(this._userGuid);
    if (this._version < 2L && this._ioUserID != 0L)
      binaryWriter.Write(this._ioUserGuid);
    binaryWriter.Write(this._modifDate);
    binaryWriter.Write(this._dateOfSign);
    binaryWriter.Write(this._resolution);
    binaryWriter.Flush();
    return output.ToArray();
  }

  public static byte[] GetHashPack(IDBObject signObject) => new HashPack(signObject).Pack();

  public static byte[] CalcHash(IDBObject signObject)
  {
    return HashPack.CalcHash(HashPack.GetHashPack(signObject));
  }

  public static byte[] CalcHash(byte[] input)
  {
    using (HMACSHA384 hmacshA384 = new HMACSHA384(HashPack.sek))
      return hmacshA384.ComputeHash(input);
  }

  public static bool CompareHash(byte[] hash1, byte[] hash2)
  {
    return string.Equals(Convert.ToBase64String(hash1), Convert.ToBase64String(hash2));
  }
}
