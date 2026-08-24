// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Workflow.WorkflowScheme
// Assembly: Intermech.ImpExp.Workflow, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 3E5C231D-9C58-4E51-9000-3F9F7E271790
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Workflow.dll

using Intermech.Workflow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Intermech.ImpExp.Workflow;

internal class WorkflowScheme
{
  public ActivityList Activities = new ActivityList();
  public List<LinkInfo> Links = new List<LinkInfo>();
  public Dictionary<string, S4Table> Table = new Dictionary<string, S4Table>();
  public Dictionary<string, object> Data = new Dictionary<string, object>();
  private long _schemeID;
  private string _name = "@#$";
  private int _typeID;
  private StringList _addData;
  private long _objectID;

  public void Clear()
  {
    this.Table.Clear();
    this.Data.Clear();
    this.Activities.Clear();
    this.Links.Clear();
    this._schemeID = 0L;
    this._typeID = 0;
    this._addData = (StringList) null;
    this._name = "@#$";
    this._objectID = 0L;
  }

  public long SchemeID
  {
    get
    {
      if (this._schemeID == 0L && this.Data.ContainsKey("schemeid"))
        this._schemeID = Convert.ToInt64(this.Data["schemeid"]);
      return this._schemeID;
    }
  }

  public string Name
  {
    get
    {
      if (this._name == "@#$")
        this._name = this.Data["schemename"].ToString();
      return this._name;
    }
  }

  public void SaveSchemeXMLToStream(Stream stream)
  {
    XmlTextWriter xmlTextWriter1 = new XmlTextWriter(stream, Encoding.UTF8);
    xmlTextWriter1.Formatting = Formatting.Indented;
    xmlTextWriter1.WriteStartElement("Intermech.Workflow");
    xmlTextWriter1.WriteStartElement("Process");
    xmlTextWriter1.WriteStartElement("Nodes");
    int num1;
    for (int index = 0; index < this.Activities.Count; ++index)
    {
      ActInfo activity = this.Activities[index];
      if (activity.ParentActivityID < 0)
      {
        xmlTextWriter1.WriteStartElement("Node");
        XmlTextWriter xmlTextWriter2 = xmlTextWriter1;
        num1 = index + 1;
        string str = num1.ToString();
        xmlTextWriter2.WriteAttributeString("id", str);
        xmlTextWriter1.WriteStartElement("Type");
        xmlTextWriter1.WriteString(activity.TypeGuid.ToString());
        xmlTextWriter1.WriteEndElement();
        xmlTextWriter1.WriteStartElement("Text");
        xmlTextWriter1.WriteString(activity.Data["name"].ToString());
        xmlTextWriter1.WriteEndElement();
        float int64_1 = (float) Convert.ToInt64(activity.Data["boxx"]);
        xmlTextWriter1.WriteStartElement("X");
        xmlTextWriter1.WriteString(int64_1.ToString());
        xmlTextWriter1.WriteEndElement();
        float int64_2 = (float) Convert.ToInt64(activity.Data["boxy"]);
        xmlTextWriter1.WriteStartElement("Y");
        xmlTextWriter1.WriteString(int64_2.ToString());
        xmlTextWriter1.WriteEndElement();
        xmlTextWriter1.WriteEndElement();
      }
    }
    xmlTextWriter1.WriteEndElement();
    xmlTextWriter1.WriteStartElement("Links");
    for (int index = 0; index < this.Links.Count; ++index)
    {
      LinkInfo link = this.Links[index];
      xmlTextWriter1.WriteStartElement("Link");
      XmlTextWriter xmlTextWriter3 = xmlTextWriter1;
      num1 = index + 1;
      string str = num1.ToString();
      xmlTextWriter3.WriteAttributeString("id", str);
      xmlTextWriter1.WriteStartElement("From");
      int num2 = this.Activities.IndexByOldID(link.ActivityID);
      if (num2 != -1)
      {
        XmlTextWriter xmlTextWriter4 = xmlTextWriter1;
        num1 = num2 + 1;
        string text = num1.ToString();
        xmlTextWriter4.WriteString(text);
      }
      xmlTextWriter1.WriteEndElement();
      xmlTextWriter1.WriteStartElement("To");
      int num3 = this.Activities.IndexByOldID(link.LinkTo);
      if (num3 != -1)
      {
        XmlTextWriter xmlTextWriter5 = xmlTextWriter1;
        num1 = num3 + 1;
        string text = num1.ToString();
        xmlTextWriter5.WriteString(text);
      }
      xmlTextWriter1.WriteEndElement();
      xmlTextWriter1.WriteEndElement();
    }
    xmlTextWriter1.WriteEndElement();
    xmlTextWriter1.WriteEndElement();
    xmlTextWriter1.WriteEndElement();
    xmlTextWriter1.Flush();
  }

  public string SchemeXML
  {
    get
    {
      MemoryStream memoryStream = new MemoryStream();
      this.SaveSchemeXMLToStream((Stream) memoryStream);
      memoryStream.Position = 0L;
      StreamReader streamReader = new StreamReader((Stream) memoryStream);
      try
      {
        return streamReader.ReadToEnd();
      }
      finally
      {
        streamReader.Close();
        memoryStream.Close();
      }
    }
  }

  public int TypeID
  {
    get
    {
      if (this._typeID == 0 && this.Data.ContainsKey("kind"))
        this._typeID = !(this.Data["kind"].ToString() == "P") ? wfConsts.SchemesTypeID : wfConsts.ProcessesTypeID;
      return this._typeID;
    }
  }

  public bool IsProcess => this.TypeID == wfConsts.ProcessesTypeID;

  public int ActivitiesLCStep => this.IsProcess ? wfConsts.ActivityExecLCStepID : 0;

  public StringList AddData
  {
    get
    {
      if (this._addData == null)
      {
        this._addData = new StringList();
        S4Table s4Table = this.Table["data"];
        if (s4Table.Count > 0)
          this._addData.Text = BasePumpHelper.BlobToString(s4Table["blobdata"]);
      }
      return this._addData;
    }
  }

  public long ObjectID
  {
    get => this._objectID;
    set => this._objectID = value;
  }
}
