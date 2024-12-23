using AuthService.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace AuthService.Domain.Tests;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldInitializeFieldCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var firstName = "Example First Name";
        var lastName = "Example Last Name";
        var username = "example";
        var email = "example@example.com";
        
        // Act
        var user = new User(id, firstName, lastName, username, email);
        
        // Assert
        user.Id.Should().Be(id);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Username.Should().Be(username);
        user.Email.Should().Be(email);
        user.EmailVerified.Should().Be(false);
        user.ImageUrl.Should().Be(string.Empty);
    }

    [Fact]
    public void Update_ShouldModifyUser()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var firstName = "Example First Name";
        var lastName = "Example Last Name";
        var username = "example";
        var email = "example@example.com";
        
        var user = new User(id, firstName, lastName, username, email);

        var newFirstName = "Example New First Name";
        var newLastName = "Example New Last Name";
        var newUsername = "newusername";
        var newEmial = "newemail@example.com";
        var imageUrl = "https://example.com/image.jpg";
        
        // Act 
        user.Update(newFirstName, newLastName, newUsername,newEmial, imageUrl);
        
        // Assert
        user.FirstName.Should().Be(newFirstName);
        user.LastName.Should().Be(newLastName);
        user.Username.Should().Be(newUsername);
        user.Email.Should().Be(newEmial);
        user.EmailVerified.Should().Be(false);
        user.ImageUrl.Should().Be(imageUrl);
    }
}