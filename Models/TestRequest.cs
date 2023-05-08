﻿// <copyright file="TestOptions.cs" company="Blizzard Entertainment">
//   Copyright (c) Blizzard Entertainment. All rights reserved.
// </copyright>

// ReSharper disable once CheckNamespace
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
    internal CancellationTokenSource CancellationTokenSource { get; set; }

  }
}
