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

  [XmlRoot("segAPPPHOTO")]
  [XmlType("segAPPPHOTO")]
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDual)]
  public class JpegFileSegAPPPHOTO : JpegFileSeg
  {
    internal JpegFileSegAPPPHOTO()
    { }

    public JpegFileSegAPPPHOTO(JpegFile jpegFile, JpegFileSegMarker id, JpegBinaryReader reader)
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
      return string.Format("JpegFileSegAPPPHOTO\t:\t{{ a: 0x{0,8:x8}, id: {1,-5} ({2,4:X4}.h), size: {3,5} (0x{4,4:x4}), signature:'{5}' }}",
              _address,
              MarkerId,
              ((ushort)MarkerId),
              size,
              size,
              data.to_str());
    }
  }

}
