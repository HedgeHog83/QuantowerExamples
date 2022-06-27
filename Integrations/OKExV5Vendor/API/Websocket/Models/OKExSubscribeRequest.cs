// Copyright QUANTOWER LLC. � 2017-2022. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace OKExV5Vendor.API.Websocket.Models
{
    [Obfuscation(Exclude = true)]
    class OKExSubscribeRequest : OKExOperationRequest<OKExChannelRequest>
    {
        public OKExSubscribeRequest() => 
            this.Op = "subscribe";
    }
}
