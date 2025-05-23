// <copyright file="StringNullChecker.cs" company="dabordukov">
// Copyright (c) dabordukov. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace NullCounter.Test;

public class StringNullChecker : INullChecker<string>
{
    public bool IsNull(string value) => string.IsNullOrEmpty(value);
}
