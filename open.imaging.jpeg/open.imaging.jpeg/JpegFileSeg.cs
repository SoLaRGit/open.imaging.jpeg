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
using System.Xml.Serialization;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace open.imaging.jpeg
{

  /// <summary>
  /// Base Jpeg Segment class that contains marker id, and start address of data block.
  /// </summary>

  // XmlInclude is no longer required with GetDerivedTypes() in generic serializer
  // but required to have xml tag name rather than d2p1:type="segXXXX" attribute on segment element
  [XmlInclude(typeof(JpegFileSegAPP))]
  [XmlInclude(typeof(JpegFileSegAPPICC))]
  [XmlInclude(typeof(JpegFileSegAPPADOBE))]
  [XmlInclude(typeof(JpegFileSegAPPPHOTO))]
  [XmlInclude(typeof(JpegFileSegCOM))]
  [XmlInclude(typeof(JpegFileSegData))]
  [XmlInclude(typeof(JpegFileSegEXIF))]
  [XmlInclude(typeof(JpegFileSegJFIF))]
  [XmlInclude(typeof(JpegFileSegJFXX))]  
  [XmlRoot("seg")]
  [XmlType("seg")]
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDual)]
  [ComSourceInterfaces(typeof(IJpegFileComEvents))]
  public class JpegFileSeg
  {
    #region ### events ###

    public event JpegFileEventDelegate OnError;

    public event JpegFileEventDelegate OnWarning;

    internal void FireOnError(JpegFileEventArgs e)
    {
      if (null != this.OnError)
        this.OnError(this, e);
    }

    internal void FireOnWarning(JpegFileEventArgs e)
    {
      if (null != this.OnWarning)
        this.OnWarning(this, e);
    }

    #endregion
    /// <summary>
    /// specifies start address of data block.
    /// </summary>
    [XmlIgnore]
    public long _address;

    /// <summary>
    /// specifies this segment type.
    /// </summary>
    [XmlIgnore]
    public JpegFileSegMarker MarkerId;

    /// <summary>
    /// Total APPn field byte count, including the byte count value (2 bytes), but excluding the APPn marker itself.     
    /// </summary>
    [XmlIgnore]
    public long size;

    /// <summary>
    /// Total APPn field byte count, including the byte count value (2 bytes), but excluding the APPn marker itself. Of type Int32
    /// </summary>
    [XmlIgnore]
    public int size_i32
    {
      get { return unchecked((int)size); }
    }
    
    /// <summary>
    /// contains data of segment
    /// </summary>
    [XmlIgnore]
    public byte[] data;

    #region ### XML SERIALIZATION PROPERTIES ###

    [XmlAttribute(AttributeName = "a")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_address
    {
      get { return string.Format("{0:x8}", (ulong)_address); }
      set { _address = value.hex_to_i8(); }
    }

    [XmlAttribute(AttributeName = "id")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_markerid
    {
      get { return string.Format("{0:x4}", (ushort)MarkerId); }
      set { MarkerId = (JpegFileSegMarker)value.hex_to_u4(); }
    }

    [XmlAttribute(AttributeName = "nm")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_markerid_name
    {
      get { return string.Format("{0,-5}", MarkerId); }
      set { /* ignore */ }
    }
    
    [XmlAttribute(AttributeName = "cb")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_size
    {
      get { return string.Format("{0:x4}", (ushort)size); }
      set { size = value.hex_to_u4(); }
    }

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

    /// <summary>
    /// specifies JpegFile instance this segment belongs to.
    /// </summary>
    [XmlIgnore]
    public JpegFile Parent;

    internal JpegFileSeg()
    { }

    /// <summary>
    /// Initializes new JpegFileSeg class with JpegFileSegMarker id.
    /// </summary>
    /// <param name="File">Base JpegFile instance used for parent member.</param>
    /// <param name="MarkerId">specifies this segment type.</param>
    public JpegFileSeg(JpegFile File, JpegFileSegMarker MarkerId)
    {
      this.Parent = File;
      this.MarkerId = MarkerId;
    }

    /// <summary>
    /// Initializes new JpegFileSeg class with JpegFileSegMarker id, and start address of data block.
    /// </summary>
    /// <param name="File">Base JpegFile instance used for parent member.</param>    
    /// <param name="MarkerId">specifies this segment type.</param>
    /// <param name="reader">reader to take current position as start of data block.</param>
    public JpegFileSeg(JpegFile File, JpegFileSegMarker MarkerId, JpegBinaryReader reader)
      : this(File, MarkerId)
    {
      this._address = reader.Position;
    }

    public virtual bool Read(JpegBinaryReader reader)
    {
      return true;
    }

    public virtual bool Read(JpegBinaryReader reader, long length)
    {
      return true;
    }

    public virtual bool Write(JpegBinaryWriter writer)
    {
      return true;
    }

    public virtual int Compare(JpegFileSeg other)
    {
      if (null == other)
      {
        throw (new ArgumentNullException("JpegFileSeg other argument must be specified."));
      }
      if (data.Length != other.data.Length)
      {
        return data.Length.CompareTo(other.data.Length);
      }
      for (int i = 0; i < other.data.Length; i++)
      {
        if (data[i] != other.data[i])
        {
          return data[i].CompareTo(other.data[i]);
        }
      }
      return 0; // EQUAL
    }

    public override string ToString()
    {
      return string.Format("JpegFileSeg:{{ a: 0x{0,8:x8}, id: {1,-4} ({2,4:X4}.h) }}", 
                            _address,                             
                            MarkerId,
                            ((ushort)MarkerId));
    }

    public virtual string Dump()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append(this.ToString());
      return sb.ToString();
    }

    public JpegFileSegAPP AsAPP
    {
      get { return this as JpegFileSegAPP; }
    }

    public JpegFileSegCOM AsCOM
    {
      get { return this as JpegFileSegCOM; }
    }

    public JpegFileSegEXIF AsEXIF
    {
      get { return this as JpegFileSegEXIF; }
    }

    public JpegFileSegJFIF AsJFIF
    {
      get { return this as JpegFileSegJFIF; }
    }

    public JpegFileSegData AsDATA
    {
      get { return this as JpegFileSegData; }
    }

    public JpegFileSegJFXX AsJFXX
    {
      get { return this as JpegFileSegJFXX; }
    }

    public JpegFileSegAPPICC AsICC
    {
      get { return this as JpegFileSegAPPICC; }
    }
  }

}
