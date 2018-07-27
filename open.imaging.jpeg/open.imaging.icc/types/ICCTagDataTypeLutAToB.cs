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
  /// 10.10 lutAToBType
  /// <remarks>Table 42 — lutAToBType encoding.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeLutAToB")]
  [XmlType("ICCTagDataTypeLutAToB")]
  public class ICCTagDataTypeLutAToB : ICCTagData
  {

    public const uint ID = 0x6D414220; // 'mAB '

    public ICCTagDataTypeLutAToB()
    { }

    public ICCTagDataTypeLutAToB(byte[] data)
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
      return string.Format("ICCTagDataTypeLutAToB:'{0}':(InputChannels:{1},OutputChannels:{2})TODO", typeName, InputChannels, OutputChannels);
    }
  }
}
