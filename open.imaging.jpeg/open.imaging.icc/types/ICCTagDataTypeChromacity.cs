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
  [XmlRoot("ICCTagDataTypeChromacity")]
  [XmlType("ICCTagDataTypeChromacity")]
  public class ICCTagDataTypeChromacity: ICCTagData
  {
    public const uint ID = 0x6368726D; // 'chrm'

    public ICCTagDataTypeChromacity()
    { }

    public ICCTagDataTypeChromacity(byte[] data)
      : base(data)
    { }

    
    public uint CountOfColorants  // n
    {
      get { return base.get_u4(8); }
      set { base.set_u4(8, value); NotifyPropertyChanged(); }
    }

    
    public byte ColorantType
    {
      get { return base.data[12]; }
      set { base.data[12] = value; NotifyPropertyChanged(); }
    }

    // TODO:
    //12           | 1   | Number of the colorant to be printed first. | uInt8Number
    //13 to (11+n) | n−1 | The remaining n−1 colorants are described in a manner consistent with the first colorant | uInt8Number

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeChromacity:'{0}'", typeName);
    }
  }
}
