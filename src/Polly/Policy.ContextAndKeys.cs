﻿namespace Polly;

public abstract partial class Policy
{
    /// <summary>
    /// Sets the PolicyKey for this <see cref="Policy"/> instance.
    /// <remarks>Must be called before the policy is first used.  Can only be set once.</remarks>
    /// </summary>
    /// <param name="policyKey">The unique, used-definable key to assign to this <see cref="Policy"/> instance.</param>
    public Policy WithPolicyKey(string policyKey)
    {
        if (policyKeyInternal != null) throw PolicyKeyMustBeImmutableException(nameof(policyKey));

        policyKeyInternal = policyKey;
        return this;
    }

    /// <summary>
    /// Sets the PolicyKey for this <see cref="Policy"/> instance.
    /// <remarks>Must be called before the policy is first used.  Can only be set once.</remarks>
    /// </summary>
    /// <param name="policyKey">The unique, used-definable key to assign to this <see cref="Policy"/> instance.</param>
    ISyncPolicy ISyncPolicy.WithPolicyKey(string policyKey)
    {
        if (policyKeyInternal != null) throw PolicyKeyMustBeImmutableException(nameof(policyKey));

        policyKeyInternal = policyKey;
        return this;
    }
}

public abstract partial class Policy<TResult>
{
    /// <summary>
    /// Sets the PolicyKey for this <see cref="Policy{TResult}"/> instance.
    /// <remarks>Must be called before the policy is first used.  Can only be set once.</remarks>
    /// </summary>
    /// <param name="policyKey">The unique, used-definable key to assign to this <see cref="Policy{TResult}"/> instance.</param>
    public Policy<TResult> WithPolicyKey(string policyKey)
    {
        if (policyKeyInternal != null) throw PolicyKeyMustBeImmutableException(nameof(policyKey));

        policyKeyInternal = policyKey;
        return this;
    }

    /// <summary>
    /// Sets the PolicyKey for this <see cref="Policy{TResult}"/> instance.
    /// <remarks>Must be called before the policy is first used.  Can only be set once.</remarks>
    /// </summary>
    /// <param name="policyKey">The unique, used-definable key to assign to this <see cref="Policy{TResult}"/> instance.</param>
    ISyncPolicy<TResult> ISyncPolicy<TResult>.WithPolicyKey(string policyKey)
    {
        if (policyKeyInternal != null) throw PolicyKeyMustBeImmutableException(nameof(policyKey));

        policyKeyInternal = policyKey;
        return this;
    }
}
