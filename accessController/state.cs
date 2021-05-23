using System.Collections.Generic;

namespace accessController
{
    internal class controllersData
    {
        public string controllerIP { get; set; }
        public long controllerSN { get; set; }

        private List<controllersData> allControllers;

        public string AddClontroller(string ip, long sn)
        {
            controllersData newController = new controllersData();
            allControllers.Add(newController);
            return ("Controller Added");
        }
    }
}