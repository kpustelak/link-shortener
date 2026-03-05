using System.ComponentModel;
using LinkShortener.API.Interface;
using LinkShortener.API.Model.Dto.Response;
using LinkShortener.API.Model.Entities;
using LinkShortener.API.Services;
using StackExchange.Redis;

namespace LinkShortener.API.Tests;
using System;
using System.Threading.Tasks;
using Moq;
using Xunit;


public class LinkShortenerTests
{
    private readonly Mock<IRedisCacheService> _redisCacheMock;
    private readonly Mock<IPasswordService> _passwordMock;
    private readonly LinkService _sut;
    public const string ValidKey = "t123";
    public const string ValidUrl = "test-1234";
    public const string ValidPassword = "t123";
    

    public LinkShortenerTests()
    {
        _redisCacheMock = new Mock<IRedisCacheService>();
        _passwordMock = new Mock<IPasswordService>();
        _sut = new LinkService(_redisCacheMock.Object, _passwordMock.Object);
        
        LinkModel? storedModel = null;
        
        _redisCacheMock.Setup(x => x.GetAsync<LinkModel>("t123"))
            .ReturnsAsync(() => storedModel);
        
        _redisCacheMock.Setup(x => x.SetAsync("t123", It.IsAny <LinkModel>(), It.IsAny<TimeSpan?>()))
            .Callback<string, LinkModel, TimeSpan?>((key, model, timeSpan) => storedModel = model)
            .Returns(Task.CompletedTask);
        _passwordMock
            .Setup(x => x.HashPassword(It.IsAny<string>()))
            .Returns("hashed-password");

        _passwordMock
            .Setup(x => x.VerifyHashedPassword("hashed-password", It.IsAny<string>()))
            .Returns(false);
        
        _passwordMock
            .Setup(x => x.VerifyHashedPassword("hashed-password", ValidPassword))
            .Returns(true);
    }

    [Theory]
    [InlineData(ValidPassword)]
    [InlineData("")]
    public async Task CreateLinkAsync_WithValidData_ConfirmsLinkExistence(string password)
    {
        //Arrange
        
        //Act
        var linkKey = await _sut.CreateLinkAsync(ValidUrl, ValidKey, password);
    
        //Assert
        var isExisting = await _sut.LinkExistsAsync(linkKey);
        Assert.True(isExisting.IsLinkExisting);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task CreateLinkAsync_WithNullData_ThrowsNullException(string url)
    {
        //Arrange
        
        //Act
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            _sut.CreateLinkAsync(url,ValidKey,""));
        
        //Assert
        Assert.Equal("One of the required values is empty.", ex.Message);
    }

    [Fact]
    public async Task CreateLinkAsync_WithValidData_ConfirmLinkEquality()
    {
        //Arrange
        
        //Act
        var key = await _sut.CreateLinkAsync(ValidUrl, ValidKey, ValidPassword);
        
        //Assert
        var linkFromDatabase = await _sut.GetLinkAsync(key, ValidPassword);
        Assert.Equal(ValidUrl, linkFromDatabase);
    }
    

    [Fact]
    public async Task AddDuplicateToDatabase_WithValidDAta_ThrowsException()
    {
        //Arrange
        await _sut.CreateLinkAsync(ValidUrl, ValidKey, ValidPassword);
        
        //Act
        var ex = await Assert.ThrowsAsync<Exception>(
                () => _sut.CreateLinkAsync(ValidUrl, ValidKey, ValidPassword));

        //Assert
        Assert.Equal("This link already exists. Try other one.", ex.Message);
    }

    [Fact]
    public async Task GetLinkAsync_WithWrongPassword_ThrowsException()
    {
        //Arrange
        var wrongPassword = "abcde";

        //Act
        var key = await _sut.CreateLinkAsync(ValidUrl, ValidKey, ValidPassword);

        //Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _sut.GetLinkAsync(key, wrongPassword));
        Assert.Equal("This password does not match.", ex.Message);
    }

    [Fact]
    public async Task GetLinkAsync_WithWrongKey_ThrowsException()
    {
        //Arrange
        var wrongKey = "abcde";
        
        //Act
        await _sut.CreateLinkAsync(ValidUrl, ValidKey, ValidPassword);

        //Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => _sut.GetLinkAsync(wrongKey, ValidPassword));
        Assert.Equal("This link does not exists.", ex.Message);
    }

}