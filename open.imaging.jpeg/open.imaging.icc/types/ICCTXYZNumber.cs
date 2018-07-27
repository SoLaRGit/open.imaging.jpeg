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
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace open.imaging.icc.types
{
  [XmlRoot("ICCTXYZNumber")]
  [XmlType("ICCTXYZNumber")]
  [StructLayout(LayoutKind.Explicit, Size = 12)]
  public unsafe struct ICCTXYZNumber
  {
    [XmlIgnore]
    [FieldOffset(0)]
    public ICCTs15Fixed16Number x;
    [XmlIgnore]
    [FieldOffset(4)]
    public ICCTs15Fixed16Number y;
    [XmlIgnore]
    [FieldOffset(8)]
    public ICCTs15Fixed16Number z;


    public static implicit operator ICCTXYZNumber(byte[] vright)
    {
      ICCTXYZNumber vleft;
      fixed (void* ptr = &vright[0])
      {
        //Marshal.Copy(vright, 0, new IntPtr(ptr), 12);        
        vleft = (ICCTXYZNumber)Marshal.PtrToStructure(new IntPtr(ptr), typeof(ICCTXYZNumber));        
      }
      return vleft;
    }

    internal byte[] data
    {
      get
      {
        byte[] value = new byte[12];
        fixed (void* ptr = &this.x)
        {
          Marshal.Copy(new IntPtr(ptr), value, 0, 12);
        }
        return value;
      }
      set
      {
        fixed (void* ptr = &this.x)
        {
          Marshal.Copy(value, 0, new IntPtr(ptr), 12);
        }        
      }
    }

    [XmlAttribute("x")]
    public float xml_x
    { 
      get { return (float)x; }
      set { x = (ICCTs15Fixed16Number)value; }
    }

    [XmlAttribute("y")]
    public float xml_y
    {
      get { return (float)y; }
      set { y = (ICCTs15Fixed16Number)value; }
    }

    [XmlAttribute("z")]
    public float xml_z
    {
      get { return (float)z; }
      set { z = (ICCTs15Fixed16Number)value; }
    }

    public override string ToString()
    {
      return string.Format("[x:{0},y:{1},z:{2}]", (float)x, (float)y, (float)z);
    }
  }
}
