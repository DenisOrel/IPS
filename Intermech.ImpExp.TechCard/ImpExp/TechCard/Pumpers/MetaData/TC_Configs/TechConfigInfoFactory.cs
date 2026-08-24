// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.TechCard.Pumpers.MetaData.TC_Configs.TechConfigInfoFactory
// Assembly: Intermech.ImpExp.TechCard, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 887E9725-1538-4175-97E8-C0643E4E85AA
// Assembly location: D:\IPS\Client\Intermech.ImpExp.TechCard.dll

using Intermech.ImpExp.TechCard.Common;
using System;
using System.Data;
using System.Text;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.TechCard.Pumpers.MetaData.TC_Configs;

internal class TechConfigInfoFactory : TechItemFactoryBase<TechConfigInfo>
{
  private const string XmlConfigFlag = "<XML_CONFIG><BASE64";
  private const string XmlConfigNodeName = "XML_CONFIG";
  private const string XmlConfigNodeBase64 = "BASE64";
  private const string XmlConfigAttrStr = "CFG_STR";
  private readonly int _idxFldKey;
  private readonly int _idxFldId;
  private readonly int _idxFldConfig;
  private readonly int _idxFldProduction;
  private readonly int _idxFldUserId;
  private readonly int _idxFldBlob;
  private readonly byte[] _buffer = new byte[4096 /*0x1000*/];
  private readonly StringBuilder _stringBuilder = new StringBuilder();

  private string LoadDataFromBlob(IDataReader idr)
  {
    if (idr.IsDBNull(this._idxFldBlob))
      return string.Empty;
    this._stringBuilder.Clear();
    Encoding encoding = Encoding.GetEncoding(1251);
    try
    {
      switch (idr.GetFieldType(this._idxFldBlob).FullName)
      {
        case "System.String":
          this._stringBuilder.Append(idr.GetString(this._idxFldBlob));
          break;
        case "System.Byte[]":
          int length = this._buffer.Length;
          int fieldOffset = 0;
          long bytes;
          for (bytes = idr.GetBytes(this._idxFldBlob, (long) fieldOffset, this._buffer, 0, length); bytes == (long) length; bytes = idr.GetBytes(this._idxFldBlob, (long) fieldOffset, this._buffer, 0, length))
          {
            this._stringBuilder.Append(encoding.GetString(this._buffer, 0, (int) bytes));
            fieldOffset += length;
          }
          this._stringBuilder.Append(encoding.GetString(this._buffer, 0, (int) bytes));
          break;
      }
    }
    catch (Exception ex)
    {
      TechcardConsts.Plugin.appManager.AddWarningMessage("Ошибка чтения таблицы TC_CONFIG: " + ex.Message);
      if (ex is OutOfMemoryException)
        throw;
    }
    string xml = this._stringBuilder.ToString().TrimEnd();
    this._stringBuilder.Clear();
    if (!xml.StartsWith("<XML_CONFIG><BASE64"))
      return xml;
    XmlDocument xmlDocument = new XmlDocument();
    try
    {
      xmlDocument.LoadXml(xml);
    }
    catch (Exception ex)
    {
      string Message = $"Ошибка чтения записи F_KEY={this.getInt32(idr, this._idxFldKey)} таблицы TC_CONFIG : {ex.Message}{Environment.NewLine}{xml}";
      TechcardConsts.Plugin.appManager.AddWarningMessage(Message);
      return string.Empty;
    }
    XmlElement documentElement = xmlDocument.DocumentElement;
    if (documentElement == null || documentElement.Name != "XML_CONFIG")
      return xml;
    foreach (XmlNode childNode in documentElement.ChildNodes)
    {
      if (childNode != null && childNode.Attributes != null && !(childNode.Name != "BASE64"))
      {
        XmlAttribute attribute = childNode.Attributes["CFG_STR"];
        if (attribute != null)
        {
          byte[] bytes = Convert.FromBase64String(attribute.Value);
          this._stringBuilder.AppendLine(encoding.GetString(bytes));
        }
      }
    }
    string str = this._stringBuilder.ToString();
    this._stringBuilder.Clear();
    return str;
  }

  public TechConfigInfoFactory(IDataReader dataReader)
    : base("TC_CONFIGS", dataReader)
  {
    this._idxFldKey = dataReader != null ? dataReader.GetOrdinal("F_KEY") : throw new ArgumentNullException(nameof (dataReader));
    this._idxFldId = dataReader.GetOrdinal("F_ID");
    this._idxFldConfig = dataReader.GetOrdinal("F_CONFIG");
    this._idxFldProduction = dataReader.GetOrdinal("F_PRODUCTION");
    this._idxFldUserId = dataReader.GetOrdinal("F_USER");
    this._idxFldBlob = dataReader.GetOrdinal("F_BLOB");
  }

  public override TechConfigInfo CreateItem(IDataReader idr)
  {
    string config = this.getString(idr, this._idxFldConfig);
    string bigData = string.Empty;
    if (string.IsNullOrEmpty(config))
      bigData = this.LoadDataFromBlob(idr);
    return new TechConfigInfo(this.getInt32(idr, this._idxFldKey), this.getInt32(idr, this._idxFldId), config, this.getInt32(idr, this._idxFldProduction), this.getInt32(idr, this._idxFldUserId), bigData);
  }
}
