// Decompiled with JetBrains decompiler
// Type: Intermech.Signs.Interfaces.CAPIConsts
// Assembly: Intermech.Search.Interfaces, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 2A64B407-09E4-412B-843D-05286AFAF9EF
// Assembly location: D:\IPS\Client\Intermech.Search.Interfaces.dll
// XML documentation location: D:\IPS\Client\Intermech.Search.Interfaces.xml

#nullable disable
namespace Intermech.Signs.Interfaces;

/// <summary>константы</summary>
public static class CAPIConsts
{
  /// <summary>
  /// Дескриптор криптопровайдера создается в режиме проверки
  /// </summary>
  public static readonly uint CRYPT_VERIFYCONTEXT = 4026531840 /*0xF0000000*/;
  /// <summary>
  /// Дескриптор криптопровайдера создается в режиме создания нового ключевого контейнера
  /// </summary>
  public static readonly uint CRYPT_NEWKEYSET = 8;
  /// <summary>
  /// Дескриптор криптопровайдера создается в режиме удаления ключевого контейнера
  /// </summary>
  public static readonly uint CRYPT_DELETEKEYSET = 16 /*0x10*/;
  /// <summary>
  /// Флаг запрещающий отображение любых графических интерфейсов при выполнении операций криптопровайдером
  /// </summary>
  public static readonly uint CRYPT_SILENT = 64 /*0x40*/;
  /// <summary>Флаг определяющий принадлежность ключевого контейнера</summary>
  public static readonly uint CRYPT_MACHINE_KEYSET = 32 /*0x20*/;
  /// <summary>Идентификатор алгоритма для формирования пары подписи</summary>
  public static readonly uint AT_KEYEXCHANGE = 1;
  /// <summary>
  /// Идентификатор алгоритма для формирования пары ключевого обмена
  /// </summary>
  public static readonly uint AT_SIGNATURE = 2;
  /// <summary>
  /// Формируемый ключ может быть экспортирован из криптопровайдера в ключевой блоб
  /// </summary>
  public static readonly uint CRYPT_EXPORTABLE = 1;
  /// <summary>Импортируется открытый ключ</summary>
  public static readonly uint PUBLICKEYBLOB = 6;
  /// <summary>
  /// 
  /// </summary>
  public const int X509_ASN_ENCODING = 1;
  /// <summary>
  /// 
  /// </summary>
  public const int PKCS_7_ASN_ENCODING = 65536 /*0x010000*/;
  /// <summary>тип</summary>
  public const int MY_TYPE = 65537 /*0x010001*/;
  /// <summary>
  /// флаг запроса.
  /// вернуть структуру с информацией об алгоритме, который поддерживает криптопровайдер
  /// </summary>
  public const int PP_ENUMALGS_EX = 22;
  /// <summary>
  /// Retrieve the first element in the enumeration.
  /// This has the same affect as resetting the enumerator.
  /// </summary>
  public const int CRYPT_FIRST = 1;
  /// <summary>
  /// Retrieve the next element in the enumeration.
  /// When there are no more elements to retrieve,
  /// this function will fail and set the last error to ERROR_NO_MORE_ITEMS.
  /// </summary>
  public const int CRYPT_NEXT = 2;
  /// <summary>алгоритмы хэширования</summary>
  public const int ALG_CLASS_HASH = 32768 /*0x8000*/;
  /// <summary>алгоритмы получения пары ключей для ЭЦП</summary>
  public const int ALG_CLASS_SIGNATURE = 8192 /*0x2000*/;
  /// <summary>алгоритмы</summary>
  public const int ALG_CLASS_DATA_ENCRYPT = 24576 /*0x6000*/;
  /// <summary>алгоритмы полчения пары ключей для шифрования</summary>
  public const int ALG_CLASS_KEY_EXCHANGE = 40960 /*0xA000*/;
  /// <summary>
  /// Key container does not exist.
  /// You do not have access to the key container.
  /// The Protected Storage Service is not running.
  /// </summary>
  public const uint NTE_BAD_KEYSET = 2148073494;
  /// <summary>
  /// The key container already exists,
  /// but you are attempting to create it.
  /// If a previous attempt to open the key failed with NTE_BAD_KEYSET,
  /// it implies that access to the key container is denied.
  /// </summary>
  public const uint NTE_EXISTS = 2148073487 /*0x8009000F*/;
  /// <summary>
  /// he Crypto Service Provider (CSP) may not be set up correctly.
  /// Use of Regsvr32.exe on CSP DLLs (Rsabase.dll or Rsaenh.dll)
  /// may fix the problem, depending on the provider being used.
  /// </summary>
  public const uint NTE_KEYSET_NOT_DEF = 2148073497;
  /// <summary>В параметре dwFlags указано некорректное значение</summary>
  public const uint NTE_BAD_FLAGS = 2148073481 /*0x80090009*/;
  /// <summary>
  /// Для CryptVerifySignature -  проверяемая электронная цифровая подпись неверна.
  /// </summary>
  public const uint NTE_BAD_SIGNATURE = 2148073478 /*0x80090006*/;

  /// <summary>ALG_ID crackers</summary>
  /// <param name="x"></param>
  /// <returns></returns>
  public static int GET_ALG_CLASS(int x) => x & 57344 /*0xE000*/;
}
