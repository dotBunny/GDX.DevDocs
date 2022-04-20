// Copyright (c) 2022 dotBunny Inc.
// dotBunny licenses this file to you under the BSL-1.0 license.
// See the LICENSE file in the project root for more information.

namespace Dox
{
    public interface IStep
    {
        /// <summary>
        ///     If you make it, you must clean it up.
        /// </summary>
        public void Clean();

        /// <summary>
        ///     Get the unique identifier used as a key when registering the step with the program. This is also the key
        ///     used when identifying steps manually.
        /// </summary>
        /// <remarks>Do not use spaces.</remarks>
        /// <returns>A string key.</returns>
        public string GetIdentifier();

        /// <summary>
        ///     Get the header text used in the console log to identify this sections output.
        /// </summary>
        /// <returns>A string name.</returns>
        public string GetHeader();

        /// <summary>
        ///     Process the given step.
        /// </summary>
        public void Process();
    }
}