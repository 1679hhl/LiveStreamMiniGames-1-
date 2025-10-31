//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Knight.Core
{
    /// <summary>
    /// 词法分析数据：转换矩阵和数据结构
    /// </summary>
    public class LexicalAnalysis
    {
        /// <summary>
        /// 注释的词法状态转换矩阵
        /// a -> '/'    a -> '*'    a -> 'other'    a -> '\n'
        ///    0           1             2              3
        /// 5表示终态是注释，6表示出错了不是注释
        /// </summary>
        public static int[][] CommentsStateMatrix = new int[][]
        {
            new int[] { 1, 6, 6, 6 },   //0
            new int[] { 4, 2, 6, 6 },   //1
            new int[] { 2, 3, 2, 2 },   //2
            new int[] { 5, 2, 2, 2 },   //3
            new int[] { 4, 4, 4, 5 },   //4
        };

        /// <summary>
        /// 特殊字符的词法状态转化矩阵
        /// a -> '\b' '\f' '\r' '\t' '\n' ' '   a -> other
        /// 2表示正常结束态，3表示出错结束态
        /// </summary>
        public static int[][] SpecialSymbolMatrix = new int[][]
        {
            new int[] { 1, 3 }, //0
            new int[] { 1, 2 }  //1
        };

        /// <summary>
        /// 字符串的词法状态转换矩阵
        /// a -> '"'    a -> '\\'       a -> pre'\\'error   a -> other
        /// 0           1               2 错误的转义码      3
        /// 1字符串开始" 2字符串正文 3字符串转移开始\ 4字符串结束" 5正常结束态，6表示错误结束态
        /// </summary>
        public static int[][] StringMatrix = new int[][]
        {
            new int[] { 1, 6, 6, 6 }, //0
            new int[] { 4, 3, 2, 2 }, //1
            new int[] { 4, 3, 2, 2 }, //2
            new int[] { 2, 2, 7, 2 }, //3
            new int[] { 5, 5, 5, 5 }, //4
        };

        /// <summary>
        /// 标示符的词法状态转换矩阵
        /// a -> letter   a -> digit  a -> _  a -> other
        ///     0             1          2       3
        /// 2表示正常结束态，3表示错误结束态
        /// </summary>
        public static int[][] IdentiferMatrix = new int[][]
        {
            new int[] { 1, 3, 1, 3 },
            new int[] { 1, 1, 1, 2 }
        };

        /// <summary>
        /// json中的关键字 false true null
        /// </summary>
        public static HashSet<string> Keywords = new HashSet<string>()
        {
            "false",
            "true",
            "null",
            "False",
            "True",
            "Null",
        };

        /// <summary>
        /// 数字的词法状态转换矩阵
        /// a -> digit(0)   a -> digit(1-9)  a -> E/e   a -> .   a -> +     a -> -    a -> other
        ///    0                    1           2         3         4          5         6
        /// 9表示正常结束态、10表示错误结束态
        /// </summary>
        public static int[][] DigitMatrix = new int[][]
        {
            new int[] { 2, 1, 10, 4,  10, 3,  10 },   //0
            new int[] { 1, 1, 6,  4,  9,  9,  9  },   //1
            new int[] { 1, 1, 6,  4,  9,  9,  9  },   //2
            new int[] { 2, 1, 10, 4,  10, 10, 10 },   //3
            new int[] { 5, 5, 6,  9,  9,  9,  9  },   //4
            new int[] { 5, 5, 6,  9,  9,  9,  9  },   //5
            new int[] { 8, 8, 10, 10, 7,  5,  10 },   //6
            new int[] { 8, 8, 10, 10, 10, 10, 10 },   //7
            new int[] { 8, 8, 9,  9,  9,  9,  9  },   //8
        };

        private static int isCommitInputChar(char c)
        {
            int j = 0;
            if (c == '/') j = 0;
            else if (c == '*') j = 1;
            else if (c == '\n') j = 3;
            else j = 2;        //other
            return j;
        }

        /// <summary>
        /// 是否是注释 /**/ 和 //
        /// </summary>
        public static string isComment(string originData, int begin, ref int end)
        {
            string result;
            checkLexical(originData, isCommitInputChar, null, LexicalAnalysis.CommentsStateMatrix, 5, 6, begin, ref end);
            if (end > begin)
                result = originData.Substring(begin, end - begin);
            else
                result = string.Empty;
            if (!string.IsNullOrEmpty(result)) result += originData[end++];
            return result;
        }

        private static int isSpecialSymbolInputChar(char c)
        {
            int j = 0;
            if (c == '\b' || c == '\f' || c == '\r' || c == '\n' || c == ' ' || c == '\0' || c == '\t')
                j = 0;
            else
                j = 1;
            return j;
        }

        /// <summary>
        /// 是否是特殊的字符 \b \f \n \r \t 空格
        /// </summary>
        public static bool isSpecialSymbol(string originData, int begin, ref int end)
        {
            checkLexical(originData, isSpecialSymbolInputChar, null, LexicalAnalysis.SpecialSymbolMatrix, 2, 3, begin, ref end);
            return end > begin;
        }

        public static void parseSpecialSymbol(string originData, int begin, ref int end)
        {
            checkLexical(originData, isSpecialSymbolInputChar, null, LexicalAnalysis.SpecialSymbolMatrix, 2, 3, begin, ref end);
        }

        private static List<char> mBackslashSymbolOne = new List<char>()
        {
            '\'', '\"', '\\', '0', 'a', 'b', 'f', 'n', 'r', 't', 'v',
        };
        private static int[] mBackslashSymbolOneIndexDict = new int[]
        {
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 0-9
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 10-19
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 20-29
            -1, -1, -1, -1,  1, -1, -1, -1, -1,  0,     // 30-39
            -1, -1, -1, -1, -1, -1, -1, -1,  3, -1,     // 40-49
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 50-59
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 60-69
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 70-79
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 80-89
            -1, -1,  2, -1, -1, -1, -1,  4,  5, -1,     // 90-99
            -1, -1,  6, -1, -1, -1, -1, -1, -1, -1,     // 100-109
             7, -1, -1, -1,  8, -1,  9, -1, 10, -1,     // 110-119
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 120-129
        };
        private static List<char> mBackslashSymbolTwo = new List<char>()
        {
            '\'', '\"', '\\', '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v',
        };
        private static int[] mBackslashSymbolTwoIndexDict = new int[]
        {
             3, -1, -1, -1, -1, -1, -1,  4,  5,  9,     // 0-9
             7, 10,  6,  8, -1, -1, -1, -1, -1, -1,     // 10-19
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 20-29
            -1, -1, -1, -1,  1, -1, -1, -1, -1,  0,     // 30-39
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 40-49
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 50-59
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 60-69
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 70-79
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 80-89
            -1, -1,  2, -1, -1, -1, -1, -1, -1, -1,     // 90-99
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 100-109
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 110-119
            -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,     // 120-129
        };
        private static int isStringInputChar(char c)
        {
            int j;
            if (c == '"')
            {
                j = 0;
            }
            else if (c == '\\')
            {
                j = 1;
            }
            else if (c < 128 && mBackslashSymbolOneIndexDict[c] == -1)
            {
                j = 2;
            }
            else
            {
                j = 3;
            }
            return j;
        }
        private static void StringPrintError(int nErrorState, string originData, int curren)
        {
            if (nErrorState == 7)
            {
                Knight.Core.LogManager.LogError($"解析中遇到了错误的转移符号 Symbol:{originData[curren]}");
            }
        }

        /// <summary>
        /// 是否为字符串 "string"
        /// </summary>
        public static bool getString(string originData, ref string str, int begin, ref int end)
        {
            checkLexicalString(originData, begin, ref end);
            if (end >= begin + 2)
            {
                str = originData.Substring(begin + 1, end - begin - 2);
                return true;
            }
            return false;
        }
        private static StringBuilder mTempSB = new StringBuilder();

        public static bool NeedBackslashSymbol(string originData)
        {
            if (originData == null) return false;
            int nIndex;
            for (int i = 0; i < originData.Length; i++)
            {
                nIndex = -1;
                if (originData[i] < 128)
                    nIndex = mBackslashSymbolTwoIndexDict[originData[i]];
                if (nIndex != -1)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 添加转移符号
        /// </summary>
        /// <param name="originData"></param>
        /// <returns></returns>
        public static string BackslashSymbol(string originData)
        {
            if (originData == null) return originData;
            mTempSB.Clear();
            var rChars = originData.ToCharArray();
            int nIndex;
            for (int i = 0; i < rChars.Length; i++)
            {
                nIndex = -1;
                if (rChars[i] < 128)
                    nIndex = mBackslashSymbolTwoIndexDict[rChars[i]];
                if (nIndex != -1)
                {
                    mTempSB.Append('\\');
                    mTempSB.Append(mBackslashSymbolOne[nIndex]);
                }
                else
                {
                    mTempSB.Append(rChars[i]);
                }
            }
            return mTempSB.ToString();
        }
        public static bool NeedUnBackslashSymbol(string originData)
        {
            if (originData == null) return false;
            int nIndex;
            for (int i = 0; i < originData.Length;)
            {
                if (originData[i] == '\\' && i < originData.Length - 1)
                {
                    nIndex = -1;
                    if (originData[i + 1] < 128)
                        nIndex = mBackslashSymbolOneIndexDict[originData[i + 1]];
                    if (nIndex != -1)
                        return true;
                }
                i++;
            }
            return false;
        }

        /// <summary>
        /// 将转移符号去掉
        /// </summary>
        /// <param name="originData"></param>
        /// <returns></returns>
        public static string UnBackslashSymbol(string originData)
        {
            if (originData == null) return originData;
            mTempSB.Clear();
            var rChars = originData.ToCharArray();
            int nIndex;
            for (int i = 0; i < rChars.Length;)
            {
                if (rChars[i] == '\\' && i < rChars.Length - 1)
                {
                    nIndex = -1;
                    if (rChars[i + 1] < 128)
                        nIndex = mBackslashSymbolOneIndexDict[rChars[i + 1]];
                    if (nIndex != -1)
                    {
                        mTempSB.Append(mBackslashSymbolTwo[nIndex]);
                        i += 2;
                    }
                    else
                    {
                        mTempSB.Append(rChars[i]);
                        i++;
                    }
                }
                else
                {
                    mTempSB.Append(rChars[i]);
                    i++;
                }
            }
            return mTempSB.ToString();
        }

        private static int isIdentifierInputChar(char c)
        {
            int j = 0;
            if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                j = 0;
            else if (c >= '0' && c <= '9')
                j = 1;
            else if (c == '_')
                j = 2;
            else
                j = 3;
            return j;
        }

        /// <summary>
        /// 是否是标识符
        /// </summary>
        public static string isIdentifier(string originData, int begin, ref int end)
        {
            checkLexical(originData, isIdentifierInputChar, null, LexicalAnalysis.IdentiferMatrix, 2, 3, begin, ref end);
            if (end > begin)
                return originData.Substring(begin, end - begin);
            return string.Empty;
        }

        /// <summary>
        /// 是否为关键字 false true null
        /// </summary>
        public static string isKeyword(string originData, int begin, ref int end)
        {
            string tempWord = isIdentifier(originData, begin, ref end);
            if (LexicalAnalysis.Keywords.Contains(tempWord))
                return tempWord;
            else
                return "";
        }

        /// <summary>
        /// 是否为不是关键字的标识符
        /// </summary>
        public static string IsNotKeywordIdentifer(string originData, int begin, ref int end)
        {
            string tempWord = isIdentifier(originData, begin, ref end);
            if (!LexicalAnalysis.Keywords.Contains(tempWord))
                return tempWord;
            else
                return "";
        }

        private static int isDigitInputChar(char c)
        {
            int j = 0;
            if (c == '0')
                j = 0;
            else if (c >= '1' && c <= '9')
                j = 1;
            else if (c == 'E' || c == 'e')
                j = 2;
            else if (c == '.')
                j = 3;
            else if (c == '+')
                j = 4;
            else if (c == '-')
                j = 5;
            else
                j = 6;
            return j;
        }

        /// <summary>
        /// 是否为数字，包括整数、实数、科学计数
        /// </summary>
        public static string isDigit(string originData, int begin, ref int end)
        {
            checkLexical(originData, isDigitInputChar, null, LexicalAnalysis.DigitMatrix, 9, 10, begin, ref end);
            if (end > begin)
                return originData.Substring(begin, end - begin);
            return string.Empty;
        }
        /// <summary>
        /// 根据词法分析状态机来分析Json字符串中的每种单词的类型
        /// </summary>
        /// <param name="originData">原始字符串</param>
        /// <param name="isInputCharFunc">输入状态检测函数 输入字符串 输出状态码</param>
        /// <param name="printErrorFunc">打印错误函数</param>
        /// <param name="stateMatrix">状态矩阵</param>
        /// <param name="endState">结束状态</param>
        /// <param name="errorState">错误状态 大于等于该状态视为错误</param>
        /// <param name="begin">字符串开始</param>
        /// <param name="end">字符串结束</param>
        private static void checkLexical(string originData, System.Func<char, int> isInputCharFunc, System.Action<int, string, int> printErrorFunc, int[][] stateMatrix,
                                  int endState, int errorState, int begin, ref int end)
        {
            int pCurrent = begin;
            int nState = 0;         //初始状态为0
            int nInputChar = 0;
            int nOriginDataLength = originData.Length;

            while (pCurrent < nOriginDataLength)
            {
                nInputChar = isInputCharFunc(originData[pCurrent]);
                nState = stateMatrix[nState][nInputChar];

                if (nState == endState)
                {
                    end = pCurrent;
                    break;
                }
                if (nState >= errorState)
                {
                    printErrorFunc?.Invoke(nState, originData, pCurrent);
                    end = pCurrent;
                    break;
                }
                pCurrent++;
            }

            if (nState != endState)
            {
                if (pCurrent >= nOriginDataLength)
                    end = pCurrent;
            }
        }

        private static void checkLexicalString(string originData, int begin, ref int end)
        {
            int pCurrent = begin;
            int nOriginDataLength = originData.Length;
            bool bBegin = false;
            bool bBackSlash = false;
            char c;
            while (pCurrent < nOriginDataLength)
            {
                c = originData[pCurrent];
                if (c == '\"')
                {
                    if (bBegin)
                    {
                        pCurrent++;
                        break;
                    }
                    bBegin = true;
                }
                else if (c == '\\')
                {
                    bBackSlash = true;
                    break;
                }
                pCurrent++;
            }
            if (bBackSlash)
                checkLexicalStringBackSlash(originData, begin, ref end);
            else
                end = pCurrent;
        }

        private static void checkLexicalStringBackSlash(string originData, int begin, ref int end)
        {
            int pCurrent = begin;
            int nState = 0;
            int nInputChar = 0;
            int nOriginDataLength = originData.Length;

            while (pCurrent < nOriginDataLength)
            {
                nInputChar = isStringInputChar(originData[pCurrent]);
                nState = StringMatrix[nState][nInputChar];

                if (nState == 5)
                {
                    end = pCurrent;
                    break;
                }
                if (nState >= 6)
                {
                    StringPrintError(nState, originData, pCurrent);
                    end = pCurrent;
                    break;
                }
                pCurrent++;
            }

            if (nState != 5)
            {
                if (pCurrent >= nOriginDataLength)
                    end = pCurrent;
            }
        }
    }
}
