// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.SearchSign
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System;
using System.Data;

#nullable disable
namespace Intermech.ImpExp.SearchData;

internal class SearchSign
{
  public int DocID;
  public int VersionID;
  public int UserID;
  public string SignAs;
  public readonly int FileSize;
  public readonly int FileSize2;
  public readonly string Checksum = "";
  public DateTime SignDate;
  public DateTime FileDate;
  public string Addinfo = "";

  public SearchSign(
    int DocID,
    int VersionID,
    int UserID,
    string SignAs,
    int FileSize,
    int FileSize2,
    string Checksum,
    DateTime SignDate,
    DateTime FileDate,
    string Addinfo)
  {
    this.DocID = DocID;
    this.VersionID = VersionID;
    this.UserID = UserID;
    this.SignAs = SignAs;
    this.FileSize = FileSize;
    this.FileSize2 = FileSize2;
    this.Checksum = Checksum;
    this.SignDate = SignDate;
    this.FileDate = FileDate;
    this.Addinfo = Addinfo;
  }

  public SearchSign(IDataReader reader)
  {
    this.DocID = Convert.ToInt32(reader["docsgn_id"]);
    this.VersionID = Convert.ToInt32(reader["version_id"]);
    this.UserID = Convert.ToInt32(reader["usersgn_id"]);
    this.SignAs = reader["sign_as"].ToString();
    this.FileSize = Convert.ToInt32(reader["file_size"]);
    object obj1 = reader["filesize2"];
    if (!DBNull.Value.Equals(obj1))
      this.FileSize2 = Convert.ToInt32(obj1);
    object obj2 = reader["checksum"];
    if (!DBNull.Value.Equals(obj2))
      this.Checksum = obj2.ToString();
    this.SignDate = Convert.ToDateTime(reader["sign_date"]);
    this.FileDate = Convert.ToDateTime(reader["file_date"]);
    object obj3 = reader["addinfo"];
    if (DBNull.Value.Equals(obj3))
      return;
    this.Addinfo = obj3.ToString();
  }

  public bool Validate(int DocFileSize, DateTime DocFileDate, long DocAdvanFilesDate)
  {
    return this.FileSize2 != 0 ? this.FileSize == this.FileSize2 && this.CheckCertificate() != SearchSign.CheckSum.WrongConst : this.FileSize == DocFileSize && this.FileDate == DocFileDate && (this.Addinfo == "" || this.Addinfo == DocAdvanFilesDate.ToString("X16")) && this.CheckCertificate() != SearchSign.CheckSum.WrongConst;
  }

  private string GetXoredStr(string StrToXor, ushort XorValue)
  {
    int startIndex = 0;
    string xoredStr = "";
    for (; startIndex < StrToXor.Length; startIndex += 4)
    {
      uint num = Convert.ToUInt32(StrToXor.Substring(startIndex, 4), 16 /*0x10*/) ^ (uint) XorValue;
      xoredStr += num.ToString("X4");
    }
    return xoredStr;
  }

  private long HexStrToInt64Def(string s, long def)
  {
    try
    {
      return Convert.ToInt64(s, 16 /*0x10*/);
    }
    catch
    {
      return def;
    }
  }

  private SearchSign.CheckSum CheckCertificate()
  {
    SearchSign.CheckSum checkSum = SearchSign.CheckSum.WrongConst;
    string str1 = this.Checksum.Trim();
    if (str1 == "")
      checkSum = SearchSign.CheckSum.OldConst;
    else if (str1[0] == '0' && str1.Length == 49)
    {
      for (uint index = 0; (long) index < (long) str1.Length; ++index)
      {
        if ((str1[(int) index] < '0' || str1[(int) index] > '9') && (str1[(int) index] < 'A' || str1[(int) index] > 'F'))
          return checkSum;
      }
      string str2 = this.SignAs + " ";
      long num = (long) (this.DocID + this.UserID + Math.Abs(this.FileSize) * Convert.ToInt32(str2[0])) + Convert.ToInt64(this.SignDate.ToOADate()) + Convert.ToInt64(this.FileDate.ToOADate() * 1000000.0) + (long) this.VersionID;
      ushort XorValue = (ushort) ((uint) Convert.ToUInt16(str1.Substring(45, 4), 16 /*0x10*/) ^ (uint) Convert.ToUInt16(str1.Substring(25, 4), 16 /*0x10*/));
      long int64Def = this.HexStrToInt64Def(this.GetXoredStr(str1.Substring(37, 8) + str1.Substring(29, 8), XorValue), 0L);
      if (num == int64Def)
      {
        string xoredStr = this.GetXoredStr(str1.Substring(1, 24), XorValue);
        uint uint32 = Convert.ToUInt32(xoredStr.Substring(0, 8), 16 /*0x10*/);
        if ((this.HexStrToInt64Def(xoredStr.Substring(8, 16 /*0x10*/), 0L) ^ Convert.ToInt64(uint32) << 32 /*0x20*/ ^ (long) uint32) == this.HexStrToInt64Def(this.Addinfo, 0L))
          checkSum = SearchSign.CheckSum.NewConst;
      }
    }
    return checkSum;
  }

  private enum CheckSum
  {
    NewConst,
    OldConst,
    WrongConst,
    NotSigned,
  }
}
