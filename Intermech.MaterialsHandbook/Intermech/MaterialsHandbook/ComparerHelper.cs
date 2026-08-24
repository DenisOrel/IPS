// Decompiled with JetBrains decompiler
// Type: Intermech.MaterialsHandbook.ComparerHelper
// Assembly: Intermech.MaterialsHandbook, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: E4BE2DED-AF23-4AD7-9825-D1A5A54C126C
// Assembly location: D:\IPS\Client\Intermech.MaterialsHandbook.dll

#nullable disable
namespace Intermech.MaterialsHandbook;

internal class ComparerHelper
{
  internal static int Compare(ITreeListNode x, ITreeListNode y)
  {
    int num;
    switch (x.NodeType)
    {
      case FieldTypes.ftInteger:
      case FieldTypes.ftDouble:
      case FieldTypes.ftMeasured:
        num = NumericComparer.Compare(x.Value, y.Value);
        break;
      case FieldTypes.ftDateTime:
        num = DateTimeComparer.Compare(x.Value, y.Value);
        break;
      default:
        num = StringComparer.Compare(x.Value, y.Value);
        break;
    }
    return num;
  }
}
