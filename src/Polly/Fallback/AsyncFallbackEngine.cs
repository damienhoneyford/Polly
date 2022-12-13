﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Fallback;

internal class AsyncFallbackEngine
{
    internal static async Task<TResult> ImplementationAsync<TResult>(
        Func<Context, CancellationToken, Task<TResult>> action,
        Context context,
        CancellationToken cancellationToken,
        ExceptionPredicates shouldHandleExceptionPredicates,
        ResultPredicates<TResult> shouldHandleResultPredicates,
        Func<DelegateResult<TResult>, Context, Task> onFallbackAsync,
        Func<DelegateResult<TResult>, Context, CancellationToken, Task<TResult>> fallbackAction,
        bool continueOnCapturedContext)
    {
        DelegateResult<TResult> delegateOutcome;

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await action(context, cancellationToken).ConfigureAwait(continueOnCapturedContext);

            if (!shouldHandleResultPredicates.AnyMatch(result))
            {
                return result;
            }

            delegateOutcome = new(result);
        }
        catch (Exception ex)
        {
            var handledException = shouldHandleExceptionPredicates.FirstMatchOrDefault(ex);
            if (handledException == null)
            {
                throw;
            }

            delegateOutcome = new(handledException);
        }

        await onFallbackAsync(delegateOutcome, context).ConfigureAwait(continueOnCapturedContext);

        return await fallbackAction(delegateOutcome, context, cancellationToken).ConfigureAwait(continueOnCapturedContext);
    }
}