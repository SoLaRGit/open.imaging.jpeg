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
  /// 10.11 lutBToAType
  /// <remarks>Table 44 — lutBToAType encoding.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeLutBToA")]
  [XmlType("ICCTagDataTypeLutBToA")]
  public class ICCTagDataTypeLutBToA : ICCTagData
  {

    public const uint ID = 0x6D424120; // 'mBA '

    public ICCTagDataTypeLutBToA()
    { }

    public ICCTagDataTypeLutBToA(byte[] data)
      : base(data)
    { }

    [XmlIgnore]
    public byte InputChannels
    {
      get { return data[8]; }
      //set { data[8] = value; }
    }

    [XmlIgnore]
    public byte OutputChannels
    {
      get { return data[9]; }
      //set { data[9] = value; }
    }
    // TODO: ...

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeLutBToA:'{0}':(InputChannels:{1},OutputChannels:{2}) TODO", typeName, InputChannels, OutputChannels);
    }
  }
}
