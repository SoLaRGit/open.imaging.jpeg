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

// heart of open.imaging.jpeg assembly everything is read using this class

#if DEBUG
// #define DEBUG_JPEGBINARYREADER
#endif

using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace open.imaging.jpeg
{
  /// <summary>
  /// Reads primitive data types as binary values in a specific encoding, 
  /// and with specific endian (byte order).
  /// </summary>
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDual)]
  public class JpegBinaryReader : IDisposable
  {

    BinaryReader reader;
    Stream BaseStream;

    void IDisposable.Dispose()
    {
      if (null != reader)
      {
#if NET40
        reader.Dispose();
#else
        reader.BaseStream.Dispose();
#endif
      }
    }

    public long Position
    {
      get { return BaseStream.Position; }
      set { BaseStream.Position = value; }
    }

    public long Length
    {
      get { return BaseStream.Length; }
    }

    /// <summary>
    /// This flag determines hardware endian used in testing hardware endian against required endian type.
    /// </summary>
    static readonly bool HWIsLittleEndian = BitConverter.IsLittleEndian;

    /// <summary>
    /// This flag determines reader endian type
    /// </summary>
    public bool ReadLittleEndian = true;

    /// <summary>
    /// Initializes a new instance of the JpegBinaryReader class based on the supplied stream and using System.Text.UTF8Encoding.
    /// </summary>
    /// <param name="input">A stream.</param>
    public JpegBinaryReader(Stream input)
    {
      this.BaseStream = input;
      this.reader = new BinaryReader(input);
    }

    #region ### read_i1 ###

    public sbyte read_i1()
    {
      return unchecked((sbyte)reader.ReadSByte());
    }

    unsafe public sbyte[] read_i1(int count)
    {
      // there is type change in read data, but no change in endian
      unchecked
      {
        byte[] data = reader.ReadBytes(count);
#if DEBUG_JPEGBINARYREADER
        StringBuilder sb = new StringBuilder();
        int length = count > 50 ? 50 : count;
        for (int i = 0; i < length; i++) sb.Append(data[i].ToString("X2") + " ");
        Debug.Print(string.Format("* read_i1_a({0})\t@\t0x{1:x8} : [{2,8}]:'{3}" + (count > 50 ? "..." : "") + "'", count, (BaseStream.Position - count), count, sb.ToString()));
#endif
        sbyte[] value = new sbyte[count];
        // fixed+marshaled copy almost as twice faster from
        // Buffer.BlockCopy(data, 0, retVal, 0, size);        
        fixed (sbyte* lp = &value[0])
        {
          Marshal.Copy(data, 0, (IntPtr)lp, count);
        }
        return value;
      }
    }

    #endregion

    #region ### read_u1, read_u1_be, read_u1_le, read_u1_re ###

    unsafe public byte read_u1()
    {
      return reader.ReadByte();
    }

    public byte[] read_u1(int count)
    {
      // efectively there is no change in both type of read data, and endian
#if DEBUG_JPEGBINARYREADER
      byte[] data = reader.ReadBytes(count);
      StringBuilder sb = new StringBuilder();
      int length = count > 50 ? 50 : count;
      for (int i = 0; i < length; i++) sb.Append(data[i].ToString("X2") + " ");
      Debug.Print(string.Format("* read_u1({0})\t@\t0x{1:x8} : [{2,8}]:'{3}" + (count > 50 ? "..." : "") + "'", count, (BaseStream.Position - count), count, sb.ToString()));
      return data;
#else
      return reader.ReadBytes(count);
#endif
    }

    public byte[] read_u1_be(int count, int elementSize)
    {
      unchecked
      {
        int byteCount = (int)(count * elementSize);
        byte[] data = reader.ReadBytes(byteCount);
        if (HWIsLittleEndian)
          data = conv_u1(data, elementSize, true);
        return data;
      }
    }

    public byte[] read_u1_le(int count, int elementSize)
    {
      unchecked
      {
        int byteCount = (int)(count * elementSize);
        byte[] data = reader.ReadBytes(byteCount);
        if (!HWIsLittleEndian)
          data = conv_u1(data, elementSize, true);
        return data;
      }
    }

    public byte[] read_u1_re(int count, int elementSize)
    {
      unchecked
      {
        int byteCount = (int)(count * elementSize);
        byte[] data = reader.ReadBytes(byteCount);
        if (HWIsLittleEndian != ReadLittleEndian)
          data = conv_u1(data, elementSize, true);
        return data;
      }
    }

    #endregion

    #region ### conv_u1, conv_u1_be, conv_u1_le, conv_u1_re ###

    public byte[] conv_u1_be(byte[] data, int elementSize)
    {
      if (HWIsLittleEndian)
        data = conv_u1(data, elementSize, true);
      return data;
    }

    public byte[] conv_u1_le(byte[] data, int elementSize)
    {
      if (!HWIsLittleEndian)
        data = conv_u1(data, elementSize, true);
      return data;
    }

    public byte[] conv_u1_re(byte[] data, int elementSize)
    {
      if (HWIsLittleEndian != ReadLittleEndian)
        data = conv_u1(data, elementSize, true);
      return data;
    }

    /// <summary>
    /// if endianConversion argument is true performs endian conversion for specific 
    /// elementSize on entire data[] array, and returns converted value. Otherwise
    /// it returns original data[] array.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="elementSize"></param>
    /// <param name="endianConversion"></param>
    /// <returns></returns>
    public byte[] conv_u1(byte[] data, int elementSize, bool endianConversion)
    {
      unchecked
      {
        if (endianConversion)
        {
          #region ### reverse the order of bytes for each element ###
          int EOD = data.Length;
          switch (elementSize)
          {
            case 1: // do nothing, return as is
              break;
            case 2:
              {
                byte[] tmp = new byte[EOD];
                int offset1;
                for (int offset0 = 0; offset0 <= (EOD - elementSize); offset0 += elementSize)
                {
                  offset1 = offset0 + 1;
                  tmp[offset0] = data[offset1];
                  tmp[offset1] = data[offset0];
                }
                data = tmp;
              }
              break;
            case 4:
              {
                byte[] tmp = new byte[EOD];
                int offset1, offset2, offset3;
                for (int offset0 = 0; offset0 <= (EOD - elementSize); offset0 += elementSize)
                {
                  offset1 = offset0 + 1;
                  offset2 = offset0 + 2;
                  offset3 = offset0 + 3;
                  tmp[offset0] = data[offset3];
                  tmp[offset1] = data[offset2];
                  tmp[offset2] = data[offset1];
                  tmp[offset3] = data[offset0];
                }
                data = tmp;
              }
              break;
            case 8:
              {
                byte[] tmp = new byte[EOD];
                int offset1, offset2, offset3, offset4, offset5, offset6, offset7;
                for (int offset0 = 0; offset0 <= (EOD - elementSize); offset0 += elementSize)
                {
                  offset1 = offset0 + 1;
                  offset2 = offset0 + 2;
                  offset3 = offset0 + 3;
                  offset4 = offset0 + 4;
                  offset5 = offset0 + 5;
                  offset6 = offset0 + 6;
                  offset7 = offset0 + 7;
                  tmp[offset0] = data[offset7];
                  tmp[offset1] = data[offset6];
                  tmp[offset2] = data[offset5];
                  tmp[offset3] = data[offset4];
                  tmp[offset4] = data[offset3];
                  tmp[offset5] = data[offset2];
                  tmp[offset6] = data[offset1];
                  tmp[offset7] = data[offset0];
                }
                data = tmp;
              }
              break;
          }
          #endregion
        }
#if DEBUG_JPEGBINARYREADER
        // TODO: ... was lazy to write proper debug ...
        byte[] data = reader.ReadBytes(count);
        StringBuilder sb = new StringBuilder();
        int length = count > 50 ? 50 : count;
        for (int i = 0; i < length; i++) sb.Append(data[i].ToString("X2") + " ");
        Debug.Print(string.Format("* conv_u1({0})\t@\t0x{1:x8} : [{2,8}]:'{3}" + (count > 50 ? "..." : "") + "'", count, (BaseStream.Position - count), count, sb.ToString()));
        return data;
#endif
        return data;
      }
    }

    #endregion

    #region ### read_u2_le, read_u2_be, read_u2_re ###

    /// <summary>
    /// USHORT (16 bit) in little endian. |
    /// Reads a two-byte unsigned integer from the current stream and advances the current position of the stream by two bytes.
    /// </summary>
    public ushort read_u2_le()
    {
      unchecked
      {
        ushort value = reader.ReadUInt16();
        if (!HWIsLittleEndian)
          value = (ushort)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8));
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_ui2_le\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 2L), value, value));
#endif
        return value;
      }
    }

    /// <summary>
    /// USHORT (16 bit) array in little endian. |
    /// Reads a two-byte unsigned integer a count number of times from the current stream and advances the current position of the stream each time by two bytes.
    /// </summary>
    public ushort[] read_u2_le(int count)
    {
      unchecked
      {
        ushort[] value = new ushort[count];
        for (int i = 0; i < count; i++)
          value[i] = read_u2_le();
        return value;
      }
    }

    /// <summary>
    /// USHORT (16 bit) in big endian. |
    /// Reads a two-byte unsigned integer from the current stream and advances the current position of the stream by two bytes.
    /// </summary>
    public ushort read_u2_be()
    {
      unchecked
      {
        ushort value = reader.ReadUInt16();
        if (HWIsLittleEndian)
          value = (ushort)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8));
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_ui2_le\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 2L), value, value));
#endif
        return value;
      }
    }

    /// <summary>
    /// USHORT (16 bit) array in big endian. |
    /// Reads a two-byte unsigned integer a count number of times from the current stream and advances the current position of the stream each time by two bytes.
    /// </summary>
    public ushort[] read_u2_be(int count)
    {
      unchecked
      {
        ushort[] value = new ushort[count];
        for (int i = 0; i < count; i++)
          value[i] = read_u2_be();
        return value;
      }
    }

    /// <summary>
    /// USHORT (16 bit) in reading endian set by <see cref="ReadLittleEndian"/> member. |
    /// Reads a two-byte unsigned integer from the current stream and advances the current position of the stream by two bytes.
    /// </summary>
    public ushort read_u2_re()
    {
      unchecked
      {
        ushort value = reader.ReadUInt16();
        if (HWIsLittleEndian != ReadLittleEndian)
          value = (ushort)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8));
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_ui2_re\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 2L), value, value));
#endif
        return value;
      }
    }

    /// <summary>
    /// USHORT (16 bit) array in reading endian set by <see cref="ReadLittleEndian"/> member. |
    /// Reads a two-byte unsigned integer a count number of times from the current stream and advances the current position of the stream each time by two bytes.
    /// </summary>
    public ushort[] read_u2_re(int count)
    {
      unchecked
      {
        ushort[] value = new ushort[count];
        for (int i = 0; i < count; i++)
          value[i] = read_u2_re();
        return value;
      }
    }

    #endregion

    #region ### read_i2_le, read_i2_be, read_i2_re ###

    /// <summary>
    /// SHORT (16 bit) in little endian. |
    /// Reads a 2-byte signed integer from the current stream and advances the current position of the stream by two bytes.
    /// </summary>
    public short read_i2_le()
    {
      unchecked
      {
        short value = reader.ReadInt16();
        if (!HWIsLittleEndian)
          value = (short)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8));
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_i2_le\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 2L), value, value));
#endif
        return value;
      }
    }

    /// <summary>
    /// SHORT (16 bit) array in little endian. |
    /// Reads a 2-byte signed integer a count number of times from the current stream and advances the current position of the stream each time by two bytes.
    /// </summary>
    public short[] read_i2_le(int count)
    {
      unchecked
      {
        short[] value = new short[count];
        for (int i = 0; i < count; i++)
          value[i] = read_i2_le();
        return value;
      }
    }

    /// <summary>
    /// SHORT (16 bit) in big endian. |
    /// Reads a 2-byte signed integer from the current stream and advances the current position of the stream by two bytes.
    /// </summary>
    public short read_i2_be()
    {
      unchecked
      {
        short value = reader.ReadInt16();
        if (HWIsLittleEndian)
          value = (short)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8));
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_i2_be\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 2L), value, value));
#endif
        return value;
      }
    }

    /// <summary>
    /// SHORT (16 bit) array in big endian. |
    /// Reads a 2-byte signed integer a count number of times from the current stream and advances the current position of the stream each time by two bytes.
    /// </summary>
    public short[] read_i2_be(int count)
    {
      unchecked
      {
        short[] value = new short[count];
        for (int i = 0; i < count; i++)
          value[i] = read_i2_be();
        return value;
      }
    }

    /// <summary>
    /// SHORT (16 bit) in reading endian set by <see cref="ReadLittleEndian"/> member. |
    /// Reads a 2-byte signed integer from the current stream and advances the current position of the stream by two bytes.
    /// </summary>
    public short read_i2_re()
    {
      unchecked
      {
        short value = reader.ReadInt16();
        if (HWIsLittleEndian != ReadLittleEndian)
          value = (short)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8));
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_i2_re\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 2L), value, value));
#endif
        return value;
      }
    }

    /// <summary>
    /// SHORT (16 bit) array in reading endian set by <see cref="ReadLittleEndian"/> member. |
    /// Reads a 2-byte signed integer a count number of times from the current stream and advances the current position of the stream each time by two bytes.
    /// </summary>
    public short[] read_i2_re(int count)
    {
      unchecked
      {
        short[] value = new short[count];
        for (int i = 0; i < count; i++)
          value[i] = read_i2_re();
        return value;
      }
    }

    #endregion

    #region ### read_u4_le, read_u4_be, read_u4_re ###

    /// <summary>
    /// UINT (32 bit) in little endian. |
    /// Reads a four-byte unsigned integer from the current stream and advances the position of the stream by four bytes.
    /// </summary>
    public uint read_u4_le()
    {
      unchecked
      {
        uint value = reader.ReadUInt32();
        if (!HWIsLittleEndian)
          value = (uint)(((value & 0xFF000000) >> 24) | ((value & 0x00FF0000) >> 8) |
                          ((value & 0x0000FF00) << 8) | ((value & 0x000000FF) << 24));
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_ui4_le\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 4L), value, value));
#endif
        return value;
      }
    }

    /// <summary>
    /// UINT (32 bit) array in little endian. |
    /// Reads a four-byte unsigned integer a count number of times from the current stream and advances the current position of the stream each time by four bytes.
    /// </summary>
    public uint[] read_u4_le(int count)
    {
      unchecked
      {
        uint[] value = new uint[count];
        for (int i = 0; i < count; i++)
          value[i] = read_u4_le();
        return value;
      }
    }

    /// <summary>
    /// UINT (32 bit) in big endian. |
    /// Reads a four-byte unsigned integer from the current stream and advances the position of the stream by four bytes.
    /// </summary>
    public uint read_u4_be()
    {
      unchecked
      {
        uint value = reader.ReadUInt32();
        if (HWIsLittleEndian)
          value = (uint)(((value & 0xFF000000) >> 24) | ((value & 0x00FF0000) >> 8) |
                          ((value & 0x0000FF00) << 8) | ((value & 0x000000FF) << 24));
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_ui4_be\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 4L), value, value));
#endif
        return value;
      }
    }

    /// <summary>
    /// UINT (32 bit) array in big endian. |
    /// Reads a four-byte unsigned integer a count number of times from the current stream and advances the current position of the stream each time by four bytes.
    /// </summary>
    public uint[] read_u4_be(int count)
    {
      unchecked
      {
        uint[] value = new uint[count];
        for (int i = 0; i < count; i++)
          value[i] = read_u4_be();
        return value;
      }
    }

    /// <summary>
    /// UINT (32 bit) in reading endian set by <see cref="ReadLittleEndian"/> member. |
    /// Reads a four-byte unsigned integer from the current stream and advances the position of the stream by four bytes.
    /// </summary>
    public uint read_u4_re()
    {
      unchecked
      {
        uint value = reader.ReadUInt32();
        if (HWIsLittleEndian != ReadLittleEndian)
          value = (uint)(((value & 0xFF000000) >> 24) | ((value & 0x00FF0000) >> 8) |
                          ((value & 0x0000FF00) << 8) | ((value & 0x000000FF) << 24));
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_i4_be\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 4L), value, value));
#endif
        return value;
      }
    }

    /// <summary>
    /// UINT (32 bit) array in reading endian set by <see cref="ReadLittleEndian"/> member. |
    /// Reads a four-byte unsigned integer a count number of times from the current stream and advances the current position of the stream each time by four bytes.
    /// </summary>
    public uint[] read_u4_re(int count)
    {
      unchecked
      {
        uint[] value = new uint[count];
        for (int i = 0; i < count; i++)
          value[i] = read_u4_re();
        return value;
      }
    }

    #endregion

    #region ### read_i4_le, read_i4_be, read_i4_re ###

    /// <summary>
    /// INT (32 bit) in little endian. |
    /// Reads a four-byte signed integer from the current stream and advances the current position of the stream by four bytes.
    /// </summary>
    public int read_i4_le()
    {
      unchecked
      {
        uint value = reader.ReadUInt32();
        if (!HWIsLittleEndian)
          value = (uint)(((value & 0xFF000000) >> 24) | ((value & 0x00FF0000) >> 8) |
                          ((value & 0x0000FF00) << 8) | ((value & 0x000000FF) << 24));
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_i4_le\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 4L), value, value));
#endif
        return (int)value;
      }
    }

    /// <summary>
    /// INT (32 bit) array in little endian. |
    /// Reads a four-byte signed integer a count number of times from the current stream and advances the current position of the stream each time by four bytes.
    /// </summary>
    public int[] read_i4_le(int count)
    {
      unchecked
      {
        int[] value = new int[count];
        for (int i = 0; i < count; i++)
          value[i] = read_i4_le();
        return value;
      }
    }

    /// <summary>
    /// INT (32 bit) in big endian. |
    /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
    /// </summary>
    public int read_i4_be()
    {
      unchecked
      {
        uint value = reader.ReadUInt32();
        if (HWIsLittleEndian)
          value = (uint)(((value & 0xFF000000) >> 24) | ((value & 0x00FF0000) >> 8) |
                          ((value & 0x0000FF00) << 8) | ((value & 0x000000FF) << 24));
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_i4_be\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 4L), value, value));
#endif
        return (int)value;
      }
    }

    /// <summary>
    /// INT (32 bit) array in big endian. |
    /// Reads a four-byte signed integer a count number of times from the current stream and advances the current position of the stream each time by four bytes.
    /// </summary>
    public int[] read_i4_be(int count)
    {
      unchecked
      {
        int[] value = new int[count];
        for (int i = 0; i < count; i++)
          value[i] = read_i4_be();
        return value;
      }
    }

    /// <summary>
    /// INT (32 bit) in reading endian set by <see cref="ReadLittleEndian"/> member. |
    /// Reads an 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
    /// </summary>
    public int read_i4_re()
    {
      unchecked
      {
        uint value = reader.ReadUInt32();
        if (HWIsLittleEndian != ReadLittleEndian)
          value = (uint)(((value & 0xFF000000) >> 24) | ((value & 0x00FF0000) >> 8) |
                          ((value & 0x0000FF00) << 8) | ((value & 0x000000FF) << 24));
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_i4_be\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 4L), value, value));
#endif
        return (int)value;
      }
    }

    /// <summary>
    /// INT (32 bit) array in reading endian set by <see cref="ReadLittleEndian"/> member. |
    /// Reads a four-byte signed integer a count number of times from the current stream and advances the current position of the stream each time by four bytes.
    /// </summary>
    public int[] read_i4_re(int count)
    {
      unchecked
      {
        int[] value = new int[count];
        for (int i = 0; i < count; i++)
          value[i] = read_i4_re();
        return value;
      }
    }

    #endregion

    #region ### read_f4_le, read_f4_be, read_f4_re ###

    [StructLayout(LayoutKind.Explicit)]
    struct FloatAndUIntUnion
    {
      [FieldOffset(0)]
      public uint ui4;
      [FieldOffset(0)]
      public float f4;
    }

    /// <summary>
    /// FLOAT (32 bit) in little endian |
    /// Reads an 4-byte float (Single) from the current stream and advances the current position of the stream by four bytes.
    /// </summary>
    public float read_f4_le()
    {
      unchecked
      {
        FloatAndUIntUnion t = new FloatAndUIntUnion() { ui4 = reader.ReadUInt32() };
        if (!HWIsLittleEndian)
          t.ui4 = ((t.ui4 & 0xFF000000) >> 24) |
                  ((t.ui4 & 0x00FF0000) >> 08) |
                  ((t.ui4 & 0x0000FF00) << 08) |
                  ((t.ui4 & 0x000000FF) << 24);
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_f4_le\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 4L), t.ui4, t.f4));
#endif
        return t.f4;
      }
    }

    /// <summary>
    /// FLOAT (32 bit) in big endian |
    /// Reads an 4-byte float (Single) from the current stream and advances the current position of the stream by four bytes.
    /// </summary>
    public float read_f4_be()
    {
      unchecked
      {
        FloatAndUIntUnion t = new FloatAndUIntUnion() { ui4 = reader.ReadUInt32() };
        if (!HWIsLittleEndian)
          t.ui4 = ((t.ui4 & 0xFF000000) >> 24) |
                  ((t.ui4 & 0x00FF0000) >> 08) |
                  ((t.ui4 & 0x0000FF00) << 08) |
                  ((t.ui4 & 0x000000FF) << 24);
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_f4_be\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 4L), t.ui4, t.f4));
#endif
        return t.f4;
      }
    }

    /// <summary>
    /// FLOAT (32 bit) in reading endian set by <see cref="ReadLittleEndian"/> member. |
    /// Reads an 4-byte float (Single) from the current stream and advances the current position of the stream by four bytes.
    /// </summary>
    public float read_f4_re()
    {
      unchecked
      {
        FloatAndUIntUnion t = new FloatAndUIntUnion() { ui4 = reader.ReadUInt32() };
        if (HWIsLittleEndian != ReadLittleEndian)
          t.ui4 = ((t.ui4 & 0xFF000000) >> 24) |
                  ((t.ui4 & 0x00FF0000) >> 08) |
                  ((t.ui4 & 0x0000FF00) << 08) |
                  ((t.ui4 & 0x000000FF) << 24);
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_f4_re\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 4L), t.ui4, t.f4));
#endif
        return t.f4;
      }
    }
    #endregion

    #region ### read_f8_le, read_f8_be, read_f8_re ###

    [StructLayout(LayoutKind.Explicit)]
    struct DoubleAndULongUnion
    {
      [FieldOffset(0)]
      public ulong ui8;
      [FieldOffset(0)]
      public double f8;
    }

    /// <summary>
    /// DOUBLE (64 bit) in little endian |
    /// Reads an eight-byte float (Double) from the current stream and advances the current position of the stream by eight bytes.
    /// </summary>
    public double read_f8_le()
    {
      unchecked
      {
        DoubleAndULongUnion t = new DoubleAndULongUnion() { ui8 = reader.ReadUInt64() };
        if (!HWIsLittleEndian)
          t.ui8 = ((t.ui8 & 0xFF00000000000000) >> 56) |
                  ((t.ui8 & 0x00FF000000000000) >> 40) |
                  ((t.ui8 & 0x0000FF0000000000) >> 24) |
                  ((t.ui8 & 0x000000FF00000000) >> 08) |
                  ((t.ui8 & 0x00000000FF000000) << 08) |
                  ((t.ui8 & 0x0000000000FF0000) << 24) |
                  ((t.ui8 & 0x000000000000FF00) << 40) |
                  ((t.ui8 & 0x00000000000000FF) << 56);
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_f8_le\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 4L), t.ui8, t.f8));
#endif
        return t.f8;
      }
    }

    /// <summary>
    /// DOUBLE (64 bit) in big endian |
    /// Reads an eight-byte float (Double) from the current stream and advances the current position of the stream by eight bytes.
    /// </summary>
    public double read_f8_be()
    {
      unchecked
      {
        DoubleAndULongUnion t = new DoubleAndULongUnion() { ui8 = reader.ReadUInt64() };
        if (HWIsLittleEndian)
          t.ui8 = ((t.ui8 & 0xFF00000000000000) >> 56) |
                  ((t.ui8 & 0x00FF000000000000) >> 40) |
                  ((t.ui8 & 0x0000FF0000000000) >> 24) |
                  ((t.ui8 & 0x000000FF00000000) >> 08) |
                  ((t.ui8 & 0x00000000FF000000) << 08) |
                  ((t.ui8 & 0x0000000000FF0000) << 24) |
                  ((t.ui8 & 0x000000000000FF00) << 40) |
                  ((t.ui8 & 0x00000000000000FF) << 56);
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_f8_le\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 4L), t.ui8, t.f8));
#endif
        return t.f8;
      }
    }

    /// <summary>
    /// DOUBLE (64 bit) in read endian set by <see cref="ReadLittleEndian"/> member. |
    /// Reads an eight-byte float (Double) from the current stream and advances the current position of the stream by eight bytes.
    /// </summary>
    public double read_f8_re()
    {
      unchecked
      {
        DoubleAndULongUnion t = new DoubleAndULongUnion() { ui8 = reader.ReadUInt64() };
        if (HWIsLittleEndian != ReadLittleEndian)
          t.ui8 = ((t.ui8 & 0xFF00000000000000) >> 56) |
                  ((t.ui8 & 0x00FF000000000000) >> 40) |
                  ((t.ui8 & 0x0000FF0000000000) >> 24) |
                  ((t.ui8 & 0x000000FF00000000) >> 08) |
                  ((t.ui8 & 0x00000000FF000000) << 08) |
                  ((t.ui8 & 0x0000000000FF0000) << 24) |
                  ((t.ui8 & 0x000000000000FF00) << 40) |
                  ((t.ui8 & 0x00000000000000FF) << 56);
#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_f8_re\t@\t0x{0:x8} : 0x{1:x4} ({2})", (BaseStream.Position - 4L), t.ui8, t.f8));
#endif
        return t.f8;
      }
    }

    #endregion

    /// <summary>
    /// ULONG (64 bit) in big endian. |
    /// Reads a eight-byte unsigned integer from the current stream and advances the position of the stream by eight bytes.
    /// </summary>
    public ulong read_u8_be()
    {
      unchecked
      {
        ulong value = reader.ReadUInt64();        
        if (HWIsLittleEndian != ReadLittleEndian)
          value = ((value & 0xFF00000000000000) >> 56) |
                  ((value & 0x00FF000000000000) >> 40) |
                  ((value & 0x0000FF0000000000) >> 24) |
                  ((value & 0x000000FF00000000) >> 08) |
                  ((value & 0x00000000FF000000) << 08) |
                  ((value & 0x0000000000FF0000) << 24) |
                  ((value & 0x000000000000FF00) << 40) |
                  ((value & 0x00000000000000FF) << 56);

#if DEBUG_JPEGBINARYREADER
        Debug.Print(string.Format("* read_ui8_be\t@\t0x{0:x8} : 0x{1:x8} ({2})", (BaseStream.Position - 4L), value, value));
#endif
        return value;
      }
    }

  }

}
