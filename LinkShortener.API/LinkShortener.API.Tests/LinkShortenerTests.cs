using LinkShortener.API.Interface;
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

    public LinkShortenerTests()
    {
        _redisCacheMock = new Mock<IRedisCacheService>();
        _passwordMock = new Mock<IPasswordService>();
        _sut = new LinkService(_redisCacheMock.Object, _passwordMock.Object);
    }
    
}