using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Aggregates;
using Infrastructure.Extensions;
using Infrastructure.Queries;

namespace eShop.Identity.User
{
    public class Handler :
        IHandleQueries<Queries.Identity>,
        IHandleMessages<Commands.Register>,
        IHandleMessages<Commands.ChangeName>,
        IHandleMessages<Commands.ChangePassword>,
        IHandleMessages<Commands.Identify>,
        IHandleMessages<Commands.Enable>,
        IHandleMessages<Commands.Disable>
    {
        public async Task Handle(Queries.Identity query, IMessageHandlerContext ctx)
        {
            var user = await ctx.For<User>().Get(query.UserName).ConfigureAwait(false);
            await ctx.Result(user).ConfigureAwait(false);
        }
        public async Task Handle(Commands.Register command, IMessageHandlerContext ctx)
        {
            var user = await ctx.For<User>().New(command.UserName).ConfigureAwait(false);
            user.Register(command.GivenName, command.Password);
        }
        public async Task Handle(Commands.ChangeName command, IMessageHandlerContext ctx)
        {
            var user = await ctx.For<User>().Get(command.UserName).ConfigureAwait(false);
            user.ChangeName(command.GivenName);
        }

        public async Task Handle(Commands.ChangePassword command, IMessageHandlerContext ctx)
        {
            var user = await ctx.For<User>().Get(command.UserName).ConfigureAwait(false);
            user.ChangePassword(command.Password, command.NewPassword);
        }
        public async Task Handle(Commands.Identify command, IMessageHandlerContext ctx)
        {
            var user = await ctx.For<User>().Get(command.UserName).ConfigureAwait(false);
            user.Identify(command.Password);
        }

        public async Task Handle(Commands.Enable command, IMessageHandlerContext ctx)
        {
            var user = await ctx.For<User>().Get(command.UserId).ConfigureAwait(false);
            user.Enable();
        }

        public async Task Handle(Commands.Disable command, IMessageHandlerContext ctx)
        {
            var user = await ctx.For<User>().Get(command.UserId).ConfigureAwait(false);
            user.Disable();
        }
    }
}
