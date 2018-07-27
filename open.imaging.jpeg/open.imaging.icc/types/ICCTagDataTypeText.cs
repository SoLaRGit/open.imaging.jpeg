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
  /// 10.22 textType
  /// <remarks>Table 75 — textType encoding.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeText")]
  [XmlType("ICCTagDataTypeText")]
  public class ICCTagDataTypeText : ICCTagData
  {
    public const uint ID = 0x74657874; // 'text'

    public ICCTagDataTypeText()
    { }

    public ICCTagDataTypeText(byte[] data)
      : base(data)
    { }

    /// <summary>
    /// 8 to end | Variable | A string of (element size 8) 7-bit ASCII characters
    /// </summary>    
    public new string value
    {
      get
      {        
        return UTF8Encoding.UTF8.GetString(base.data, 8, base.data.Length - 9);        
      }
      set
      {        
        byte[] valuedata = UTF8Encoding.UTF8.GetBytes(value + '\0');
        Array.Resize<byte>(ref this.data, 8 + valuedata.Length);
        Buffer.BlockCopy(valuedata, 0, this.data, 8, valuedata.Length);
        NotifyPropertyChanged();
      }
    }

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeText:'{0}':'{1}'", typeName, this.value);
    }
  }
}
