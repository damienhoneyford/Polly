﻿using System;
using FluentAssertions;
using Polly.Caching;
using Xunit;

namespace Polly.Specs.Caching;

public class ResultTtlSpecs
{
    [Fact]
    public void Should_throw_when_func_is_null()
    {
        Action configure = () => new ResultTtl<object>((Func<object, Ttl>)null);

        configure.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("ttlFunc");
    }

    [Fact]
    public void Should_throw_when_func_is_null_using_context()
    {
        Action configure = () => new ResultTtl<object>((Func<Context, object, Ttl>)null);

        configure.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("ttlFunc");
    }

    [Fact]
    public void Should_not_throw_when_func_is_set()
    {
        Action configure = () => new ResultTtl<object>(_ => new());

        configure.Should().NotThrow();
    }

    [Fact]
    public void Should_not_throw_when_func_is_set_using_context()
    {
        Action configure = () => new ResultTtl<object>((_, _) => new());

        configure.Should().NotThrow();
    }

    [Fact]
    public void Should_return_func_result()
    {
        var ttl = TimeSpan.FromMinutes(1);
        Func<dynamic, Ttl> func = result => new Ttl(result.Ttl);

        var ttlStrategy = new ResultTtl<dynamic>(func);

        var retrieved = ttlStrategy.GetTtl(new("someOperationKey"), new { Ttl = ttl });
        retrieved.Timespan.Should().Be(ttl);
        retrieved.SlidingExpiration.Should().BeFalse();
    }

    [Fact]
    public void Should_return_func_result_using_context()
    {
        const string specialKey = "specialKey";

        var ttl = TimeSpan.FromMinutes(1);
        Func<Context, dynamic, Ttl> func = (context, result) => context.OperationKey == specialKey ? new(TimeSpan.Zero) : new Ttl(result.Ttl);

        var ttlStrategy = new ResultTtl<dynamic>(func);

        ttlStrategy.GetTtl(new("someOperationKey"), new { Ttl = ttl }).Timespan.Should().Be(ttl);
        ttlStrategy.GetTtl(new(specialKey), new { Ttl = ttl }).Timespan.Should().Be(TimeSpan.Zero);
    }
}