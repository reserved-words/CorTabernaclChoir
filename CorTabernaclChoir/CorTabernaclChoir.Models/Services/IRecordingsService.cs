﻿using CorTabernaclChoir.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorTabernaclChoir.Common.Services
{
    public interface IRecordingsService
    {
        RecordingsViewModel Get();
    }
}