// Decompiled with JetBrains decompiler
// Type: Intermech.XmlExchange.IpsXml.Converter.ToSAP.XmlExtention.XmlConvertor2SAPExtention
// Assembly: Intermech.XmlExchange.IpsXml.Converter.ToSAP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 946972C6-4ABC-4C4A-94A5-3ADC51FD9A58
// Assembly location: D:\IPS\Client\Intermech.XmlExchange.IpsXml.Converter.ToSAP.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.XmlExchange;
using Intermech.Interfaces.XmlExchange.Common;
using Intermech.XmlExchange.Server;
using Intermech.XmlExchange.Server.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

#nullable disable
namespace Intermech.XmlExchange.IpsXml.Converter.ToSAP.XmlExtention;

internal class XmlConvertor2SAPExtention : IXmlExchangeExportExtension, IXmlExchangeExtension
{
  private static Guid cnt_PluginGuid = new Guid("{376B29C0-476A-496A-8CA7-F70763686C75}");

  private void ConvertXmlData(object task)
  {
    if (task == null)
      throw new ArgumentNullException(nameof (task));
    if (!(task is XmlExchangeExportWorker exchangeExportWorker))
      return;
    string[] files = Directory.GetFiles(exchangeExportWorker.DataFolder, "*.*", SearchOption.TopDirectoryOnly);
    IPSToSAPService ipsToSapService = new IPSToSAPService(((XmlExchangeBaseTask) exchangeExportWorker.Task).Session);
    bool flag = true;
    string[] fileNames = files;
    if (!(ipsToSapService.Convert(fileNames) != null & flag))
      return;
    foreach (string path in files)
      File.Delete(path);
    foreach (string directory in Directory.GetDirectories(exchangeExportWorker.DataFolder, "*.*", SearchOption.TopDirectoryOnly))
      Directory.Delete(directory, true);
  }

  public XmlExtensionPriority Priority => XmlExtensionPriority.Default;

  public Guid Guid => XmlConvertor2SAPExtention.cnt_PluginGuid;

  public bool IsSystem => false;

  public XmlExportExtAction Actions { get; protected set; }

  public bool CanProcess(XmlExportExtAction action)
  {
    return this.Actions != (XmlExportExtAction) 0 && (this.Actions & action) == action;
  }

  public void StartTask(object task)
  {
  }

  public void EndTask(object task) => this.ConvertXmlData(task);

  public Dictionary<string, object> Execute(
    XmlExportExtAction action,
    object subtask,
    params object[] args)
  {
    return (Dictionary<string, object>) null;
  }

  public object[] GetObjectLinkedInfo(
    object subtask,
    IDBObject dbObject,
    ObjectRecord objRecord,
    object objExportData)
  {
    return (object[]) null;
  }

  public object[] GetRelationLinkedInfo(
    object subtask,
    IDBRelation dbRelation,
    RelationRecord relRecord,
    object partExportData,
    ObjInfoItem projObjInfo)
  {
    return (object[]) null;
  }

  public object[] GetAttributeLinkedInfo(
    object subtask,
    IDBAttributable dbAttributable,
    object objExportData,
    DataRow attrRow,
    object attrRecord)
  {
    return (object[]) null;
  }
}
