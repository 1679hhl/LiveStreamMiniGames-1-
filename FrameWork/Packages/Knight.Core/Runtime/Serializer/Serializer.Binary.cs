//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System.IO;
using System;

namespace Knight.Core.Serializer
{
    public interface IReadWriteFile
    {
        void Save(string rFilePath);
        void Read(byte[] rBytes);
    }

    [TSIgnore]
    public class SerializerBinary : IReadWriteFile
    {
        public virtual void Serialize(BinaryWriter rWriter) { }
        public virtual void Deserialize(BinaryReader rReader) { }
        public virtual void Save(string rFilePath) { }
        public virtual void Read(byte[] rBytes) { }

        public virtual void CustomHandle() { }
    }

    public class SerializerBinaryTypes : TypeSearchDefault<SerializerBinary> { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SBIgnoreAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SBEnableAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SBDynamicAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SBGroupAttribute : Attribute
    {
        public string GroupName;

        public SBGroupAttribute(string rGroupName)
        {
            this.GroupName = rGroupName;
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class SBGroupIneritedAttribute : SBGroupAttribute
    {
        public SBGroupIneritedAttribute(string rGroupName)
            : base(rGroupName)
        {
        }
    }

    public class SBFileReadWriteAttribute : Attribute
    {
        public SBFileReadWriteAttribute()
        {
        }
    }
}