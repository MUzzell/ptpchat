using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ptpchat.VerbHandlers
{
    using ptpchat.Class_Definitions;
    using ptpchat.Client_Class;
    using ptpchat.Communication_Messages;

    class ConnectVerbHandler : IVerbHandler
    {
        private ConnectMessage Message { get; set; }

        public void ParseBaseMessage(string messageJson)
        {
            throw new NotImplementedException();
        }

        public bool HandleMessage(ref SocketManager socketManager, ref PtpList<SocketManager> serverSocketManagers, ref PtpList<SocketManager> clientSocketManagers)
        {
            throw new NotImplementedException();
        }
    }
}
