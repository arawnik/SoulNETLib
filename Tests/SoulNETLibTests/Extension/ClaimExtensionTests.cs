using FluentAssertions;
using SoulNETLib.Common.Extension;
using SoulNETLibTests.Common.TestData.Enums;
using SoulNETLibTests.Common.TestData.Models;
using System;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace SoulNETLibTests.Extension
{
    public class ClaimExtensionTests
    {

        #region GetUserId

        [Fact]
        public void GetUserId_SpecificallySetIdentifier_ReturnId()
        {
            // Arrange
            IPrincipal test = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, SampleStrings.str1)
            }));

            // Act
            var ret = test.GetUserId();

            // Assert
            ret.Should().Be(SampleStrings.str1);
        }

        [Fact]
        public void GetUserId_IncorrectIdentifier_ThrowsNullReferenceException()
        {
            // Arrange
            IPrincipal test = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            { //Set only incorrect ClaimType
                new Claim(ClaimTypes.MobilePhone, SampleStrings.str1)
            }));

            // Act
            Func<string> act = () => test.GetUserId();

            // Assert
            act.Should().ThrowExactly<NullReferenceException>();
        }

        [Fact]
        public void GetUserId_EmptyPrincipal_ThrowsNullReferenceException()
        {
            // Arrange
            IPrincipal test = new ClaimsPrincipal();

            // Act
            Func<string> act = () => test.GetUserId();

            // Assert
            act.Should().ThrowExactly<NullReferenceException>();
        }

        #endregion

    }
}
