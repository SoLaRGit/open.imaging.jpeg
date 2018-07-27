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

// usage : coding contraints

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

/// <summary>
/// Allows declaring constant arrays as static readonly where elements can't be overwritten.
/// </summary>
/// <remarks>
/// <example>
/// public static readonly ReadOnlyArray&lt;byte> CONST_NAME = new byte[] { 0x00, 0x01, 0x02, 0x03 };
/// </example>
/// </remarks>
/// <typeparam name="VAT"></typeparam>
public struct ReadOnlyArray<VAT>
  where VAT : struct
{
  /// <summary>
  /// protected readonly data.
  /// </summary>
  readonly VAT[] data;

  /// <summary>
  /// Base contructor T[].
  /// </summary>
  public ReadOnlyArray(VAT[] array)
  {
    // keep internal copy of array instead of reference
    this.data = new VAT[array.Length];
    Array.Copy(array, this.data, array.Length);    
  }

  /// <summary>
  /// elements
  /// </summary>
  public VAT this[int index]
  {
    get
    {
      return data[index];
    }
  }

  public static bool operator ==(ReadOnlyArray<VAT> A, VAT[] B)
  {
    // (object)A - prevents circular operator dependency 
    //             also no OutOfStackException ;)
    if (null == (object)A && null == B) return true;
    if (null == (object)A || null == A.data || null == B) return false;
    if (A.data.Length != B.Length) return false;
    for (int i = 0; i < A.data.Length; ++i)
    {
      // can't compare T == T : nonsense of C# generics
      if (!A.data[i].Equals(B[i])) return false;
    }
    return true;
  }

  public static bool operator ==(ReadOnlyArray<VAT> A, ReadOnlyArray<VAT> B)
  {
    // (object)A - prevents circular operator dependency 
    //             also no OutOfStackException ;)
    if (null == (object)A && null == (object)B) return true;
    if (null == (object)A || null == (object)A || null == A.data || null == B.data) return false;
    if (A.data.Length != B.data.Length) return false;
    for (int i = 0; i < A.data.Length; ++i)
    {
      // can't compare T == T : nonsense of C# generics
      if (!A.data[i].Equals(B.data[i])) return false;
    }
    return true;
  }

  public static bool operator !=(ReadOnlyArray<VAT> A, VAT[] B)
  {
    // must NEGATE like this, call to first operator
    return !(A == B); 
  }

  public static bool operator !=(ReadOnlyArray<VAT> A, ReadOnlyArray<VAT> B)
  {
    // must NEGATE like this, call to first operator
    return !(A == B);
  }

  public static bool operator ==(VAT[] B, ReadOnlyArray<VAT> A)  
  {
    // must test like this, call to first operator
    return (A == B);
  }

  public static bool operator !=(VAT[] B, ReadOnlyArray<VAT> A)
  {
    // must NEGATE like this, call to first operator
    return !(A == B); 
  }

  public override bool Equals(object obj)
  {
    if (obj.GetType() == typeof(VAT[]))
    {
      // must test like this, call to first operator
      return (this == (obj as VAT[])); 
    }
    return base.Equals(obj);
  }

  public override int GetHashCode()
  {
    return base.GetHashCode();
  }

  public static implicit operator VAT[](ReadOnlyArray<VAT> rvalue)
  {
    VAT[] clone = new VAT[rvalue.data.Length];
    Array.Copy(rvalue.data, clone, clone.Length);
    return clone;
  }

  public static implicit operator ReadOnlyArray<VAT>(VAT[] rvalue)
  {
    // contructor copies value already
    return new ReadOnlyArray<VAT>(rvalue);
  }

  //public Type ElementType
  //{
  //  get { return typeof(VAT); }
  //}

  //public int ElementSize
  //{
  //  get { return Marshal.SizeOf(typeof(VAT)); }
  //}

  //public Type ElementsSize
  //{
  //  get { return Marshal.SizeOf(typeof(VAT)) * data.Length; }
  //}

  public override string ToString()
  {
    StringBuilder sb = new StringBuilder();
    int length = data.Length < 100 ? data.Length : 100;
    int tsize = Marshal.SizeOf(typeof(VAT));
    string format = "{0:X" + (tsize * 2) + "} "; // HEX numbers 2 letters per byte
    for (int i = 0; i < data.Length; ++i)
    {
      sb.Append(string.Format(format, data[i]));
    }
    return sb.ToString();
  }

}