using System;
using System.Text;
using System.IO;
using System.Collections.Generic;


namespace dSPACE.Programming.Task
{
    public class SignalDecoder : IDisposable
    {
        private FileStream stream;
        private StreamReader reader;        

        public SignalDecoder()
        {
            stream = null;
            reader = null;            
            pduDataList = new List<PduData>();
        }

        public SignalDecoder(string path) : this()
        {
            Open(path);
        }

        ~SignalDecoder()
        {
            Close();
        }

        public List<PduData> pduDataList
        {
            get;
        }

        public void Open(string path)
        {
            // Make sure we are in a valid state
            Close();

            /*
             * Copy this Resource folder in dSPACE.Programming.Task\bin\Debug\netcoreapp3.1 folder
             * or copy fullpath from Resources folder and set path value.
             * For example: 
             *  path = E:\dspace_prog_test\dSPACE.Programming.Task\dSPACE.Programming.Task\Resources\InputFile.txt;
             */


            // Open a stream
            stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            reader = new StreamReader(stream, Encoding.UTF8);

            string data = String.Empty;            

            while ((data = reader.ReadLine()) != null)
            {
                var pduData = CreatePduPayloadData(data);
                pduDataList.Add(pduData);               
            }                       
        }        

        public PduData CreatePduPayloadData(string data)
        {
            var pdu = new PduData();
            var protocol = new ProtocolData();
            int dataLen = data.Length;

            /*
             * Exp: 0102 => DataLen = 4, go to else condition and get PduID and ProtocolId without signal
             * Exp: 010 => DataLen = 3, get PduId = 01 and ProtocolId = null
             * Exp: 01 => DataLen = 2, get PduId = 01 and ProtocolId = null
            */

            if (dataLen < 4)
             {
                if (dataLen < 2)
                    pdu = null;
                else
                {                    
                    pdu.PDUId = data.Substring(0, 2);

                    if (dataLen % 2 != 0)
                    {
                        pdu.PayloadData = data.Substring(2, data.Length - 2);
                        protocol.ProtocolId = pdu.PayloadData;
                    }
                }                
            }
            else
            {
                pdu.PDUId = data.Substring(0, 2);
                pdu.PayloadData = data.Substring(2, data.Length - 2);

                protocol.ProtocolId = pdu.PayloadData.Substring(0, 2);
                protocol.ProtocolsData = pdu.PayloadData.Substring(2, (pdu.PayloadData.Length - 2));
                protocol.Signals = CreateSignalList(protocol.ProtocolsData);
            }

            pdu.ProtocolData = protocol;

            return pdu; 
        }


        public List<string> CreateSignalList(string protocolsData)
        {
            var signalList = new List<string>(); 

            if(protocolsData == "")
            {
                signalList = null;
            }
            else
            {
                int len = protocolsData.Length;

                // check nibble data length and if it is odd, add extra "0" value  
                if (len % 2 != 0)
                {
                    protocolsData += "0";
                    len += 1;
                }

                // Create Signal_0 and Signal_1 according to ProtocolId
                string signal = "";
                int flag = 0;

                for (int i = 0; i < len; i++)
                {
                    signal += protocolsData[i];

                    if (i % 2 == 1)
                    {
                        signalList.Add(signal);
                        signal = "";
                        flag++;

                        if (flag == 2)
                            break;
                    }
                }
            }            
            return signalList;
        }

        public void Close()
        {
            stream?.Close();
            stream = null;
        }

        public void Dispose()
        {
            Close();
        }
    }
}
