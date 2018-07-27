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

#if DEBUG
//#define TEST_COMPILER_ERRORS

using System;
using System.Collections.Generic;

/// <summary>
/// usage : coding contraints (helps detect errors when coders accidently set value to readonly variable).
/// Will generate compiler errors in code if we try to set base type VT value to ReadOnly VT variables.
/// Also this class can't be used in Release configurations, creating another compile error if coder tries to do it.
/// </summary>
/// <typeparam name="VT"></typeparam>
public struct ReadOnly_T<VT>
  where VT : struct
{
  private VT value;

  public ReadOnly_T(VT value)
  {
    this.value = value;
  }

  public static implicit operator VT(ReadOnly_T<VT> rvalue)
  {
    return rvalue.value;
  }

  /// <summary>forces explicit conversion only</summary>
  public static explicit operator ReadOnly_T<VT>(VT rvalue)
  {
    return new ReadOnly_T<VT>(rvalue);
  }

  public override string ToString()
  {
    return string.Format(" 0x{0:x16} ({1}) ", value, value);
  }
}

public static class ReadOnly_T_Test
{
  static void TestCall()
  {
    long a = 0;
    
    // CALL USAGE 1.
    // explicite cast must exist in call to this function
    // and clearly states it will be readonly inside TestCalled function.
#if TEST_COMPILER_ERRORS
    TestCalled(a);                  // invalid call, we must have explicit cast to ReadOnly<T>
#endif
    TestCalled((ReadOnly_T<long>)a);  // explicit cast to ReadOnly<T>

    // CALL USAGE 2.
    // Debug vs Release call has no difference - no compiler errors
    TestCalled2(a);

  }

  // ARG USAGE 1.
  public static void TestCalled(ReadOnly_T<long> a)
  {
    // invalid operations, compiler errors
#if TEST_COMPILER_ERRORS
    a = 10L;
    a += 2L;
    a -= 2L;
    a *= 2L;
    a /= 2L;
    a++;
    a--;
#endif
    // valid operations
    long l;
    l = a + 2;
    l = a - 2;
    l = a * 2;
    l = a / 2;
    l = a ^ 2;
    l = a | 2;
    l = a & 2;
    l = a << 2;
    l = a >> 2;
    l = ~a;
  }


  // ARG USAGE 2.
#if DEBUG
  static void TestCalled2(long a2_writable)
  {
    ReadOnly_T<long> a = new ReadOnly_T<long>(a2_writable);
    ReadOnly_T<long> other = new ReadOnly_T<long>(a2_writable);
#else
  public static void TestCalled2(long a)
  {
    long other = 9;
#endif
    // invalid operations
    // compiler will have errors in debug configuration
    // compiler will compile in release
#if TEST_COMPILER_ERRORS
    a = 10L;
    a += 2L;
    a -= 2L;
    a *= 2L;
    a /= 2L;
    a++;
    a--;
#endif
    a = other;
    // valid operations
    // compiler will compile in both, debug and release configurations
    long l;
    l = a + 2;
    l = a - 2;
    l = a * 2;
    l = a / 2;
    l = a ^ 2;
    l = a | 2;
    l = a & 2;
    l = a << 2;
    l = a >> 2;
    l = ~a;
  }
}

#endif