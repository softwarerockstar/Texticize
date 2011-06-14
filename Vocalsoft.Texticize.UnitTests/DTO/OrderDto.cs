﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vocalsoft.Texticize.UnitTests.DTO
{
    class OrderDto
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}