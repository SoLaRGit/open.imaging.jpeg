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
  /// 10.29 XYZType
  /// <remarks>Table 82 — XYZType encoding.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeXYZ")]
  [XmlType("ICCTagDataTypeXYZ")]
  public class ICCTagDataTypeXYZ : ICCTagData
  {
    public const uint ID = 0x58595A20; // 'XYZ '

    public ICCTagDataTypeXYZ()
    { }

    public ICCTagDataTypeXYZ(byte[] data)
      : base(data)
    { }

    // TODO:
    // 8 to end | Variable | An array of PCSXYZ, CIEXYZ, or nCIEXYZ values | XYZNumber    
    public new ICCTXYZNumber value
    {
      get { return get_icctXYZNumber(8); }
      set { set_icctXYZNumber(8, value); NotifyPropertyChanged(); }
    }

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeXYZ:'{0}':value[0]:{1}", typeName, this.value);
    }
  }
}
