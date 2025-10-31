//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System;
using System.Collections.Generic;

namespace Knight.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventAttribute : Attribute
    {
        public ulong  MsgCode;

        public EventAttribute(ulong nMsgCode)
        {
            this.MsgCode = nMsgCode;
        }
    }
}
