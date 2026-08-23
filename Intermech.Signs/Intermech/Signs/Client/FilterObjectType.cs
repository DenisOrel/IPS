// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.FilterObjectType
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\Client\Intermech.Signs.dll
// XML documentation location: D:\IPS\Client\Intermech.Signs.xml

using Intermech.Interfaces;
using Intermech.PropertyEditors;
using Intermech.Signs.Interfaces;
using System;

#nullable disable
namespace Intermech.Signs.Client;

/// <summary>
/// для фильтрации  в диалоге выбора типов объектов
/// нужны только те типы объектов, которые можно подписывать
/// </summary>
public class FilterObjectType : ISelectorFilter
{
  /// <summary>проверка на попадание в фильтр.</summary>
  /// <param name="category">категория объекта</param>
  /// <param name="id">id типа обеъкта</param>
  /// <returns></returns>
  public bool IsInFilter(int category, object id)
  {
    if (category.Equals(4) && id != null && id.GetType().Equals(typeof (int)))
    {
      int int32 = Convert.ToInt32(id);
      if (MetaDataHelper.HasApplicability(int32, SignsHolder.SignObjectTypeID, SignsHolder.SignRelationTypeID))
        return true;
      foreach (int parObjTypeID in MetaDataHelper.GetObjectTypeChildrenIDRecursive(int32))
      {
        if (MetaDataHelper.HasApplicability(parObjTypeID, SignsHolder.SignObjectTypeID, SignsHolder.SignRelationTypeID))
          return true;
      }
    }
    return false;
  }
}
