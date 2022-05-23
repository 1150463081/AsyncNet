﻿using System;
using System.Collections.Generic;
using System.Text;
using AsyncProtocol;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace AsyncNet
{
    public partial class Utility
    {

        public byte[] Serialize(AsyncMsg msg)
        {
            byte[] data = null;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                bf.Serialize(ms, msg);
                ms.Seek(0, SeekOrigin.Begin);
                data = ms.ToArray();
            }
            catch (SerializationException e)
            {
                LogError("Failed to Serialize:{0}", e.Message);
            }
            finally
            {
                ms.Close();
            }
            return data;
        }
        public AsyncMsg DeSerialize(byte[] data)
        {
            AsyncMsg msg = null;
            MemoryStream ms = new MemoryStream(data);
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                msg= bf.Deserialize(ms) as AsyncMsg;
            }
            catch (SerializationException e)
            {
                LogError("Failed to Serialize:{0}", e.Message);
            }
            finally
            {
                ms.Close();
            }
            return msg;
        }

    }
}