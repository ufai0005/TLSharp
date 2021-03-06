﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleSharp.TL
{
    public class TLVector<T> : TLObject
    {
        [TLObject(481674261)]
        public List<T> lists = new List<T>() ;
        public override int Constructor
        {
            get
            {
                return 481674261;
            }
        }

        public override void DeserializeBody(BinaryReader br)
        {
            int count = br.ReadInt32();
            for(int i= 0;i< count;i++)
            {
                if (typeof(T) == typeof(int))
                {
                    lists.Add((T)Convert.ChangeType(br.ReadInt32(),typeof(T)));
                }
                else if (typeof(T)==typeof(long))
                {
                    lists.Add((T)Convert.ChangeType(br.ReadInt64(), typeof(T)));
                }
                else if (typeof(T) ==typeof(string))
                {
                    lists.Add((T)Convert.ChangeType(StringUtil.Deserialize(br), typeof(T)));
                }
                else if(typeof(T)==typeof(double))
                {
                    lists.Add((T)Convert.ChangeType(br.ReadDouble(), typeof(T)));
                }
                else if (typeof(T).BaseType == typeof(TLObject))
                {
                    int constructor = br.ReadInt32();
                    Type type = TLContext.getType(constructor);
                    object obj = Activator.CreateInstance(type);
                    type.GetMethod("DeserializeBody").Invoke(obj, new object[] { br });
                    lists.Add((T)Convert.ChangeType(obj, type));
                }
            }
        }

        public override void SerializeBody(BinaryWriter bw)
        {
			bw.Write(Constructor);
	        foreach (var item in lists.Cast<TLObject>())
	        {
		        item.SerializeBody(bw);
	        }
        }
    }
}
