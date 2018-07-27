using System;
using System.Runtime.InteropServices;

using INT8 = System.SByte;
using INT16 = System.Int16;
using INT32 = System.Int32;
using INT64 = System.Int64;

using UINT8 = System.Byte;
using UINT16 = System.UInt16;
using UINT32 = System.UInt32;
using UINT64 = System.UInt64;


[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class ComTest
{
  public INT8 i1;  
  public INT16 i2;  
  public INT32 i4;  
  public INT64 i8;

  public UINT8 u1;
  public UINT16 u2;
  public UINT32 u4;
  public UINT64 u8;
}


