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
using System.Diagnostics;
using System.Xml.Serialization;

namespace open.imaging.icc.types
{
  /// <summary>
  /// 10.5 curveType
  /// <remarks>Table 34 — curveType encoding.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeCurve")]
  [XmlType("ICCTagDataTypeCurve")]
  public class ICCTagDataTypeCurve: ICCTagData
  {
    public const uint ID = 0x63757276; // 'curv'

    public ICCTagDataTypeCurve()
    { }

    public ICCTagDataTypeCurve(byte[] data)
      : base(data)
    { }

    /// <summary>
    /// 8 to 11   | 4   | Count value specifying the number of entries (n) that follow                      | uInt32Number
    /// </summary>
    public uint valueCount
    {
      get { return base.get_u4(8); }
      set { base.set_u4(8, value); NotifyPropertyChanged(); }
    }

    // TODO: handle exclussion ... (* If n = 1) ...

    /// <summary>
    /// 12 to end | 2n* | Actual curve values starting with the zeroth entry and ending with the entry n −1 | uInt16Number [...] *
    /// * If n = 1, the field length is 1 and the value is encoded as a u8Fixed8Number
    /// </summary>
    // TODO: MAKE XML SERIALIZATION/DESERIALIZATION FOR SHORT/USHORT ARRAYS.
    [XmlIgnore]
    public new ushort[] value
    {
      get 
      {
        //Debug.Assert(valueCount > 1, "EXCLUSSION ICCTagDataTypeCurve");
        return base.get_u2_array(12, valueCount); 
      }
      set 
      {
        //Debug.Assert(valueCount > 1, "EXCLUSSION ICCTagDataTypeCurve");
        base.set_u2_array(12 + value.Length, 12, value);
        NotifyPropertyChanged();
      }
    }
    
    public string valueString
    {
      get { return value.to_u2_fmt("{0,5},", 10); }
      set { /**/ }
    }

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeCurve:'{0}':ushort[{1}]", typeName, valueCount);
    }
  }
}
