using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPFlow
{
    static class Program
    {
        static void DoTicks(Controller controller, uint ticks)
        {
            for (uint i = 0; i < ticks; ++i)
                controller.Tick();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Controller controller = new Controller(20);

            controller.network.AddLostPacket(1);
            controller.network.AddLostAck(1);

            Model.DataPacket packet = new Model.DataPacket(0, 0);
            controller.log.OnPacketSent(packet);
            controller.network.Send(packet);

            DoTicks(controller, 20);
            controller.log.OnPacketDelivered(packet);
            Model.Ack ack = new Model.Ack(20, 1);
            controller.log.OnAckSent(ack);
            controller.network.Send(ack);

            DoTicks(controller, 20);
            packet = new Model.DataPacket(40, 1);
            controller.log.OnPacketSent(packet);
            controller.network.Send(packet);

            DoTicks(controller, 20);
            ack = new Model.Ack(60, 1);
            controller.log.OnAckSent(ack);
            controller.network.Send(ack);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FlowForm(controller));
        }
    }
}
