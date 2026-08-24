// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SearchScheme.SearchSheme
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Interfaces;
using Intermech.Interfaces.Pdm;
using Intermech.PropertyEditors;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Pdm.SearchScheme;

internal class SearchSheme
{
  public long SchemeID;
  public string Name = string.Empty;
  private SearchDirection _direction;
  public bool IsPersonal;
  public long SelectionID = -1;
  public string SelectionName = string.Empty;
  public List<GlobalType> ObjectTypes = new List<GlobalType>();
  public List<GlobalType> TypesToExpand = new List<GlobalType>();
  public List<GlobalType> TypesToDisableExpand = new List<GlobalType>();
  public List<GlobalType> RelationTypes = new List<GlobalType>();
  public List<ColumnSchemeAttProxy> ViewColumns = new List<ColumnSchemeAttProxy>();
  public List<SearchSchemeRole> Roles = new List<SearchSchemeRole>();
  public SearchOptions Options;
  public Guid VersionRule = Guid.Empty;

  public SearchDirection Direction
  {
    get => this._direction;
    set => this._direction = value;
  }

  public void LoadFromObject(IUserSession session, long anObjectID)
  {
    this.SchemeID = anObjectID;
    if (anObjectID == 0L)
      return;
    IDBObject scheme = session.GetObject(anObjectID);
    this.IsPersonal = scheme.ObjectType == MetaDataHelper.GetObjectTypeID("cad0012b-306c-11d8-b4e9-00304f19f545");
    this.Name = scheme.Caption;
    IDBAttribute attributeByGuid1 = scheme.GetAttributeByGuid(new Guid("cad00131-306c-11d8-b4e9-00304f19f545"));
    if (attributeByGuid1.Value != null && attributeByGuid1.Value != DBNull.Value)
      this.Direction = (SearchDirection) Convert.ToInt32(attributeByGuid1.Value);
    IDBAttribute attributeByGuid2 = scheme.GetAttributeByGuid(new Guid("cad00621-306c-11d8-b4e9-00304f19f545"));
    if (attributeByGuid2.Value != null && attributeByGuid2.Value != DBNull.Value)
    {
      IDBObject dbObject = session.GetObject(Convert.ToInt64(attributeByGuid2.Value));
      this.SelectionID = dbObject.ObjectID;
      this.SelectionName = dbObject.Caption != string.Empty ? dbObject.Caption : $"<{dbObject.ObjectID}>";
    }
    IDBAttribute attributeByGuid3 = scheme.GetAttributeByGuid(new Guid("cad0014a-306c-11d8-b4e9-00304f19f545"));
    if (attributeByGuid3 != null && attributeByGuid3.ValuesCount > 0)
    {
      foreach (object obj in attributeByGuid3.Values)
      {
        if (obj != null && obj != DBNull.Value)
          this.RelationTypes.Add(new GlobalType(obj.ToString(), 6, session));
      }
    }
    this.ReadAttributeToCollection(session, scheme, new Guid("cad00149-306c-11d8-b4e9-00304f19f545"), this.ObjectTypes);
    this.ReadAttributeToCollection(session, scheme, PDMHelper.attributeTypesToExpand, this.TypesToExpand);
    this.ReadAttributeToCollection(session, scheme, PDMHelper.attributeTypesToDisableExpand, this.TypesToDisableExpand);
    IDBAttribute attributeByGuid4 = scheme.GetAttributeByGuid(new Guid("cad00620-306c-11d8-b4e9-00304f19f545"));
    if (attributeByGuid4.Value != null && attributeByGuid4.Value != DBNull.Value)
    {
      foreach (object obj in attributeByGuid4.Values)
      {
        if (obj != null && (obj != DBNull.Value || obj.ToString().Length <= 0))
          this.ViewColumns.Add(new ColumnSchemeAttProxy(obj.ToString()));
      }
    }
    IDBAttribute attributeByGuid5 = scheme.GetAttributeByGuid(new Guid(SearchConsts.attributeVersionRule));
    if (attributeByGuid5 != null && attributeByGuid5.Value != DBNull.Value)
      this.VersionRule = new Guid(attributeByGuid5.AsString);
    if (!this.IsPersonal)
    {
      IDBAttribute attributeByGuid6 = scheme.GetAttributeByGuid(new Guid("cad00d18-306c-11d8-b4e9-00304f19f545"));
      if (attributeByGuid6 != null && attributeByGuid6.Values != null && attributeByGuid6.Values.Length != 0)
      {
        foreach (object obj in attributeByGuid6.Values)
        {
          if (CompareValuesHelper.NormalizedValue(obj) != null)
          {
            SearchSchemeRole searchSchemeRole = new SearchSchemeRole(obj.ToString(), session);
            if (searchSchemeRole.ValidRole)
              this.Roles.Add(searchSchemeRole);
          }
        }
      }
    }
    this.Options = SearchOptions.None;
    IDBAttribute attributeByGuid7 = scheme.GetAttributeByGuid(new Guid(SearchConsts.attributeSearchOptions));
    if (attributeByGuid7 == null || attributeByGuid7.Value == null || attributeByGuid7.Value == DBNull.Value)
      return;
    this.Options = (SearchOptions) attributeByGuid7.AsInteger;
  }

  public void SaveToObject(IUserSession session)
  {
    IDBObject scheme = session.GetObject(this.SchemeID);
    scheme.Caption = this.Name;
    scheme.GetAttributeByGuid(new Guid("cad00131-306c-11d8-b4e9-00304f19f545")).Value = (object) (int) this.Direction;
    scheme.GetAttributeByGuid(new Guid("cad00621-306c-11d8-b4e9-00304f19f545")).Value = this.SelectionID < 0L ? (object) 0L : (object) this.SelectionID;
    IDBAttribute attributeByGuid1 = scheme.GetAttributeByGuid(new Guid("cad0014a-306c-11d8-b4e9-00304f19f545"));
    if (this.RelationTypes.Count == 0)
    {
      attributeByGuid1.ClearValues();
    }
    else
    {
      List<string> stringList = new List<string>();
      foreach (GlobalType relationType in this.RelationTypes)
        stringList.Add(relationType.TypeGuid.ToString());
      attributeByGuid1.Values = (object[]) stringList.ToArray();
    }
    this.SaveCollectionToAttribute(scheme, new Guid("cad00149-306c-11d8-b4e9-00304f19f545"), this.ObjectTypes);
    this.SaveCollectionToAttribute(scheme, PDMHelper.attributeTypesToExpand, this.TypesToExpand);
    this.SaveCollectionToAttribute(scheme, PDMHelper.attributeTypesToDisableExpand, this.TypesToDisableExpand);
    IDBAttribute attributeByGuid2 = scheme.GetAttributeByGuid(new Guid("cad00620-306c-11d8-b4e9-00304f19f545"));
    if (this.ViewColumns.Count == 0)
    {
      attributeByGuid2.ClearValues();
    }
    else
    {
      List<string> stringList = new List<string>();
      foreach (ColumnSchemeAttProxy viewColumn in this.ViewColumns)
        stringList.Add(viewColumn.Value);
      attributeByGuid2.Values = (object[]) stringList.ToArray();
    }
    if (!this.IsPersonal)
    {
      IDBAttribute attributeByGuid3 = scheme.GetAttributeByGuid(new Guid("cad00d18-306c-11d8-b4e9-00304f19f545"));
      if (this.Roles.Count == 0)
      {
        attributeByGuid3.ClearValues();
      }
      else
      {
        List<string> stringList = new List<string>();
        foreach (SearchSchemeRole role in this.Roles)
          stringList.Add(role.RoleGuid.ToString());
        attributeByGuid3.Values = (object[]) stringList.ToArray();
      }
    }
    scheme.GetAttributeByGuid(new Guid(SearchConsts.attributeSearchOptions)).Value = (object) (int) this.Options;
    IDBAttribute dbAttribute = scheme.GetAttributeByGuid(new Guid(SearchConsts.attributeVersionRule));
    if (this.VersionRule == Guid.Empty)
    {
      if (dbAttribute == null || dbAttribute.Value == DBNull.Value)
        return;
      dbAttribute.Clear();
    }
    else
    {
      if (dbAttribute == null)
        dbAttribute = scheme.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(SearchConsts.attributeVersionRule), false);
      dbAttribute.Value = (object) this.VersionRule;
    }
  }

  private void ReadAttributeToCollection(
    IUserSession session,
    IDBObject scheme,
    Guid attributeGuid,
    List<GlobalType> collection)
  {
    collection.Clear();
    IDBAttribute attributeByGuid = scheme.GetAttributeByGuid(attributeGuid);
    if (attributeByGuid == null || attributeByGuid.ValuesCount <= 0)
      return;
    foreach (object obj in attributeByGuid.Values)
    {
      if (obj != null && obj != DBNull.Value)
        collection.Add(new GlobalType(obj.ToString(), 4, session));
    }
  }

  private void SaveCollectionToAttribute(
    IDBObject scheme,
    Guid attributeGuid,
    List<GlobalType> collection)
  {
    IDBAttribute dbAttribute = scheme.GetAttributeByGuid(attributeGuid);
    if (collection.Count == 0)
    {
      dbAttribute?.ClearValues();
    }
    else
    {
      if (dbAttribute == null)
        dbAttribute = scheme.Attributes.AddAttribute(MetaDataHelper.GetAttributeTypeID(attributeGuid), false);
      dbAttribute.Values = (object[]) collection.ConvertAll<string>((Converter<GlobalType, string>) (item => item.TypeGuid.ToString())).ToArray();
    }
  }
}
