﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveDescribe.Interfaces
{
    public interface IHistoryItem
    {
        void Execute();
        void UnExecute();
    }
}
