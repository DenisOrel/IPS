// Decompiled with JetBrains decompiler
// Type: Intermech.ImpExp.Interface.PumperType
// Assembly: Intermech.ImpExp.Interface, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 37E5557D-7CCE-4F6F-9D9E-D0629D76BFC1
// Assembly location: D:\IPS\Client\Intermech.ImpExp.Interface.dll
// XML documentation location: D:\IPS\Client\Intermech.ImpExp.Interface.xml

#nullable disable
namespace Intermech.ImpExp.Interface;

/// <summary>
/// Вид пампера: MetaData стартуют первыми, Standard - во время основной перекачки
/// </summary>
public enum PumperType
{
  MetaData,
  Standard,
}
