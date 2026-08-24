// Decompiled with JetBrains decompiler
// Type: OxyPlot.JpegDecoder
// Assembly: Intermech.Statistics, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 407EEBC5-347E-45B1-B946-E45BC6430606
// Assembly location: D:\IPS\Client\Intermech.Statistics.dll

using System;
using System.IO;

#nullable disable
namespace OxyPlot;

public class JpegDecoder : IImageDecoder
{
  public OxyImageInfo GetImageInfo(byte[] bytes)
  {
    MemoryStream memoryStream = new MemoryStream(bytes);
    BinaryReader binaryReader = new BinaryReader((Stream) memoryStream);
    byte[] numArray1 = binaryReader.ReadBytes(2);
    if (numArray1[0] != byte.MaxValue || numArray1[1] != (byte) 216)
      throw new FormatException("Invalid SOI");
    byte[] numArray2 = binaryReader.ReadBytes(2);
    if (numArray2[0] != byte.MaxValue || numArray2[1] != (byte) 224 /*0xE0*/)
      throw new FormatException("Invalid APP0 marker");
    int num1 = (int) binaryReader.ReadUInt16();
    binaryReader.ReadString(4);
    int num2 = (int) binaryReader.ReadByte();
    binaryReader.ReadBytes(2);
    int num3 = (int) binaryReader.ReadByte();
    int num4 = (int) binaryReader.ReadUInt16();
    int num5 = (int) binaryReader.ReadUInt16();
    byte num6 = binaryReader.ReadByte();
    byte num7 = binaryReader.ReadByte();
    binaryReader.ReadBytes(3 * (int) num6 * (int) num7);
    while (binaryReader.ReadByte() == byte.MaxValue)
    {
      byte num8 = binaryReader.ReadByte();
      ushort num9 = binaryReader.ReadUInt16();
      if (num8 == (byte) 225)
      {
        if (binaryReader.ReadString(4) != "Exif")
          throw new FormatException("Invalid Exif identifier");
        binaryReader.ReadBytes(2);
        long position1 = memoryStream.Position;
        byte[] numArray3 = binaryReader.ReadBytes(2);
        bool isLittleEndian = numArray3[0] == (byte) 73 && numArray3[1] == (byte) 73;
        byte[] numArray4 = binaryReader.ReadBytes(2);
        if (numArray4[0] != (byte) 0 || numArray4[1] != (byte) 42)
          throw new FormatException("Invalid TIFF identifier");
        uint num10 = 0;
        uint num11 = binaryReader.ReadUInt32(isLittleEndian);
        memoryStream.Seek((long) (num11 - 8U), SeekOrigin.Current);
        ushort num12 = binaryReader.ReadUInt16(isLittleEndian);
        for (int index = 0; index < (int) num12; ++index)
        {
          JpegDecoder.ExifTags exifTags = (JpegDecoder.ExifTags) binaryReader.ReadUInt16(isLittleEndian);
          ushort fieldType = binaryReader.ReadUInt16(isLittleEndian);
          int count = (int) binaryReader.ReadUInt32(isLittleEndian);
          long position2 = memoryStream.Position;
          object obj = JpegDecoder.ReadValue(binaryReader, memoryStream, isLittleEndian, fieldType, count, position1);
          if (exifTags == (JpegDecoder.ExifTags) 34665)
            num10 = (uint) obj;
          memoryStream.Position = position2 + 4L;
        }
        memoryStream.Position = position1 + (long) num10;
        ushort num13 = binaryReader.ReadUInt16(isLittleEndian);
        for (int index = 0; index < (int) num13; ++index)
        {
          JpegDecoder.ExifTags exifTags = (JpegDecoder.ExifTags) binaryReader.ReadUInt16(isLittleEndian);
          ushort fieldType = binaryReader.ReadUInt16(isLittleEndian);
          int count = (int) binaryReader.ReadUInt32(isLittleEndian);
          long position3 = memoryStream.Position;
          object obj = JpegDecoder.ReadValue(binaryReader, memoryStream, isLittleEndian, fieldType, count, position1);
          if (exifTags == (JpegDecoder.ExifTags) 34665)
          {
            uint num14 = (uint) obj;
          }
          memoryStream.Position = position3 + 4L;
        }
      }
      else
        memoryStream.Seek((long) ((int) num9 - 2), SeekOrigin.Current);
    }
    throw new FormatException("Invalid marker");
  }

  public OxyColor[,] Decode(byte[] bytes) => throw new NotImplementedException();

  private static object ReadValue(
    BinaryReader inputReader,
    MemoryStream ms,
    bool isLittleEndian,
    ushort fieldType,
    int count,
    long baseOffset)
  {
    switch (fieldType)
    {
      case 1:
        uint num1 = inputReader.ReadUInt32(isLittleEndian);
        if (count == 1)
        {
          int num2 = (int) inputReader.ReadByte();
          inputReader.ReadBytes(3);
          return (object) (byte) num2;
        }
        if (count < 4)
        {
          byte[] numArray = inputReader.ReadBytes(count);
          if (count >= 4)
            return (object) numArray;
          inputReader.ReadBytes(4 - count);
          return (object) numArray;
        }
        ms.Position = baseOffset + (long) num1;
        return (object) inputReader.ReadBytes(count);
      case 2:
        if (count <= 4)
        {
          string str = inputReader.ReadString(count).Trim(new char[1]);
          if (count >= 4)
            return (object) str;
          inputReader.ReadBytes(4 - count);
          return (object) str;
        }
        uint num3 = inputReader.ReadUInt32(isLittleEndian);
        ms.Position = baseOffset + (long) num3;
        return (object) inputReader.ReadString(count).Trim(new char[1]);
      case 3:
        if (count == 1)
        {
          int num4 = (int) inputReader.ReadUInt16(isLittleEndian);
          int num5 = (int) inputReader.ReadUInt16(isLittleEndian);
          return (object) (ushort) num4;
        }
        if (count == 2)
          return (object) inputReader.ReadUInt16Array(count, isLittleEndian);
        uint num6 = inputReader.ReadUInt32(isLittleEndian);
        ms.Position = baseOffset + (long) num6;
        return (object) inputReader.ReadUInt16Array(count, isLittleEndian);
      case 4:
        if (count == 1)
          return (object) inputReader.ReadUInt32(isLittleEndian);
        uint num7 = inputReader.ReadUInt32(isLittleEndian);
        ms.Position = baseOffset + (long) num7;
        return (object) inputReader.ReadUInt32Array(count, isLittleEndian);
      case 5:
        uint num8 = inputReader.ReadUInt32(isLittleEndian);
        ms.Position = baseOffset + (long) num8;
        if (count == 1)
          return (object) ((double) inputReader.ReadUInt32(isLittleEndian) / (double) inputReader.ReadUInt32(isLittleEndian));
        throw new NotImplementedException();
      case 10:
        int num9 = inputReader.ReadInt32(isLittleEndian);
        ms.Position = baseOffset + (long) num9;
        if (count == 1)
          return (object) ((double) inputReader.ReadInt32(isLittleEndian) / (double) inputReader.ReadInt32(isLittleEndian));
        throw new NotImplementedException();
      default:
        throw new NotImplementedException();
    }
  }

  public OxyColor[,] Decode(Stream s) => throw new NotImplementedException();

  public enum ExifTags
  {
    GPSVersionID = 0,
    GPSLatitudeRef = 1,
    GPSLatitude = 2,
    GPSLongitudeRef = 3,
    GPSLongitude = 4,
    GPSAltitudeRef = 5,
    GPSAltitude = 6,
    GPSTimestamp = 7,
    GPSSatellites = 8,
    GPSStatus = 9,
    GPSMeasureMode = 10, // 0x0000000A
    GPSDOP = 11, // 0x0000000B
    GPSSpeedRef = 12, // 0x0000000C
    GPSSpeed = 13, // 0x0000000D
    GPSTrackRef = 14, // 0x0000000E
    GPSTrack = 15, // 0x0000000F
    GPSImgDirectionRef = 16, // 0x00000010
    GPSImgDirection = 17, // 0x00000011
    GPSMapDatum = 18, // 0x00000012
    GPSDestLatitudeRef = 19, // 0x00000013
    GPSDestLatitude = 20, // 0x00000014
    GPSDestLongitudeRef = 21, // 0x00000015
    GPSDestLongitude = 22, // 0x00000016
    GPSDestBearingRef = 23, // 0x00000017
    GPSDestBearing = 24, // 0x00000018
    GPSDestDistanceRef = 25, // 0x00000019
    GPSDestDistance = 26, // 0x0000001A
    GPSProcessingMethod = 27, // 0x0000001B
    GPSAreaInformation = 28, // 0x0000001C
    GPSDateStamp = 29, // 0x0000001D
    GPSDifferential = 30, // 0x0000001E
    ImageWidth = 256, // 0x00000100
    ImageLength = 257, // 0x00000101
    BitsPerSample = 258, // 0x00000102
    Compression = 259, // 0x00000103
    PhotometricInterpretation = 262, // 0x00000106
    ImageDescription = 270, // 0x0000010E
    Make = 271, // 0x0000010F
    Model = 272, // 0x00000110
    StripOffsets = 273, // 0x00000111
    Orientation = 274, // 0x00000112
    SamplesPerPixel = 277, // 0x00000115
    RowsPerStrip = 278, // 0x00000116
    StripByteCounts = 279, // 0x00000117
    XResolution = 282, // 0x0000011A
    YResolution = 283, // 0x0000011B
    PlanarConfiguration = 284, // 0x0000011C
    ResolutionUnit = 296, // 0x00000128
    TransferFunction = 301, // 0x0000012D
    Software = 305, // 0x00000131
    DateTime = 306, // 0x00000132
    Artist = 315, // 0x0000013B
    WhitePoint = 318, // 0x0000013E
    PrimaryChromaticities = 319, // 0x0000013F
    JPEGInterchangeFormat = 513, // 0x00000201
    JPEGInterchangeFormatLength = 514, // 0x00000202
    YCbCrCoefficients = 529, // 0x00000211
    YCbCrSubSampling = 530, // 0x00000212
    YCbCrPositioning = 531, // 0x00000213
    ReferenceBlackWhite = 532, // 0x00000214
    Copyright = 33432, // 0x00008298
    ExposureTime = 33434, // 0x0000829A
    FNumber = 33437, // 0x0000829D
    ExposureProgram = 34850, // 0x00008822
    SpectralSensitivity = 34852, // 0x00008824
    ISOSpeedRatings = 34855, // 0x00008827
    OECF = 34856, // 0x00008828
    ExifVersion = 36864, // 0x00009000
    DateTimeOriginal = 36867, // 0x00009003
    DateTimeDigitized = 36868, // 0x00009004
    ComponentsConfiguration = 37121, // 0x00009101
    CompressedBitsPerPixel = 37122, // 0x00009102
    ShutterSpeedValue = 37377, // 0x00009201
    ApertureValue = 37378, // 0x00009202
    BrightnessValue = 37379, // 0x00009203
    ExposureBiasValue = 37380, // 0x00009204
    MaxApertureValue = 37381, // 0x00009205
    SubjectDistance = 37382, // 0x00009206
    MeteringMode = 37383, // 0x00009207
    LightSource = 37384, // 0x00009208
    Flash = 37385, // 0x00009209
    FocalLength = 37386, // 0x0000920A
    SubjectArea = 37396, // 0x00009214
    MakerNote = 37500, // 0x0000927C
    UserComment = 37510, // 0x00009286
    SubsecTime = 37520, // 0x00009290
    SubsecTimeOriginal = 37521, // 0x00009291
    SubsecTimeDigitized = 37522, // 0x00009292
    FlashpixVersion = 40960, // 0x0000A000
    ColorSpace = 40961, // 0x0000A001
    PixelXDimension = 40962, // 0x0000A002
    PixelYDimension = 40963, // 0x0000A003
    RelatedSoundFile = 40964, // 0x0000A004
    FlashEnergy = 41483, // 0x0000A20B
    SpatialFrequencyResponse = 41484, // 0x0000A20C
    FocalPlaneXResolution = 41486, // 0x0000A20E
    FocalPlaneYResolution = 41487, // 0x0000A20F
    FocalPlaneResolutionUnit = 41488, // 0x0000A210
    SubjectLocation = 41492, // 0x0000A214
    ExposureIndex = 41493, // 0x0000A215
    SensingMethod = 41495, // 0x0000A217
    FileSource = 41728, // 0x0000A300
    SceneType = 41729, // 0x0000A301
    CFAPattern = 41730, // 0x0000A302
    CustomRendered = 41985, // 0x0000A401
    ExposureMode = 41986, // 0x0000A402
    WhiteBalance = 41987, // 0x0000A403
    DigitalZoomRatio = 41988, // 0x0000A404
    FocalLengthIn35mmFilm = 41989, // 0x0000A405
    SceneCaptureType = 41990, // 0x0000A406
    GainControl = 41991, // 0x0000A407
    Contrast = 41992, // 0x0000A408
    Saturation = 41993, // 0x0000A409
    Sharpness = 41994, // 0x0000A40A
    DeviceSettingDescription = 41995, // 0x0000A40B
    SubjectDistanceRange = 41996, // 0x0000A40C
    ImageUniqueID = 42016, // 0x0000A420
  }
}
