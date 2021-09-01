using System;

namespace accessController
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Controller cont = new Controller ("192.168.0.121",  223211117 );
            cont.OpenDoor(1);
        }
    }
}
