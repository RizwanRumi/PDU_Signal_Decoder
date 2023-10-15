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

            if (dataLen < 4)
            {
                if (dataLen < 2)
                    pdu = null;
                else
                {
                    pdu.PDUId = data.Substring(0, 2);                    
                }                
            }
            else
            {
                pdu.PDUId = data.Substring(0, 2);
                pdu.PayloadData = data.Substring(2, data.Length - 2);

                protocol.ProtocolId = pdu.PayloadData.Substring(0, 2);
                protocol.ProtocolsData = pdu.PayloadData.Substring(2, (pdu.PayloadData.Length - 2));

                string signals = protocol.ProtocolsData;

                int len = signals.Length;

                // check nibble data length and if it is odd, add extra 0 value  
                if (len % 2 != 0)
                {
                    signals += "0";
                    len += 1;
                }

                // Create Signal_0 and Signal_1 according to ProtocolId
                protocol.Signals = new List<string>();

                string signal = "";
                int flag = 0;

                for (int i = 0; i < len; i++)
                {
                    signal += signals[i];

                    if (i % 2 == 1)
                    {
                        protocol.Signals.Add(signal);
                        signal = "";
                        flag++;

                        if (flag == 2)
                            break;
                    }

                }

                pdu.ProtocolData = protocol;
            }

            return pdu; 
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
