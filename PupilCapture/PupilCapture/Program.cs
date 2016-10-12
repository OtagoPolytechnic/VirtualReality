using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;
using MsgPack.Serialization;
using System.IO;
using MsgPack;

namespace PupilCapture
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = "192.168.204.128";
            int port = 50020;
            ZSocket requester = new ZSocket(ZSocketType.REQ);
            string reqAddress = string.Format("tcp://{0}:{1}", ip, port);
            requester.Connect("tcp://192.168.204.128:50020");
            requester.Send(new ZFrame("SUB_PORT"));

            ZFrame reply = requester.ReceiveFrame();
            string sub_port = reply.ReadString();

            ZSocket subscriber = new ZSocket(ZSocketType.SUB);
            string subAddress = string.Format("tcp://{0}:{1}", ip, sub_port);
            subscriber.Connect(subAddress);
            subscriber.SetOption(ZSocketOption.SUBSCRIBE, "pupil.0");
            subscriber.SetOption(ZSocketOption.SUBSCRIBE, "pupil.1");

            while (true)
            {
                IEnumerable<ZFrame> response = subscriber.ReceiveFrames(2);
                ZFrame[] responseArray = response.ToArray();
                string topic = responseArray[0].ReadString();
                byte[] payload = responseArray[1].Read();

                //MessagePackObjectDictionary mpoDict = MessagePackSerializer.Create<MessagePackObjectDictionary>().Unpack(new MemoryStream(rawMessage));

                var mpoDict = Unpacking.UnpackDictionary(payload).Value;


                string message = mpoDict["norm_pos"].ToString();
                Console.WriteLine("{0}: {1}", topic, message);
            }                            
            
        }
    }
}
