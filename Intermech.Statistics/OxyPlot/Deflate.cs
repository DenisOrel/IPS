// Decompiled with JetBrains decompiler
// Type: OxyPlot.Deflate
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.IO;

#nullable disable
namespace OxyPlot;

public class Deflate : IDisposable
{
  private static readonly CodeTree FixedLiteralLengthCode;
  private static readonly CodeTree FixedDistanceCode;
  private readonly CircularDictionary dictionary;
  private readonly BitReader input;
  private readonly BinaryWriter output;
  private readonly MemoryStream outputStream;
  private bool disposed;

  static Deflate()
  {
    int[] numArray1 = new int[288];
    Arrays.Fill<int>(numArray1, 0, 144 /*0x90*/, 8);
    Arrays.Fill<int>(numArray1, 144 /*0x90*/, 256 /*0x0100*/, 9);
    Arrays.Fill<int>(numArray1, 256 /*0x0100*/, 280, 7);
    Arrays.Fill<int>(numArray1, 280, 288, 8);
    Deflate.FixedLiteralLengthCode = new CanonicalCode(numArray1).ToCodeTree();
    int[] numArray2 = new int[32 /*0x20*/];
    Arrays.Fill<int>(numArray2, 0, 32 /*0x20*/, 5);
    Deflate.FixedDistanceCode = new CanonicalCode(numArray2).ToCodeTree();
  }

  private Deflate(BitReader reader)
  {
    this.input = reader;
    this.outputStream = new MemoryStream();
    this.output = new BinaryWriter((Stream) this.outputStream);
    this.dictionary = new CircularDictionary(32768 /*0x8000*/);
    int num1;
    do
    {
      num1 = this.input.ReadNoEof() != -1 ? 1 : 0;
      int num2 = this.ReadInt(2);
      switch (num2)
      {
        case 0:
          this.DecompressUncompressedBlock();
          break;
        case 1:
        case 2:
          CodeTree literalLengthCode;
          CodeTree fixedDistanceCode;
          if (num2 == 1)
          {
            literalLengthCode = Deflate.FixedLiteralLengthCode;
            fixedDistanceCode = Deflate.FixedDistanceCode;
          }
          else
          {
            CodeTree[] codeTreeArray = this.DecodeHuffmanCodes();
            literalLengthCode = codeTreeArray[0];
            fixedDistanceCode = codeTreeArray[1];
          }
          this.DecompressHuffmanBlock(literalLengthCode, fixedDistanceCode);
          break;
        case 3:
          throw new FormatException("Invalid block type");
        default:
          throw new NotImplementedException();
      }
    }
    while (num1 == 0);
  }

  public static byte[] Decompress(Stream input)
  {
    return new Deflate((BitReader) new ByteBitReader(input)).outputStream.ToArray();
  }

  public static byte[] Decompress(BitReader input) => new Deflate(input).outputStream.ToArray();

  public static byte[] Decompress(byte[] input)
  {
    return Deflate.Decompress((Stream) new MemoryStream(input));
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  private CodeTree[] DecodeHuffmanCodes()
  {
    int num1 = this.ReadInt(5) + 257;
    int num2 = this.ReadInt(5) + 1;
    int num3 = this.ReadInt(4) + 4;
    int[] codeLengths1 = new int[19];
    codeLengths1[16 /*0x10*/] = this.ReadInt(3);
    codeLengths1[17] = this.ReadInt(3);
    codeLengths1[18] = this.ReadInt(3);
    codeLengths1[0] = this.ReadInt(3);
    for (int index = 0; index < num3 - 4; ++index)
    {
      if (index % 2 == 0)
        codeLengths1[8 + index / 2] = this.ReadInt(3);
      else
        codeLengths1[7 - index / 2] = this.ReadInt(3);
    }
    CodeTree codeTree1 = new CanonicalCode(codeLengths1).ToCodeTree();
    int[] source = new int[num1 + num2];
    int num4 = -1;
    int num5 = 0;
    for (int index = 0; index < source.Length; ++index)
    {
      if (num5 > 0)
      {
        source[index] = num4;
        --num5;
      }
      else
      {
        int num6 = this.DecodeSymbol(codeTree1);
        if (num6 < 16 /*0x10*/)
        {
          source[index] = num6;
          num4 = num6;
        }
        else
        {
          switch (num6)
          {
            case 16 /*0x10*/:
              if (num4 == -1)
                throw new FormatException("No code length value to copy");
              num5 = this.ReadInt(2) + 3;
              break;
            case 17:
              num4 = 0;
              num5 = this.ReadInt(3) + 3;
              break;
            case 18:
              num4 = 0;
              num5 = this.ReadInt(7) + 11;
              break;
            default:
              throw new Exception();
          }
          --index;
        }
      }
    }
    if (num5 > 0)
      throw new FormatException("Run exceeds number of codes");
    CodeTree codeTree2 = new CanonicalCode(Arrays.CopyOf<int>(source, num1)).ToCodeTree();
    int[] codeLengths2 = Arrays.CopyOfRange<int>(source, num1, source.Length);
    CodeTree codeTree3 = codeLengths2.Length != 1 || codeLengths2[0] != 0 ? new CanonicalCode(codeLengths2).ToCodeTree() : (CodeTree) null;
    return new CodeTree[2]{ codeTree2, codeTree3 };
  }

  private void DecompressUncompressedBlock()
  {
    while (this.input.GetBitPosition() != 0)
      this.input.Read();
    int num1 = this.ReadInt(16 /*0x10*/);
    int num2 = this.ReadInt(16 /*0x10*/);
    if ((num1 ^ (int) ushort.MaxValue) != num2)
      throw new FormatException("Invalid length in uncompressed block");
    for (int index = 0; index < num1; ++index)
    {
      int b = this.input.ReadByte();
      if (b == -1)
        throw new Exception("EOF");
      this.output.Write((byte) b);
      this.dictionary.Append(b);
    }
  }

  private void DecompressHuffmanBlock(CodeTree litLenCode, CodeTree distCode)
  {
    if (litLenCode == null)
      throw new InvalidOperationException();
    while (true)
    {
      int num = this.DecodeSymbol(litLenCode);
      if (num != 256 /*0x0100*/)
      {
        if (num < 256 /*0x0100*/)
        {
          this.output.Write((byte) num);
          this.dictionary.Append(num);
        }
        else
        {
          int len = this.DecodeRunLength(num);
          if (distCode != null)
            this.dictionary.Copy(this.DecodeDistance(this.DecodeSymbol(distCode)), len, this.output);
          else
            goto label_6;
        }
      }
      else
        break;
    }
    return;
label_6:
    throw new FormatException("Length symbol encountered with empty distance code");
  }

  private int DecodeSymbol(CodeTree code)
  {
    InternalNode internalNode = code.Root;
    Node node;
    while (true)
    {
      switch (this.input.ReadNoEof())
      {
        case 0:
          node = internalNode.LeftChild;
          break;
        case 1:
          node = internalNode.RightChild;
          break;
        default:
          goto label_4;
      }
      switch (node)
      {
        case Leaf _:
          goto label_6;
        case InternalNode _:
          internalNode = (InternalNode) node;
          continue;
        default:
          goto label_8;
      }
    }
label_4:
    throw new Exception();
label_6:
    return ((Leaf) node).Symbol;
label_8:
    throw new InvalidOperationException();
  }

  private int DecodeRunLength(int sym)
  {
    if (sym < 257 || sym > 285)
      throw new FormatException("Invalid run length symbol: " + (object) sym);
    if (sym <= 264)
      return sym - 254;
    if (sym > 284)
      return 258;
    int numBits = (sym - 261) / 4;
    return ((sym - 265) % 4 + 4 << numBits) + 3 + this.ReadInt(numBits);
  }

  private int DecodeDistance(int sym)
  {
    if (sym <= 3)
      return sym + 1;
    if (sym > 29)
      throw new FormatException("Invalid distance symbol: " + (object) sym);
    int numBits = sym / 2 - 1;
    return (sym % 2 + 2 << numBits) + 1 + this.ReadInt(numBits);
  }

  private void Dispose(bool disposing)
  {
    if (!this.disposed && disposing)
    {
      this.output.Dispose();
      this.outputStream.Dispose();
    }
    this.disposed = true;
  }

  private int ReadInt(int numBits)
  {
    if (numBits < 0 || numBits >= 32 /*0x20*/)
      throw new ArgumentException();
    int num = 0;
    for (int index = 0; index < numBits; ++index)
      num |= this.input.ReadNoEof() << index;
    return num;
  }
}
