using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vnc.Viewer
{
    class LOGHelper
    {
        private const string LogCategory = "RMS_RDPClient";

        public static RMS.Util.LOG.ILOG GetLogger(string name)
        {
            return RMS.Util.LOG.LOGManager.GetLogger(name, LogCategory);
        }

        public static RMS.Util.LOG.ILOG GetLogger(Type type)
        {
            return RMS.Util.LOG.LOGManager.GetLogger(type, LogCategory);
        }
    }
}
