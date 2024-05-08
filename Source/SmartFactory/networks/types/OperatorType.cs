// MIT License
//
// Copyright (c) 2024. SuperComic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

namespace SmartFactory
{
    public enum OperatorType
    {
        /// <summary>
        /// x + y
        /// </summary>
        Add,

        /// <summary>
        /// x - y
        /// </summary>
        Sub,
        
        /// <summary>
        /// x * y
        /// </summary>
        Mul,

        /// <summary>
        /// x / y
        /// </summary>
        Div,

        /// <summary>
        /// x % y
        /// </summary>
        Mod,

        /// <summary>
        /// x &amp; y
        /// </summary>
        And,

        /// <summary>
        /// x | y
        /// </summary>
        Or,
        
        /// <summary>
        /// x ^ y
        /// </summary>
        Xor,

        CmpEQ,
        CmpNE,
        CmpLE,
        CmpGE,
        CmpLT,
        CmpGT,

        /// <summary>
        /// !x (~x)
        /// </summary>
        Not,

        /// <summary>
        /// -x
        /// </summary>
        Neg,

        /// <summary>
        /// x &lt; 0 ? -x : x
        /// </summary>
        Abs,
    }
}
