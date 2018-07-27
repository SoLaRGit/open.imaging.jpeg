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
  /// 10.6 dataType
  /// <remarks>Table 35 — dataType encoding</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeViewingConditions")]
  [XmlType("ICCTagDataTypeViewingConditions")]
  public class ICCTagDataTypeViewingConditions : ICCTagData
  {

    public const uint ID = 0x76696577; // 'view'

    public ICCTagDataTypeViewingConditions()
    { }

    public ICCTagDataTypeViewingConditions(byte[] data)
      : base(data)
    { }

    /// <summary>
    /// 8 to 19  | 12 | Un-normalized CIEXYZ values for illuminant (in which Y is in cd/m2) | XYZNumber
    /// 20 to 31 | 12 | Un-normalized CIEXYZ values for surround (in which Y is in cd/m2) | XYZNumber
    /// 32 to 35 | 4  | Illuminant type | As described in measurementType
    /// </summary>
    
    public ICCTXYZNumber valueIlluminant
    {
      get { return get_icctXYZNumber(8); }
      set { set_icctXYZNumber(8, value); NotifyPropertyChanged(); }
    }
    
    public ICCTXYZNumber valueSurround
    {
      get { return get_icctXYZNumber(20); }
      set { set_icctXYZNumber(20, value); NotifyPropertyChanged(); }
    }

    public uint valueIlluminantType
    {
      get { return get_u4(32); }
      set { set_u4(32, value); NotifyPropertyChanged(); }    
    }

    // TODO: ...
    // public new byte[] value

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeViewingConditions:'{0}':(IlluminantType:{1},Illuminant:{2},Surround:{3})", typeName, this.valueIlluminantType, this.valueIlluminant, this.valueSurround);
    }
  }
}
