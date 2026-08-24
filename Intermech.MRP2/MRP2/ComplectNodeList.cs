// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.ComplectNodeList
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Extensions;
using Intermech.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace Intermech.MRP2;

internal class ComplectNodeList : ArrayList
{
  public void LoadData(IUserSession session, string xml)
  {
    this.Clear();
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(xml);
    foreach (XmlNode childNode in xmlDocument.FirstChild.ChildNodes)
      this.Add((object) new ComplectNode(childNode, session));
  }

  public void LoadData(IDBRelation rel)
  {
    IDBAttribute byId = rel?.Attributes.FindByID(MRP2Consts.attrIdApplicabilityInKomplekt);
    if (byId != null)
      this.LoadData(rel.Session, byId.Value.ToString());
    else
      this.Clear();
  }

  public void LoadData(IUserSession session, long relID)
  {
    this.LoadData(session.GetRelation(relID, false));
  }

  public void SaveData(IDBRelation rel)
  {
    if (this.Count > 0)
    {
      XmlDocument xmlDocument = new XmlDocument();
      XmlNode element = (XmlNode) xmlDocument.CreateElement("r");
      xmlDocument.AppendChild(element);
      foreach (object obj in (ArrayList) this)
      {
        if (obj is ComplectNode complectNode)
          complectNode.SaveToXml(element);
      }
      string outerXml = xmlDocument.OuterXml;
      rel.SetAttributesValues(new AttributeValues[1]
      {
        new AttributeValues(MRP2Consts.attrIdApplicabilityInKomplekt, (object) outerXml)
      });
    }
    else
      rel.Attributes.FindByID(MRP2Consts.attrIdApplicabilityInKomplekt)?.Delete(0L);
  }

  public void SaveData(IUserSession session, long relID)
  {
    using (new SessionKeeper())
      this.SaveData(session.GetRelation(relID, true));
  }

  public void AppendData(
    IUserSession session,
    long plObjectID,
    long exitAsmID,
    int start,
    int end)
  {
    QuickObjectInfo qoi = session.GetObjectInfo(plObjectID);
    IEnumerable<ComplectNode> complectNodes = this.GeneralWhere<ComplectNode>((Func<ComplectNode, bool>) (x => x.ID == qoi.ID));
    if (complectNodes.IsEmpty<ComplectNode>())
    {
      this.Add((object) new ComplectNode(plObjectID, exitAsmID, start, end, session));
    }
    else
    {
      ComplectNode parent1 = complectNodes.FirstOrDefault<ComplectNode>();
      string pkdse = session.GetObjectAttributeByID(exitAsmID, MRP2Consts.attrIdPKDSE_Id).AsString;
      IEnumerable<AssemblyNode> assemblyNodes = parent1.ChildNodes.GeneralWhere<AssemblyNode>((Func<AssemblyNode, bool>) (x => x.id_PKDSE == pkdse));
      if (assemblyNodes.IsEmpty<AssemblyNode>())
      {
        AssemblyNode parent2 = new AssemblyNode(parent1, exitAsmID, session);
        NumbersNode numbersNode = new NumbersNode(parent2, start, end);
        parent2.ChildNodes.Add((object) numbersNode);
        parent1.ChildNodes.Add((object) parent2);
      }
      else
      {
        AssemblyNode parent3 = assemblyNodes.FirstOrDefault<AssemblyNode>();
        if (!parent3.ChildNodes.GeneralWhere<NumbersNode>((Func<NumbersNode, bool>) (x => x.s == start && x.e == end)).IsEmpty<NumbersNode>())
          return;
        NumbersNode numbersNode = new NumbersNode(parent3, start, end);
        parent3.ChildNodes.Add((object) numbersNode);
      }
    }
  }
}
