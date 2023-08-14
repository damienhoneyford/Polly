using System.Runtime.CompilerServices;

namespace Polly.Core.Benchmarks;

#pragma warning disable CA1822 // Mark members as static

public class ResiliencePipelineBenchmark
{
    [Benchmark(Baseline = true)]
    public async ValueTask ExecuteOutcomeAsync()
    {
        var context = ResilienceContextPool.Shared.Get();
        await NullResiliencePipeline.Instance.ExecuteOutcomeAsync((_, _) => Outcome.FromResultAsTask("dummy"), context, "state").ConfigureAwait(false);
        ResilienceContextPool.Shared.Return(context);
    }

    [Benchmark]
    public async ValueTask ExecuteAsync_ResilienceContextAndState()
    {
        var context = ResilienceContextPool.Shared.Get();
        await NullResiliencePipeline.Instance.ExecuteAsync((_, _) => new ValueTask<string>("dummy"), context, "state").ConfigureAwait(false);
        ResilienceContextPool.Shared.Return(context);
    }

    [Benchmark]
    public async ValueTask ExecuteAsync_CancellationToken()
    {
        await NullResiliencePipeline.Instance.ExecuteAsync(_ => new ValueTask<string>("dummy"), CancellationToken.None).ConfigureAwait(false);
    }

    [Benchmark]
    public async ValueTask ExecuteAsync_GenericStrategy_CancellationToken()
    {
        await NullResiliencePipeline<string>.Instance.ExecuteAsync(_ => new ValueTask<string>("dummy"), CancellationToken.None).ConfigureAwait(false);
    }

    [Benchmark]
    public void Execute_ResilienceContextAndState()
    {
        var context = ResilienceContextPool.Shared.Get();
        NullResiliencePipeline.Instance.Execute((_, _) => "dummy", context, "state");
        ResilienceContextPool.Shared.Return(context);
    }

    [Benchmark]
    public void Execute_CancellationToken()
    {
        NullResiliencePipeline.Instance.Execute(_ => "dummy", CancellationToken.None);
    }

    [Benchmark]
    public void Execute_GenericStrategy_CancellationToken()
    {
        NullResiliencePipeline<string>.Instance.Execute(_ => "dummy", CancellationToken.None);
    }

    public class NonGenericStrategy
    {
        [MethodImpl(MethodImplOptions.NoOptimization)]
        public virtual ValueTask<T> ExecuteAsync<T>(Func<ValueTask<T>> callback)
        {
            return callback();
        }
    }
}