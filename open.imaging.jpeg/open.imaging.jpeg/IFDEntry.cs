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
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using System.ComponentModel;

namespace open.imaging.jpeg
{

  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDual)]
  public class IFDEntry 
  {
    #region ### TIFF IFD ENTRY ###

    [XmlIgnore]
    public long _address;

    [XmlIgnore]
    public long _address_value;
    
    [XmlIgnore]
    public IFDTag tag;

    //debug purpose only
    [XmlIgnore]
    ushort tagNumber { get { return (ushort)tag; } }
    
    [XmlIgnore]
    public IFDType type;

    //debug purpose only
    [XmlIgnore]
    ushort typeNumber { get { return (ushort)type; } }

    /// <summary>Number of elements (not the size of value).</summary>
    [XmlIgnore]
    public uint count;

    /// <summary>
    /// Offset from TIFF header to a value it self is recorded. 
    /// In case where value is smaller than 4 bytes, the value it self is recorded.
    /// depending on the format of machine creating this (big endian, small endian).
    /// </summary>
    [XmlIgnore]
    public uint _value_offset;

    /// <summary>IFD entry value bytes</summary>
    [XmlIgnore]
    public byte[] data;

    #region ### XML SERIALIZATION ###

    [XmlAttribute("a")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_address
    {
      get { return string.Format("{0:x8}", (ulong)_address); }
      set { _address = value.hex_to_i8(); }
    }

    [XmlAttribute("av")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_address_value
    {
      get
      {
        // more readable without
        // if (_address_value == default(long)) return null;
        return string.Format("{0:x8}", (ulong)_address_value);
      }
      set { _address_value = value.hex_to_i8(); }
    }

    [XmlAttribute("g")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_tag
    {
      get { return string.Format("{0:x4}", (ushort)tag); }
      set { tag = (IFDTag)value.hex_to_u2(); }
    }

    [XmlAttribute("t")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_type
    {
      get { return string.Format("{0:x4}", (ushort)type); }
      set { type = (IFDType)value.hex_to_u2(); }
    }

    [XmlAttribute("c")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_count
    {
      get { return string.Format("{0:x8}", (uint)count); }
      set { count = value.hex_to_u4(); }
    }

    [XmlAttribute("o")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_value_offset
    {
      get 
      {
        // more readable without
        // if (_value_offset == default(uint)) return null;
        return string.Format("{0:x8}", (uint)_value_offset); 
      }
      set { _value_offset = value.hex_to_u4(); }
    }

    [XmlAttribute("tn")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_type_name
    {
      get { return string.Format("{0,-9}", type.ToString()); }
      set { /* ignore */ }
    }

    [XmlAttribute("gn")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_tag_name
    {
      get { return string.Format("{0,-24}", tag.ToString()); }
      set { /* ignore */ }
    }

    [ComVisible(false)] // speeds watch window display
    [XmlAttribute("data")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_data
    {
      get { return data.to_hex_fmt(); }
      set { data = value.hex_to_a_u1(); }
    }

    [ComVisible(false)] // speeds watch window display
    [XmlAttribute("sdata")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_sdata
    {
      get 
      {
        if (count > 0)
        {
          switch (this.type)
          {
            case IFDType.ASCII: 
              return getString();
            default:
              if (this.count < 256)
                return getValueString();
              break;
          }
        }
        return null;
      }
      set { /* ignore */ }
    }

    #endregion

    #endregion

    [XmlIgnore]
    public readonly IFD ParentIFD;

    //[XmlArray(ElementName = "IFD")]
    //[XmlArrayItem(ElementName = "item")]
    public IFD IFD;

    //public static implicit operator IFD(IFDEntry entry)
    //{
    //  return entry.IFD;
    //}

    public IFDEntry()
    { }

    public IFDEntry(IFD ParentIFD = null)
    {
      this.ParentIFD = ParentIFD;
    }

    private const int SIZE_SHORT = sizeof(ushort);
    private const int SIZE_SSHORT = sizeof(short);
    private const int SIZE_LONG = sizeof(uint);
    private const int SIZE_SLONG = sizeof(int);
    private const int SIZE_RATIONAL = sizeof(uint) << 1;
    private const int SIZE_SRATIONAL = sizeof(uint) << 1;
    private const int SIZE_DOUBLE = sizeof(double);
    private const int SIZE_FLOAT = sizeof(float);

    /// <summary>returns size of single element</summary>
    public int elementSize()
    {
      switch (type)
      {
        case IFDType.BYTE:
        case IFDType.SBYTE:
        case IFDType.ASCII:
        case IFDType.UNDEFINED: 
          return 1;
        case IFDType.SHORT:
        case IFDType.SSHORT:
          return SIZE_SHORT;
        case IFDType.LONG: 
        case IFDType.SLONG:
        case IFDType.FLOAT:
        case IFDType.IFD:
          return SIZE_LONG;
        case IFDType.DOUBLE: 
        case IFDType.RATIONAL: 
        case IFDType.SRATIONAL:
          return SIZE_RATIONAL;        
        default:
          // ouch, someone wants to tell us there is new new new new TYPE?
          // ... all tags must be ordered by tag (ushort) value order ...  
          // and now how can we skip it if we do not know what sizeof(TYPE) is?          
          // dudes EXIF standard sucks multiple times on multiple levels.
          // someone needs to make some rules in this orderly disorder.
          //Debug.Assert(false);
          return 1;
      }  
    }

    /// <summary>returns converted value string from data elements (with detection is string unicode or utf8)</summary>
    public string getString()
    {
      // encoding detection: unicode
      int nullindex = 0;      
      if (count > 2 && data[1] == 0x00)
      {
        nullindex = Array.IndexOf<byte>(data, 0x00);
        while (data.Length - 1 > nullindex + 1 && data[nullindex + 1] != 0x00)
        {
          nullindex = Array.IndexOf<byte>(data, 0x00, nullindex + 1);
        }
        return System.Text.UnicodeEncoding.Unicode.GetString(data, 0, (int)nullindex);
      }
      nullindex = Array.IndexOf<byte>(data, 0x00);
      return System.Text.UTF8Encoding.UTF8.GetString(data, 0, (int)nullindex);
    }

    /// <summary>returns converted value array from data elements</summary>
    public T[] ToArray<T>() where T : struct
    { 
      Type TT = typeof(T);
      int TTSIZE = Marshal.SizeOf(TT);
      int TTCOUNT = this.data.Length / TTSIZE;
      T[] returnValue = new T[TTCOUNT];
      Buffer.BlockCopy(this.data, 0, returnValue, 0, TTCOUNT);
      return returnValue;
    }

    /// <summary>returns converted value (2 bytes) from data elements at index location</summary>
    public ushort getSHORT(int index = 0)
    {
      return BitConverter.ToUInt16(data, index * SIZE_SHORT);
    }

    /// <summary>returns converted value (2 bytes) from data elements at index location</summary>
    public short getSSHORT(int index = 0)
    {
      return BitConverter.ToInt16(data, index * SIZE_SSHORT);
    }

    /// <summary>returns converted value (4 bytes) from data elements at index location</summary>
    public uint getLONG(int index = 0)
    {
      return BitConverter.ToUInt32(data, index * SIZE_LONG);
    }

    /// <summary>returns converted value (4 bytes) from data elements at index location</summary>
    public int getSLONG(int index = 0)
    {
      return BitConverter.ToInt32(data, index * SIZE_SLONG);
    }

    /// <summary>returns converted value (8 bytes) from data elements at index location</summary>
    public IFDTypeRATIONAL getRATIONAL(int index = 0)
    {
      int vindex = index * SIZE_RATIONAL;
      IFDTypeRATIONAL t = new IFDTypeRATIONAL();
      t.ui4_1 = BitConverter.ToUInt32(data, vindex);
      t.ui4_2 = BitConverter.ToUInt32(data, vindex + 4);
      return t;
    }

    /// <summary>returns converted value (8 bytes) from data elements at index location</summary>
    public IFDTypeSRATIONAL getSRATIONAL(int index = 0)
    {
      int vindex = index * SIZE_SRATIONAL;
      IFDTypeSRATIONAL t = new IFDTypeSRATIONAL();
      t.i4_1 = BitConverter.ToInt32(data, vindex);
      t.i4_2 = BitConverter.ToInt32(data, vindex + 4);
      return t;
    }

    /// <summary>
    /// TODO: not implemented generic get.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index"></param>
    /// <returns></returns>
    public T get<T>(int index = 0)      
      where T : struct
    {
      Type TT = typeof(T);

      throw new NotImplementedException("TODO: not implemented generic get<T>");

      //return default(T);
    }

    /// <summary>returns converted value (8 bytes) from data elements at index location</summary>
    public double getDOUBLE(int index = 0)
    {
      return BitConverter.ToDouble(data, index * SIZE_DOUBLE);
    }

    /// <summary>returns converted value (4 bytes) from data elements at index location</summary>
    public float getFLOAT(int index = 0)
    {
      return BitConverter.ToSingle(data, index * SIZE_FLOAT);
    }

    /// <summary>
    /// Returns converted value (n bytes depending on size of element) from data elements at index position.
    /// <remarks>Passing -1 for index will return all elements up to MAX_BYTES divided with size of element.</remarks>
    /// </summary>    
    public string getValueString(int index = 0)
    {
      const int MAX_BYTES = 64;

      IFDType _type = type;
      if (this.IFD != null) _type = IFDType.IFD;
      switch (_type)
      {
        case IFDType.ASCII: return data.to_str() + " {" + data.to_hex(MAX_BYTES) + "}";
        case IFDType.BYTE: return data.to_hex(MAX_BYTES);
        case IFDType.SBYTE: return data.to_hex(MAX_BYTES);
        case IFDType.UNDEFINED: return data.to_hex(MAX_BYTES);
        case IFDType.SHORT:
          {
            if (index == -1)
            {
              StringBuilder sb = new StringBuilder();
              int icount = (int)(this.count < (MAX_BYTES / SIZE_SHORT) ? this.count : (MAX_BYTES / SIZE_SHORT));
              if (icount > this.data.Length / SIZE_SHORT) icount = this.data.Length / SIZE_SHORT;
              for (int i = 0; i < icount; i++)
                sb.Append(string.Format("{0:X4} ", getSHORT(i)));
              if (icount < this.count) sb.Append("...");
              return sb.ToString();
            }
            return getSHORT(index).ToString();
          }
        case IFDType.SSHORT:
          {
            if (index == -1)
            {
              StringBuilder sb = new StringBuilder();
              int icount = (int)(this.count < (MAX_BYTES / SIZE_SSHORT) ? this.count : (MAX_BYTES / SIZE_SSHORT));
              if (icount > this.data.Length / SIZE_SSHORT) icount = this.data.Length / SIZE_SSHORT;
              for (int i = 0; i < icount; i++)
                sb.Append(string.Format("{0:X4} ", getSSHORT(i)));
              if (icount < this.count) sb.Append("...");
              return sb.ToString();
            }
            return getSSHORT(index).ToString();
          }
        case IFDType.LONG:
          {
            if (index == -1)
            {
              StringBuilder sb = new StringBuilder();
              int icount = (int)(this.count < (MAX_BYTES / SIZE_LONG) ? this.count : (MAX_BYTES / SIZE_LONG));
              if (icount > this.data.Length / SIZE_LONG) icount = this.data.Length / SIZE_LONG;
              for (int i = 0; i < icount; i++)
                sb.Append(string.Format("{0:X8} ", getLONG(i)));
              if (icount < this.count) sb.Append("...");
              return sb.ToString();
            }
            return getLONG(index).ToString();
          }
        case IFDType.SLONG:
          {
            if (index == -1)
            {
              StringBuilder sb = new StringBuilder();
              int icount = (int)(this.count < (MAX_BYTES / SIZE_SLONG) ? this.count : (MAX_BYTES / SIZE_SLONG));
              if (icount > this.data.Length / SIZE_SLONG) icount = this.data.Length / SIZE_SLONG;
              for (int i = 0; i < icount; i++)
                sb.Append(string.Format("{0:X8} ", getSLONG(i)));
              if (icount < this.count) sb.Append("...");
              return sb.ToString();
            }
            return getSLONG(index).ToString();
          }
        case IFDType.RATIONAL:
          {
            if (index == -1)
            {
              StringBuilder sb = new StringBuilder();
              int icount = (int)(this.count < (MAX_BYTES / SIZE_RATIONAL) ? this.count : (MAX_BYTES / SIZE_RATIONAL));
              if (icount > this.data.Length / SIZE_RATIONAL) icount = this.data.Length / SIZE_RATIONAL;
              for (int i = 0; i < icount; i++)
              {
                IFDTypeRATIONAL t = getRATIONAL(i);
                sb.Append(string.Format("{0:X8}.{1:X8} ", t.ui4_1, t.ui4_2));
              }
              if (icount < this.count) sb.Append("...");
              return sb.ToString();
            }
            return getRATIONAL(index).ToString();
          }
        case IFDType.SRATIONAL:
          {
            if (index == -1)
            {
              StringBuilder sb = new StringBuilder();
              int icount = (int)(this.count < (MAX_BYTES / SIZE_SRATIONAL) ? this.count : (MAX_BYTES / SIZE_SRATIONAL));
              if (icount > this.data.Length / SIZE_SRATIONAL) icount = this.data.Length / SIZE_SRATIONAL;
              for (int i = 0; i < icount; i++)
              {
                IFDTypeSRATIONAL t = getSRATIONAL(i);
                sb.Append(string.Format("{0:X8}.{1:X8} ", t.i4_1, t.i4_2));
              }
              if (icount < this.count) sb.Append("...");
              return sb.ToString();
            }
            return getSRATIONAL(index).ToString();
          }
        case IFDType.DOUBLE:
          {
            if (index == -1)
            {
              StringBuilder sb = new StringBuilder();
              int icount = (int)(this.count < (MAX_BYTES / SIZE_DOUBLE) ? this.count : (MAX_BYTES / SIZE_DOUBLE));
              if (icount > this.data.Length / SIZE_DOUBLE) icount = this.data.Length / SIZE_DOUBLE;
              for (int i = 0; i < icount; i++)
              {                
                sb.Append(string.Format("{0:X16} ", getDOUBLE(i)));
              }
              if (icount < this.count) sb.Append("...");
              return sb.ToString();
            }
            return getDOUBLE(index).ToString();
          }          
        case IFDType.FLOAT:
          {
            if (index == -1)
            {
              StringBuilder sb = new StringBuilder();
              int icount = (int)(this.count < (MAX_BYTES / SIZE_FLOAT) ? this.count : (MAX_BYTES / SIZE_FLOAT));
              if (icount > this.data.Length / SIZE_FLOAT) icount = this.data.Length / SIZE_FLOAT;
              for (int i = 0; i < icount; i++)
              {                
                sb.Append(string.Format("{0:X8} ", getFLOAT(i)));
              }
              if (icount < this.count) sb.Append("...");
              return sb.ToString();
            }
            return getFLOAT(index).ToString();
          }
        case IFDType.IFD:
        //case IFDType.TYPE_00:
          {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("\t\tItems[{0}]", this.IFD.Count));
            sb.AppendLine("\t\t[");
            for(int i = 0; i < this.IFD.Count; i++)
            {
              IFDEntry entry = this.IFD[i];
              sb.AppendLine(string.Format("\t\t[{0}]: {1}", i, entry.ToString()));
            }
            sb.AppendLine("\t\t]");
            return sb.ToString();
          }
      }
      return "<unknown.type>";
    }

    /// <summary>extends data array to fit new elements, if data array to small</summary>
    void _setIndex(int index, int elementSize)
    {
      unchecked
      {
        if (index > count + 1)
        {
          this.count = (ushort)(index + 1);
          int byteCount = (int)(count * elementSize);
          Array.Resize<byte>(ref data, byteCount);
        }
      }
    }

    /// <summary>sets data elements at index location with value bytes (2 bytes)</summary>
    public void setSHORT(int index, ushort value)
    {
      _setIndex(index, SIZE_SHORT);
      byte[] b = BitConverter.GetBytes(value);
      Buffer.BlockCopy(b, 0, data, index << 1, SIZE_SHORT);
    }

    /// <summary>sets data elements at index location with value bytes (2 bytes)</summary>
    public void setSSHORT(int index, short value)
    {
      _setIndex(index, SIZE_SSHORT);
      byte[] b = BitConverter.GetBytes(value);
      Buffer.BlockCopy(b, 0, data, index << 1, SIZE_SHORT);
    }

    /// <summary>sets data elements at index location with value bytes (4 bytes)</summary>
    public void setLONG(int index, uint value)
    {
      _setIndex(index, SIZE_LONG);
      byte[] tmp = BitConverter.GetBytes(value);
      Buffer.BlockCopy(tmp, 0, data, index << 2, SIZE_LONG);
    }

    /// <summary>sets data elements at index location with value bytes (4 bytes)</summary>
    public void setSLONG(int index, int value)
    {
      _setIndex(index, SIZE_SLONG);
      byte[] tmp = BitConverter.GetBytes(value);
      Buffer.BlockCopy(tmp, 0, data, index << 2, SIZE_SLONG);
    }

    /// <summary>sets data elements at index location with value bytes (4 bytes)</summary>
    public void setFLOAT(int index, float value)
    {
      _setIndex(index, SIZE_FLOAT);
      byte[] b = BitConverter.GetBytes(value);
      Buffer.BlockCopy(b, 0, data, index << 2, SIZE_FLOAT);
    }

    /// <summary>sets data elements at index location with value bytes (8 bytes)</summary>
    public void setDOUBLE(int index, double value)
    {
      _setIndex(index, SIZE_DOUBLE);
      byte[] b = BitConverter.GetBytes(value);
      Buffer.BlockCopy(b, 0, data, index << 3, SIZE_DOUBLE);
    }

    /// <summary>sets data elements at index location with value bytes (8 bytes)</summary>
    public void setRATIONAL(int index, IFDTypeRATIONAL value)
    {
      _setIndex(index, SIZE_RATIONAL);
      byte[] b = BitConverter.GetBytes(value.ui4_1);
      Buffer.BlockCopy(b, 0, data, index << 3, SIZE_LONG);
      b = BitConverter.GetBytes(value.ui4_2);
      Buffer.BlockCopy(b, 0, data, (index << 3) + SIZE_LONG, SIZE_LONG);
    }

    /// <summary>sets data elements at index location with value bytes (8 bytes)</summary>
    public void setSRATIONAL(int index, IFDTypeSRATIONAL value)
    {
      _setIndex(index, SIZE_SRATIONAL);
      byte[] b = BitConverter.GetBytes(value.i4_1);
      Buffer.BlockCopy(b, 0, data, index << 3, SIZE_SLONG);
      b = BitConverter.GetBytes(value.i4_2);
      Buffer.BlockCopy(b, 0, data, (index << 3) + SIZE_SLONG, SIZE_SLONG);
    }

    /// <summary>
    /// Sets data elements depending on entry type.
    /// <remarks>
    /// Not very generic way, we must take in account correct type conversion.
    /// (TODO: this generic function needs more work, like validation of types and counts...)
    /// </remarks>
    /// </summary>
    /// <param name="index">index of element to set</param>
    /// <param name="value">element value to set that must be of correct entry type</param>
    public void set(int index, object value)
    {
      switch (type)
      {
        case IFDType.ASCII:
        case IFDType.BYTE:
        case IFDType.SBYTE:
        case IFDType.UNDEFINED:
          data[index] = (byte)value; 
          return;
        case IFDType.SHORT: 
          setSHORT(index, unchecked((ushort)value)); 
          return;
        case IFDType.SSHORT: 
          setSSHORT(index, unchecked((short)value)); 
          return;
        case IFDType.LONG: 
          setLONG(index, unchecked((uint)value)); 
          return;
        case IFDType.SLONG: 
          setSLONG(index, unchecked((int)value)); 
          return;
        case IFDType.FLOAT:
          setFLOAT(index, unchecked((float)value));
          return;
        case IFDType.DOUBLE: 
          setDOUBLE(index, unchecked((double)value)); 
          return;
        case IFDType.RATIONAL: 
          setRATIONAL(index, (IFDTypeRATIONAL)value);
          return;
        case IFDType.SRATIONAL: 
          setSRATIONAL(index, (IFDTypeSRATIONAL)value); 
          return;
      }
    }

#if DEBUG
    /// <summary>
    /// reads single IFD entry
    /// </summary>    
    /// <param name="reader">reader to read from</param>
    /// <param name="in_block_address">starting address of IFD entry</param>
    /// <param name="in_block_size">size of IFD entry</param>
    public void Read(JpegBinaryReader reader, long in_block_address, long in_block_size)
    {
      // make sure no errors were made writting to this values ...
      ReadOnly_T<long> block_address = new ReadOnly_T<long>(in_block_address);
      ReadOnly_T<long> block_size = new ReadOnly_T<long>(in_block_size);      
#else
    /// <summary>
    /// reads single IFD entry
    /// </summary>
    /// <param name="reader">reader to read from</param>
    /// <param name="block_address">starting address of IFD entry</param>
    /// <param name="block_size">size of IFD entry</param>
    public void Read(JpegBinaryReader reader, long block_address, long block_size)
    {
#endif
      //IFDEntry entry = this;
      // start address of the block
      this._address = reader.Position;
      
      //-BLOCK-DATA-------------------------------------------------------------------
      this.tag = (IFDTag)reader.read_u2_re();             // read 2 bytes      
      this.type = (IFDType)reader.read_u2_re();           // read 2 bytes
      this.count = reader.read_u4_re();                   // read 4 bytes
      this._value_offset = reader.read_u4_re();           // read 4 bytes
      //------------------------------------------------------------------------------
      // remmember previous position to next entry
      long address_next = reader.Position;

      int byteCount = unchecked((int)(count * elementSize()));      
      if (byteCount <= 4)
      {
        // read offset as value
        #region ### Convert voffset to byte array as value ###
        switch (this.type)
        {
          case IFDType.FLOAT:
            {
              this.data = BitConverter.GetBytes(this._value_offset);
              // LE => conv_BE + conv_BE == LE (idiots)
              //this.data = reader.conv_u1_re(reader.conv_u1_re(this.data, 4), 4);
            }
            break;

          case IFDType.LONG:
          case IFDType.SLONG:
            {
              this.data = BitConverter.GetBytes(this._value_offset);
              // LE => conv_BE + conv_BE == LE (idiots)
              //this.data = reader.conv_u1_re(reader.conv_u1_re(this.data, 4), 4);
            }
            break;

          case IFDType.SSHORT: // read as 4 bytes anyway (buggy Big Endian == NON ENDIAN)
          case IFDType.SHORT:  // read as 4 bytes anyway (buggy Big Endian == NON ENDIAN)
            {
              this.data = BitConverter.GetBytes(this._value_offset);
              // IDIOTIC ^ 3
              // EXIF big endian value of 1 == 0x00010000
              // UINT -> LITTLE ENDIAN      == 0x00000100
              // DATA IS IN BIG ENDIAN once again (magicaly)
              // USHORT[0+1] -> LITTLE END. == 0x00000001

              // to be more exact:
              // correct intepretation of hex numbers in order with 2 HEX USHORT values
              // little endian: 0x3412 0x7856 == { 0x12, 0x34, 0x56, 0x78 } 
              // big endian:    0x3412 0x7856 == { 0x34, 0x12, 0x78, 0x56 } 
              // everything by the book (T)
              // but written in EXIF big endian: { 0x56, 0x78, 0x12, 0x34 }
              // everything not by the book (Fail!!!)
              //
              // endiness of each type short swapped bytes (correct big endian)
              // but all bytes reversed (hello DOIK buy new brain).
              // this is equal to little endian 2 shorts swapped places.
              // there is no Big Endian here, also out of order data...
              // not only that we do not have more than 1 SHORT in most cases,
              // but this is not how big endian is intepreted.
              // resulting in total mess of NON ENDINESS value!
              //
              // I would says someone missed the point of BigEdian 
              // when going to computer science classes.
              // I would fire this asshole, and post on internet idiotic
              // thing he did so no one would ever dream to hire him to 
              // do anything computer related.
              //
              
              // data -> our endian (double conversion : in groups of 4 then in groups by 2)
              this.data = reader.conv_u1_re(reader.conv_u1_re(this.data, 4), 2);
            }
            break;

          case IFDType.ASCII:          
          case IFDType.BYTE:
          case IFDType.SBYTE:
          case IFDType.UNDEFINED:
          case IFDType.IFD:
          //case IFDType.TYPE_00:
          default:
            {
              // endian should not play role here 
              // (thanks god they didn't mess up byte array somehow).
              int elementCount = unchecked((int)this.count);
              if (elementCount >= 0)
              {
                this.data = BitConverter.GetBytes(this._value_offset);                
                // resize data to be of offset size
                Array.Resize<byte>(ref this.data, 4);
              }
            }
            break;    
        }
        #endregion        
      }
      else
      {
        // value is at offset (jump trough position hoops) ...   
        // calc value address position
        this._address_value = block_address + this._value_offset;
        // set offset position for reading value
        reader.Position = this._address_value;      
        #region ### handle endian reading ###
        switch (this.type)
        {
          case IFDType.DOUBLE:
            this.data = reader.read_u1_re((int)this.count, 8);
            break;
          case IFDType.FLOAT:
            this.data = reader.read_u1_re((int)this.count, 4);
            break;
          case IFDType.RATIONAL:
          case IFDType.SRATIONAL:
            this.data = reader.read_u1_re((int)this.count << 1, 4);
            break;
          case IFDType.LONG:
          case IFDType.SLONG:
            this.data = reader.read_u1_re((int)this.count, 4);
            break;
          case IFDType.SHORT:
          case IFDType.SSHORT:
            this.data = reader.read_u1_re((int)this.count, 2);
            break;
          case IFDType.ASCII:
          case IFDType.BYTE:
          case IFDType.SBYTE:
          case IFDType.UNDEFINED:
          case IFDType.IFD:
          //case IFDType.TYPE_00:
          default:
            this.data = reader.read_u1_re((int)this.count, 1);
            break;
        }
        #endregion        
      }

      // and to have even more complicated things we have read recursion for pointer blocks             
      #region ### READ IFD POINTER BLOCKS ###
      //
      // TODO: not sure how to handle when vcount > 1,
      //       is it even possible ?
      //
      switch (this.tag)
      {
        case IFDTag.IFDExifPointer:
        case IFDTag.IFDGPSInfoPointer:
        case IFDTag.IFDInteropPointer:
        // case IFDTag.MakerNote: // bad idea, since manufacturers can generate whatever they like ...
          {
            long address_tmp = reader.Position;
            try
            {
              reader.Position = block_address + this._value_offset;
              this.IFD = new IFD(null, this);
              this.IFD.Read(reader, block_address, block_size);
            }
            catch (Exception ex)
            {
              if (ParentIFD != null)
                ParentIFD.FireOnError(new IFDEventArgs()
                  {
                    Address = address_tmp,
                    Id = JpegFileEventId.ERROR_EXIFIFD_READ,
                    IFD = ParentIFD,
                    Entry = this,
                    Message = ex.Message
                  });
            }
            finally
            {
              // restore reading position as attempt to recover reader
              reader.Position = address_tmp;
            }
          }
          break;       
      }
      #endregion  

      // restore previous position to next entry 
      reader.Position = address_next;       
    }

    /// <summary>
    /// Writes entry at the current address location, and offsets value data to address_offset_value.    
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="addr_blck"></param>
    /// <param name="addr_offs"></param>
#if DEBUG
    public void Write(JpegBinaryWriter writer, long in_addr_blck, ref long addr_offs)
    {
      // make sure: addr_blck must not be touched by this function.
      ReadOnly_T<long> addr_blck = new ReadOnly_T<long>(in_addr_blck);
#else
    public void Write(JpegBinaryWriter writer, long addr_blck, ref long addr_offs)
    {
#endif      

      writer.write_u2_we((ushort)this.tag);                     // writes 2 bytes - tag
      writer.write_u2_we((ushort)this.type);                    // writes 2 bytes - type        
      writer.write_u4_we((uint)this.count);                     // writes 4 bytes - number of elements

      // remmember next entry address
      long addr_next = writer.Position + 4;

      int byteCount = unchecked((int)(count * elementSize()));
      if (this.IFD != null)
      {
        #region ### (sizeof(TT) > 4) write offset+IFD ###
        // calc real offset for this block
        uint offset = (uint)(addr_offs - addr_blck);
        // offset to value from this header offset
        writer.write_u4_we((uint)offset);                       // writes 4 bytes
        // set position for value write
        writer.Position = addr_offs;
        // write value (SubIFDs Entries)
        IFD.Write(writer, addr_blck, ref addr_offs, 4);         // writes N bytes
        // check if anything was written at offset value
        if (writer.Position < addr_offs)
        {
          // move writer position to end of last value.
          writer.Position = addr_offs;
        }
        #endregion
      }
      else
      if (byteCount > 4)
      {
        #region ### (sizeof(TT) > 4) write offset+value ###
        // calc real offset for this block
        uint offset = (uint)(addr_offs - addr_blck);
        // offset to value from this header offset
        writer.write_u4_we((uint)offset);                       // writes 4 bytes        
        // set position for value write
        writer.Position = addr_offs;
        // write value by type
        switch (this.type)
        {
          case IFDType.DOUBLE:
            writer.write_u1_we(this.data, 8);                 // writes (count) * 8 bytes
            break;
          case IFDType.FLOAT:
            writer.write_u1_we(this.data, 4);                 // writes count * 4 bytes
            break;
          case IFDType.RATIONAL:
          case IFDType.SRATIONAL:
            writer.write_u1_we(this.data, 4);                 // writes (count << 1) * 4 bytes
            break;
          case IFDType.LONG:
          case IFDType.SLONG:
            writer.write_u1_we(this.data, 4);                 // writes count * 4 bytes
            break;
          case IFDType.SHORT:
          case IFDType.SSHORT:            
            writer.write_u1_we(this.data, 2);                 // writes count * 2 bytes
            break;
          case IFDType.ASCII:
          case IFDType.BYTE:
          case IFDType.SBYTE:
          case IFDType.UNDEFINED:           
          case IFDType.IFD:
          //case IFDType.TYPE_00:
          default:
            writer.write_u1(data);                            // writes count bytes
            break;
        }          
        // update offset value pointer to next
        addr_offs = writer.Position;
        #endregion
      }
      else
      {
        #region ### (sizeof(TT) <= 4) write value instead (in place) of offset ###
        switch (this.type)
        {
          case IFDType.FLOAT:   // todo: check how float is written in different endians
            writer.write_u1_we(data, 4);
            break;
          case IFDType.LONG:
          case IFDType.SLONG:
            writer.write_u1_we(data, 4);
            break;
          case IFDType.SHORT:   // NON ENDIAN (not Little Endian, not Big Endian) 
          case IFDType.SSHORT:
            // NOTICE: check reader of same offset as value how to...
            writer.write_u1_we(writer.conv_u1_we(this.data, 2), 4);
            break;
          case IFDType.ASCII:
          case IFDType.BYTE:
          case IFDType.SBYTE:
          case IFDType.UNDEFINED:
          default:
            Array.Resize<byte>(ref this.data, 4);
            writer.write_u1(data);
            break;
        }
        #endregion
      }
      // restore next entry address
      writer.Position = addr_next;
    }

    public override string ToString()
    {
      if (this.IFD != null)
      {
        return string.Format("[a:{0:x8}] {{ ({1:X4}.H) {2,-24} : {3,-15} [0x{4:X8}] ((IFD*)[0x{4:X8}]):\r\n{5}\t}}", _address, ((ushort)tag), tag, "" + type + "[" + count + "]", _value_offset, getValueString(-1));
        //return "{ tag(0x" + ((ushort)tag).ToString("X4") + "): " + tag + ",\ttype:" + type + "[" + vcount + "],\tvalue:\r\n" + get() + "\t}";
      }
      return string.Format("[a:{0:x8}] {{ ({1:X4}.H) {2,-24} : {3,-15} [0x{4:X8}] '{5}' }}", _address, ((ushort)tag), tag, "" + type + "[" + count + "]", _value_offset, getValueString(-1));
      //return "{ tag(0x" + ((ushort)tag).ToString("X4") + "): " + tag + ",\ttype:" + type + "[" + vcount + "],\tvalue: '" + get() + "'\t}";
    }
  }
}
