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
  ///* textDescription */
  //typedef struct {
  //    icUInt32Number      count;          /* Description length */
  //    icInt8Number        data[icAny];    /* Descriptions follow */
  ///*
  // *  Data that follows is of this form
  // *
  // * icInt8Number         desc[count]     * NULL terminated ascii string
  // * icUInt32Number       ucLangCode;     * UniCode language code
  // * icUInt32Number       ucCount;        * UniCode description length
  // * icInt16Number        ucDesc[ucCount];* The UniCode description
  // * icUInt16Number       scCode;         * ScriptCode code
  // * icUInt8Number        scCount;        * ScriptCode count
  // * icInt8Number         scDesc[67];     * ScriptCode Description
  // */
  //} icTextDescription;

  /// <summary>
  /// not documented type ???
  /// </summary>
  [XmlRoot("ICCTagDataTypeDescription")]
  [XmlType("ICCTagDataTypeDescription")]
  public class ICCTagDataTypeDescription : ICCTagData
  {

    public const uint ID = 0x64657363; // 'desc'

    public ICCTagDataTypeDescription()
    { }

    public ICCTagDataTypeDescription(byte[] data)
      : base(data)
    { }

    public uint count
    {
      get { return get_u4(8); }
      set { set_u4(8, value); NotifyPropertyChanged(); }
    }
    
    public new string value
    {
      get { return get_string(12, count - 1); }
      set { set_string(12 + value.Length, 12, value); NotifyPropertyChanged(); }
    }

    // TODO: ...

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeDescription:'{0}':'{1}'", typeName, this.value);
    }
  }
}
