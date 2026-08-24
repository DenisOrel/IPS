// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.SearchData.Article
// Assembly: Intermech.ImpExp.SearchData, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 218D3933-9EC7-421F-AD43-19C3596D6EE8
// Assembly location: D:\IPS\Client\Intermech.ImpExp.SearchData.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.SearchData;

public class Article : S4DBItem
{
  private int _sectID = -1;
  private int _artVerID = -2;
  private int _docID;
  private int _docType = -1;
  public long ExistedDocObjectID;
  public int ExistedArtID;
  public int DocTypeToCreate = -1;
  public Dictionary<int, Article> Versions = new Dictionary<int, Article>();
  internal DocsLinks DocsLinks;
  public bool SBFromSP;

  public int ID
  {
    get
    {
      if (this._id == 0 && this.Data.ContainsKey("art_id"))
        this._id = Convert.ToInt32(this.Data["art_id"]);
      return this._id;
    }
  }

  public int SectID
  {
    get
    {
      if (this._sectID == -1 && this.Data.ContainsKey("section_id"))
        this._sectID = Convert.ToInt32(this.Data["section_id"]);
      return this._sectID;
    }
  }

  internal static int SuffixPos(string s, bool NumericKeysOnly)
  {
    int length = s.Length;
    if (length <= 2)
      return -1;
    int index = length - 1;
    if (NumericKeysOnly)
    {
      while (index > 2 && s[index] >= '0' && s[index] <= '9')
        --index;
    }
    else
    {
      while (index > 2 && s[index] != '^')
        --index;
    }
    return s[index - 1] == '&' && s[index] == '^' ? index - 1 : -1;
  }

  internal static int SuffixPos(string s) => Article.SuffixPos(s, true);

  protected override string getDesignation()
  {
    string designation = base.getDesignation();
    int startIndex = Article.SuffixPos(designation);
    if (startIndex > 0)
      designation = designation.Remove(startIndex);
    if (this.SectID != 1 && PluginSettings.ArtSuffixesToDelete != null && PluginSettings.ArtSuffixesToDelete.Count > 0)
      designation = PumpHelper.TrimArticleDesignationSuffix(designation);
    return designation;
  }

  public string SectTableName => $"SECT_{this.SectID}";

  public int ArtVerID
  {
    get
    {
      if (this._artVerID == -2)
      {
        object obj = this.Data["art_ver_id"];
        this._artVerID = !DBNull.Value.Equals(obj) ? Convert.ToInt32(obj) : 0;
      }
      return this._artVerID;
    }
  }

  public void HackArtVerID(int value) => this._artVerID = value;

  public int DocID
  {
    get
    {
      if (this._docID == 0)
        this._docID = Convert.ToInt32(this.Data["doc_id"]);
      return this._docID;
    }
  }

  public int DocType
  {
    get
    {
      if (this._docType == -1)
        this._docType = DBNull.Value.Equals(this.Data["doc_type"]) ? 0 : Convert.ToInt32(this.Data["doc_type"]);
      return this._docType;
    }
  }

  public void HackDocID(int value) => this._docID = value;

  internal override void Clear()
  {
    base.Clear();
    this._sectID = -1;
    this._docID = 0;
    this._artVerID = -2;
    this.ExistedDocObjectID = 0L;
    this.ExistedArtID = 0;
    this.DocTypeToCreate = -1;
    this.DocsLinks = (DocsLinks) null;
    this.Versions.Clear();
    this.SBFromSP = false;
    this._docType = -1;
  }

  public List<Article> PlainList
  {
    get
    {
      List<Article> plainList = new List<Article>();
      int artVerId = this.ArtVerID;
      if (!PluginSettings.PumpArtVersions)
        plainList.Add(this);
      else if (!PumpHelper.IsNewPCFormat)
      {
        plainList.Add(this);
        foreach (KeyValuePair<int, Article> version in this.Versions)
          plainList.Add(version.Value);
      }
      else
      {
        foreach (KeyValuePair<int, Article> version in this.Versions)
        {
          if (version.Key == this.ArtVerID)
          {
            plainList.Add(this);
            Article article = version.Value;
            this.Data["doc_ver_id"] = article.Data["doc_ver_id"];
            this.Data["vart_type"] = article.Data["vart_type"];
            this.Data["author"] = article.Data["author"];
            this.Data["vart_note"] = article.Data["vart_note"];
            this.Data["prev_art_ver_id"] = article.Data["prev_art_ver_id"];
          }
          else
            plainList.Add(version.Value);
        }
      }
      return plainList;
    }
  }

  internal VartType VartType
  {
    get
    {
      object obj = (object) null;
      return this.Data.TryGetValue("vart_type", out obj) ? (VartType) Convert.ToInt32(obj) : VartType.Version;
    }
  }

  internal ArtClass ArtClass
  {
    get
    {
      object obj = (object) null;
      return this.Data.TryGetValue("art_class", out obj) ? (ArtClass) Convert.ToInt32(obj) : ArtClass.Serial;
    }
  }

  private long GroupingID
  {
    get
    {
      if (this.DocID <= 0)
        return 0;
      long groupingId = (long) (this.DocID * 1000);
      object obj = (object) -1;
      if (this.Data.TryGetValue("doc_ver_id", out obj))
        groupingId += (long) (Convert.ToInt32(obj) + 1);
      return groupingId;
    }
  }

  public string GroupingGuid
  {
    get => this.DocID > 0 ? new Guid("cad0000000000000" + $"{this.GroupingID:x16}").ToString() : "";
  }
}
