﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rent.Dtos
{
    public class PaymentConfirmDto
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
    }
}