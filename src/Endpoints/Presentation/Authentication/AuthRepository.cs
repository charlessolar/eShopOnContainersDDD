using System;
using System.Collections.Generic;
using System.Linq;
using Aggregates.Extensions;
using Infrastructure.Extensions;
using Infrastructure.Queries;
using NServiceBus;
using Serilog;
using ServiceStack;
using ServiceStack.Auth;

namespace eShop.Presentation.Authentication
{
    public class AuthRepository : IUserAuthRepository
    {
        private IMessageSession _bus;
        private readonly ILogger _logger;

        public AuthRepository(IMessageSession bus)
        {
            _bus = bus;
            _logger = Log.Logger.For<AuthRepository>();
        }


        public void LoadUserAuth(IAuthSession session, IAuthTokens tokens)
        {
            throw new NotImplementedException();
        }

        public void SaveUserAuth(IAuthSession authSession)
        {
            throw new NotImplementedException();
        }

        public List<IUserAuthDetails> GetUserAuthDetails(string userAuthId)
        {
            return new List<IUserAuthDetails>();
        }

        public IUserAuthDetails CreateOrMergeAuthSession(IAuthSession authSession, IAuthTokens tokens)
        {
            throw new NotImplementedException();
        }

        public IUserAuth GetUserAuth(IAuthSession authSession, IAuthTokens tokens)
        {
            return GetUserAuth(authSession.UserAuthName);
        }

        public IUserAuth GetUserAuthByUserName(string userNameOrEmail)
        {
            return GetUserAuth(userNameOrEmail);
        }

        public void SaveUserAuth(IUserAuth userAuth)
        {
            var original = GetUserAuth(userAuth.UserName);

            if (original.DisplayName != userAuth.DisplayName)
                _bus.CommandToDomain(new Identity.User.Commands.ChangeName
                {
                    UserName = userAuth.UserName,
                    GivenName = userAuth.DisplayName
                }).Wait();

        }

        public bool TryAuthenticate(string userName, string password, out IUserAuth userAuth)
        {
            try
            {
                _bus.CommandToDomain(new Identity.User.Commands.Identify { UserName = userName, Password = password }).Wait();

                userAuth = GetUserAuthByUserName(userName);
            }
            catch (Exception e)
            {
                _logger.WarnEvent("AuthFailure", e, "Authenticate: {ExceptionType}: {ExceptionMessage}", e.GetType().Name, e.Message);
                userAuth = null;
                this.RecordInvalidLoginAttempt(userAuth);
                return false;
            }
            this.RecordSuccessfulLogin(userAuth);
            return true;
        }

        public bool TryAuthenticate(Dictionary<string, string> digestHeaders, string privateKey, int nonceTimeOut, string sequence,
            out IUserAuth userAuth)
        {
            throw new NotImplementedException();
        }

        public IUserAuth CreateUserAuth(IUserAuth newUser, string password)
        {
            newUser.ValidateNewUser(password);

            var command = new Identity.User.Commands.Register
            {
                UserName = newUser.UserName,
                Password = password,
                GivenName = newUser.DisplayName
            };

            _bus.CommandToDomain(command).Wait();

            return newUser;
        }

        public IUserAuth UpdateUserAuth(IUserAuth existingUser, IUserAuth newUser)
        {
            SaveUserAuth(newUser);
            return newUser;
        }

        public IUserAuth UpdateUserAuth(IUserAuth existingUser, IUserAuth newUser, string password)
        {
            _bus.CommandToDomain(new Identity.User.Commands.ChangePassword
            {
                UserName = newUser.UserName,
                NewPassword = password,
            }).Wait();
            SaveUserAuth(newUser);
            return newUser;
        }

        public IUserAuth GetUserAuth(string userAuthId)
        {
            var reply = _bus.RequestQuery<Identity.User.Queries.Identity, Identity.User.Models.User>(new Identity.User.Queries.Identity
            {
                UserName = userAuthId
            }).Result as Infrastructure.ServiceStack.QueryResponse<Identity.User.Models.User>;

            var user = reply?.Payload as Identity.User.Models.User;
            if (user == null)
                return null;

            var userAuth = new UserAuth
            {
                DisplayName = user.GivenName,
                PrimaryEmail = user.Id,
                LastLoginAttempt = DateTimeOffset.FromUnixTimeMilliseconds(user.LastLogin).DateTime,
                Roles = user.Roles?.ToList(),
            };

            return userAuth;
        }

        public void DeleteUserAuth(string userAuthId)
        {
            _bus.CommandToDomain(new Identity.User.Commands.Disable
            {
                UserName = userAuthId
            }).Wait();
        }
    }
}
