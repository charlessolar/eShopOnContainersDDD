using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Ordering.Order
{
    public class Status : Enumeration
    {
        public static Status Submitted = new Status("SUBMITTED", "Submitted", "Order has been submitted for processing");
        public static Status WaitingValidation = new Status("WAITING_VALIDATION", "Waiting Validation", "Order is waiting automatic validation");
        public static Status Confirmed = new Status("CONFIRMED", "Confirmed", "All items are confirmed with available stock");
        public static Status Paid = new Status("PAID", "Paid", "Order has been paid");
        public static Status Shipped = new Status("SHIPPED", "Shipped", "The order was shipped");
        public static Status StockException = new Status("NO_STOCK", "Stock Exception", "All the items are not in stock");
        public static Status Cancelled = new Status("CANCELLED", "Cancelled", "The order was cancelled");

        public Status(string value, string displayName, string description) : base(value, displayName)
        {
            this.Description = description;
        }

        public string Description { get; private set; }
    }
}
