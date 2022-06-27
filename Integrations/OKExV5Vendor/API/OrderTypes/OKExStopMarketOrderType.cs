// Copyright QUANTOWER LLC. � 2017-2022. All rights reserved.

using System.Collections.Generic;
using TradingPlatform.BusinessLayer;
using TradingPlatform.BusinessLayer.Utils;

namespace OKExV5Vendor.API.OrderTypes
{
    class OKExStopMarketOrderType : StopOrderType
    {
        public OKExStopMarketOrderType(params TimeInForce[] allowedTimeInForce)
            : base(allowedTimeInForce)
        {
        }

        public override IList<SettingItem> GetOrderSettings(OrderRequestParameters parameters, FormatSettings formatSettings)
        {
            var settings = base.GetOrderSettings(parameters, formatSettings);

            if (parameters.Symbol.SymbolType != SymbolType.Options)
            {
                OKExOrderTypeHelper.AddTradeMode(parameters, settings);

                if (parameters.Symbol.SymbolType == SymbolType.Crypto)
                    OKExOrderTypeHelper.AddReduceOnly(settings);
                else if (parameters.Symbol.SymbolType != SymbolType.Options)
                    OKExOrderTypeHelper.AddOrderBehaviour(settings);
            }

            return settings;
        }

    }
}
