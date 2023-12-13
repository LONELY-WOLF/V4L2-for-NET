using System.Runtime.InteropServices;

namespace V4L2_for_NET
{
    public static class VIDIOC
    {
        //#define VIDIOC_QUERYCAP                 _IOR('V',  0, struct v4l2_capability)
        public static int QUERYCAP(int fd, v4l2_capability cap)
        {
            return DoIoctl(fd, 0, IoctlAccess.Read, cap);
        }

        //#define VIDIOC_RESERVED                  _IO('V',  1)

        //#define VIDIOC_ENUM_FMT         _IOWR('V',  2, struct v4l2_fmtdesc)
        public static int ENUM_FMT(int fd, v4l2_fmtdesc fmt)
        {
            return DoIoctl(fd, 2, IoctlAccess.RW, fmt);
        }

        //#define VIDIOC_G_FMT                _IOWR('V',  4, struct v4l2_format)
        public static int G_FMT(int fd, v4l2_format fmt)
        {
            return DoIoctl(fd, 4, IoctlAccess.RW, fmt);
        }

        //#define VIDIOC_S_FMT                _IOWR('V',  5, struct v4l2_format)
        public static int S_FMT(int fd, v4l2_format fmt)
        {
            return DoIoctl(fd, 5, IoctlAccess.RW, fmt);
        }

        //#define VIDIOC_REQBUFS                _IOWR('V',  8, struct v4l2_requestbuffers)
        public static int REQBUFS(int fd, v4l2_requestbuffers bufs)
        {
            return DoIoctl(fd, 8, IoctlAccess.RW, bufs);
        }

        //#define VIDIOC_QUERYBUF                _IOWR('V',  9, struct v4l2_buffer)
        public static int QUERYBUF(int fd, v4l2_buffer buf)
        {
            return DoIoctl(fd, 9, IoctlAccess.RW, buf);
        }

        //#define VIDIOC_G_FBUF                 _IOR('V', 10, struct v4l2_framebuffer)
        //#define VIDIOC_S_FBUF                 _IOW('V', 11, struct v4l2_framebuffer)
        //#define VIDIOC_OVERLAY                 _IOW('V', 14, int)

        //#define VIDIOC_QBUF                _IOWR('V', 15, struct v4l2_buffer)
        public static int QBUF(int fd, v4l2_buffer buf)
        {
            return DoIoctl(fd, 15, IoctlAccess.RW, buf);
        }

        //#define VIDIOC_EXPBUF		_IOWR('V', 16, struct v4l2_exportbuffer)
        public static int EXPBUF(int fd, v4l2_exportbuffer exportbuffer)
        {
            return DoIoctl(fd, 16, IoctlAccess.RW, exportbuffer);
        }

        //#define VIDIOC_DQBUF                _IOWR('V', 17, struct v4l2_buffer)
        public static int DQBUF(int fd, v4l2_buffer buf)
        {
            return DoIoctl(fd, 17, IoctlAccess.RW, buf);
        }

        //#define VIDIOC_STREAMON                 _IOW('V', 18, int)
        public static unsafe int STREAMON(int fd, v4l2_buf_type type)
        {
            return DoIoctl(fd, 18, IoctlAccess.Write, new IntPtr(&type), 4);
        }

        //#define VIDIOC_STREAMOFF         _IOW('V', 19, int)
        public static unsafe int STREAMOFF(int fd, v4l2_buf_type type)
        {
            return DoIoctl(fd, 19, IoctlAccess.Write, new IntPtr(&type), 4);
        }

        //#define VIDIOC_G_PARM                _IOWR('V', 21, struct v4l2_streamparm)
        //#define VIDIOC_S_PARM                _IOWR('V', 22, struct v4l2_streamparm)
        //#define VIDIOC_G_STD                 _IOR('V', 23, v4l2_std_id)
        //#define VIDIOC_S_STD                 _IOW('V', 24, v4l2_std_id)
        //#define VIDIOC_ENUMSTD                _IOWR('V', 25, struct v4l2_standard)
        //#define VIDIOC_ENUMINPUT        _IOWR('V', 26, struct v4l2_input)
        //#define VIDIOC_G_CTRL                _IOWR('V', 27, struct v4l2_control)
        //#define VIDIOC_S_CTRL                _IOWR('V', 28, struct v4l2_control)
        //#define VIDIOC_G_TUNER                _IOWR('V', 29, struct v4l2_tuner)
        //#define VIDIOC_S_TUNER                 _IOW('V', 30, struct v4l2_tuner)
        //#define VIDIOC_G_AUDIO                 _IOR('V', 33, struct v4l2_audio)
        //#define VIDIOC_S_AUDIO                 _IOW('V', 34, struct v4l2_audio)
        //#define VIDIOC_QUERYCTRL        _IOWR('V', 36, struct v4l2_queryctrl)
        //#define VIDIOC_QUERYMENU        _IOWR('V', 37, struct v4l2_querymenu)
        //#define VIDIOC_G_INPUT                 _IOR('V', 38, int)
        //#define VIDIOC_S_INPUT                _IOWR('V', 39, int)
        //#define VIDIOC_G_OUTPUT                 _IOR('V', 46, int)
        //#define VIDIOC_S_OUTPUT                _IOWR('V', 47, int)
        //#define VIDIOC_ENUMOUTPUT        _IOWR('V', 48, struct v4l2_output)
        //#define VIDIOC_G_AUDOUT                 _IOR('V', 49, struct v4l2_audioout)
        //#define VIDIOC_S_AUDOUT                 _IOW('V', 50, struct v4l2_audioout)
        //#define VIDIOC_G_MODULATOR        _IOWR('V', 54, struct v4l2_modulator)
        //#define VIDIOC_S_MODULATOR         _IOW('V', 55, struct v4l2_modulator)
        //#define VIDIOC_G_FREQUENCY        _IOWR('V', 56, struct v4l2_frequency)
        //#define VIDIOC_S_FREQUENCY         _IOW('V', 57, struct v4l2_frequency)
        //#define VIDIOC_CROPCAP                _IOWR('V', 58, struct v4l2_cropcap)
        //#define VIDIOC_G_CROP                _IOWR('V', 59, struct v4l2_crop)
        public static int G_CROP(int fd, v4l2_crop crop)
        {
            return DoIoctl(fd, 59, IoctlAccess.RW, crop);
        }
        //#define VIDIOC_S_CROP                 _IOW('V', 60, struct v4l2_crop)
        public static int S_CROP(int fd, v4l2_crop crop)
        {
            return DoIoctl(fd, 60, IoctlAccess.Write, crop);
        }
        //#define VIDIOC_G_JPEGCOMP         _IOR('V', 61, struct v4l2_jpegcompression)
        //#define VIDIOC_S_JPEGCOMP         _IOW('V', 62, struct v4l2_jpegcompression)
        //#define VIDIOC_QUERYSTD               _IOR('V', 63, v4l2_std_id)
        //#define VIDIOC_TRY_FMT              _IOWR('V', 64, struct v4l2_format)
        //#define VIDIOC_ENUMAUDIO        _IOWR('V', 65, struct v4l2_audio)
        //#define VIDIOC_ENUMAUDOUT        _IOWR('V', 66, struct v4l2_audioout)
        //#define VIDIOC_G_PRIORITY        _IOR('V', 67, enum v4l2_priority)
        //#define VIDIOC_S_PRIORITY        _IOW('V', 68, enum v4l2_priority)
        //#define VIDIOC_G_SLICED_VBI_CAP _IOWR('V', 69, struct v4l2_sliced_vbi_cap)
        //#define VIDIOC_LOG_STATUS         _IO('V', 70)
        //#define VIDIOC_G_EXT_CTRLS        _IOWR('V', 71, struct v4l2_ext_controls)
        //#define VIDIOC_S_EXT_CTRLS        _IOWR('V', 72, struct v4l2_ext_controls)
        //#define VIDIOC_TRY_EXT_CTRLS        _IOWR('V', 73, struct v4l2_ext_controls)
        //#if 1
        //#define VIDIOC_ENUM_FRAMESIZES        _IOWR('V', 74, struct v4l2_frmsizeenum)
        //#define VIDIOC_ENUM_FRAMEINTERVALS _IOWR('V', 75, struct v4l2_frmivalenum)
        //#define VIDIOC_G_ENC_INDEX       _IOR('V', 76, struct v4l2_enc_idx)
        //#define VIDIOC_ENCODER_CMD      _IOWR('V', 77, struct v4l2_encoder_cmd)
        //#define VIDIOC_TRY_ENCODER_CMD  _IOWR('V', 78, struct v4l2_encoder_cmd)
        //#endif

        //#if 1
        ///* Experimental, meant for debugging, testing and internal use.
        //   Only implemented if CONFIG_VIDEO_ADV_DEBUG is defined.
        //   You must be root to use these ioctls. Never use these in applications! */
        //#define VIDIOC_DBG_S_REGISTER          _IOW('V', 79, struct v4l2_dbg_register)
        //#define VIDIOC_DBG_G_REGISTER         _IOWR('V', 80, struct v4l2_dbg_register)

        ///* Experimental, meant for debugging, testing and internal use.
        //   Never use this ioctl in applications! */
        //#define VIDIOC_DBG_G_CHIP_IDENT _IOWR('V', 81, struct v4l2_dbg_chip_ident)
        //#endif

        //#define VIDIOC_S_HW_FREQ_SEEK         _IOW('V', 82, struct v4l2_hw_freq_seek)
        //        /* Reminder: when adding new ioctls please add support for them to
        //           drivers/media/video/v4l2-compat-ioctl32.c as well! */

        //# ifdef __OLD_VIDIOC_
        //        /* for compatibility, will go away some day */
        //#define VIDIOC_OVERLAY_OLD             _IOWR('V', 14, int)
        //#define VIDIOC_S_PARM_OLD               _IOW('V', 22, struct v4l2_streamparm)
        //#define VIDIOC_S_CTRL_OLD               _IOW('V', 28, struct v4l2_control)
        //#define VIDIOC_G_AUDIO_OLD             _IOWR('V', 33, struct v4l2_audio)
        //#define VIDIOC_G_AUDOUT_OLD            _IOWR('V', 49, struct v4l2_audioout)
        //#define VIDIOC_CROPCAP_OLD              _IOR('V', 58, struct v4l2_cropcap)
        //#endif

        //#define BASE_VIDIOC_PRIVATE        192                /* 192-255 are private */

        private static int DoIoctl(int fd, uint nr, IoctlAccess access, V4L2Struct arg)
        {
            int size = arg.GetSize();
            uint code = GetIoctlCode(nr, access, size);
            IntPtr ptr = arg.GetPointer();
            int ret = ioctl(fd, code, ptr);
            arg.UpdateFromUnmanaged();
            return ret;
        }

        private static int DoIoctl(int fd, uint nr, IoctlAccess access, IntPtr arg, int size)
        {
            uint code = GetIoctlCode(nr, access, size);
            int ret = ioctl(fd, code, arg);
            return ret;
        }

        enum IoctlAccess : UInt32
        {
            None = 0,
            Write = 1,
            Read = 2,
            RW = 3,
        }

        private static uint GetIoctlCode(uint number, IoctlAccess access, int size)
        {
            const uint t = 86; // 'V'
            uint code = 0;
            code |= ((UInt32)access) << (14 + 16);
            code |= ((uint)size & 0b0011_1111_1111_1111) << 16;
            code |= (t & 0xFF) << 8;
            code |= (number & 0xFF);
            return code;
        }

        [DllImport("libc", SetLastError = true)]
        internal static extern int ioctl(int fd, uint code, IntPtr argp);

        [DllImport("libc", SetLastError = true)]
        internal static extern int ioctl(int fd, int code, IntPtr argp);

        [DllImport("libc", SetLastError = true)]
        internal static extern int ioctl(int fd, uint code, ulong argp);
    }
}