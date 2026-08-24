// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ArticlesView.ListArticlesView
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Bars;
using Intermech.DataFormats;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using Intermech.Interfaces.Pdm;
using Intermech.Localization;
using Intermech.Navigator.ArticlesList;
using Intermech.Navigator.Controls;
using Intermech.Navigator.DBObjects;
using Intermech.Navigator.Interfaces;
using Intermech.Navigator.Views;
using Intermech.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Intermech.Pdm.ArticlesView;

[ViewDescriptionProvider(typeof (ListArticlesView.ListArticlesViewDescriptionProvider))]
public class ListArticlesView : UserControl, IView, ICommandTarget
{
  private NavigatorControl _navControl;
  private int _imageIndex = -1;
  private AdvancedServiceContainer _services;
  private bool _dataLoaded;
  private int _documentType;
  private long _documentID;
  public ArticlesListDescriptor _descriptor;
  private List<long> _articles;
  private bool _activated;
  private IContainer components;
  private Label label1;

  public ListArticlesView()
  {
    this.InitializeComponent();
    this._services = new AdvancedServiceContainer();
    this._services.AddService(typeof (ObjectsSelectionOptionsHolder), (object) new ObjectsSelectionOptionsHolder(ObjectsSelectionOptions.ShowAllModifications));
    this._navControl.NavTreeView.Services = (IServiceProvider) this._services;
    this._navControl.NavTreeView.SupportedColumns = Intermech.Navigator.Utils.DefaultSupportedColumnsObjects();
    INamedImageList service1 = (INamedImageList) ServicesManager.GetService(typeof (INamedImageList));
    if (service1 != null)
      this._imageIndex = service1.ImageIndex("imgObject");
    INotificationService service2 = (INotificationService) ServicesManager.GetService(typeof (INotificationService));
    if (service2 == null)
      return;
    service2.Subscribe("ObjectsCreated", new NotificationEventHandler(this.OnChangesListInstances));
    service2.Subscribe("ObjectsChanged", new NotificationEventHandler(this.ObjectsChangedHandler));
    service2.Subscribe("ObjectsRemoved", new NotificationEventHandler(this.OnChangesListInstances));
    service2.Subscribe("ObjectsCheckedOut", new NotificationEventHandler(this.ObjectsChangedHandler));
    service2.Subscribe("ObjectsCheckedIn", new NotificationEventHandler(this.ObjectsChangedHandler));
    service2.Subscribe("ObjectsChangesCancelled", new NotificationEventHandler(this.ObjectsChangedHandler));
    service2.Subscribe("RelationsCreated", new NotificationEventHandler(this.RelationChangedHandler));
    service2.Subscribe("RelationsRemoved", new NotificationEventHandler(this.RelationChangedHandler));
  }

  private void ObjectsChangedHandler(object sender, NotificationEventArgs e)
  {
    if (!(e is DBObjectsEventArgs objectsEventArgs) || objectsEventArgs.ObjectIDs == null)
      return;
    this.OnChange(objectsEventArgs.ObjectIDs);
  }

  private void OnChangesListInstances(object sender, NotificationEventArgs e)
  {
    if (this._articles == null || !(e is DBObjectsEventArgs objectsEventArgs) || objectsEventArgs.ObjectIDs == null)
      return;
    List<Article> articles = this.GetArticles();
    List<long> collection = articles.ConvertAll<long>((Converter<Article, long>) (item => item.ArticleID));
    int count = this._articles != null ? this._articles.Count : 0;
    if (count == 0 || collection.Count == count && new HashSet<long>((IEnumerable<long>) collection).SetEquals((IEnumerable<long>) this._articles))
      return;
    this._dataLoaded = false;
    this.ReloadTreeView(0L, articles);
  }

  private void RelationChangedHandler(object sender, NotificationEventArgs e)
  {
    if (!(e is DBRelationsEventArgs relationsEventArgs) || relationsEventArgs.ProjIDs == null)
      return;
    this.OnChange((IList<long>) relationsEventArgs.ProjIDs);
  }

  private void OnChange(IList<long> changedObjs)
  {
    if (this._articles == null || changedObjs == null)
      return;
    foreach (long changedObj in (IEnumerable<long>) changedObjs)
    {
      if (this._articles.Contains(changedObj))
      {
        this._dataLoaded = false;
        this.ReloadTreeView(changedObj, (List<Article>) null);
        break;
      }
    }
  }

  public void Initialize(ISelectedItems items, IServiceProvider provider)
  {
    IDBTypedObjectID itemData = items.GetItemData(0, typeof (IDBTypedObjectID)) as IDBTypedObjectID;
    this._documentID = itemData.ObjectID;
    this._documentType = itemData.ObjectType;
    this._services.AdvancedProvider = provider;
    this._dataLoaded = false;
  }

  protected void SetTreeViewColumns()
  {
    INavigatorColumnsService navigatorColumnsService = ServiceLocator.Get<INavigatorColumnsService>();
    if (this._descriptor == null)
      return;
    INodeID recordNodeId = this._descriptor.GetRecordNodeID();
    if (recordNodeId == null)
      return;
    NavigatorColumns navigatorColumns = navigatorColumnsService.GetNavigatorColumns(recordNodeId.CategoryID, recordNodeId.TypeID, "TreeView", true);
    if (navigatorColumns == null)
      return;
    this._navControl.NavTreeView.SetColumns(navigatorColumns.Columns);
  }

  private bool ReloadTreeView(long currentObjectID, List<Article> articles)
  {
    if (articles == null)
      articles = this.GetArticles();
    if (articles.Count == 0)
      return false;
    int objectTypeID = -1;
    if (articles.Count > 0)
    {
      this._articles = articles.ConvertAll<long>((Converter<Article, long>) (article => article.ArticleID));
      objectTypeID = articles[0].ArticleType;
    }
    else
      this._articles = (List<long>) null;
    if (this._descriptor == null)
    {
      this._descriptor = new ArticlesListDescriptor(new Dictionary<int, List<long>>()
      {
        {
          objectTypeID,
          this._articles
        }
      }, objectTypeID);
      this.SetTreeViewColumns();
    }
    else
      this._descriptor.SetArticles(this._articles, objectTypeID);
    try
    {
      this._navControl.NavTreeView.Build((IDescriptor) this._descriptor);
      if (this._navControl.NavTreeView.Nodes[0].Children.Count > 0)
      {
        int index1 = 0;
        for (int index2 = 0; index2 < this._navControl.NavTreeView.Nodes[0].Children.Count; ++index2)
        {
          NavigatorTreeNode child = this._navControl.NavTreeView.Nodes[0].Children[index2];
          if (currentObjectID != 0L && child.NodeID is NodeID && ((NodeID) child.NodeID).ObjectID == currentObjectID)
            index1 = index2;
          child.Expanded = true;
        }
        this._navControl.NavTreeView.FocusedNode = this._navControl.NavTreeView.Nodes[0].Children[index1];
      }
    }
    catch
    {
    }
    this._dataLoaded = true;
    return articles.Count > 0;
  }

  public void Activate(IView previousView)
  {
    this._activated = true;
    if (this._dataLoaded)
      return;
    if (this.ReloadTreeView(0L, (List<Article>) null))
    {
      this.label1.Visible = false;
      this._navControl.Visible = true;
    }
    else
    {
      this._navControl.Visible = false;
      this.label1.Visible = true;
    }
  }

  public void Deactivate(IView nextView) => this._activated = false;

  public string Caption => LocalizationHolder.rm.GetString("Pdm_27");

  public int ImageIndex => this._imageIndex;

  public int OrderID => 12;

  private List<Article> GetArticles()
  {
    using (SessionKeeper sessionKeeper = new SessionKeeper())
    {
      ISubscriberArticlesView subscriber = SubscribersArticlesView.GetSubscriber(this._documentType);
      if (subscriber != null)
        return subscriber.GetArticles(sessionKeeper.Session, this._documentID);
      List<Article> articles = new List<Article>();
      IArticleService service1 = (IArticleService) ServicesManager.GetService(typeof (IArticleService));
      IFiltrationService service2 = (IFiltrationService) ServicesManager.GetService(typeof (IFiltrationService));
      long documentId = this._documentID;
      string filtrationServiceOwnerId = service2.FiltrationServiceOwnerID;
      IUserSession session = sessionKeeper.Session;
      List<QuickObjectInfo> listInstances = service1.FindListInstances(documentId, filtrationServiceOwnerId, (object) session);
      if (listInstances != null && listInstances.Count > 0)
      {
        for (int index = 0; index < listInstances.Count; ++index)
        {
          bool baseArticle = index == 0;
          articles.Add(new Article(listInstances[index].ObjectID, baseArticle, listInstances[index].ObjectTypeID, listInstances[index].Caption));
        }
      }
      return articles;
    }
  }

  public bool Execute(ICommandState commandState) => this._navControl.Execute(commandState);

  public bool QueryStatus(ICommandState commandState) => this._navControl.QueryStatus(commandState);

  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      INotificationService service = (INotificationService) ServicesManager.GetService(typeof (INotificationService));
      if (service != null)
      {
        service.Unsubscribe("ObjectsCreated", new NotificationEventHandler(this.OnChangesListInstances));
        service.Unsubscribe("ObjectsChanged", new NotificationEventHandler(this.ObjectsChangedHandler));
        service.Unsubscribe("ObjectsRemoved", new NotificationEventHandler(this.OnChangesListInstances));
        service.Unsubscribe("ObjectsCheckedOut", new NotificationEventHandler(this.ObjectsChangedHandler));
        service.Unsubscribe("ObjectsCheckedIn", new NotificationEventHandler(this.ObjectsChangedHandler));
        service.Unsubscribe("ObjectsChangesCancelled", new NotificationEventHandler(this.ObjectsChangedHandler));
        service.Unsubscribe("RelationsCreated", new NotificationEventHandler(this.RelationChangedHandler));
        service.Unsubscribe("RelationsRemoved", new NotificationEventHandler(this.RelationChangedHandler));
      }
      if (this.components != null)
        this.components.Dispose();
    }
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    this._navControl = new NavigatorControl();
    this.label1 = new Label();
    this.SuspendLayout();
    this.label1.AccessibleDescription = (string) null;
    this.label1.AccessibleName = (string) null;
    this.label1.Font = (Font) null;
    this.label1.Name = "label1";
    this.label1.Text = "Нет исполнений по документу";
    this.label1.AutoSize = false;
    this.label1.TextAlign = ContentAlignment.MiddleCenter;
    this.label1.Dock = DockStyle.Fill;
    this._navControl.Dock = DockStyle.Fill;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this._navControl);
    this.Name = nameof (ListArticlesView);
    this.Size = new Size(367, 266);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  private sealed class ListArticlesViewDescriptionProvider : BaseViewDescriptionProvider
  {
    public override ViewDescription DoGetViewDescription(
      ISelectedItems selectedItems,
      IServiceProvider serviceProvider)
    {
      if (!(serviceProvider.GetService(typeof (INamedImageList)) is INamedImageList service))
        service = ServicesManager.GetService(typeof (INamedImageList)) as INamedImageList;
      INamedImageList namedImageList = service;
      return new ViewDescription()
      {
        Caption = LocalizationHolder.rm.GetString("Pdm_27"),
        ImageIndex = namedImageList != null ? namedImageList.ImageIndex("imgObject") : -1,
        OrderID = 12
      };
    }
  }
}
