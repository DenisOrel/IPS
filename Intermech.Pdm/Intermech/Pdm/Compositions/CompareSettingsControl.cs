// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.Compositions.CompareSettingsControl
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.Compositions;

internal sealed class CompareSettingsControl : IDisposable
{
  public SettingsButtonClick SettingsButtonClickEvent;
  public RelTypesChanged RelTypesChangedEvent;
  private readonly CompareObjectsInfo _info;
  private ButtonItem _buttonRecursive;
  private ButtonItem _buttonDifferences;
  private ButtonItem _buttonCompatibility;
  private ButtonItem _buttonComposition;
  private ButtonItem _buttonStart;
  private ButtonItem _buttonRelTypes;
  private ButtonItem _buttonResetColumn;
  private StatusStrip _statusBar;
  private ToolStripStatusLabel _ttslCircle;
  private Thread _circleThread;
  private int _iconStart = -1;
  private int _iconStop = -1;
  private bool _selfChecked;
  private readonly string _strQueryStart = LocalizationHolder.rm.GetString("Pdm_38");
  private readonly string _strQueryStop = LocalizationHolder.rm.GetString("Pdm_39");

  public CompareSettingsControl(
    Intermech.Bars.ToolBar toolBar,
    StatusStrip statusBar,
    CompareObjectsInfo info,
    bool enableDifButtons)
  {
    this._info = info;
    this._statusBar = statusBar;
    this.InitializeControls(toolBar, statusBar);
    if (!enableDifButtons)
    {
      this._buttonDifferences.Visible = false;
      this._buttonCompatibility.Visible = false;
      this._buttonComposition.Visible = false;
    }
    this.ToolTipText4RelTypes(info.RelationTypes, info.CompareAttributes);
  }

  private void InitializeControls(Intermech.Bars.ToolBar toolBar, StatusStrip statusBar)
  {
    if (this._circleThread != null)
      this._circleThread.Abort();
    INamedImageList service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
    this._iconStart = service.ImageIndex("imgStart");
    this._iconStop = service.ImageIndex("imgStop2");
    toolBar.ImageList = service.ImageList;
    this._buttonRecursive = new ButtonItem();
    this._buttonRecursive.BeginGroup = true;
    this._buttonRecursive.Tag = (object) SettingsButtonCommands.Recursive;
    this._buttonRecursive.ToolTipText = "Развернутый состав объекта";
    this._buttonRecursive.ImageIndex = service.ImageIndex("imgExpandComposition");
    this._buttonRecursive.Checked = this._info.Recursive;
    this._buttonRecursive.Click += new EventHandler(this.OnSettingsButtonClick);
    this._buttonDifferences = new ButtonItem();
    this._buttonDifferences.BeginGroup = true;
    this._buttonDifferences.Tag = (object) SettingsButtonCommands.Differences;
    this._buttonDifferences.ToolTipText = "Различия объектов";
    this._buttonDifferences.ImageIndex = service.ImageIndex("imgDistinctions");
    this._buttonDifferences.Click += new EventHandler(this.OnSettingsButtonClick);
    this._buttonCompatibility = new ButtonItem();
    this._buttonCompatibility.Tag = (object) SettingsButtonCommands.Compatibility;
    this._buttonCompatibility.ToolTipText = "Общая часть объектов";
    this._buttonCompatibility.ImageIndex = service.ImageIndex("imgCoincidences");
    this._buttonCompatibility.Click += new EventHandler(this.OnSettingsButtonClick);
    this._buttonComposition = new ButtonItem();
    this._buttonComposition.Checked = true;
    this._buttonComposition.Tag = (object) SettingsButtonCommands.Composition;
    this._buttonComposition.ToolTipText = "Состав объекта";
    this._buttonComposition.ImageIndex = service.ImageIndex("imgContains");
    this._buttonComposition.Click += new EventHandler(this.OnSettingsButtonClick);
    this._buttonStart = new ButtonItem();
    this._buttonStart.Tag = (object) SettingsButtonCommands.Start;
    this._buttonStart.BeginGroup = true;
    this._buttonStart.ImageIndex = this._iconStart;
    this._buttonStart.ToolTipText = this._strQueryStart;
    this._buttonStart.Click += new EventHandler(this.OnSettingsButtonClick);
    this._buttonRelTypes = new ButtonItem();
    this._buttonRelTypes.BeginGroup = true;
    this._buttonRelTypes.CommandName = "RelTypes";
    this._buttonRelTypes.ToolTipText = "Типы связей для сравнения";
    this._buttonRelTypes.ImageIndex = service.ImageIndex("imgViewSettings");
    this._buttonRelTypes.Click += new EventHandler(this.RelTypesClick);
    this._buttonResetColumn = new ButtonItem();
    this._buttonResetColumn.Tag = (object) SettingsButtonCommands.ResetColumns;
    this._buttonResetColumn.CommandName = "ResetColumn";
    this._buttonResetColumn.ToolTipText = "Сбросить настройки отображения";
    this._buttonResetColumn.ImageIndex = service.ImageIndex("imgColumnsReset");
    this._buttonResetColumn.Click += new EventHandler(this.OnSettingsButtonClick);
    toolBar.Items.AddRange(new ToolbarItemBase[7]
    {
      (ToolbarItemBase) this._buttonResetColumn,
      (ToolbarItemBase) this._buttonRelTypes,
      (ToolbarItemBase) this._buttonDifferences,
      (ToolbarItemBase) this._buttonCompatibility,
      (ToolbarItemBase) this._buttonComposition,
      (ToolbarItemBase) this._buttonRecursive,
      (ToolbarItemBase) this._buttonStart
    });
    this._ttslCircle = new ToolStripStatusLabel();
    this._ttslCircle.Name = "ttslCircle";
    statusBar.Items.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this._ttslCircle
    });
  }

  public void SetStartButton(bool start)
  {
    this._buttonStart.ImageIndex = start ? this._iconStart : this._iconStop;
    this._buttonStart.ToolTipText = start ? this._strQueryStart : this._strQueryStop;
  }

  public void Dispose()
  {
    if (this._circleThread == null)
      return;
    this._circleThread.Abort();
  }

  public void SetCircleThread(bool start)
  {
    if (start)
    {
      this._circleThread = new Thread(new ThreadStart(this.CircleMethod));
      this._circleThread.Name = "ContainsCircleThread";
      this._circleThread.IsBackground = true;
      this._circleThread.Start();
    }
    else
    {
      if (this._circleThread == null)
        return;
      this._circleThread.Abort();
      this._circleThread.Join();
      this.SetCircle(string.Empty);
      this._circleThread = (Thread) null;
    }
  }

  private void OnSettingsButtonClick(object sender, EventArgs e)
  {
    if (this._selfChecked)
      return;
    ButtonItem buttonItem = sender as ButtonItem;
    SettingsButtonCommands tag = (SettingsButtonCommands) buttonItem.Tag;
    switch (tag)
    {
      case SettingsButtonCommands.Differences:
      case SettingsButtonCommands.Compatibility:
      case SettingsButtonCommands.Composition:
        if (buttonItem.Checked)
          return;
        break;
    }
    SettingsButtonClick buttonClickEvent = this.SettingsButtonClickEvent;
    if (buttonClickEvent != null)
      buttonClickEvent((object) this, new SettingsButtonClickEventArgs(tag, buttonItem.Checked));
    if (tag == SettingsButtonCommands.Start || tag == SettingsButtonCommands.ResetColumns)
      return;
    this._selfChecked = true;
    try
    {
      buttonItem.Checked = !buttonItem.Checked;
      this.HandleButtonsState(tag);
    }
    finally
    {
      this._selfChecked = false;
    }
  }

  private void HandleButtonsState(SettingsButtonCommands command)
  {
    switch (command)
    {
      case SettingsButtonCommands.Differences:
        this._buttonCompatibility.Checked = false;
        this._buttonComposition.Checked = false;
        break;
      case SettingsButtonCommands.Compatibility:
        this._buttonComposition.Checked = false;
        this._buttonDifferences.Checked = false;
        break;
      case SettingsButtonCommands.Composition:
        this._buttonCompatibility.Checked = false;
        this._buttonDifferences.Checked = false;
        break;
    }
  }

  private void RelTypesClick(object sender, EventArgs e)
  {
    using (ChangeRelationTypesForm relationTypesForm = new ChangeRelationTypesForm(this._info.RelationTypes, this._info.CompareAttributes))
    {
      relationTypesForm.Init();
      if (relationTypesForm.ShowDialog() != DialogResult.OK || this.RelTypesChangedEvent == null)
        return;
      this.RelTypesChangedEvent((object) this, new EventArgs());
      this.ToolTipText4RelTypes(this._info.RelationTypes, this._info.CompareAttributes);
    }
  }

  private void ToolTipText4RelTypes(Dictionary<int, bool> relTypes, List<int> attrs)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (relTypes == null || relTypes.Count == 0 || !relTypes.ContainsValue(true))
    {
      stringBuilder.AppendLine(LocalizationHolder.rm.GetString("Pdm_521"));
    }
    else
    {
      stringBuilder.AppendLine(LocalizationHolder.rm.GetString("Pdm_522"));
      stringBuilder.AppendLine("--------------------------");
      foreach (KeyValuePair<int, bool> relType in relTypes)
      {
        if (relType.Value)
          stringBuilder.AppendLine(MetaDataHelper.GetRelationTypeName(relType.Key));
      }
      if (attrs != null && attrs.Count > 0)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine(LocalizationHolder.rm.GetString("Pdm_523"));
        stringBuilder.AppendLine("----------------------");
        for (int index = 0; index < attrs.Count; ++index)
          stringBuilder.AppendLine(MetaDataHelper.GetAttributeTypeName(attrs[index]));
      }
    }
    this._buttonRelTypes.ToolTipText = stringBuilder.ToString();
  }

  private void CircleMethod()
  {
    string[] strArray = new string[4]{ "|", "/", "-", "\\" };
    int index = 0;
    while (true)
    {
      this._statusBar.Invoke((Delegate) new CompareSettingsControl.SetCircleHandler(this.SetCircle), (object) strArray[index]);
      if (index == 3)
        index = 0;
      else
        ++index;
      Thread.Sleep(500);
    }
  }

  private void SetCircle(string text) => this._ttslCircle.Text = text;

  private delegate void SetCircleHandler(string text);
}
