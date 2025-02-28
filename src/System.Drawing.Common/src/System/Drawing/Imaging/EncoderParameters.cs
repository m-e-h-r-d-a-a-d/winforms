﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.InteropServices;
using Gdip = System.Drawing.SafeNativeMethods.Gdip;

namespace System.Drawing.Imaging;

public sealed class EncoderParameters : IDisposable
{
    public EncoderParameters(int count) => Param = new EncoderParameter[count];

    public EncoderParameters() => Param = new EncoderParameter[1];

    public EncoderParameter[] Param { get; set; }

    internal unsafe nint ConvertToNative()
    {
        int length = Param.Length;

        // The struct has the first EncoderParameter in it.
        nint native = Marshal.AllocHGlobal(sizeof(EncoderParametersNative) + ((length - 1) * sizeof(EncoderParameterNative)));

        ((EncoderParametersNative*)native)->Count = (uint)length;
        var parameters = ((EncoderParametersNative*)native)->Parameters;

        for (int i = 0; i < length; i++)
        {
            parameters[i] = Param[i].ToNative();
        }

        return native;
    }

    internal static unsafe EncoderParameters ConvertFromNative(EncoderParametersNative* native)
    {
        if (native is null)
        {
            throw Gdip.StatusException(Gdip.InvalidParameter);
        }

        var nativeParameters = native->Parameters;
        EncoderParameters parameters = new(nativeParameters.Length);
        for (int i = 0; i < nativeParameters.Length; i++)
        {
            parameters.Param[i] = new EncoderParameter(
                new Encoder(nativeParameters[i].ParameterGuid),
                (int)nativeParameters[i].NumberOfValues,
                nativeParameters[i].ParameterValueType,
                (nint)nativeParameters[i].ParameterValue);
        }

        return parameters;
    }

    public void Dispose()
    {
        foreach (EncoderParameter p in Param)
        {
            p?.Dispose();
        }

        Param = null!;
    }
}
