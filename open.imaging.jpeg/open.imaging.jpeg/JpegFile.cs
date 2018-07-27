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

// heart of open.imaging.jpeg assembly everything is read into/by this module

#if DEBUG
// #define DEBUG_JPEGFILE
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using open.imaging.icc;

namespace open.imaging.jpeg
{


  [ComVisible(true)]
  public delegate void JpegFileEventDelegate(object sender, JpegFileEventArgs e);


  [ComVisible(true)]
  [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
  public interface IJpegFileComEvents
  {
    [DispId(1)]
    void OnError(object sender, JpegFileEventArgs e);
    [DispId(2)]
    void OnWarning(object sender, JpegFileEventArgs e);
  }


  /// <summary>
  /// 
  /// </summary>
  [XmlRoot("JpegFile")]
  [XmlType("JpegFile")]
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDual)]
  [ComSourceInterfaces(typeof(IJpegFileComEvents))]
  public class JpegFile
  {
    #region ### events ###

    public event JpegFileEventDelegate OnError;
    public event JpegFileEventDelegate OnWarning;

    private void FireOnError(JpegFileEventArgs e)
    {
      if (null != this.OnError)
        this.OnError(this, e);
    }

    private void FireOnWarning(JpegFileEventArgs e)
    {
      if (null != this.OnWarning)
        this.OnWarning(this, e);
    }

    #endregion

    /// <summary>
    /// This flag determines type of reading that is performed.
    /// Check <see cref="JpegFileReadFlags"/> for more details.
    /// </summary>
    public JpegFileReadFlags ReadFlags = JpegFileReadFlags.ReadAll;

    [XmlIgnore]
    public string FileName;

    #region ### XML Serialization of array items as different element type names ###

    [XmlElement("segAPP", typeof(JpegFileSegAPP))]
    [XmlElement("segAPPICC", typeof(JpegFileSegAPPICC))]
    [XmlElement("segAPPADOBE", typeof(JpegFileSegAPPADOBE))]
    [XmlElement("segAPPPHOTO", typeof(JpegFileSegAPPPHOTO))]
    [XmlElement("segCOM", typeof(JpegFileSegCOM))]    
    [XmlElement("segData", typeof(JpegFileSegData))]
    [XmlElement("segEXIF", typeof(JpegFileSegEXIF))]
    [XmlElement("segJFIF", typeof(JpegFileSegJFIF))]
    [XmlElement("segJFXX", typeof(JpegFileSegJFXX))]    
    [XmlElement("segment")]
    [ComVisible(false)] // COM will use Item and Item_2 members
    public readonly List<JpegFileSeg> segments;

    /// <summary>
    /// COM Interface (Item)
    /// </summary>
    /// <param name="index">Index of segment in array.</param>
    /// <returns>Retuns the instance of indexed segment from the array.</returns>
    [XmlIgnore]
    public JpegFileSeg this[int index]
    {
      get { return segments[index]; }
      set { segments[index] = value; }
    }

    /// <summary>
    /// COM Interface (Item_2)
    /// </summary>
    /// <param name="markerId">markerId of the segment to be searched for first segment in array.</param>
    /// <returns>Retuns the instance of the first segment with markerId from the array.</returns>
    /// <remarks>Notice that array can actually hold more than one the same markerId segment.</remarks>
    [XmlIgnore]
    public JpegFileSeg this[JpegFileSegMarker markerId]
    {
      get
      {
        for (int i = 0; i < segments.Count; i++)
          if (segments[i].MarkerId == markerId) return segments[i];
        return null;
      }
    }

    [XmlIgnore]
    public int Count
    {
      get
      {
        return this.segments.Count;
      }
    }

    public void Add(JpegFileSeg segment)
    {
      this.segments.Add(segment);
    }

    public void Insert(int index, JpegFileSeg segment)
    {
      this.segments.Insert(index, segment);
    }

    #endregion

    public virtual string Dump()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine(this.ToString());
      for (int i = 0; i < this.Count; i++)
      {
        sb.AppendFormat("[{0,2}] {1}\r\n", i, this[i].Dump());
      }
      return sb.ToString();
    }

    public static readonly ReadOnlyArray<byte> APPn_SIGNATURE_JFIF;
    public static readonly ReadOnlyArray<byte> APPn_SIGNATURE_EXIF;
    public static readonly ReadOnlyArray<byte> APPn_SIGNATURE_PHOT;
    public static readonly ReadOnlyArray<byte> APPn_SIGNATURE_HTTP;
    public static readonly ReadOnlyArray<byte> APPn_SIGNATURE_ICC_;
    public static readonly ReadOnlyArray<byte> APPn_SIGNATURE_ADOB;
    public static readonly ReadOnlyArray<byte> APPn_SIGNATURE_DUCK;
    public static readonly ReadOnlyArray<byte> APPn_SIGNATURE_JFXX;
    public static readonly ReadOnlyArray<byte> APPn_SIGNATURE_FPXR;
    public static readonly ReadOnlyArray<byte> APPn_SIGNATURE_0x4d4d002a;
    public static readonly ReadOnlyArray<byte> APPn_SIGNATURE_0x3c786d70;
    public static readonly ReadOnlyArray<byte> APPn_SIGNATURE_0x20435335;
    
    public static readonly ReadOnlyArray<byte> CHARSET_ASCII;
    public static readonly ReadOnlyArray<byte> CHARSET_JIS;
    public static readonly ReadOnlyArray<byte> CHARSET_UNICODE;
    public static readonly ReadOnlyArray<byte> CHARSET_UNDEFINED;

    // initializes static variables
    static JpegFile()
    {
      //Table 9 Character Codes and their Designation
      //Character Code Code Designation (8 Bytes) References
      // ITU-T T.50 IA5
      CHARSET_ASCII     = new byte[]{ 0x41, 0x53, 0x43, 0x49, 0x49, 0x00, 0x00, 0x00 };
      // JIS X208-1990
      CHARSET_JIS       = new byte[]{ 0x4A, 0x49, 0x53, 0x00, 0x00, 0x00, 0x00, 0x00 };
      // Unicode Standard
      CHARSET_UNICODE   = new byte[]{ 0x55, 0x4E, 0x49, 0x43, 0x4F, 0x44, 0x45, 0x00 };
      // Undefined
      CHARSET_UNDEFINED = new byte[]{ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

      // TODO: need better recognition...

      // JFIF
      APPn_SIGNATURE_JFIF = new byte[] { 0x4a, 0x46, 0x49, 0x46 };

      // EXIF
      APPn_SIGNATURE_EXIF = new byte[] { 0x45, 0x78, 0x69, 0x66 };

      // APP13 "Photoshop 3.0\08BIM\..."
      APPn_SIGNATURE_PHOT = new byte[] { 0x50, 0x68, 0x6f, 0x74 };
      // "\r\n50686f746f73686f7020332e300

      // APP1 "http://ns.adobe.com/xap/1.0/\0..."
      APPn_SIGNATURE_HTTP = new byte[] { 0x68, 0x74, 0x74, 0x70 };

      // APP2 "ICC_PROFILE\0\0\0\fHLino\0\0mntrRGB XYZ..."
      APPn_SIGNATURE_ICC_ = new byte[] { 0x49, 0x43, 0x43, 0x5f };

      // APP14 "Adobe\0d@\0\0\0"
      APPn_SIGNATURE_ADOB = new byte[] { 0x41, 0x64, 0x6f, 0x62 };

      // APP12 "Duck\0d@\0\0\0"
      APPn_SIGNATURE_DUCK = new byte[] { 0x44, 0x75, 0x63, 0x6b };

      // APPn_SIGNATURE_JFXX
      APPn_SIGNATURE_JFXX = new byte[] { 0x4a, 0x46, 0x58, 0x58 };

      // APPn_SIGNATURE_FPXR
      APPn_SIGNATURE_FPXR = new byte[] { 0x46, 0x50, 0x58, 0x52 };

      // APPn_SIGNATURE_0x4d4d002a "MM *"
      APPn_SIGNATURE_0x4d4d002a = new byte[] { 0x4d, 0x4d, 0x00, 0x2a };

      // APPn_SIGNATURE_0x3c786d70 "<xmp"
      APPn_SIGNATURE_0x3c786d70 = new byte[] { 0x3c, 0x78, 0x6d, 0x70 };

      // APPn_SIGNATURE_0x20435335 " CS5"
      APPn_SIGNATURE_0x20435335 = new byte[] { 0x20, 0x43, 0x53, 0x35 };

    }
    
    public JpegFile()
    {
      this.segments = new List<JpegFileSeg>();
    }

    #region ### read ###

    #region ### WIN32 API ###

    [Flags]
    public enum EFileAccess : uint
    {
      GenericRead = 0x80000000,
      GenericWrite = 0x40000000,
      GenericExecute = 0x20000000,
      GenericAll = 0x10000000,
    }

    [Flags]
    public enum EFileShare : uint
    {
      None = 0x00000000,
      Read = 0x00000001,
      Write = 0x00000002,
      Delete = 0x00000004,
    }

    public enum ECreationDisposition : uint
    {
      New = 1,
      CreateAlways = 2,
      OpenExisting = 3,
      OpenAlways = 4,
      TruncateExisting = 5,
    }

    [Flags]
    public enum EFileAttributes : uint
    {
      Readonly = 0x00000001,
      Hidden = 0x00000002,
      System = 0x00000004,
      Directory = 0x00000010,
      Archive = 0x00000020,
      Device = 0x00000040,
      Normal = 0x00000080,
      Temporary = 0x00000100,
      SparseFile = 0x00000200,
      ReparsePoint = 0x00000400,
      Compressed = 0x00000800,
      Offline = 0x00001000,
      NotContentIndexed = 0x00002000,
      Encrypted = 0x00004000,
      Write_Through = 0x80000000,
      Overlapped = 0x40000000,
      NoBuffering = 0x20000000,
      RandomAccess = 0x10000000,
      SequentialScan = 0x08000000,
      DeleteOnClose = 0x04000000,
      BackupSemantics = 0x02000000,
      PosixSemantics = 0x01000000,
      OpenReparsePoint = 0x00200000,
      OpenNoRecall = 0x00100000,
      FirstPipeInstance = 0x00080000
    }

    //[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    //internal static extern SafeFileHandle CreateFileW(
    //  string lpFileName,
    //  EFileAccess dwDesiredAccess,
    //  EFileShare dwShareMode,
    //  IntPtr lpSecurityAttributes,
    //  ECreationDisposition dwCreationDisposition,
    //  EFileAttributes dwFlagsAndAttributes,
    //  IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern IntPtr CreateFileW(
      string lpFileName,
      EFileAccess dwDesiredAccess,
      EFileShare dwShareMode,
      IntPtr lpSecurityAttributes,
      ECreationDisposition dwCreationDisposition,
      EFileAttributes dwFlagsAndAttributes,
      IntPtr hTemplateFile);

    // typedef int BOOL
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern bool DeleteFileW(string path);

    // typedef int BOOL
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern bool CloseHandle(SafeFileHandle handle);

    // typedef int BOOL
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = false)]
    internal static extern bool CloseHandle(IntPtr handle);

    /// <summary>
    /// Long file names support
    /// </summary>
    internal bool FileExists(string FilePath)
    {
      string formattedName = @"\\?\" + FilePath;
      IntPtr handle = CreateFileW(formattedName, 
                                  EFileAccess.GenericRead, 
                                  EFileShare.Read, 
                                  IntPtr.Zero, 
                                  (ECreationDisposition)3, 
                                  0, 
                                  IntPtr.Zero);
      if (handle == new IntPtr(-1) || handle == IntPtr.Zero)
      {
        return false;
      }
      CloseHandle(handle);
      return true;
    }

    /// <summary>
    /// Long file names support
    /// </summary>
    internal bool FileDelete(string FilePath)
    {
      // support for long filenames
      string formattedName = @"\\?\" + FilePath;      
      return DeleteFileW(formattedName);
    }
    
    #endregion

    JpegBinaryReader reader = null;

    [DispId(1)]
    public void Read(string FileName, JpegFileReadFlags ReadFlags = JpegFileReadFlags.Segments)
    {
      
      this.FileName = FileName;
      // [NB:2016-06-24] old and insane bug with long paths
      string formattedName = @"\\?\" + FileName;
      /* SafeFileHandle - aparently doesn't work as desired throwing random SEHException */
      IntPtr fileHandle = CreateFileW(formattedName, 
                                      EFileAccess.GenericRead, 
                                      EFileShare.Read | EFileShare.Write, 
                                      IntPtr.Zero, 
                                      ECreationDisposition.OpenExisting, 
                                      0, 
                                      IntPtr.Zero);
      // Check for errors      
      if (fileHandle == new IntPtr(-1) || fileHandle == IntPtr.Zero)
      {
        int lastWin32Error = Marshal.GetLastWin32Error();
        throw new System.ComponentModel.Win32Exception(lastWin32Error);
      }
      //using (FileStream input = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      using (FileStream input = new FileStream(fileHandle, FileAccess.Read))
      {
        this.Read(input, ReadFlags);        
      }
    }

    [DispId(2)]
    public void Read(Stream input, JpegFileReadFlags ReadFlags = JpegFileReadFlags.Segments)
    {
      this.ReadFlags = ReadFlags;
      if (null != reader)
      {        
        reader = null;
        GC.Collect(0);
        GC.Collect(2);
      }
      reader = new JpegBinaryReader(input);
      //using (JpegBinaryReader reader = new JpegBinaryReader(input))
      {
        while (reader.Position < reader.Length - 1)
        {
          // must be in bigendian
          ushort id = reader.read_u2_be();                   // read 2 bytes
          long segment_address = reader.Position;  // remmember segment start address
          switch ((JpegFileSegMarker)id)
          {
            #region ### PADDING: markers pre padding with 0xFF (WORD|DWORD aligment) ###
            case JpegFileSegMarker._FFFF:
              {
                // markers can be pre padded with 0xFF to force WORD|DWORD aligment.
                reader.Position = reader.Position - 1;
              }
              break;
            #endregion
            #region ### SEGMENT(s): without size (length) ###
            case JpegFileSegMarker._FF00:
            case JpegFileSegMarker.SOI:
            case JpegFileSegMarker.EOI:
            case JpegFileSegMarker.RSTm0:
            case JpegFileSegMarker.RSTm2:
            case JpegFileSegMarker.RSTm3:
            case JpegFileSegMarker.RSTm4:
            case JpegFileSegMarker.RSTm5:
            case JpegFileSegMarker.RSTm6:
            case JpegFileSegMarker.RSTm7:
              {
                JpegFileSeg segment = new JpegFileSeg(this, (JpegFileSegMarker)id, reader);
                this.Add(segment);
              }
              break;
            #endregion
            #region ### SEGMENT(s): Application Specific Tags ###
            case JpegFileSegMarker.APP0:
            case JpegFileSegMarker.APP1:
            case JpegFileSegMarker.APP2:
            case JpegFileSegMarker.APP3:
            case JpegFileSegMarker.APP4:
            case JpegFileSegMarker.APP5:
            case JpegFileSegMarker.APP6:
            case JpegFileSegMarker.APP7:
            case JpegFileSegMarker.APP8:
            case JpegFileSegMarker.APP9:
            case JpegFileSegMarker.APP10:
            case JpegFileSegMarker.APP11:
            case JpegFileSegMarker.APP12:
            case JpegFileSegMarker.APP13:
            case JpegFileSegMarker.APP14:
            case JpegFileSegMarker.APP15:
              {
                // read segment size
                ushort segment_size = reader.read_u2_be();
                bool IsRead = false;

                if (this.ReadFlags.HasFlag(JpegFileReadFlags.ParseAPPn))
                {
                  // extract signature    
                  ReadOnlyArray<byte> signature = new ReadOnlyArray<byte>(reader.read_u1(4));

                  //
                  // TODO: requires better recognition (but it works well for now)
                  //

                  // parse by signature
                  #region ### APP::EXIF ###
                  if (signature == JpegFile.APPn_SIGNATURE_EXIF)
                  {
                    
                    // go back to start of segment
                    reader.Position = segment_address;
                    JpegFileSeg segment = new JpegFileSegEXIF(this, (JpegFileSegMarker)id, reader);
                    this.Add(segment);                    
                    segment.OnError += new JpegFileEventDelegate(segment_OnError);
                    segment.OnWarning += new JpegFileEventDelegate(segment_OnWarning);
                    IsRead = segment.Read(reader); // read/decode APPn segment data as EXIF
                    
                  }
                  #endregion
                  #region ### APP::JFIF ###
                  else if (signature == JpegFile.APPn_SIGNATURE_JFIF)
                  {
                    // go back to start of segment
                    reader.Position = segment_address;
                    JpegFileSeg segment = new JpegFileSegJFIF(this, (JpegFileSegMarker)id, reader);
                    this.Add(segment);
                    segment.OnError += new JpegFileEventDelegate(segment_OnError);
                    segment.OnWarning += new JpegFileEventDelegate(segment_OnWarning);
                    IsRead = segment.Read(reader); // read/decode APPn segment data as JFIF
                  }
                  #endregion
                  #region ### APP::JFXX ###
                  else if (signature == JpegFile.APPn_SIGNATURE_JFXX)
                  {
                    // go back to start of segment
                    reader.Position = segment_address;
                    JpegFileSeg segment = new JpegFileSegJFXX(this, (JpegFileSegMarker)id, reader);
                    this.Add(segment);
                    segment.OnError += new JpegFileEventDelegate(segment_OnError);
                    segment.OnWarning += new JpegFileEventDelegate(segment_OnWarning);
                    // read/decode APPn segment data as JFXX
                    IsRead = segment.Read(reader); 
                  }
                  #endregion
                  #region ### APP::ICC ###
                  else if (signature == JpegFile.APPn_SIGNATURE_ICC_) 
                  {
                    // go back to start of segment
                    reader.Position = segment_address;
                    JpegFileSeg segment = new JpegFileSegAPPICC(this, (JpegFileSegMarker)id, reader);
                    this.Add(segment);
                    segment.OnError += new JpegFileEventDelegate(segment_OnError);
                    segment.OnWarning += new JpegFileEventDelegate(segment_OnWarning);
                    // read/decode APPn segment data as ICC
                    IsRead = segment.Read(reader);                     
                  }
                  #endregion
                  #region ### APP::ADOB ###
                  else if (signature == JpegFile.APPn_SIGNATURE_ADOB)
                  {
                    // go back to start of segment
                    reader.Position = segment_address;
                    JpegFileSeg segment = new JpegFileSegAPPADOBE(this, (JpegFileSegMarker)id, reader);
                    this.Add(segment);
                    segment.OnError += new JpegFileEventDelegate(segment_OnError);
                    segment.OnWarning += new JpegFileEventDelegate(segment_OnWarning);
                    // read/decode APPn segment data as ADOBE
                    IsRead = segment.Read(reader);
                  }
                  #endregion
                  #region ### APP::PHOT ###
                  else if (signature == JpegFile.APPn_SIGNATURE_PHOT)
                  {
                    // go back to start of segment
                    reader.Position = segment_address;
                    JpegFileSeg segment = new JpegFileSegAPPPHOTO(this, (JpegFileSegMarker)id, reader);
                    this.Add(segment);
                    segment.OnError += new JpegFileEventDelegate(segment_OnError);
                    segment.OnWarning += new JpegFileEventDelegate(segment_OnWarning);
                    // read/decode APPn segment data as ADOBE
                    IsRead = segment.Read(reader);                  
                  }
                  #endregion                  
#pragma warning disable 642
                  else if (signature == JpegFile.APPn_SIGNATURE_HTTP) ;
                  else if (signature == JpegFile.APPn_SIGNATURE_DUCK) ;
                  else if (signature == JpegFile.APPn_SIGNATURE_FPXR) ;
                  else if (signature == JpegFile.APPn_SIGNATURE_0x4d4d002a) ;
                  else if (signature == JpegFile.APPn_SIGNATURE_0x3c786d70) ;
                  else if (signature == JpegFile.APPn_SIGNATURE_0x20435335) ;
#pragma warning restore 642
                  #region ### APP::else ###
                  else
                  {
                    FireOnWarning(new JpegFileEventArgs(
                      segment_address,
                      JpegFileEventId.WARNING_SIGNATURE_UNKNOWN,
                      string.Format("Unrecognized '{0}' signature: '{1}' [{2}].",                        
                        (JpegFileSegMarker)id,
                        ((byte[])signature).to_str(),
                        ((byte[])signature).to_hex()                        
                      )));
                  }
                  #endregion
                  // else unknown signature
                }
                if (!IsRead)
                {
                  // go back to start of segment
                  reader.Position = segment_address;
                  JpegFileSeg segment = new JpegFileSegAPP(this, (JpegFileSegMarker)id, reader);
                  this.Add(segment);
                  if (this.ReadFlags.HasFlag(JpegFileReadFlags.ReadData))
                  {
                    segment.OnError += new JpegFileEventDelegate(segment_OnError);
                    segment.OnWarning += new JpegFileEventDelegate(segment_OnWarning);
                    segment.Read(reader); // read APPn as data segment
                  }
                }
                reader.Position = segment_address + segment_size;  // advance to next marker position
              }
              break;
            #endregion
            #region ### SEGMENT: Start Of Scan (Image Data) ###
            case JpegFileSegMarker.SOS:
              {
                // NOTICE: we are unable to determine the size of this block without decoder.
                //         and any following frame/scan blocks starting addresses.
                //         there for we are going to seek last marker (EOI), and continue parser from there.
                //                
                JpegFileSeg segment = new JpegFileSegData(this, (JpegFileSegMarker)id, reader);
                this.Add(segment);
                // seek End Of Image (EOI) marker
                reader.Position = reader.Length - 2;
                long endofimage = SeekBack(reader, JpegFileSegMarker.EOI);
                if (endofimage == -1) endofimage = reader.Length; // read to the EOF (end of file)
                if (this.ReadFlags.HasFlag(JpegFileReadFlags.ReadData))
                {
                  reader.Position = (segment_address + 2);     // start = SOS + sizeof(length)
                  long size = endofimage - (segment_address + 2);         // size = EOI - (SOS + sizeof(length))
                  segment.OnError += new JpegFileEventDelegate(segment_OnError);
                  segment.OnWarning += new JpegFileEventDelegate(segment_OnWarning);
                  segment.Read(reader, size);                                 // read data
                }
                // set up next address
                reader.Position = endofimage;
              }
              break;
            #endregion
            #region ### SEGMENT: Comment ###
            case JpegFileSegMarker.COM:
              {
                JpegFileSeg segment = new JpegFileSegCOM(this, (JpegFileSegMarker)id, reader);
                this.Add(segment);
                if (this.ReadFlags.HasFlag(JpegFileReadFlags.ReadData))
                {
                  segment.OnError += new JpegFileEventDelegate(segment_OnError);
                  segment.OnWarning += new JpegFileEventDelegate(segment_OnWarning);
                  segment.Read(reader);                                       // read data
                }
                reader.Position = segment_address + segment.size;  // advance to next marker position
              }
              break;
            #endregion
            #region ### SEGMENT(s): undefined segments by this parser, and we are assuming they have data length ###
            default:
              {

                // we do not know the tag, check for correctness of tag
                if (!IsMarker(id))
                {
                  // EOF
                  if (reader.Position >= reader.Length - 1)
                    break;

                  // Debug.Assert(false || id == 0);
                  // implemented seek tag function to correct incorrect images

                  long new_address = SeekFore(reader);
                  if (new_address != -1)
                  {
                    FireOnWarning(new JpegFileEventArgs()
                    {
                      Address = segment_address,
                      Id = JpegFileEventId.WARNING_INVALID_MARKER_ID,
                      Message = string.Format("Marker (0x{0:x4}) is invalid, (recovery address: {1:x8}).", id, new_address)
                    });
                    // continue reading from this address
                    reader.Position = new_address;
                    continue;
                  }
                  else
                  {
                    FireOnError(new JpegFileEventArgs()
                    {
                      Address = segment_address,
                      Id = JpegFileEventId.ERROR_INVALID_MARKER_ID,
                      Message = string.Format("Marker (0x{0:x4}) is invalid.", id, segment_address)
                    });

                    // we are unable to recover (no markers found)
                    throw (new JpegFileReadException(string.Format("Marker (0x{0:x4}) is invalid (address: 0x{1:x8}).", id, segment_address)));
                  }
                }
                // could throw exception on invalid data structure
                {
                  JpegFileSeg segment = new JpegFileSegData(this, (JpegFileSegMarker)id, reader);
                  this.Add(segment);

                  if (this.ReadFlags.HasFlag(JpegFileReadFlags.ReadData))
                  {
                    segment.OnError += new JpegFileEventDelegate(segment_OnError);
                    segment.OnWarning += new JpegFileEventDelegate(segment_OnWarning);
                    // read data (depending on segment type)
                    segment.Read(reader);
                  }
                  else
                  {
                    // override to read image dimmensions
                    switch ((JpegFileSegMarker)id)
                    {
                      case JpegFileSegMarker.SOF0:
                      case JpegFileSegMarker.SOF1:
                      case JpegFileSegMarker.SOF2:
                      case JpegFileSegMarker.SOF3:
                        segment.OnError += new JpegFileEventDelegate(segment_OnError);
                        segment.OnWarning += new JpegFileEventDelegate(segment_OnWarning);
                        // read data (depending on segment type)
                        segment.Read(reader);
                        break;
                    }
                  }
                  // advance to next marker position
                  reader.Position = segment_address + segment.size;  
                }
              }
              break;
            #endregion
          } // switch ((JpegFileTag)id)...
        } // (fs.Position < fs.Length)...
      } // using(JpegFileReader...)     
    }

    /// <summary>
    /// returns true if value is in format of JPEG tag (value &amp; 0xff == 0xff).
    /// </summary>
    static bool IsMarker(ushort value)
    {
      return ((value & (ushort)JpegFileSegMarker._FF00) == (ushort)JpegFileSegMarker._FF00);
    }

    /// <summary>
    /// returns true if value is one of defined markers, otherwise false.
    /// </summary>
    static bool IsMarkerValid(ushort value)
    {
      if (!IsMarker(value)) return false;
      switch ((JpegFileSegMarker)value)
      {
        //case JpegFileSegMarker._FF00:
        case JpegFileSegMarker._FFFF:
        case JpegFileSegMarker.APP0:
        case JpegFileSegMarker.APP1:
        case JpegFileSegMarker.APP2:
        case JpegFileSegMarker.APP3:
        case JpegFileSegMarker.APP4:
        case JpegFileSegMarker.APP5:
        case JpegFileSegMarker.APP6:
        case JpegFileSegMarker.APP7:
        case JpegFileSegMarker.APP8:
        case JpegFileSegMarker.APP9:
        case JpegFileSegMarker.APP10:
        case JpegFileSegMarker.APP11:
        case JpegFileSegMarker.APP12:
        case JpegFileSegMarker.APP13:
        case JpegFileSegMarker.APP14:
        case JpegFileSegMarker.APP15:
        case JpegFileSegMarker.COM:
        case JpegFileSegMarker.DAC:
        case JpegFileSegMarker.DHP:
        case JpegFileSegMarker.DHT:
        case JpegFileSegMarker.DNL:
        case JpegFileSegMarker.DQT:
        case JpegFileSegMarker.DRI:
        case JpegFileSegMarker.EOI:
        case JpegFileSegMarker.EXP:
        case JpegFileSegMarker.JEX0:
        case JpegFileSegMarker.JEX1:
        case JpegFileSegMarker.JEX2:
        case JpegFileSegMarker.JEX3:
        case JpegFileSegMarker.JEX4:
        case JpegFileSegMarker.JEX5:
        case JpegFileSegMarker.JEX6:
        case JpegFileSegMarker.JEX7:
        case JpegFileSegMarker.JEX8:
        case JpegFileSegMarker.JEX9:
        case JpegFileSegMarker.JEX10:
        case JpegFileSegMarker.JEX11:
        case JpegFileSegMarker.JEX12:
        case JpegFileSegMarker.JEX13:
        case JpegFileSegMarker.JPG:
        case JpegFileSegMarker.RSTm0:
        case JpegFileSegMarker.RSTm1:
        case JpegFileSegMarker.RSTm2:
        case JpegFileSegMarker.RSTm3:
        case JpegFileSegMarker.RSTm4:
        case JpegFileSegMarker.RSTm5:
        case JpegFileSegMarker.RSTm6:
        case JpegFileSegMarker.RSTm7:
        case JpegFileSegMarker.SOF0:
        case JpegFileSegMarker.SOF1:
        case JpegFileSegMarker.SOF2:
        case JpegFileSegMarker.SOF3:
        case JpegFileSegMarker.SOF5:
        case JpegFileSegMarker.SOF6:
        case JpegFileSegMarker.SOF7:
        case JpegFileSegMarker.SOF9:
        case JpegFileSegMarker.SOF10:
        case JpegFileSegMarker.SOF11:
        case JpegFileSegMarker.SOF13:
        case JpegFileSegMarker.SOF14:
        case JpegFileSegMarker.SOF15:
        case JpegFileSegMarker.SOI:
        case JpegFileSegMarker.SOS:
        case JpegFileSegMarker.TEM:
          return true;
      }
      return false;
    }

    /// <summary>
    /// returns address if any marker was found, otherwise -1 to indicate marker not found state.
    /// </summary>
    static long SeekFore(JpegBinaryReader reader)
    {
      long address_cur = reader.Position;
      // step back 1 byte (offset 1 byte from previous 2 byte read)
      reader.Position -= 1;
      // must be in bigendian
      ushort rMarkerId = reader.read_u2_be();
      while (!IsMarkerValid(rMarkerId) && reader.Position > address_cur && reader.Position < reader.Length - 2)
      {
        reader.Position = reader.Position - 1;
        // must be in bigendian
        rMarkerId = reader.read_u2_be();
      }
      if (IsMarkerValid(rMarkerId)) return (reader.Position - 2);
      return -1L;
    }

    /// <summary>
    /// returns address if marker we seek was found, otherwise -1 to indicate marker not found state.
    /// </summary>
    static long SeekBack(JpegBinaryReader reader, JpegFileSegMarker marker)
    {
      long address_cur = reader.Position;
      // must be in bigendian
      JpegFileSegMarker mm = (JpegFileSegMarker)reader.read_u2_be();
      while (reader.Position > address_cur && mm != marker)
      {
        reader.Position = reader.Position - 3;
        // must be in bigendian
        mm = (JpegFileSegMarker)reader.read_u2_be();
      }
      if (mm == marker) return (reader.Position - 2);
      return -1L;
    }
   
    void segment_OnError(object sender, JpegFileEventArgs e)
    {
      FireOnError(e);
    }

    void segment_OnWarning(object sender, JpegFileEventArgs e)
    {
      FireOnWarning(e);
    }

    #endregion

    #region ### write ###

    JpegBinaryWriter writer = null;

    /// <summary>
    /// Writes internal JpegFile structure into file specified with FileName argument.
    /// If FileName argument is not specified, or it is a null, it uses FileName member.
    /// And if both argument and member FileName's are null it throws ArgumentNullException.
    /// </summary>
    /// <param name="FileName">[Optional] Full filename path.</param>
    /// <param name="WriteLittleEndian">[Optional] Write endian.</param>
    /// <param name="OverWrite">[Optional] force write even if file already exists. Original file will be deleted.</param>
    [DispId(3)]
    public void Write(string FileName = null, bool WriteLittleEndian = true, bool OverWrite = true)
    {
      if (FileName == null)
      {
        FileName = this.FileName;
      }
      if (FileName == null)
      {
        throw (new ArgumentNullException("<JpegFile>.Write: FileName is null and <JpegFile>.FileName is null"));
      }
      if (FileExists(FileName) && !OverWrite)
      {
        throw new IOException(string.Format("<JpegFile>.Write: File already exists: '{0}'.", FileName));
      }
      else
      {
        FileDelete(FileName);
      }
      // support for long filenames
      string formattedName = @"\\?\" + FileName;
      // Create a file with generic write access
      IntPtr fileHandle = CreateFileW(formattedName, 
                                      EFileAccess.GenericWrite, 
                                      EFileShare.Read | EFileShare.Write, 
                                      IntPtr.Zero, 
                                      ECreationDisposition.CreateAlways, 
                                      0, 
                                      IntPtr.Zero);
      // Checking for win32 errors
      int lastWin32Error = Marshal.GetLastWin32Error();
      if (fileHandle == new IntPtr(-1) || fileHandle == IntPtr.Zero)
      {
        throw new System.ComponentModel.Win32Exception(lastWin32Error);
      }
      // write
      using (FileStream output = new FileStream(fileHandle, FileAccess.Write))
      {
        Write(output, WriteLittleEndian);
      }
    }

    /// <summary>
    /// Writes binary data
    /// </summary>
    /// <param name="output"></param>
    /// <param name="WriteLittleEndian"></param>
    /// <remarks>
    /// NOTICE: Some PS and ACDSee doesn't handle bigendian byte orders. 
    ///         We should be enforcing little endian write regardless.
    /// </remarks>
    [DispId(4)]
    public void Write(Stream output, bool WriteLittleEndian = true)
    {
      if (null != writer)
      {
        writer = null;
        GC.Collect(0);
        GC.Collect(2);
      }
      writer = new JpegBinaryWriter(output);
      {
        writer.Position = 0;
        writer.WriteLittleEndian = WriteLittleEndian;
        foreach (JpegFileSeg segment in this.segments)
        {
          JpegFileSegMarker id = segment.MarkerId;

#if DEBUG_JPEGFILE
          Debug.Print("SEG [0x{0:X8}] : (0x{1:X4}) {2}", writer.Position, (ushort)id, (ushort)id);
#endif
          // must be in bigendian
          writer.write_u2_be((ushort)id);                  // write 2 bytes
          segment.Write(writer);

        }
        writer.Flush();
      }
    }

    #endregion

    #region ### Query Interface ###

    [DispId(5)]
    public IFDEntries Query(string QueryPath)
    {
      string[] QueryPathPairs = QueryPath.Split(new char[]{ '\\', '/' });
      JpegFileSegMarker markerID;
      uint[] QueryPathTags = QueryParser(QueryPathPairs, out markerID);
      if (QueryPathTags.Length == 0) return new IFDEntries();
      return Query(markerID, QueryPathTags); 
    }

    [DispId(6)]
    public static uint[] QueryParser(string[] QueryPath, out JpegFileSegMarker markerId)
    {
      markerId = JpegFileSegMarker._FFFF; // all segments ...
      if (QueryPath.Length == 0) return null;
      int QueryPathOffset = 1;
      
      if (QueryPath[0].Length > 0 && "!^#$xX?".IndexOf(QueryPath[0][0]) != -1)
      {
        QueryPathOffset = 0;
      }
      switch (QueryPath[0].ToUpper())
      {
        case "APP0": markerId = JpegFileSegMarker.APP0; break;
        case "APP1": markerId = JpegFileSegMarker.APP1; break;
        case "APP2": markerId = JpegFileSegMarker.APP2; break;
        case "APP3": markerId = JpegFileSegMarker.APP3; break;
        case "APP4": markerId = JpegFileSegMarker.APP4; break;
        case "APP5": markerId = JpegFileSegMarker.APP5; break;
        case "APP6": markerId = JpegFileSegMarker.APP6; break;
        case "APP7": markerId = JpegFileSegMarker.APP7; break;
        case "APP8": markerId = JpegFileSegMarker.APP8; break;
        case "APP9": markerId = JpegFileSegMarker.APP9; break;
        case "APP10": markerId = JpegFileSegMarker.APP10; break;
        case "APP11": markerId = JpegFileSegMarker.APP11; break;
        case "APP12": markerId = JpegFileSegMarker.APP12; break;
        case "APP13": markerId = JpegFileSegMarker.APP13; break;
        case "APP14": markerId = JpegFileSegMarker.APP14; break;
        case "APP15": markerId = JpegFileSegMarker.APP15; break;
      }

      uint[] QueryPathTags = new uint[QueryPath.Length - QueryPathOffset];
      for (int i = 0; i < QueryPathTags.Length; i++)
      {
        string tagName = QueryPath[i + QueryPathOffset];
        bool tagConverted = false;

        if (tagName.Length == 0)
        {
          QueryPathTags[i] = QueryPathTags[i] | IFD.QUERY_CTRL_ANY;
          tagConverted = true;
        }
        while (!tagConverted)
        {
          // check for control chars
          if (tagName.Length > 0)
          {
            switch (tagName[0])
            {
              case '*':
                tagName = tagName.Substring(1);
                QueryPathTags[i] = QueryPathTags[i] | IFD.QUERY_CTRL_ANY;
                tagConverted = tagName.Length == 0;
                continue;
              case '?':
                tagName = tagName.Substring(1);
                QueryPathTags[i] = QueryPathTags[i] | IFD.QUERY_CTRL_FIRST;
                tagConverted = tagName.Length == 0;
                continue;
              case '$':              
                tagName = tagName.Substring(1);
                QueryPathTags[i] = QueryPathTags[i] | IFD.QUERY_CTRL_ANYLEVEL;
                tagConverted = tagName.Length == 0;
                continue;
              case '!':
              case '^':
                tagName = tagName.Substring(1);
                QueryPathTags[i] = QueryPathTags[i] | IFD.QUERY_CTRL_NOT;
                tagConverted = tagName.Length == 0;
                continue;
            }
          }
          uint tag_ui4 = 0;
          IFDTag tag;
          // try literal
          if (Enum.TryParse<IFDTag>(tagName, true, out tag))
          {
            QueryPathTags[i] = (QueryPathTags[i] & IFD.QUERY_CTRL_MASK) | (uint)tag;
            tagConverted = true;
            break;
          }
          // try numeric
          if (uint.TryParse(tagName, out tag_ui4))
          {
            QueryPathTags[i] = (QueryPathTags[i] & IFD.QUERY_CTRL_MASK) | tag_ui4;
            tagConverted = true;
            break;
          }
          // try hex
          if (tagName[0] == 'x' || tagName[0] == 'X' || tagName[0] == 'h' || tagName[0] == 'H' || tagName[0] == '#')
          {
            tagName = tagName.Substring(1);
            if (tagName.hex_try_to_u4(out tag_ui4))
            {
              QueryPathTags[i] = (QueryPathTags[i] & IFD.QUERY_CTRL_MASK) | tag_ui4;
              tagConverted = true;
              break;
            }
          }
          // no more notations supported, exit parser
          throw (new ArgumentException("Tag: (" + tagName + ") can't be converted to internal IFDTag. Please check supported notations."));
        }
      }
      return QueryPathTags;
    }

    [DispId(7)]
    public IFDEntries Query(JpegFileSegMarker markerID, uint[] QueryPathTags)
    {
      IFDEntries entries = new IFDEntries();
      bool quit = false;
      for (int i = 0; i < segments.Count && !quit; i++)
      { 
        if (markerID == JpegFileSegMarker._FFFF || this[i].MarkerId == markerID)
        {
          JpegFileSegEXIF SEG = this[i] as JpegFileSegEXIF;
          if (SEG != null && SEG.IFD0 != null)
          {
            IFDEntries subentries = SEG.IFD0.Query(QueryPathTags, ref quit);
            entries.AddRange(subentries.entries);
            if (quit) break;
          }
        }
      }
      return entries;
    }

    // current state: it's in experimental stage only
    public ICCProfile GetICC()
    {
      ICCProfile icc = new ICCProfile();
      for (int i = 0; i < segments.Count; i++)
      {
        JpegFileSegAPPICC segICC = this[i].AsICC;
        if (segICC != null)
        {
          icc.Push(segICC.ICCData);
        }
      }
      icc.Parse();
      return icc;
    }

    #endregion
  }
}
