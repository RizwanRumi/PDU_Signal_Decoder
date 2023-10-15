using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dSPACE.Programming.Task
{
    public class Program
    {
        private static SignalDecoder signalDecoder = null;

        private static List<PduData> pduList = null;

        private static StringBuilder signal_1_1 = null;
        private static StringBuilder signal_2_1 = null;
        private static StringBuilder signal_2_2 = null;

        static void Main(string[] args)
        {
            Console.WriteLine("dSPACE Programming Task Solution:\n");

            signalDecoder = new SignalDecoder(@"Resources/InputFile.txt");

            try
            {
                pduList = signalDecoder.pduDataList.ToList();

                foreach (var pdu in pduList)
                {
                    int row = pduList.IndexOf(pdu) + 1;

                    if (pdu == null)
                    {
                        Console.WriteLine($"Invalid data format in Row {row}");
                    }
                    else
                    {
                        if (pdu.ProtocolData != null)
                        {
                            if(pdu.ProtocolData.Signals != null)
                            {                                
                                GetSignal(pdu, row, out int sigDec, out string sigHex);
                            }
                            else
                            {
                                Console.WriteLine($"Signal data is missing in Row {row}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Protocol data is missing in Row {row}");
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

        public static void GetSignal(PduData pduData, int rowNumber, out int signalDec, out string signalHex)
        {
            signalDec = 0;
            signalHex = "";

            var signalList = pduData.ProtocolData.Signals;             

            if (pduData.PDUId == "01")
            {
                if (pduData.ProtocolData.ProtocolId == "02")
                {
                    // Signal_1_1 is stored in BYTE 0 of PDU's ProtocolsData
                    signal_1_1 = new StringBuilder(signalList[0]);
                    int signal_1_1_decimal = Convert.ToInt32(signal_1_1.ToString(), 16);
                    signalDec = signal_1_1_decimal;
                    signalHex = $"0x{ signal_1_1}";
                    Console.WriteLine($"Signal_1_1 value in Row {rowNumber} = {signal_1_1_decimal} (0x{signal_1_1})");
                }
                else
                {
                    Console.WriteLine($"Invalid protocol ID in Row {rowNumber}");
                }
            }
            else if (pduData.PDUId == "02")
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


                if (pduData.ProtocolData.ProtocolId == "01")
                {
                    // Signal_2_1 is stored in BYTE 0 of PDU's ProtocolsData                                
                    int signal_2_1_decimal = Convert.ToInt32(signal_2_1.ToString(), 16);
                    signalDec = signal_2_1_decimal;
                    signalHex = $"0x{signal_2_1}";
                    Console.WriteLine($"Signal_2_1 value in Row {rowNumber} = {signal_2_1_decimal} (0x{signal_2_1})");
                }
                else if (pduData.ProtocolData.ProtocolId == "02")
                {
                    // Signal_2_2 is stored in BYTE 1 of PDU's ProtocolsData                                
                    int signal_2_2_decimal = Convert.ToInt32(signal_2_2.ToString(), 16);
                    signalDec = signal_2_2_decimal;
                    signalHex = $"0x{signal_2_2}";
                    Console.WriteLine($"Signal_2_2 value in Row {rowNumber} = {signal_2_2_decimal} (0x{signal_2_2})");
                }
                else
                {
                    Console.WriteLine($"Invalid protocol data format in Row {rowNumber}");
                }
            }
            else
            {
                Console.WriteLine($"Invalid PDU ID in Row {rowNumber}");
            }            
        }
    }
}
