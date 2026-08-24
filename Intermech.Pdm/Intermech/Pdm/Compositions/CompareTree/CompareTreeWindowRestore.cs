// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareTree.CompareTreeWindowRestore
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Localization;
using Intermech.Navigator.Interfaces;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.Pdm.Compositions.CompareTree;

internal sealed class CompareTreeWindowRestore
{
  private readonly ICompareTreeWindowRestoreData _compareTreeWindow;
  private static readonly string _xmlNodeSettings = "Settings";
  private static readonly string _xmlNodeRootItems = "RootItems";
  private static readonly string _xmlNodeLeftItemID = "LeftItemID";
  private static readonly string _xmlNodeRightItemID = "RightItemID";
  private static readonly string _xmlNodeProperties = "Properties";
  private static readonly string _xmlNodeRuleID = "RuleID";
  private static readonly string _xmlNodeRecursive = "Recursive";

  public CompareTreeWindowRestore(ICompareTreeWindowRestoreData compareTreeWindow)
  {
    this._compareTreeWindow = compareTreeWindow;
  }

  public static CompareTreeWindow RestoreWindow(string persistString)
  {
    try
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(persistString);
      XmlNode xmlNode1 = xmlDocument.SelectSingleNode("/" + CompareTreeWindowRestore._xmlNodeSettings);
      if (xmlNode1 == null)
        return (CompareTreeWindow) null;
      long num1 = 0;
      long num2 = 0;
      Guid ruleID = Guid.Empty;
      bool recursive = false;
      XmlNode xmlNode2 = xmlNode1.SelectSingleNode($"{CompareTreeWindowRestore._xmlNodeRootItems}/{CompareTreeWindowRestore._xmlNodeLeftItemID}");
      if (xmlNode2 != null)
        num1 = XmlConvert.ToInt64(xmlNode2.InnerText);
      XmlNode xmlNode3 = xmlNode1.SelectSingleNode($"{CompareTreeWindowRestore._xmlNodeRootItems}/{CompareTreeWindowRestore._xmlNodeRightItemID}");
      if (xmlNode3 != null)
        num2 = XmlConvert.ToInt64(xmlNode3.InnerText);
      XmlNode xmlNode4 = xmlNode1.SelectSingleNode($"{CompareTreeWindowRestore._xmlNodeProperties}/{CompareTreeWindowRestore._xmlNodeRuleID}");
      if (xmlNode4 != null)
        ruleID = XmlConvert.ToGuid(xmlNode4.InnerText);
      XmlNode xmlNode5 = xmlNode1.SelectSingleNode($"{CompareTreeWindowRestore._xmlNodeProperties}/{CompareTreeWindowRestore._xmlNodeRecursive}");
      if (xmlNode5 != null)
        recursive = XmlConvert.ToBoolean(xmlNode5.InnerText);
      CompareTreeWindow window = new CompareTreeWindow(num1, num2, ruleID, recursive);
      try
      {
        ((IWellKnownNavigators) ServicesManager.GetService(typeof (IWellKnownNavigators)))?.Register("CompareTreeWindow", (Control) window);
        return window;
      }
      catch
      {
      }
    }
    catch (Exception ex)
    {
      IOutputView service = ServicesManager.GetService(typeof (IOutputView)) as IOutputView;
      service.WriteString("Navigator", LocalizationHolder.rm.GetString("Client.Core_326"));
      service.WriteString("Navigator", ex.Message);
      return (CompareTreeWindow) null;
    }
    return (CompareTreeWindow) null;
  }

  public static string GetPersistString(ICompareTreeWindowRestoreData compareTreeWindow)
  {
    return new CompareTreeWindowRestore(compareTreeWindow).GetPersistString();
  }

  private string GetPersistString()
  {
    try
    {
      XmlDocument state = this.GetState();
      using (TextWriter w1 = (TextWriter) new StringWriter())
      {
        XmlWriter w2 = (XmlWriter) new XmlTextWriter(w1);
        state.WriteTo(w2);
        w2.Flush();
        w2.Close();
        return w1.ToString();
      }
    }
    catch (Exception ex)
    {
      ExceptionHelper.ExceptionService.ShowException(ex);
      return (string) null;
    }
  }

  private XmlDocument GetState()
  {
    XmlDocument xmlDoc = new XmlDocument();
    XmlNode element = (XmlNode) xmlDoc.CreateElement(CompareTreeWindowRestore._xmlNodeSettings);
    element.AppendChild(this.GetRootItemsNode(xmlDoc));
    element.AppendChild(this.GetPropertiesNode(xmlDoc));
    xmlDoc.AppendChild(element);
    return xmlDoc;
  }

  private XmlNode GetRootItemsNode(XmlDocument xmlDoc)
  {
    XmlElement element1 = xmlDoc.CreateElement(CompareTreeWindowRestore._xmlNodeRootItems);
    XmlNode element2 = (XmlNode) xmlDoc.CreateElement(CompareTreeWindowRestore._xmlNodeLeftItemID);
    element2.AppendChild((XmlNode) xmlDoc.CreateTextNode(XmlConvert.ToString(this._compareTreeWindow.LeftItemID)));
    element1.AppendChild(element2);
    XmlNode element3 = (XmlNode) xmlDoc.CreateElement(CompareTreeWindowRestore._xmlNodeRightItemID);
    element3.AppendChild((XmlNode) xmlDoc.CreateTextNode(XmlConvert.ToString(this._compareTreeWindow.RightItemID)));
    element1.AppendChild(element3);
    return (XmlNode) element1;
  }

  private XmlNode GetPropertiesNode(XmlDocument xmlDoc)
  {
    XmlElement element1 = xmlDoc.CreateElement(CompareTreeWindowRestore._xmlNodeProperties);
    XmlNode element2 = (XmlNode) xmlDoc.CreateElement(CompareTreeWindowRestore._xmlNodeRuleID);
    element2.AppendChild((XmlNode) xmlDoc.CreateTextNode(XmlConvert.ToString(this._compareTreeWindow.RuleID)));
    element1.AppendChild(element2);
    XmlNode element3 = (XmlNode) xmlDoc.CreateElement(CompareTreeWindowRestore._xmlNodeRecursive);
    element3.AppendChild((XmlNode) xmlDoc.CreateTextNode(XmlConvert.ToString(this._compareTreeWindow.Recursive)));
    element1.AppendChild(element3);
    return (XmlNode) element1;
  }
}
