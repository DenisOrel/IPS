// Decompiled with JetBrains decompiler
// Type: Intermech.Serialization.ClassicFormatters.FixupHolder
// Assembly: Intermech.Serialization.Compatibility, Version=1.0.1.74, Culture=neutral, PublicKeyToken=null
// MVID: D3658D7B-7F63-413B-8D5F-ACD5662A960C
// Assembly location: D:\Projects\IPS Code\AltiumDesigner\IPSAddIn\Intermech.Serialization.Compatibility.dll

#nullable disable
namespace Intermech.Serialization.ClassicFormatters;

internal sealed class FixupHolder
{
  internal const int ArrayFixup = 1;
  internal const int MemberFixup = 2;
  internal const int DelayedFixup = 4;
  internal long _id;
  internal object _fixupInfo;
  internal readonly int _fixupType;

  internal FixupHolder(long id, object fixupInfo, int fixupType)
  {
    this._id = id;
    this._fixupInfo = fixupInfo;
    this._fixupType = fixupType;
  }
}
