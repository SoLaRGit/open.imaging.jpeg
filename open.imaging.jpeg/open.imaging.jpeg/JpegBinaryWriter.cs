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

// heart of open.imaging.jpeg assembly everything is writen using this class

#if DEBUG
//#define DEBUG_JPEGBINARYWRITER
#endif

using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace open.imaging.jpeg
{
  /// <summary>
  /// Writes primitive types in binary to a stream and supports writing strings in a specific encoding, 
  /// and with specific endian (byte order).
  /// </summary>
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDual)]
  public class JpegBinaryWriter : IDisposable
  {

    BinaryWriter writer;
    Stream BaseStream;

    void IDisposable.Dispose()
    {
      if (null != writer)
      {
#if NET40
        writer.Dispose();
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
    /// Causes underlying data to be writen to device, and clears all buffers.
    /// </summary>
    public void Flush()
    {
      BaseStream.Flush();
    }

    /// <summary>
    /// This flag determines hardware endian used in testing hardware endian against required endian type.
    /// </summary>
    static readonly bool HWIsLittleEndian = BitConverter.IsLittleEndian;

    /// <summary>
    /// This flag determines writer endian type when using following member functions:
    /// <see cref="write_u1_we"/>, <see cref="write_u2_we"/>, <see cref="write_i2_we"/>, <see cref="write_u4_we"/>, <see cref="write_i4_we"/>, <see cref="write_f4_we"/>, <see cref="write_f8_we"/>.
    /// </summary>
    public bool WriteLittleEndian = true;

    /// <summary>
    /// Initializes a new instance of the JpegFileWriter class based on the supplied stream and using UTF-8 as the encoding for strings.
    /// </summary>
    /// <param name="output">The output stream.</param>
    public JpegBinaryWriter(Stream output)      
    {
      BaseStream = output;
      writer = new BinaryWriter(output);
    }

#if DEBUG_JPEGBINARYWRITER
    public override void Write(byte[] value)
    {
      StringBuilder sb = new StringBuilder();
      int length = value.Length > 50 ? 50 : value.Length;
      for (int i = 0; i < length; i++) sb.Append(value[i].ToString("X2") + " ");
      Debug.Print(string.Format("* Write(byte[])\t@\t0x{0:x8} : [{1,8}]:'{2}" + (value.Length > 50 ? "..." : "") + "'", BaseStream.Position, value.Length, sb.ToString()));
      base.Write(value);
    }
#endif

    #region ### write_u1, write_u1_be, write_u1_le, write_u1_we ###

    /// <summary>
    /// Writes single byte
    /// </summary>
    public void write_u1(byte value)
    {
      writer.Write(value);
    }

    /// <summary>
    /// Writes array of bytes
    /// </summary>
    public void write_u1(byte[] value)
    {
      writer.Write(value);
    }
    
    /// <summary>
    /// Writes array of bytes in big endian
    /// </summary>
    public void write_u1_be(byte[] value, int elementSize)
    {
      if (HWIsLittleEndian)
        value = conv_u1(value, elementSize, true);
      writer.Write(value);
    }

    /// <summary>
    /// Writes array of bytes in little endian
    /// </summary>
    public void write_u1_le(byte[] value, int elementSize)
    {
      if (!HWIsLittleEndian)
        value = conv_u1(value, elementSize, true);
      writer.Write(value);
    }

    /// <summary>
    /// Writes array of bytes in Writer endian
    /// </summary>
    public void write_u1_we(byte[] value, int elementSize)
    {
      if (HWIsLittleEndian != WriteLittleEndian)
        value = conv_u1(value, elementSize, true);
      writer.Write(value);
    }

    #endregion

    #region ### conv_u1, conv_u1_be, conv_u1_le, conv_u1_we ###

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

    public byte[] conv_u1_we(byte[] data, int elementSize)
    {
      if (HWIsLittleEndian != WriteLittleEndian)
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
                tmp[offset1] = data[offset1];
                tmp[offset2] = data[offset2];
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
                tmp[offset5] = data[offset1];
                tmp[offset6] = data[offset2];
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
      StringBuilder sb = new StringBuilder();
      int length = count > 50 ? 50 : count;
      for (int i = 0; i < length; i++) sb.Append(data[i].ToString("X2") + " ");
      Debug.Print(string.Format("* conv_u1({0})\t@\t0x{1:x8} : [{2,8}]:'{3}" + (count > 50 ? "..." : "") + "'", count, (BaseStream.Position - count), count, sb.ToString()));
#endif
      return data;
    }

    #endregion

    #region ### write_u2_le, write_u2_be, write_u2_we ###

    /// <summary>
    /// USHORT (16 bit) in little endian. |
    /// Writes a two-byte unsigned integer to the current stream and advances the stream position by two bytes.
    /// </summary>
    public void write_u2_le(ushort value)
    {
      if (!HWIsLittleEndian) 
        value = unchecked((ushort)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8)));
#if DEBUG_JPEGBINARYWRITER
      Debug.Print(string.Format("* write_ui2_le\t@\t0x{0:x8} : 0x{1:x4} ({2})", BaseStream.Position, value, value));
#endif     
      writer.Write(value);
    }

    /// <summary>
    /// USHORT (16 bit) in big endian. | 
    /// Writes a two-byte unsigned integer to the current stream and advances the stream position by two bytes.
    /// </summary>
    public void write_u2_be(ushort value)
    {
      if (HWIsLittleEndian) 
        value = unchecked((ushort)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8)));
#if DEBUG_JPEGBINARYWRITER
      Debug.Print(string.Format("* write_ui2_be\t@\t0x{0:x8} : 0x{1:x4} ({2})", BaseStream.Position, value, value));
#endif
      writer.Write(value);
    }

    /// <summary>
    /// USHORT (16 bit) in write endian set by <see cref="WriteLittleEndian"/> member. |
    /// Writes a two-byte unsigned integer to the current stream and advances the stream position by two bytes.
    /// </summary>
    public void write_u2_we(ushort value)
    {
      if (HWIsLittleEndian != WriteLittleEndian)
        value = unchecked((ushort)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8)));
#if DEBUG_JPEGBINARYWRITER
      Debug.Print(string.Format("* write_ui2_re\t@\t0x{0:x8} : 0x{1:x4} ({2})", BaseStream.Position, value, value));
#endif
      writer.Write(value);
    }

    #endregion

    #region ### write_i2_le, write_i2_be, write_i2_we ###

    /// <summary>
    /// SHORT (16 bit) in little endian. |
    /// Writes a two-byte signed integer to the current stream and advances the stream position by two bytes.
    /// </summary>
    public void write_i2_le(short value)
    {
      if (!HWIsLittleEndian) 
        value = unchecked((short)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8)));
#if DEBUG_JPEGBINARYWRITER
      Debug.Print(string.Format("* write_i2_le\t@\t0x{0:x8} : 0x{1:x4} ({2})", BaseStream.Position, value, value));
#endif
      writer.Write(value);
    }

    /// <summary>
    /// SHORT (16 bit) in big endian. |
    /// Writes a two-byte signed integer to the current stream and advances the stream position by two bytes.
    /// </summary>
    public void write_i2_be(short value)
    {
      if (HWIsLittleEndian) 
        value = unchecked((short)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8)));
#if DEBUG_JPEGBINARYWRITER
      Debug.Print(string.Format("* write_i2_be\t@\t0x{0:x8} : 0x{1:x4} ({2})", BaseStream.Position, value, value));
#endif
      writer.Write(value);
    }

    /// <summary>
    /// SHORT (16 bit) in write endian set by <see cref="WriteLittleEndian"/> member. |
    /// Writes a two-byte signed integer to the current stream and advances the stream position by two bytes.
    /// </summary>
    public void write_i2_we(short value)
    {
      if (HWIsLittleEndian != WriteLittleEndian)
        value = unchecked((short)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8)));
#if DEBUG_JPEGBINARYWRITER
      Debug.Print(string.Format("* write_i2_re\t@\t0x{0:x8} : 0x{1:x4} ({2})", BaseStream.Position, value, value));
#endif
      writer.Write(value);
    }

    #endregion

    #region ### write_u4_le, write_u4_be, write_u4_we ###

    /// <summary>
    /// UINT (32 bit) in little endian. |
    /// Writes a four-byte unsigned integer to the current stream and advances the stream position by four bytes.
    /// </summary>
    public void write_u4_le(uint value)
    {
      if (!HWIsLittleEndian) 
        value = unchecked((uint)(((value & 0xFF000000) >> 24) | ((value & 0x00FF0000) >> 8) |
                                  ((value & 0x0000FF00) << 8) | ((value & 0x000000FF) << 24)));
#if DEBUG_JPEGBINARYWRITER
      Debug.Print(string.Format("* write_ui4_le\t@\t0x{0:x8} : 0x{1:x8} ({2})", BaseStream.Position, value, value));
#endif     
      writer.Write(value);
    }

    /// <summary>
    /// UINT (32 bit) in big endian. |
    /// Writes a four-byte unsigned integer to the current stream and advances the stream position by four bytes.
    /// </summary>
    public void write_u4_be(uint value)
    {
      if (HWIsLittleEndian) 
        value = unchecked((uint)(((value & 0xFF000000) >> 24) | ((value & 0x00FF0000) >> 8) |
                                  ((value & 0x0000FF00) << 8) | ((value & 0x000000FF) << 24)));
#if DEBUG_JPEGBINARYWRITER
      Debug.Print(string.Format("* write_ui4_be\t@\t0x{0:x8} : 0x{1:x8} ({2})", BaseStream.Position, value, value));
#endif
      writer.Write(value);
    }

    /// <summary>
    /// UINT (32 bit) in write endian set by <see cref="WriteLittleEndian"/> member. |
    /// Writes a four-byte unsigned integer to the current stream and advances the stream position by four bytes.
    /// </summary>
    public void write_u4_we(uint value)
    {
      if (HWIsLittleEndian != WriteLittleEndian)
        value = unchecked((uint)((((uint)value & 0xFF000000) >> 24) | (((uint)value & 0x00FF0000) >> 08) |
                                 (((uint)value & 0x0000FF00) << 08) | (((uint)value & 0x000000FF) << 24)));
#if DEBUG_JPEGBINARYWRITER
      Debug.Print(string.Format("* write_ui4_re\t@\t0x{0:x8} : 0x{1:x8} ({2})", BaseStream.Position, value, value));
#endif
      writer.Write(value);
    }

    #endregion

    #region ### write_i4_le, write_i4_be, write_i4_we ###

    /// <summary>
    /// INT (32 bit) in little endian. |
    /// Writes a four-byte signed integer to the current stream and advances the stream position by four bytes.
    /// </summary>
    public void write_i4_le(int value)
    {
      if (!HWIsLittleEndian) 
        value = unchecked((int)((((uint)value & 0xFF000000) >> 24) | (((uint)value & 0x00FF0000) >> 8) |
                                 (((uint)value & 0x0000FF00) << 8) | (((uint)value & 0x000000FF) << 24)));
#if DEBUG_JPEGBINARYWRITER
      Debug.Print(string.Format("* write_i4_le\t@\t0x{0:x8} : 0x{1:x8} ({2})", BaseStream.Position, value, value));
#endif     
      writer.Write(value);
    }

    /// <summary>
    /// INT (32 bit) in big endian. |
    /// Writes a four-byte signed integer to the current stream and advances the stream position by four bytes.
    /// </summary>
    public void write_i4_be(int value)
    {
      if (HWIsLittleEndian) 
        value = unchecked((int)((((uint)value & 0xFF000000) >> 24) | (((uint)value & 0x00FF0000) >> 8) |
                                 (((uint)value & 0x0000FF00) << 8) | (((uint)value & 0x000000FF) << 24)));
#if DEBUG_JPEGBINARYWRITER
      Debug.Print(string.Format("* write_i4_be\t@\t0x{0:x8} : 0x{1:x8} ({2})", BaseStream.Position, value, value));
#endif     
      writer.Write(value);
    }

    /// <summary>
    /// INT (32 bit) in write endian set by <see cref="WriteLittleEndian"/> member. |
    /// Writes a four-byte signed integer to the current stream and advances the stream position by four bytes.
    /// </summary>
    public void write_i4_we(int value)
    {
      if (HWIsLittleEndian != WriteLittleEndian) 
        value = unchecked((int)((((uint)value & 0xFF000000) >> 24) | (((uint)value & 0x00FF0000) >> 08) |
                                (((uint)value & 0x0000FF00) << 08) | (((uint)value & 0x000000FF) << 24)));
#if DEBUG_JPEGBINARYWRITER
      Debug.Print(string.Format("* write_i4_re\t@\t0x{0:x8} : 0x{1:x8} ({2})", BaseStream.Position, value, value));
#endif     
      writer.Write(value);
    }

    #endregion

    #region ### write_f4_le, write_f4_be, write_f4_we ###

    [StructLayout(LayoutKind.Explicit)]
    struct FloatAndUIntUnion
    {
      [FieldOffset(0)] public uint ui4;
      [FieldOffset(0)] public float f4;
    }

    /// <summary>
    /// FLOAT (32 bit) in little endian |
    /// Writes a four-byte float (Single) to the current stream and advances the stream position by four bytes.
    /// </summary>
    public void write_f4_le(float value)
    {
      unchecked
      {
        FloatAndUIntUnion t = new FloatAndUIntUnion() { f4 = value };
        if (!HWIsLittleEndian)
          t.ui4 = ((t.ui4 & 0xFF000000) >> 24) |
                  ((t.ui4 & 0x00FF0000) >> 08) |
                  ((t.ui4 & 0x0000FF00) << 08) |
                  ((t.ui4 & 0x000000FF) << 24);
#if DEBUG_JPEGBINARYWRITER
        Debug.Print(string.Format("* write_f4_le\t@\t0x{0:x8} : 0x{1:x8} ({2})", BaseStream.Position, t.ui4, t.f4));
#endif
        writer.Write(t.ui4);
      }
    }

    /// <summary>
    /// FLOAT (32 bit) in big endian |
    /// Writes a four-byte float (Single) to the current stream and advances the stream position by four bytes.
    /// </summary>
    public void write_f4_be(float value)
    {
      unchecked
      {
        FloatAndUIntUnion t = new FloatAndUIntUnion() { f4 = value };
        if (HWIsLittleEndian)
          t.ui4 = ((t.ui4 & 0xFF000000) >> 24) |
                  ((t.ui4 & 0x00FF0000) >> 08) |
                  ((t.ui4 & 0x0000FF00) << 08) |
                  ((t.ui4 & 0x000000FF) << 24);
#if DEBUG_JPEGBINARYWRITER
        Debug.Print(string.Format("* write_f4_be\t@\t0x{0:x8} : 0x{1:x8} ({2})", BaseStream.Position, t.ui4, t.f4));
#endif
        writer.Write(t.ui4);
      }
    }

    /// <summary>
    /// FLOAT (32 bit) in write endian set by <see cref="WriteLittleEndian"/> member. |
    /// Writes a four-byte float (Single) to the current stream and advances the stream position by four bytes.
    /// </summary>
    public void write_f4_we(float value)
    {
      unchecked
      {
        FloatAndUIntUnion t = new FloatAndUIntUnion() { f4 = value };
        if (HWIsLittleEndian != WriteLittleEndian)
          t.ui4 = ((t.ui4 & 0xFF000000) >> 24) |
                  ((t.ui4 & 0x00FF0000) >> 08) |
                  ((t.ui4 & 0x0000FF00) << 08) |
                  ((t.ui4 & 0x000000FF) << 24);
#if DEBUG_JPEGBINARYWRITER
        Debug.Print(string.Format("* write_f4_we\t@\t0x{0:x8} : 0x{1:x8} ({2})", BaseStream.Position, t.ui4, t.f4));
#endif
        writer.Write(t.ui4);
      }
    }

    #endregion

    #region ### write_f8_le, write_f8_be, write_f8_we ###

    [StructLayout(LayoutKind.Explicit)]
    struct DoubleAndUInt64Union
    {
      [FieldOffset(0)] public ulong ui8;
      [FieldOffset(0)] public double f8;
    }

    /// <summary>
    /// DOUBLE (64 bit) in write little endian |
    /// Writes a eight-byte float (Double) to the current stream and advances the stream position by eight bytes.
    /// </summary>
    public void write_f8_le(double value)
    {
      unchecked
      {
        DoubleAndUInt64Union t = new DoubleAndUInt64Union() { f8 = value };
        if (!HWIsLittleEndian)
          t.ui8 = ((t.ui8 & 0xFF00000000000000) >> 56) |
                  ((t.ui8 & 0x00FF000000000000) >> 40) |
                  ((t.ui8 & 0x0000FF0000000000) >> 24) |
                  ((t.ui8 & 0x000000FF00000000) >> 08) |
                  ((t.ui8 & 0x00000000FF000000) << 08) |
                  ((t.ui8 & 0x0000000000FF0000) << 24) |
                  ((t.ui8 & 0x000000000000FF00) << 40) |
                  ((t.ui8 & 0x00000000000000FF) << 56);
#if DEBUG_JPEGBINARYWRITER
        Debug.Print(string.Format("* write_f8_le\t@\t0x{0:x8} : 0x{1:x8} ({2})", BaseStream.Position, t.ui8, t.f8));
#endif
        writer.Write(t.ui8);
      }
    }

    /// <summary>
    /// DOUBLE (64 bit) in write big endian |
    /// Writes a eight-byte float (Double) to the current stream and advances the stream position by eight bytes.
    /// </summary>
    public void write_f8_be(double value)
    {
      unchecked
      {
        DoubleAndUInt64Union t = new DoubleAndUInt64Union() { f8 = value };
        if (HWIsLittleEndian)
          t.ui8 = ((t.ui8 & 0xFF00000000000000) >> 56) |
                  ((t.ui8 & 0x00FF000000000000) >> 40) |
                  ((t.ui8 & 0x0000FF0000000000) >> 24) |
                  ((t.ui8 & 0x000000FF00000000) >> 08) |
                  ((t.ui8 & 0x00000000FF000000) << 08) |
                  ((t.ui8 & 0x0000000000FF0000) << 24) |
                  ((t.ui8 & 0x000000000000FF00) << 40) |
                  ((t.ui8 & 0x00000000000000FF) << 56);
#if DEBUG_JPEGBINARYWRITER
        Debug.Print(string.Format("* write_f8_be\t@\t0x{0:x8} : 0x{1:x8} ({2})", BaseStream.Position, t.ui8, t.f8));
#endif
        writer.Write(t.ui8);
      }
    }

    /// <summary>
    /// DOUBLE (64 bit) in write endian set by <see cref="WriteLittleEndian"/> member. |
    /// Writes a eight-byte float (Double) to the current stream and advances the stream position by eight bytes.
    /// </summary>
    public void write_f8_we(double value)
    {
      unchecked
      {
        DoubleAndUInt64Union t = new DoubleAndUInt64Union() { f8 = value };
        if (HWIsLittleEndian != WriteLittleEndian)
          t.ui8 = ((t.ui8 & 0xFF00000000000000) >> 56) |
                  ((t.ui8 & 0x00FF000000000000) >> 40) |
                  ((t.ui8 & 0x0000FF0000000000) >> 24) |
                  ((t.ui8 & 0x000000FF00000000) >> 08) |
                  ((t.ui8 & 0x00000000FF000000) << 08) |
                  ((t.ui8 & 0x0000000000FF0000) << 24) |
                  ((t.ui8 & 0x000000000000FF00) << 40) |
                  ((t.ui8 & 0x00000000000000FF) << 56);
#if DEBUG_JPEGBINARYWRITER
        Debug.Print(string.Format("* write_f8_we\t@\t0x{0:x8} : 0x{1:x8} ({2})", BaseStream.Position, t.ui8, t.f8));
#endif
        writer.Write(t.ui8);
      }
    }

    #endregion

  }

}
