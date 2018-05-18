using Infrastructure.Enumeration;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Payment.Payment
{
    public class Status : Enumeration<Status, string>
    {
        public static Status Submitted = new Status("SUBMITTED", "Submitted", "Payment has been submitted");
        public static Status Settled = new Status("SETTLED", "Settled", "Paymnet fully completed");
        public static Status Cancelled = new Status("CANCELLED", "Cancelled", "The payment was cancelled");

        public Status(string value, string displayName, string description) : base(value, displayName)
        {
            this.Description = description;
        }

        public string Description { get; private set; }
    }
}
