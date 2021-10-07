using System;

namespace accessController
{
    class Program
    {
        static void Main(string[] args)
        {
            DeviceController con = new("192.168.0.122", 153162793);
            con.OpenDoor(1);
        }
    }
}
