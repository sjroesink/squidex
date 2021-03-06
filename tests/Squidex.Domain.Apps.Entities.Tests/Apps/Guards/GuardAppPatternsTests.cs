﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschränkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;
using Squidex.Domain.Apps.Core.Apps;
using Squidex.Domain.Apps.Entities.Apps.Commands;
using Squidex.Domain.Apps.Entities.Apps.Guards;
using Squidex.Infrastructure;
using Xunit;

#pragma warning disable SA1310 // Field names must not contain underscore

namespace Squidex.Domain.Apps.Write.Apps.Guards
{
    public class GuardAppPatternsTests
    {
        private readonly Guid patternId = Guid.NewGuid();
        private readonly AppPatterns patterns_0 = AppPatterns.Empty;

        [Fact]
        public void CanAdd_should_throw_exception_if_name_empty()
        {
            var command = new AddPattern { PatternId = patternId, Name = string.Empty, Pattern = ".*" };

            Assert.Throws<ValidationException>(() => GuardAppPattern.CanAdd(patterns_0, command));
        }

        [Fact]
        public void CanAdd_should_throw_exception_if_id_empty_guid()
        {
            var command = new AddPattern { Name = string.Empty, Pattern = ".*" };

            Assert.Throws<ValidationException>(() => GuardAppPattern.CanAdd(patterns_0, command));
        }

        [Fact]
        public void CanAdd_should_throw_exception_if_pattern_empty()
        {
            var command = new AddPattern { PatternId = patternId, Name = "any", Pattern = string.Empty };

            Assert.Throws<ValidationException>(() => GuardAppPattern.CanAdd(patterns_0, command));
        }

        [Fact]
        public void CanAdd_should_throw_exception_if_pattern_not_valid()
        {
            var command = new AddPattern { PatternId = patternId, Name = "any", Pattern = "[0-9{1}" };

            Assert.Throws<ValidationException>(() => GuardAppPattern.CanAdd(patterns_0, command));
        }

        [Fact]
        public void CanAdd_should_throw_exception_if_name_exists()
        {
            var patterns_1 = patterns_0.Add(Guid.NewGuid(), "any", "[a-z]", "Message");

            var command = new AddPattern { PatternId = patternId, Name = "any", Pattern = ".*" };

            Assert.Throws<ValidationException>(() => GuardAppPattern.CanAdd(patterns_1, command));
        }

        [Fact]
        public void CanAdd_should_not_throw_exception_if_success()
        {
            var command = new AddPattern { PatternId = patternId, Name = "any", Pattern = ".*" };

            GuardAppPattern.CanAdd(patterns_0, command);
        }

        [Fact]
        public void CanDelete_should_throw_exception_if_pattern_not_found()
        {
            var command = new DeletePattern { PatternId = patternId };

            Assert.Throws<DomainObjectNotFoundException>(() => GuardAppPattern.CanDelete(patterns_0, command));
        }

        [Fact]
        public void CanDelete_should_not_throw_exception_if_success()
        {
            var patterns_1 = patterns_0.Add(patternId, "any", ".*", "Message");

            var command = new DeletePattern { PatternId = patternId };

            GuardAppPattern.CanDelete(patterns_1, command);
        }

        [Fact]
        public void CanUpdate_should_throw_exception_if_name_empty()
        {
            var patterns_1 = patterns_0.Add(patternId, "any", ".*", "Message");

            var command = new UpdatePattern { PatternId = patternId, Name = string.Empty, Pattern = ".*" };

            Assert.Throws<ValidationException>(() => GuardAppPattern.CanUpdate(patterns_1, command));
        }

        [Fact]
        public void CanUpdate_should_throw_exception_if_pattern_empty()
        {
            var patterns_1 = patterns_0.Add(patternId, "any", ".*", "Message");

            var command = new UpdatePattern { PatternId = patternId, Name = "any", Pattern = string.Empty };

            Assert.Throws<ValidationException>(() => GuardAppPattern.CanUpdate(patterns_1, command));
        }

        [Fact]
        public void CanUpdate_should_throw_exception_if_pattern_not_valid()
        {
            var patterns_1 = patterns_0.Add(patternId, "any", ".*", "Message");

            var command = new UpdatePattern { PatternId = patternId, Name = "any", Pattern = "[0-9{1}" };

            Assert.Throws<ValidationException>(() => GuardAppPattern.CanUpdate(patterns_1, command));
        }

        [Fact]
        public void CanUpdate_should_throw_exception_if_name_exists()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var patterns_1 = patterns_0.Add(id1, "Pattern1", "[0-5]", "Message");
            var patterns_2 = patterns_1.Add(id2, "Pattern2", "[0-4]", "Message");

            var command = new UpdatePattern { PatternId = id2, Name = "Pattern1", Pattern = "[0-4]" };

            Assert.Throws<ValidationException>(() => GuardAppPattern.CanUpdate(patterns_2, command));
        }

        [Fact]
        public void CanUpdate_should_throw_exception_if_pattern_exists()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var patterns_1 = patterns_0.Add(id1, "Pattern1", "[0-5]", "Message");
            var patterns_2 = patterns_1.Add(id2, "Pattern2", "[0-4]", "Message");

            var command = new UpdatePattern { PatternId = id2, Name = "Pattern2", Pattern = "[0-5]" };

            Assert.Throws<ValidationException>(() => GuardAppPattern.CanUpdate(patterns_2, command));
        }

        [Fact]
        public void CanUpdate_should_throw_exception_if_pattern_does_not_exists()
        {
            var command = new UpdatePattern { PatternId = patternId, Name = "Pattern1", Pattern = ".*" };

            Assert.Throws<DomainObjectNotFoundException>(() => GuardAppPattern.CanUpdate(patterns_0, command));
        }

        [Fact]
        public void CanUpdate_should_not_throw_exception_if_pattern_exist_with_valid_command()
        {
            var patterns_1 = patterns_0.Add(patternId, "any", ".*", "Message");

            var command = new UpdatePattern { PatternId = patternId, Name = "Pattern1", Pattern = ".*" };

            GuardAppPattern.CanUpdate(patterns_1, command);
        }
    }
}
