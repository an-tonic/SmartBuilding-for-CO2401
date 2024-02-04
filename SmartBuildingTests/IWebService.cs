using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuildingTests
{
    public interface IWebService
    {
        public void LogFireAlarm(string logDetails);

        public void LogStateChange(string logDetails);

        public void LogEngineerRequired(string logDetails);
    }
}
