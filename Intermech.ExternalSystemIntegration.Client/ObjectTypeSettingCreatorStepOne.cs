// Decompiled with JetBrains decompiler
// Type: Intermech.ExternalSystemIntegration.Client.ObjectTypeSettingCreatorStepOne
// Assembly: Intermech.ExternalSystemIntegration.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 7B2572D1-83D9-44E0-9FE5-1A0AEA2F505B
// Assembly location: D:\IPS\Client\Intermech.ExternalSystemIntegration.Client.dll

using Intermech.Client.Core;
using Intermech.Client.Core.ObjectCreator;
using Intermech.Client.Core.ObjectCreator.Controls;
using Intermech.ExternalSystemIntegration.Interfaces;
using Intermech.Interfaces;
using Intermech.Kernel.Search;
using Intermech.PropertyEditors;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.ExternalSystemIntegration.Client;

public class ObjectTypeSettingCreatorStepOne : ObjectCreatorControl
{
  private IContainer components;
  private ButtonedEdit edObjectType;

  public ObjectTypeSettingCreatorStepOne(CreatedObjectItem createdObject)
    : base(createdObject)
  {
    this.InitializeComponent();
  }

  public override bool Save(PageSaveArgs args)
  {
    bool flag = false;
    try
    {
      if (this.edObjectType.Tag == null || !((string) this.edObjectType.Tag != string.Empty))
        throw new Exception("Не указан тип объекта!");
      using (SessionKeeper sessionKeeper = new SessionKeeper())
      {
        string tag = (string) this.edObjectType.Tag;
        if (sessionKeeper.Session.GetObjectCollection(Const.TypeSettingItemObjTypeID).Select(new DBRecordSetParams(new ConditionStructure[1]
        {
          new ConditionStructure(Const.ObjectTypeIDAttrTypeID, RelationalOperators.Equal, (object) tag, LogicalOperators.NONE, 0, false)
        }, new object[1]
        {
          (object) ObligatoryObjectAttributes.F_OBJECT_ID
        }, (object[]) null, (SortOrders[]) null, 0L, (object) null, -1, true, "MyObjects")).Rows.Count > 0)
          throw new Exception("Настройка для данного типа объекта уже существует!");
        if (sessionKeeper.Session.GetObject(this.CreatedObject.ObjectID, false) is IObjTypeSettingItemObject settingItemObject)
        {
          settingItemObject.ObjTypeGUID = tag;
          settingItemObject.Caption = sessionKeeper.Session.GetObjectType(new Guid(tag)).ObjectTypeName;
          flag = true;
        }
      }
      return flag;
    }
    catch (Exception ex)
    {
      args.Error = ex;
      return false;
    }
  }

  private void edObjectType_ButtonClick(object sender, EventArgs e)
  {
    using (SelectorForm selectorForm = new SelectorForm("Выберите тип объекта", 4, false))
    {
      if (selectorForm.ShowDialog() != DialogResult.OK || selectorForm.IDList.Count != 1)
        return;
      string g = MetaDataHelper.GetObjectTypeGuid((int) selectorForm.IDList[0]).ToString();
      if (!(g != string.Empty))
        return;
      this.edObjectType.Value = MetaDataHelper.GetObjectTypeName(new Guid(g));
      this.edObjectType.Image = ServiceHolder.CategoryTypeIconService.ImageList.Images[ServiceHolder.CategoryTypeIconService.IndexOf(4, MetaDataHelper.GetObjectTypeID(new Guid(g)))];
      this.edObjectType.Tag = (object) g;
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.edObjectType = new ButtonedEdit();
    this.SuspendLayout();
    this.edObjectType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.edObjectType.ButtonImage = (Image) null;
    this.edObjectType.ButtonText = "...";
    this.edObjectType.Caption = "Тип объекта:";
    this.edObjectType.CaptionFont = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.edObjectType.Image = (Image) null;
    this.edObjectType.Location = new Point(30, 50);
    this.edObjectType.MinimumSize = new Size(40, 20);
    this.edObjectType.Name = "edObjectType";
    this.edObjectType.ReadOnly = true;
    this.edObjectType.Size = new Size(540, 42);
    this.edObjectType.TabIndex = 2;
    this.edObjectType.ButtonClick += new EventHandler(this.edObjectType_ButtonClick);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.edObjectType);
    this.Name = nameof (ObjectTypeSettingCreatorStepOne);
    this.Size = new Size(600, 300);
    this.ResumeLayout(false);
  }
}
