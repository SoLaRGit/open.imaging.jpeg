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
using System.Xml.Serialization;

namespace open.imaging.icc.types
{
  public struct ICCTs15Fixed16Number
  {
    [XmlAttribute]
    public short n1i2;
    [XmlAttribute]
    public ushort n2u2;

    public static implicit operator ICCTs15Fixed16Number(float vright)
    {
      ICCTs15Fixed16Number vleft;
      vleft.n1i2 = (short)vright;
      vleft.n2u2 = (ushort)((vright - vleft.n1i2) * 65536);
      return vleft;
    }

    public static implicit operator float(ICCTs15Fixed16Number vright)
    {
      return (float)vright.n1i2 + ((float)vright.n2u2 / 65536);
    }

    public override string ToString()
    {
      return string.Format("{0}", (float)this);
    }
  }
}
