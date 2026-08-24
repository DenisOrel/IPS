// Decompiled with JetBrains decompiler
// Type: Intermech.NX.Integrator.DrawCreator.ChoiceDocument
// Assembly: Intermech.NX.Integrator, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D5A5DA32-DA1F-4D5A-845A-F0226BC2C153
// Assembly location: D:\IPS\Client\Intermech.NX.Integrator.dll

using Intermech.Client.Core.ObjectCreator;
using Intermech.Client.Core.ObjectCreator.Controls;
using Intermech.DataFormats;
using Intermech.Files;
using Intermech.Interfaces;
using Intermech.PropertyEditors;
using Intermech.Tools.Data;
using Intermech.Tools.Integrators;
using Intermech.Tools.Integrators.CADInterface;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.NX.Integrator.DrawCreator;

internal class ChoiceDocument : ObjectCreatorControl, IStepRefreshManager, IButtonManager
{
  private IIntegrator _nxIntegrator;
  private IFileVault _fileVaultService;
  private NXModelDrawingsService _modelDrawingsService;
  private DrawCreatorResult _result;
  private bool _isVisibleFlag;
  private IContainer components;
  private Label creatorTitleLabel;
  private TextBox selectObjectCaption;
  private Button selectObjectBtn;
  private ToolTip toolTip1;

  public ChoiceDocument(
    CreatedObjectItem objItem,
    IIntegrator nxIntegrator,
    IFileVault fileVaultService,
    NXModelDrawingsService modelDrawingsService,
    DrawCreatorResult result)
    : base(objItem)
  {
    this.InitializeComponent();
    this._nxIntegrator = nxIntegrator;
    this._fileVaultService = fileVaultService;
    this._modelDrawingsService = modelDrawingsService;
    this._result = result;
  }

  public void SetPageData()
  {
    this.creatorTitleLabel.Text = "Выберите объект, на основании которого будет создаваться " + this.CreatedObject.ObjectTypeCaption;
  }

  private void selectObjectBtn_Click(object sender, EventArgs e)
  {
    ICADSettingsService service = ServiceUtils.GetService<ICADSettingsService>((object) this._nxIntegrator, true);
    DocumentGroup byName1 = service.GetCADSettings().FileDocumentGroups.FindByName("AssemblyDrawing", false);
    DocumentGroup byName2 = service.GetCADSettings().FileDocumentGroups.FindByName("PartDrawing", false);
    if (byName1.ContainsType(this.CreatedObject.ObjectTypeID))
    {
      IDBObjectID[] dbObjectIdArray = SelectorForm.SelectObjects(service.GetCADSettings().FileDocumentGroups.FindByName("Assembly", true).AsIdList().ToArray(), (long[]) null, false, false, false);
      if (dbObjectIdArray == null || dbObjectIdArray.Length != 1)
        return;
      this.SetNameAndDesignation(dbObjectIdArray[0]);
    }
    else
    {
      if (!byName2.ContainsType(this.CreatedObject.ObjectTypeID))
        return;
      IDBObjectID[] dbObjectIdArray = SelectorForm.SelectObjects(service.GetCADSettings().FileDocumentGroups.FindByName("Part", true).AsIdList().ToArray(), (long[]) null, false, false, false);
      if (dbObjectIdArray == null || dbObjectIdArray.Length != 1)
        return;
      this.SetNameAndDesignation(dbObjectIdArray[0]);
    }
  }

  private void SetNameAndDesignation(IDBObjectID selectedObjects)
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IDBObject dbObject1 = sessionKeeper.Session.GetObject(selectedObjects.Value);
      IDBObject dbObject2 = sessionKeeper.Session.GetObject(this.CreatedObject.ObjectID);
      IDBAttribute attributeByGuid1 = dbObject2.GetAttributeByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"));
      IDBAttribute attributeByGuid2 = dbObject2.GetAttributeByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
      IDBAttribute attributeByGuid3 = dbObject1.GetAttributeByGuid(new Guid("cad00020-306c-11d8-b4e9-00304f19f545"));
      IDBAttribute attributeByGuid4 = dbObject1.GetAttributeByGuid(new Guid("cad0001f-306c-11d8-b4e9-00304f19f545"));
      attributeByGuid1.Value = attributeByGuid3.Value;
      string str = DocumentDesignationHelper.AppendDocCode(DocumentDesignationHelper.RemoveDocCode(attributeByGuid4.Value.ToString(), dbObject1.ObjectType), dbObject2.ObjectType);
      attributeByGuid2.Value = (object) str;
      this.selectObjectCaption.Text = dbObject1.Caption;
      string masterFileName = this._fileVaultService.DBFilesInfo.GetMasterFileName(dbObject1.ObjectID, true);
      if (dbObject2.GetAttributeByGuid(new Guid("cad0004b-306c-11d8-b4e9-00304f19f545")) is IDBFileAttribute attributeByGuid5)
      {
        SetFileAttrPrototype.Execute((IDBAttribute) attributeByGuid5, sessionKeeper.Session, dbObject2);
        attributeByGuid5.Rename(this.GenerateUniqueDrawingFileName(dbObject2, masterFileName));
      }
      this._result.ModelID = selectedObjects.Value;
      if (this._result.DrawingToModelRelationID != 0L)
      {
        sessionKeeper.Session.GetRelation(this._result.DrawingToModelRelationID, false)?.Delete(0L);
        this._result.DrawingToModelRelationID = 0L;
      }
      this._result.DrawingToModelRelationID = sessionKeeper.Session.GetRelationCollection(IDCache.Default.DocumentTree.Id).Create(dbObject2.ObjectID, dbObject1.ObjectID).RelationID;
      if (this.SetButtonEnabledEvent != null)
        this.SetButtonEnabledEvent(ButtonType.Finish, this.StepIsReady);
      this.Refresh();
    }
  }

  private string GenerateUniqueDrawingFileName(IDBObject newDrawingObject, string modelFileName)
  {
    if (this._modelDrawingsService.SettingsProvider.GetDrawingSuffixes().Count == 0)
      throw new BadIntegratorSettingsException(this._nxIntegrator.DisplayName, "В настройках интегратора не заданых суффиксы для имен файлов чертежей.");
    string drawingFile;
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      IFileNamesService fileNamesService = ServiceUtils.GetService<IFileNamesService>((object) sessionKeeper.Session, true);
      Guid sessionGuid = sessionKeeper.Session.SessionGUID;
      drawingFile = this._modelDrawingsService.FindDrawingFile(modelFileName, (Func<string, bool>) (possibleDrawingFileName => fileNamesService.GetIDByFileName(possibleDrawingFileName, sessionGuid) == -1L));
    }
    return drawingFile != null ? drawingFile : throw new FaultException("Не удалость создать уникальное имя файла для чертежа.");
  }

  public override bool StepIsReady
  {
    get => this.selectObjectCaption.Text.Length > 0 || !this._isVisibleFlag;
  }

  public override bool StepIsReadyCheckRequired => true;

  public override bool ShowBeforeDesForms => true;

  public bool RefreshOnNextStep => true;

  public bool RefreshOnPrevStep => true;

  public event SetButtonEnabledHandler SetButtonEnabledEvent;

  public bool IsButtonEnabledEventSubscribed => this.SetButtonEnabledEvent != null;

  public override bool Save(PageSaveArgs args)
  {
    this._isVisibleFlag = false;
    return base.Save(args);
  }

  public override bool Refresh(PageRefreshArgs args)
  {
    this._isVisibleFlag = true;
    return base.Refresh(args);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this.creatorTitleLabel = new Label();
    this.selectObjectCaption = new TextBox();
    this.selectObjectBtn = new Button();
    this.toolTip1 = new ToolTip(this.components);
    this.SuspendLayout();
    this.creatorTitleLabel.AutoSize = true;
    this.creatorTitleLabel.Location = new Point(3, 4);
    this.creatorTitleLabel.Name = "creatorTitleLabel";
    this.creatorTitleLabel.Size = new Size(35, 13);
    this.creatorTitleLabel.TabIndex = 0;
    this.creatorTitleLabel.Text = "label1";
    this.selectObjectCaption.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.selectObjectCaption.BackColor = SystemColors.ControlLightLight;
    this.selectObjectCaption.Location = new Point(6, 26);
    this.selectObjectCaption.Name = "selectObjectCaption";
    this.selectObjectCaption.ReadOnly = true;
    this.selectObjectCaption.Size = new Size(595, 20);
    this.selectObjectCaption.TabIndex = 1;
    this.selectObjectBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.selectObjectBtn.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
    this.selectObjectBtn.Location = new Point(604, 24);
    this.selectObjectBtn.Name = "selectObjectBtn";
    this.selectObjectBtn.Size = new Size(27, 23);
    this.selectObjectBtn.TabIndex = 2;
    this.selectObjectBtn.Text = "...";
    this.toolTip1.SetToolTip((Control) this.selectObjectBtn, "Выбрать объект");
    this.selectObjectBtn.UseVisualStyleBackColor = true;
    this.selectObjectBtn.Click += new EventHandler(this.selectObjectBtn_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.selectObjectBtn);
    this.Controls.Add((Control) this.selectObjectCaption);
    this.Controls.Add((Control) this.creatorTitleLabel);
    this.Name = nameof (ChoiceDocument);
    this.Size = new Size(642, 60);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
