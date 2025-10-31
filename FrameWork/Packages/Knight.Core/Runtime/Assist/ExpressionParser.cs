using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Knight.Core
{
    public class ExpressionParser
    {
        public enum ExpressionSymbolType
        {
            Unknown = 0,    // 未知
            Identifier,     // 标识符
            Digit,          // 数字
            BracketsLeft,   // 左括号 '('
            BracketsRight,  // 右括号 ')'
            Operator,       // 操作符 + - * \
        }

        public class ExpressionSymbolItem
        {
            public string               Value;
            public ExpressionSymbolType Type;
            public int                  Priority;   // 优先级
            public float                RealValue;  // 值

            public ExpressionSymbolItem()
            {
            }

            public ExpressionSymbolItem(ExpressionSymbolItem rItem)
            {
                this.Value = rItem.Value;
                this.Type  = rItem.Type;
            }
        }
        
        public string                       OriginData;
        public bool                         IsValid;
        public List<ExpressionSymbolItem>   ResultQueue;
        public Stack<ExpressionSymbolItem>  CalcStack;

        public ExpressionParser(string rOriginData)
        {
            this.OriginData = rOriginData.Trim();
            this.IsValid    = true;
            this.CalcStack  = new Stack<ExpressionSymbolItem>();
        }

        public void Parser()
        {
            ExpressionSymbolItem rCurSymbol = null;
            this.IsValid = true;
            int end = 0;
            int i = 0;

            this.ResultQueue = new List<ExpressionSymbolItem>();
            var rOperatorStack = new Stack<ExpressionSymbolItem>();
            while(i < this.OriginData.Length)
            {
                if (LexicalAnalysis.isSpecialSymbol(this.OriginData, i, ref end))
                {
                    i = end;
                    continue;
                }

                rCurSymbol = this.BuildSymbolItem(i, ref end);
                if (rCurSymbol != null)
                {
                    switch (rCurSymbol.Type)
                    {
                        case ExpressionSymbolType.Unknown:
                            Knight.Core.LogManager.LogError("Expression format error.");
                            break;
                        case ExpressionSymbolType.Identifier:
                            this.ResultQueue.Add(rCurSymbol);
                            break;
                        case ExpressionSymbolType.Digit:
                            this.ResultQueue.Add(rCurSymbol);
                            break;
                        case ExpressionSymbolType.BracketsLeft:
                            rOperatorStack.Push(rCurSymbol);
                            break;
                        case ExpressionSymbolType.BracketsRight:
                            if (rOperatorStack.Count != 0)
                            {
                                var rTempSymbol = rOperatorStack.Peek();
                                while (rOperatorStack.Count != 0 && rTempSymbol.Type != ExpressionSymbolType.BracketsLeft)
                                {
                                    rTempSymbol = rOperatorStack.Pop();
                                    this.ResultQueue.Add(rTempSymbol);
                                }
                                if (rOperatorStack.Count != 0 && rTempSymbol.Type == ExpressionSymbolType.BracketsLeft)
                                {
                                    rOperatorStack.Pop();
                                }
                            }
                            break;
                        case ExpressionSymbolType.Operator:
                            if (rOperatorStack.Count != 0)
                            {
                                var rTempSymbol1 = rOperatorStack.Peek();
                                while (rOperatorStack.Count != 0 && rTempSymbol1.Priority > rCurSymbol.Priority)
                                {
                                    rTempSymbol1 = rOperatorStack.Pop();
                                    this.ResultQueue.Add(rTempSymbol1);
                                }
                            }
                            rOperatorStack.Push(rCurSymbol);
                            break;
                    }
                    i = end;
                    continue;
                }
                i++;
            }

            while(rOperatorStack.Count != 0)
            {
                var rTempSymbol2 = rOperatorStack.Pop();
                this.ResultQueue.Add(rTempSymbol2);
            }
        }

        public void SetIdentiferValue(string rIdentiferName, float fValue)
        {
            foreach (var rPair in this.ResultQueue)
            {
                if (rPair.Value.Equals(rIdentiferName))
                {
                    rPair.RealValue = fValue;
                }
            }
        }

        public float Calclate()
        {
            this.CalcStack.Clear();

            ExpressionSymbolItem rCurSymbol = null;
            int nResultQueueIndex = 0;
            while (nResultQueueIndex < this.ResultQueue.Count)
            {
                rCurSymbol = this.ResultQueue[nResultQueueIndex];
                nResultQueueIndex++;

                switch (rCurSymbol.Type)
                {
                    case ExpressionSymbolType.Identifier:
                    case ExpressionSymbolType.Digit:
                        this.CalcStack.Push(rCurSymbol);
                        break;
                    case ExpressionSymbolType.Operator:
                        var rDigitItem1 = this.CalcStack.Pop();
                        float rDigitItem2RealValue = 0;
                        if (CalcStack.Count!=0)
                        {
                            rDigitItem2RealValue = this.CalcStack.Pop().RealValue;
                        }

                        float fNewValue = 0.0f;

                        if (rCurSymbol.Value == "+")
                            fNewValue = rDigitItem2RealValue + rDigitItem1.RealValue;
                        else if (rCurSymbol.Value == "-")
                            fNewValue = rDigitItem2RealValue - rDigitItem1.RealValue;
                        else if (rCurSymbol.Value == "*")
                            fNewValue = rDigitItem2RealValue * rDigitItem1.RealValue;
                        else if (rCurSymbol.Value == "/")
                            fNewValue = rDigitItem2RealValue / rDigitItem1.RealValue;

                        var rNewDigitItem = new ExpressionSymbolItem()
                        {
                            Type = ExpressionSymbolType.Digit,
                            Value = fNewValue.ToString(),
                            RealValue = fNewValue,
                        };
                        this.CalcStack.Push(rNewDigitItem);
                        break;
                }
            }

            if (this.CalcStack.Count == 0) return 0;

            return this.CalcStack.Peek().RealValue;
        }

        private ExpressionSymbolItem BuildSymbolItem(int begin, ref int end)
        {
            if (this.OriginData[begin] == '(')
            {
                end = begin + 1;
                return new ExpressionSymbolItem() { Value = "(", Type = ExpressionSymbolType.BracketsLeft};
            }
            else if (this.OriginData[begin] == ')')
            {
                end = begin + 1;
                return new ExpressionSymbolItem() { Value = ")", Type = ExpressionSymbolType.BracketsRight };
            }
            else if (this.OriginData[begin] == '+')
            {
                end = begin + 1;
                return new ExpressionSymbolItem() { Value = "+", Type = ExpressionSymbolType.Operator, Priority = 1 };
            }
            else if (this.OriginData[begin] == '-')
            {
                end = begin + 1;
                return new ExpressionSymbolItem() { Value = "-", Type = ExpressionSymbolType.Operator, Priority = 1 };
            }
            else if (this.OriginData[begin] == '*')
            {
                end = begin + 1;
                return new ExpressionSymbolItem() { Value = "*", Type = ExpressionSymbolType.Operator, Priority = 2 };
            }
            else if (this.OriginData[begin] == '/')
            {
                end = begin + 1;
                return new ExpressionSymbolItem() { Value = "/", Type = ExpressionSymbolType.Operator, Priority = 2 };
            }

            string tempWord = "";
            // 如果是标识符
            if (!string.IsNullOrEmpty(tempWord = LexicalAnalysis.isIdentifier(this.OriginData, begin, ref end)))
            {
                return new ExpressionSymbolItem() { Value = tempWord, Type = ExpressionSymbolType.Identifier };
            }
            // 如果是数字
            if (!string.IsNullOrEmpty(tempWord = LexicalAnalysis.isDigit(this.OriginData, begin, ref end)))
            {
                return new ExpressionSymbolItem()
                {
                    Value = tempWord,
                    Type = ExpressionSymbolType.Digit,
                    RealValue = float.Parse(tempWord)
                };
            }

            this.IsValid = false;
            return null;
        }
    }
}
