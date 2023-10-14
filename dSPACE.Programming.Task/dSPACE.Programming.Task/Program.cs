using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dSPACE.Programming.Task
{
    internal class Program
    {
        private static SignalDecoder signalDecoder = null;

        private static List<PduData> pduData = null;

        private static StringBuilder signal_1_1 = null;
        private static StringBuilder signal_2_1 = null;
        private static StringBuilder signal_2_2 = null;

        static void Main(string[] args)
        {
            Console.WriteLine("dSPACE Programming Task Solution:\n");

            signalDecoder = new SignalDecoder(@"Resources/InputFile.txt");

            try
            {
                pduData = signalDecoder.pduDataList.ToList();

                foreach (var pdu in pduData)
                {
                    if (pdu == null)
                    {
                        Console.WriteLine($"Invalid data format in Row {pduData.IndexOf(pdu) + 1}");
                    }
                    else
                    {
                        var signalList = pdu.ProtocolData.Signals;

                        if (pdu.PDUId == "01")
                        {
                            if (pdu.ProtocolData.ProtocolId == "02")
                            {
                                // Signal_1_1 is stored in BYTE 0 of PDU's ProtocolsData
                                signal_1_1 = new StringBuilder(signalList[0]);
                                int signal_1_1_decimal = Convert.ToInt32(signal_1_1.ToString(), 16);
                                Console.WriteLine($"Signal_1_1 value in Row {pduData.IndexOf(pdu) + 1} = {signal_1_1_decimal} (0x{signal_1_1})");
                            }
                            else
                            {
                                Console.WriteLine($"Invalid protocol data format in Row {pduData.IndexOf(pdu) + 1}");
                            }
                        }
                        else if (pdu.PDUId == "02")
                        {                           
                            
                            // check signal list and set '00' value for missing data of BYTE 1  
                            if (signalList.Count != 2)
                            {
                                signal_2_1 = new StringBuilder(signalList[0]);
                                signal_2_2 = new StringBuilder("00");
                            }
                            else
                            {
                                signal_2_1 = new StringBuilder(signalList[0]); 
                                signal_2_2 = new StringBuilder(signalList[1]);
                            }

                            
                            if (pdu.ProtocolData.ProtocolId == "01")
                            {
                                // Signal_2_1 is stored in BYTE 0 of PDU's ProtocolsData                                
                                int signal_2_1_decimal = Convert.ToInt32(signal_2_1.ToString(), 16);
                                Console.WriteLine($"Signal_2_1 value in Row {pduData.IndexOf(pdu) + 1} = {signal_2_1_decimal} (0x{signal_2_1})");
                            }
                            else if (pdu.ProtocolData.ProtocolId == "02")
                            {
                                // Signal_2_2 is stored in BYTE 1 of PDU's ProtocolsData                                
                                int signal_2_2_decimal = Convert.ToInt32(signal_2_2.ToString(), 16);
                                Console.WriteLine($"Signal_2_2 value in Row {pduData.IndexOf(pdu) + 1} = {signal_2_2_decimal} (0x{signal_2_2})");
                            }
                            else
                            {
                                Console.WriteLine($"Invalid protocol data format in Row {pduData.IndexOf(pdu) + 1}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Invalid PDU ID in Row {pduData.IndexOf(pdu) + 1}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                signalDecoder.Dispose();
                signal_1_1.Clear();
                signal_2_1.Clear();
                signal_2_2.Clear();
            }
        }
    }
}
