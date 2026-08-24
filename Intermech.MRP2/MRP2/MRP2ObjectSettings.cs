// Decompiled with JetBrains decompiler
// Type: Intermech.MRP2.MRP2ObjectSettings
// Assembly: Intermech.MRP2, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: C0BCFFEE-338E-4233-ADA0-6E6F7936896C
// Assembly location: D:\IPS\Client\Intermech.MRP2.dll
// XML documentation location: D:\IPS\Client\Intermech.MRP2.xml

using Intermech.Controls.Grid;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.MRP;
using Intermech.PropertyEditors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace Intermech.MRP2;

public class MRP2ObjectSettings : UserControl, IPropertyPage, IPropertyPageSearchOptionEvents
{
  /// <summary>Required designer variable.</summary>
  private IContainer components;
  private GroupBox groupBox1;
  private ListGrid attrGrid;
  private Button button3;
  private Button button2;

  public MRP2ObjectSettings()
  {
    this.InitializeComponent();
    this.LoadSettings();
  }

  private void button2_Click(object sender, EventArgs e)
  {
    using (SelectorForm selectorForm = new SelectorForm("Выберите тип объекта", 4, false))
    {
      selectorForm.SelectorFilter = (ISelectorFilter) new TypeSelectorFilter(new int[1]
      {
        MetaDataHelper.GetObjectTypeID("cad00268-306c-11d8-b4e9-00304f19f545")
      }, true, true);
      selectorForm.NodeSelectorFilter = (INodeSelectorFilter) new NodeSelectorFilter();
      if (selectorForm.ShowDialog() != DialogResult.OK || selectorForm.IDList.Count != 1)
        return;
      int id1 = (int) selectorForm.IDList[0];
      IMSObjectType objectType1 = MetaDataHelper.GetObjectType(id1);
      if (objectType1 == null)
        return;
      selectorForm.Text = "Выберите тип производственной копии";
      selectorForm.SelectorFilter = (ISelectorFilter) new TypeSelectorFilter(new int[1]
      {
        MRP2Consts.objtypeIdProductionCopy
      }, true, true);
      if (selectorForm.ShowDialog() != DialogResult.OK || selectorForm.IDList.Count != 1)
        return;
      int id2 = (int) selectorForm.IDList[0];
      Tuple<int, int> tuple = new Tuple<int, int>(id1, id2);
      ListItem listItem1 = (ListItem) null;
      foreach (ListItem listItem2 in (CollectionBase) this.attrGrid.Items)
      {
        if (((Tuple<int, int>) listItem2.Tag).Item1 == id1)
        {
          listItem1 = listItem2;
          listItem1.Text = objectType1.ObjectTypeName;
          objectType1 = MetaDataHelper.GetObjectType(id2);
          listItem1.SubItems[1].Text = objectType1.ObjectTypeName;
          break;
        }
      }
      if (listItem1 == null)
      {
        listItem1 = new ListItem(this.attrGrid);
        listItem1.Text = objectType1.ObjectTypeName;
        IMSObjectType objectType2 = MetaDataHelper.GetObjectType(id2);
        listItem1.SubItems.Add(objectType2.ObjectTypeName);
        this.attrGrid.Items.Add(listItem1);
      }
      listItem1.Tag = (object) tuple;
      this.OnChanged();
    }
  }

  private void button3_Click(object sender, EventArgs e)
  {
    for (int index = this.attrGrid.Items.Count - 1; index >= 0; --index)
    {
      if (this.attrGrid.Items[index].Selected)
        this.attrGrid.Items.RemoveAt(index);
    }
    this.attrGrid.Invalidate();
    this.OnChanged();
  }

  private void LoadSettings()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      this.attrGrid.Items.Clear();
      BlobInformation config_info;
      byte[] config_file;
      sessionKeeper.Session.Configurations.LoadConfigData("mrp2settings.xml", out config_info, out config_file, 0L);
      if (config_info.RealFileSize <= 0L)
        return;
      MemoryStream inStream = new MemoryStream(config_file);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.Load((Stream) inStream);
      foreach (XmlNode childNode in xmlDocument.FirstChild.ChildNodes)
      {
        IMSObjectType objectType1 = MetaDataHelper.GetObjectType(new Guid(childNode.Attributes["F_OBJECT_TYPE"].Value));
        IMSObjectType objectType2 = MetaDataHelper.GetObjectType(new Guid(childNode.Attributes["F_COPY_TYPE"].Value));
        if (objectType1 != null && objectType2 != null)
        {
          Tuple<int, int> tuple = new Tuple<int, int>(objectType1.ObjectTypeID, objectType2.ObjectTypeID);
          this.attrGrid.Items.Add(new ListItem(this.attrGrid)
          {
            Text = objectType1.ObjectTypeName,
            SubItems = {
              objectType2.ObjectTypeName
            },
            Tag = (object) tuple
          });
        }
      }
    }
  }

  private void SaveSettings()
  {
    MemoryStream outStream = new MemoryStream();
    XmlDocument xmlDocument = new XmlDocument();
    XmlNode element1 = (XmlNode) xmlDocument.CreateElement("MRP2TypeSettings");
    xmlDocument.AppendChild(element1);
    for (int nItemIndex = 0; nItemIndex < this.attrGrid.Items.Count; ++nItemIndex)
    {
      XmlNode element2 = (XmlNode) xmlDocument.CreateElement("row");
      Tuple<int, int> tag = (Tuple<int, int>) this.attrGrid.Items[nItemIndex].Tag;
      XmlAttribute attribute1 = xmlDocument.CreateAttribute("F_OBJECT_TYPE");
      XmlAttribute xmlAttribute1 = attribute1;
      Guid objectTypeGuid = MetaDataHelper.GetObjectTypeGuid(tag.Item1);
      string str1 = objectTypeGuid.ToString();
      xmlAttribute1.Value = str1;
      element2.Attributes.Append(attribute1);
      XmlAttribute attribute2 = xmlDocument.CreateAttribute("F_COPY_TYPE");
      XmlAttribute xmlAttribute2 = attribute2;
      objectTypeGuid = MetaDataHelper.GetObjectTypeGuid(tag.Item2);
      string str2 = objectTypeGuid.ToString();
      xmlAttribute2.Value = str2;
      element2.Attributes.Append(attribute2);
      element1.AppendChild(element2);
    }
    xmlDocument.Save((Stream) outStream);
    BlobInformation config_info = new BlobInformation(outStream.Length, outStream.Length, DateTime.Now, "mrp2settings.xml", ArcMethods.NotPacked, string.Empty);
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      sessionKeeper.Session.Configurations.WriteConfigData(config_info, outStream.ToArray(), 0L);
      MRP2Consts.InitCopyTypesSettings(sessionKeeper.Session);
      try
      {
        if (!(sessionKeeper.Session.GetCustomService(typeof (IMRPSettings)) is IMRPSettings customService))
          return;
        customService.LoadSettings(sessionKeeper.Session);
      }
      catch
      {
      }
    }
  }

  private void OnChanged()
  {
    if (this.Changed == null)
      return;
    this.Changed((object) this, new EventArgs());
  }

  public event EventHandler Changed;

  public PropertyPageType Type => PropertyPageType.Control;

  public object Control => (object) this;

  public string PageName => "Настройка соответствия типов копий";

  public void Apply() => this.SaveSettings();

  public void Cancel() => this.LoadSettings();

  public string HelpTopicID { get; }

  public string HeaderText { get; }

  public List<string> GetOptionNames()
  {
    return IPropertyPageHelper.GetOptionNames((System.Windows.Forms.Control) this.groupBox1);
  }

  /// <summary>Clean up any resources being used.</summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  /// <summary>
  /// Required method for Designer support - do not modify
  /// the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    ListColumn listColumn1 = new ListColumn();
    ListColumn listColumn2 = new ListColumn();
    this.groupBox1 = new GroupBox();
    this.attrGrid = new ListGrid();
    this.button3 = new Button();
    this.button2 = new Button();
    this.groupBox1.SuspendLayout();
    this.SuspendLayout();
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.attrGrid);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.button3);
    this.groupBox1.Controls.Add((System.Windows.Forms.Control) this.button2);
    this.groupBox1.Dock = DockStyle.Fill;
    this.groupBox1.Location = new Point(0, 0);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(689, 435);
    this.groupBox1.TabIndex = 6;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Настройка выбора типа производственной копии для типа изделия";
    this.attrGrid.AlternateBackground = Color.DarkGreen;
    this.attrGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.attrGrid.BackColor = SystemColors.ControlLightLight;
    listColumn1.Name = "Column1";
    listColumn1.Text = "Тип изделия";
    listColumn1.Width = 200;
    listColumn2.Name = "Column2";
    listColumn2.Text = "Тип копии";
    listColumn2.Width = 200;
    this.attrGrid.Columns.AddRange(new ListColumn[2]
    {
      listColumn1,
      listColumn2
    });
    this.attrGrid.GridColor = Color.LightGray;
    this.attrGrid.HeaderHeight = 22;
    this.attrGrid.HotTrackingColor = Color.LightGray;
    this.attrGrid.ImageList = (ImageList) null;
    this.attrGrid.ItemHeight = 17;
    this.attrGrid.Location = new Point(6, 49);
    this.attrGrid.Name = "attrGrid";
    this.attrGrid.SelectedTextColor = Color.White;
    this.attrGrid.SelectionColor = Color.DarkBlue;
    this.attrGrid.Size = new Size(677, 383);
    this.attrGrid.SuperFlatHeaderColor = Color.White;
    this.attrGrid.TabIndex = 3;
    this.attrGrid.Text = "listGrid1";
    this.button3.Location = new Point(83, 21);
    this.button3.Name = "button3";
    this.button3.Size = new Size(69, 22);
    this.button3.TabIndex = 2;
    this.button3.Text = "Удалить";
    this.button3.UseVisualStyleBackColor = true;
    this.button3.Click += new EventHandler(this.button3_Click);
    this.button2.Location = new Point(6, 21);
    this.button2.Name = "button2";
    this.button2.Size = new Size(71, 22);
    this.button2.TabIndex = 1;
    this.button2.Text = "Добавить ";
    this.button2.UseVisualStyleBackColor = true;
    this.button2.Click += new EventHandler(this.button2_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((System.Windows.Forms.Control) this.groupBox1);
    this.Name = nameof (MRP2ObjectSettings);
    this.Size = new Size(689, 435);
    this.groupBox1.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
