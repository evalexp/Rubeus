﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Rubeus.Kerberos.PAC {
    public class ClientName : PacInfoBuffer {
        public ClientName(DateTime clientId, string name) {
            ClientId = clientId;
            NameLength = (short)(name.Length * 2);
            Name = name;
        }

        public ClientName(byte[] data) : base(data, PacInfoBufferType.ClientName) {
            Decode(data);
        }

        public DateTime ClientId { get; set; }
        public short NameLength { get; private set; }
        public string Name { get; set; }

        protected override void Decode(byte[] data) {           
            ClientId = DateTime.FromFileTime(br.ReadInt64());
            NameLength = br.ReadInt16();
            Name = Encoding.Unicode.GetString(br.ReadBytes(NameLength));
        }

        public override byte[] Encode() {
            BinaryWriter bw = new BinaryWriter(new MemoryStream());
            bw.Write(ClientId.ToFileTime());
            bw.Write(NameLength);
            bw.Write(Encoding.Unicode.GetBytes(Name));
            return ((MemoryStream)bw.BaseStream).ToArray();            
        }   
    }
}
