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
  /// 10.7 dateTimeType
  /// <remarks>Table 36 — dateTimeType encoding.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeDateTime")]
  [XmlType("ICCTagDataTypeDateTime")]
  public class ICCTagDataTypeDateTime : ICCTagData
  {

    public const uint ID = 0x6474696D; // 'dtim'

    public ICCTagDataTypeDateTime()
    { }

    public ICCTagDataTypeDateTime(byte[] data)
      : base(data)
    { }

    [XmlIgnore]
    public ushort vYear
    {
      get { return get_u2(8); }
      set { set_u2(8, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ushort vMonth
    {
      get { return get_u2(10); }
      set { set_u2(10, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ushort vDay
    {
      get { return get_u2(12); }
      set { set_u2(12, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ushort vHour
    {
      get { return get_u2(14); }
      set { set_u2(14, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ushort vMinute
    {
      get { return get_u2(16); }
      set { set_u2(16, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ushort vSecond
    {
      get { return get_u2(18); }
      set { set_u2(18, value); NotifyPropertyChanged(); }
    }

    /// <summary>
    ///  8 to 19 | 12 | Date and time | dateTimeNumber
    ///  YYMMDDHHMMSS
    ///  4.2 dateTimeNumber
    /// A dateTimeNumber is a 12-byte value representation of the time and date, where the byte usage is assigned as specified in Table 1. The actual values are encoded as 16-bit unsigned integers (uInt16Number, see 4.10).
    /// Table 1 — dateTimeNumber
    /// Byte position | Field length bytes | Content                        | Encoded as
    ///   0 to 1      |  2   | Number of the year (actual year, e.g. 1994)  | uInt16Number
    ///   2 to 3      |  2   | Number of the month (1 to 12)                | uInt16Number
    ///   4 to 5      |  2   | Number of the day of the month (1 to 31)     | uInt16Number
    ///   6 to 7      |  2   | Number of hours (0 to 23)                    | uInt16Number
    ///   8 to 9      |  2   | Number of minutes (0 to 59)                  | uInt16Number
    ///   10 to 11    |  2   | Number of seconds (0 to 59)                  | uInt16Number
    /// </summary>    
    public new DateTime value
    {
      get { return new DateTime(vYear, vMonth, vDay, vHour, vMinute, vSecond); }
      set 
      {
        try
        {
          PropertyChangedLocked = true;
          vYear = (ushort)value.Year;
          vMonth = (ushort)value.Month;
          vDay = (ushort)value.Day;
          vHour = (ushort)value.Hour;
          vMinute = (ushort)value.Minute;
          vSecond = (ushort)value.Second;
        }
        finally
        {
          PropertyChangedLocked = false;
        }
        NotifyPropertyChanged();
      }
    }

    public override string GetAttributes()
    {
      return string.Format("[ICCTagDataTypeDateTime]:'{0}':'{1}'", typeName, this.value.ToString("yyyy-mm-dd HH:nn:ss.ffffff"));
    }
  }
}
