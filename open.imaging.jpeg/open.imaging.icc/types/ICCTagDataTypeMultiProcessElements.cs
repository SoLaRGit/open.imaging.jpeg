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
  /// 10.14 multiProcessElementsType
  /// <remarks>Table 52 — multiProcessElementsType encoding.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeMultiProcessElements")]
  [XmlType("ICCTagDataTypeMultiProcessElements")]
  public class ICCTagDataTypeMultiProcessElements : ICCTagData
  {

    public const uint ID = 0x6D706574; // 'mpet'

    public ICCTagDataTypeMultiProcessElements()
    { }

    public ICCTagDataTypeMultiProcessElements(byte[] data)
      : base(data)
    { }

    public ushort InputChannels
    {
      get { return get_u2(8); }
      // set { set_u2(8, value); }
    }

    public ushort OutputChannels
    {
      get { return get_u2(10); }
      // set { set_u2(10, value); }
    }
    
    public uint ProcessingElements
    {
      get { return get_u4(12); }
      // set { set_u4(12, value); }
    }

    // TODO: ...

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeMultiProcessElements:'{0}':(InputChannels:{1},OutputChannels:{2},ProcessingElements:{3}) TODO", typeName, InputChannels, OutputChannels, ProcessingElements);
    }
  }
}
