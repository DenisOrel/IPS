// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.CAPIBaseMethods
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Intermech.Signs.Interfaces;

[Obsolete]
public class CAPIBaseMethods
{
  /// <summary>
  /// Фунция создания дескриптора криптопровайдера
  /// с одновременной его инициализацией конкретным ключевым контейнером.
  /// </summary>
  /// <param name="phProv">Адрес в который функция копирует созданный дескриптор криптопровайдера</param>
  /// <param name="szContainer">Имя ключевого контейнера</param>
  /// <param name="szProvider">Имя криптопровайдера</param>
  /// <param name="dwProvType">Тип криптопровайдера</param>
  /// <param name="dwFlags">Флаги</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  public static extern bool CryptAcquireContext(
    out IntPtr phProv,
    string szContainer,
    string szProvider,
    int dwProvType,
    uint dwFlags);

  /// <summary>Функция освобождения дескриптора криптопровайдера</summary>
  /// <param name="hProv">Удаляемый дескриптор криптопровайдера</param>
  /// <param name="dwFlags">Флаги</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  public static extern bool CryptReleaseContext(IntPtr hProv, uint dwFlags);

  /// <summary>
  /// Функция предназначена для выработки значения электронной цифровой подписи по значению объекта хэширования.
  /// </summary>
  /// <param name="hHash">Дескриптор подписываемого объекта хэширования</param>
  /// <param name="dwKeySpec">Тип ключевой пары, секретный ключ которой используется для выработки значения электронной цифровой подписи</param>
  /// <param name="sDescription">Не используется</param>
  /// <param name="dwFlags">Флаги</param>
  /// <param name="pbSignature">Электронной цифровой подписи</param>
  /// <param name="pdwSigLen">Размер электронной цифровой подписи</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  public static extern bool CryptSignHash(
    IntPtr hHash,
    uint dwKeySpec,
    IntPtr sDescription,
    uint dwFlags,
    byte[] pbSignature,
    out uint pdwSigLen);

  /// <summary>
  /// Функция предназначена для проверки электронной цифровой подписи с использованием значения объекта хэширования.
  /// </summary>
  /// <param name="hHash">Дескриптор объекта хэширования</param>
  /// <param name="pbSignature">Проверяемое значение электронной цифровой подписи</param>
  /// <param name="dwSigLen">Размер проверяемого значения электронной цифровой подписи</param>
  /// <param name="hPubKey">Дескриптор открытого ключа, используемый для проверки значения электронной цифровой подписи</param>
  /// <param name="sDescription">Не используется</param>
  /// <param name="dwFlags">Флаги</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  public static extern bool CryptVerifySignature(
    IntPtr hHash,
    byte[] pbSignature,
    uint dwSigLen,
    IntPtr hPubKey,
    IntPtr sDescription,
    uint dwFlags);

  /// <summary>
  /// Функция предназначена для получения дескриптора одной из долговременных ключевых пар пользователя.
  /// </summary>
  /// <param name="hProv">Дескриптор криптопровайдера</param>
  /// <param name="dwKeySpec">Тип получаемой ключевой пары.</param>
  /// <param name="phUserKey">Адрес, по которому в случае упеха записывается дескриптор ключа</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  public static extern bool CryptGetUserKey(IntPtr hProv, uint dwKeySpec, out IntPtr phUserKey);

  /// <summary>Функция освобождения дескриптора ключа</summary>
  /// <param name="hKey">Дескриптор удаляемого ключа</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  public static extern bool CryptDestroyKey(IntPtr hKey);

  /// <summary>
  /// Функция предназначена для генерации криптографических ключей
  /// </summary>
  /// <param name="hProv">Дескриптор криптопровайдера</param>
  /// <param name="Algid">Идентификатор алгоритма, для которого необходимо сформировать ключ</param>
  /// <param name="dwFlags">Флаги</param>
  /// <param name="phKey">Адрес дескриптора сгенерированного ключа</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  public static extern bool CryptGenKey(IntPtr hProv, uint Algid, uint dwFlags, out IntPtr phKey);

  /// <summary>
  /// Функция используется для импорта
  /// криптографических ключей из ключевого блоба в криптопровайдер.
  /// </summary>
  /// <param name="hProv">Дескриптор криптопровайдера</param>
  /// <param name="pbData">Импортируемый ключевой блоб</param>
  /// <param name="dwDataLen">Длина импортируемого ключевого блоба</param>
  /// <param name="hPubKey">Дескриптор ключа, используемый для импорта ключевого блоба</param>
  /// <param name="dwFlags">Флаги</param>
  /// <param name="phKey"> Адрес дескриптора импортированного ключа</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  public static extern bool CryptImportKey(
    IntPtr hProv,
    byte[] pbData,
    uint dwDataLen,
    IntPtr hPubKey,
    uint dwFlags,
    out IntPtr phKey);

  /// <summary>
  /// Функция предназначена для экспорта криптографических ключей из криптопровайдера
  /// </summary>
  /// <param name="hKey">Дескриптор криптопровайдера</param>
  /// <param name="hExpKey">Дескриптор экспортируемого ключа</param>
  /// <param name="dwBlobType">Тип ключевого блоба</param>
  /// <param name="dwFlags">Флаги</param>
  /// <param name="pbData">Блоб, в котором будет храниться экспортируемый ключ</param>
  /// <param name="pdwDataLen">Длина блоба, в котором будет храниться экспортируемый ключ</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  public static extern bool CryptExportKey(
    IntPtr hKey,
    IntPtr hExpKey,
    uint dwBlobType,
    uint dwFlags,
    byte[] pbData,
    out uint pdwDataLen);

  /// <summary>
  /// Функция предназначена для создания и инициализации нового дескриптора объекта хэширования.
  /// </summary>
  /// <param name="hProv">Дескриптор криптопровайдера</param>
  /// <param name="Algid">Идентификатор алгоритма хэширования</param>
  /// <param name="hKey"> Дескриптор сессионного ключа шифрования (0)</param>
  /// <param name="dwFlags">Флаги</param>
  /// <param name="phHash">Адрес дескриптора объекта хэширования</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  public static extern bool CryptCreateHash(
    IntPtr hProv,
    uint Algid,
    IntPtr hKey,
    uint dwFlags,
    out IntPtr phHash);

  /// <summary>
  /// Функция предназначена для добавления данных в объект хэширования.
  /// </summary>
  /// <param name="hHash">Дескриптор объекта хэширования, в который производится добавление данных</param>
  /// <param name="pbData">Данные добавляемые в объект хэширования</param>
  /// <param name="dwDataLen">Размер переданных в параметре pbData данных</param>
  /// <param name="dwFlags">Флаги</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  public static extern bool CryptHashData(
    IntPtr hHash,
    byte[] pbData,
    uint dwDataLen,
    uint dwFlags);

  /// <summary>
  /// Функция используется для освобждения дескриптора объекта хэширования
  /// </summary>
  /// <param name="hHash">Указатель удаляемого объекта хэширования</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  public static extern bool CryptDestroyHash(IntPtr hHash);

  /// <summary>
  /// Функция для получения полного перечня строковых имен криптопровайдеров
  /// </summary>
  /// <param name="dwIndex">индекс провайдера для получения информации</param>
  /// <param name="dwReserved">null</param>
  /// <param name="dwFlags">Флаги</param>
  /// <param name="pdwProvType">тип провайдера</param>
  /// <param name="pszProvName">имя провайдера</param>
  /// <param name="pcbProvName">длина имени</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  public static extern bool CryptEnumProviders(
    int dwIndex,
    IntPtr dwReserved,
    int dwFlags,
    out IntPtr pdwProvType,
    string pszProvName,
    ref int pcbProvName);

  /// <summary>Описание криптопровайдера</summary>
  /// <param name="hProv">Дескриптор криптопровайдера </param>
  /// <param name="dwParam">Параметры запрашиваемой информации</param>
  /// <param name="pbData">Буфер с информацией о криптопровайдере</param>
  /// <param name="pdwDataLen">Длина буфера</param>
  /// <param name="dwFlags">Флаги</param>
  /// <returns></returns>
  [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  public static extern bool CryptGetProvParam(
    IntPtr hProv,
    int dwParam,
    byte[] pbData,
    ref int pdwDataLen,
    int dwFlags);

  /// <summary>вернуть код последней ошибки</summary>
  /// <returns></returns>
  [DllImport("Kernel32.DLL", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  public static extern uint GetLastError();
}
