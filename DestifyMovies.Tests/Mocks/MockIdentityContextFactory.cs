using System;
using DestifyMovies.Core.Identity;
using DestifyMovies.Core.Services.Authorization;
using NSubstitute;

namespace DestifyMovies.Tests.Mocks
{
    public class MockIdentityContextFactory : IIdentityContextFactory
    {
        private readonly IIdentityContext _identityContext;
        private readonly User _testUser;
        public MockIdentityContextFactory()
        {
            _testUser = new User
            {
                EmailAddress = "UnitTest@test.com",
                FirstName = "UnitTest",
                LastName = "UnitTest",
                Id = 1,
                UserApiToken = "Test_Token"
            };

            _identityContext = Substitute.For<IIdentityContext>();
            _identityContext.User.Returns(_testUser);
        }
        public IIdentityContext Create(IApiKeyService apiKeyService)
        {
            return _identityContext;
        }
    }
}
