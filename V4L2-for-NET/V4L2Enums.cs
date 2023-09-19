using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V4L2_for_NET
{
    public enum v4l2_field : UInt32
    {
        /// <summary>
        /// driver can choose from none, top, bottom, interlaced depending on whatever it thinks is approximate
        /// </summary>
        ANY = 0,
        /// <summary>
        /// this device has no fields
        /// </summary>
        NONE = 1,
        /// <summary>
        /// top field only
        /// </summary>
        TOP = 2,
        /// <summary>
        /// bottom field only
        /// </summary>
        BOTTOM = 3,
        /// <summary>
        /// both fields interlaced
        /// </summary>
        INTERLACED = 4,
        /// <summary>
        /// both fields sequential into one buffer, top-bottom order
        /// </summary>
        SEQ_TB = 5,
        /// <summary>
        /// same as above + bottom-top order
        /// </summary>
        SEQ_BT = 6,
        /// <summary>
        /// both fields alternating into separate buffers
        /// </summary>
        ALTERNATE = 7,
        /// <summary>
        /// both fields interlaced, top field first and the top field is transmitted first
        /// </summary>
        INTERLACED_TB = 8,
        /// <summary>
        /// both fields interlaced, top field first and the bottom field is transmitted first
        /// </summary>
        INTERLACED_BT = 9,
    };

    public enum v4l2_buf_type : UInt32
    {
        VIDEO_CAPTURE = 1,
        VIDEO_OUTPUT = 2,
        VIDEO_OVERLAY = 3,
        VBI_CAPTURE = 4,
        VBI_OUTPUT = 5,
        SLICED_VBI_CAPTURE = 6,
        SLICED_VBI_OUTPUT = 7,
        VIDEO_OUTPUT_OVERLAY = 8,
        VIDEO_CAPTURE_MPLANE = 9,
        VIDEO_OUTPUT_MPLANE = 10,
        SDR_CAPTURE = 11,
        SDR_OUTPUT = 12,
        META_CAPTURE = 13,
        META_OUTPUT = 14,
        /// <summary>
        /// Deprecated, do not use
        /// </summary>
        PRIVATE = 0x80,
    };

    public enum v4l2_tuner_type : UInt32
    {
        RADIO = 1,
        ANALOG_TV = 2,
        DIGITAL_TV = 3,
        SDR = 4,
        RF = 5,
    };

    public enum v4l2_memory : UInt32
    {
        MMAP = 1,
        USERPTR = 2,
        OVERLAY = 3,
        DMABUF = 4,
    };

    public enum v4l2_colorspace : UInt32
    {
        /// <summary>
        /// Default colorspace, i.e. let the driver figure it out. Can only be used with video capture.
        /// </summary>
        DEFAULT = 0,
        /// <summary>
        /// SMPTE 170M: used for broadcast NTSC/PAL SDTV
        /// </summary>
        SMPTE170M = 1,
        /// <summary>
        /// Obsolete pre-1998 SMPTE 240M HDTV standard, superseded by Rec 709
        /// </summary>
        SMPTE240M = 2,
        /// <summary>
        /// Rec.709: used for HDTV
        /// </summary>
        REC709 = 3,
        /// <summary>
        /// Deprecated, do not use. No driver will ever return this. This was based on a misunderstanding of the bt878 datasheet.
        /// </summary>
        BT878 = 4,
        /// <summary>
        /// NTSC 1953 colorspace. This only makes sense when dealing with really, really old NTSC recordings. Superseded by SMPTE 170M.
        /// </summary>
        _470_SYSTEM_M = 5,
        /// <summary>
        /// EBU Tech 3213 PAL/SECAM colorspace.
        /// </summary>
        _470_SYSTEM_BG = 6,
        /// <summary>
        /// Effectively shorthand for SRGB, V4L2_YCBCR_ENC_601 and V4L2_QUANTIZATION_FULL_RANGE. To be used for (Motion-)JPEG.
        /// </summary>
        JPEG = 7,
        /// <summary>
        /// For RGB colorspaces such as produces by most webcams
        /// </summary>
        SRGB = 8,
        /// <summary>
        /// opRGB colorspace
        /// </summary>
        OPRGB = 9,
        /// <summary>
        /// BT.2020 colorspace, used for UHDTV.
        /// </summary>
        BT2020 = 10,
        /// <summary>
        /// Raw colorspace: for RAW unprocessed images
        /// </summary>
        RAW = 11,
        /// <summary>
        /// DCI-P3 colorspace, used by cinema projectors
        /// </summary>
        DCI_P3 = 12,
    };

    public enum v4l2_xfer_func : UInt32
    {
        DEFAULT = 0,
        _709 = 1,
        SRGB = 2,
        OPRGB = 3,
        SMPTE240M = 4,
        NONE = 5,
        DCI_P3 = 6,
        SMPTE2084 = 7,
    };

    public enum v4l2_ycbcr_encoding : UInt32
    {
        DEFAULT = 0,
        /// <summary>
        /// ITU-R 601 -- SDTV
        /// </summary>
        _601 = 1,
        /// <summary>
        /// Rec. 709 -- HDTV
        /// </summary>
        _709 = 2,
        /// <summary>
        /// ITU-R 601/EN 61966-2-4 Extended Gamut -- SDTV
        /// </summary>
        XV601 = 3,
        /// <summary>
        /// Rec. 709/EN 61966-2-4 Extended Gamut -- HDTV
        /// </summary>
        XV709 = 4,
        /// <summary>
        /// BT.2020 Non-constant Luminance Y'CbCr
        /// </summary>
        BT2020 = 6,
        /// <summary>
        /// BT.2020 Constant Luminance Y'CbcCrc
        /// </summary>
        BT2020_CONST_LUM = 7,
        /// <summary>
        /// SMPTE 240M -- Obsolete HDTV
        /// </summary>
        SMPTE240M = 8,
    };

    public enum v4l2_hsv_encoding : UInt32
    {
        /// <summary>
        /// Hue mapped to 0 - 179
        /// </summary>
        _180 = 128,
        /// <summary>
        /// Hue mapped to 0-255
        /// </summary>
        _256 = 129,
    };

    public enum v4l2_quantization : UInt32
    {
        DEFAULT = 0,
        FULL_RANGE = 1,
        LIM_RANGE = 2,
    };

    public enum v4l2_priority
    {
        /// <summary>
        /// not initialized
        /// </summary>
        UNSET = 0,
        BACKGROUND = 1,
        INTERACTIVE = 2,
        RECORD = 3,
        DEFAULT = INTERACTIVE,
    };

    public enum v4l2_frmsizetypes : UInt32
    {
        DISCRETE = 1,
        CONTINUOUS = 2,
        STEPWISE = 3,
    };

    public enum v4l2_frmivaltypes
    {
        DISCRETE = 1,
        CONTINUOUS = 2,
        STEPWISE = 3,
    };

    public enum v4l2_ctrl_type : UInt32
    {
        INTEGER = 1,
        BOOLEAN = 2,
        MENU = 3,
        BUTTON = 4,
        INTEGER64 = 5,
        CTRL_CLASS = 6,
        STRING = 7,
        BITMASK = 8,
        INTEGER_MENU = 9,

        /* Compound types are >= 0x0100 */
        COMPOUND_TYPES = 0x0100,
        U8 = 0x0100,
        U16 = 0x0101,
        U32 = 0x0102,
        AREA = 0x0106,

        HDR10_CLL_INFO = 0x0110,
        HDR10_MASTERING_DISPLAY = 0x0111,

        H264_SPS = 0x0200,
        H264_PPS = 0x0201,
        H264_SCALING_MATRIX = 0x0202,
        H264_SLICE_PARAMS = 0x0203,
        H264_DECODE_PARAMS = 0x0204,
        H264_PRED_WEIGHTS = 0x0205,

        FWHT_PARAMS = 0x0220,

        VP8_FRAME = 0x0240,

        MPEG2_QUANTISATION = 0x0250,
        MPEG2_SEQUENCE = 0x0251,
        MPEG2_PICTURE = 0x0252,

        VP9_COMPRESSED_HDR = 0x0260,
        VP9_FRAME = 0x0261,

        HEVC_SPS = 0x0270,
        HEVC_PPS = 0x0271,
        HEVC_SLICE_PARAMS = 0x0272,
        HEVC_SCALING_MATRIX = 0x0273,
        HEVC_DECODE_PARAMS = 0x0274,

        AV1_SEQUENCE = 0x280,
        AV1_TILE_GROUP_ENTRY = 0x281,
        AV1_FRAME = 0x282,
        AV1_FILM_GRAIN = 0x283,
    };
}
