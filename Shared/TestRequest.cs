// <copyright file="TestRequest.cs" company="Garden Systems"
//   Copyright (c) Garden Systems. All rights reserved.
// </copyright>

// ReSharper disable once CheckNamespace
// using PerfRunner.V1.Perf.PerfBase;

namespace PerfRunner.V1
{
  /// <summary>
  /// Extend the protobuf type to add some internal fields.
  /// </summary>
  public sealed partial class TestRequest
  {
    /// <summary>
    /// The <see cref="TestStateManager"/> the test should run with.
    /// </summary>
    public CancellationTokenSource CancellationTokenSource { get; set; }
  }
}
