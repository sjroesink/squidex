﻿// ==========================================================================
//  TaskExtensions.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System;
using System.Threading.Tasks;

namespace Squidex.Infrastructure.Tasks
{
    public static class TaskExtensions
    {
        public static void Forget(this Task task)
        {
        }

        public static Func<T, Task> ToAsync<T>(this Action<T> action)
        {
            return x =>
            {
                action(x);

                return TaskHelper.Done;
            };
        }
    }
}
