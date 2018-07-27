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
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

[Flags]
public enum GenericSerializerType
{
  XML = 1,
  XMLGZIP = 2,
  BINARY = 4,
  BASE64 = 8,
}

public class GenericSerializer<T>
  where T : class
{
  /// <summary>
  /// JIT will first time you use this class generate this class with its static member of type T.
  /// Every other call to static member will reuse the same instance of static member.
  /// <remarks>I know crazy code ;)</remarks>
  /// </summary>
  public static GenericSerializer<T> Serializer = new GenericSerializer<T>();

  public bool Write(string filename, T obj, GenericSerializerType SerializationType)
  {
    switch (SerializationType)
    {
      case GenericSerializerType.XMLGZIP:
        return WriteGZip(filename, obj);
      case GenericSerializerType.BINARY:
        return WriteBin(filename, obj);
      default:
        return WriteXml(filename, obj);
    }
  }

  public bool Write(Stream output, T obj, GenericSerializerType SerializationType)
  {
    switch (SerializationType)
    {
      case GenericSerializerType.XMLGZIP:
        return WriteGZip(output, obj);
      case GenericSerializerType.BINARY:
        return WriteBin(output, obj);
      default:
        return WriteXml(output, obj);
    }
  }

  //
  // TODO: include member derived types
  //

  public Type[] GetDerivedTypes()
  {
    // use reflection to get all derived types
    List<Type> derived = new List<Type>();
    Assembly asm;
    Type[] asmtypes;

    Type TT = typeof(T);
    // application or service
    asm = Assembly.GetEntryAssembly();
    asmtypes = asm.GetTypes();
    foreach (Type t in asmtypes)
      if (TT.IsAssignableFrom(t) && derived.IndexOf(t) == -1) derived.Add(t);

    // calling (application or service, or another library)
    asm = Assembly.GetCallingAssembly();
    asmtypes = asm.GetTypes();
    foreach (Type t in asmtypes)
      if (TT.IsAssignableFrom(t) && derived.IndexOf(t) == -1) derived.Add(t);

    // this library
    asm = Assembly.GetExecutingAssembly();
    asmtypes = asm.GetTypes();
    foreach (Type t in asmtypes)
      if (TT.IsAssignableFrom(t) && derived.IndexOf(t) == -1) derived.Add(t);

    MemberInfo[] members = TT.GetMembers();
    for (int i = 0; i < members.Length; i++)
    {
      MemberInfo member = members[i];
      //
      // TODO: actually I've made proper XML SOAP so ... no need for this
      //       maybe for other projects it will be necessary
      //
    }

    //
    // need to include type of segments (same as above)
    //

    // for this library to include derived types
    asm = Assembly.GetExecutingAssembly();
    asmtypes = asm.GetTypes();
    Type TToijSeg = typeof(open.imaging.jpeg.JpegFileSeg);
    foreach (Type t in asmtypes)
      if (TToijSeg.IsAssignableFrom(t) && derived.IndexOf(t) == -1) derived.Add(t);

    Type TToijIcc = typeof(open.imaging.icc.types.ICCTagData);
    foreach (Type t in asmtypes)
      if (TToijIcc.IsAssignableFrom(t) && derived.IndexOf(t) == -1) derived.Add(t);

    return derived.ToArray();
  }

  public T Read(string filename, GenericSerializerType SerializationType)
  {
    switch (SerializationType)
    {
      case GenericSerializerType.XMLGZIP:
        return ReadGZip(filename);
      case GenericSerializerType.BINARY:
        return ReadBin(filename);
      default:
        return ReadXml(filename);
    }
  }

  public T Read(Stream input, GenericSerializerType SerializationType)
  {
    switch (SerializationType)
    {
      case GenericSerializerType.XMLGZIP:
        return ReadGZip(input);
      case GenericSerializerType.BINARY:
        return ReadBin(input);
      default:
        return ReadXml(input);
    }
  }

  public bool WriteXml(string filename, T obj, XmlAttributeOverrides Overrides = null)
  {
    HighPerformanceCounter hpc = new HighPerformanceCounter();
    if (obj == null)
    {
      Debug.Print(string.Format("{0}::WriteXml() nothing to write into '{1}' : {2}.", typeof(T).Name, filename, hpc.Duration));
      return true;
    }
    try
    {
      if (File.Exists(filename))
      {
        File.Delete(filename);
      }
      using (FileStream fs = new FileStream(filename, FileMode.CreateNew))
      {
        using (MemoryStream output = new MemoryStream())
        {
          bool result = WriteXml(output, obj, Overrides);
          if (!result) throw (new IOException(string.Format("{0}::WriteXml('{1}') error writing.", typeof(T).Name, filename)));
          // rewind
          output.Position = 0;
          output.WriteTo(fs);
          fs.Close();
        }
      }
      return true;
    }
    catch (Exception ex)
    {
      Debug.Print(ex.ToString());
      Debug.Assert(false);
      return false;
    }
    finally
    {
      Debug.Print(string.Format("{0}::WriteXml() wrote '{1}' : {2}.", typeof(T).Name, filename, hpc.Duration));
    }
  }

  public bool WriteXml(Stream output, T obj, XmlAttributeOverrides Overrides = null)
  {
    HighPerformanceCounter hpc = new HighPerformanceCounter();
    if (obj == null)
    {
      Debug.Print(string.Format("{0}::WriteXml() nothing to write into 'Stream output' : {1}.", typeof(T).Name, hpc.Duration));
      return true;
    }
    try
    {     
      Type TType = typeof(T);
      XmlSerializer xs;
      if (null != Overrides)
      {
        xs = new XmlSerializer(TType, Overrides);
      }
      else if (TType.Module.Name == Assembly.GetExecutingAssembly().GetTypes()[0].Module.Name)
      {
        Type[] derived = GetDerivedTypes();
        xs = new XmlSerializer(TType, derived);
      }
      else
      {
        xs = new XmlSerializer(TType);
      }

      XmlSerializerNamespaces xn = new XmlSerializerNamespaces();
      xn.Add("", ""); // I want plain XML
      xs.Serialize(output, obj, xn);
      return true;
    }
    catch (Exception ex)
    {
      Debug.Print(ex.ToString());
      Debug.Assert(false);
      return false;      
    }
    finally
    {
      Debug.Print(string.Format("{0}::WriteXml() wrote 'Stream output' : {1}.", typeof(T).Name, hpc.Duration));
    }
  }

  //public bool WriteGZip(string filename, T obj, XmlAttributeOverrides Overrides = null)
  //{
  //  HighPerformanceCounter hpc = new HighPerformanceCounter();
  //  string gzfilename = filename + ".gz";
  //  if (obj == null)
  //  {
  //    Debug.Print("{0}::WriteGZip nothing to write into '{1}' : {2}.", typeof(T).Name, gzfilename, hpc.Duration));
  //    return true;
  //  }
  //  try
  //  {
  //    if (File.Exists(gzfilename))
  //    {
  //      File.Delete(gzfilename);
  //    }
  //    Directory.CreateDirectory(Path.GetDirectoryName(gzfilename));
  //    using (MemoryStream memory = new MemoryStream())
  //    {
  //      XmlSerializer xs = new XmlSerializer(typeof(T));
  //      XmlSerializerNamespaces xn = new XmlSerializerNamespaces();
  //      xn.Add("", ""); // I want plain XML
  //      xs.Serialize(memory, obj, xn);
  //      using (FileStream gzfile = File.Create(gzfilename))
  //      {
  //        using (GZipStream output = new GZipStream(gzfile, CompressionMode.Compress))
  //        {
  //          // output.CopyTo(memory);   // System.NotSupportedException - stupid
  //          byte[] buffer = memory.GetBuffer(); // entire memory stream buffer
  //          output.Write(buffer, 0, (int)memory.Position); // limit to actually written bytes
  //          gzfile.Flush();
  //          // gzfile.Close();
  //        }
  //      }
  //      memory.Flush();
  //      memory.Close();
  //    }
  //    GC.Collect(0);
  //    GC.Collect(2);
  //    return true;
  //  }
  //  catch (Exception ex)
  //  {
  //    Debug.Print(ex.ToString());
  //    Debug.Assert(false);
  //    return false;
  //  }
  //  finally
  //  {
  //    Debug.Print(string.Format("{0}::WriteGZip wrote '{1}' : {2}.", typeof(T).Name, gzfilename, hpc.Duration));
  //  }
  //}

  public bool WriteGZip(string filename, T obj, XmlAttributeOverrides Overrides = null)
  {
    HighPerformanceCounter hpc = new HighPerformanceCounter();
    string gzfilename = filename + ".gz";
    if (obj == null)
    {
      Debug.Print(string.Format("{0}::WriteGZip nothing to write into '{1}' : {2}.", typeof(T).Name, gzfilename, hpc.Duration));
      return true;
    }
    try
    {
      if (File.Exists(gzfilename))
      {
        File.Delete(gzfilename);
      }
      Directory.CreateDirectory(Path.GetDirectoryName(gzfilename));
      using (FileStream gzfile = File.Create(gzfilename))
      {
        bool result = WriteGZip(gzfile, obj, Overrides);
        if (!result) throw (new IOException(string.Format("{0}::WriteGZip('{1}') error writting.", typeof(T).Name, gzfilename)));
      }

      GC.Collect(0);
      GC.Collect(2);
      return true;
    }
    catch (Exception ex)
    {
      Debug.Print(ex.ToString());
      Debug.Assert(false);
      return false;
    }
    finally
    {
      Debug.Print(string.Format("{0}::WriteGZip wrote '{1}' : {2}.", typeof(T).Name, gzfilename, hpc.Duration));
    }
  }

  public bool WriteGZip(Stream output, T obj, XmlAttributeOverrides Overrides = null)
  {
    HighPerformanceCounter hpc = new HighPerformanceCounter();
    if (obj == null)
    {
      Debug.Print(string.Format("{0}::WriteGZip nothing to write into 'Stream output' : {1}.", typeof(T).Name, hpc.Duration));
      return true;
    }
    try
    {
      using (MemoryStream memory = new MemoryStream())
      {
        XmlSerializer xs;
        if (null != Overrides)
        {
          xs = new XmlSerializer(typeof(T), Overrides);
        }
        else
        {
          Type[] derived = GetDerivedTypes();
          xs = new XmlSerializer(typeof(T), derived);
        }        
        XmlSerializerNamespaces xn = new XmlSerializerNamespaces();
        xn.Add("", ""); // I want plain XML
        xs.Serialize(memory, obj, xn);
        using (GZipStream gzipoutput = new GZipStream(output, CompressionMode.Compress))
        {
          byte[] buffer = memory.GetBuffer(); // entire memory stream buffer
          gzipoutput.Write(buffer, 0, (int)memory.Position); // limit to actually written bytes          
        }
        memory.Flush();
        memory.Close();
      }
      GC.Collect(0);
      GC.Collect(2);
      return true;
    }
    catch (Exception ex)
    {
      Debug.Print(ex.ToString());
      Debug.Assert(false);
      return false;
    }
    finally
    {
      Debug.Print(string.Format("{0}::WriteGZip wrote 'Stream output' : {1}.", typeof(T).Name, hpc.Duration));
    }
  }

  public T ReadXml(string filename, XmlAttributeOverrides Overrides = null)
  {
    HighPerformanceCounter hpc = new HighPerformanceCounter();
    try
    {
      T obj = default(T);

      if (!File.Exists(filename))
        return obj;

      using (FileStream fs = new FileStream(filename, FileMode.Open))
      {
        obj = ReadXml(fs, Overrides);
        fs.Flush();
        fs.Close();
      }
      return obj;
    }
    catch (Exception ex)
    {
      Debug.Print(ex.ToString());
      Debug.Assert(false);
      return default(T);
    }
    finally
    {
      Debug.Print(string.Format("{0}::ReadXml read '{1}' : {2}.", typeof(T).Name, filename, hpc.Duration));
    }
  }

  public T ReadXml(Stream input, XmlAttributeOverrides Overrides = null)
  {
    HighPerformanceCounter hpc = new HighPerformanceCounter();
    try
    {
      T obj = default(T);
      
      XmlSerializer xs;
      if (null != Overrides)
      {
        xs = new XmlSerializer(typeof(T), Overrides);
      }
      else
      {
        Type[] derived = GetDerivedTypes();
        xs = new XmlSerializer(typeof(T), derived);
      }
      obj = (T)xs.Deserialize(input);
      return obj;
    }
    catch (Exception ex)
    {
      Debug.Print(ex.ToString());
      Debug.Assert(false);
      return default(T);
    }
    finally
    {
      Debug.Print(string.Format("{0}::ReadXml read 'Stream input' : {1}.", typeof(T).Name, hpc.Duration));
    }
  }

  //public T ReadGZip(string filename)
  //{
  //  HighPerformanceCounter hpc = new HighPerformanceCounter();
  //  string gzfilename = filename + ".gz";
  //  try
  //  {
  //    T obj = default(T);

  //    if (!File.Exists(gzfilename))
  //      return obj;

  //    using (MemoryStream memory = new MemoryStream())
  //    {
  //      // decompress
  //      using (FileStream fs = new FileStream(gzfilename, FileMode.Open))
  //      {
  //        using (GZipStream gzfile = new GZipStream(fs, CompressionMode.Decompress, false))
  //        {
  //          gzfile.CopyTo(memory);
  //          //gzfile.Flush();
  //          gzfile.Close();
  //        }
  //        fs.Close();
  //      }
  //      GC.Collect(0);
  //      GC.Collect(2);
  //      memory.Position = 0;
  //      XmlSerializer xs = new XmlSerializer(typeof(T));
  //      obj = (T)xs.Deserialize(memory);
  //      memory.Close();
  //    }
  //    return obj;
  //  }
  //  catch (Exception ex)
  //  {
  //    Debug.Print(ex.ToString());
  //    Debug.Assert(false);
  //    return default(T);
  //  }
  //  finally
  //  {
  //    Debug.Print(string.Format("{0}::ReadGZip read '{1}' : {2}.", typeof(T).Name, gzfilename, hpc.Duration));
  //  }
  //}

  public T ReadGZip(string filename, XmlAttributeOverrides Overrides = null)
  {
    HighPerformanceCounter hpc = new HighPerformanceCounter();
    string gzfilename = filename + ".gz";
    try
    {
      T obj = default(T);

      if (!File.Exists(gzfilename))
        return obj;
      
      using (FileStream fs = new FileStream(gzfilename, FileMode.Open))
      {
        // decompress
        obj = ReadGZip(fs, Overrides);
        fs.Close();
      }
      return obj;
    }
    catch (Exception ex)
    {
      Debug.Print(ex.ToString());
      Debug.Assert(false);
      return default(T);
    }
    finally
    {
      Debug.Print(string.Format("{0}::ReadGZip read '{1}' : {2}.", typeof(T).Name, gzfilename, hpc.Duration));
    }
  }

  public T ReadGZip(Stream input, XmlAttributeOverrides Overrides = null)
  {
    HighPerformanceCounter hpc = new HighPerformanceCounter();
    
    try
    {
      T obj = default(T);

      using (MemoryStream memory = new MemoryStream())
      {
        // decompress
        using (GZipStream gzfile = new GZipStream(input, CompressionMode.Decompress, false))
        {
#if NET40
          gzfile.CopyTo(memory);
#else
          byte[] buffer = new byte[1024 * 1024];
          int numRead = gzfile.Read(buffer, 0, buffer.Length);
          while (numRead != 0)
          {
            memory.Write(buffer, 0, numRead);
            memory.Flush();
            numRead = gzfile.Read(buffer, 0, buffer.Length);
          }
#endif
          //gzfile.Flush();
          gzfile.Close();
        }
        GC.Collect(0);
        GC.Collect(2);
        memory.Position = 0;

        XmlSerializer xs;
        if (null != Overrides)
        {
          xs = new XmlSerializer(typeof(T), Overrides);
        }
        else
        {
          Type[] derived = GetDerivedTypes();
          xs = new XmlSerializer(typeof(T), derived);
        }        
        obj = (T)xs.Deserialize(memory);
        memory.Close();
      }
      return obj;
    }
    catch (Exception ex)
    {
      Debug.Print(ex.ToString());
      Debug.Assert(false);
      return default(T);
    }
    finally
    {
      Debug.Print(string.Format("{0}::ReadGZip read 'Stream input' : {1}.", typeof(T).Name, hpc.Duration));
    }
  }

  public bool WriteBin(string filename, T obj)
  {
    try
    {
      if (File.Exists(filename))
      {
        File.Delete(filename);
      }
      using (FileStream output = new FileStream(filename, FileMode.CreateNew))
      {
        bool result = WriteBin(output, obj);
        output.Close();
        if (!result)
        {
          throw (new IOException(string.Format("WriteBin('{0}', {1}) Unable to write", filename, typeof(T).Name)));
        }
      }
      return true;
    }
    catch (Exception ex)
    {
      Debug.Print(ex.ToString());
      Debug.Assert(false);
      return false;
    }
  }

  public bool WriteBin(Stream output, T obj)
  {
    try
    {  
      BinaryFormatter formatter = new BinaryFormatter();
      formatter.Serialize(output, obj);
      output.Flush();
      return true;
    }
    catch (Exception ex)
    {
      Debug.Print(ex.ToString());
      Debug.Assert(false);
      return false;
    }
  }

  public T ReadBin(string filename)
  {
    try
    {
      T obj = default(T);

      if (!File.Exists(filename))
        return obj;

      using (FileStream input = new FileStream(filename, FileMode.Open))
      {
        obj = ReadBin(input);
        input.Close();
      }
      return obj;
    }
    catch (Exception ex)
    {
      Debug.Print(ex.ToString());
      Debug.Assert(false);
      return default(T);
    }
  }

  public T ReadBin(Stream input)
  {
    try
    {
      T obj = default(T);
      BinaryFormatter formatter = new BinaryFormatter();
      obj = (T)formatter.Deserialize(input);      
      return obj;
    }
    catch (Exception ex)
    {
      Debug.Print(ex.ToString());
      Debug.Assert(false);
      return default(T);
    }
  }

}

