using AuthService.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace AuthService.Domain.Tests;

public class UserTests
{
    [Fact]
    public void CreateUser_ShouldInitializeFieldCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var firstName = "Example First Name";
        var lastName = "Example Last Name";
        var username = "example";
        var email = "example@example.com";
        
        // Act
        var user = User.Create(id, firstName, lastName, username, email);
        
        // Assert
        user.Id.Should().Be(id);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Username.Value.Should().Be(username);
        user.Email.Value.Should().Be(email);
        user.EmailVerified.Should().BeFalse();
        user.ImageUrl.Should().BeEmpty();
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
        
        var user = User.Create(id, firstName, lastName, username, email);

        var newFirstName = "Example New First Name";
        var newLastName = "Example New Last Name";
        var newUsername = "newusername";
        var newEmail = "newemail@example.com";
        var imageUrl = "https://example.com/image.jpg";
        
        // Act 
        user.Update(newFirstName, newLastName, newUsername,newEmail, imageUrl);
        
        // Assert
        user.FirstName.Should().Be(newFirstName);
        user.LastName.Should().Be(newLastName);
        user.Username.Value.Should().Be(newUsername);
        user.Email.Value.Should().Be(newEmail);
        user.EmailVerified.Should().BeFalse();
        user.ImageUrl.Should().Be(imageUrl);
    }

    [Fact]
    public void VerifyEmail_ShouldVerifyEmailCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var firstName = "Example First Name";
        var lastName = "Example Last Name";
        var username = "example";
        var email = "example@example.com";
        
        var user = User.Create(id, firstName, lastName, username, email);
        // Act
        user.VerifyEmail();
        
        // Assert
        user.Id.Should().Be(id);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Username.Value.Should().Be(username);
        user.Email.Value.Should().Be(email);
        user.EmailVerified.Should().BeTrue();
        user.ImageUrl.Should().BeEmpty();
    }
    
    [Fact]
    public void CreateUser_ShouldThrowArgumentException_WhenIdIsEmpty()
    {
        // Arrange
        var id = string.Empty;
        var firstName = "Example First Name";
        var lastName = "Example Last Name";
        var username = "example";
        var email = "example@example.com";
        
        // Act
        var act = () => User.Create(id, firstName, lastName, username, email);
        
        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage($"Id cannot be null or empty. (Parameter '{nameof(id)}')")
            .And.ParamName.Should().Be("id");
    }
    
    [Fact]
    public void CreateUser_ShouldThrowArgumentException_WhenUsernameIsEmpty()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var firstName = "Example First Name";
        var lastName = "Example Last Name";
        var username = string.Empty;
        var email = "example@example.com";
        
        // Act
        var act = () => User.Create(id, firstName, lastName, username, email);
        
        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Username cannot be null or empty. (Parameter 'value')")
            .And.ParamName.Should().Be("value");
    }
    
    [Fact]
    public void CreateUser_ShouldThrowArgumentException_WhenEmailIsEmpty()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var firstName = "Example First Name";
        var lastName = "Example Last Name";
        var username = "example";
        var email = string.Empty;
        
        // Act
        var act = () => User.Create(id, firstName, lastName, username, email);
        
        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Email cannot be null or empty. (Parameter 'value')")
            .And.ParamName.Should().Be("value");
    }
    
    [Fact]
    public void CreateUser_ShouldThrowArgumentException_WhenEmailIsInvalid()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var firstName = "Example First Name";
        var lastName = "Example Last Name";
        var username = "example";
        var email = "example.gmail.com";
        
        // Act
        var act = () => User.Create(id, firstName, lastName, username, email);
        
        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Email is not in a valid format. (Parameter 'value')")
            .And.ParamName.Should().Be("value");
    }
}