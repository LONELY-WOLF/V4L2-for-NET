using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace V4L2_for_NET
{
    public enum VIDEO_MAX
    {
        FRAME = 32,
        PLANES = 8,
    }

    public enum V4L2_CAP : UInt32
    {
        /// <summary>
        /// Is a video capture device
        /// </summary>
        VIDEO_CAPTURE = 0x00000001,
        /// <summary>
        /// Is a video output device
        /// </summary>
        VIDEO_OUTPUT = 0x00000002,
        /// <summary>
        /// Can do video overlay
        /// </summary>
        VIDEO_OVERLAY = 0x00000004,
        /// <summary>
        /// Is a raw VBI capture device
        /// </summary>
        VBI_CAPTURE = 0x00000010,
        /// <summary>
        /// Is a raw VBI output device
        /// </summary>
        VBI_OUTPUT = 0x00000020,
        /// <summary>
        /// Is a sliced VBI capture device
        /// </summary>
        SLICED_VBI_CAPTURE = 0x00000040,
        /// <summary>
        /// Is a sliced VBI output device
        /// </summary>
        SLICED_VBI_OUTPUT = 0x00000080,
        /// <summary>
        /// RDS data capture
        /// </summary>
        RDS_CAPTURE = 0x00000100,
        /// <summary>
        /// Can do video output overlay
        /// </summary>
        VIDEO_OUTPUT_OVERLAY = 0x00000200,
        /// <summary>
        /// Can do hardware frequency seek
        /// </summary>
        HW_FREQ_SEEK = 0x00000400,
        /// <summary>
        /// Is an RDS encoder
        /// </summary>
        RDS_OUTPUT = 0x00000800,
        /// <summary>
        /// Is a video capture device that supports multiplanar formats
        /// </summary>
        VIDEO_CAPTURE_MPLANE = 0x00001000,
        /// <summary>
        /// Is a video output device that supports multiplanar formats
        /// </summary>
        VIDEO_OUTPUT_MPLANE = 0x00002000,
        /// <summary>
        /// Is a video mem-to-mem device that supports multiplanar formats
        /// </summary>
        VIDEO_M2M_MPLANE = 0x00004000,
        /// <summary>
        /// Is a video mem-to-mem device
        /// </summary>
        VIDEO_M2M = 0x00008000,
        /// <summary>
        /// has a tuner
        /// </summary>
        TUNER = 0x00010000,
        /// <summary>
        /// has audio support
        /// </summary>
        AUDIO = 0x00020000,
        /// <summary>
        /// is a radio device
        /// </summary>
        RADIO = 0x00040000,
        /// <summary>
        /// has a modulator
        /// </summary>
        MODULATOR = 0x00080000,
        /// <summary>
        /// Is a SDR capture device
        /// </summary>
        SDR_CAPTURE = 0x00100000,
        /// <summary>
        /// Supports the extended pixel format
        /// </summary>
        EXT_PIX_FORMAT = 0x00200000,
        /// <summary>
        /// Is a SDR output device
        /// </summary>
        SDR_OUTPUT = 0x00400000,
        /// <summary>
        /// Is a metadata capture device
        /// </summary>
        META_CAPTURE = 0x00800000,
        /// <summary>
        /// read/write systemcalls
        /// </summary>
        READWRITE = 0x01000000,
        /// <summary>
        /// streaming I/O ioctls
        /// </summary>
        STREAMING = 0x04000000,
        /// <summary>
        /// Is a metadata output device
        /// </summary>
        META_OUTPUT = 0x08000000,
        /// <summary>
        /// Is a touch device
        /// </summary>
        TOUCH = 0x10000000,
        /// <summary>
        /// Is input/output controlled by the media controller
        /// </summary>
        IO_MC = 0x20000000,
        /// <summary>
        /// sets device capabilities field
        /// </summary>
        DEVICE_CAPS = 0x80000000,
    }

    public enum V4L2_FMT_FLAG
    {
        COMPRESSED = 0x0001,
        EMULATED = 0x0002,
        CONTINUOUS_BYTESTREAM = 0x0004,
        DYN_RESOLUTION = 0x0008,
        ENC_CAP_FRAME_INTERVAL = 0x0010,
        CSC_COLORSPACE = 0x0020,
        CSC_XFER_FUNC = 0x0040,
        CSC_YCBCR_ENC = 0x0080,
        CSC_HSV_ENC = CSC_YCBCR_ENC,
        CSC_QUANTIZATION = 0x0100,
    }

    public enum V4L2_TC_TYPE
    {
        _24FPS = 1,
        _25FPS = 2,
        _30FPS = 3,
        _50FPS = 4,
        _60FPS = 5,
    }

    public enum V4L2_TC_FLAG
    {
        /// <summary>
        /// "drop-frame" mode
        /// </summary>
        DROPFRAME = 0x0001,
        COLORFRAME = 0x0002,
        USERBITS_field = 0x000C,
        USERBITS_USERDEFINED = 0x0000,
        USERBITS_8BITCHARS = 0x0008,
    }

    public enum V4L2_JPEG_MARKER
    {
        /// <summary>
        /// Define Huffman Tables
        /// </summary>
        DHT = (1 << 3),
        /// <summary>
        /// Define Quantization Tables
        /// </summary>
        DQT = (1 << 4),
        /// <summary>
        /// Define Restart Interval
        /// </summary>
        DRI = (1 << 5),
        /// <summary>
        /// Comment segment
        /// </summary>
        COM = (1 << 6),
        /// <summary>
        /// App segment, driver will always use APP0
        /// </summary>
        APP = (1 << 7),
    }

    public enum V4L2_BUF_CAP_SUPPORTS
    {
        MMAP = (1 << 0),
        USERPTR = (1 << 1),
        DMABUF = (1 << 2),
        REQUESTS = (1 << 3),
        ORPHANED_BUFS = (1 << 4),
        M2M_HOLD_CAPTURE_BUF = (1 << 5),
        MMAP_CACHE_HINTS = (1 << 6),
    }

    public enum V4L2_BUF_FLAG
    {
        /// <summary>
        /// Buffer is mapped (flag)
        /// </summary>
        MAPPED = 0x00000001,
        /// <summary>
        /// Buffer is queued for processing
        /// </summary>
        QUEUED = 0x00000002,
        /// <summary>
        /// Buffer is ready
        /// </summary>
        DONE = 0x00000004,
        /// <summary>
        /// Image is a keyframe (I-frame)
        /// </summary>
        KEYFRAME = 0x00000008,
        /// <summary>
        /// Image is a P-frame
        /// </summary>
        PFRAME = 0x00000010,
        /// <summary>
        /// Image is a B-frame
        /// </summary>
        BFRAME = 0x00000020,
        /// <summary>
        /// Buffer is ready, but the data contained within is corrupted.
        /// </summary>
        ERROR = 0x00000040,
        /// <summary>
        /// Buffer is added to an unqueued request
        /// </summary>
        IN_REQUEST = 0x00000080,
        /// <summary>
        /// timecode field is valid
        /// </summary>
        TIMECODE = 0x00000100,
        /// <summary>
        /// Don't return the capture buffer until OUTPUT timestamp changes
        /// </summary>
        M2M_HOLD_CAPTURE_BUF = 0x00000200,

        /// <summary>
        /// Buffer is prepared for queuing
        /// </summary>
        PREPARED = 0x00000400,
        NO_CACHE_INVALIDATE = 0x00000800,
        NO_CACHE_CLEAN = 0x00001000,
        TIMESTAMP_MASK = 0x0000e000,
        TIMESTAMP_UNKNOWN = 0x00000000,
        TIMESTAMP_MONOTONIC = 0x00002000,
        TIMESTAMP_COPY = 0x00004000,
        TSTAMP_SRC_MASK = 0x00070000,
        TSTAMP_SRC_EOF = 0x00000000,
        TSTAMP_SRC_SOE = 0x00010000,
        LAST = 0x00100000,
        /// <summary>
        /// request_fd is valid
        /// </summary>
        REQUEST_FD = 0x00800000,
    }

    public enum V4L2_FBUF_CAP
    {
        EXTERNOVERLAY = 0x0001,
        CHROMAKEY = 0x0002,
        LIST_CLIPPING = 0x0004,
        BITMAP_CLIPPING = 0x0008,
        LOCAL_ALPHA = 0x0010,
        GLOBAL_ALPHA = 0x0020,
        LOCAL_INV_ALPHA = 0x0040,
        SRC_CHROMAKEY = 0x0080,
    }

    public enum V4L2_FBUF_FLAG
    {
        PRIMARY = 0x0001,
        OVERLAY = 0x0002,
        CHROMAKEY = 0x0004,
        LOCAL_ALPHA = 0x0008,
        GLOBAL_ALPHA = 0x0010,
        LOCAL_INV_ALPHA = 0x0020,
        SRC_CHROMAKEY = 0x0040,
    }

    public enum V4L2_STD : UInt64
    {
        PAL_B = 0x00000001,
        PAL_B1 = 0x00000002,
        PAL_G = 0x00000004,
        PAL_H = 0x00000008,
        PAL_I = 0x00000010,
        PAL_D = 0x00000020,
        PAL_D1 = 0x00000040,
        PAL_K = 0x00000080,

        PAL_M = 0x00000100,
        PAL_N = 0x00000200,
        PAL_Nc = 0x00000400,
        PAL_60 = 0x00000800,

        /// <summary>
        /// BTSC
        /// </summary>
        NTSC_M = 0x00001000,
        /// <summary>
        /// EIA-J
        /// </summary>
        NTSC_M_JP = 0x00002000,
        NTSC_443 = 0x00004000,
        /// <summary>
        /// FM A2
        /// </summary>
        NTSC_M_KR = 0x00008000,

        SECAM_B = 0x00010000,
        SECAM_D = 0x00020000,
        SECAM_G = 0x00040000,
        SECAM_H = 0x00080000,
        SECAM_K = 0x00100000,
        SECAM_K1 = 0x00200000,
        SECAM_L = 0x00400000,
        SECAM_LC = 0x00800000,

        ATSC_8_VSB = 0x01000000,
        ATSC_16_VSB = 0x02000000,
    }

    /// <summary>
    /// Values for the 'type' field
    /// </summary>
    public enum V4L2_INPUT_TYPE
    {
        TUNER = 1,
        CAMERA = 2,
        TOUCH = 3,
    }

    public enum V4L2_IN_ST
    {
        /// <summary>
        /// Attached device is off
        /// </summary>
        NO_POWER = 0x00000001,
        NO_SIGNAL = 0x00000002,
        NO_COLOR = 0x00000004,

        /// <summary>
        /// Frames are flipped horizontally
        /// </summary>
        HFLIP = 0x00000010,
        /// <summary>
        /// Frames are flipped vertically
        /// </summary>
        VFLIP = 0x00000020,

        /// <summary>
        /// No horizontal sync lock
        /// </summary>
        NO_H_LOCK = 0x00000100,
        /// <summary>
        /// Color killer is active
        /// </summary>
        COLOR_KILL = 0x00000200,
        /// <summary>
        /// No vertical sync lock
        /// </summary>
        NO_V_LOCK = 0x00000400,
        /// <summary>
        /// No standard format lock
        /// </summary>
        NO_STD_LOCK = 0x00000800,

        /// <summary>
        /// No synchronization lock
        /// </summary>
        NO_SYNC = 0x00010000,
        /// <summary>
        /// No equalizer lock
        /// </summary>
        NO_EQU = 0x00020000,
        /// <summary>
        /// Carrier recovery failed
        /// </summary>
        NO_CARRIER = 0x00040000,

        /// <summary>
        /// Macrovision detected
        /// </summary>
        MACROVISION = 0x01000000,
        /// <summary>
        /// Conditional access denied
        /// </summary>
        NO_ACCESS = 0x02000000,
        /// <summary>
        /// VTR time constant
        /// </summary>
        VTR = 0x04000000,
    }

    public enum V4L2_IN_CAP
    {
        /// <summary>
        /// Supports S_DV_TIMINGS
        /// </summary>
        DV_TIMINGS = 0x00000002,
        /// <summary>
        /// For compatibility
        /// </summary>
        CUSTOM_TIMINGS = DV_TIMINGS,
        /// <summary>
        /// Supports S_STD
        /// </summary>
        STD = 0x00000004,
        /// <summary>
        /// Supports setting native size
        /// </summary>
        NATIVE_SIZE = 0x00000008,
    }

    public enum V4L2_OUTPUT_TYPE
    {
        MODULATOR = 1,
        ANALOG = 2,
        ANALOGVGAOVERLAY = 3,
    }

    public enum V4L2_OUT_CAP
    {
        /// <summary>
        /// Supports S_DV_TIMINGS
        /// </summary>
        DV_TIMINGS = 0x00000002,
        /// <summary>
        /// For compatibility
        /// </summary>
        CUSTOM_TIMINGS = DV_TIMINGS,
        /// <summary>
        /// Supports S_STD
        /// </summary>
        STD = 0x00000004,
        /// <summary>
        /// Supports setting native size
        /// </summary>
        NATIVE_SIZE = 0x00000008,
    }

    public enum V4L2_CTRL
    {
        ID_MASK = 0x0fffffff,
        MAX_DIMS = 4,
        WHICH_CUR_VAL = 0,
        WHICH_DEF_VAL = 0x0f000000,
        WHICH_REQUEST_VAL = 0x0f010000,
    }

    public enum V4L2_CTRL_FLAG : UInt32
    {
        DISABLED = 0x0001,
        GRABBED = 0x0002,
        READ_ONLY = 0x0004,
        UPDATE = 0x0008,
        INACTIVE = 0x0010,
        SLIDER = 0x0020,
        WRITE_ONLY = 0x0040,
        VOLATILE = 0x0080,
        HAS_PAYLOAD = 0x0100,
        EXECUTE_ON_WRITE = 0x0200,
        MODIFY_LAYOUT = 0x0400,
        DYNAMIC_ARRAY = 0x0800,

        /*  Query flags, to be ORed with the control ID */
        NEXT_CTRL = 0x80000000,
        NEXT_COMPOUND = 0x40000000,
    }

    public enum V4L2_TUNER_CAP
    {
        LOW = 0x0001,
        NORM = 0x0002,
        HWSEEK_BOUNDED = 0x0004,
        HWSEEK_WRAP = 0x0008,
        STEREO = 0x0010,
        LANG2 = 0x0020,
        SAP = 0x0020,
        LANG1 = 0x0040,
        RDS = 0x0080,
        RDS_BLOCK_IO = 0x0100,
        RDS_CONTROLS = 0x0200,
        FREQ_BANDS = 0x0400,
        HWSEEK_PROG_LIM = 0x0800,
        _1HZ = 0x1000,
    }

    public enum V4L2_TUNER_SUB
    {
        MONO = 0x0001,
        STEREO = 0x0002,
        LANG2 = 0x0004,
        SAP = 0x0004,
        LANG1 = 0x0008,
        RDS = 0x0010,
    }

    public enum V4L2_TUNER_MODE
    {
        MONO = 0x0000,
        STEREO = 0x0001,
        LANG2 = 0x0002,
        SAP = 0x0002,
        LANG1 = 0x0003,
        LANG1_LANG2 = 0x0004,
    }

    public enum V4L2_BAND_MODULATION
    {
        VSB = (1 << 1),
        FM = (1 << 2),
        AM = (1 << 3),
    }

    public enum V4L2_RDS_BLOCK
    {
        MSK = 0x7,
        BLOCK_A = 0,
        BLOCK_B = 1,
        BLOCK_C = 2,
        BLOCK_D = 3,
        BLOCK_C_ALT = 4,
        BLOCK_INVALID = 7,

        BLOCK_CORRECTED = 0x40,
        BLOCK_ERROR = 0x80,
    }

    public enum V4L2_AUDCAP
    {
        STEREO = 0x00001,
        AVL = 0x00002,
    }

    public enum V4L2_AUDMODE
    {
        AVL = 0x00001,
    }

    public enum V4L2_ENC_IDX_FRAME
    {
        I = (0),
        P = (1),
        B = (2),
        MASK = (0xf),
    }

    public enum V4L2_ENC_IDX
    {
        ENTRIES = 64,
    }

    public enum V4L2_ENC_CMD
    {
        START = 0,
        STOP = 1,
        PAUSE = 2,
        RESUME = 3,
        STOP_AT_GOP_END = (1 << 0),
    }

    public enum V4L2_DEC_CMD
    {
        START = (0),
        STOP = (1),
        PAUSE = (2),
        RESUME = (3),
        FLUSH = (4),
        START_MUTE_AUDIO = (1 << 0),
        PAUSE_TO_BLACK = (1 << 0),
        STOP_TO_BLACK = (1 << 0),
        STOP_IMMEDIATELY = (1 << 1),
    }

    public enum V4L2_DEC_START_FMT
    {
        NONE = (0),
        /// <summary>
        /// The decoder requires full GOPs
        /// </summary>
        GOP = (1),
    }

    public enum V4L2_VBI
    {
        UNSYNC = (1 << 0),
        INTERLACED = (1 << 1),
    }

    public enum V4L2_VBI_ITU
    {
        _525_F1_START = 1,
        _525_F2_START = 264,
        _625_F1_START = 1,
        _625_F2_START = 314,
    }

    public enum V4L2_SLICED
    {
        /// <summary>
        /// Teletext World System Teletext (WST), defined on ITU-R BT.653-2
        /// </summary>
        TELETEXT_B = (0x0001),
        /// <summary>
        /// Video Program System, defined on ETS 300 231
        /// </summary>
        VPS = (0x0400),
        /// <summary>
        /// Closed Caption, defined on EIA-608
        /// </summary>
        CAPTION_525 = (0x1000),
        /// <summary>
        /// Wide Screen System, defined on ITU-R BT1119.1
        /// </summary>
        WSS_625 = (0x4000),
    }

    public enum V4L2_MPEG_VBI_IVTV
    {
        TELETEXT_B = 1,
        CAPTION_525 = 4,
        WSS_625 = 5,
        VPS = 7,
    }

    public enum V4L2_EVENT
    {
        ALL = 0,
        VSYNC = 1,
        EOS = 2,
        CTRL = 3,
        FRAME_SYNC = 4,
        SOURCE_CHANGE = 5,
        MOTION_DET = 6,
        PRIVATE_START = 0x08000000,
    }

    public enum V4L2_EVENT_CTRL_CH
    {
        VALUE = (1 << 0),
        FLAGS = (1 << 1),
        RANGE = (1 << 2),
        DIMENSIONS = (1 << 3),
    }

    public enum V4L2_EVENT_SRC_CH
    {
        RESOLUTION = (1 << 0),
    }

    public enum V4L2_EVENT_MD_FL
    {
        HAVE_FRAME_SEQ = (1 << 0),
    }
}