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
  /// 10.12 measurementType
  /// <remarks>Table 46 — measurementType structure.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeMeasurement")]
  [XmlType("ICCTagDataTypeMeasurement")]
  public class ICCTagDataTypeMeasurement: ICCTagData
  {
    public const uint ID = 0x6D656173; // 'meas'

    public ICCTagDataTypeMeasurement()
    { }

    public ICCTagDataTypeMeasurement(byte[] data)
      : base(data)
    { }
    
    public ICCStandardObserver StandardObserver
    {
      get { return (ICCStandardObserver)base.get_u4(8); }
      set { base.set_u4(8, (uint)value); NotifyPropertyChanged(); }
    }

    
    public ICCTXYZNumber TristimulusBacking
    {
      get { return base.get_icctXYZNumber(12); }
      set { base.set_icctXYZNumber(12, value); NotifyPropertyChanged(); }
    }

    
    public ICCMeasurementGeometry MeasurementGeometry
    {
      get { return (ICCMeasurementGeometry)get_u4(24); }
      set { set_u4(24, (uint)value); NotifyPropertyChanged(); }
    }

    
    public ICCTu15Fixed16Number MeasurementFlare
    {
      get { return get_ICCTu15Fixed16Number(28); }
      set { set_ICCTu15Fixed16Number(28, value); NotifyPropertyChanged(); }
    }

    
    public ICCStandardIllumination StandardIllumination
    {
      get { return (ICCStandardIllumination)get_u4(32); }
      set { set_u4(32, (uint)value); NotifyPropertyChanged(); }
    }


    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeMeasurement:'{0}':(StandardObserver:{1},TristimulusBacking:{2},MeasurementGeometry:{3},MeasurementFlare:{4},StandardIllumination:{5})", 
        typeName, StandardObserver, TristimulusBacking, MeasurementGeometry, (float)MeasurementFlare, StandardIllumination);
    }
  }

  /// <summary>
  /// Table 47 — Standard observer encodings
  /// </summary>
  public enum ICCStandardObserver
  {
    Unknown = 0,
    CIE1931 = 1,
    CIE1964 = 2
  }

  /// <summary>
  /// Table 48 — Measurement geometry encodings
  /// </summary>
  public enum ICCMeasurementGeometry
  {
    Unknown = 0,
    MG_0deg_to_45deg_or_45deg_to_0deg = 1,
    MG_0deg_to_delta_or_delta_to_0deg = 2,
  }

  /// <summary>
  /// Table 49 — Measurement flare encodings
  /// <remarks>u16Fixed16Number - this is stupid can't be represented as enum.
  /// actually it can but it would be huge one.</remarks>
  /// </summary>
  public enum ICCMeasurementFlare
  {
    Percent_0_point_0 = 0x00000000,
    // ...
    Percent_100_point_0 = 0x00010000,
  }

  /// <summary>
  /// Table 50 — Standard illuminant encodings
  /// </summary>
  public enum ICCStandardIllumination
  {
    Unknown = 0,
    D50 = 1,
    D65 = 2,
    D93 = 3,
    F2 = 4,
    D55 = 5,
    A = 6,
    EquiPowerE = 7,
    F8 = 8
  }
}
