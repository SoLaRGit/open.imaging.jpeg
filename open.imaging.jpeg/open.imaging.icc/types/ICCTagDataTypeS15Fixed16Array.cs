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
  /// <summary>
  /// 10.20 s15Fixed16ArrayType
  /// <remarks>Table 73 — s15Fixed16ArrayType encoding.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeS15Fixed16Array")]
  [XmlType("ICCTagDataTypeS15Fixed16Array")]
  public class ICCTagDataTypeS15Fixed16Array : ICCTagData
  {

    public const uint ID = 0x73663332; // 'sf32'

    public ICCTagDataTypeS15Fixed16Array()
    { }

    public ICCTagDataTypeS15Fixed16Array(byte[] data)
      : base(data)
    { }
    
    public uint valueCount
    {
      // (sizeof(data) - 8) / sizeof(s15Fixed16Number)
      get { return (uint)((data.Length - 8) >> 2); }  
    }
    
    public new ICCTs15Fixed16Number[] value
    {
      get
      {
        ICCTs15Fixed16Number[] value = new ICCTs15Fixed16Number[this.valueCount];
        int offset = 8;
        for (int i = 0; i < this.valueCount; i++)
        {
          value[i] = get_ICCTs15Fixed16Number(offset);
          offset += 4;
        }
        return value;
      }
      // TODO: set ...
    }

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeS15Fixed16Array:'{0}':(valueCount:{1}):TODO", typeName, valueCount);
    }
  }
}
