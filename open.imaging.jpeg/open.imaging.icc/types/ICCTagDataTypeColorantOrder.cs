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
  /// 10.3 colorantOrderType
  /// <remarks>Table 32 — colorantOrderType encoding.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeColorantOrder")]
  [XmlType("ICCTagDataTypeColorantOrder")]
  public class ICCTagDataTypeColorantOrder: ICCTagData
  {
    public const uint ID = 0x636c726f; // 'clro'

    public ICCTagDataTypeColorantOrder()
    { }

    public ICCTagDataTypeColorantOrder(byte[] data)
      : base(data)
    { }
    
    public ushort NumberOfDeviceChannels
    {
      get { return base.get_u2(8); }
      set { base.set_u2(8, value); NotifyPropertyChanged(); }
    }

    public ushort ColorantType
    {
      get { return base.get_u2(10); }
      set { base.set_u2(10, value); NotifyPropertyChanged(); }
    }

    // TODO:
    //10 to 11 | 2      | Encoded value of phosphor or colorant type | see Table 31
    //12 to 19 | 8      | CIE xy coordinate values of channel 1      | u16Fixed16Number[2]
    //20 to end| 8(n−1) | CIE xy coordinate values of other channels (if needed) | u16Fixed16Number [...]

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeColorantOrder:'{0}'", typeName);
    }
  }
}
