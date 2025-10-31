//#define CHECK_LOOP_REFERENCE_OFF
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Knight.Core
{/// <summary>
 /// 字节缓冲区
 /// </summary>
    public class ByteBuffer
    {
        /// <summary>
        /// 指示网络大小端
        /// </summary>
        public static Boolean IsLittleEndian = true;
        /// <summary>
        /// 空数据
        /// </summary>
        public static readonly Byte[] EmptyData = new Byte[0];
        /// <summary>
        /// 默认数据长度
        /// </summary>
        public const Int32 DefultDataLength = 4;
        /// <summary>
        /// 数据集合 不可使用Data.Length获取长度 应使用Length
        /// </summary>
        protected Byte[] Data;
        /// <summary>
        /// 读取偏移
        /// </summary>
        public Int32 ReadPos;
        /// <summary>
        /// 写入偏移 修改后将会覆盖之后的数据
        /// </summary>
        public Int32 WritePos;
        /// <summary>
        /// 字符串编码
        /// </summary>
        protected Encoding StringEncoding = Encoding.UTF8;
        /// <summary>
        /// 容量
        /// </summary>
        public virtual Int32 Capacity
        {
            get
            {
                return this.Data.Length;
            }
            set
            {
                if (value < this.WritePos)
                {
                    return;
                }
                if (value != this.Data.Length)
                {
                    if (value > 0)
                    {
                        var array = new Byte[value];
                        if (this.WritePos > 0)
                        {
                            Array.Copy(this.Data, 0, array, 0, this.WritePos);
                        }
                        this.Data = array;
                    }
                }
            }
        }
        /// <summary>
        /// 数据长度
        /// </summary>
        public virtual Int32 Length
        {
            get
            {
                return this.WritePos;
            }
        }
        /// <summary>
        /// 判断是否已读取到末尾
        /// </summary>
        public virtual Boolean Eof
        {
            get
            {
                return this.ReadPos >= this.Data.Length;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="capacity">容量</param>
        public ByteBuffer(Int32 capacity = 4)
        {
            this.Data = EmptyData;
            this.Capacity = capacity;
        }
        /// <summary>
        /// 以指定数据创建,不复制源数据
        /// </summary>
        /// <param name="data"></param>
        public ByteBuffer(Byte[] data)
        {
            this.Data = data ?? throw new ArgumentNullException(nameof(data));
            this.WritePos = data.Length;
        }
        /// <summary>
        /// 获取一个数据
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public virtual Byte this[Int32 index]
        {
            get
            {
                return this.Data[index];
            }
        }
        /// <summary>
        /// 获取一段数据(拷贝)
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public virtual Byte[] this[Int32 index, Int32 length]
        {
            get
            {
                return this.GetBytesAt(index, length);
            }
        }
        /// <summary>
        /// 清空 重置读取写入偏移 
        /// </summary>
        /// <param name="resetData"></param>
        public virtual void Clear(Boolean resetData = false)
        {
            this.ReadPos = 0;
            this.WritePos = 0;
            if (resetData)
            {
                Array.Clear(this.Data, 0, this.WritePos);
            }
        }
        /// <summary>
        /// 清空，并设置新的数据源
        /// </summary>
        /// <param name="data"></param>
        public virtual void Clear(Byte[] data)
        {
            this.Data = data ?? throw new ArgumentNullException(nameof(data));
            this.ReadPos = 0;
            this.WritePos = data.Length;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public virtual Byte[] ToArray()
        {
            return this.GetBytesAt(0, this.Length);
        }
        /// <summary>
        /// 获取数据 到目标buffer
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        public virtual void ToArray(byte[] buffer, int offset)
        {
            Array.Copy(this.Data, 0, buffer, offset, this.Length);
        }
        /// <summary>
        /// 获取数据，切记不要在外部进行修改
        /// </summary>
        /// <returns></returns>
        public virtual Byte[] GetData()
        {
            return this.Data;
        }
        /// <summary>
        /// 动态分配大小
        /// </summary>
        /// <param name="size"></param>
        protected virtual void EnsureCapacity(Int32 size)
        {
            var min = this.WritePos + size;
            if (this.Data.Length < min)
            {
                var num = (this.Data.Length == 0) ? DefultDataLength : (this.Data.Length * 2);
                if (num < min)
                {
                    num = min;
                }
                this.Capacity = num;
            }
        }
        /// <summary>
        /// 追加数据 不写入数据长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual ByteBuffer Append(Byte[] data)
        {
            return this.Append(data, 0, data.Length);
        }
        /// <summary>
        /// 追加数据 不写入数据长度
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual ByteBuffer Append(Byte[] data, Int32 index, Int32 length)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (index < 0 || length < 0 || index + length > data.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.EnsureCapacity(length);
            Array.Copy(data, index, this.Data, this.WritePos, length);
            this.WritePos += length;
            return this;
        }
        /// <summary>
        /// 获取数据 更新读取偏移
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual Byte[] GetBytes(Int32 length)
        {
            var data = this.GetBytesAt(this.ReadPos, length);
            this.ReadPos += length;
            return data;
        }
        /// <summary>
        /// 获取数据 不更新读取偏移
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual Byte[] GetBytesAt(Int32 index, Int32 length)
        {
            return this.Data.GetArray(index, length);
        }
        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Boolean Equals(Object obj)
        {
            if (!(obj is ByteBuffer))
            {
                return false;
            }
            if (!(obj is ByteBuffer other))
            {
                return false;
            }
            return this.GetBytesAt(0, this.Length).ArrayEquals(other.GetBytesAt(0, other.Length));
        }
        /// <summary>
        /// 获取HashCode(使用当前数据进行计算)
        /// </summary>
        /// <returns></returns>
        public override Int32 GetHashCode()
        {
            var hashCode = 0;
            var data = this.GetBytesAt(0, this.Length);
            for (var i = 0; i < data.Length; i++)
            {
                hashCode += data[i].GetHashCode();

            }
            return hashCode;
        }
        #region 压入数据
        /// <summary>
        /// 压入Byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push(Byte value)
        {
            this.EnsureCapacity(1);
            this.Data[this.WritePos++] = value;
            return this;
        }
        /// <summary>
        /// 压入SByte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push(SByte value)
        {
            return this.Push((Byte)value);
        }
        /// <summary>
        /// 压入Boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push(Boolean value)
        {
            this.EnsureCapacity(1);
            Byte b = 0;
            if (value)
            {
                b = 1;
            }
            this.Data[this.WritePos++] = b;
            return this;
        }
        /// <summary>
        /// 压入Int16
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push(Int16 value)
        {
            this.EnsureCapacity(2);
            if (IsLittleEndian)
            {
                this.Data[this.WritePos++] = (Byte)value;
                this.Data[this.WritePos++] = (Byte)(value >> 8);
            }
            else
            {
                this.Data[this.WritePos++] = (Byte)(value >> 8);
                this.Data[this.WritePos++] = (Byte)value;
            }
            return this;
        }
        /// <summary>
        /// 压入UInt16
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push(UInt16 value)
        {
            return this.Push((Int16)value);
        }
        /// <summary>
        /// 压入Char
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push(Char value)
        {
            return this.Push((Int16)value);
        }
        /// <summary>
        /// 压入Int32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push(Int32 value)
        {
            this.EnsureCapacity(4);
            if (IsLittleEndian)
            {
                this.Data[this.WritePos++] = (Byte)value;
                this.Data[this.WritePos++] = (Byte)(value >> 8);
                this.Data[this.WritePos++] = (Byte)(value >> 16);
                this.Data[this.WritePos++] = (Byte)(value >> 24);
            }
            else
            {
                this.Data[this.WritePos++] = (Byte)(value >> 24);
                this.Data[this.WritePos++] = (Byte)(value >> 16);
                this.Data[this.WritePos++] = (Byte)(value >> 8);
                this.Data[this.WritePos++] = (Byte)value;
            }
            return this;
        }
        /// <summary>
        /// 压入7-bit的Int32
        /// </summary>
        public virtual ByteBuffer PushVarints(UInt32 value)
        {
            this.EnsureCapacity(5);
            if (value < 128)
            {
                this.Data[this.WritePos++] = (Byte)value;
            }
            else
            {
                while (value > 127)
                {
                    this.Data[this.WritePos++] = (Byte)(value | 0x80U);
                    value >>= 7;
                }
                this.Data[this.WritePos++] = (Byte)value;
            }
            return this;
        }
        public virtual UInt32 Pop_UInt_varints()
        {
            // Varints encode int32 max size is 5
            Int32 tmp = this.Data[this.ReadPos++];
            if (tmp < 128)
            {
                return (UInt32)tmp;
            }
            var result = tmp & 0x7F;
            if ((tmp = this.Data[this.ReadPos++]) < 128)
            {
                result |= tmp << 7;
            }
            else
            {
                result |= (tmp & 0x7F) << 7;
                if ((tmp = this.Data[this.ReadPos++]) < 128)
                {
                    result |= tmp << 14;
                }
                else
                {
                    result |= (tmp & 0x7F) << 14;
                    if ((tmp = this.Data[this.ReadPos++]) < 128)
                    {
                        result |= tmp << 21;
                    }
                    else
                    {
                        result |= (tmp & 0x7F) << 21;
                        result |= (tmp = this.Data[this.ReadPos++]) << 28;
                        if (tmp >= 128)
                        {
                            for (var i = 0; i < 5; i++)
                            {
                                if (this.Data[this.ReadPos++] < 128)
                                {
                                    return (UInt32)result;
                                }
                            }
                        }
                    }
                }
            }
            return (UInt32)result;
        }
        /// <summary>
        /// 压入UInt32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push(UInt32 value)
        {
            return this.Push((Int32)value);
        }
        /// <summary>
        /// 压入Single
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual unsafe ByteBuffer Push(Single value)
        {
            return this.Push(*(Int32*)&value);
        }
        /// <summary>
        /// 压入Int64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push(Int64 value)
        {
            this.EnsureCapacity(8);
            if (IsLittleEndian)
            {
                this.Data[this.WritePos++] = (Byte)value;
                this.Data[this.WritePos++] = (Byte)(value >> 8);
                this.Data[this.WritePos++] = (Byte)(value >> 16);
                this.Data[this.WritePos++] = (Byte)(value >> 24);
                this.Data[this.WritePos++] = (Byte)(value >> 32);
                this.Data[this.WritePos++] = (Byte)(value >> 40);
                this.Data[this.WritePos++] = (Byte)(value >> 48);
                this.Data[this.WritePos++] = (Byte)(value >> 56);
            }
            else
            {
                this.Data[this.WritePos++] = (Byte)(value >> 56);
                this.Data[this.WritePos++] = (Byte)(value >> 48);
                this.Data[this.WritePos++] = (Byte)(value >> 40);
                this.Data[this.WritePos++] = (Byte)(value >> 32);
                this.Data[this.WritePos++] = (Byte)(value >> 24);
                this.Data[this.WritePos++] = (Byte)(value >> 16);
                this.Data[this.WritePos++] = (Byte)(value >> 8);
                this.Data[this.WritePos++] = (Byte)value;
            }
            return this;
        }
        /// <summary>
        /// 压入UInt64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push(UInt64 value)
        {
            return this.Push((Int64)value);
        }
        /// <summary>
        /// 压入Double
        /// </summary>
        /// <param name="vlaue"></param>
        /// <returns></returns>
        public virtual unsafe ByteBuffer Push(Double vlaue)
        {
            return this.Push(*(Int64*)&vlaue);
        }
        /// <summary>
        /// 压入Byte[]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push(Byte[] value)
        {
            if (value.Length > UInt16.MaxValue)
            {
                throw new ArgumentOutOfRangeException("value", $"value array length can't greater than{UInt16.MaxValue}");
            }
            _ = this.Push((UInt16)value.Length);
            return this.Append(value);
        }
        /// <summary>
        /// 压入String
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return this.Push((UInt16)0);
            }
            else
            {
                return this.Push(this.StringEncoding.GetBytes(value));
            }
        }
        /// <summary>
        /// 压入原始字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer PushOriginalStr(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return this.Push((UInt16)0);
            }
            else
            {
                var length = value.Length * 2;
                this.Push((UInt16)length);
                for (int i = 0; i < value.Length; i++)
                {
                    this.Push(value[i]);
                }
                return this;
            }
        }
        /// <summary>
        /// 压入字面常量Byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push_byte(Int32 value)
        {
            return this.Push((Byte)value);
        }
        /// <summary>
        /// 压入字面常量SByte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push_sbyte(Int32 value)
        {
            return this.Push((SByte)value);
        }
        /// <summary>
        /// 压入字面常量Int16
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push_short(Int32 value)
        {
            return this.Push((Int16)value);
        }
        /// <summary>
        /// 压入字面常量UInt16
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push_ushort(Int32 value)
        {
            return this.Push((UInt16)value);
        }
        /// <summary>
        /// 压入字面常量Char
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push_char(Int32 value)
        {
            return this.Push((Char)value);
        }
        /// <summary>
        /// 压入字面常量Int32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push_int(Int32 value)
        {
            return this.Push(value);
        }
        /// <summary>
        /// 压入字面常量UInt32
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push_uint(Int32 value)
        {
            return this.Push((UInt32)value);
        }
        /// <summary>
        /// 压入字面常量Single
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push_float(Int32 value)
        {
            return this.Push((Single)value);
        }
        /// <summary>
        /// 压入字面常量Single
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push_float(Double value)
        {
            return this.Push((Single)value);
        }
        /// <summary>
        /// 压入字面常量Int64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push_long(Int32 value)
        {
            return this.Push((Int64)value);
        }
        /// <summary>
        /// 压入字面常量UInt64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push_ulong(Int32 value)
        {
            return this.Push((UInt64)value);
        }
        /// <summary>
        /// 压入字面常量Double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push_double(Int32 value)
        {
            return this.Push((Double)value);
        }
        /// <summary>
        /// 压入字面常量Double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ByteBuffer Push_double(Double value)
        {
            return this.Push((Double)value);
        }
        #endregion
        #region 弹出数据
        /// <summary>
        /// 弹出Byte
        /// </summary>
        /// <returns></returns>
        public virtual Byte Pop_byte()
        {
            if (!this.Eof && this.ReadPos + 1 <= this.Length)
            {
                return this.Data[this.ReadPos++];
            }
            throw new ArgumentOutOfRangeException();
        }
        /// <summary>
        /// 弹出SByte
        /// </summary>
        /// <returns></returns>
        public virtual SByte Pop_sbyte()
        {
            return (SByte)this.Pop_byte();
        }
        /// <summary>
        /// 弹出Boolean
        /// </summary>
        /// <returns></returns>
        public virtual Boolean Pop_bool()
        {
            var b = false;
            if (this.Pop_byte() == 1)
            {
                b = true;
            }
            return b;

        }
        /// <summary>
        /// 弹出Int16
        /// </summary>
        /// <returns></returns>
        public virtual Int16 Pop_short()
        {
            if (this.Eof || this.ReadPos + 2 > this.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (IsLittleEndian)
            {
                return (Int16)(
                    (this.Data[this.ReadPos++] << 0) |
                    (this.Data[this.ReadPos++] << 8));
            }
            else
            {
                return (Int16)(
                    (this.Data[this.ReadPos++] << 8) |
                    (this.Data[this.ReadPos++] << 0));
            }
        }
        /// <summary>
        /// 弹出UInt16
        /// </summary>
        /// <returns></returns>
        public virtual UInt16 Pop_ushort()
        {
            return (UInt16)this.Pop_short();
        }
        /// <summary>
        /// 弹出Char
        /// </summary>
        /// <returns></returns>
        public virtual Char Pop_char()
        {
            return (Char)this.Pop_short();
        }
        /// <summary>
        /// 弹出Int32
        /// </summary>
        /// <returns></returns>
        public virtual Int32 Pop_int()
        {
            if (this.Eof || this.ReadPos + 4 > this.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (IsLittleEndian)
            {
                return
                    (this.Data[this.ReadPos++] << 0) |
                    (this.Data[this.ReadPos++] << 8) |
                    (this.Data[this.ReadPos++] << 16) |
                    (this.Data[this.ReadPos++] << 24);
            }
            else
            {
                return
                    (this.Data[this.ReadPos++] << 24) |
                    (this.Data[this.ReadPos++] << 16) |
                    (this.Data[this.ReadPos++] << 8) |
                    (this.Data[this.ReadPos++] << 0);
            }
        }
        /// <summary>
        /// 弹出UInt32
        /// </summary>
        /// <returns></returns>
        public virtual UInt32 Pop_uint()
        {
            return (UInt32)this.Pop_int();
        }
        /// <summary>
        /// 弹出Single
        /// </summary>
        /// <returns></returns>
        public virtual unsafe Single Pop_float()
        {
            var value = this.Pop_int();
            return *(Single*)&value;
        }
        /// <summary>
        /// 弹出Int64
        /// </summary>
        /// <returns></returns>
        public virtual Int64 Pop_long()
        {
            if (this.Eof || this.ReadPos + 8 > this.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (IsLittleEndian)
            {
                return
                    (((Int64)this.Data[this.ReadPos++]) << 0) |
                    (((Int64)this.Data[this.ReadPos++]) << 8) |
                    (((Int64)this.Data[this.ReadPos++]) << 16) |
                    (((Int64)this.Data[this.ReadPos++]) << 24) |
                    (((Int64)this.Data[this.ReadPos++]) << 32) |
                    (((Int64)this.Data[this.ReadPos++]) << 40) |
                    (((Int64)this.Data[this.ReadPos++]) << 48) |
                    (((Int64)this.Data[this.ReadPos++]) << 56);
            }
            else
            {
                return
                    (((Int64)this.Data[this.ReadPos++]) << 56) |
                    (((Int64)this.Data[this.ReadPos++]) << 48) |
                    (((Int64)this.Data[this.ReadPos++]) << 40) |
                    (((Int64)this.Data[this.ReadPos++]) << 32) |
                    (((Int64)this.Data[this.ReadPos++]) << 24) |
                    (((Int64)this.Data[this.ReadPos++]) << 16) |
                    (((Int64)this.Data[this.ReadPos++]) << 8) |
                    (((Int64)this.Data[this.ReadPos++]) << 0);
            }
        }
        /// <summary>
        /// 弹出UInt64
        /// </summary>
        /// <returns></returns>
        public virtual UInt64 Pop_ulong()
        {
            return (UInt64)this.Pop_long();
        }
        /// <summary>
        /// 弹出Double
        /// </summary>
        /// <returns></returns>
        public virtual unsafe Double Pop_double()
        {
            var value = this.Pop_long();
            return *(Double*)&value;
        }
        /// <summary>
        /// 弹出Byte数组
        /// </summary>
        /// <returns></returns>
        public virtual Byte[] Pop_bytes()
        {
            var length = this.Pop_ushort();
            return this.GetBytes(length);
        }
        /// <summary>
        /// 弹出String
        /// </summary>
        /// <returns></returns>
        public virtual String Pop_string()
        {
            return this.StringEncoding.GetString(this.Pop_bytes());
        }
        /// <summary>
        /// 弹出原始字符串
        /// </summary>
        /// <returns></returns>
        public virtual String Pop_originalString()
        {
            var length = this.Pop_ushort() / 2;
            var chars = new Char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = this.Pop_char();
            }
            var value = new String(chars);
            return value;
        }
        #endregion
        #region 自动压入弹出
        /// <summary>
        /// 是否处理私有字段
        /// </summary>
        public static Boolean IsHandlePrivateField { get; set; } = false;
        /// <summary>
        /// 是否处理私有属性
        /// </summary>
        public static Boolean IsHandlePrivateProperty { get; set; } = false;
        /// <summary>
        /// 类型比较
        /// </summary>
        private static readonly Type ByteType = typeof(Byte);
        private static readonly Type StringType = typeof(String);
        private static readonly Type IListType = typeof(IList);
        private static readonly Type IDictionaryType = typeof(IDictionary);
        /// <summary>
        /// 特性判断类型
        /// </summary>
        private static readonly Type ByteBufferIgnoreAttributeType = typeof(ByteBufferIgnoreAttribute);
#if !CHECK_LOOP_REFERENCE_OFF
        /// <summary>
        /// 自动压入
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public virtual ByteBuffer AutoPush(Object obj, Dictionary<Object, Int32> flags = null)
#else
        /// <summary>
        /// 自动压入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual ByteBuffer AutoPush(Object obj)
#endif
        {
            var type = obj.GetType();
            var typeInfo = type as TypeInfo;
            if (obj == null)
            {
                throw new NullReferenceException("Push object can't been null!");
            }
#if !CHECK_LOOP_REFERENCE_OFF
            if (flags == null)
            {
                flags = new Dictionary<Object, Int32>();
            }
            if (flags.ContainsKey(obj))
            {
                throw new Exception("Push object has loop reference!");
            }
#endif
            //值类型
            if (type.IsValueType)
            {
                //基础类型
                if (type.IsPrimitive)
                {
                    switch (type.Name)
                    {
                        case "Byte":
                            this.Push((Byte)obj);
                            break;
                        case "SByte":
                            this.Push((SByte)obj);
                            break;
                        case "Boolean":
                            this.Push((Boolean)obj);
                            break;
                        case "Int16":
                            this.Push((Int16)obj);
                            break;
                        case "UInt16":
                            this.Push((UInt16)obj);
                            break;
                        case "Char":
                            this.Push((Char)obj);
                            break;
                        case "Int32":
                            this.Push((Int32)obj);
                            break;
                        case "UInt32":
                            this.Push((UInt32)obj);
                            break;
                        case "Single":
                            this.Push((Single)obj);
                            break;
                        case "Int64":
                            this.Push((Int64)obj);
                            break;
                        case "UInt64":
                            this.Push((UInt64)obj);
                            break;
                        case "Double":
                            this.Push((Double)obj);
                            break;
                        //无法解析的基础类型
                        default:
                            throw new Exception("Auto push fail,can't analysis basse type!");
                    }
                }
                //枚举类型
                else if (type.IsEnum)
                {
                    this.Push((Int32)obj);
                }
                //结构体
                else
                {
#if !CHECK_LOOP_REFERENCE_OFF
                    this.AutoPushStructOrClass(ref typeInfo, ref obj, flags);
#else
                    this.AutoPushStructOrClass(ref typeInfo, ref obj);
#endif
                }
            }
            //类类型
            else if (type.IsClass)
            {
                //Array
                if (type.IsArray)
                {
#if !CHECK_LOOP_REFERENCE_OFF
                    //保存标记,防止循环引用
                    flags.Add(obj, 1);
#endif
                    if (obj is Array array)
                    {
                        var elementType = type.GetElementType();
                        //如果是Byte数组,直接进行写入
                        if (ByteType.IsAssignableFrom(elementType))
                        {
                            this.Push((Byte[])obj);
                        }
                        else
                        {
                            var length = array.Length;
                            this.Push((UInt16)length);
                            for (var i = 0; i < length; i++)
                            {
#if !CHECK_LOOP_REFERENCE_OFF
                                this.AutoPush(array.GetValue(i), flags);
#else
                                this.AutoPush(array.GetValue(i));
#endif
                            }
                        }
                    }
                }
                //String
                else if (StringType.IsAssignableFrom(type))
                {
                    this.Push((String)obj);
                }
                //IList
                else if (IListType.IsAssignableFrom(type))
                {
#if !CHECK_LOOP_REFERENCE_OFF
                    //保存标记,防止循环引用
                    flags.Add(obj, 1);
#endif
                    if (obj is IList list)
                    {
                        var count = list.Count;
                        this.Push((UInt16)count);
                        for (var i = 0; i < count; i++)
                        {
#if !CHECK_LOOP_REFERENCE_OFF
                            this.AutoPush(list[i], flags);
#else
                            this.AutoPush(list[i]);
#endif
                        }
                    }
                }
                //IDictionary
                else if (IDictionaryType.IsAssignableFrom(type))
                {
#if !CHECK_LOOP_REFERENCE_OFF
                    //保存标记,防止循环引用
                    flags.Add(obj, 1);
#endif
                    if (obj is IDictionary dictionary)
                    {
                        var count = dictionary.Count;
                        this.Push((UInt16)count);
                        foreach (DictionaryEntry kv in dictionary)
                        {
#if !CHECK_LOOP_REFERENCE_OFF
                            this.AutoPush(kv.Key, flags);
                            this.AutoPush(kv.Value, flags);
#else
                            this.AutoPush(kv.Key);
                            this.AutoPush(kv.Value);
#endif
                        }
                    }
                }
                //其他类类型
                else
                {
#if !CHECK_LOOP_REFERENCE_OFF
                    this.AutoPushStructOrClass(ref typeInfo, ref obj, flags);
#else
                    this.AutoPushStructOrClass(ref typeInfo, ref obj);
#endif
                }

            }
            //其他类型(不处理的类型)
            else
            {

            }
            return this;
        }
#if !CHECK_LOOP_REFERENCE_OFF

        /// <summary>
        /// 自动压入结构体或类
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <param name="obj"></param>
        /// <param name="flags"></param>
        public virtual void AutoPushStructOrClass(ref TypeInfo typeInfo, ref Object obj, Dictionary<Object, Int32> flags = null)
#else
        /// <summary>
        /// 自动压入结构体或类
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <param name="obj"></param>
        /// <param name="flags"></param>
        public virtual void AutoPushStructOrClass(ref TypeInfo typeInfo, ref Object obj, Dictionary<Object, Int32> flags = null)
#endif
        {
            //处理字段 跳过忽略属性
            var fieldInfoList = new List<FieldInfo>();
            //此处获取的字段包含了public标记与private标记
            fieldInfoList.AddRange(typeInfo.DeclaredFields);
            fieldInfoList.Sort((l, r) =>
            {
                return l.Name.CompareTo(r.Name);
            });
            var fieldInfos = fieldInfoList.ToArray();
            for (var i = 0; i < fieldInfos.Length; i++)
            {
                var fieldInfo = fieldInfos[i];
                if (!fieldInfo.IsDefined(ByteBufferIgnoreAttributeType, false) && (!fieldInfo.IsPrivate || IsHandlePrivateField))
                {
#if !CHECK_LOOP_REFERENCE_OFF
                    this.AutoPush(fieldInfo.GetValue(obj), flags);
#else
                    this.AutoPush(fieldInfo.GetValue(obj));
#endif
                }
            }
            //处理属性 跳过忽略属性
            var propertyInfoList = new List<PropertyInfo>();
            //此处获取的属性包含了public标记与private标记
            propertyInfoList.AddRange(typeInfo.DeclaredProperties);
            propertyInfoList.Sort((l, r) =>
            {
                return l.Name.CompareTo(r.Name);
            });
            var propertyInfos = propertyInfoList.ToArray();
            for (var i = 0; i < propertyInfos.Length; i++)
            {
                var propertyInfo = propertyInfos[i];
                if (!propertyInfo.IsDefined(ByteBufferIgnoreAttributeType, false) && ((propertyInfo.PropertyType.Attributes & TypeAttributes.Public) == TypeAttributes.Public || IsHandlePrivateProperty))
                {
#if !CHECK_LOOP_REFERENCE_OFF
                    this.AutoPush(propertyInfo.GetValue(obj), flags);
#else
                    this.AutoPush(propertyInfo.GetValue(obj));
#endif
                }
            }
        }
        /// <summary>
        /// 自动弹出
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual Object AutoPop(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            Object obj = null;
            var typeInfo = type as TypeInfo;
            //值类型
            if (type.IsValueType)
            {
                //基础类型
                if (type.IsPrimitive)
                {
#pragma warning disable IDE0066 // 将 switch 语句转换为表达式
                    switch (type.Name)
#pragma warning restore IDE0066 // 将 switch 语句转换为表达式
                    {
                        case "Byte":
                            obj = this.Pop_byte();
                            break;
                        case "SByte":
                            obj = this.Pop_sbyte();
                            break;
                        case "Boolean":
                            obj = this.Pop_bool();
                            break;
                        case "Int16":
                            obj = this.Pop_short();
                            break;
                        case "UInt16":
                            obj = this.Pop_ushort();
                            break;
                        case "Char":
                            obj = this.Pop_char();
                            break;
                        case "Int32":
                            obj = this.Pop_int();
                            break;
                        case "UInt32":
                            obj = this.Pop_uint();
                            break;
                        case "Single":
                            obj = this.Pop_float();
                            break;
                        case "Int64":
                            obj = this.Pop_long();
                            break;
                        case "UInt64":
                            obj = this.Pop_ulong();
                            break;
                        case "Double":
                            obj = this.Pop_double();
                            break;
                        //无法解析的基础类型
                        default:
                            throw new Exception("Auto pop fail,can't analysis basse type!");
                    }
                }
                //枚举类型
                else if (type.IsEnum)
                {
                    obj = this.Pop_int();
                }
                //结构体
                else
                {
                    obj = Activator.CreateInstance(type);
                    this.AutoPopStructOrClass(ref typeInfo, ref obj);
                }
            }
            //类类型
            else if (type.IsClass)
            {
                //Array
                if (type.IsArray)
                {
                    var elementType = type.GetElementType();
                    if (ByteType.IsAssignableFrom(elementType))
                    {
                        obj = this.Pop_bytes();
                    }
                    else
                    {
                        Int32 length = this.Pop_ushort();
                        var array = Array.CreateInstance(elementType, length);
                        for (var i = 0; i < length; i++)
                        {
                            array.SetValue(this.AutoPop(elementType), i);
                        }
                        obj = array;
                    }
                }
                //String
                else if (StringType.IsAssignableFrom(type))
                {
                    obj = this.Pop_string();
                }
                //IList
                else if (IListType.IsAssignableFrom(type))
                {
                    var genericArguments = type.GetGenericArguments();
                    Int32 count = this.Pop_ushort();
                    var list = Activator.CreateInstance(type) as IList;
                    for (var i = 0; i < count; i++)
                    {
                        _ = list.Add(this.AutoPop(genericArguments[0]));
                    }
                    obj = list;
                }
                //IDictionary
                else if (IDictionaryType.IsAssignableFrom(type))
                {
                    var genericArguments = type.GetGenericArguments();
                    Int32 count = this.Pop_ushort();
                    var dictionary = Activator.CreateInstance(type) as IDictionary;
                    for (var i = 0; i < count; i++)
                    {
                        dictionary.Add(this.AutoPop(genericArguments[0]), this.AutoPop(genericArguments[1]));
                    }
                    obj = dictionary;
                }
                //其他类类型
                else
                {
                    obj = Activator.CreateInstance(type);
                    this.AutoPopStructOrClass(ref typeInfo, ref obj);
                }
            }
            //其他类型(不处理的类型)
            else
            {

            }
            return obj;
        }
        /// <summary>
        /// 自动弹出结构体或类
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <param name="obj"></param>
        public virtual void AutoPopStructOrClass(ref TypeInfo typeInfo, ref Object obj)
        {
            //处理字段 仅处理具有ByteBufferAttribute特性的字段
            var fieldInfoList = new List<FieldInfo>();
            //此处获取的字段包含了public标记与private标记
            fieldInfoList.AddRange(typeInfo.DeclaredFields);
            fieldInfoList.Sort((l, r) =>
            {
                return l.Name.CompareTo(r.Name);
            });
            var fieldInfos = fieldInfoList.ToArray();
            for (var i = 0; i < fieldInfos.Length; i++)
            {
                var fieldInfo = fieldInfos[i];
                if (!fieldInfo.IsDefined(ByteBufferIgnoreAttributeType, false) && (!fieldInfo.IsPrivate || IsHandlePrivateField))
                {
                    fieldInfo.SetValue(obj, this.AutoPop(fieldInfo.FieldType));
                }
            }
            //处理属性 仅处理具有ByteBufferAttribute特性的属性
            var propertyInfoList = new List<PropertyInfo>();
            //此处获取的属性包含了public标记与private标记
            propertyInfoList.AddRange(typeInfo.DeclaredProperties);
            propertyInfoList.Sort((l, r) =>
            {
                return l.Name.CompareTo(r.Name);
            });
            var propertyInfos = propertyInfoList.ToArray();
            for (var i = 0; i < propertyInfos.Length; i++)
            {
                var propertyInfo = propertyInfos[i];
                if (!propertyInfo.IsDefined(ByteBufferIgnoreAttributeType, false) && ((propertyInfo.PropertyType.Attributes & TypeAttributes.Public) == TypeAttributes.Public || IsHandlePrivateProperty))
                {
                    propertyInfo.SetValue(obj, this.AutoPop(propertyInfo.PropertyType));
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// ByteBuffer自动压入弹出忽略属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ByteBufferIgnoreAttribute : Attribute { }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class BBIgnoreAttribute : ByteBufferIgnoreAttribute { }
}
