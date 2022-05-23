using System;
using System.Collections.Generic;
using System.Text;

namespace AsyncNet
{
    //网络数据包
    public class AsyncPkg
    {
        public const int HeadLen = 4;
        public byte[] headBuff = null;
        public int headIndex = 0;

        public int bodyLen = 0;
        public byte[] bodyBuff = null;
        public int bodyIndex = 0;
        public AsyncPkg()
        {
            headBuff = new byte[HeadLen];
        }

        public void InitBodyBuff()
        {
            bodyLen = BitConverter.ToInt32(headBuff, 0);
            bodyBuff = new byte[bodyLen];
        }
        public void Reset()
        {
            headBuff = null;
            headIndex = 0;
            bodyBuff = null;
            bodyIndex = 0;
        }
    }
}
