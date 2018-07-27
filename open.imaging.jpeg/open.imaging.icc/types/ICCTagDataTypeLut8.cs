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
  /// 10.9 lut8Type
  /// <remarks>Table 41 — lut8Type encoding.</remarks>
  /// </summary>
  [XmlRoot("ICCTagDataTypeLut8")]
  [XmlType("ICCTagDataTypeLut8")]
  public class ICCTagDataTypeLut8: ICCTagData
  {
    public const uint ID = 0x6D667431; // 'mft1'

    public ICCTagDataTypeLut8()
    { }

    public ICCTagDataTypeLut8(byte[] data)
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

    [XmlIgnore]
    public byte CLUTGridPoints
    {
      get { return data[10]; }
      //set { data[10] = value; }
    }

    [XmlIgnore]
    public byte Reserved0
    {
      get { return data[11]; }
      //set { data[11] = value; }
    }

    [XmlIgnore]
    public ICCTs15Fixed16Number e1
    {
      get { return get_ICCTs15Fixed16Number(12); }
      set { set_ICCTs15Fixed16Number(12, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ICCTs15Fixed16Number e2
    {
      get { return get_ICCTs15Fixed16Number(16); }
      set { set_ICCTs15Fixed16Number(16, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ICCTs15Fixed16Number e3
    {
      get { return get_ICCTs15Fixed16Number(20); }
      set { set_ICCTs15Fixed16Number(20, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ICCTs15Fixed16Number e4
    {
      get { return get_ICCTs15Fixed16Number(24); }
      set { set_ICCTs15Fixed16Number(24, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ICCTs15Fixed16Number e5
    {
      get { return get_ICCTs15Fixed16Number(28); }
      set { set_ICCTs15Fixed16Number(28, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ICCTs15Fixed16Number e6
    {
      get { return get_ICCTs15Fixed16Number(32); }
      set { set_ICCTs15Fixed16Number(32, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ICCTs15Fixed16Number e7
    {
      get { return get_ICCTs15Fixed16Number(36); }
      set { set_ICCTs15Fixed16Number(36, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ICCTs15Fixed16Number e8
    {
      get { return get_ICCTs15Fixed16Number(40); }
      set { set_ICCTs15Fixed16Number(40, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public ICCTs15Fixed16Number e9
    {
      get { return get_ICCTs15Fixed16Number(44); }
      set { set_ICCTs15Fixed16Number(44, value); NotifyPropertyChanged(); }
    }

    [XmlIgnore]
    public const int InputTableOffset = 48;

    [XmlIgnore]
    public int InputTableSize
    {
      get { return (int)(256 * InputChannels); }
    }

    [XmlIgnore]
    public byte[] InputTable
    {
      get { return get_u1_array(InputTableOffset, InputTableSize); }
      set { setArray(1, value); }
    }

    [XmlIgnore]
    public int CLUTValuesOffset
    {
      get { return InputTableOffset + InputTableSize; }
    }

    [XmlIgnore]
    public int CLUTValuesSize
    {
      get { return (int)(Math.Pow(CLUTGridPoints, InputChannels) * OutputChannels); }
    }

    [XmlIgnore]
    public byte[] CLUTValues
    {
      get { return get_u1_array(CLUTValuesOffset, CLUTValuesSize); }
      set { setArray(2, value); }
    }

    [XmlIgnore]
    public int OutputTableOffset
    {
      get { return InputTableOffset + InputTableSize + CLUTValuesSize; }
    }

    [XmlIgnore]
    public int OutputTableSize
    {
      get { return 256 * OutputChannels; }
    }

    [XmlIgnore]
    public byte[] OutputTable
    {
      get { return get_u1_array(OutputTableOffset, OutputTableSize); }
      set { setArray(3, value); }
    }

    private void setArray(int arrayIndex, byte[] array)
    {
      int arraySize = array.Length;
      int size = InputTableOffset;
      switch (arrayIndex)
      {
        case 1: size += arraySize + CLUTValuesSize + OutputTableSize; break;
        case 2: size += InputTableSize + arraySize + OutputTableSize; break;
        case 3: size += InputTableSize + CLUTValuesSize + arraySize; break;
      }
      byte[] newData = new byte[size];
      Buffer.BlockCopy(base.data, 0, newData, 0, InputTableOffset);
      switch (arrayIndex)
      {
        case 1:
          {
            Buffer.BlockCopy(array, 0, newData, InputTableOffset, arraySize);
            Buffer.BlockCopy(data, CLUTValuesOffset, newData, InputTableOffset + arraySize, CLUTValuesSize);
            Buffer.BlockCopy(data, OutputTableOffset, newData, InputTableOffset + arraySize + CLUTValuesSize, OutputTableSize);
            data[8] = (byte)(arraySize >> 8);
          }
          break;
        case 2:
          {
            Buffer.BlockCopy(data, InputTableOffset, newData, InputTableOffset, InputTableSize);
            Buffer.BlockCopy(array, 0, newData, InputTableOffset + InputTableSize, arraySize);
            Buffer.BlockCopy(data, OutputTableOffset, newData, InputTableOffset + InputTableSize + arraySize, OutputTableSize);
            int iCLUTGridPoints = (arraySize / OutputChannels);
            for (int i = 1; i < this.InputChannels; i++)
            {
              iCLUTGridPoints /= this.CLUTGridPoints;
            }
            data[10] = (byte)iCLUTGridPoints;
          }
          break;
        case 3:
          {
            Buffer.BlockCopy(data, InputTableOffset, newData, InputTableOffset, InputTableSize);
            Buffer.BlockCopy(data, CLUTValuesOffset, newData, InputTableOffset + InputTableSize, CLUTValuesSize);
            Buffer.BlockCopy(array, 0, newData, InputTableOffset + InputTableSize + CLUTValuesSize, arraySize);
            this.data[9] = (byte)(arraySize >> 8);
          }
          break;
      }
      NotifyPropertyChanged();
    }

    public override string GetAttributes()
    {
      return string.Format("ICCTagDataTypeLut8:'{0}':(InputChannels:{1},OutputChannels:{2},CLUTGridPoint:{3},...)", 
        typeName, InputChannels, OutputChannels, CLUTGridPoints);
    }
  }
}
