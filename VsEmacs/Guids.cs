// Guids.cs
// MUST match guids.h

using System;

namespace Company.VSPackage2
{
    internal static class GuidList
    {
        public const string guidVSPackage2PkgString = "eb4dc421-b163-4fd9-adca-133654d7f739";
        public const string guidVSPackage2CmdSetString = "135b6383-128d-469e-9ba8-9074f7ecfee3";

        public static readonly Guid guidVSPackage2CmdSet = new Guid(guidVSPackage2CmdSetString);
    };
}