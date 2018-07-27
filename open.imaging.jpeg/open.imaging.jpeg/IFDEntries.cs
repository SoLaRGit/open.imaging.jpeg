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
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace open.imaging.jpeg
{
  
  [ComVisible(true)]
  [ClassInterface(ClassInterfaceType.AutoDual)]
  public class IFDEntries : IEnumerable<IFDEntry>
  {

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

    /// <summary>
    /// o_entries as IEnumerable&lt;IFDEntry&gt;.
    /// </summary>
    /// <param name="o_entries"></param>
    public void AddRange(object o_entries)
    {
      IEnumerable<IFDEntry> entries = o_entries as IEnumerable<IFDEntry>;
      if (null != entries) this.entries.AddRange(entries);
    }

    public void Insert(int index, IFDEntry entry)
    {
      this.entries.Insert(index, entry);
    }

    public override string ToString()
    {
      return string.Format("IFDEntries{count:{0}}", Count);
    }

    public IEnumerator<IFDEntry> GetEnumerator()
    {
      return (IEnumerator<IFDEntry>)this.entries.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return (System.Collections.IEnumerator)this.entries;
    }
  }

}
