// Decompiled with JetBrains decompiler
// Type: Intermech.Pdm.SearchScheme.GlobalType
// Assembly: Intermech.Pdm, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: D833CF8C-C82D-4973-9ACD-BA96843B4864
// Assembly location: D:\IPS\Client\Intermech.Pdm.dll

using Intermech.Client.Core;
using Intermech.Interfaces;
using System;
using System.Drawing;

#nullable disable
namespace Intermech.Pdm.SearchScheme;

internal sealed class GlobalType
{
  public Guid TypeGuid = Guid.Empty;
  public string TypeName = string.Empty;
  public int TypeID = -1;
  public Icon TypeIcon;

  public GlobalType(string guid, int category, IUserSession session)
  {
    this.TypeGuid = new Guid(guid);
    switch (category)
    {
      case 4:
        IDBObjectType objectType = session.GetObjectType(this.TypeGuid, false);
        if (objectType == null)
          break;
        this.TypeName = objectType.ObjectTypeName;
        this.TypeID = objectType.ObjectType;
        break;
      case 6:
        IDBRelationType relationType = session.GetRelationType(this.TypeGuid, false);
        if (relationType == null)
          break;
        this.TypeName = relationType.Description;
        this.TypeID = relationType.RelationType;
        break;
    }
  }

  public GlobalType(string guid, int category, IUserSession session, bool getIcon)
    : this(guid, category, session)
  {
    if (!getIcon || !(this.TypeGuid != Guid.Empty))
      return;
    this.TypeIcon = this.SetIcon(category);
  }

  public GlobalType(int id, int category, IUserSession session, bool getIcon)
    : this(id, category, session)
  {
    if (!getIcon || !(this.TypeGuid != Guid.Empty))
      return;
    this.TypeIcon = this.SetIcon(category);
  }

  private Icon SetIcon(int category)
  {
    if (category == 4)
      return UIHelper.GetObjTypeIcon(this.TypeGuid);
    return category == 6 ? (this.TypeIcon = UIHelper.GetRelTypeIcon(this.TypeGuid)) : (Icon) null;
  }

  public GlobalType(int id, int category, IUserSession session)
  {
    this.TypeID = id;
    switch (category)
    {
      case 4:
        IDBObjectType objectType = session.GetObjectType(this.TypeID, false);
        if (objectType == null)
          break;
        this.TypeName = objectType.ObjectTypeName;
        this.TypeGuid = (objectType as IDBGuid).GUID;
        break;
      case 6:
        IDBRelationType relationType = session.GetRelationType(this.TypeID, false);
        if (relationType == null)
          break;
        this.TypeName = relationType.Description;
        this.TypeGuid = (relationType as IDBGuid).GUID;
        break;
    }
  }

  public override string ToString()
  {
    return this.TypeID == -1 || this.TypeName == string.Empty ? $"{{{this.TypeGuid}}}" : this.TypeName;
  }
}
