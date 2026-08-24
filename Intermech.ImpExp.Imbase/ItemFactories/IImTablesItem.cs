// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Imbase.ItemFactories.IImTablesItem
// Assembly: Intermech.ImpExp.Imbase, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 14B82A62-153A-4D0C-8A5E-F24874681A1E
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Imbase.dll

using Intermech.ImpExp.Interface;
using Intermech.ImpExp.Interface.CommonData.SettingsItems;
using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.ImpExp.Imbase.ItemFactories;

internal interface IImTablesItem : ISettingsGroupItem
{
  int Key { get; }

  string TableName { get; }

  ImTablesType TableType { get; }

  ImFileAtt State { get; }

  string Description { get; }

  DateTime Created { get; }

  DateTime Modified { get; }

  string User { get; }

  int Openmode { get; }

  int Order { get; }

  int Nextkey { get; }

  int TextID { get; }

  int GraphID { get; }

  int Access { get; }

  Guid RecordsTypeGuid { get; set; }

  IList<ITableFieldInfo> ExistingFields { get; }

  bool FieldExistInBase(string fieldName);

  ITableFieldInfo GetFieldInfo(string fieldName);

  long ObjectID { get; set; }

  IList<string> UsedInCatalogs { get; }
}
