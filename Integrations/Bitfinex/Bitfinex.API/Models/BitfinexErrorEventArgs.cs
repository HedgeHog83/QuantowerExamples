// Copyright QUANTOWER LLC. © 2017-2024. All rights reserved.

using System;

namespace Bitfinex.API.Models;

public class BitfinexErrorEventArgs : EventArgs
{
    public Exception Exception { get; internal set; }
}