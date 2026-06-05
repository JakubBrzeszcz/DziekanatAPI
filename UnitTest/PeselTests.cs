using System;
using CoreApp.ValueObjects;
using Xunit;

namespace UnitTest;

public class PeselTests
{
    [Fact]
    public void Constructor_WithValidPesel_ShouldSetProperty()
    {
        // Arrange
        string validPesel = "99010112342";

        // Act
        var pesel = new Pesel(validPesel);

        // Assert
        Assert.Equal(validPesel, pesel.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1234567890")] // 10 znaków
    [InlineData("123456789012")] // 12 znaków
    [InlineData("99010a12342")] // Zawiera literę
    public void Constructor_WithInvalidFormat_ShouldThrowArgumentException(string? invalidPesel)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Pesel(invalidPesel!));
        Assert.Equal("PESEL musi składać się dokładnie z 11 cyfr.", exception.Message);
    }

    [Fact]
    public void Constructor_WithInvalidChecksum_ShouldThrowArgumentException()
    {
        // Arrange
        string invalidChecksumPesel = "99010112345"; // Zła suma kontrolna (na końcu 5 zamiast 2)

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Pesel(invalidChecksumPesel));
        Assert.Equal("Nieprawidłowa suma kontrolna PESEL.", exception.Message);
    }
}