using FluentValidation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace Infrastructure.Validation
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderInitial<T, string> IsColor<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Custom((data, context) =>
            {
                try
                {
                    int argb = Int32.Parse(data.Replace("#", ""), NumberStyles.HexNumber);
                    Color clr = Color.FromArgb(argb);
                }
                catch
                {
                    context.AddFailure("invalid rgb color");
                }
            });
        }

    }
}
