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
using System.Runtime.InteropServices;

#if NET40

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
public static class StringHexExtension
{

  public static bool hex_try_to_u8(this string hex, out ulong result)
  {
    result = 0;
    try
    {
      result = Convert.ToUInt64(hex, 16);
      return true;
    }
    catch
    {
      try
      {
        result = Convert.ToUInt64("0x" + hex, 16);
        return true;
      }
      catch { }
    }
    return false;
  }

  public static ulong hex_to_u8(this string hex)
  {
    ulong result;
    if (hex_try_to_u8(hex, out result)) return result;
    throw new FormatException(string.Format("String '{0}' is not in valid hex format (u8).", hex));
  }

  // used in query parser for tag hex numbers
  public static bool hex_try_to_u4(this string hex, out uint result)
  {
    result = 0;
    try
    {
      result = Convert.ToUInt32(hex, 16);
      return true;
    }
    catch
    {
      try
      {
        result = Convert.ToUInt32("0x" + hex, 16);
        return true;
      }
      catch { }
    }
    return false;
  }

  // used in query parser for tag hex numbers
  public static uint hex_to_u4(this string hex)
  {
    uint result;
    if (hex_try_to_u4(hex, out result)) return result;
    throw new FormatException(string.Format("String '{0}' is not in valid hex format (u4).", hex));
  }

  public static bool hex_try_to_u2(this string hex, out ushort result)
  {
    result = 0;
    try
    {
      result = Convert.ToUInt16(hex, 16);
      return true;
    }
    catch
    {
      try
      {
        result = Convert.ToUInt16("0x" + hex, 16);
        return true;
      }
      catch { }
    }
    return false;
  }

  public static ushort hex_to_u2(this string hex)
  {
    ushort result;
    if (hex_try_to_u2(hex, out result)) return result;
    throw new FormatException(string.Format("String '{0}' is not in valid hex format (u2).", hex));
  }

  public static bool hex_try_to_u1(this string hex, out byte result)
  {
    result = 0;
    try
    {
      result = Convert.ToByte(hex, 16);
      return true;
    }
    catch
    {
      try
      {
        result = Convert.ToByte("0x" + hex, 16);
        return true;
      }
      catch { }
    }
    return false;
  }

  public static byte hex_to_u1(this string hex)
  {
    byte result;
    if (hex_try_to_u1(hex, out result)) return result;
    throw new FormatException(string.Format("String '{0}' is not in valid hex format (u1).", hex));
  }

  public static bool hex_try_to_i8(this string hex, out long result)
  {
    result = 0;
    try
    {
      result = Convert.ToInt64(hex, 16);
      return true;
    }
    catch
    {
      try
      {
        result = Convert.ToInt64("0x" + hex, 16);
        return true;
      }
      catch { }
    }
    return false;
  }

  public static long hex_to_i8(this string hex)
  {
    long result;
    if (hex_try_to_i8(hex, out result)) return result;
    throw new FormatException(string.Format("String '{0}' is not in valid hex format (i8).", hex));
  }

  // used in query parser for tag hex numbers
  public static bool hex_try_to_i4(this string hex, out int result)
  {
    result = 0;
    try
    {
      result = Convert.ToInt32(hex, 16);
      return true;
    }
    catch
    {
      try
      {
        result = Convert.ToInt32("0x" + hex, 16);
        return true;
      }
      catch { }
    }
    return false;
  }

  // used in query parser for tag hex numbers
  public static int hex_to_i4(this string hex)
  {
    int result;
    if (hex_try_to_i4(hex, out result)) return result;
    throw new FormatException(string.Format("String '{0}' is not in valid hex format (i4).", hex));
  }

  public static bool hex_try_to_i2(this string hex, out short result)
  {
    result = 0;
    try
    {
      result = Convert.ToInt16(hex, 16);
      return true;
    }
    catch
    {
      try
      {
        result = Convert.ToInt16("0x" + hex, 16);
        return true;
      }
      catch { }
    }
    return false;
  }

  public static short hex_to_i2(this string hex)
  {
    short result;
    if (hex_try_to_i2(hex, out result)) return result;
    throw new FormatException(string.Format("String '{0}' is not in valid hex format (i2).", hex));
  }

  public static bool hex_try_to_i1(this string hex, out sbyte result)
  {
    result = 0;
    try
    {
      result = Convert.ToSByte(hex, 16);
      return true;
    }
    catch
    {
      try
      {
        result = Convert.ToSByte("0x" + hex, 16);
        return true;
      }
      catch { }
    }
    return false;
  }

  public static sbyte hex_to_i1(this string hex)
  {
    sbyte result;
    if (hex_try_to_i1(hex, out result)) return result;
    throw new FormatException(string.Format("String '{0}' is not in valid hex format (i1).", hex));
  }

  public static bool hex_try_to_i1(this string hex, out sbyte[] result)
  {
    result = null;
    int count = hex.Length;
    try
    {
      result = new sbyte[count >> 1];
      for (int i = 0; i < count; i += 2)
      {
        string twochars = hex.Substring(i, 2);
        if (twochars == "\r\n") continue;
        result[i] = Convert.ToSByte("0x" + twochars, 16);
      }
      return true;
    }
    catch { }
    return false;
  }

  public static sbyte[] hex_to_a_i1(this string hex)
  {
    sbyte[] result;
    if (hex_try_to_i1(hex, out result)) return result;
    throw new FormatException(string.Format("String '{0}' is not in valid hex format (a_i1).", hex));
  }

  public static bool hex_try_to_u1(this string hex, out byte[] result)
  {
    result = null;
    int count = hex.Length;
    try
    {
      result = new byte[count >> 1];
      for (int i = 0; i < count; i += 2)
      {
        string twochars = hex.Substring(i, 2);
        if (twochars == "\r\n") continue;
        result[i] = Convert.ToByte("0x" + twochars, 16);
      }
      return true;
    }
    catch { }
    return false;
  }

  public static byte[] hex_to_a_u1(this string hex)
  {
    byte[] result;
    if (hex_try_to_u1(hex, out result)) return result;
    throw new FormatException(string.Format("String '{0}' is not in valid hex format (a_u1).", hex));
  }

}
#endif