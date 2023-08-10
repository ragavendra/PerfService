// <copyright file="TestRequest.cs" company="Garden Systems"
//   Copyright (c) Garden Systems. All rights reserved.
// </copyright>

// ReSharper disable once CheckNamespace
// using PerfRunner.V1.Perf.PerfBase;

using System.Diagnostics;

namespace PerfRunner.V1
{
  /// <summary>
  /// Extend the protobuf type to add some internal fields.
  /// </summary>
  public sealed partial class ActionOption
  {
    /// <summary>
    /// The <see cref="ActionOption"/> duration in int equivalent for razor.
    /// </summary>
    public long DurationSeconds { get => this.Duration.Seconds; set => this.Duration.Seconds = value; }
  }
}