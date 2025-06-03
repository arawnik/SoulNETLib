using System;
using System.Security.Claims;
using System.Security.Principal;
using SoulNETLib.Common.Extension;
using SoulNETLibTests.Common.TestData.Models;
using Xunit;

namespace SoulNETLib.Tests.Extension
{
    public class ClaimExtensionTests
    {
        #region GetUserId

        [Fact]
        public void GetUserId_SpecificallySetIdentifier_ReturnId()
        {
            // Arrange
            IPrincipal test = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[] { new Claim(ClaimTypes.NameIdentifier, SampleStrings.str1) }
                )
            );

            // Act
            var ret = test.GetUserId();

            // Assert
            Assert.Equal(SampleStrings.str1, ret);
        }

        [Fact]
        public void GetUserId_IncorrectIdentifier_ThrowsNullReferenceException()
        {
            // Arrange
            IPrincipal test = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]
                    { //Set only incorrect ClaimType
                        new Claim(ClaimTypes.MobilePhone, SampleStrings.str1),
                    }
                )
            );

            // Act
            Func<string> act = () => test.GetUserId();

            // Assert
            Assert.Throws<NullReferenceException>(act);
        }

        [Fact]
        public void GetUserId_EmptyPrincipal_ThrowsNullReferenceException()
        {
            // Arrange
            IPrincipal test = new ClaimsPrincipal();

            // Act
            Func<string> act = () => test.GetUserId();

            // Assert
            Assert.Throws<NullReferenceException>(act);
        }

        #endregion
    }
}
