﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuildingTests
{
    public interface IFireAlarmManager : IManager
    {
        public void SetAlarm(bool isActive);
    }
}
