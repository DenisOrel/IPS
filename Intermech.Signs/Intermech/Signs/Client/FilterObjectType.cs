// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Client.FilterObjectType
// Assembly: Intermech.Signs, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: A3C02709-D794-49CE-8C55-5624449406B7
// Assembly location: D:\IPS\IPS.Installer.Full\IPS.InstClient\Client\Intermech.Signs.dll

using Intermech.Interfaces;
using Intermech.PropertyEditors;
using Intermech.Signs.Interfaces;
using System;

#nullable disable
namespace Intermech.Signs.Client;

public class FilterObjectType : ISelectorFilter
{
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
