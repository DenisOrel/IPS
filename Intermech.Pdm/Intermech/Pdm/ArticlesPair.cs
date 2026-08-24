// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.ArticlesPair
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using Intermech.Interfaces.Client;
using System;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm;

public class ArticlesPair : IComparable<ArticlesPair>
{
  public long TemplateArticleID;
  public int TemplateArticleTypeID = -1;
  public string TemplateArticleDesignation = string.Empty;
  public string TemplateArticleName = string.Empty;
  public bool TemplateArticleIsMain;
  public long TemplateArticleCheckedOutBy;
  public Icon TemplateArticleTypeIcon;
  public string NewTemplateDesignation = string.Empty;
  public bool NewTemplateEnabled = true;
  public long NewTemplateID;

  public ArticlesPair()
  {
  }

  public ArticlesPair(IDBObject artObject, string docDesignation)
  {
    if (artObject == null)
      return;
    string str1 = string.Empty;
    IDBAttribute attributeById1 = artObject.GetAttributeByID(MetaDataHelper.GetAttributeTypeID("cad0001f-306c-11d8-b4e9-00304f19f545"));
    if (attributeById1 != null)
      str1 = attributeById1.AsString;
    string str2 = string.Empty;
    IDBAttribute attributeById2 = artObject.GetAttributeByID(MetaDataHelper.GetAttributeTypeID("cad00020-306c-11d8-b4e9-00304f19f545"));
    if (attributeById2 != null)
      str2 = attributeById2.AsString;
    this.TemplateArticleID = artObject.ObjectID;
    this.TemplateArticleTypeID = artObject.ObjectType;
    this.TemplateArticleDesignation = str1;
    this.TemplateArticleName = str2;
    this.TemplateArticleIsMain = str1.ToUpperInvariant().Equals(docDesignation.ToUpperInvariant());
    this.TemplateArticleCheckedOutBy = artObject.CheckoutBy;
    this.TemplateArticleTypeIcon = this.GetObjTypeIcon(this.TemplateArticleTypeID);
    this.NewTemplateDesignation = str1 + ".1";
    this.NewTemplateEnabled = true;
  }

  public ArticlesPair(
    long templateArticleID,
    int templateArticleTypeID,
    string templateArticleDesignation,
    string templateArticleName,
    bool templateArticleIsMain,
    long templateArticleCheckedOutBy,
    Icon templateArticleTypeIcon,
    string newTemplateDesignation,
    bool newTemplateEnabled)
  {
    this.TemplateArticleID = templateArticleID;
    this.TemplateArticleTypeID = templateArticleTypeID;
    this.TemplateArticleDesignation = templateArticleDesignation;
    this.TemplateArticleName = templateArticleName;
    this.TemplateArticleIsMain = templateArticleIsMain;
    this.TemplateArticleCheckedOutBy = templateArticleCheckedOutBy;
    this.TemplateArticleTypeIcon = templateArticleTypeIcon;
    this.NewTemplateDesignation = newTemplateDesignation;
    this.NewTemplateEnabled = newTemplateEnabled;
  }

  protected virtual Icon GetObjTypeIcon(int objTypeID)
  {
    objTypeID = Math.Max(objTypeID, -1);
    ICategoryTypeIconService service = ServicesManager.GetService(typeof (ICategoryTypeIconService)) as ICategoryTypeIconService;
    return service.IndexOf(4, objTypeID) < 0 ? (Icon) null : ImagesResizeHelper.ResizeIconTo32x16(service.GetIcon(4, objTypeID), SystemColors.Window);
  }

  public override bool Equals(object obj) => this.CompareTo(obj as ArticlesPair) == 0;

  public override int GetHashCode() => this.TemplateArticleID.GetHashCode();

  public int CompareTo(ArticlesPair other)
  {
    if (other == null)
      return 1;
    if (!other.TemplateArticleIsMain && this.TemplateArticleIsMain)
      return -1;
    return other.TemplateArticleIsMain && !this.TemplateArticleIsMain ? 1 : this.TemplateArticleDesignation.ToUpperInvariant().CompareTo(other.TemplateArticleDesignation.ToUpperInvariant());
  }
}
