// Decompiled with JetBrains decompiler
// Type: Intermech.PdmConfigurator.AppConditionsEditor
// Assembly: Intermech.PdmConfigurator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: B5CB2E26-657B-4329-B46C-77AE46A32171
// Assembly location: D:\IPS\Client\Intermech.PdmConfigurator.dll

using DevExpress.IM.XtraEditors.Controls;
using DevExpress.IM.XtraEditors.Mask;
using DevExpress.IM.XtraTreeList.Nodes;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Compositions;
using Intermech.Interfaces.PdmConfigurator;
using Intermech.Localization;
using System;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace Intermech.PdmConfigurator;

public sealed class AppConditionsEditor : IncompatibilityEditor
{
  private PdmContextAccessRights _pdmContextAccessRights;
  private IPdmCriterion _pdmCollection;
  private IContainer components;

  public AppConditionsEditor()
  {
    this.InitializeComponent();
    this.cbVisibleValue.AutoComplete = true;
    this.cbVisibleValue.TextEditStyle = TextEditStyles.Standard;
    LocalizationHolder.rm.GetString("PdmConfigurator_28");
    HelpProvidersClass.SetHelpOptionForControl((Control) this, 1836);
  }

  public IPdmCriterion PdmCriterionCollection
  {
    get => this._pdmCriterionsCollection.Clone() as IPdmCriterion;
  }

  public void LoadOptions(
    IPdmCriterion collection,
    RelationPair parentKey,
    PdmContextAccessRights accessRights)
  {
    this._pdmContextAccessRights = accessRights;
    this._pdmCollection = collection;
    if (parentKey == null || parentKey.F_PROJ_ID == 0L)
      return;
    ObjectOptionsHolder options = PdmConfiguratorObjectOptionsCache.GetObjectOptions(parentKey.F_PROJ_ID);
    if (options == null)
    {
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        IDBObject source = sessionKeeper.Session.GetObject(parentKey.F_PROJ_ID, false);
        if (source == null)
          return;
        options = new ObjectOptionsHolder((object) source);
      }
    }
    this.LoadOptions(options, (OptionHolder) null, (OptionValue) null, accessRights == PdmContextAccessRights.FullAccess ? OptionAccessRights.FullAccess : OptionAccessRights.ReadOnly);
  }

  internal PdmContextAccessRights CheckAccessRights(IDBAttributable item)
  {
    PdmContextAccessRights contextAccessRights1 = PdmContextAccessRights.ReadOnly;
    if (item == null)
      return contextAccessRights1;
    IDBAttribute byId = item.Attributes.FindByID(Intermech.Interfaces.PdmConfigurator.Consts.attributeObjectApplicabilityCondID);
    PdmContextAccessRights contextAccessRights2 = byId == null || byId.ReadOnly ? PdmContextAccessRights.ReadOnly : PdmContextAccessRights.FullAccess;
    if (byId != null || !(item is IDBRelation dbRelation))
      return contextAccessRights2;
    IDBObject dbObject = dbRelation.Session.GetObject(dbRelation.ProjID, false);
    if (dbObject != null)
    {
      try
      {
        dbObject.CheckRelationsEdit();
        contextAccessRights2 = PdmContextAccessRights.FullAccess;
      }
      catch
      {
      }
    }
    return contextAccessRights2;
  }

  protected override void InitAvailableOptions()
  {
    this.cbAvailableOptions.TextEditStyle = TextEditStyles.DisableTextEditor;
    if (this.cbAvailableOptions.Buttons.Count > 0)
      this.cbAvailableOptions.Buttons[0].Width = 14;
    this.cbAvailableOptions.DropDownRows = 15;
    this.cbAvailableOptions.BeginUpdate();
    try
    {
      this.cbAvailableOptions.Items.Clear();
      foreach (long option1 in this._objectOptionsHolder.Options)
      {
        OptionHolder option2 = PdmConfiguratorCache.CacheFindOption(option1);
        if (option2 == null)
        {
          using (SessionKeeper sessionKeeper = new SessionKeeper())
          {
            PdmConfiguratorCache.CacheAddOption(sessionKeeper.Session, option1);
            option2 = PdmConfiguratorCache.CacheFindOption(option1);
          }
        }
        if (option2 != null)
          this.cbAvailableOptions.Items.Add((object) new MyElement((object) option2.OptionGuid, option2.OptionCaption, (object) option2));
      }
    }
    finally
    {
      this.cbAvailableOptions.EndUpdate();
    }
  }

  protected override bool DisableEdit() => this._objectOptionsHolder == null;

  protected override void FindCriterionCollection()
  {
    this._pdmCriterionsCollection = (IPdmCriterion) new PdmCriterionsCollection();
    this._pdmCriterionsCollection.Assign((object) this._pdmCollection);
  }

  protected override PdmCriterion SaveCriterion(TreeListNode currentNode)
  {
    ObjectsApplicabilitiesCriterion applicabilitiesCriterion = new ObjectsApplicabilitiesCriterion();
    applicabilitiesCriterion.Option = !(currentNode[(object) "CONFLICT_OPTION"] is MyElement myElement1) ? Guid.Empty : (Guid) myElement1.Value;
    applicabilitiesCriterion.Value = !(currentNode[(object) "CONFLICT_VALUE"] is MyElement myElement2) ? string.Empty : myElement2.Value.ToString();
    applicabilitiesCriterion.Operator = !(currentNode[(object) "OPERATION"] is MyElement myElement3) ? Operator.Undefined : (Operator) myElement3.Value;
    LogicalFunction logicalFunction = LogicalFunction.And;
    if (!this.IsLastNode(currentNode))
      logicalFunction = (LogicalFunction) this.NextNode(currentNode).ImageIndex;
    applicabilitiesCriterion.Function = logicalFunction;
    applicabilitiesCriterion.Not = currentNode[(object) "NotColumnKey"] == (object) "НЕ";
    return (PdmCriterion) applicabilitiesCriterion;
  }

  protected override bool EnabledAddition() => this._objectOptionsHolder.Options.Count > 0;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AppConditionsEditor));
    this.cbVisibleValue.BeginInit();
    this.cbAvailableOptions.BeginInit();
    this.cbOperators.BeginInit();
    this.teReadOnly.BeginInit();
    this.SuspendLayout();
    this.cbVisibleValue.MaskData.BeepOnError = (bool) componentResourceManager.GetObject("cbVisibleValue.MaskData.BeepOnError");
    this.cbVisibleValue.MaskData.Blank = componentResourceManager.GetString("cbVisibleValue.MaskData.Blank");
    this.cbVisibleValue.MaskData.EditMask = componentResourceManager.GetString("cbVisibleValue.MaskData.EditMask");
    this.cbVisibleValue.MaskData.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("cbVisibleValue.MaskData.IgnoreMaskBlank");
    this.cbVisibleValue.MaskData.MaskType = (MaskType) componentResourceManager.GetObject("cbVisibleValue.MaskData.MaskType");
    this.cbVisibleValue.MaskData.SaveLiteral = (bool) componentResourceManager.GetObject("cbVisibleValue.MaskData.SaveLiteral");
    this.cbAvailableOptions.MaskData.BeepOnError = (bool) componentResourceManager.GetObject("cbAvailableOptions.MaskData.BeepOnError");
    this.cbAvailableOptions.MaskData.Blank = componentResourceManager.GetString("cbAvailableOptions.MaskData.Blank");
    this.cbAvailableOptions.MaskData.EditMask = componentResourceManager.GetString("cbAvailableOptions.MaskData.EditMask");
    this.cbAvailableOptions.MaskData.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("cbAvailableOptions.MaskData.IgnoreMaskBlank");
    this.cbAvailableOptions.MaskData.MaskType = (MaskType) componentResourceManager.GetObject("cbAvailableOptions.MaskData.MaskType");
    this.cbAvailableOptions.MaskData.SaveLiteral = (bool) componentResourceManager.GetObject("cbAvailableOptions.MaskData.SaveLiteral");
    this.cbOperators.MaskData.BeepOnError = (bool) componentResourceManager.GetObject("cbOperators.MaskData.BeepOnError");
    this.cbOperators.MaskData.Blank = componentResourceManager.GetString("cbOperators.MaskData.Blank");
    this.cbOperators.MaskData.EditMask = componentResourceManager.GetString("cbOperators.MaskData.EditMask");
    this.cbOperators.MaskData.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("cbOperators.MaskData.IgnoreMaskBlank");
    this.cbOperators.MaskData.MaskType = (MaskType) componentResourceManager.GetObject("cbOperators.MaskData.MaskType");
    this.cbOperators.MaskData.SaveLiteral = (bool) componentResourceManager.GetObject("cbOperators.MaskData.SaveLiteral");
    this.teReadOnly.MaskData.BeepOnError = (bool) componentResourceManager.GetObject("teReadOnly.MaskData.BeepOnError");
    this.teReadOnly.MaskData.Blank = componentResourceManager.GetString("teReadOnly.MaskData.Blank");
    this.teReadOnly.MaskData.EditMask = componentResourceManager.GetString("teReadOnly.MaskData.EditMask");
    this.teReadOnly.MaskData.IgnoreMaskBlank = (bool) componentResourceManager.GetObject("teReadOnly.MaskData.IgnoreMaskBlank");
    this.teReadOnly.MaskData.MaskType = (MaskType) componentResourceManager.GetObject("teReadOnly.MaskData.MaskType");
    this.teReadOnly.MaskData.SaveLiteral = (bool) componentResourceManager.GetObject("teReadOnly.MaskData.SaveLiteral");
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Name = nameof (AppConditionsEditor);
    this.cbVisibleValue.EndInit();
    this.cbAvailableOptions.EndInit();
    this.cbOperators.EndInit();
    this.teReadOnly.EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
