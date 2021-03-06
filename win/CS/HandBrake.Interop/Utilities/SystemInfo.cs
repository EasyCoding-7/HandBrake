﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemInfo.cs" company="HandBrake Project (http://handbrake.fr)">
//   This file is part of the HandBrake source code - It may be used under the terms of the GNU General Public License.
// </copyright>
// <summary>
//   The System Information.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace HandBrake.Interop.Utilities
{
    using System;
    using System.Diagnostics;

    using HandBrake.Interop.Interop.HbLib.Wrappers.Interfaces;
    using HandBrake.Interop.Interop.Providers;
    using HandBrake.Interop.Interop.Providers.Interfaces;

    using Interop.HbLib;

    public class SystemInfo
    {
        private static bool? isNvencH264Available;  // Local cache to prevent log spam.
        private static bool? isNvencH265Available;

        private static IHbFunctions hbFunctions;

        static SystemInfo()
        {
            IHbFunctionsProvider hbFunctionsProvider = new HbFunctionsProvider();
            hbFunctions = hbFunctionsProvider.GetHbFunctionsWrapper();
        }


        /* QuickSync Support */

        public static bool IsQsvAvailable
        {
            get
            {
                try
                {
                    return hbFunctions.hb_qsv_available() != 0;
                }
                catch (Exception)
                {
                    // Silent failure. Typically this means the dll hasn't been built with --enable-qsv
                    return false;
                }
            }
        }

        public static bool IsQsvAvailableH264
        {
            get
            {
                try
                {
                    return (hbFunctions.hb_qsv_available() & NativeConstants.HB_VCODEC_QSV_H264) != 0;
                }
                catch (Exception)
                {
                    // Silent failure. Typically this means the dll hasn't been built with --enable-qsv
                    return false;
                }
            }
        }

        public static bool IsQsvAvailableH265
        {
            get
            {
                try
                {
                    return (hbFunctions.hb_qsv_available() & NativeConstants.HB_VCODEC_QSV_H265) != 0;
                }
                catch (Exception)
                {
                    // Silent failure. Typically this means the dll hasn't been built with --enable-qsv
                    return false;
                }
            }
        }

        public static int QsvHardwareGeneration
        {
            get
            {
                try
                {
                    int cpu_platform = hbFunctions.hb_get_cpu_platform();
                    int hardware = hbFunctions.qsv_hardware_generation(cpu_platform);
                    return hardware;
                }
                catch (Exception exc)
                {
                    // Silent failure. -1 means unsupported.
                    Debug.WriteLine(exc);
                    return -1;
                }
            }
        }


        public static bool IsQsvAvailableH26510bit
        {
            get
            {
                try
                {
                    return (hbFunctions.hb_qsv_available() & NativeConstants.HB_VCODEC_QSV_H265_10BIT) != 0;
                }
                catch (Exception)
                {
                    // Silent failure. Typically this means the dll hasn't been built with --enable-qsv
                    return false;
                }
            }
        }


        /* AMD VCE Support */

        public static bool IsVceH264Available
        {
            get
            {
                try
                {
                    return hbFunctions.hb_vce_h264_available() != 0;
                }
                catch (Exception)
                {
                    // Silent failure. Typically this means the dll hasn't been built with --enable-qsv
                    return false;
                }
            }
        }

        public static bool IsVceH265Available
        {
            get
            {
                try
                {
                    return hbFunctions.hb_vce_h265_available() != 0;
                }
                catch (Exception)
                {
                    // Silent failure. Typically this means the dll hasn't been built with --enable-qsv
                    return false;
                }
            }
        }

        /* Nvidia NVEnc Support */

        public static bool IsNVEncH264Available
        {
            get
            {
                try
                {
                    if (isNvencH264Available == null)
                    {
                        isNvencH264Available = hbFunctions.hb_nvenc_h264_available() != 0;
                    }
                    
                    return isNvencH264Available.Value;
                }
                catch (Exception)
                {
                    // Silent failure. Typically this means the dll hasn't been built with --enable-qsv
                    return false;
                }
            }
        }

        public static bool IsNVEncH265Available
        {
            get
            {
                try
                {
                    if (!IsNVEncH264Available)
                    {
                        return false;
                    }

                    if (isNvencH265Available == null)
                    {
                        isNvencH265Available = hbFunctions.hb_nvenc_h265_available() != 0;
                    }

                    return isNvencH265Available.Value;
                }
                catch (Exception)
                {
                    // Silent failure. Typically this means the dll hasn't been built with --enable-qsv
                    return false;
                }
            }
        }
    }
}
