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
using System.Runtime.InteropServices;

namespace open.imaging.jpeg
{

  [XmlRoot("segCOM")]
  [XmlType("segCOM")]
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDual)]
  public class JpegFileSegCOM : JpegFileSeg
  {

    [XmlElement]
    public string Comment
    {
      get
      {
        return System.Text.UTF8Encoding.UTF8.GetString(this.data);
      }
      set
      {
        this.data = System.Text.UTF8Encoding.UTF8.GetBytes(value);
        this.size = 2 + this.data.Length;
      }
    }

    internal JpegFileSegCOM()
    { }

    public JpegFileSegCOM(JpegFile jpegFile, string Comment)
      : base(jpegFile, JpegFileSegMarker.COM)
    {
      this.Comment = Comment;
    }

    public JpegFileSegCOM(JpegFile jpegFile, JpegFileSegMarker id, JpegBinaryReader reader)
      : base(jpegFile, id, reader)
    {
      long address = reader.Position;
      size = reader.read_u2_be();                         // read 2 bytes      
    }

    public override bool Read(JpegBinaryReader reader)
    {
      return Read(reader, (int)(size - 2));
    }

    public override bool Read(JpegBinaryReader reader, long length)
    {
      if (size > 0) data = reader.read_u1((int)length);   // read data
      return true;        
    }

    public override bool Write(JpegBinaryWriter writer)
    {
      writer.write_u2_be((ushort)size);                   // write size 2 bytes
      writer.write_u1(data);
      return true;
    }

    public override string ToString()
    {
      return string.Format("JpegFileSegCOM\t:\t{{ a: 0x{0,8:x8}, id: {1,-5} ({2,4:X4}.h), size: {3,5} (0x{4,4:x4}) }}",
                            _address,
                            MarkerId,
                            ((ushort)MarkerId),
                            size,
                            size);
    }
  }
}
