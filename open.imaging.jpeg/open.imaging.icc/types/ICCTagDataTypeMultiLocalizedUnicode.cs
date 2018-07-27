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
  /// 10.13 multiLocalizedUnicodeType
  /// <remarks>Table 51 — multiLocalizedUnicodeType.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeMultiLocalizedUnicode")]
  [XmlType("ICCTagDataTypeMultiLocalizedUnicode")]
  public class ICCTagDataTypeMultiLocalizedUnicode : ICCTagData
  {
    public const uint ID = 0x6D6C7563; // 'mluc'

    public ICCTagDataTypeMultiLocalizedUnicode()
    { }

    public ICCTagDataTypeMultiLocalizedUnicode(byte[] data)
      : base(data)
    { }
    
    public uint NumberOfRecords
    {
      get { return get_u4(8); }
      set { set_u4(8, value); NotifyPropertyChanged(); }
    }

    public uint RecordSize
    {
      get { return get_u4(12); }
      set { set_u4(12, value); NotifyPropertyChanged(); }
    }
    // TODO: I think this is repeated standard is not clear about this
    public ushort FirstRecordLanguageCode
    {
      get { return get_u2(16); }
      set { set_u2(16, value); NotifyPropertyChanged(); }
    }
    public ushort FirstRecordCountryCode
    {
      get { return get_u2(18); }
      set { set_u2(18, value); NotifyPropertyChanged(); }
    }
    public uint FirstRecordStringLength
    {
      get { return get_u4(20); }
      set { set_u4(20, value); NotifyPropertyChanged(); }
    }
    public uint FirstRecordStringOffset
    {
      get { return get_u4(24); }
      set { set_u4(24, value); NotifyPropertyChanged(); }
    }

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeMultiLocalizedUnicode:'{0}'", this.typeName);
    }
  }
}
