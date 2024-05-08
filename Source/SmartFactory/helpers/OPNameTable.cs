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
    /// <summary>
    /// <see cref="OperatorType"/> name table
    /// </summary>
    internal static class OPNameTable
    {
        public static readonly string[] Names =
        {
            // def
            "2 ADD ( + )",
            "2 SUB ( - )",
            "2 MUL ( * )",
            "2 DIV ( / )",
            "2 MOD ( % )",

            // bit
            "2 AND ( & )",
            "2 OR ( | )",
            "2 XOR ( ^ )",

            // compare
            "2 EQ ( == )",
            "2 NE ( != )",
            "2 LE ( <= )",
            "2 GE ( >= )",
            "2 LT ( < )",
            "2 GT ( > )",

            // unary
            "1 NOT",
            "1 NEG",
            "1 ABS"
        };
    }
}
