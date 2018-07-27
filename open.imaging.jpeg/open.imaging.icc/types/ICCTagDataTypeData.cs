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
  [XmlRoot("ICCTagDataTypeData")]
  [XmlType("ICCTagDataTypeData")]
  public class ICCTagDataTypeData : ICCTagData
  {

    public const uint ID = 0x64617461; // 'data'

    public ICCTagDataTypeData()
    { }

    public ICCTagDataTypeData(byte[] data)
      : base(data)
    { }

    /// <summary>
    /// Data flag, 00000000h represents ASCII data, 00000001h represents binary data, 
    /// other values are reserved for future use.
    /// </summary>    
    public uint valueType
    {
      get { return get_u4(8); }
      set { set_u4(8, value); NotifyPropertyChanged(); }
    }

    public string valueAsString
    {
      get { return get_string(12, (uint)(base.data.Length - 12)); }
      set { set_string(12 + value.Length, 12, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public new byte[] value
    {
      get
      {
        byte[] value = new byte[this.data.Length - 12];
        Buffer.BlockCopy(this.data, 12, value, 0, value.Length);
        return value;
      }
      set
      {
        Array.Resize<byte>(ref this.data, 12 + value.Length);
        Buffer.BlockCopy(value, 0, this.data, 12, value.Length);
        NotifyPropertyChanged();
      }
    }

    // TODO: ...

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeData:'{0}':'{1}'", typeName, this.valueAsString);
    }
  }
}
