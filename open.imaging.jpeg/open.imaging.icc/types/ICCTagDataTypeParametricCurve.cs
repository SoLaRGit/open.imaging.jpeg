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
  /// 10.16 parametricCurveType
  /// <remarks>Table 64 — parametricCurveType encoding.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeParametricCurve")]
  [XmlType("ICCTagDataTypeParametricCurve")]
  public class ICCTagDataTypeParametricCurve: ICCTagData
  {
    public const uint ID = 0x70617261; // 'para'

    public ICCTagDataTypeParametricCurve()
    { }

    public ICCTagDataTypeParametricCurve(byte[] data)
      : base(data)
    { }

    /// <summary>
    /// 8 to 9 | 2 | Encoded value of the function type | uInt16Number (see Table 65)
    /// </summary>
    public ICCParametricCurveFunctionType FunctionType
    {
      get { return (ICCParametricCurveFunctionType)base.get_u2(8); }
      set { base.set_u2(8, (ushort)value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ushort Reserved0
    {
      get { return base.get_u2(10); }
      set { base.set_u2(10, value); NotifyPropertyChanged(); }
    }

    // TODO: return value ...

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeParametricCurve:'{0}':FunctionType:{1}:TODO", typeName, FunctionType);
    }
  }

  /// <summary>
  /// Table 65 — parametricCurveType function type encoding
  /// </summary>
  public enum ICCParametricCurveFunctionType : ushort
  { 
    /// <summary>
    /// Field size (4).
    /// <code>Y = X ^ g</code>
    /// </summary>
    g = 0,
    /// <summary>
    /// Field size (12).
    /// CIE 122-1996
    /// <code>
    /// (X &lt;= -b/a) => Y = (a*X + b) ^ g,
    /// (X > -b/a) => Y = 0
    /// </code>
    /// </summary>
    gab = 1,
    /// <summary>
    /// Field size (16).
    /// IEC 61966-3
    /// <code>
    /// (X &lt;= -b/a) => Y = ((a*X + b) ^ g) + c,
    /// (X > -b/a) => Y = c
    /// </code>
    /// </summary>
    gabc = 2,
    /// <summary>
    /// Field size (20).
    /// IEC 61966-2-1 (sRGB)
    /// <code>
    /// (X &lt;= d) => Y = ((a*X + b) ^ g),
    /// (X > d) => Y = c*X
    /// </code>
    /// </summary>
    gabcd = 3,
    /// <summary>
    /// Field size (28).
    /// 
    /// <code>
    /// (X &lt;= d) => Y = (((a*X) + b) ^ g) + c,
    /// (X > d) => Y = (c*X) + f
    /// </code>
    /// </summary>
    gabcdef = 4
  }
}
