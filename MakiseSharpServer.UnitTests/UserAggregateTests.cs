using System;
using MakiseSharpServer.Domain.Entities.UserAggregate;
using Xunit;

namespace MakiseSharpServer.UnitTests
{
    public class UserAggregateTests
    {
        User user;
        public UserAggregateTests()
        {
            user = new User(1, "a", "b");
        }

        [Fact]
        public void User_ctr_should_not_accept_nulls()
        {
            Assert.Throws<ArgumentNullException>(() => new User(1, null, "xyz"));
            Assert.Throws<ArgumentNullException>(() => new User(1, "xyz", null));
        }

        [Fact]
        public void User_should_not_change_credentials_null()
        {
            Assert.Throws<ArgumentNullException>(() => user.ChangeDiscordCredentials(null, "xyz"));
            Assert.Throws<ArgumentNullException>(() => user.ChangeDiscordCredentials("xyz", null));
        }

        [Fact]
        public void User_should_change_credentials()
        {
            //Arrange
            const string accessToken = "abc";
            const string refreshToken = "xyz";

            //Act
            user.ChangeDiscordCredentials(accessToken, refreshToken);

            //Assert
            Assert.Equal(accessToken, user.DiscordAccessToken);
            Assert.Equal(refreshToken, user.DiscordRefreshToken);
        }
    }
}
