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
using System.IO;
using open.imaging.jpeg;
using System.Runtime.InteropServices;

using open.imaging.icc.types;
using System.Xml.Serialization;
using System.ComponentModel;

namespace open.imaging.icc
{

  //public class multiLocalizedUnicodeType
  //{
  //  public uint Signature; // 0x6d6c7563
  //  public uint Reserved; // 0x00000000
  //  public uint NumberOfRecords; // n
  //  public uint RecordSize; // 0x0000000c
  //  //
  //  public ushort FRLanguageCode; // ISO 639-1
  //  public ushort FRCountryCode; // ISO 3166-1
  //  public uint FRStringLength; // bytes
  //  public uint FRStringOffset; // offset from the start of tag to the start of string
  //  // additional records
  //  // storage area
  //}

  //public class ICCTagDataType
  //{
  //  public uint type;
  //  public uint reserved;

  //  public string typeName
  //  {
  //    get
  //    {
  //      unchecked
  //      {
  //        byte[] data = new byte[]
  //      { 
  //        (byte)(type >> 24), 
  //        (byte)(type >> 16), 
  //        (byte)(type >> 8), 
  //        (byte)(type)
  //      };
  //        return System.Text.ASCIIEncoding.ASCII.GetString(data);
  //      }
  //    }
  //    set
  //    {
  //      unchecked
  //      {
  //        byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(value);
  //        type = (uint)((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3]);
  //      }
  //    }
  //  }
  //  public override string ToString()
  //  {
  //    return string.Format("'{0}'", typeName);
  //  }

  //}

  //public class ICCTagDataTypeText : ICCTagDataType
  //{
  //  public byte[] value;

  //  public override string ToString()
  //  {
  //    return System.Text.UTF8Encoding.UTF8.GetString(this.value);
  //  }
  //}

  public enum ICCProfileDeviceClass : uint
  {
    // Unknown = 0x00000000, // \0\0\0\0
    Input = 0x73636e72, // scnr
    Display = 0x6d6e7472, // mntr
    Output = 0x70727472, // prtr
    Link = 0x6c696e6b, // link
    ColorSpace = 0x73706163, // spac
    Abstract = 0x61627374, // abst
    NamedColor = 0x6e6d636c, // nmcl
  }

  public enum ICCColorSpaceClass : uint
  {
    // Unknown = 0x00000000, // '\0\0\0\0'
    nCIEXYZorPCSXYZa = 0x58595A20, // ‘XYZ ’
    CIELABorPCSLABb = 0x4C616220, // ‘Lab ’
    CIELUV = 0x4C757620, // ‘Luv ’
    YCbCr = 0x59436272, // ‘YCbr’
    CIEYxy = 0x59787920, // ‘Yxy ’
    RGB = 0x52474220, // ‘RGB ’
    Gray = 0x47524159, // ‘GRAY’
    HSV = 0x48535620, // ‘HSV ’
    HLS = 0x484C5320, // ‘HLS ’
    CMYK = 0x434D594B, // ‘CMYK’
    CMY = 0x434D5920, // ‘CMY ’
    Colour2 = 0x32434C52, // ‘2CLR’
    Colour3 = 0x33434C52, // ‘3CLR’ (other than those listed above)
    Colour4 = 0x34434C52, // ‘4CLR’ (other than CMYK)
    Colour5 = 0x35434C52, // ‘5CLR’
    Colour6 = 0x36434C52, // ‘6CLR’
    Colour7 = 0x37434C52, // ‘7CLR’
    Colour8 = 0x38434C52, // ‘8CLR’
    Colour9 = 0x39434C52, // ‘9CLR’
    Colour10 = 0x3A434C52, // ‘ACLR’
    Colour11 = 0x3B434C52, // ‘BCLR’
    Colour12 = 0x3C434C52, // ‘CCLR’
    Colour13 = 0x3D434C52, // ‘DCLR’
    Colour14 = 0x3E434C52, // ‘ECLR’
    Colour15 = 0x3F434C52, // ‘FCLR’
  }

  public enum ICCPrimaryPlatform : uint
  {
    None = 0x00000000, // '\0\0\0\0'
    AppleComputerInc = 0x4150504C, // ‘APPL’
    MicrosoftCorporation = 0x4D534654, // ‘MSFT’
    SiliconGraphicsInc = 0x53474920, // ‘SGI ’
    SunMicrosystemsInc = 0x53554E57, // ‘SUNW’
  }

  [Flags]
  public enum ICCProfileFlags : uint
  {
    ProfileIndenpendent = 0,
    Embedded = 1,
    NonIndenpendent = 2,
    // EmbeddedNonIndenpendent = 3,
  }

  [Flags]
  public enum ICCDeviceAttributes : ulong
  {
    Reflective = 0,
    Transparency = 1, // else reflective
    Matte = 2,    // else glossy  
    Negative = 4, // else positive
    BWMedia = 8,  // else color
    //  4 to 31 | 28 | Reserved (set to binary zero)
    // 32 to 63 | 32 | Use not defined by ICC (vendor specific)
  }

  public enum ICCRenderingIntent : uint
  {
    Perceptual = 0,
    MediaRelativeColorimetric = 1,
    Saturation = 2,
    ICCAbsoluteColorimetric = 3
  }

  // todo: ICCManufacturer
  // todo: ICCModel (or maybe not too many and may overlap)...
  //

  //public class ICCTag
  //{
  //  public uint Signature;
  //  public uint Offset;
  //  public uint Length;
  //  public byte[] data;

  //  public string SignatureString
  //  {
  //    get
  //    {
  //      unchecked
  //      {
  //        byte[] data = new byte[]
  //      { 
  //        (byte)(Signature >> 24), 
  //        (byte)(Signature >> 16), 
  //        (byte)(Signature >> 8), 
  //        (byte)(Signature)
  //      };
  //        return System.Text.ASCIIEncoding.ASCII.GetString(data);
  //      }
  //    }
  //    set
  //    {
  //      unchecked
  //      {
  //        byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(value);
  //        Signature = (uint)((data[0] << 24) | (data[1] << 16) | (data[2] << 8) | data[3]);
  //      }
  //    }
  //  }
  //  public override string ToString()
  //  {
  //    return string.Format("{1,8:x8}h:'{0}', O:{2}, L:{3}, base:{4}", this.SignatureString, this.Signature, this.Offset, this.Length, _base.ToString());
  //  }

    //internal ICCTagDataType _base
    //{
    //  get
    //  {
    //    ICCTagDataType v = new ICCTagDataType();
    //    v.type = _type;
    //    return v;
    //  }
    //}
    //internal uint _type
    //{
    //  get
    //  {

    //    return unchecked(((uint)this.data[0] << 24) |
    //                     ((uint)this.data[1] << 16) |
    //                     ((uint)this.data[2] << 8) |
    //                     ((uint)this.data[3]));
    //  }
    //}

    //internal multiLocalizedUnicodeType _cprt
    //{
    //  get
    //  {
    //    if (this._type != 0x6D6C7563) return null;
    //    multiLocalizedUnicodeType mluc = new multiLocalizedUnicodeType();
    //    using (MemoryStream ms = new MemoryStream(this.data))
    //    {
    //      using (JpegBinaryReader br = new JpegBinaryReader(ms))
    //      {
    //        mluc.Signature = br.read_u4_be();
    //        mluc.Reserved = br.read_u4_be();
    //        mluc.NumberOfRecords = br.read_u4_be();
    //        mluc.RecordSize = br.read_u4_be();
    //        mluc.FRLanguageCode = br.read_u2_be();
    //        mluc.FRCountryCode = br.read_u2_be();
    //        mluc.FRStringLength = br.read_u4_be();
    //        mluc.FRStringOffset = br.read_u4_be();
    //        //...
    //      }
    //    }
    //    return mluc;
    //  }
    //  set
    //  {

    //  }
    //}
    //internal ICCTagDataTypeText _text
    //{
    //  get
    //  {
    //    if (this._type != 0x74657874) return null;
    //    ICCTagDataTypeText v = new ICCTagDataTypeText();
    //    using (MemoryStream ms = new MemoryStream(this.data))
    //    {
    //      using (JpegBinaryReader br = new JpegBinaryReader(ms))
    //      {
    //        v.type = br.read_u4_be();
    //        v.reserved = br.read_u4_be();
    //        v.value = br.read_u1_be((int)this.Length - 8, 1);
    //      }
    //    }
    //    return v;
    //  }
    //}
  //}

  [XmlRoot("icc")]
  [XmlType("icc")]
  public class ICCProfile
  {
    // header (128 bytes)
    [XmlIgnore]
    public uint Length;       // total length of ICC stream  
    [XmlIgnore]
    public uint CMMType;
    [XmlIgnore]
    public uint ProfileVersionNumber;
    [XmlIgnore]
    public ICCProfileDeviceClass ProfileDeviceClass;
    [XmlIgnore]
    public ICCColorSpaceClass ColourSpace;
    [XmlIgnore]
    public ICCColorSpaceClass PCS;
    [XmlIgnore]
    public byte[] CreationDateTimeBytes;
    [XmlIgnore]
    public DateTime CreationDateTime;
    [XmlIgnore]
    public uint ProfileFileSignature; // 'acsp'
    [XmlIgnore]
    public ICCPrimaryPlatform PrimaryPlatform;
    [XmlIgnore]
    public ICCProfileFlags ProfileFlags;
    [XmlIgnore]
    public uint DeviceManufacturer;
    [XmlIgnore]
    public uint DeviceModel;
    [XmlIgnore]
    public ICCDeviceAttributes DeviceAttributes;
    [XmlIgnore]
    public ICCRenderingIntent RenderingIntent;
    [XmlIgnore]
    public byte[] CIEXYZValues; // 12 bytes
    [XmlIgnore]
    public ICCTXYZNumber CIEXYZValuesData;
    [XmlIgnore]
    public uint ProfileCreatorSignature;
    [XmlIgnore]    
    public byte[] ProfileID; // 16 bytes;    
    [XmlIgnore]
    public byte[] Reserved; // 28 bytes reserved for future use and it should be 00h    
    [XmlIgnore]
    public ICCTags Tags;   // [TagCount,12] array // tag table (TagCount * 12 bytes for each tag)

    [XmlIgnore]
    public byte[] data = new byte[0];

    #region ### xml ###

    [XmlAttribute("size")]
    public uint xml_size
    {
      get { return Length; }
      set { Length = value; }
    }

    [XmlElement("CMMType")]
    public string xml_CMMType
    {
      get { return string.Format("{0,8:X8}", CMMType); }
      set { CMMType = value.hex_to_u4(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlElement("CMMTypeName")]
    public string xml_CMMTypeName
    {
      get { return CMMType.chararray_as_str(); }
      set { /**/ }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlElement("ProfileVersionNumber")]
    public string xml_ProfileVersionNumber
    {
      get { return string.Format("{0:x8}", ProfileVersionNumber); }
      set { ProfileVersionNumber = value.hex_to_u4(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlElement("ProfileVersionNumberString")]
    public string xml_ProfileVersionNumberString
    {
      get { return string.Format("{0}.{1}.{2}.{3}", (byte)(ProfileVersionNumber>>24), (byte)(ProfileVersionNumber>>16), (byte)(ProfileVersionNumber>>8), (byte)(ProfileVersionNumber) ); }
      set { /**/ }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlElement("ProfileDeviceClass")]
    public string xml_ProfileDeviceClass
    {
      get { return string.Format("{0,8:X8}", (uint)ProfileDeviceClass); }
      set { ProfileDeviceClass = (ICCProfileDeviceClass)value.hex_to_u4(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlElement("ProfileDeviceClassName")]
    public string xml_ProfileDeviceClassName
    {
      get { return ((uint)ProfileDeviceClass).chararray_as_str(); }
      set { /**/ }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlElement("ProfileDeviceClassString")]
    public string xml_ProfileDeviceClassString
    {
      get { return ProfileDeviceClass.ToString(); }
      set { /**/ }
    }
    
    /// <summary>
    /// 
    /// </summary>
    [XmlElement("ColourSpace")]
    public string xml_ColourSpace
    {
      get { return string.Format("{0,8:X8}", (uint)ColourSpace); }
      set { ColourSpace = (ICCColorSpaceClass)value.hex_to_u4(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlElement("ColourSpaceName")]
    public string xml_ColourSpaceName
    {
      get { return ((uint)ColourSpace).chararray_as_str(); }
      set { /**/ }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlElement("ColourSpaceString")]
    public string xml_ColourSpaceString
    {
      get { return ColourSpace.ToString(); }
      set { /**/ }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlElement("PCS")]
    public string xml_PCS
    {
      get { return string.Format("{0,8:X8}", (uint)PCS); }
      set { PCS = (ICCColorSpaceClass)value.hex_to_u4(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [XmlElement("PCSName")]
    public string xml_PCSName
    {
      get { return ((uint)PCS).chararray_as_str(); }
      set { /**/ }
    }

    [XmlElement("PCSString")]
    public string xml_PCSString
    {
      get { return PCS.ToString(); }
      set { /**/ }
    }

    [ComVisible(false)] // speeds watch window display
    [XmlElement("CreationDateTime")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_CreationDateTime
    {
      get
      {
        if (CreationDateTimeBytes == null) return null;
        return CreationDateTimeBytes.to_hex_fmt(wrapindex: CreationDateTimeBytes.Length > 80 ? 80 : -1);
      }
      set { CreationDateTimeBytes = value.hex_to_a_u1(); }
    }
    
    [XmlElement("CreationDateTimeString")]    
    public string xml_CreationDateTimeString
    {
      get { return CreationDateTime.ToString("yyyy-MM-dd HH:mm:ss.ffffff"); }
      set { /**/ }
    }

    [XmlElement("ProfileFileSignature")]
    public string xml_ProfileFileSignature
    {
      get { return string.Format("{0,8:X8}", ProfileFileSignature); }
      set { ProfileFileSignature = value.hex_to_u4(); }
    }

    [XmlElement("ProfileFileSignatureName")]
    public string xml_ProfileFileSignatureName
    {
      get { return ProfileFileSignature.chararray_as_str(); }
      set { ProfileFileSignature = value.chararray_to_u4(); }
    }

    [XmlElement("PrimaryPlatform")]
    public string xml_PrimaryPlatform
    {
      get { return string.Format("{0,8:X8}", (uint)PrimaryPlatform); }
      set { PrimaryPlatform = (ICCPrimaryPlatform)value.hex_to_u4(); }
    }

    [XmlElement("PrimaryPlatformName")]
    public string xml_PrimaryPlatformName
    {
      get { return ((uint)PrimaryPlatform).chararray_as_str(); }
      set { /**/ }
    }

    [XmlElement("PrimaryPlatformString")]
    public string xml_PrimaryPlatformString
    {
      get { return PrimaryPlatform.ToString(); }
      set { /**/ }
    }

    // TODO: flags as bitfield for example : 00000000 00000000 00000000 00000000
    [XmlElement("ProfileFlags")]
    public string xml_ProfileFlags
    {
      get { return string.Format("{0,8:X8}", (uint)ProfileFlags); }
      set { ProfileFlags = (ICCProfileFlags)value.hex_to_u4(); }
    }

    //[XmlElement("ProfileFlagsName")]
    //public string xml_ProfileFlagsName
    //{
    //  get { return ((uint)ProfileFlags).chararray_as_str(); }
    //  set { /**/ }
    //}

    //[XmlElement("ProfileFlagsString")]
    //public string xml_ProfileFlagsString
    //{
    //  get { return ((uint)ProfileFlags).ToString(); }
    //  set { /**/ }
    //}

    [XmlElement("DeviceManufacturer")]
    public string xml_DeviceManufacturer
    {
      get { return string.Format("{0,8:X8}", DeviceManufacturer); }
      set { DeviceManufacturer = value.hex_to_u4(); }
    }

    [XmlElement("DeviceManufacturerName")]
    public string xml_DeviceManufacturerName
    {
      get { return DeviceManufacturer.chararray_as_str(); }
      set { /**/ }
    }

    [XmlElement("DeviceModel")]
    public string xml_DeviceModel
    {
      get { return string.Format("{0,8:X8}", DeviceModel); }
      set { DeviceModel = value.hex_to_u4(); }
    }

    [XmlElement("DeviceModelName")]
    public string xml_DeviceModelName
    {
      get { return DeviceModel.chararray_as_str(); }
      set { /**/ }
    }

    // TODO: flags as bitfield for example : 00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000000
    [XmlElement("DeviceAttributes")]
    public string xml_DeviceAttributes
    {
      get { return string.Format("{0,16:X16}", (ulong)DeviceAttributes); }
      set { DeviceAttributes = (ICCDeviceAttributes)value.hex_to_u8(); }
    }

    //[XmlElement("DeviceAttributesName")]
    //public string xml_DeviceAttributesName
    //{
    //  get { return ((ulong)DeviceAttributes).chararray_as_str(); }
    //  set { /**/ }
    //}

    [XmlElement("RenderingIntent")]
    public string xml_RenderingIntent
    {
      get { return string.Format("{0,8:X8}", (uint)RenderingIntent); }
      set { RenderingIntent = (ICCRenderingIntent)value.hex_to_u4(); }
    }

    //[XmlElement("RenderingIntentName")]
    //public string xml_RenderingIntentName
    //{
    //  get { return ((uint)RenderingIntent).chararray_as_str(); }
    //  set { /**/ }
    //}

    [XmlElement("RenderingIntentString")]
    public string xml_RenderingIntentString
    {
      get { return RenderingIntent.ToString(); }
      set { /**/ }
    }
    
    [ComVisible(false)] // speeds watch window display
    [XmlElement("CIEXYZValues")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_CIEXYZValues
    {
      get
      {
        if (CIEXYZValues == null) return null;
        return CIEXYZValues.to_hex_fmt(wrapindex: CIEXYZValues.Length > 80 ? 80 : -1);
      }
      set { CIEXYZValues = value.hex_to_a_u1(); }
    }

    [XmlElement("CIEXYZValuesData")]
    public ICCTXYZNumber xml_CIEXYZValuesData
    {
      get { return CIEXYZValuesData; }
      set { CIEXYZValuesData = value; }
    }

    [XmlElement("ProfileCreatorSignature")]
    public string xml_ProfileCreatorSignature
    {
      get { return string.Format("{0,8:X8}", ProfileCreatorSignature); }
      set { ProfileCreatorSignature = value.hex_to_u4(); }
    }

    [XmlElement("ProfileCreatorSignatureName")]
    public string xml_ProfileCreatorSignatureName
    {
      get { return ProfileCreatorSignature.chararray_as_str(); }
      set { ProfileCreatorSignature = value.chararray_to_u4(); }
    }    

    [ComVisible(false)] // speeds watch window display
    [XmlElement("ProfileID")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_ProfileID
    {
      get
      {
        if (ProfileID == null) return null;
        return ProfileID.to_hex_fmt(wrapindex: ProfileID.Length > 80 ? 80 : -1);
      }
      set { ProfileID = value.hex_to_a_u1(); }
    }

    // tag table (TagCount * 12 bytes for each tag)
    [XmlArray("tags")]
    [XmlArrayItem("tag", typeof(ICCTag))]
    public ICCTags xml_tags
    {
      get { return Tags; }
      set { Tags = value; }
    }

    #endregion

    internal void Push(byte[] ICCData)
    {
      int offset = data.Length;
      Array.Resize<byte>(ref data, offset + ICCData.Length);
      Buffer.BlockCopy(ICCData, 0, data, offset, ICCData.Length);
    }

    internal unsafe void Parse()
    {
      if (null == data || data.Length == 0)
      {
        return;
      }
      using (MemoryStream ms = new MemoryStream(data))
      {
        using (JpegBinaryReader br = new JpegBinaryReader(ms))
        {
          this.Length = br.read_u4_be();
          this.CMMType = br.read_u4_be();
          this.ProfileVersionNumber = br.read_u4_be();
          this.ProfileDeviceClass = (ICCProfileDeviceClass)br.read_u4_be();
          this.ColourSpace = (ICCColorSpaceClass)br.read_u4_be();
          this.PCS = (ICCColorSpaceClass)br.read_u4_be();
          this.CreationDateTimeBytes = br.read_u1_be(12, 1); // DateTime skip
          this.CreationDateTime = new DateTime(this.CreationDateTimeBytes[1] + (this.CreationDateTimeBytes[0] << 8),
                                               this.CreationDateTimeBytes[3] + (this.CreationDateTimeBytes[2] << 8),
                                               this.CreationDateTimeBytes[5] + (this.CreationDateTimeBytes[4] << 8),
                                               this.CreationDateTimeBytes[7] + (this.CreationDateTimeBytes[6] << 8),
                                               this.CreationDateTimeBytes[9] + (this.CreationDateTimeBytes[8] << 8),
                                               this.CreationDateTimeBytes[11] + (this.CreationDateTimeBytes[10] << 8));
          this.ProfileFileSignature = br.read_u4_be();
          this.PrimaryPlatform = (ICCPrimaryPlatform)br.read_u4_be();
          this.ProfileFlags = (ICCProfileFlags)br.read_u4_be();
          this.DeviceManufacturer = br.read_u4_be();
          this.DeviceModel = br.read_u4_be();
          this.DeviceAttributes = (ICCDeviceAttributes)br.read_u8_be();
          this.RenderingIntent = (ICCRenderingIntent)br.read_u4_be();

          this.CIEXYZValues = br.read_u1_be(12, 1);
          this.CIEXYZValuesData.data = this.CIEXYZValues;
          this.ProfileCreatorSignature = br.read_u4_be();
          this.ProfileID = br.read_u1_be(16, 1);
          this.Reserved = br.read_u1_be(28, 1);

          this.Tags = new ICCTags();
          this.Tags.TagCount = br.read_u4_be();
          for (int i = 0; i < Tags.TagCount; i++)
          {
            ICCTag tag = new ICCTag();
            tag.signature = br.read_u4_be();
            tag.offset = br.read_u4_be();
            tag.length = br.read_u4_be();
            byte[] tagdata = new byte[tag.length];
            Buffer.BlockCopy(data, (int)tag.offset, tagdata, 0, (int)tag.length);
            tag.data = tagdata;
            this.Tags.Add(tag);
          }

        }
      }

    }

  }

}