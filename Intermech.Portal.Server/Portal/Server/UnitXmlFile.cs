// Decompiled with JetBrains decompiler
// Type: Intermech.Portal.Server.UnitXmlFile
// Assembly: Intermech.Portal.Server, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 814BABAA-794A-446D-BCF7-B9A0D67EFF42
// Assembly location: D:\IPS\IPS.Installer.Full\InstServer\Server\Intermech.Portal.Server.dll

using Intermech.Interfaces;
using Intermech.Interfaces.WebPortal;
using Intermech.Localization;
using System;
using System.Globalization;
using System.IO;
using System.Xml;

#nullable disable
namespace Intermech.Portal.Server;

internal class UnitXmlFile
{
  public static XmlNode GetInfo(
    IUserSession session,
    out TransferedObject unit,
    string unitFile,
    XmlDocument xmlDocument)
  {
    unit = TransferedObject.LoadFromFile(unitFile);
    FileInfo fileInfo = new FileInfo(Path.Combine(TempStorage.GetPublishUnitPath(unit.GUID), PortalConsts.AttributesXmlFileName));
    if (!fileInfo.Exists)
      return (XmlNode) null;
    IPackedStream service = ServiceUtils.GetService<IPackedStream>((object) ApplicationServices.Container, true);
    using (MemoryStream memoryStream = new MemoryStream())
    {
      using (FileStream inStream = new FileStream(fileInfo.FullName, FileMode.Open))
      {
        try
        {
          service.UnpackStream((Stream) memoryStream, (Stream) inStream);
        }
        catch (Exception ex)
        {
          if (TraceLog.Enabled)
            TraceLog.Write($"Ошибка при распаковке файла {fileInfo.FullName}: {ex.Message}");
          throw;
        }
      }
      memoryStream.Position = 0L;
      xmlDocument.Load((Stream) memoryStream);
    }
    XmlNode xmlNode = (XmlNode) null;
    for (int i = 0; i < xmlDocument.ChildNodes.Count; ++i)
    {
      if (xmlDocument.ChildNodes[i].Name == PortalConsts.XmlRootNodeAttributes)
      {
        xmlNode = xmlDocument.ChildNodes[i];
        break;
      }
    }
    return xmlNode != null ? xmlNode : throw new Exception(LocalizationHolder.rm.GetString("PortalServer_18"));
  }

  public static ValueInfo GetValueInfo(XmlNode node, int index)
  {
    ValueInfo valueInfo = new ValueInfo();
    string nodeAttributeValue1 = UnitXmlFile.GetNodeAttributeValue(node, "F_STRING_VALUE");
    valueInfo.StringValue = !string.IsNullOrEmpty(nodeAttributeValue1) ? nodeAttributeValue1 : string.Empty;
    string nodeAttributeValue2 = UnitXmlFile.GetNodeAttributeValue(node, "F_DATE_VALUE");
    valueInfo.DateValue = !string.IsNullOrEmpty(nodeAttributeValue2) ? Convert.ToDateTime(nodeAttributeValue2, (IFormatProvider) CultureInfo.InvariantCulture) : DateTime.Now;
    if (valueInfo.DateValue == DateTime.MinValue)
      valueInfo.DateValue = DateTime.Now;
    string nodeAttributeValue3 = UnitXmlFile.GetNodeAttributeValue(node, "F_INTEGER_VALUE");
    valueInfo.IntValue = !string.IsNullOrEmpty(nodeAttributeValue3) ? Convert.ToInt64(nodeAttributeValue3) : 0L;
    string nodeAttributeValue4 = UnitXmlFile.GetNodeAttributeValue(node, "F_DOUBLE_VALUE");
    valueInfo.FloatValue = !string.IsNullOrEmpty(nodeAttributeValue4) ? Convert.ToDouble(nodeAttributeValue4, (IFormatProvider) CultureInfo.InvariantCulture) : 0.0;
    string nodeAttributeValue5 = UnitXmlFile.GetNodeAttributeValue(node, "F_FILE");
    valueInfo.FileName = !string.IsNullOrEmpty(nodeAttributeValue5) ? nodeAttributeValue5 : string.Empty;
    string nodeAttributeValue6 = UnitXmlFile.GetNodeAttributeValue(node, "F_ARC_METHOD");
    valueInfo.ArcMethod = !string.IsNullOrEmpty(nodeAttributeValue6) ? (ArcMethods) Convert.ToInt32(nodeAttributeValue6) : ArcMethods.NotPacked;
    string nodeAttributeValue7 = UnitXmlFile.GetNodeAttributeValue(node, "F_FILE_TYPE");
    valueInfo.FileType = !string.IsNullOrEmpty(nodeAttributeValue7) ? (FileTypes) Convert.ToInt32(nodeAttributeValue7) : FileTypes.ftNormal;
    string nodeAttributeValue8 = UnitXmlFile.GetNodeAttributeValue(node, "F_FILE_AUTHOR");
    valueInfo.FileAuthor = !string.IsNullOrEmpty(nodeAttributeValue8) ? Convert.ToString(nodeAttributeValue8) : string.Empty;
    string nodeAttributeValue9 = UnitXmlFile.GetNodeAttributeValue(node, "F_INLIST_ID");
    valueInfo.Index = !string.IsNullOrEmpty(nodeAttributeValue9) ? Convert.ToInt32(nodeAttributeValue9) : index;
    return valueInfo;
  }

  private static string GetNodeAttributeValue(XmlNode node, string attributeName)
  {
    return node.Attributes[attributeName] != null ? node.Attributes[attributeName].Value : (string) null;
  }
}
