using System.Security.Claims;
using System.Security.Principal;
using SoulNETLib.Common.Extension;
using SoulNETLibTests.Common.TestData.Models;

namespace SoulNETLib.Tests.Extension;

public class ClaimExtensionTests
{
    #region GetUserId

    [Fact]
    public void GetUserId_SpecificallySetIdentifier_ReturnId()
    {
        // Arrange
        IPrincipal test = new ClaimsPrincipal(
            new ClaimsIdentity([new(ClaimTypes.NameIdentifier, SampleStrings.str1)])
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
                [ //Set only incorrect ClaimType
                    new Claim(ClaimTypes.MobilePhone, SampleStrings.str1),
                ]
            )
        );

        // Act
        string act() => test.GetUserId();

        // Assert
        Assert.Throws<NullReferenceException>((Func<string>)act);
    }

    [Fact]
    public void GetUserId_EmptyPrincipal_ThrowsNullReferenceException()
    {
        // Arrange
        IPrincipal test = new ClaimsPrincipal();

        // Act
        string act() => test.GetUserId();

        // Assert
        Assert.Throws<NullReferenceException>((Func<string>)act);
    }

    #endregion
}
