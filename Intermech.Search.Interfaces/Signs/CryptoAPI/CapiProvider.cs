// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.CryptoAPI.CapiProvider
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

using Intermech.Signs.Interfaces;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

#nullable disable
namespace Intermech.Signs.CryptoAPI;

[Obsolete]
public class CapiProvider : IDisposable
{
  /// <summary>параметры</summary>
  private CspParameters csp;
  /// <summary>адреса дескрипторов провайдера</summary>
  private IntPtr hProvider = IntPtr.Zero;
  /// <summary>адрес дескриптора ключа</summary>
  private IntPtr hKey = IntPtr.Zero;
  /// <summary>адрес дескриптора хэша</summary>
  private IntPtr hHash = IntPtr.Zero;
  /// <summary>
  /// имя контйнера с ключами
  /// (если emty - значит режим CRYPT_SILENT,
  ///  импортируем открытый ключ)
  /// </summary>
  private string containerName = string.Empty;
  /// <summary>имя провайдера</summary>
  private string provName = string.Empty;
  /// <summary>has the last call succeded ?</summary>
  private bool lastResult;
  /// <summary>тип провайдера</summary>
  private int provType;

  /// <summary>Последняя ошибка, возникшая при выполнении</summary>
  public int Error => Marshal.GetLastWin32Error();

  /// <summary>false - возникла ошибка</summary>
  public bool Result => this.lastResult;

  /// <summary>Конструктор</summary>
  /// <param name="csp">Параметры криптопровайдера</param>
  public CapiProvider(CspParameters csp)
  {
    this.provType = csp.ProviderType;
    this.provName = csp.ProviderName;
    this.containerName = csp.KeyContainerName;
    this.csp = csp;
    this.lastResult = !string.IsNullOrEmpty(this.containerName) ? (csp.Flags != CspProviderFlags.UseExistingKey ? CAPIBaseMethods.CryptAcquireContext(out this.hProvider, this.containerName, this.provName, this.provType, CAPIConsts.CRYPT_NEWKEYSET) : CAPIBaseMethods.CryptAcquireContext(out this.hProvider, this.containerName, this.provName, this.provType, 0U)) : CAPIBaseMethods.CryptAcquireContext(out this.hProvider, (string) null, this.provName, this.provType, CAPIConsts.CRYPT_SILENT | CAPIConsts.CRYPT_VERIFYCONTEXT);
    if (!this.lastResult)
      throw new Win32Exception(this.Error);
  }

  /// <summary>
  /// Создать контейенер с ключами.
  /// Вернуть открытый ключ.
  /// </summary>
  /// <returns></returns>
  public byte[] GeneratePublicKey()
  {
    this.hKey = IntPtr.Zero;
    this.lastResult = CAPIBaseMethods.CryptGenKey(this.hProvider, CAPIConsts.AT_SIGNATURE, CAPIConsts.CRYPT_EXPORTABLE, out this.hKey);
    if (this.lastResult)
    {
      uint pdwDataLen = 0;
      this.lastResult = CAPIBaseMethods.CryptExportKey(this.hKey, IntPtr.Zero, CAPIConsts.PUBLICKEYBLOB, 0U, (byte[]) null, out pdwDataLen);
      if (this.lastResult)
      {
        byte[] pbData = new byte[(int) pdwDataLen];
        if (CAPIBaseMethods.CryptExportKey(this.hKey, IntPtr.Zero, CAPIConsts.PUBLICKEYBLOB, 0U, pbData, out pdwDataLen))
          return pbData;
      }
    }
    throw new Win32Exception(this.Error);
  }

  /// <summary>
  /// Создать хэш объекта и подписать его указанным алгоритмом.
  /// Создать хэш хэша... Масло маслянное.
  /// Первый хэш - это хэш на основе всех нужных атрибутов объекта.
  /// Вычисляется в методе IDBObject.GetHashFile()
  /// На основе этого хэша создаётся хэш с помощью выбранного криптопровайдера.
  /// Это уже в методе CryptHashData
  /// </summary>
  /// <param name="objectHash">подписываемый хэш </param>
  /// <param name="algID">алгоритм хэширования </param>
  /// <returns> подпись</returns>
  public byte[] SignObjectHash(byte[] objectHash, int algID)
  {
    this.hKey = IntPtr.Zero;
    this.lastResult = CAPIBaseMethods.CryptCreateHash(this.hProvider, (uint) algID, IntPtr.Zero, 0U, out this.hHash);
    if (this.lastResult)
    {
      this.lastResult = CAPIBaseMethods.CryptHashData(this.hHash, objectHash, (uint) objectHash.Length, 0U);
      if (this.lastResult)
      {
        uint pdwSigLen = 0;
        this.lastResult = CAPIBaseMethods.CryptSignHash(this.hHash, (uint) this.csp.KeyNumber, IntPtr.Zero, 0U, (byte[]) null, out pdwSigLen);
        if (this.lastResult)
        {
          byte[] pbSignature = new byte[(int) pdwSigLen];
          this.lastResult = CAPIBaseMethods.CryptSignHash(this.hHash, (uint) this.csp.KeyNumber, IntPtr.Zero, 0U, pbSignature, out pdwSigLen);
          if (this.lastResult)
            return pbSignature;
        }
      }
    }
    throw new Win32Exception(this.Error);
  }

  /// <summary>проверить подпись</summary>
  /// <param name="sign">проверяемое значение эцп объекта</param>
  /// <param name="objectHash">хэш объект (данные для сравнения)</param>
  /// <param name="openKey">открытый ключ</param>
  /// <param name="algID">алгоритм хэширования</param>
  /// <returns>true - подпись верна, false - подпись не верна</returns>
  public bool VerifyObjectSign(byte[] sign, byte[] objectHash, byte[] openKey, int algID)
  {
    this.hHash = IntPtr.Zero;
    this.hKey = IntPtr.Zero;
    this.lastResult = CAPIBaseMethods.CryptCreateHash(this.hProvider, (uint) algID, IntPtr.Zero, 0U, out this.hHash);
    if (this.lastResult)
    {
      this.lastResult = CAPIBaseMethods.CryptHashData(this.hHash, objectHash, (uint) objectHash.Length, 0U);
      if (this.lastResult)
      {
        this.lastResult = CAPIBaseMethods.CryptImportKey(this.hProvider, openKey, (uint) openKey.Length, IntPtr.Zero, 0U, out this.hKey);
        if (this.lastResult)
        {
          this.lastResult = CAPIBaseMethods.CryptVerifySignature(this.hHash, sign, (uint) sign.Length, this.hKey, IntPtr.Zero, 0U);
          if (this.lastResult)
            return true;
          if (CAPIBaseMethods.GetLastError() == 2148073478U /*0x80090006*/)
            return false;
        }
      }
    }
    throw new Win32Exception(this.Error);
  }

  /// <summary>release unmanaged resources</summary>
  public void Dispose()
  {
    if (!(this.hProvider != IntPtr.Zero))
      return;
    if (this.hKey != IntPtr.Zero)
      CAPIBaseMethods.CryptDestroyKey(this.hKey);
    if (this.hHash != IntPtr.Zero)
      CAPIBaseMethods.CryptDestroyHash(this.hHash);
    CAPIBaseMethods.CryptReleaseContext(this.hProvider, 0U);
    GC.KeepAlive((object) this);
    this.hProvider = IntPtr.Zero;
    this.hKey = IntPtr.Zero;
    this.hHash = IntPtr.Zero;
    GC.SuppressFinalize((object) this);
  }
}
