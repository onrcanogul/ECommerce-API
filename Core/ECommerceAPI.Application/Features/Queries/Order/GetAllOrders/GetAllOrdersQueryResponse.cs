﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Features.Queries.Order.GetAllOrders
{
    public class GetAllOrdersQueryResponse
    {
        public int TotalCount { get; set; }
        public object Orders { get; set; }
    }
    // 'orderCode', 'userName', 'totalPrice', 'createdDate'
}
