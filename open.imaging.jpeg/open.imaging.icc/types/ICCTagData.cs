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
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

//using open.imaging.icc.types;

namespace open.imaging.icc.types
{
  // XmlInclude : support xml serialization out of box
  
  [XmlInclude(typeof(ICCTagDataTypeChromacity))]
  [XmlInclude(typeof(ICCTagDataTypeColorantOrder))]
  [XmlInclude(typeof(ICCTagDataTypeCurve))]
  [XmlInclude(typeof(ICCTagDataTypeData))]
  [XmlInclude(typeof(ICCTagDataTypeDateTime))]
  [XmlInclude(typeof(ICCTagDataTypeDescription))]
  [XmlInclude(typeof(ICCTagDataTypeLut16))]
  [XmlInclude(typeof(ICCTagDataTypeLut8))]
  [XmlInclude(typeof(ICCTagDataTypeLutAToB))]
  [XmlInclude(typeof(ICCTagDataTypeLutBToA))]
  [XmlInclude(typeof(ICCTagDataTypeMeasurement))]
  [XmlInclude(typeof(ICCTagDataTypeMultiLocalizedUnicode))]
  [XmlInclude(typeof(ICCTagDataTypeMultiProcessElements))]
  [XmlInclude(typeof(ICCTagDataTypeParametricCurve))]
  [XmlInclude(typeof(ICCTagDataTypeS15Fixed16Array))]
  [XmlInclude(typeof(ICCTagDataTypeSignature))]
  [XmlInclude(typeof(ICCTagDataTypeText))]
  [XmlInclude(typeof(ICCTagDataTypeViewingConditions))]
  [XmlInclude(typeof(ICCTagDataTypeXYZ))]
  [XmlRoot("ICCTagData")]
  [XmlType("ICCTagData")]
  public class ICCTagData : INotifyPropertyChanged
  {
    #region ### INotifyPropertyChanged ###

    public event PropertyChangedEventHandler PropertyChanged;

    [XmlIgnore]
    internal bool PropertyChangedLocked;

    // This method is called by the Set accessor of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    internal void NotifyPropertyChanged(String propertyName = "")
    {
      if (PropertyChangedLocked) return;
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
    #endregion

    #region ### constructors ###

    public ICCTagData()
    { }

    public ICCTagData(byte[] data)
    {
      this.data = data;
    }

    #endregion

    #region ### operators ###

    public static implicit operator ICCTagData(byte[] data)
    {
      return CreateTagData(data);
    }

    public static implicit operator byte[](ICCTagData tagdata)
    {
      return (tagdata.data);
    }

    #endregion

    #region ### properties ###

    [XmlIgnore]
    public byte[] data;

    [XmlIgnore]
    public uint type
    {
      get
      {        
        return unchecked((uint)((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3]));
      }
      set
      {
        unchecked
        {
          data[0] = (byte)(value >> 24);
          data[1] = (byte)(value >> 16);
          data[2] = (byte)(value >> 8);
          data[3] = (byte)(value);
        }
      }
    }
    
    [XmlAttribute]
    public string typeName
    {
      get 
      {
        unchecked
        {
          return ASCIIEncoding.ASCII.GetString(data, 0, 4);
        }
      }
      set
      {
        unchecked
        {
          data[0] = (byte)(value[0]);
          data[1] = (byte)(value[1]);
          data[2] = (byte)(value[2]);
          data[3] = (byte)(value[3]);
        }
      }
    }

    [XmlIgnore]
    public byte[] value
    {
      get
      {
        byte[] value = new byte[this.data.Length - 8];
        Buffer.BlockCopy(this.data, 8, value, 0, value.Length);
        return value;
      }
      set
      {
        Array.Resize<byte>(ref this.data, 8 + value.Length);
        Buffer.BlockCopy(value, 0, this.data, 8, value.Length);
      }
    }

    #endregion

    #region ### XML ###

    [ComVisible(false)] // speeds watch window display
    [XmlElement("data")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_data
    {
      get
      {
        if (data == null) return null;
        return data.to_hex_fmt(wrapindex: data.Length > 80 ? 80 : -1);
      }
      set { data = value.hex_to_a_u1(); }
    }

    #endregion


    public override string ToString()
    {
      return this.GetAttributes();
    }

    public virtual string GetAttributes()
    {
      return String.Format("ICCTagData:'{0}':{1,8:X8}", this.typeName, this.type);
    }
    

    #region ### GET/SET Value Offset ###

    public byte[] get_u1_array(int offset, int count)
    {
      byte[] items = new byte[count];
      Buffer.BlockCopy(data, offset, items, 0, count);
      return items;
    }

    public void set_u1_array(int offset, byte[] value)
    {
      Buffer.BlockCopy(value, 0, this.data, offset, value.Length);
    }

    public void set_u1_array(int newDataLength, int offset, byte[] value)
    {
      if (data.Length != newDataLength)
      {
        Array.Resize<byte>(ref data, newDataLength);
      }
      Buffer.BlockCopy(value, 0, this.data, offset, value.Length);    
    }

    public uint get_u4(int offset)
    {
      return unchecked((uint)((data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset+3]));
    }

    public void set_u4(int offset, uint value)
    {
      unchecked
      {
        data[offset] = (byte)(value >> 24);
        data[offset + 1] = (byte)(value >> 16);
        data[offset + 2] = (byte)(value >> 8);
        data[offset + 3] = (byte)(value);
      }    
    }
    
    public ushort get_u2(int offset)
    {
      return unchecked((ushort)((data[offset] << 8) | (data[offset + 1])));
    }

    public short get_i2(int offset)
    {
      return unchecked((short)((data[offset] << 8) | (data[offset + 1])));
    }

    public ushort[] get_u2_array(int offset, uint count)
    {
      ushort[] items = new ushort[count];
      for (int i = 0; i < count; i++)
      {        
        items[i] = unchecked((ushort)((data[offset] << 8) | (data[offset + 1])));
        offset += 2;
      }
      return items;
    }

    public void set_u2(int offset, ushort value)
    {
      unchecked
      {
        data[offset] = (byte)(value >> 8);
        data[offset + 1] = (byte)(value);
      }
    }

    public void set_i2(int offset, short value)
    {
      unchecked
      {
        data[offset] = (byte)(value >> 8);
        data[offset + 1] = (byte)(value);
      }
    }

    public void set_u2_array(int newDataLength, int offset, ushort[] items)
    {
      if (data.Length != newDataLength)
      {
        Array.Resize<byte>(ref data, newDataLength);
      }
      unchecked
      {
        int index = 0;
        for (int i = 0; i < items.Length; i++)
        {
          index = offset + (i << 2);
          data[index] = (byte)(items[i] >> 8);
          data[index + 1] = (byte)(items[i]);
        }
      }
    }

    public string get_string(int offset, uint count)
    {
      return System.Text.UTF8Encoding.UTF8.GetString(data, offset, (int)count);
    }

    public void set_string(int newDataSize, int offset, string svalue)
    {
      Array.Resize<byte>(ref data, newDataSize);
      byte[] value = System.Text.UTF8Encoding.UTF8.GetBytes(svalue);
      Buffer.BlockCopy(value, 0, data, offset, value.Length);
    }

    public ICCTXYZNumber get_icctXYZNumber(int offset)
    {
      ICCTXYZNumber xyz = new ICCTXYZNumber();
      xyz.data = get_u1_array(offset, 12);
      return xyz;
    }

    public void set_icctXYZNumber(int offset, ICCTXYZNumber xyz)
    {
      set_u1_array(data.Length, offset, xyz.data);
    }

    public ICCTu15Fixed16Number get_ICCTu15Fixed16Number(int offset)
    {
      ICCTu15Fixed16Number v = new ICCTu15Fixed16Number();
      v.n1u2 = get_u2(offset);
      v.n2u2 = get_u2(offset + 2);
      return v;
    }

    public void set_ICCTu15Fixed16Number(int offset, ICCTu15Fixed16Number v)
    {
      set_u2(offset, v.n1u2);
      set_u2(offset + 2, v.n2u2);
    }

    public ICCTs15Fixed16Number get_ICCTs15Fixed16Number(int offset)
    {
      ICCTs15Fixed16Number v = new ICCTs15Fixed16Number();
      v.n1i2 = get_i2(offset);
      v.n2u2 = get_u2(offset + 2);
      return v;
    }

    public void set_ICCTs15Fixed16Number(int offset, ICCTs15Fixed16Number v)
    {
      set_i2(offset, v.n1i2);
      set_u2(offset + 2, v.n2u2);
    }

    #endregion

    #region ### Create Tag Data Factory ###

    public static ICCTagData CreateTagData(byte[] data)
    {      
      uint type = unchecked((uint)((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3]));
      switch (type)
      {
        case ICCTagDataTypeDescription.ID:
          return (new ICCTagDataTypeDescription(data));

        case ICCTagDataTypeText.ID: 
          return (new ICCTagDataTypeText(data));

        case ICCTagDataTypeMultiLocalizedUnicode.ID: 
          return (new ICCTagDataTypeMultiLocalizedUnicode(data));

        case ICCTagDataTypeChromacity.ID:
          return (new ICCTagDataTypeChromacity(data));

        case ICCTagDataTypeColorantOrder.ID:
          return (new ICCTagDataTypeColorantOrder(data));

        case ICCTagDataTypeXYZ.ID:
          return (new ICCTagDataTypeXYZ(data));

        case ICCTagDataTypeDateTime.ID:
          return (new ICCTagDataTypeDateTime(data));

        case ICCTagDataTypeCurve.ID:
          return (new ICCTagDataTypeCurve(data));

        case ICCTagDataTypeViewingConditions.ID:
          return (new ICCTagDataTypeViewingConditions(data));

        case ICCTagDataTypeMeasurement.ID:
          return (new ICCTagDataTypeMeasurement(data));

        case ICCTagDataTypeSignature.ID:
          return (new ICCTagDataTypeSignature(data));

        // not yet tested or finished

        case ICCTagDataTypeLut8.ID:
          return (new ICCTagDataTypeLut8(data));

        case ICCTagDataTypeLut16.ID:
          return (new ICCTagDataTypeLut16(data));

        case ICCTagDataTypeLutAToB.ID:
          return (new ICCTagDataTypeLutAToB(data));

        case ICCTagDataTypeLutBToA.ID:
          return (new ICCTagDataTypeLutBToA(data));


      }

      // TODO: handle other types UNKNOWN ...
      return (new ICCTagData(data));
    }

    #endregion

  }
 

}
