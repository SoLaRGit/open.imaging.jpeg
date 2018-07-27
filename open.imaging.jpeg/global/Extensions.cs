/////////////////////////////////////////////////////////////////////////////// 
// 
// This file is part of open.imaging.jpeg project
//
// Copyright (c) 2017 Nikola Bozovic. All rights reserved. 
// 
// This code is licensed under the MIT License (MIT). 
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
// THE SOFTWARE. 
// 
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Xml.Serialization;
using System.Runtime;
using System.Runtime.InteropServices;

#if NET40
[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
public static class Extensions
{
  #region ### uint ###

  public static bool IsSet(this uint thisobj, uint flag)
  {
    return ((thisobj & flag) == flag);
  }

  public static bool IsNot(this uint thisobj, uint flag)
  {
    return ((thisobj & flag) != flag);
  }

  public static string chararray_as_str(this uint thisobj)
  {
    unchecked
    {
      return new string(new char[]
        {
        (char)((byte)(thisobj >> 24)), 
        (char)((byte)(thisobj >> 16)), 
        (char)((byte)(thisobj >> 8)), 
        (char)((byte)(thisobj >> 0))
        });
    }
  }

  public static string chararray_as_str(this ulong thisobj)
  {
    unchecked
    {
      return new string(new char[]
        {
        (char)((byte)(thisobj >> 56)), 
        (char)((byte)(thisobj >> 48)), 
        (char)((byte)(thisobj >> 40)), 
        (char)((byte)(thisobj >> 32)),
        (char)((byte)(thisobj >> 24)), 
        (char)((byte)(thisobj >> 16)), 
        (char)((byte)(thisobj >> 8)), 
        (char)((byte)(thisobj >> 0))
        });
    }
  }

  public static uint chararray_to_u4(this string thisobj)
  {
    unchecked
    {
      return (uint)(
        ((uint)((byte)thisobj[0]) << 24) |
        ((uint)((byte)thisobj[1]) << 16) |
        ((uint)((byte)thisobj[2]) << 8) |
        ((uint)((byte)thisobj[3])));
    }  
  }

  public static ulong chararray_to_u8(this string thisobj)
  {
    unchecked
    {
      return (ulong)(
        ((ulong)((byte)thisobj[0]) << 56) |
        ((ulong)((byte)thisobj[1]) << 48) |
        ((ulong)((byte)thisobj[2]) << 40) |
        ((ulong)((byte)thisobj[3]) << 32) |
        ((ulong)((byte)thisobj[0]) << 24) |
        ((ulong)((byte)thisobj[1]) << 16) |
        ((ulong)((byte)thisobj[2]) << 8) |
        ((ulong)((byte)thisobj[3])));
    }
  }

  #endregion

  #region ### byte[] ###

  public static string to_hex(this byte[] array, int maxBytes = -1)
  {
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    if (maxBytes == -1) maxBytes = array.Length;
    maxBytes = maxBytes < array.Length ? maxBytes : array.Length;
    for (int i = 0; i < maxBytes; i++)
    {
      sb.AppendFormat("{0:x2} ", array[i]);
    }
    if (maxBytes != array.Length) sb.Append("...");
    return sb.ToString();
  }

  public static string to_hex_fmt(this byte[] array, string format = "{0:x2}", int wrapindex = -1, int maxBytes = -1)
  {
    const string CRLF = "\r\n";
    if (array == null) return null;
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    if (wrapindex != -1) sb.Append(CRLF);
    if (maxBytes == -1) maxBytes = array.Length;
    maxBytes = maxBytes < array.Length ? maxBytes : array.Length;
    for (int i = 0; i < maxBytes; i++)
    {
      sb.AppendFormat(format, array[i]);
      if (wrapindex > -1 && ((i + 1) % wrapindex == 0)) sb.Append(CRLF);
    }
    if (maxBytes != array.Length) sb.Append("...");
    if (wrapindex != -1) sb.Append(CRLF + "    ");
    return sb.ToString();
  }


  public static string to_u2_fmt(this ushort[] array, string format = "{0}", int wrapindex = -1, int maxLength = -1)
  {
    const string CRLF = "\r\n";
    if (array == null) return null;
    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    if (wrapindex != -1) sb.Append(CRLF);
    if (maxLength == -1) maxLength = array.Length;
    maxLength = maxLength < array.Length ? maxLength : array.Length;
    for (int i = 0; i < maxLength; i++)
    {
      sb.AppendFormat(format, array[i]);
      if (wrapindex > -1 && ((i + 1) % wrapindex == 0)) sb.Append(CRLF);
    }
    if (maxLength != array.Length) sb.Append("...");
    if (wrapindex != -1) sb.Append(CRLF + "    ");
    return sb.ToString();
  }

  public static string to_str(this byte[] array)
  {    
    int nullindex = 0;
    // encoding detection: unicode
    if (array.Length > 2 && array[1] == 0x00)
    {
      nullindex = Array.IndexOf<byte>(array, 0x00);
      if (nullindex == -1) nullindex = array.Length;
      while (array.Length - 1 > nullindex + 1 && array[nullindex + 1] != 0x00)
      {
        nullindex = Array.IndexOf<byte>(array, 0x00, nullindex + 1);
      }
      return System.Text.UnicodeEncoding.Unicode.GetString(array, 0, (int)nullindex);
    }
    nullindex = Array.IndexOf<byte>(array, 0x00);
    if (nullindex == -1) nullindex = array.Length;
    return System.Text.UTF8Encoding.UTF8.GetString(array, 0, (int)nullindex);
  }

  public static string to_str(this byte[] array, int offset, int lenght)
  {
    int nullindex = 0;
    // encoding detection: unicode
    if (array.Length > offset + 2 && array[offset + 1] == 0x00)
    {
      nullindex = Array.IndexOf<byte>(array, 0x00, offset, lenght << 1);
      if (nullindex == -1) nullindex = array.Length;
      while (array.Length - 1 > nullindex + 1 && array[nullindex + 1] != 0x00)
      {
        nullindex = Array.IndexOf<byte>(array, 0x00, nullindex + 1, lenght << 1);
      }
      return System.Text.UnicodeEncoding.Unicode.GetString(array, 0, (int)nullindex);
    }
    nullindex = Array.IndexOf<byte>(array, 0x00, offset, lenght);
    if (nullindex == -1) nullindex = array.Length < lenght ? array.Length : lenght;
    return System.Text.UTF8Encoding.UTF8.GetString(array, offset, (int)nullindex);  
  }
  
  #endregion

  #region ### string ###

  public static byte[] to_utf8(this string text)
  {
    return System.Text.UTF8Encoding.UTF8.GetBytes(text);
  }

  public static byte[] to_u16(this string text)
  {
    return System.Text.UTF8Encoding.Unicode.GetBytes(text);
  }

  public static byte[] to_utf32(this string text)
  {
    return System.Text.UTF8Encoding.UTF32.GetBytes(text);
  }

  #endregion

  internal static void Add(this XmlAttributeOverrides overrides, Type type, string ElementName)
  {
    XmlAttributes attrs = new XmlAttributes();
    attrs.XmlArrayItems.Add(new XmlArrayItemAttribute(ElementName));
    overrides.Add(type, attrs);
  }


}
#endif