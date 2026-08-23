// Decompiled with JetBrains decompiler
// Type: Intermech.Archives.Common.TypesAndUsers
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Intermech.Archives.Common;

/// <summary>
/// Структура, содержащая типы документов и ИД пользователей для авторазмещения в архиве
/// </summary>
[Serializable]
public class TypesAndUsers
{
  private readonly List<int> docTypeIDs;
  private readonly List<long> userIDs;

  public List<int> DocTypeIDs => this.docTypeIDs;

  public List<long> UserIDs => this.userIDs;

  public TypesAndUsers(List<int> docTypeIDs, List<long> userIDs)
  {
    this.docTypeIDs = docTypeIDs;
    this.userIDs = userIDs;
  }
}
