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
        V4L2_FIELD_ANY = 0,
        /// <summary>
        /// this device has no fields
        /// </summary>
        V4L2_FIELD_NONE = 1,
        /// <summary>
        /// top field only
        /// </summary>
        V4L2_FIELD_TOP = 2,
        /// <summary>
        /// bottom field only
        /// </summary>
        V4L2_FIELD_BOTTOM = 3,
        /// <summary>
        /// both fields interlaced
        /// </summary>
        V4L2_FIELD_INTERLACED = 4,
        /// <summary>
        /// both fields sequential into one buffer, top-bottom order
        /// </summary>
        V4L2_FIELD_SEQ_TB = 5,
        /// <summary>
        /// same as above + bottom-top order
        /// </summary>
        V4L2_FIELD_SEQ_BT = 6,
        /// <summary>
        /// both fields alternating into separate buffers
        /// </summary>
        V4L2_FIELD_ALTERNATE = 7,
        /// <summary>
        /// both fields interlaced, top field first and the top field is transmitted first
        /// </summary>
        V4L2_FIELD_INTERLACED_TB = 8,
        /// <summary>
        /// both fields interlaced, top field first and the bottom field is transmitted first
        /// </summary>
        V4L2_FIELD_INTERLACED_BT = 9,
    };

    public enum v4l2_buf_type : UInt32
    {
        V4L2_BUF_TYPE_VIDEO_CAPTURE = 1,
        V4L2_BUF_TYPE_VIDEO_OUTPUT = 2,
        V4L2_BUF_TYPE_VIDEO_OVERLAY = 3,
        V4L2_BUF_TYPE_VBI_CAPTURE = 4,
        V4L2_BUF_TYPE_VBI_OUTPUT = 5,
        V4L2_BUF_TYPE_SLICED_VBI_CAPTURE = 6,
        V4L2_BUF_TYPE_SLICED_VBI_OUTPUT = 7,
        V4L2_BUF_TYPE_VIDEO_OUTPUT_OVERLAY = 8,
        V4L2_BUF_TYPE_VIDEO_CAPTURE_MPLANE = 9,
        V4L2_BUF_TYPE_VIDEO_OUTPUT_MPLANE = 10,
        V4L2_BUF_TYPE_SDR_CAPTURE = 11,
        V4L2_BUF_TYPE_SDR_OUTPUT = 12,
        V4L2_BUF_TYPE_META_CAPTURE = 13,
        V4L2_BUF_TYPE_META_OUTPUT = 14,
        /// <summary>
        /// Deprecated, do not use
        /// </summary>
        V4L2_BUF_TYPE_PRIVATE = 0x80,
    };

    public enum v4l2_tuner_type : UInt32
    {
        V4L2_TUNER_RADIO = 1,
        V4L2_TUNER_ANALOG_TV = 2,
        V4L2_TUNER_DIGITAL_TV = 3,
        V4L2_TUNER_SDR = 4,
        V4L2_TUNER_RF = 5,
    };

    public enum v4l2_memory : UInt32
    {
        V4L2_MEMORY_MMAP = 1,
        V4L2_MEMORY_USERPTR = 2,
        V4L2_MEMORY_OVERLAY = 3,
        V4L2_MEMORY_DMABUF = 4,
    };

    public enum v4l2_colorspace : UInt32
    {
        /// <summary>
        /// Default colorspace, i.e. let the driver figure it out. Can only be used with video capture.
        /// </summary>
        V4L2_COLORSPACE_DEFAULT = 0,
        /// <summary>
        /// SMPTE 170M: used for broadcast NTSC/PAL SDTV
        /// </summary>
        V4L2_COLORSPACE_SMPTE170M = 1,
        /// <summary>
        /// Obsolete pre-1998 SMPTE 240M HDTV standard, superseded by Rec 709
        /// </summary>
        V4L2_COLORSPACE_SMPTE240M = 2,
        /// <summary>
        /// Rec.709: used for HDTV
        /// </summary>
        V4L2_COLORSPACE_REC709 = 3,
        /// <summary>
        /// Deprecated, do not use. No driver will ever return this. This was based on a misunderstanding of the bt878 datasheet.
        /// </summary>
        V4L2_COLORSPACE_BT878 = 4,
        /// <summary>
        /// NTSC 1953 colorspace. This only makes sense when dealing with really, really old NTSC recordings. Superseded by SMPTE 170M.
        /// </summary>
        V4L2_COLORSPACE_470_SYSTEM_M = 5,
        /// <summary>
        /// EBU Tech 3213 PAL/SECAM colorspace.
        /// </summary>
        V4L2_COLORSPACE_470_SYSTEM_BG = 6,
        /// <summary>
        /// Effectively shorthand for V4L2_COLORSPACE_SRGB, V4L2_YCBCR_ENC_601 and V4L2_QUANTIZATION_FULL_RANGE. To be used for (Motion-)JPEG.
        /// </summary>
        V4L2_COLORSPACE_JPEG = 7,
        /// <summary>
        /// For RGB colorspaces such as produces by most webcams
        /// </summary>
        V4L2_COLORSPACE_SRGB = 8,
        /// <summary>
        /// opRGB colorspace
        /// </summary>
        V4L2_COLORSPACE_OPRGB = 9,
        /// <summary>
        /// BT.2020 colorspace, used for UHDTV.
        /// </summary>
        V4L2_COLORSPACE_BT2020 = 10,
        /// <summary>
        /// Raw colorspace: for RAW unprocessed images
        /// </summary>
        V4L2_COLORSPACE_RAW = 11,
        /// <summary>
        /// DCI-P3 colorspace, used by cinema projectors
        /// </summary>
        V4L2_COLORSPACE_DCI_P3 = 12,
    };

    public enum v4l2_xfer_func : UInt32
    {
        V4L2_XFER_FUNC_DEFAULT = 0,
        V4L2_XFER_FUNC_709 = 1,
        V4L2_XFER_FUNC_SRGB = 2,
        V4L2_XFER_FUNC_OPRGB = 3,
        V4L2_XFER_FUNC_SMPTE240M = 4,
        V4L2_XFER_FUNC_NONE = 5,
        V4L2_XFER_FUNC_DCI_P3 = 6,
        V4L2_XFER_FUNC_SMPTE2084 = 7,
    };

    public enum v4l2_ycbcr_encoding : UInt32
    {
        V4L2_YCBCR_ENC_DEFAULT = 0,
        /// <summary>
        /// ITU-R 601 -- SDTV
        /// </summary>
        V4L2_YCBCR_ENC_601 = 1,
        /// <summary>
        /// Rec. 709 -- HDTV
        /// </summary>
        V4L2_YCBCR_ENC_709 = 2,
        /// <summary>
        /// ITU-R 601/EN 61966-2-4 Extended Gamut -- SDTV
        /// </summary>
        V4L2_YCBCR_ENC_XV601 = 3,
        /// <summary>
        /// Rec. 709/EN 61966-2-4 Extended Gamut -- HDTV
        /// </summary>
        V4L2_YCBCR_ENC_XV709 = 4,
        /// <summary>
        /// BT.2020 Non-constant Luminance Y'CbCr
        /// </summary>
        V4L2_YCBCR_ENC_BT2020 = 6,
        /// <summary>
        /// BT.2020 Constant Luminance Y'CbcCrc
        /// </summary>
        V4L2_YCBCR_ENC_BT2020_CONST_LUM = 7,
        /// <summary>
        /// SMPTE 240M -- Obsolete HDTV
        /// </summary>
        V4L2_YCBCR_ENC_SMPTE240M = 8,
    };

    public enum v4l2_hsv_encoding : UInt32
    {
        /// <summary>
        /// Hue mapped to 0 - 179
        /// </summary>
        V4L2_HSV_ENC_180 = 128,
        /// <summary>
        /// Hue mapped to 0-255
        /// </summary>
        V4L2_HSV_ENC_256 = 129,
    };

    public enum v4l2_quantization : UInt32
    {
        V4L2_QUANTIZATION_DEFAULT = 0,
        V4L2_QUANTIZATION_FULL_RANGE = 1,
        V4L2_QUANTIZATION_LIM_RANGE = 2,
    };

    public enum v4l2_priority
    {
        /// <summary>
        /// not initialized
        /// </summary>
        V4L2_PRIORITY_UNSET = 0,
        V4L2_PRIORITY_BACKGROUND = 1,
        V4L2_PRIORITY_INTERACTIVE = 2,
        V4L2_PRIORITY_RECORD = 3,
        V4L2_PRIORITY_DEFAULT = V4L2_PRIORITY_INTERACTIVE,
    };

    public enum v4l2_frmsizetypes : UInt32
    {
        V4L2_FRMSIZE_TYPE_DISCRETE = 1,
        V4L2_FRMSIZE_TYPE_CONTINUOUS = 2,
        V4L2_FRMSIZE_TYPE_STEPWISE = 3,
    };

    public enum v4l2_frmivaltypes
    {
        V4L2_FRMIVAL_TYPE_DISCRETE = 1,
        V4L2_FRMIVAL_TYPE_CONTINUOUS = 2,
        V4L2_FRMIVAL_TYPE_STEPWISE = 3,
    };

    public enum v4l2_ctrl_type : UInt32
    {
        V4L2_CTRL_TYPE_INTEGER = 1,
        V4L2_CTRL_TYPE_BOOLEAN = 2,
        V4L2_CTRL_TYPE_MENU = 3,
        V4L2_CTRL_TYPE_BUTTON = 4,
        V4L2_CTRL_TYPE_INTEGER64 = 5,
        V4L2_CTRL_TYPE_CTRL_CLASS = 6,
        V4L2_CTRL_TYPE_STRING = 7,
        V4L2_CTRL_TYPE_BITMASK = 8,
        V4L2_CTRL_TYPE_INTEGER_MENU = 9,

        /* Compound types are >= 0x0100 */
        V4L2_CTRL_COMPOUND_TYPES = 0x0100,
        V4L2_CTRL_TYPE_U8 = 0x0100,
        V4L2_CTRL_TYPE_U16 = 0x0101,
        V4L2_CTRL_TYPE_U32 = 0x0102,
        V4L2_CTRL_TYPE_AREA = 0x0106,

        V4L2_CTRL_TYPE_HDR10_CLL_INFO = 0x0110,
        V4L2_CTRL_TYPE_HDR10_MASTERING_DISPLAY = 0x0111,

        V4L2_CTRL_TYPE_H264_SPS = 0x0200,
        V4L2_CTRL_TYPE_H264_PPS = 0x0201,
        V4L2_CTRL_TYPE_H264_SCALING_MATRIX = 0x0202,
        V4L2_CTRL_TYPE_H264_SLICE_PARAMS = 0x0203,
        V4L2_CTRL_TYPE_H264_DECODE_PARAMS = 0x0204,
        V4L2_CTRL_TYPE_H264_PRED_WEIGHTS = 0x0205,

        V4L2_CTRL_TYPE_FWHT_PARAMS = 0x0220,

        V4L2_CTRL_TYPE_VP8_FRAME = 0x0240,

        V4L2_CTRL_TYPE_MPEG2_QUANTISATION = 0x0250,
        V4L2_CTRL_TYPE_MPEG2_SEQUENCE = 0x0251,
        V4L2_CTRL_TYPE_MPEG2_PICTURE = 0x0252,

        V4L2_CTRL_TYPE_VP9_COMPRESSED_HDR = 0x0260,
        V4L2_CTRL_TYPE_VP9_FRAME = 0x0261,

        V4L2_CTRL_TYPE_HEVC_SPS = 0x0270,
        V4L2_CTRL_TYPE_HEVC_PPS = 0x0271,
        V4L2_CTRL_TYPE_HEVC_SLICE_PARAMS = 0x0272,
        V4L2_CTRL_TYPE_HEVC_SCALING_MATRIX = 0x0273,
        V4L2_CTRL_TYPE_HEVC_DECODE_PARAMS = 0x0274,

        V4L2_CTRL_TYPE_AV1_SEQUENCE = 0x280,
        V4L2_CTRL_TYPE_AV1_TILE_GROUP_ENTRY = 0x281,
        V4L2_CTRL_TYPE_AV1_FRAME = 0x282,
        V4L2_CTRL_TYPE_AV1_FILM_GRAIN = 0x283,
    };
}
