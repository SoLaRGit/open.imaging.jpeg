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
using System.Runtime.InteropServices;

namespace open.imaging.jpeg
{
  /// <summary>
  /// ICC1v43_2010-12.pdf - Specification ICC.1:2010 (Profile version 4.3.0.0)
  /// Annex B.4 Embedding ICC profiles in JPEG files
  /// The JPEG standard (ISO/IEC 10918-1[2]) supports application specific data segments. 
  /// These segments may be used for tagging images with ICC profiles. The APP2 marker is 
  /// used to introduce the ICC profile tag. Given that there are only 15 supported APP 
  /// markers, there is a chance of many applications using the same marker. ICC tags are 
  /// thus identified by beginning the data with a special null terminated byte sequence, 
  /// “ICC_PROFILE”. 
  /// 
  /// The length field of a JPEG marker is only two bytes long; the length of the length 
  /// field is included in the total. Hence, the values 0 and 1 are not legal lengths. 
  /// This would limit the maximum data length to 65 533. The identification sequence would
  /// lower this even further. As it is quite possible for an ICC profile to be longer than
  /// this, a mechanism is required to break the profile into chunks and place each chunk 
  /// in a separate marker. A mechanism to identify each chunk in sequence order is 
  /// therefore necessary.
  /// 
  /// The identifier sequence is followed by one byte indicating the sequence number of the
  /// chunk (counting starts at 1) and one byte indicating the total number of chunks. All
  /// chunks in the sequence should indicate the same total number of chunks. The 1-byte 
  /// chunk count limits the size of embeddable profiles to 16 707 345 bytes.
  /// </summary>

  [XmlRoot("segAPPICC")]
  [XmlType("segAPPICC")]
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDual)]
  public class JpegFileSegAPPICC : JpegFileSeg
  {

    [XmlAttribute("iccindex")]
    public byte ChunkIndex
    {
      get
      {
        return this.data[12];
      }
      set
      {
        this.data[12] = value;
      }
    }

    [XmlAttribute("icctotal")]
    public byte ChunksTotal
    {
      get
      {
        return this.data[13];
      }
      set
      {
        this.data[13] = value;
      }
    }

    [XmlIgnore]
    public byte[] ICCData
    {
      get
      {
        byte[] iccdata = new byte[this.data.Length - 14];
        Buffer.BlockCopy(this.data, 14, iccdata, 0, iccdata.Length);
        return iccdata;
      }
      set
      {
        this.size = 2 + 14 + value.Length;
        if (this.data.Length != this.size)
        {
          Array.Resize<byte>(ref this.data, (int)this.size);
        }
        Buffer.BlockCopy(value, 0, this.data, 14, value.Length);
      }
    }

    internal JpegFileSegAPPICC()
    { }

    public JpegFileSegAPPICC(JpegFile jpegFile, JpegFileSegMarker id, JpegBinaryReader reader)
      : base(jpegFile, id, reader)
    {
      long address = reader.Position;
      // must be bigendian
      size = reader.read_u2_be();                           // read 2 bytes      
    }

    public override bool Read(JpegBinaryReader reader)
    {
      return Read(reader, (int)(size - 2));
    }

    public override bool Read(JpegBinaryReader reader, long length)
    {
      if (size > 0) data = reader.read_u1((int)length);     // read data length bytes
      return true;
    }

    public override bool Write(JpegBinaryWriter writer)
    {
      // must be bigendian
      writer.write_u2_be((ushort)size);                     // write 2 bytes
      writer.write_u1(data);                                // write data (data.length) bytes
      return true;
    }

    public override int Compare(JpegFileSeg other)
    {
      if (null == other)
      {
        throw (new ArgumentNullException("JpegFileSeg other argument must be specified."));
      }
      if (null == other.AsAPP)
      {
        throw (new ArgumentNullException("JpegFileSeg other argument must be of type JpegFileSegAPP."));
      }
      if (data.Length != other.data.Length)
      {
        return data.Length.CompareTo(other.data.Length);
      }
      for (int i = 0; i < other.data.Length; i++)
      {
        if (data[i] != other.data[i])
        {
          return data[i].CompareTo(other.data[i]);
        }
      }
      return 0; // EQUAL
    }

    public override string ToString()
    {
      return string.Format("JpegFileSegICC\t:\t{{ a: 0x{0,8:x8}, id: {1,-5} ({2,4:X4}.h), size: {3,5} (0x{4,4:x4}), signature:'{5}' }}",
              _address,
              MarkerId,
              ((ushort)MarkerId),
              size,
              size,
              data.to_str());
    }
  }

}
