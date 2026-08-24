// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.IMaterials
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>Материалы</summary>
public interface IMaterials
{
  MaterialInfo GetMaterial(string materialName, int createType);

  MaterialInfo GetMaterial(string materialName);

  /// <summary>Используется, когда надо добавить в кэш материал</summary>
  /// <param name="materialName"></param>
  /// <param name="imbaseKey"></param>
  void AddToCache(string materialName, string imbaseKey, int typeID, long objectID);
}
