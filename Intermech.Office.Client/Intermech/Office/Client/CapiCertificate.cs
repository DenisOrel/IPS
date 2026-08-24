// Decompiled with JetBrains decompiler
// Type: Intermech.Office.Client.CapiCertificate
// Assembly: Intermech.Office.Client, Version=7.0.2.1112, Culture=neutral, PublicKeyToken=null
// MVID: 87EC380F-A344-4B99-B3CC-05ECB303FAD4
// Assembly location: D:\IPS\Client\Intermech.Office.Client.dll

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Intermech.Office.Client;

public class CapiCertificate : IDisposable
{
  private IntPtr pSignerCert = IntPtr.Zero;
  private bool lastResult;
  private string algKey = string.Empty;
  private IntPtr[] messages = new IntPtr[1];

  public int Error => Marshal.GetLastWin32Error();

  public CapiCertificate(X509Certificate2 cert)
  {
    if (cert != null)
      this.pSignerCert = cert.Handle;
    this.algKey = "1.3.6.1.4.1.12656.1.42";
  }

  public byte[] SignObjectHash(byte[] objectHash)
  {
    if (!(this.pSignerCert != IntPtr.Zero))
      throw new KernelException();
    CRYPT_SIGN_MESSAGE_PARA pSignPara = new CRYPT_SIGN_MESSAGE_PARA();
    pSignPara.cbSize = Marshal.SizeOf<CRYPT_SIGN_MESSAGE_PARA>(pSignPara);
    pSignPara.dwMsgEncodingType = 65537 /*0x010001*/;
    pSignPara.pSigningCert = this.pSignerCert;
    pSignPara.HashAlgorithm.pszObjId = this.algKey;
    pSignPara.HashAlgorithm.Parameters.pbData = IntPtr.Zero;
    pSignPara.HashAlgorithm.Parameters.cbData = 0;
    pSignPara.pvHashAuxInfo = IntPtr.Zero;
    pSignPara.cMsgCert = 1;
    GCHandle gcHandle = GCHandle.Alloc((object) this.pSignerCert, GCHandleType.Pinned);
    pSignPara.rgpMsgCert = gcHandle.AddrOfPinnedObject();
    gcHandle.Free();
    pSignPara.cMsgCrl = 0;
    pSignPara.rgpMsgCrl = IntPtr.Zero;
    pSignPara.cAuthAttr = 0;
    pSignPara.rgAuthAttr = IntPtr.Zero;
    pSignPara.cUnauthAttr = 0;
    pSignPara.rgUnauthAttr = IntPtr.Zero;
    pSignPara.dwFlags = 0;
    pSignPara.dwInnerContentType = 0;
    int length = objectHash.Length;
    this.messages[0] = Marshal.AllocHGlobal(length);
    Marshal.Copy(objectHash, 0, this.messages[0], length);
    int[] rgcbToBeSigned = new int[1]{ length };
    int pcbSignedBlob = 0;
    this.lastResult = Win32.CryptSignMessage(ref pSignPara, true, 1, this.messages, rgcbToBeSigned, (byte[]) null, ref pcbSignedBlob);
    if (!this.lastResult)
      throw new Win32Exception(Marshal.GetLastWin32Error());
    byte[] pbSignedBlob = new byte[pcbSignedBlob];
    this.lastResult = Win32.CryptSignMessage(ref pSignPara, true, 1, this.messages, rgcbToBeSigned, pbSignedBlob, ref pcbSignedBlob);
    if (!this.lastResult)
      throw new Win32Exception(this.Error);
    return pbSignedBlob;
  }

  public void Dispose()
  {
    if (!(this.messages[0] != IntPtr.Zero))
      return;
    Marshal.FreeHGlobal(this.messages[0]);
  }
}
