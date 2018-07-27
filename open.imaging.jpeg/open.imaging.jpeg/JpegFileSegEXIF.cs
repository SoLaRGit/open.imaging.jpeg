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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Text;

namespace open.imaging.jpeg
{
  // JEITA CP-3451C CIPA DC-008-2012
  // ###############################
  //
  // 4.7.2 Interoperability Structure of APP1 in Compressed Data
  // ===========================================================
  //
  //  APP1 consists of an APP1 marker indicating that it is an application area, a length code indicating its
  //  size, and primary image attribute information patterned after the TIFF structure. The APP1 segment
  //  cannot record more than 64 KBytes, a limitation that shall be kept in mind when recording thumbnail
  //  images. APP1 also records attribute information for JPEG compressed images. The features of this
  //  marker segment are described below.
  //
  //  A. Order of APP1 recording
  //
  //      APP1 shall be recorded immediately after the SOI marker indicating the start of the file (see Figure 7).
  //
  //  B. APP1 Interoperability structure
  //
  //      APP1 consists of an APP1 marker, Exif ID code and the attribute information itself (see Figure 30).
  //
  //     Address | Offset | Code Meaning
  //    ---------------------------------------------------
  //      (Hex)  | (Hex)  |
  //    ---------------------------------------------------
  //      +00    | FF     | Marker Prefix
  //      +01    | E1     | APP1
  //      +02    |        | Length of field
  //      +04    | 45     | 'E'
  //      +05    | 78     | 'x'
  //      +06    | 69     | 'i'
  //      +07    | 66     | 'f'
  //      +08    | 00     | NULL
  //      +09    | 00     | Padding
  //      +0A    |        | Attribute information
  //    ---------------------------------------------------
  //     Figure 30 Basic Structure of APP1 Marker Segment
  //
  //  C. Exif ID code
  //
  //      The Exif ID code indicates that the APP1 segment Interoperability is Exif format. After a 4-Byte code,
  //      00.H is recorded in 2 Bytes. The reason for recording this code is to avoid duplication with other
  //      applications making use of JPEG application marker segments (APPn).
  //


  /// <summary>
  /// 
  /// </summary>
  [XmlRoot("segEXIF")]
  [XmlType("segEXIF")]
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDual)]
  public class JpegFileSegEXIF : JpegFileSeg
  {

    #region ### XML SERIALIZATION PROPERTIES ###

    [XmlAttribute(AttributeName = "sg")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_signature
    {
      get { return signature.to_hex_fmt(); }
      set { signature = value.hex_to_a_u1(); }
    }

    [XmlAttribute(AttributeName = "bo")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_byteOrder
    {
      get { return string.Format("{0:x4}", (ushort)byteOrder); }
      set { byteOrder = (IFDByteOrder)value.hex_to_u2(); }
    }

    [XmlAttribute(AttributeName = "an")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_arbitaryNumber
    {
      get { return string.Format("{0:x4}", (ushort)arbitaryNumber); }
      set { arbitaryNumber = value.hex_to_u2(); }
    }

    [XmlAttribute(AttributeName = "o1")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_OffsetTo1stIFDEntry
    {
      get { return string.Format("{0:x8}", OffsetTo1stIFDEntry); }
      set { OffsetTo1stIFDEntry = value.hex_to_u4(); }
    }

    [ComVisible(false)] // speeds watch window display
    [XmlElement("IFD1Thumb")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_IFD1Thumb
    {
      get { return IFD1Thumb.to_hex_fmt(wrapindex: 80); }
      set { IFD1Thumb = value.hex_to_a_u1(); }
    }

    #endregion

    [XmlIgnore]
    public byte[] signature;

    [XmlIgnore]
    public IFDByteOrder byteOrder;

    [XmlIgnore]
    public ushort arbitaryNumber;

    [XmlIgnore]
    public uint OffsetTo1stIFDEntry;

    //[XmlElement(ElementName = "IFD0")]
    //[XmlArray(ElementName = "IFD0")]
    //[XmlArrayItem(ElementName = "item")]
    public IFD IFD0;

    //[XmlElement(ElementName = "IFD1")]
    //[XmlArray(ElementName = "IFD1")]
    //[XmlArrayItem(ElementName = "item")]
    public IFD IFD1;

    [XmlIgnore]
    public byte[] IFD1Thumb;

    internal JpegFileSegEXIF()
    { }

    public JpegFileSegEXIF(JpegFile jpegFile, JpegFileSegMarker markerId)
      : base(jpegFile, markerId)
    { 
      // TODO: ... initialize data ...
    }

    public JpegFileSegEXIF(JpegFile jpegFile, JpegFileSegMarker markerId, JpegBinaryReader reader)
      : base(jpegFile, markerId, reader)
    {
      // must be in bigendian
      size = reader.read_u2_be();                           // read 2 bytes

      // advance address to the end of segment
      reader.Position = reader.Position - 2 + size;
    }

    public override bool Read(JpegBinaryReader reader)
    {
      // remmember reader endian
      bool ReadLittleEndian_cur = reader.ReadLittleEndian;  

      // restore position to its original position
      reader.Position = this._address;

      // segment size (or should we set segment address (+2 skip size)?)
      size = reader.read_u2_be();                           // read 2 bytes

      // read signature (TODO: need conformance tests with this field)
      signature = reader.read_u1(6);                        // read 6 bytes

      // need for reading offseted values
      long address_offset = reader.Position;

      // ### Image File Header - TIFF rev 6 standard ###
      byteOrder = (IFDByteOrder)reader.read_u2_re();        // read 2 bytes
      // set reader endian      
      reader.ReadLittleEndian = (byteOrder == IFDByteOrder.LittleEndian);
      arbitaryNumber = reader.read_u2_re();                 // read 2 bytes
      OffsetTo1stIFDEntry = reader.read_u4_re();            // read 4 bytes

      // read IFD              
      //Debug.Assert(OffsetTo1stIFDEntry == 0x00000008, "can't go beyond 0x8");
      if (OffsetTo1stIFDEntry != 0x00000008)
      {
        FireOnWarning(new JpegFileEventArgs(
          this._address,
          JpegFileEventId.WARNING_EXIFHDR_OFFSET_TO_1ST,
          string.Format("Uncommon OffsetTo1stIFDEntry value '0x{0:x4}', expected value '0x0008'.", OffsetTo1stIFDEntry)
          ));
      }
      // set offset to first IFD Entry    
      reader.Position = address_offset + (OffsetTo1stIFDEntry); 
      // read IFD
      try
      {
        if (size > reader.Position - _address)
        {
          IFD0 = new IFD(null);
          IFD0.OnError += new IFDEventDelegate(IFD_OnError);
          IFD0.OnWarning += new IFDEventDelegate(IFD_OnWarning);
          IFD0.Read(reader, address_offset, (size - 8));

          // NextIFDOffset
          uint NextIFDOffset = reader.read_u4_re();
          long address_tmp = reader.Position;
          try
          {
            if (NextIFDOffset >= 0x0000ffff)
            {
              // EXIF v2.3 - Figure 62 Conversion to Flashpix Extensions (2)
              // there is mention of data size (0xffffffff) that may be related to this
              FireOnError(new IFDEventArgs()
              {
                Address = address_tmp,
                Id = JpegFileEventId.ERROR_EXIFIFD1ST_READ,
                IFD = this.IFD1,
                Entry = null,
                Message = string.Format("NextIFDOffset == 0x{0,8:x8}", NextIFDOffset)
              });
              // by standard this should be '0' zero if there is no next IFD
              NextIFDOffset = 0; 
              // throw (new JpegFileReadSkipException());
            }
            else if (NextIFDOffset != 0)
            {
              reader.Position = address_offset + (long)NextIFDOffset;
              this.IFD1 = new IFD(null);
              this.IFD1.Read(reader, address_offset, (size - 8));

              address_tmp = reader.Position;

              int IFD1ThumbOffset = 0;
              int IFD1ThumbLength = 0;
              IFDEntries entries = this.IFD1.Query("$JPEGInterchangeFormat");
              if (entries.Count > 0)
              {
                IFD1ThumbOffset = entries[0].getSLONG();
                if (IFD1ThumbOffset > 0)
                {
                  reader.Position = address_offset + IFD1ThumbOffset;
                  entries = this.IFD1.Query("$JPEGInterchangeFormatLength");
                  if (entries.Count > 0)
                  {
                    IFD1ThumbLength = entries[0].getSLONG();
                  }
                  if (IFD1ThumbLength > 0)
                  {
                    this.IFD1Thumb = reader.read_u1(IFD1ThumbLength);
                  }
                }
              }
            }
            address_tmp = reader.Position;
          }
          catch (JpegFileReadSkipException /**/)
          { }
          catch (Exception ex)
          {
            FireOnError(new IFDEventArgs()
            {
              Address = address_tmp,
              Id = JpegFileEventId.ERROR_EXIFIFD1ST_READ,
              IFD = this.IFD1,
              Entry = null,
              Message = ex.Message
            });
          }
          finally
          {
            // restore reading position as attempt to recover reader
            reader.Position = address_tmp;
          }
          

        }
      }
      catch (Exception ex)
      {
        FireOnError(new JpegFileEventArgs( 
          this._address,
          JpegFileEventId.ERROR_EXIFIFD_READ,
          string.Format("JpegFileSegEXIF read error : {0}", ex) 
          ));
        Debug.Assert(false, ex.Message);
      }

      // restore previous endian
      reader.ReadLittleEndian = ReadLittleEndian_cur;       
      // advance address to the end of segment
      reader.Position = this._address + this.size;
      return true;
    }

    public override bool Read(JpegBinaryReader reader, long length)
    {
      return Read(reader);
    }

    void IFD_OnError(object sender, IFDEventArgs e)
    {
      FireOnError(e);
    }

    void IFD_OnWarning(object sender, IFDEventArgs e)
    {
      FireOnWarning(e);      
    }

    public override bool Write(JpegBinaryWriter writer)
    {
      // remmember size position
      long addr_entr = writer.Position;            
      // skip size (we will write it last)
      writer.Position += 2;                                 
      writer.write_u1(signature);                           // writes 6 bytes
      // take block offset address
      long addr_blck = writer.Position;                 
      // 0 - null entry
      long addr_offs = 0;                              
      // set correct ByteOrder endian value to be written in header
      byteOrder = (writer.WriteLittleEndian ? IFDByteOrder.LittleEndian : IFDByteOrder.BigEndian);
      // write header
      writer.write_u2_we((ushort)byteOrder);                // write 2 bytes
      writer.write_u2_we((ushort)arbitaryNumber);           // write 2 bytes
      writer.write_u4_we((uint)0x00000008);                 // write 8 bytes
      
      #region ### write IFD0 ###
      if (null != IFD0)
      {
        IFD0.Write(writer, addr_blck, ref addr_offs, 4);    // write N bytes
      }
      else
      {
        // write 0 count
        writer.write_u2_we((ushort)0x0000);                 // write 2 bytes
        addr_offs = writer.Position;
      }
      #endregion

      #region ### write IFD1 ###

      if (null != IFD1)
      {
        // write offset to NextIFD
        uint NextIFDOffset = (uint)(addr_offs - addr_blck);
        writer.write_u4_we(NextIFDOffset);                  // write 4 bytes
        // move to offset
        writer.Position = addr_offs;
        IFD1.Write(writer, addr_blck, ref addr_offs, 4);    // write N bytes
        // move writer position to end of last value.
        writer.Position = addr_offs;

        //// by standard: null it NextIFDSegment (IFD2?)
        //writer.write_u4_we((uint)0);                      // write 4 bytes
        //offset_address = writer.Position;

        if (IFD1Thumb != null && IFD1Thumb.Length > 0)
        {
          long thumb_address = writer.Position;
          int IFD1ThumbOffset = (int)(thumb_address - addr_blck);
          int IFD1ThumbLength = IFD1Thumb.Length;
          IFDEntries entries = this.IFD1.Query("$JPEGInterchangeFormat");
          if (entries.Count > 0)
          {
            entries[0].setSLONG(0, IFD1ThumbOffset);
            writer.Position = entries[0]._address;
            entries[0].Write(writer, addr_blck, ref addr_offs);
            writer.Position = thumb_address;
            if (IFD1ThumbOffset > 0)
            {
              entries = this.IFD1.Query("$JPEGInterchangeFormatLength");
              if (entries.Count > 0)
              {
                entries[0].setSLONG(0, IFD1ThumbLength);
                writer.Position = entries[0]._address;
                entries[0].Write(writer, addr_blck, ref addr_offs);
                writer.Position = thumb_address;
              }
              if (IFD1ThumbLength > 0)
              {
                writer.write_u1(IFD1Thumb);
              }
            }
          }
          addr_offs = writer.Position;
        }
      }
      else
      {
        // we do not have IFD1 pointer.
        // NextIFDPointer = NULL;
        writer.write_u4_we((uint)0);                        // write 4 bytes
      }      
      #endregion

      #region ### write block size ###
      // back to size position
      writer.Position = addr_entr;
      // calc entry size
      ushort size = (ushort)(addr_offs - addr_entr);  
      // must be bigendian
      writer.write_u2_be((ushort)size);                     // write 2 bytes
      #endregion

      // move writer position to end of last value.
      writer.Position = addr_offs;
      return true;          
    }

    public override string ToString()
    {
      return string.Format("JpegFileSegEXIF\t:\t{{ a: 0x{0,8:x8}, id: {1,-5} ({2,4:X4}.h), size: {3,5} (0x{4,4:x4}), entries:{5} }}",
                            _address,
                            MarkerId,
                            ((ushort)MarkerId),
                            size,
                            size,
                            (IFD0 != null ? IFD0.Count : -1));
    }

    public override string Dump()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine(this.ToString());

      if (IFD0 != null)
      {
        sb.AppendLine("IFD0:");
        sb.AppendLine(IFD0.Dump());
      }
      if (IFD1 != null)
      {
        sb.AppendLine("IFD1:");
        sb.AppendLine(IFD1.Dump());
      }
      return sb.ToString();
    }

  }

}
