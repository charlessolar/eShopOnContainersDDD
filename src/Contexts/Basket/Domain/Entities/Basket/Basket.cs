using eShop.Identity.User.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Aggregates;

namespace eShop.Basket.Basket
{
    public class Basket : Aggregates.Entity<Basket, State>
    {
        private Basket() { }

        public void Initiate(Identity.User.State user)
        {
            Apply<Events.Initiated>(x =>
            {
                x.BasketId = Id;
                x.UserName = user?.Id;
            });
        }

        public void Claim(Identity.User.State user)
        {
            if (!string.IsNullOrEmpty(State.UserName))
                throw new BusinessException("Basket already claimed");

            Apply<Events.BasketClaimed>(x =>
            {
                x.BasketId = Id;
                x.UserName = user.Id;
            });
        }
        public void Destroy()
        {
            Apply<Events.Destroyed>(x =>
            {
                x.BasketId = Id;
            });
        }
    }
}
