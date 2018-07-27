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
using System.Xml.Serialization;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace open.imaging.jpeg
{

  // ISO/IEC 10918-5:2012 (E) 6 Rec. ITU-T T.871 (05/2011)
  // JPEG File Interchange Format (JFIF) specification

  // 10.1 JFIF file syntax
  // The syntax of a JFIF file shall conform to the syntax for the interchange format as specified in Annex B of
  // Rec. ITU-T T.81 | ISO/IEC 10918-1. In addition, a JFIF file shall contain APP0 marker segments with certain parameter
  // constraints in the frame header as specified below. Values are represented using unsigned binary representations except
  // as otherwise specified. X'nn' indicates a byte value in hexadecimal notation. Fields that follow the Lp parameter as
  // specified below shall appear in the sequence listed below in the application data bytes Api of the JFIF APP0 marker
  // segment.
  
  /// <summary>
  /// APP0 segment in compliance to JFIF standard
  /// </summary>

  [XmlRoot("segJFIF")]
  [XmlType("segJFIF")]
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDual)]
  public class JpegFileSegJFIF : JpegFileSeg
  {
    #region ### XML SERIALIZATION PROPERTIES ###

    [XmlAttribute(AttributeName = "sg")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_signature
    {
      get { return signature.to_hex_fmt(); }
      set { signature = value.hex_to_a_u1(); }
    }

    [XmlAttribute(AttributeName = "ver")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_version
    {
      get { return string.Format("{0:x4}", (ushort)version); }
      set { version = value.hex_to_u2(); }
    }

    [XmlAttribute(AttributeName = "iu")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_image_units
    {
      get { return string.Format("{0:x2}", (byte)image_units); }
      set { image_units = value.hex_to_u1(); }
    }

    [XmlAttribute(AttributeName = "iw")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_image_width
    {
      get { return string.Format("{0:x4}", image_width); }
      set { image_width = value.hex_to_u2(); }
    }

    [XmlAttribute(AttributeName = "ih")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_image_height
    {
      get { return string.Format("{0:x4}", image_height); }
      set { image_height = value.hex_to_u2(); }
    }

    [XmlAttribute(AttributeName = "tw")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_thumb_width
    {
      get { return string.Format("{0:x2}", (byte)thumb_width); }
      set { thumb_width = value.hex_to_u1(); }
    }

    [XmlAttribute(AttributeName = "th")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_thumb_height
    {
      get { return string.Format("{0:x2}", (byte)thumb_height); }
      set { thumb_height = value.hex_to_u1(); }
    }

    [ComVisible(false)] // speeds watch window display
    [XmlElement("ThumbData")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_thumb_data
    {
      get { return thumb_data.to_hex_fmt(wrapindex: 80); }
      set { thumb_data = value.hex_to_a_u1(); }
    }

    #endregion

    /// <summary>
    /// = X'4A', X'46', X'49', X'46', X'00' This zero-terminated string ("JFIF", according to
    /// Rec. ITU-T T.50 or ISO 646 coding) identifies the JFIF APP0 marker.
    /// </summary>
    [XmlIgnore]
    public byte[] signature;      // X'4A,46,49,46,00' 5 bytes

    /// <summary>
    /// The most significant byte is used for major revisions, the least significant byte for minor revisions. 
    /// Files encoded according to this Recommendation | International Standard shall use the value X'0102' 
    /// (corresponding to version 1.02).
    /// </summary>
    [XmlIgnore]
    public ushort version;        // X'0102' == 1.02 version

    /// <summary>
    /// Units for the H (horizontal) and V (vertical) densities:
    /// = X'00': units unspecified; H and V densities, expressed in dots per arbitrary unit, specify only 
    ///          the pixel aspect ratio (width:height pixel aspect ratio = Vdensity:Hdensity).
    /// = X'01': H and V densities are dots per inch (dots per 2.54 cm).
    /// = X'02': H and V densities are dots per cm.
    /// </summary>
    [XmlIgnore]
    public byte image_units;          // X'00' - unspecified, X'01' - DPI (dots/2.54cm),  X'01' - DPC (dots/1cm)

    /// <summary>
    /// Horizontal pixel density. Must be non-zero.
    /// </summary>
    [XmlIgnore]
    public ushort image_width;        // !0

    /// <summary>
    /// Vertical pixel density. Must be non-zero.
    /// </summary>
    [XmlIgnore]
    public ushort image_height;       // !0

    /// <summary>
    /// Horizontal thumbnail pixel count. May be zero.
    /// </summary>
    [XmlIgnore]
    public byte thumb_width;        // can be zero

    /// <summary>
    /// Vertical thumbnail pixel count. May be zero.
    /// </summary>
    [XmlIgnore]
    public byte thumb_height;       // can be zero

    /// <summary>
    /// Packed (byte-interleaved) 24-bit RGB values (8 bits per colour channel) for the thumbnail pixels, 
    /// in the order R0, G0, B0, ... Rk, Gk, Bk, with k = HthumbnailA * VthumbnailA.
    /// </summary>
    [XmlIgnore]
    public byte[] thumb_data;         // 3 * (thumb_width * thumb_height)

    internal JpegFileSegJFIF()
    { }

    public JpegFileSegJFIF(JpegFile jpegFile, JpegFileSegMarker markerId)
      : base(jpegFile, markerId)
    {
      // make sure we have signature for writing
      signature = new byte[] { 0x49, 0x46, 0x49, 0x46, 0x00 };
      // make sure we have version
      version = 0x0102; // written as big endian
    }

    public JpegFileSegJFIF(JpegFile jpegFile, JpegFileSegMarker markerId, JpegBinaryReader reader)
      : base(jpegFile, markerId, reader)
    {
      // must be in bigendian
      base.size = reader.read_u2_be();                  // read 2 bytes

      // advance to next segment address
      reader.Position = reader.Position - 2 + base.size;
    }

    public override bool Read(JpegBinaryReader reader)
    {
      // restore position to its original position
      reader.Position = this._address;

      base.size = reader.read_u2_be();                 // read 2 bytes
      signature = reader.read_u1(5);                   // read 5 bytes
      version = reader.read_u2_be();                   // read 2 bytes
      image_units = reader.read_u1();                  // read 1 byte
      image_width = reader.read_u2_be();               // read 2 bytes
      image_height = reader.read_u2_be();              // read 2 bytes
      thumb_width = reader.read_u1();                  // read 1 byte
      thumb_height = reader.read_u1();                 // read 1 byte
      if (thumb_width != 0 && thumb_height != 0)        
      {
        // Shall be equal to 16 + 3 * k, with k = HthumbnailA * VthumbnailA.
        int k = 3 * thumb_width * thumb_height;
        thumb_data = reader.read_u1(k);               // read k bytes
      }
      // advance address to the end of segment
      reader.Position = this._address + base.size;
      return true;
    }

    public override bool Read(JpegBinaryReader reader, long length)
    {
      return Read(reader);
    }

    public override bool Write(JpegBinaryWriter writer)
    {
      unchecked
      {
        // NOTICE: thumbnail can go beyond size of this block
        //         erase thumbnail in such case.
        uint tmp_size = (uint)(16 + (3 * thumb_width * thumb_height));
        if (tmp_size > ushort.MaxValue)
        {
          thumb_width = 0;
          thumb_height = 0;
          thumb_data = null;
          tmp_size = (uint)(16);
        }
        base.size = (ushort)tmp_size;
        writer.write_u2_be((ushort)base.size);           // write 2 bytes
        writer.write_u1(signature);                      // write 5 bytes
        writer.write_u2_be(version);                     // write 2 bytes
        writer.write_u1(image_units);                    // write 1 byte
        writer.write_u2_be(image_width);                 // write 2 bytes
        writer.write_u2_be(image_height);                // write 2 bytes
        writer.write_u1(thumb_width);                    // write 1 byte
        writer.write_u1(thumb_height);                   // write 1 byte
        if (thumb_data != null && thumb_data.Length > 0)
        {
          writer.write_u1(thumb_data);                   // write thumb_data.length bytes
        }
        return true;
      }
    }

    public override string ToString()
    {
      return string.Format("JpegFileSegJFIF\t:\t{{ a: 0x{0,8:x8}, id: {1,-5} ({2,4:X4}.h), size: {3,5} (0x{4,4:x4}) }}",
                            _address,
                            MarkerId,
                            ((ushort)MarkerId),
                            base.size,
                            base.size);
    }

  }
}
