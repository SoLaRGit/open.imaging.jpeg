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
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace open.imaging.jpeg
{

  [ComVisible(true)]
  public delegate void IFDEventDelegate(object sender, IFDEventArgs e);

  [ComVisible(true)]
  [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
  public interface IIFDComEvents
  {
    [DispId(1)]
    void OnError(object sender, IFDEventArgs e);
    [DispId(2)]
    void OnWarning(object sender, IFDEventArgs e);
  }

  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDual)]
  [ComSourceInterfaces(typeof(IIFDComEvents))]
  public class IFD //: List<IFDEntry>
  {
    
    #region ### events ###

    public event IFDEventDelegate OnError = null;

    public event IFDEventDelegate OnWarning = null;

    [XmlIgnore]
    public IFD Root
    {
      get
      {
        IFD root = this;
        while (root.ParentEntry != null && root.ParentEntry.ParentIFD != null)
        {
          root = root.ParentEntry.ParentIFD;
        }
        return root;
      }
    }

    internal void FireOnWarning(IFDEventArgs e)
    {
      if (OnWarning != null)
      {
        OnWarning(this, e);
      }
      else if (!Object.ReferenceEquals(Root, this))
      {
        Root.FireOnWarning(e);
      }
    }

    internal void FireOnError(IFDEventArgs e)
    {
      if (OnError != null)
      {
        OnError(this, e);
      }
      else if (!Object.ReferenceEquals(Root, this))
      {
        Root.FireOnError(e);
      }
    }

    #endregion

    [XmlIgnore]
    public long _address;

    [XmlIgnore]
    public IFDEntry ParentEntry;

    [XmlIgnore]
    public JpegFile Owner;

    /// <summary>The count of IFD Entries read from file</summary>    
    [XmlIgnore]
    public ushort _EntryCount;

    #region ### XML SERIALIZATION ###

    [XmlAttribute(AttributeName = "a")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_address
    {
      get { return string.Format("{0:x8}", (ulong)_address); }
      set { _address = value.hex_to_i8(); }
    }

    [XmlAttribute(AttributeName = "c")]
    [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
    public string xml_entrycount
    {
      get { return string.Format("{0:x4}", (ushort)_EntryCount); }
      set { _EntryCount = value.hex_to_u2(); }
    }

    [XmlElement("item")]
    [ComVisible(false)] // COM will use Item and Item_2 members
    public readonly List<IFDEntry> entries = new List<IFDEntry>();

    [XmlIgnore]
    public IFDEntry this[int index]
    {
      get { return entries[index]; }
      set { entries[index] = value; }
    }

    [XmlIgnore]
    public int Count
    {
      get
      {
        return this.entries.Count;
      }
    }

    public void Add(IFDEntry entry)
    {
      this.entries.Add(entry);
    }

    public void Insert(int index, IFDEntry entry)
    {
      this.entries.Insert(index, entry);
    }

    #endregion

    internal IFD()
    { }

    public IFD(JpegFile Owner = null, IFDEntry ParentEntry = null)
    {
      this.Owner = Owner;
      this.ParentEntry = ParentEntry;
    }

    public void Read(JpegBinaryReader reader, long block_address, long block_size)
    {
      // remember address
      _address = reader.Position;

      _EntryCount = reader.read_u2_re();                   // read 2 bytes
      for (int i = 0; i < _EntryCount; i++)
      {
        if (reader.Position > block_address + block_size - 12)
        {
          FireOnWarning(new IFDEventArgs()
            {
              Address = block_address,
              Id = JpegFileEventId.WARNING_EXIFIFD_END_OF_BLOCK,
              IFD = this,
              Entry = null,
              Message = string.Format("IFD.Read() reached the end of block_size (address: 0x{0:x8}).", reader.Position)
            });
          break;
        }
        IFDEntry entry = new IFDEntry(this);
        this.Add(entry);
        entry.Read(reader, block_address, block_size);    // read N bytes
      }


    }

    public void Write(JpegBinaryWriter writer, long addr_blck, ref long addr_offs, long NextIFDOffsetSize = 4)
    {
      // we must offset value to end of all entry blocks
      // current_address+2(count)+(count*12)+4(NextIFDPointer)
      addr_offs = writer.Position + 2 + (this.Count * 12) + NextIFDOffsetSize;

      // NOTICE: it sais nowhere in standard I should WORD ALIGN data
      addr_offs += ((2 - (addr_offs & 0x00000001)) % 2); // WORD ALIGN

      // NOTICE: standard EXIF stipulation 
      // "Note that field Interoperability shall be recorded in sequence starting from 
      //  the smallest tag number. There is no stipulation regarding the order or position 
      //  of tag value (Value) recording."
      //

      // Follow the standard (sort by tags)
      this.entries.Sort(IFDEntrySortByTag.Comparer);

      writer.write_u2_we((ushort)this.Count);            // writes 2 bytes - entry count
      for (int i = 0; i < this.Count; i++)
      {
        IFDEntry entry = this[i];
        entry._address = writer.Position;                // need address later
        entry.Write(writer, addr_blck, ref addr_offs);
        // NOTICE: it sais nowhere in standard I should WORD ALIGN data
        addr_offs += ((2 - (addr_offs & 0x00000001)) % 2); // WORD ALIGN
      }
      
    }

    #region ### Query Interface ###

    public const uint QUERY_TAG_MASK      = 0x0000ffff;

    public const uint QUERY_CTRL_MASK     = 0xffff0000;

    public const uint QUERY_CTRL_ANY      = 0x00010000;    
    public const uint QUERY_CTRL_FIRST    = 0x00020000;
    public const uint QUERY_CTRL_NOT      = 0x00040000;
    public const uint QUERY_CTRL_ANYLEVEL = 0x00080000;

    public IFDEntries Query(string QueryPath)
    {
      JpegFileSegMarker markerID;
      string[] QueryPathPairs = QueryPath.Split(new char[] { '\\', '/' });      
      uint[] QueryPathTags = JpegFile.QueryParser(QueryPathPairs, out markerID);
      if (QueryPathTags.Length == 0)
      {
        return new IFDEntries();
      }
      bool quit = false;
      return Query(QueryPathTags, ref quit);
    }

    public IFDEntries Query(uint[] QueryPathTags, ref bool quit)
    {
      IFDEntries entries = new IFDEntries();
      if (QueryPathTags.Length == 0) return entries;
      IFDTag tag = (IFDTag)(QueryPathTags[0] & QUERY_TAG_MASK);
      uint ctrl = (QueryPathTags[0] & QUERY_CTRL_MASK);
      for (int i = 0; i < this.Count; i++)
      {
        if ((ctrl.IsSet(QUERY_CTRL_ANY)) || 
            (ctrl.IsNot(QUERY_CTRL_NOT) && tag == this[i].tag) || 
            (ctrl.IsSet(QUERY_CTRL_NOT) && tag != this[i].tag)) 
        {
          // if query tag is any, or if query tag is specified and equal to entry tag
          if (QueryPathTags.Length == 1)
          {
            // Query path end, add entry
            entries.Add(this[i]);
          }
          else
          {
            // Query path continues, query sub entries
            if (this[i].IFD != null)
            {
              uint[] SubQueryPathTags = new uint[QueryPathTags.Length - 1];
              Array.Copy(QueryPathTags, 1, SubQueryPathTags, 0, SubQueryPathTags.Length);
              entries.AddRange(this[i].IFD.Query(SubQueryPathTags, ref quit));              
            }
          }
        }
        if (!quit && ctrl.IsSet(QUERY_CTRL_FIRST))
        {
          quit = true;
          break;
        }
        if (!quit && ctrl.IsSet(QUERY_CTRL_ANYLEVEL))
        {
          if (this[i].IFD != null)
          {
            // try with current path
            entries.AddRange(this[i].IFD.Query(QueryPathTags, ref quit));
            if (!quit)
            {
              // try subpath 
              uint[] SubQueryPathTags = new uint[QueryPathTags.Length - 1];
              Array.Copy(QueryPathTags, 1, SubQueryPathTags, 0, SubQueryPathTags.Length);
              entries.AddRange(this[i].IFD.Query(SubQueryPathTags, ref quit).entries);
            }            
          }
        }
        if (quit) break;
      }
      return entries;
    }

    #endregion

    public virtual string Dump()
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine(this.ToString());
      for (int i = 0; i < this.Count; i++)
      {
        sb.AppendFormat("  [{0,2}] {1}:\r\n  {{\r\n    {2}\r\n  }}\r\n", i, this[i].ToString(), this[i].getValueString(-1));
      }
      return sb.ToString();
    }
  }

}
