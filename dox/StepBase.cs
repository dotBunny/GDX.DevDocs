// Copyright (c) 2020-2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

namespace Dox
{
    public abstract class StepBase : IStep
    {
        /// <inheritdoc />
        public virtual void Clean()
        {

        }

        /// <inheritdoc />
        public virtual string GetIdentifier()
        {
            return null;
        }

        /// <inheritdoc />
        public virtual string[] GetRequiredStepIdentifiers()
        {
            return null;
        }

        /// <inheritdoc />
        public virtual string GetHeader()
        {
            return null;
        }

        /// <inheritdoc />
        public virtual void Process()
        {
        }

        /// <inheritdoc />
        public virtual void Setup()
        {
        }
    }
}