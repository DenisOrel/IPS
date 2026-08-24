// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ImbaseFieldsSettings.ImbaseTableRecordTypeItemList
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Imbase.ImbaseFieldsSettings;

internal class ImbaseTableRecordTypeItemList
{
  protected Dictionary<Guid, ImbaseTableRecordTypeItem> guids;
  protected Dictionary<string, ImbaseTableRecordTypeItem> names;
  protected Dictionary<string, ImbaseTableRecordTypeItem> shortNames;
  protected List<ImbaseTableRecordTypeItem> items;

  public List<ImbaseTableRecordTypeItem> Items => this.items;

  public ImbaseTableRecordTypeItemList()
  {
    this.guids = new Dictionary<Guid, ImbaseTableRecordTypeItem>();
    this.names = new Dictionary<string, ImbaseTableRecordTypeItem>();
    this.shortNames = new Dictionary<string, ImbaseTableRecordTypeItem>();
    this.items = new List<ImbaseTableRecordTypeItem>();
  }

  public void Add(ImbaseTableRecordTypeItem item) => this.items.Add(item);

  public bool ExistsByGuid(Guid guid) => this.guids.ContainsKey(guid);

  public bool ExistsByName(string name) => this.names.ContainsKey(name);

  public bool ExistsByShortName(string shortName) => this.shortNames.ContainsKey(shortName);

  public ImbaseTableRecordTypeItem GetByGuid(Guid guid)
  {
    return !this.ExistsByGuid(guid) ? (ImbaseTableRecordTypeItem) null : this.guids[guid];
  }

  public ImbaseTableRecordTypeItem GetByName(string name)
  {
    return !this.ExistsByName(name) ? (ImbaseTableRecordTypeItem) null : this.names[name];
  }

  public ImbaseTableRecordTypeItem GetByShortName(string shortName)
  {
    return !this.ExistsByShortName(shortName) ? (ImbaseTableRecordTypeItem) null : this.shortNames[shortName];
  }
}
