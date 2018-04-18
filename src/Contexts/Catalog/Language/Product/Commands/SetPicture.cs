﻿using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Catalog.Product.Commands
{
    public class SetPicture : StampedCommand
    {
        public Guid ProductId { get; set; }
        public string PictureUrl { get; set; }
    }
}
