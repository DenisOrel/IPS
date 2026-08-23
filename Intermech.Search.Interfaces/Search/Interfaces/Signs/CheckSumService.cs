// Decompiled with JetBrains decompiler
// Type: Intermech.Search.Interfaces.Signs.CheckSumService
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Checksums;
using Intermech.Interfaces;
using Intermech.Signs.Interfaces;
using System;
using System.IO;
using System.Threading;

#nullable disable
namespace Intermech.Search.Interfaces.Signs;

/// <summary>
/// Вспомогательный класс для получения контрольной суммы потока
/// </summary>
public class CheckSumService
{
  /// <summary>Разрешено обновлять контрольную сумму в файлах</summary>
  /// <returns></returns>
  public bool CanSetChecksum() => SignsHolder.SignOutputEnabled;

  public string GetChecksum(
    IUserSession session,
    long elementId,
    AttributableElements kind,
    int attributeId,
    int index)
  {
    string checksum1 = "";
    if (!SignsHolder.SignOutputEnabled || !(session.GetCustomService(typeof (IChecksumsService)) is IChecksumsService customService))
      return checksum1;
    Guid taskGuid = customService.CalcChecksum(session.SessionGUID, elementId, kind, attributeId, index, SignsHolder.CheckSumType);
    while (true)
    {
      ChecksumTaskProgress checksumTaskProgress = customService.GetChecksumTaskProgress(taskGuid);
      if (checksumTaskProgress == null || checksumTaskProgress.Operation != ChecksumOperationType.Finished)
      {
        if (checksumTaskProgress != null && checksumTaskProgress.Operation != ChecksumOperationType.Error)
          Thread.Sleep(100);
        else
          goto label_5;
      }
      else
        break;
    }
    ChecksumClass checksum2 = customService.GetChecksum(taskGuid);
    customService.ChecksumFree(taskGuid);
    return checksum2.ToString();
label_5:
    customService.ChecksumFree(taskGuid);
    return "Ошибка вычисления";
  }

  /// <summary>Получить контрольную сумму</summary>
  /// <param name="stream">Поток сумму которого получаем</param>
  /// <returns>Контрольная сумма</returns>
  public string GetChecksum(Stream stream)
  {
    string checksum1 = "";
    if (SignsHolder.SignOutputEnabled)
    {
      Checksum checksum2 = (Checksum) null;
      switch (SignsHolder.CheckSumType)
      {
        case ChecksumAlgorithm.Crc32:
          checksum2 = (Checksum) new Crc32Checksum();
          break;
        case ChecksumAlgorithm.Md5:
          checksum2 = (Checksum) new Md5Checksum();
          break;
        case ChecksumAlgorithm.Gost3411_2012_256:
        case ChecksumAlgorithm.Gost3411_2012_512:
          checksum2 = (Checksum) new GostChecksum(SignsHolder.CheckSumType);
          break;
      }
      ChecksumClass checksumClass = checksum2.Compute(stream);
      if (checksumClass != null)
        checksum1 = checksumClass.ToString();
    }
    return checksum1;
  }
}
