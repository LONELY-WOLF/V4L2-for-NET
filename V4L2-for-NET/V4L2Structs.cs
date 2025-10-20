using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

[assembly: InternalsVisibleTo("Tests")]

namespace V4L2_for_NET
{
    public abstract class V4L2Struct
    {
        protected UnmanagedMemoryStream ms;
        protected BinaryReader br;
        protected BinaryWriter bw;
        protected IntPtr selfPtr;
        protected readonly bool isNested;

        //public abstract byte[] Buffer { get; set; }

        /// <summary>
        /// Allocates unmanaged memory for struct
        /// </summary>
        public unsafe V4L2Struct()
        {
            isNested = false;
            selfPtr = Marshal.AllocHGlobal(GetSize());
            ms = new UnmanagedMemoryStream((byte*)selfPtr.ToPointer(), GetSize(), GetSize(), FileAccess.ReadWrite);
            br = new BinaryReader(ms, Encoding.UTF8, true);
            bw = new BinaryWriter(ms, Encoding.UTF8, true);
        }

        /// <summary>
        /// Uses preallocated unmanaged memory from parrent structure
        /// </summary>
        /// <param name="ptr">Pointer to memory</param>
        public unsafe V4L2Struct(byte* ptr)
        {
            isNested = true;
            selfPtr = (IntPtr)ptr;
            ms = new UnmanagedMemoryStream(ptr, GetSize(), GetSize(), FileAccess.ReadWrite);
            br = new BinaryReader(ms, Encoding.UTF8, true);
            bw = new BinaryWriter(ms, Encoding.UTF8, true);
        }

        unsafe ~V4L2Struct()
        {
            if (!isNested)
            {
                if (selfPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(selfPtr);
                }
            }
        }

        public abstract void UpdateFromUnmanaged();
        public abstract IntPtr GetPointer();

        protected void Fill(int count)
        {
            const byte zero = 0;
            for (int i = 0; i < count; i++)
            {
                bw.Write(zero);
            }
        }

        protected static void WriteStringToBuffer(string s, byte[] b)
        {
            Array.Clear(b);
            Encoding.ASCII.GetBytes(s, 0, Math.Min(s.Length, b.Length), b, 0);
        }

        public const int NativeSize = 0;

        public abstract int GetSize();

        internal int GetStreamPosition()
        {
            return (int)ms.Position;
        }

        protected unsafe T ReadValue<T>() where T : struct
        {
            T value = Marshal.PtrToStructure<T>((nint)ms.PositionPointer);
            ms.PositionPointer += Marshal.SizeOf<T>();
            return value;
        }

        protected static unsafe T ReadValue<T>(nint ptr) where T : struct
        {
            T value = Marshal.PtrToStructure<T>(ptr);
            return value;
        }

        protected unsafe void WriteValue<T>(T value) where T : struct
        {
            Marshal.StructureToPtr(value, (nint)ms.PositionPointer, false);
			ms.PositionPointer += Marshal.SizeOf(value);
        }

        protected static unsafe void WriteValue<T>(T value, nint ptr) where T : struct
        {
            Marshal.StructureToPtr(value, ptr, false);
        }
    }

    public struct v4l2_rect
    {
        public Int32 left;
        public Int32 top;
        public UInt32 width;
        public UInt32 height;
    }

    public struct v4l2_fract
    {
        public UInt32 numerator;
        public UInt32 denominator;
    }

    public struct v4l2_area
    {
        public UInt32 width;
        public UInt32 height;
    }

    public class v4l2_capability : V4L2Struct
    {
        public string driver = "";
        public string card = "";
        public string bus_info = "";
        internal byte[] driver_buf = new byte[16];
        internal byte[] card_buf = new byte[32];
        internal byte[] bus_info_buf = new byte[32];
        public UInt32 version;
        public UInt32 capabilities;
        public UInt32 device_caps;
        public UInt32[] reserved = new UInt32[3];

        public new const int NativeSize = 4 * 6 + 16 + 32 + 32;

        public override int GetSize()
        {
            return NativeSize;
        }

        public override void UpdateFromUnmanaged()
        {
            ms.Position = 0;
            driver_buf = br.ReadBytes(16);
            card_buf = br.ReadBytes(32);
            bus_info_buf = br.ReadBytes(32);
            version = br.ReadUInt32();
            capabilities = br.ReadUInt32();
            device_caps = br.ReadUInt32();
            reserved[0] = br.ReadUInt32();
            reserved[1] = br.ReadUInt32();
            reserved[2] = br.ReadUInt32();

            driver = Encoding.ASCII.GetString(driver_buf);
            card = Encoding.ASCII.GetString(card_buf);
            bus_info = Encoding.ASCII.GetString(bus_info_buf);
        }

        public override nint GetPointer()
        {
            WriteStringToBuffer(driver, driver_buf);
            WriteStringToBuffer(card, card_buf);
            WriteStringToBuffer(bus_info, bus_info_buf);

            ms.Position = 0;
            bw.Write(driver_buf);
            bw.Write(card_buf);
            bw.Write(bus_info_buf);
            bw.Write(version);
            bw.Write(capabilities);
            bw.Write(device_caps);
            bw.Write(reserved[0]);
            bw.Write(reserved[1]);
            bw.Write(reserved[2]);
            return selfPtr;
        }
    };

    public class v4l2_pix_format : V4L2Struct
    {
        public UInt32 width;
        public UInt32 height;
        public string pixelformat = "";
        internal byte[] pixelformat_buf = new byte[4];
        public v4l2_field field;
        public UInt32 bytesperline; /* for padding, zero if unused */
        public UInt32 sizeimage;
        public v4l2_colorspace colorspace;
        public UInt32 priv;     /* private data, depends on pixelformat */
        public UInt32 flags;        /* format flags (V4L2_PIX_FMT_FLAG_*) */
        //union {
        UInt32 union;
        public v4l2_ycbcr_encoding ycbcr_enc
        {
            get { return (v4l2_ycbcr_encoding)union; }
            set { union = (UInt32)value; }
        }
        public v4l2_hsv_encoding hsv_enc
        {
            get { return (v4l2_hsv_encoding)union; }
            set { union = (UInt32)value; }
        }
        //}
        public v4l2_quantization quantization;
        public v4l2_xfer_func xfer_func;

        public new const int NativeSize = 12 * 4;

        public unsafe v4l2_pix_format(byte* ptr) : base(ptr)
        {
        }

        public override int GetSize()
        {
            return NativeSize;
        }

        public override void UpdateFromUnmanaged()
        {
            ms.Position = 0;
            width = br.ReadUInt32();
            height = br.ReadUInt32();
            pixelformat_buf = br.ReadBytes(4);
            field = (v4l2_field)br.ReadUInt32();
            bytesperline = br.ReadUInt32();
            sizeimage = br.ReadUInt32();
            colorspace = (v4l2_colorspace)br.ReadUInt32();
            priv = br.ReadUInt32();
            flags = br.ReadUInt32();
            union = br.ReadUInt32();
            quantization = (v4l2_quantization)br.ReadUInt32();
            xfer_func = (v4l2_xfer_func)br.ReadUInt32();

            pixelformat = Encoding.ASCII.GetString(pixelformat_buf);
        }

        public override nint GetPointer()
        {
            WriteStringToBuffer(pixelformat, pixelformat_buf);

            ms.Position = 0;
            bw.Write(width);
            bw.Write(height);
            bw.Write(pixelformat_buf);
            bw.Write((UInt32)field);
            bw.Write(bytesperline);
            bw.Write(sizeimage);
            bw.Write((UInt32)colorspace);
            bw.Write(priv);
            bw.Write(flags);
            bw.Write(union);
            bw.Write((UInt32)quantization);
            bw.Write((UInt32)xfer_func);
            return selfPtr;
        }
    }
    public class v4l2_fmtdesc : V4L2Struct
    {
        /// <summary>
        /// Format number
        /// </summary>
        public UInt32 index;
        public v4l2_buf_type type;
        public UInt32 flags;
        /// <summary>
        /// Description string
        /// </summary>
        public string description = "";
        internal byte[] description_buf = new byte[32];
        /// <summary>
        /// Format fourcc
        /// </summary>
        public string pixelformat = "";
        internal byte[] pixelformat_buf = new byte[4];
        /// <summary>
        /// Media bus code
        /// </summary>
        public UInt32 mbus_code;
        public UInt32[] reserved = new UInt32[3];

        public new const int NativeSize = 4 * 8 + 32;

        public override int GetSize()
        {
            return NativeSize;
        }

        public override void UpdateFromUnmanaged()
        {
            ms.Position = 0;
            index = br.ReadUInt32();
            type = (v4l2_buf_type)br.ReadUInt32();
            flags = br.ReadUInt32();
            description_buf = br.ReadBytes(32);
            pixelformat_buf = br.ReadBytes(4);
            mbus_code = br.ReadUInt32();
            reserved[0] = br.ReadUInt32();
            reserved[1] = br.ReadUInt32();
            reserved[2] = br.ReadUInt32();

            description = Encoding.ASCII.GetString(description_buf);
            pixelformat = Encoding.ASCII.GetString(pixelformat_buf);
        }

        public override nint GetPointer()
        {
            WriteStringToBuffer(description, description_buf);
            WriteStringToBuffer(pixelformat, pixelformat_buf);

            ms.Position = 0;
            bw.Write(index);
            bw.Write((UInt32)type);
            bw.Write(flags);
            bw.Write(description_buf);
            bw.Write(pixelformat_buf);
            bw.Write(mbus_code);
            bw.Write(reserved[0]);
            bw.Write(reserved[1]);
            bw.Write(reserved[2]);
            return selfPtr;
        }
    }

    public struct v4l2_frmsize_discrete
    {
        public UInt32 width;        /* Frame width [pixel] */
        public UInt32 height;       /* Frame height [pixel] */
    }

    public struct v4l2_frmsize_stepwise
    {
        public UInt32 min_width;    /* Minimum frame width [pixel] */
        public UInt32 max_width;    /* Maximum frame width [pixel] */
        public UInt32 step_width;   /* Frame width step size [pixel] */
        public UInt32 min_height;   /* Minimum frame height [pixel] */
        public UInt32 max_height;   /* Maximum frame height [pixel] */
        public UInt32 step_height;  /* Frame height step size [pixel] */
    }

    public class v4l2_frmsizeenum : V4L2Struct
    {
        /// <summary>
        /// Frame size number
        /// </summary>
        public UInt32 index;
        /// <summary>
        /// Pixel format
        /// </summary>
        public string pixel_format = "";
        internal byte[] pixel_format_buf = new byte[4];
        /// <summary>
        /// Frame size type the device supports.
        /// </summary>
        public v4l2_frmsizetypes type;

        //union {					/* Frame size */
        public v4l2_frmsize_discrete discrete;
        public v4l2_frmsize_stepwise stepwise;
        int u_size => Math.Max(Marshal.SizeOf<v4l2_frmsize_discrete>(), Marshal.SizeOf<v4l2_frmsize_stepwise>());
        //};

        public UInt32[] reserved = new UInt32[2];			/* Reserved space for future use */

        public unsafe v4l2_frmsizeenum() : base()
        {
            ms.Position = 4 * 3; // Skip 3 UInt32s
            discrete = ReadValue<v4l2_frmsize_discrete>();
            ms.Position = 4 * 3; // Skip 3 UInt32s
            stepwise = ReadValue<v4l2_frmsize_stepwise>();
        }

        public new int NativeSize => 4 * 5 + u_size;

        public override int GetSize()
        {
            return NativeSize;
        }

        public override void UpdateFromUnmanaged()
        {
            ms.Position = 0;
            index = br.ReadUInt32();
            pixel_format_buf = br.ReadBytes(4);
            type = (v4l2_frmsizetypes)br.ReadUInt32();
            switch (type)
            {
                case v4l2_frmsizetypes.DISCRETE:
                    {
                        discrete = ReadValue<v4l2_frmsize_discrete>();
                        break;
                    }
                case v4l2_frmsizetypes.CONTINUOUS:
                case v4l2_frmsizetypes.STEPWISE:
                    {
                        stepwise = ReadValue<v4l2_frmsize_stepwise>();
                        break;
                    }
            }
            ms.Position = NativeSize - 8;
            reserved[0] = br.ReadUInt32();
            reserved[1] = br.ReadUInt32();

            pixel_format = Encoding.ASCII.GetString(pixel_format_buf);
        }

        public override nint GetPointer()
        {
            WriteStringToBuffer(pixel_format, pixel_format_buf);

            ms.Position = 0;
            bw.Write(index);
            bw.Write(pixel_format_buf);
            bw.Write((UInt32)type);
            switch (type)
            {
                case v4l2_frmsizetypes.DISCRETE:
                    {
                        WriteValue(discrete);
                        break;
                    }
                case v4l2_frmsizetypes.CONTINUOUS:
                case v4l2_frmsizetypes.STEPWISE:
                    {
                        WriteValue(stepwise);
                        break;
                    }
            }
            ms.Position = NativeSize - 8;
            bw.Write(reserved[0]);
            bw.Write(reserved[1]);
            return selfPtr;
        }
    }

    public struct v4l2_frmival_stepwise
    {
        public v4l2_fract min;       /* Minimum frame interval [s] */
        public v4l2_fract max;       /* Maximum frame interval [s] */
        public v4l2_fract step;      /* Frame interval step size [s] */
    }

    public class v4l2_frmivalenum : V4L2Struct
    {
        /// <summary>
        /// Frame format index
        /// </summary>
        public UInt32 index;
        /// <summary>
        /// Pixel format
        /// </summary>
        public string pixel_format = "";
        internal byte[] pixel_format_buf = new byte[4];
        /// <summary>
        /// Frame width
        /// </summary>
        public UInt32 width;
        /// <summary>
        /// Frame height
        /// </summary>
        public UInt32 height;
        /// <summary>
        /// Frame interval type the device supports.
        /// </summary>
        public v4l2_frmivaltypes type;

        //union {					/* Frame interval */
        public v4l2_fract discrete;
        public v4l2_frmival_stepwise stepwise;
        int u_size => Math.Max(Marshal.SizeOf<v4l2_fract>(), Marshal.SizeOf<v4l2_frmival_stepwise>());
        //};

        public UInt32[] reserved = new UInt32[2];			/* Reserved space for future use */

        public unsafe v4l2_frmivalenum() : base()
        {
            ms.Position = 4 * 5;
            discrete = ReadValue<v4l2_fract>();
            ms.Position = 4 * 5;
            stepwise = ReadValue<v4l2_frmival_stepwise>();
        }

        public new int NativeSize => 4 * 7 + u_size;

        public override int GetSize()
        {
            return NativeSize;
        }

        public override void UpdateFromUnmanaged()
        {
            ms.Position = 0;
            index = br.ReadUInt32();
            pixel_format_buf = br.ReadBytes(4);
            width = br.ReadUInt32();
            height = br.ReadUInt32();
            type = (v4l2_frmivaltypes)br.ReadUInt32();
            switch (type)
            {
                case v4l2_frmivaltypes.DISCRETE:
                    {
                        discrete = ReadValue<v4l2_fract>();
                        break;
                    }
                case v4l2_frmivaltypes.CONTINUOUS:
                case v4l2_frmivaltypes.STEPWISE:
                    {
                        stepwise = ReadValue<v4l2_frmival_stepwise>();
                        break;
                    }
            }
            ms.Position += u_size;
            reserved[0] = br.ReadUInt32();
            reserved[1] = br.ReadUInt32();

            pixel_format = Encoding.ASCII.GetString(pixel_format_buf);
        }

        public override nint GetPointer()
        {
            WriteStringToBuffer(pixel_format, pixel_format_buf);

            ms.Position = 0;
            bw.Write(index);
            bw.Write(pixel_format_buf);
            bw.Write(width);
            bw.Write(height);
            bw.Write((UInt32)type);
            switch (type)
            {
                case v4l2_frmivaltypes.DISCRETE:
                    {
                        WriteValue(discrete);
                        break;
                    }
                case v4l2_frmivaltypes.CONTINUOUS:
                case v4l2_frmivaltypes.STEPWISE:
                    {
                        WriteValue(stepwise);
                        break;
                    }
            }
            ms.Position += u_size;
            bw.Write(reserved[0]);
            bw.Write(reserved[1]);
            return selfPtr;
        }
    }

    public struct v4l2_timecode
    {
        public UInt32 type;
        public UInt32 flags;
        public byte frames;
        public byte seconds;
        public byte minutes;
        public byte hours;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public byte[] userbits;
    }

    public struct v4l2_jpegcompression
    {
        public int quality;
        /// <summary>
        /// Number of APP segment to be written, must be 0..15
        /// </summary>
        public int APPn;
        /// <summary>
        /// Length of data in JPEG APPn segment
        /// </summary>
        public int APP_len;
		/// <summary>
		/// Data in the JPEG APPn segment
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
		public byte[] APP_data;
        /// <summary>
        /// Length of data in JPEG COM segment
        /// </summary>
        public int COM_len;
		/// <summary>
		/// Data in JPEG COM segment
		/// </summary>
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
		public byte[] COM_data;

        /// <summary>
        /// Which markers should go into the JPEG output. <br/>
		/// Unless you exactly know what you do, leave them untouched. <br/>
		/// Including less markers will make the resulting code smaller, but there will be fewer applications which can read it. <br/>
		/// The presence of the APP and COM marker is influenced by APP_len and COM_len ONLY, not by this property!
        /// </summary>
        public UInt32 jpeg_markers;
    }

    public struct v4l2_requestbuffers
    {
        public UInt32 count;
        public v4l2_buf_type type;
        public v4l2_memory memory;
        public UInt32 capabilities;
        public byte flags;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public byte[] reserved;
    }

    public class v4l2_plane : V4L2Struct
    {
        public UInt32 bytesused;
        public UInt32 length;
        //union {
        byte[] union = new byte[8];
        public UInt32 mem_offset
        {
            get
            {
                return BitConverter.ToUInt32(union);
            }
            set
            {
                BitConverter.TryWriteBytes(union, value);
            }
        }
        public UInt64 userptr
        {
            get
            {
                return BitConverter.ToUInt64(union);
            }
            set
            {
                BitConverter.TryWriteBytes(union, value);
            }
        }
        public Int32 fd
        {
            get
            {
                return BitConverter.ToInt32(union);
            }
            set
            {
                BitConverter.TryWriteBytes(union, value);
            }
        }
        //} m;
        public UInt32 data_offset;
        public UInt32[] reserved = new UInt32[11];

        public unsafe v4l2_plane(byte* ptr) : base(ptr)
        {
        }

        public new const int NativeSize = 4 * 16;

        public override int GetSize()
        {
            return NativeSize;
        }

        public const int StructSize = 4 * 16;

        public override void UpdateFromUnmanaged()
        {
            ms.Position = 0;
            bytesused = br.ReadUInt32();
            length = br.ReadUInt32();
            union = br.ReadBytes(8);
            data_offset = br.ReadUInt32();
            for (int i = 0; i < 11; i++)
            {
                reserved[i] = br.ReadUInt32();
            }
        }

        public override nint GetPointer()
        {
            ms.Position = 0;
            bw.Write(bytesused);
            bw.Write(length);
            bw.Write(union);
            bw.Write(data_offset);
            for (int i = 0; i < 11; i++)
            {
                bw.Write(reserved[i]);
            }
            return selfPtr;
        }
    }

    public struct timeval
    {
        public long tv_sec;
        public long tv_usec;
    }

    public class v4l2_buffer : V4L2Struct
    {
        public UInt32 index;
        public v4l2_buf_type type;
        public UInt32 bytesused;
        public UInt32 flags;
        public UInt32 field;
        public timeval timestamp;
        public v4l2_timecode timecode;
        public UInt32 sequence;

        /* memory location */
        public v4l2_memory memory;
        //union {
        public UInt32 offset;
        public UInt64 userptr;
        // as void*
        public v4l2_plane[] planes = new v4l2_plane[(int)VIDEO_MAX.PLANES];
        public Int32 fd;
        //} m;
        public UInt32 length;
        public UInt32 reserved2;
        public Int32 request_fd;

        IntPtr planes_data;

        public unsafe v4l2_buffer() : base()
        {
            ms.Position = 4 * 6; // Alignment
            timestamp = ReadValue<timeval>();
            timecode = ReadValue<v4l2_timecode>();

            planes_data = Marshal.AllocHGlobal(v4l2_plane.StructSize * (int)VIDEO_MAX.PLANES);
            byte* planes_ptr = (byte*)planes_data.ToPointer();
            for (int i = 0; i < (int)VIDEO_MAX.PLANES; i++)
            {
                planes[i] = new v4l2_plane(planes_ptr + (v4l2_plane.StructSize * i));
            }
        }

        ~v4l2_buffer()
        {
            Marshal.FreeHGlobal(planes_data);
        }

        // TODO: There is one more alignment somewhere
        public new int NativeSize => 4 * 12 + Marshal.SizeOf<timeval>() + Marshal.SizeOf<v4l2_timecode>() + 8;

        public override int GetSize()
        {
            return NativeSize;
        }

        public override void UpdateFromUnmanaged()
        {
            ms.Position = 0;
            index = br.ReadUInt32();
            type = (v4l2_buf_type)br.ReadUInt32();
            bytesused = br.ReadUInt32();
            flags = br.ReadUInt32();
            field = br.ReadUInt32();
            timestamp = ReadValue<timeval>();
            timecode = ReadValue<v4l2_timecode>();
            ms.Position += 4; // Alignment
            sequence = br.ReadUInt32();
            memory = (v4l2_memory)br.ReadUInt32();
            if (type == v4l2_buf_type.VIDEO_CAPTURE_MPLANE || type == v4l2_buf_type.VIDEO_OUTPUT_MPLANE)
            {
                // Is it something to be done?
                ms.Position += 8;
            }
            else
            {
                switch (memory)
                {
                    case v4l2_memory.MMAP:
                        {
                            offset = br.ReadUInt32();
                            ms.Position += 4;
                            break;
                        }
                    case v4l2_memory.USERPTR:
                        {
                            userptr = br.ReadUInt64();
                            break;
                        }
                    case v4l2_memory.DMABUF:
                        {
                            fd = br.ReadInt32();
                            ms.Position += 4;
                            break;
                        }
                    default:
                        {
                            throw new NotSupportedException();
                        }
                }
            }
            length = br.ReadUInt32();
            reserved2 = br.ReadUInt32();
            request_fd = br.ReadInt32();

            if (type == v4l2_buf_type.VIDEO_CAPTURE_MPLANE || type == v4l2_buf_type.VIDEO_OUTPUT_MPLANE)
            {
                if(length > planes.Length)
                {
                    Console.WriteLine("length: {0} > {1}", length, planes.Length);
                }
                // Now we can read planes if needed
                for (int i = 0; i < length; i++)
                {
                    planes[i].UpdateFromUnmanaged();
                }
            }
        }

        public override nint GetPointer()
        {
            ms.Position = 0;
            bw.Write(index);
            bw.Write((UInt32)type);
            bw.Write(bytesused);
            bw.Write(flags);
            bw.Write(field);
            WriteValue(timestamp);
            WriteValue(timecode);
            ms.Position += 4; // Alignment
            bw.Write(sequence);
            bw.Write((UInt32)memory);
            if (type == v4l2_buf_type.VIDEO_CAPTURE_MPLANE || type == v4l2_buf_type.VIDEO_OUTPUT_MPLANE)
            {
                for (int i = 0; i < length; i++)
                {
                    planes[i].GetPointer();
                }
                bw.Write(planes_data);
            }
            else
            {
                switch (memory)
                {
                    case v4l2_memory.MMAP:
                        {
                            bw.Write(offset);
                            Fill(4);
                            break;
                        }
                    case v4l2_memory.USERPTR:
                        {
                            bw.Write(userptr);
                            break;
                        }
                    case v4l2_memory.DMABUF:
                        {
                            bw.Write(fd);
                            Fill(4);
                            break;
                        }
                    default:
                        {
                            throw new NotSupportedException();
                        }
                }
            }
            bw.Write(length);
            bw.Write(reserved2);
            bw.Write(request_fd);
            return selfPtr;
        }
    }

    public struct v4l2_exportbuffer
    {
        public v4l2_buf_type type;
        public UInt32 index;
        public UInt32 plane;
        public UInt32 flags;
        public Int32 fd;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
		public UInt32[] reserved;
    }

    public class v4l2_framebuffer : V4L2Struct
    {
        public UInt32 capability;
        public UInt32 flags;
        public IntPtr base_ptr; // void*
                                //struct {
        public UInt32 width;
        public UInt32 height;
        public string pixelformat = "";
        internal byte[] pixelformat_buf = new byte[4];
        public v4l2_field field;
        /// <summary>
        /// for padding, zero if unused
        /// </summary>
        public UInt32 bytesperline;
        public UInt32 sizeimage;
        public v4l2_colorspace colorspace;
        /// <summary>
        /// reserved field, set to 0
        /// </summary>
        public UInt32 priv;
        //   } fmt;

        public new const int NativeSize = 4 * 12;

        public override int GetSize()
        {
            return NativeSize;
        }

        public override void UpdateFromUnmanaged()
        {
            ms.Position = 0;
            capability = br.ReadUInt32();
            flags = br.ReadUInt32();
            base_ptr = (IntPtr)br.ReadInt64();
            width = br.ReadUInt32();
            height = br.ReadUInt32();
            pixelformat_buf = br.ReadBytes(4);
            field = (v4l2_field)br.ReadUInt32();
            bytesperline = br.ReadUInt32();
            sizeimage = br.ReadUInt32();
            colorspace = (v4l2_colorspace)br.ReadUInt32();
            priv = br.ReadUInt32();

            pixelformat = Encoding.ASCII.GetString(pixelformat_buf);
        }

        public override nint GetPointer()
        {
            WriteStringToBuffer(pixelformat, pixelformat_buf);

            ms.Position = 0;
            bw.Write(capability);
            bw.Write(flags);
            bw.Write(base_ptr);
            bw.Write(width);
            bw.Write(height);
            bw.Write(pixelformat_buf);
            bw.Write((UInt32)field);
            bw.Write(bytesperline);
            bw.Write(sizeimage);
            bw.Write((UInt32)colorspace);
            bw.Write(priv);
            return selfPtr;
        }
    }

    public struct v4l2_clip
    {
        public v4l2_rect c;
        // Docs say it's always NULL
        public nint next;
    }

    public class v4l2_window : V4L2Struct
    {
        public v4l2_rect w;
        public v4l2_field field;
        public UInt32 chromakey;
        public v4l2_clip[] clips = new v4l2_clip[16]; // As pointer
        public UInt32 clipcount;
        public IntPtr bitmap;
        public byte global_alpha;

        IntPtr clips_data;

        public new int NativeSize => Marshal.SizeOf(w) + 4 * 7 + 1;

        public override int GetSize()
        {
            return NativeSize;
        }

        public unsafe v4l2_window(byte* ptr) : base(ptr)
        {
            ms.Position = 0;
            w = ReadValue<v4l2_rect>();
            clips_data = Marshal.AllocHGlobal(v4l2_plane.StructSize * (int)VIDEO_MAX.PLANES);
            for (int i = 0; i < (int)VIDEO_MAX.PLANES; i++)
            {
                clips[i] = ReadValue<v4l2_clip>(clips_data + (Marshal.SizeOf<v4l2_clip>() * i));
            }
        }

        ~v4l2_window()
        {
            Marshal.FreeHGlobal(clips_data);
        }

        public override void UpdateFromUnmanaged()
        {
            ms.Position = 0;
            w = ReadValue<v4l2_rect>();
            field = (v4l2_field)ReadValue<UInt32>();
            chromakey = ReadValue<UInt32>();
            if(br.ReadInt64() != clips_data)
            {
                throw new Exception("Clips pointer changed!");
            }
            clipcount = ReadValue<UInt32>();
            if(clipcount > 16)
            {
                throw new IndexOutOfRangeException();
            }
            bitmap = ReadValue<nint>();
            global_alpha = ReadValue<byte>();

            // Now we can read clips if needed
            for (int i = 0; i < clipcount; i++)
            {
                clips[i] = ReadValue<v4l2_clip>(clips_data + (Marshal.SizeOf<v4l2_clip>() * i));
            }
        }

        public override nint GetPointer()
        {
            ms.Position = 0;
            WriteValue(w);
            WriteValue((UInt32)field);
            WriteValue(chromakey);
            for (int i = 0; i < clipcount; i++)
            {
                WriteValue(clips[i], clips_data + (Marshal.SizeOf<v4l2_clip>() * i));
            }
            WriteValue(clips_data);
            WriteValue(clipcount);
            WriteValue(bitmap);
            WriteValue(global_alpha);
            return selfPtr;
        }
    }

    public struct v4l2_captureparm
    {
        /// <summary>
        /// Supported modes
        /// </summary>
        public UInt32 capability;
        /// <summary>
        /// Current mode
        /// </summary>
        public UInt32 capturemode;
        /// <summary>
        /// Time per frame in seconds
        /// </summary>
        public v4l2_fract timeperframe;
        /// <summary>
        /// Driver-specific extensions
        /// </summary>
	    public UInt32 extendedmode;
        /// <summary>
        /// # of buffers for read
        /// </summary>
        public UInt32 readbuffers;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public UInt32[] reserved;
    }

    public struct v4l2_outputparm
    {
        /// <summary>
        /// Supported modes
        /// </summary>
        public UInt32 capability;
        /// <summary>
        /// Current mode
        /// </summary>
        public UInt32 outputmode;
        /// <summary>
        /// Time per frame in seconds
        /// </summary>
        public v4l2_fract timeperframe;
        /// <summary>
        /// Driver-specific extensions
        /// </summary>
        public UInt32 extendedmode;
        /// <summary>
        /// # of buffers for write
        /// </summary>
        public UInt32 writebuffers;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public UInt32[] reserved;
    }

    public struct v4l2_cropcap
    {
        public v4l2_buf_type type;
        public v4l2_rect bounds;
        public v4l2_rect defrect;
        public v4l2_fract pixelaspect;
    }

    public struct v4l2_crop
    {
        public v4l2_buf_type type;
        public v4l2_rect c;
    }

    public struct v4l2_selection
    {
        public v4l2_buf_type type;
        public UInt32 target;
        public UInt32 flags;
        public v4l2_rect r;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public UInt32[] reserved;
    }

    public class v4l2_standard : V4L2Struct
    {
        public UInt32 index;
        public UInt64 id;
        public string name = "";
        internal byte[] name_buf = new byte[24];
        /// <summary>
        /// Frames, not fields
        /// </summary>
        public v4l2_fract frameperiod;
        public UInt32 framelines;
        public UInt32[] reserved = new UInt32[4];

        public unsafe v4l2_standard() : base()
        {
            ms.Position = 4 + 8 + 24;
            frameperiod = ReadValue<v4l2_fract>();
        }

        public new int NativeSize => 4 * 8 + 24 + Marshal.SizeOf<v4l2_fract>();

        public override int GetSize()
        {
            return NativeSize;
        }

        public override void UpdateFromUnmanaged()
        {
            ms.Position = 0;
            index = br.ReadUInt32();
            id = br.ReadUInt64();
            name_buf = br.ReadBytes(24);
            frameperiod = ReadValue<v4l2_fract>();
            framelines = br.ReadUInt32();
            for (int i = 0; i < 4; i++)
            {
                reserved[i] = br.ReadUInt32();
            }

            name = Encoding.ASCII.GetString(name_buf);
        }

        public override nint GetPointer()
        {
            WriteStringToBuffer(name, name_buf);

            ms.Position = 0;
            bw.Write(index);
            bw.Write(id);
            bw.Write(name_buf);
            WriteValue(frameperiod);
            bw.Write(framelines);
            for (int i = 0; i < 4; i++)
            {
                bw.Write(reserved[i]);
            }
            return selfPtr;
        }
    }

    public struct v4l2_bt_timings
    {
        public UInt32 width;
        public UInt32 height;
        public UInt32 interlaced;
        public UInt32 polarities;
        public UInt64 pixelclock;
        public UInt32 hfrontporch;
        public UInt32 hsync;
        public UInt32 hbackporch;
        public UInt32 vfrontporch;
        public UInt32 vsync;
        public UInt32 vbackporch;
        public UInt32 il_vfrontporch;
        public UInt32 il_vsync;
        public UInt32 il_vbackporch;
        public UInt32 standards;
        public UInt32 flags;
        public v4l2_fract picture_aspect;
        public byte cea861_vic;
        public byte hdmi_vic;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 46)]
        public byte[] reserved;
    } //__attribute__((packed));

    public class v4l2_dv_timings
    {
        UInt32 type;
        //union {
        v4l2_bt_timings bt;
        UInt32[] reserved = new UInt32[32];
        //};
    } //__attribute__((packed));

    public class v4l2_enum_dv_timings
    {
        UInt32 index;
        UInt32 pad;
        UInt32[] reserved = new UInt32[2];
        v4l2_dv_timings timings;
    }

    public struct v4l2_bt_timings_cap
    {
        public UInt32 min_width;
        public UInt32 max_width;
        public UInt32 min_height;
        public UInt32 max_height;
        public UInt64 min_pixelclock;
        public UInt64 max_pixelclock;
        public UInt32 standards;
        public UInt32 capabilities;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public UInt32[] reserved;
    } //__attribute__((packed));

    public class v4l2_dv_timings_cap
    {
        UInt32 type;
        UInt32 pad;
        UInt32[] reserved = new UInt32[2];
        //union {
        v4l2_bt_timings_cap bt;
        UInt32[] raw_data = new UInt32[32];
        //};
    }

    public class v4l2_input
    {
        UInt32 index;        /*  Which input */
        byte[] name = new byte[32];      /*  Label */
        UInt32 type;     /*  Type of input */
        UInt32 audioset;     /*  Associated audios (bitfield) */
        UInt32 tuner;             /*  Tuner index */
        UInt64 std;
        UInt32 status;
        UInt32 capabilities;
        UInt32[] reserved = new UInt32[3];
    }

    public class v4l2_output
    {
        UInt32 index;        /*  Which output */
        byte[] name = new byte[32];      /*  Label */
        UInt32 type;     /*  Type of output */
        UInt32 audioset;     /*  Associated audios (bitfield) */
        UInt32 modulator;         /*  Associated modulator */
        UInt64 std;
        UInt32 capabilities;
        UInt32[] reserved = new UInt32[3];
    }

    public struct v4l2_control
    {
        public UInt32 id;
        public Int32 value;
    }

    // TODO: need id -> payload dict

    //public class v4l2_ext_control
    //{
    //    UInt32 id;
    //    UInt32 size;
    //    UInt32 reserved2;
    //    //union {
    //  Int32 value;
    //        Int64 value64;
    //        char __user *string;
    //  byte __user *p_u8;
    //  UInt16 __user *p_u16;
    //  UInt32 __user *p_u32;
    //  Int32 __user *p_s32;
    //  Int64 __user *p_s64;
    //        v4l2_area __user* p_area;
    //        v4l2_ctrl_h264_sps __user* p_h264_sps;
    //        v4l2_ctrl_h264_pps *p_h264_pps;
    //  v4l2_ctrl_h264_scaling_matrix __user* p_h264_scaling_matrix;
    //        v4l2_ctrl_h264_pred_weights __user* p_h264_pred_weights;
    //        v4l2_ctrl_h264_slice_params __user* p_h264_slice_params;
    //        v4l2_ctrl_h264_decode_params __user* p_h264_decode_params;
    //        v4l2_ctrl_fwht_params __user* p_fwht_params;
    //        v4l2_ctrl_vp8_frame __user* p_vp8_frame;
    //        v4l2_ctrl_mpeg2_sequence __user* p_mpeg2_sequence;
    //        v4l2_ctrl_mpeg2_picture __user* p_mpeg2_picture;
    //        v4l2_ctrl_mpeg2_quantisation __user* p_mpeg2_quantisation;
    //        v4l2_ctrl_vp9_compressed_hdr __user* p_vp9_compressed_hdr_probs;
    //        v4l2_ctrl_vp9_frame __user* p_vp9_frame;
    //        v4l2_ctrl_hevc_sps __user* p_hevc_sps;
    //        v4l2_ctrl_hevc_pps __user* p_hevc_pps;
    //        v4l2_ctrl_hevc_slice_params __user* p_hevc_slice_params;
    //        v4l2_ctrl_hevc_scaling_matrix __user* p_hevc_scaling_matrix;
    //        v4l2_ctrl_hevc_decode_params __user* p_hevc_decode_params;
    //        v4l2_ctrl_av1_sequence __user* p_av1_sequence;
    //        v4l2_ctrl_av1_tile_group_entry __user* p_av1_tile_group_entry;
    //        v4l2_ctrl_av1_frame __user* p_av1_frame;
    //        v4l2_ctrl_av1_film_grain __user* p_av1_film_grain;
    //        UIntPtr ptr;
    // //};
    //} //__attribute__((packed));

    //public class v4l2_ext_controls
    //{
    //    UInt32 which;
    //    UInt32 count;
    //    UInt32 error_idx;
    //    Int32 request_fd;
    //    UInt32 reserved;
    //    v4l2_ext_control *controls;
    //};

    public class v4l2_queryctrl
    {
        UInt32 id;
        v4l2_ctrl_type type;
        byte[] name = new byte[32];  /* Whatever */
        Int32 minimum;  /* Note signedness */
        Int32 maximum;
        Int32 step;
        Int32 default_value;
        UInt32 flags;
        UInt32[] reserved = new UInt32[2];
    }

    public class v4l2_query_ext_ctrl
    {
        UInt32 id;
        UInt32 type;
        char[] name = new char[32];
        Int64 minimum;
        Int64 maximum;
        UInt64 step;
        Int64 default_value;
        UInt32 flags;
        UInt32 elem_size;
        UInt32 elems;
        UInt32 nr_of_dims;
        UInt32[] dims = new UInt32[(int)V4L2_CTRL.MAX_DIMS];
        UInt32[] reserved = new UInt32[32];
    }

    public class v4l2_querymenu
    {
        UInt32 id;
        UInt32 index;
        //union {
        byte[] name = new byte[32];  /* Whatever */
        Int64 value;
        //};
        UInt32 reserved;
    } //__attribute__((packed));

    public class v4l2_tuner
    {
        UInt32 index;
        byte[] name = new byte[32];
        v4l2_tuner_type type;
        UInt32 capability;
        UInt32 rangelow;
        UInt32 rangehigh;
        UInt32 rxsubchans;
        UInt32 audmode;
        Int32 signal;
        Int32 afc;
        UInt32[] reserved = new uint[4];
    }

    public class v4l2_modulator
    {
        UInt32 index;
        byte[] name = new byte[32];
        UInt32 capability;
        UInt32 rangelow;
        UInt32 rangehigh;
        UInt32 txsubchans;
        v4l2_tuner_type type;
        UInt32[] reserved = new UInt32[3];
    }

    public class v4l2_frequency
    {
        UInt32 tuner;
        v4l2_tuner_type type;
        UInt32 frequency;
        UInt32[] reserved = new UInt32[8];
    }

    public class v4l2_frequency_band
    {
        UInt32 tuner;
        v4l2_tuner_type type;
        UInt32 index;
        UInt32 capability;
        UInt32 rangelow;
        UInt32 rangehigh;
        UInt32 modulation;
        UInt32[] reserved = new UInt32[9];
    }

    public class v4l2_hw_freq_seek
    {
        UInt32 tuner;
        v4l2_tuner_type type;
        UInt32 seek_upward;
        UInt32 wrap_around;
        UInt32 spacing;
        UInt32 rangelow;
        UInt32 rangehigh;
        UInt32[] reserved = new UInt32[5];
    }

    public class v4l2_rds_data
    {
        byte lsb;
        byte msb;
        byte block;
    } //__attribute__((packed));

    public class v4l2_audio
    {
        UInt32 index;
        byte[] name = new byte[32];
        UInt32 capability;
        UInt32 mode;
        UInt32[] reserved = new UInt32[2];
    }

    public class v4l2_audioout
    {
        UInt32 index;
        byte[] name = new byte[32];
        UInt32 capability;
        UInt32 mode;
        UInt32[] reserved = new UInt32[2];
    }

    public class v4l2_enc_idx_entry
    {
        UInt64 offset;
        UInt64 pts;
        UInt32 length;
        UInt32 flags;
        UInt32[] reserved = new UInt32[2];
    }

    public class v4l2_enc_idx
    {
        UInt32 entries;
        UInt32 entries_cap;
        UInt32[] reserved = new UInt32[4];
        v4l2_enc_idx_entry[] entry = new v4l2_enc_idx_entry[(int)V4L2_ENC_IDX.ENTRIES];
    }

    public class v4l2_encoder_cmd
    {
        UInt32 cmd;
        UInt32 flags;
        UInt32[] raw_data = new UInt32[8];
    }

    public class v4l2_decoder_cmd
    {
        UInt32 cmd;
        UInt32 flags;
        //        union {
        UInt64 stop_pts;

        //		struct {
        //            /* 0 or 1000 specifies normal speed,
        //			   1 specifies forward single stepping,
        //			   -1 specifies backward single stepping,
        //			   >1: playback at speed/1000 of the normal speed,
        //			   <-1: reverse playback at (-speed/1000) of the normal speed. */
        Int32 speed;
        UInt32 format;
        //} start;
        UInt32[] raw_data = new UInt32[16];
    }

    public class v4l2_vbi_format
    {
        UInt32 sampling_rate;        /* in 1 Hz */
        UInt32 offset;
        UInt32 samples_per_line;
        UInt32 sample_format;        /* V4L2_PIX_FMT_* */
        Int32[] start = new Int32[2];
        UInt32[] count = new UInt32[2];
        UInt32 flags;            /* V4L2_VBI_* */
        UInt32[] reserved = new UInt32[2];      /* must be zero */
    }

    public class v4l2_sliced_vbi_format
    {
        UInt16 service_set;
        /* service_lines[0][...] specifies lines 0-23 (1-23 used) of the first field
           service_lines[1][...] specifies lines 0-23 (1-23 used) of the second field
                     (equals frame lines 313-336 for 625 line video
                      standards, 263-286 for 525 line standards) */
        UInt16[,] service_lines = new UInt16[2, 24];
        UInt32 io_size;
        UInt32[] reserved = new UInt32[2];            /* must be zero */
    }

    public class v4l2_sliced_vbi_cap
    {
        UInt16 service_set;
        /* service_lines[0][...] specifies lines 0-23 (1-23 used) of the first field
           service_lines[1][...] specifies lines 0-23 (1-23 used) of the second field
                     (equals frame lines 313-336 for 625 line video
                      standards, 263-286 for 525 line standards) */
        UInt16[,] service_lines = new UInt16[2, 24];
        v4l2_buf_type type;
        UInt32[] reserved = new UInt32[3];    /* must be 0 */
    }

    public class v4l2_sliced_vbi_data
    {
        UInt32 id;
        UInt32 field;          /* 0: first field, 1: second field */
        UInt32 line;           /* 1-23 */
        UInt32 reserved;       /* must be 0 */
        byte[] data = new byte[48];
    }

    public class v4l2_mpeg_vbi_itv0_line
    {
        byte id;    /* One of V4L2_MPEG_VBI_IVTV_* above */
        byte[] data = new byte[42];  /* Sliced VBI data for the line */
    } //__attribute__((packed));

    public class v4l2_mpeg_vbi_itv0
    {
        UInt32[] linemask = new UInt32[2]; /* Bitmasks of VBI service lines present */
        v4l2_mpeg_vbi_itv0_line[] line = new v4l2_mpeg_vbi_itv0_line[35];
    } // __attribute__((packed));

    public class v4l2_mpeg_vbi_ITV0
    {
        v4l2_mpeg_vbi_itv0_line[] line = new v4l2_mpeg_vbi_itv0_line[36];
    } //__attribute__((packed));

    public class v4l2_mpeg_vbi_fmt_ivtv
    {
        byte[] magic = new byte[4];
        //union {
        v4l2_mpeg_vbi_itv0 itv0;
        v4l2_mpeg_vbi_ITV0 ITV0;
        //};
    } //__attribute__((packed));

    public struct v4l2_plane_pix_format
    {
        public UInt32 sizeimage;
        public UInt32 bytesperline;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public UInt16[] reserved;
    } //__attribute__((packed));

    public class v4l2_pix_format_mplane : V4L2Struct
    {
        public UInt32 width;
        public UInt32 height;
        public string pixelformat = "";
        internal byte[] pixelformat_buf = new byte[4];
        public v4l2_field field;
        public v4l2_colorspace colorspace;

        public v4l2_plane_pix_format[] plane_fmt = new v4l2_plane_pix_format[(int)VIDEO_MAX.PLANES];
        public byte num_planes;
        public byte flags;
        //union {
        byte union;
        public v4l2_ycbcr_encoding ycbcr_enc { get { return (v4l2_ycbcr_encoding)union; } set { union = (byte)value; } } // 1 byte!
        public v4l2_hsv_encoding hsv_enc { get { return (v4l2_hsv_encoding)union; } set { union = (byte)value; } } // 1 byte!
        //};
        public v4l2_quantization quantization; // 1 byte!
        public v4l2_xfer_func xfer_func; // 1 byte!
        public byte[] reserved = new byte[7];

        public unsafe v4l2_pix_format_mplane(byte* ptr) : base(ptr)
        {
            ms.Position = 4 * 5;
            for (int i = 0; i < (int)VIDEO_MAX.PLANES; i++)
            {
                plane_fmt[i] = ReadValue<v4l2_plane_pix_format>();
            }
        }

        public new int NativeSize => 4 * 5 + 12 + (Marshal.SizeOf<v4l2_plane_pix_format>() * (int)VIDEO_MAX.PLANES);

        public override int GetSize()
        {
            return NativeSize;
        }

        public override void UpdateFromUnmanaged()
        {
            ms.Position = 0;
            width = br.ReadUInt32();
            height = br.ReadUInt32();
            pixelformat_buf = br.ReadBytes(4);
            field = (v4l2_field)br.ReadUInt32();
            colorspace = (v4l2_colorspace)br.ReadUInt32();
            for (int i = 0; i < (int)VIDEO_MAX.PLANES; i++)
            {
                plane_fmt[i] = ReadValue<v4l2_plane_pix_format>();
            }
            num_planes = br.ReadByte();
            flags = br.ReadByte();
            union = br.ReadByte();
            quantization = (v4l2_quantization)br.ReadByte();
            xfer_func = (v4l2_xfer_func)br.ReadByte();
            for (int i = 0; i < 7; i++)
            {
                reserved[i] = br.ReadByte();
            }

            pixelformat = Encoding.ASCII.GetString(pixelformat_buf);
        }

        public override nint GetPointer()
        {
            WriteStringToBuffer(pixelformat, pixelformat_buf);

            ms.Position = 0;
            bw.Write(width);
            bw.Write(height);
            bw.Write(pixelformat_buf);
            bw.Write((UInt32)field);
            bw.Write((UInt32)colorspace);
            for (int i = 0; i < (int)VIDEO_MAX.PLANES; i++)
            {
                WriteValue(plane_fmt[i]);
            }
            bw.Write(num_planes);
            bw.Write(flags);
            bw.Write(union);
            bw.Write((byte)quantization);
            bw.Write((byte)xfer_func);
            for (int i = 0; i < 7; i++)
            {
                bw.Write(reserved[i]);
            }
            return selfPtr;
        }
    } //__attribute__((packed));

    public struct v4l2_sdr_format
    {
        public UInt32 pixelformat;
        public UInt32 buffersize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public byte[] reserved;
    } //__attribute__((packed));

    public struct v4l2_meta_format
    {
        public UInt32 dataformat;
        public UInt32 buffersize;
    } //__attribute__((packed));

    public class v4l2_format : V4L2Struct
    {
        public v4l2_buf_type type;
        //union {
        public v4l2_pix_format pix;     /* V4L2_BUF_TYPE_VIDEO_CAPTURE */
        public v4l2_pix_format_mplane pix_mp;  /* V4L2_BUF_TYPE_VIDEO_CAPTURE_MPLANE */
        public v4l2_window win;     /* V4L2_BUF_TYPE_VIDEO_OVERLAY */
        //public v4l2_vbi_format vbi;     /* V4L2_BUF_TYPE_VBI_CAPTURE */
        //public v4l2_sliced_vbi_format sliced;  /* V4L2_BUF_TYPE_SLICED_VBI_CAPTURE */
        //public v4l2_sdr_format sdr;     /* V4L2_BUF_TYPE_SDR_CAPTURE */
        public v4l2_meta_format meta;    /* V4L2_BUF_TYPE_META_CAPTURE */
        //public byte[] raw_data = new byte[200];                   /* user-defined */
        //} fmt;

        public unsafe v4l2_format() : base()
        {
            ms.Position = 8; // Alignment?
            byte* p = ms.PositionPointer;
            pix = new v4l2_pix_format(p);
            pix_mp = new v4l2_pix_format_mplane(p);
            win = new v4l2_window(p);
            //vbi = new v4l2_vbi_format(p);
            //sliced = new v4l2_sliced_vbi_format(p);
            //sdr = new v4l2_sdr_format(p);
            ms.Position = 8;
            meta = ReadValue<v4l2_meta_format>();
        }

        public new const int NativeSize = 8 + 200; // Should be OK

        public override int GetSize()
        {
            return NativeSize;
        }

        public override void UpdateFromUnmanaged()
        {
            ms.Position = 0;
            type = (v4l2_buf_type)br.ReadUInt32();
            ms.Position = 8;
            switch (type)
            {
                case v4l2_buf_type.VIDEO_CAPTURE:
                    {
                        pix.UpdateFromUnmanaged();
                        break;
                    }
                case v4l2_buf_type.VIDEO_CAPTURE_MPLANE:
                    {
                        pix_mp.UpdateFromUnmanaged();
                        break;
                    }
                case v4l2_buf_type.VIDEO_OVERLAY:
                    {
                        win.UpdateFromUnmanaged();
                        break;
                    }
                case v4l2_buf_type.META_CAPTURE:
                    {
                        meta = ReadValue<v4l2_meta_format>();
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException($"Format {type:G} is not supported");
                    }
            }
            ms.Position = 208;
        }

        public override nint GetPointer()
        {
            ms.Position = 0;
            bw.Write((UInt32)type);
            ms.Position = 8;
            switch (type)
            {
                case v4l2_buf_type.VIDEO_CAPTURE:
                    {
                        pix.GetPointer();
                        break;
                    }
                case v4l2_buf_type.VIDEO_CAPTURE_MPLANE:
                    {
                        pix_mp.GetPointer();
                        break;
                    }
                case v4l2_buf_type.VIDEO_OVERLAY:
                    {
                        win.GetPointer();
                        break;
                    }
                case v4l2_buf_type.META_CAPTURE:
                    {
                        WriteValue(meta);
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException($"Format {type:G} is not supported");
                    }
            }
            ms.Position = 208;
            return selfPtr;
        }
    }

    //public class v4l2_streamparm
    //{
    //    UInt32 type;         /* enum v4l2_buf_type */
    //    //union {
    //  v4l2_captureparm capture;
    //  v4l2_outputparm  output;
    //  byte[] raw_data = new byte[200];  /* user-defined */
    //    //} parm;
    //};

    //public class v4l2_event_vsync
    //{
    //    /* Can be V4L2_FIELD_ANY, _NONE, _TOP or _BOTTOM */
    //    byte field;
    //} //__attribute__((packed));

    //public class v4l2_event_ctrl
    //{
    //    UInt32 changes;
    //    UInt32 type;
    //    //union {
    //  Int32 value;
    //        Int64 value64;
    //    //};
    //    UInt32 flags;
    //    Int32 minimum;
    //    Int32 maximum;
    //    Int32 step;
    //    Int32 default_value;
    //};

    //public class v4l2_event_frame_sync
    //{
    //    UInt32 frame_sequence;
    //};

    //public class v4l2_event_src_change
    //{
    //    UInt32 changes;
    //};

    //public class v4l2_event_motion_det
    //{
    //    UInt32 flags;
    //    UInt32 frame_sequence;
    //    UInt32 region_mask;
    //};

    //public class v4l2_event
    //{
    //    UInt32 type;
    //    //union {
    //  v4l2_event_vsync     vsync;
    //  v4l2_event_ctrl      ctrl;
    //  v4l2_event_frame_sync    frame_sync;
    //  v4l2_event_src_change    src_change;
    //  v4l2_event_motion_det    motion_det;
    //  byte[] data = new byte[64];
    //    //} u;
    // UInt32 pending;
    //    UInt32 sequence;
    // timespec         timestamp;
    // UInt32 id;
    //    UInt32[] reserved = new UInt32[8];
    //};

    //public class v4l2_event_subscription
    //{
    //    UInt32 type;
    //    UInt32 id;
    //    UInt32 flags;
    //    UInt32[] reserved = new UInt32[5];
    //};

    //public class v4l2_dbg_match
    //{
    //    UInt32 type; /* Match type */
    //    //union {     /* Match this chip, meaning determined by type */
    //  UInt32 addr;
    //        char[] name = new char[32];
    //    //};
    //} //__attribute__((packed));

    //public class v4l2_dbg_register
    //{
    //    v4l2_dbg_match match;
    // UInt32 size; /* register size in bytes */
    //    UInt64 reg;
    //    UInt64 val;
    //} //__attribute__((packed));

    //public class v4l2_dbg_chip_info
    //{
    //    v4l2_dbg_match match;
    // char[] name = new char[32];
    //    UInt32 flags;
    //    UInt32[] reserved = new UInt32[32];
    //} //__attribute__((packed));

    //public class v4l2_create_buffers
    //{
    //    UInt32 index;
    //    UInt32 count;
    //    UInt32 memory;
    //    v4l2_format  format;
    // UInt32 capabilities;
    //    UInt32 flags;
    //    UInt32[] reserved = new UInt32[6];
    //};
}