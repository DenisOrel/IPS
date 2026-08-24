// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.CommonData.ItemsToCreate.IObjectTypeToCreateList
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

using System;

#nullable disable
namespace Intermech.ImpExp.Interface.CommonData.ItemsToCreate;

public interface IObjectTypeToCreateList : IItemToCreateList<IObjectTypeToCreate>
{
  IObjectTypeToCreate AddItem(bool isNew, string name, string shortName, Guid guid, long sysID);

  IObjectTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string instanceName,
    Guid guid,
    long sysID);

  IObjectTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string instanceName,
    Guid guid,
    long sysID,
    byte[] icon);

  IObjectTypeToCreate AddItem(
    bool isNew,
    string name,
    string shortName,
    string instanceName,
    Guid guid,
    long sysID,
    byte[] icon,
    ObjectVersionModes versionable);

  bool ExistsByShortName(string shortName);

  IObjectTypeToCreate GetByShortName(string shortName);

  void UpdateCasheShortName(string shortName, IObjectTypeToCreate item);
}
