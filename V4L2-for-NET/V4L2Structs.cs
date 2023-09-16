using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace V4L2_for_NET
{
    public abstract class V4L2Struct
    {
        protected MemoryStream ms;
        protected BinaryReader br;
        protected BinaryWriter bw;

        public abstract byte[] Buffer { get; set; }

        public V4L2Struct()
        {
            ms = new MemoryStream();
            br = new BinaryReader(ms, Encoding.UTF8, true);
            bw = new BinaryWriter(ms, Encoding.UTF8, true); 
        }

        protected void Fill(int count)
        {
            for (int i = 0; i < count; i++)
            {
                ms.WriteByte(0);
            }
        }

        public abstract int GetSize();
    }

    public class v4l2_rect : V4L2Struct
    {
        public Int32 left;
        public Int32 top;
        public UInt32 width;
        public UInt32 height;

        public override int GetSize()
        {
            return 4 + 4 + 4 + 4;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(left);
                bw.Write(top);
                bw.Write(width);
                bw.Write(height);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                left = br.ReadInt32();
                top = br.ReadInt32();
                width = br.ReadUInt32();
                height = br.ReadUInt32();
            }
        }
    }
    public class v4l2_fract : V4L2Struct
    {
        public UInt32 numerator;
        public UInt32 denominator;

        public override int GetSize()
        {
            return 4 + 4;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(numerator);
                bw.Write(denominator);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                numerator = br.ReadUInt32();
                denominator = br.ReadUInt32();
            }
        }
    };

    public class v4l2_area : V4L2Struct
    {
        public UInt32 width;
        public UInt32 height;

        public override int GetSize()
        {
            return 4 + 4;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(width);
                bw.Write(height);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                width = br.ReadUInt32();
                height = br.ReadUInt32();
            }
        }
    };

    public class v4l2_capability : V4L2Struct
    {
        byte[] driver = new byte[16];
        byte[] card = new byte[32];
        byte[] bus_info = new byte[32];
        UInt32 version;
        UInt32 capabilities;
        UInt32 device_caps;
        UInt32[] reserved = new UInt32[3];

        public override int GetSize()
        {
            return 4 * 6 + 64;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                for (int i = 0; i < 16; i++)
                {
                    bw.Write(driver[i]);
                }
                for (int i = 0; i < 32; i++)
                {
                    bw.Write(card[i]);
                }
                for (int i = 0; i < 32; i++)
                {
                    bw.Write(bus_info[i]);
                }
                bw.Write(version);
                bw.Write(capabilities);
                bw.Write(device_caps);
                bw.Write(reserved[0]);
                bw.Write(reserved[1]);
                bw.Write(reserved[2]);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                driver= br.ReadBytes(16);
                card= br.ReadBytes(32);
                bus_info = br.ReadBytes(32);
                version = br.ReadUInt32();
                capabilities = br.ReadUInt32();
                device_caps = br.ReadUInt32();
                reserved[0] = br.ReadUInt32();
                reserved[1] = br.ReadUInt32();
                reserved[2] = br.ReadUInt32();
            }
        }
    };

    public class v4l2_pix_format : V4L2Struct
    {
        public UInt32 width;
        public UInt32 height;
        public UInt32 pixelformat;
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

        public override int GetSize()
        {
            return 12 * 4;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(width);
                bw.Write(height);
                bw.Write(pixelformat);
                bw.Write((UInt32)field);
                bw.Write(bytesperline);
                bw.Write(sizeimage);
                bw.Write((UInt32)colorspace);
                bw.Write(priv);
                bw.Write(flags);
                bw.Write(union);
                bw.Write((UInt32)quantization);
                bw.Write((UInt32)xfer_func);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                width = br.ReadUInt32();
                height = br.ReadUInt32();
                pixelformat = br.ReadUInt32();
                field = (v4l2_field)br.ReadUInt32();
                bytesperline = br.ReadUInt32();
                sizeimage = br.ReadUInt32();
                colorspace = (v4l2_colorspace)br.ReadUInt32();
                priv = br.ReadUInt32();
                flags = br.ReadUInt32();
                union = br.ReadUInt32();
                quantization = (v4l2_quantization)br.ReadUInt32();
                xfer_func = (v4l2_xfer_func)br.ReadUInt32();
            }
        }
    };

    public class v4l2_fmtdesc : V4L2Struct
    {
        public UInt32 index;             /* Format number      */
        public v4l2_buf_type type;
        public UInt32 flags;
        public byte[] description = new byte[32];   /* Description string */
        public UInt32 pixelformat;       /* Format fourcc      */
        public UInt32 mbus_code;        /* Media bus code    */
        public UInt32[] reserved = new UInt32[3];

        public override int GetSize()
        {
            return 4 * 8 + 32;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(index);
                bw.Write((UInt32)type);
                bw.Write(flags);
                bw.Write(description);
                bw.Write(pixelformat);
                bw.Write(mbus_code);
                bw.Write(reserved[0]);
                bw.Write(reserved[1]);
                bw.Write(reserved[2]);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                index = br.ReadUInt32();
                type = (v4l2_buf_type)br.ReadUInt32();
                flags = br.ReadUInt32();
                description = br.ReadBytes(32);
                pixelformat = br.ReadUInt32();
                mbus_code = br.ReadUInt32();
                reserved[0] = br.ReadUInt32();
                reserved[1] = br.ReadUInt32();
                reserved[2] = br.ReadUInt32();
            }
        }
    };

    public class v4l2_frmsize_discrete : V4L2Struct
    {
        public UInt32 width;        /* Frame width [pixel] */
        public UInt32 height;       /* Frame height [pixel] */

        public override int GetSize()
        {
            return 4 + 4;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(width);
                bw.Write(height);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                width = br.ReadUInt32();
                height = br.ReadUInt32();
            }
        }
    }
    public class v4l2_frmsize_stepwise : V4L2Struct
    {
        public UInt32 min_width;    /* Minimum frame width [pixel] */
        public UInt32 max_width;    /* Maximum frame width [pixel] */
        public UInt32 step_width;   /* Frame width step size [pixel] */
        public UInt32 min_height;   /* Minimum frame height [pixel] */
        public UInt32 max_height;   /* Maximum frame height [pixel] */
        public UInt32 step_height;  /* Frame height step size [pixel] */

        public override int GetSize()
        {
            return 4 * 6;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(min_width);
                bw.Write(max_width);
                bw.Write(step_width);
                bw.Write(min_height);
                bw.Write(max_height);
                bw.Write(step_height);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                min_width = br.ReadUInt32();
                max_width = br.ReadUInt32();
                step_width = br.ReadUInt32();
                min_height = br.ReadUInt32();
                max_height = br.ReadUInt32();
                step_height = br.ReadUInt32();
            }
        }
    };

    public class v4l2_frmsizeenum : V4L2Struct
    {
        public UInt32 index;        /* Frame size number */
        public UInt32 pixel_format; /* Pixel format */
        public v4l2_frmsizetypes type;     /* Frame size type the device supports. */

        //union {					/* Frame size */
        public v4l2_frmsize_discrete discrete = new v4l2_frmsize_discrete();
        public v4l2_frmsize_stepwise stepwise = new v4l2_frmsize_stepwise();
        int u_size { get { return Math.Max(discrete.GetSize(), stepwise.GetSize()); } }
        //};

        public UInt32[] reserved = new UInt32[2];			/* Reserved space for future use */

        public override int GetSize()
        {
            return 4 * 3 + u_size;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(index);
                bw.Write(pixel_format);
                bw.Write((UInt32)type);
                switch(type)
                {
                    case v4l2_frmsizetypes.V4L2_FRMSIZE_TYPE_DISCRETE:
                        {
                            bw.Write(discrete.Buffer);
                            Fill(u_size - discrete.GetSize());
                            break;
                        }
                    case v4l2_frmsizetypes.V4L2_FRMSIZE_TYPE_CONTINUOUS:
                    case v4l2_frmsizetypes.V4L2_FRMSIZE_TYPE_STEPWISE:
                        {
                            bw.Write(stepwise.Buffer);
                            Fill(u_size - stepwise.GetSize());
                            break;
                        }
                }
                bw.Write(reserved[0]);
                bw.Write(reserved[1]);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                index = br.ReadUInt32();
                pixel_format = br.ReadUInt32();
                type = (v4l2_frmsizetypes)br.ReadUInt32();
                switch (type)
                {
                    case v4l2_frmsizetypes.V4L2_FRMSIZE_TYPE_DISCRETE:
                        {
                            discrete.Buffer = br.ReadBytes(discrete.GetSize());
                            ms.Position += u_size - discrete.GetSize();
                            break;
                        }
                    case v4l2_frmsizetypes.V4L2_FRMSIZE_TYPE_CONTINUOUS:
                    case v4l2_frmsizetypes.V4L2_FRMSIZE_TYPE_STEPWISE:
                        {
                            stepwise.Buffer = br.ReadBytes(stepwise.GetSize());
                            ms.Position += u_size - stepwise.GetSize();
                            break;
                        }
                }
                reserved[0] = br.ReadUInt32();
                reserved[1] = br.ReadUInt32();
            }
        }
    };

    public class v4l2_frmival_stepwise : V4L2Struct
    {
        public v4l2_fract min = new v4l2_fract();       /* Minimum frame interval [s] */
        public v4l2_fract max = new v4l2_fract();       /* Maximum frame interval [s] */
        public v4l2_fract step = new v4l2_fract();      /* Frame interval step size [s] */

        public override int GetSize()
        {
            return min.GetSize() * 3;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(min.Buffer);
                bw.Write(max.Buffer);
                bw.Write(step.Buffer);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                min.Buffer = br.ReadBytes(min.GetSize());
                max.Buffer = br.ReadBytes(max.GetSize());
                step.Buffer = br.ReadBytes(step.GetSize());
            }
        }
    };

    public class v4l2_frmivalenum : V4L2Struct
    {
        public UInt32 index;        /* Frame format index */
        public UInt32 pixel_format; /* Pixel format */
        public UInt32 width;        /* Frame width */
        public UInt32 height;       /* Frame height */
        public v4l2_frmivaltypes type;     /* Frame interval type the device supports. */

        //union {					/* Frame interval */
        public v4l2_fract discrete = new v4l2_fract();
        public v4l2_frmival_stepwise stepwise = new v4l2_frmival_stepwise();
        int u_size { get { return Math.Max(discrete.GetSize(), stepwise.GetSize()); } }
        //};

        public UInt32[] reserved = new UInt32[2];			/* Reserved space for future use */

        public override int GetSize()
        {
            return 4 * 5 + u_size;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(index);
                bw.Write(pixel_format);
                bw.Write(width);
                bw.Write(height);
                bw.Write((UInt32)type);
                switch (type)
                {
                    case v4l2_frmivaltypes.V4L2_FRMIVAL_TYPE_DISCRETE:
                        {
                            bw.Write(discrete.Buffer);
                            Fill(u_size - discrete.GetSize());
                            break;
                        }
                    case v4l2_frmivaltypes.V4L2_FRMIVAL_TYPE_CONTINUOUS:
                    case v4l2_frmivaltypes.V4L2_FRMIVAL_TYPE_STEPWISE:
                        {
                            bw.Write(stepwise.Buffer);
                            Fill(u_size - stepwise.GetSize());
                            break;
                        }
                }
                bw.Write(reserved[0]);
                bw.Write(reserved[1]);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                index = br.ReadUInt32();
                pixel_format = br.ReadUInt32();
                width = br.ReadUInt32();
                height = br.ReadUInt32();
                type = (v4l2_frmivaltypes)br.ReadUInt32();
                switch (type)
                {
                    case v4l2_frmivaltypes.V4L2_FRMIVAL_TYPE_DISCRETE:
                        {
                            discrete.Buffer = br.ReadBytes(discrete.GetSize());
                            ms.Position += u_size - discrete.GetSize();
                            break;
                        }
                    case v4l2_frmivaltypes.V4L2_FRMIVAL_TYPE_CONTINUOUS:
                    case v4l2_frmivaltypes.V4L2_FRMIVAL_TYPE_STEPWISE:
                        {
                            stepwise.Buffer = br.ReadBytes(stepwise.GetSize());
                            ms.Position += u_size - stepwise.GetSize();
                            break;
                        }
                }
                reserved[0] = br.ReadUInt32();
                reserved[1] = br.ReadUInt32();
            }
        }
    };

    public class v4l2_timecode : V4L2Struct
    {
        public UInt32 type;
        public UInt32 flags;
        public byte frames;
        public byte seconds;
        public byte minutes;
        public byte hours;
        public byte[] userbits = new byte[4];

        public override int GetSize()
        {
            return 4 * 4;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(type);
                bw.Write(flags);
                bw.Write(frames);
                bw.Write(seconds);
                bw.Write(minutes);
                bw.Write(hours);
                bw.Write(userbits[0]);
                bw.Write(userbits[1]);
                bw.Write(userbits[2]);
                bw.Write(userbits[3]);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                type = br.ReadUInt32();
                flags = br.ReadUInt32();
                frames = br.ReadByte();
                seconds = br.ReadByte();
                minutes = br.ReadByte();
                hours = br.ReadByte();
                userbits[0] = br.ReadByte();
                userbits[1] = br.ReadByte();
                userbits[2] = br.ReadByte();
                userbits[3] = br.ReadByte();
            }
        }
    };

    public class v4l2_jpegcompression : V4L2Struct
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
        public byte[] APP_data = new byte[60];
        /// <summary>
        /// Length of data in JPEG COM segment
        /// </summary>
        public int COM_len;
        /// <summary>
        /// Data in JPEG COM segment
        /// </summary>
        public byte[] COM_data = new byte[60];

        /// <summary>
        /// Which markers should go into the JPEG output. <br/>
		/// Unless you exactly know what you do, leave them untouched. <br/>
		/// Including less markers will make the resulting code smaller, but there will be fewer applications which can read it. <br/>
		/// The presence of the APP and COM marker is influenced by APP_len and COM_len ONLY, not by this property!
        /// </summary>
        public UInt32 jpeg_markers;

        public override int GetSize()
        {
            return 4 * 5 + 120;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(quality);
                bw.Write(APPn);
                bw.Write(APP_len);
                for(int i = 0; i < 60; i++)
                {
                    bw.Write(APP_data[i]);
                }
                bw.Write(COM_len);
                for (int i = 0; i < 60; i++)
                {
                    bw.Write(COM_data[i]);
                }
                bw.Write(jpeg_markers);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                quality = br.ReadInt32();
                APPn = br.ReadInt32();
                APP_len = br.ReadInt32();
                APP_data = br.ReadBytes(60);
                COM_len = br.ReadInt32();
                COM_data = br.ReadBytes(60);
                jpeg_markers = br.ReadUInt32();
            }
        }
    };

    public class v4l2_requestbuffers : V4L2Struct
    {
        public UInt32 count;
        public v4l2_buf_type type;
        public v4l2_memory memory;
        public UInt32 capabilities;
        public byte flags;
        public byte[] reserved = new byte[3];

        public override int GetSize()
        {
            return 4 * 5;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(count);
                bw.Write((UInt32)type);
                bw.Write((UInt32)memory);
                bw.Write(capabilities);
                bw.Write(flags);
                bw.Write(reserved[0]);
                bw.Write(reserved[1]);
                bw.Write(reserved[2]);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                count = br.ReadUInt32();
                type = (v4l2_buf_type)br.ReadUInt32();
                memory = (v4l2_memory)br.ReadUInt32();
                capabilities = br.ReadUInt32();
                flags = br.ReadByte();
                reserved[0] = br.ReadByte();
                reserved[1] = br.ReadByte();
                reserved[2] = br.ReadByte();
            }
        }
    };

    public class v4l2_plane : V4L2Struct
    {
        public UInt32 bytesused;
        public UInt32 length;
        //union {
        MemoryStream u_ms;
        BinaryWriter u_bw;
        BinaryReader u_br;
        public UInt32 mem_offset
        {
            get
            {
                ms.Position = 0;
                return u_br.ReadUInt32();
            }
            set
            {
                ms.Position = 0;
                u_bw.Write(value);
            }
        }
        public UInt64 userptr
        {
            get
            {
                ms.Position = 0;
                return u_br.ReadUInt64();
            }
            set
            {
                ms.Position = 0;
                u_bw.Write(value);
            }
        }
        public Int32 fd
        {
            get
            {
                ms.Position = 0;
                return u_br.ReadInt32();
            }
            set
            {
                ms.Position = 0;
                u_bw.Write(value);
            }
        }
        //} m;
        public UInt32 data_offset;
        public UInt32[] reserved = new UInt32[11];

        public v4l2_plane() : base()
        {
            u_ms = new MemoryStream(8);
            u_bw = new BinaryWriter(u_ms);
            u_br = new BinaryReader(u_ms);
        }

        public override int GetSize()
        {
            return 4 * 16;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(bytesused);
                bw.Write(length);
                u_ms.Position = 0;
                u_ms.CopyTo(ms);
                bw.Write(data_offset);
                for (int i = 0; i < 11; i++)
                {
                    bw.Write(reserved[i]);
                }
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                bytesused = br.ReadUInt32();
                length = br.ReadUInt32();
                u_ms.Position = 0;
                u_ms.Write(br.ReadBytes(8));
                data_offset = br.ReadUInt32();
                for (int i = 0; i < 11; i++)
                {
                    reserved[i] = br.ReadByte();
                }
            }
        }
    };

    public class timeval : V4L2Struct
    {
        public long tv_sec;
        public long tv_usec;

        public override int GetSize()
        {
            return 8 + 8;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(tv_sec);
                bw.Write(tv_usec);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                tv_sec = br.ReadInt64();
                tv_usec = br.ReadInt64();
            }
        }
    }

    public class v4l2_buffer : V4L2Struct
    {
        public UInt32 index;
        public v4l2_buf_type type;
        public UInt32 bytesused;
        public UInt32 flags;
        public UInt32 field;
        public timeval timestamp = new timeval();
        public v4l2_timecode timecode = new v4l2_timecode();
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

        public v4l2_buffer() : base()
        {
            planes_data = Marshal.AllocHGlobal(planes[0].GetSize() * (int)VIDEO_MAX.PLANES);
        }

        ~v4l2_buffer()
        {
            Marshal.FreeHGlobal(planes_data);
        }

        public override int GetSize()
        {
            return 4 * 11 + timestamp.GetSize() + timecode.GetSize();
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(index);
                bw.Write((UInt32)type);
                bw.Write(bytesused);
                bw.Write(flags);
                bw.Write(field);
                bw.Write(timestamp.Buffer);
                bw.Write(timecode.Buffer);
                bw.Write(sequence);
                bw.Write((UInt32)memory);
                if (type == v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE_MPLANE || type == v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_OUTPUT_MPLANE)
                {
                    for (int i =0; i<length; i++)
                    {
                        Marshal.Copy(planes[i].Buffer, 0, planes_data + (planes[i].GetSize() * i), planes[i].GetSize());
                    }
                    bw.Write(planes_data);
                }
                else
                {
                    switch (memory)
                    {
                        case v4l2_memory.V4L2_MEMORY_MMAP:
                            {
                                bw.Write(offset);
                                Fill(4);
                                break;
                            }
                        case v4l2_memory.V4L2_MEMORY_USERPTR:
                            {
                                bw.Write(userptr);
                                break;
                            }
                        case v4l2_memory.V4L2_MEMORY_DMABUF:
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
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                index = br.ReadUInt32();
                type = (v4l2_buf_type)br.ReadUInt32();
                index = br.ReadUInt32();
                bytesused = br.ReadUInt32();
                flags = br.ReadUInt32();
                field = br.ReadUInt32();
                timestamp.Buffer = br.ReadBytes(timestamp.GetSize());
                timecode.Buffer = br.ReadBytes(timecode.GetSize());
                sequence = br.ReadUInt32();
                memory = (v4l2_memory)br.ReadUInt32();
                if (type == v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE_MPLANE || type == v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_OUTPUT_MPLANE)
                {
                    // Is it something to be done?
                }
                else
                {
                    switch(memory)
                    {
                        case v4l2_memory.V4L2_MEMORY_MMAP:
                            {
                                offset = br.ReadUInt32();
                                ms.Position += 4;
                                break;
                            }
                        case v4l2_memory.V4L2_MEMORY_USERPTR:
                            {
                                userptr = br.ReadUInt64();
                                break;
                            }
                        case v4l2_memory.V4L2_MEMORY_DMABUF:
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

                if (type == v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE_MPLANE || type == v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_OUTPUT_MPLANE)
                {
                    // Now we can read planes if needed
                    for (int i = 0; i < length; i++)
                    {
                        byte[] buf = new byte[planes[i].GetSize()];
                        Marshal.Copy(planes_data, buf, 0, planes[i].GetSize());
                        planes[i].Buffer = buf;
                    }
                }
            }
        }
    };

    public class v4l2_exportbuffer : V4L2Struct
    {
        public v4l2_buf_type type;
        public UInt32 index;
        public UInt32 plane;
        public UInt32 flags;
        public Int32 fd;
        public UInt32[] reserved = new UInt32[11];

        public override int GetSize()
        {
            return 4 * 16;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write((UInt32)type);
                bw.Write(index);
                bw.Write(plane);
                bw.Write(flags);
                bw.Write(fd);
                for (int i = 0; i < 11; i++)
                {
                    bw.Write(reserved[i]);
                }
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                type = (v4l2_buf_type)br.ReadUInt32();
                index = br.ReadUInt32();
                plane = br.ReadUInt32();
                flags = br.ReadUInt32();
                fd = br.ReadInt32();
                for (int i = 0; i < 11; i++)
                {
                    reserved[i] = br.ReadUInt32();
                }
            }
        }
    };

    public class v4l2_framebuffer : V4L2Struct
    {
        public UInt32 capability;
        public UInt32 flags;
        /* FIXME: in theory we should pass something like PCI device + memory
         * region + offset instead of some physical address */
        public IntPtr base_ptr; // void*
	    //struct {
            public UInt32 width;
            public UInt32 height;
            public UInt32 pixelformat;
            public v4l2_field field;
            public UInt32 bytesperline; /* for padding, zero if unused */
            public UInt32 sizeimage;
            public v4l2_colorspace colorspace;
            public UInt32 priv;     /* reserved field, set to 0 */
        //   } fmt;

        public override int GetSize()
        {
            return 4 * 12;
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(capability);
                bw.Write(flags);
                bw.Write(base_ptr);
                bw.Write(width);
                bw.Write(height);
                bw.Write(pixelformat);
                bw.Write((UInt32)field);
                bw.Write(bytesperline);
                bw.Write(sizeimage);
                bw.Write((UInt32)colorspace);
                bw.Write(priv);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                capability = br.ReadUInt32();
                flags = br.ReadUInt32();
                base_ptr = (IntPtr)br.ReadInt64();
                width = br.ReadUInt32();
                height = br.ReadUInt32();
                pixelformat = br.ReadUInt32();
                field = (v4l2_field)br.ReadUInt32();
                bytesperline = br.ReadUInt32();
                sizeimage = br.ReadUInt32();
                colorspace = (v4l2_colorspace)br.ReadUInt32();
                priv = br.ReadUInt32();
            }
        }
    };

    //public class v4l2_clip
    //{
    //    v4l2_rect        c;
	   // v4l2_clip    __user* next;
    //};

    //public class v4l2_window
    //{
    //    v4l2_rect        w;
    //    v4l2_field field;
    //    UInt32 chromakey;
    //    v4l2_clip    *clips;
	   // UInt32 clipcount;
    //    UIntPtr bitmap;
	   // byte global_alpha;
    //};

    public class v4l2_captureparm : V4L2Struct
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
        public v4l2_fract timeperframe = new v4l2_fract();
        /// <summary>
        /// Driver-specific extensions
        /// </summary>
	    public UInt32 extendedmode;
        /// <summary>
        /// # of buffers for read
        /// </summary>
        public UInt32 readbuffers;
        public UInt32[] reserved = new UInt32[4];

        public override int GetSize()
        {
            return 4 * 8 + timeperframe.GetSize();
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(capability);
                bw.Write(capturemode);
                bw.Write(timeperframe.Buffer);
                bw.Write(extendedmode);
                bw.Write(readbuffers);
                bw.Write(reserved[0]);
                bw.Write(reserved[1]);
                bw.Write(reserved[2]);
                bw.Write(reserved[3]);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                capability = br.ReadUInt32();
                capturemode = br.ReadUInt32();
                timeperframe.Buffer = br.ReadBytes(timeperframe.GetSize());
                extendedmode = br.ReadUInt32();
                readbuffers = br.ReadUInt32();
                reserved[0] = br.ReadUInt32();
                reserved[1] = br.ReadUInt32();
                reserved[2] = br.ReadUInt32();
                reserved[3] = br.ReadUInt32();
            }
        }
    };

    public class v4l2_outputparm : V4L2Struct
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
        public v4l2_fract timeperframe = new v4l2_fract();
        /// <summary>
        /// Driver-specific extensions
        /// </summary>
        public UInt32 extendedmode;
        /// <summary>
        /// # of buffers for write
        /// </summary>
        public UInt32 writebuffers;
        public UInt32[] reserved = new UInt32[4];

        public override int GetSize()
        {
            return 4 * 8 + timeperframe.GetSize();
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(capability);
                bw.Write(outputmode);
                bw.Write(timeperframe.Buffer);
                bw.Write(extendedmode);
                bw.Write(writebuffers);
                bw.Write(reserved[0]);
                bw.Write(reserved[1]);
                bw.Write(reserved[2]);
                bw.Write(reserved[3]);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                capability = br.ReadUInt32();
                outputmode = br.ReadUInt32();
                timeperframe.Buffer = br.ReadBytes(timeperframe.GetSize());
                extendedmode = br.ReadUInt32();
                writebuffers = br.ReadUInt32();
                reserved[0] = br.ReadUInt32();
                reserved[1] = br.ReadUInt32();
                reserved[2] = br.ReadUInt32();
                reserved[3] = br.ReadUInt32();
            }
        }
    };

    public class v4l2_cropcap : V4L2Struct
    {
        public v4l2_buf_type type;
        public v4l2_rect bounds = new v4l2_rect();
        public v4l2_rect defrect = new v4l2_rect();
        public v4l2_fract pixelaspect = new v4l2_fract();

        public override int GetSize()
        {
            return 4 + (bounds.GetSize() * 2) + pixelaspect.GetSize();
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write((UInt32)type);
                bw.Write(bounds.Buffer);
                bw.Write(defrect.Buffer);
                bw.Write(pixelaspect.Buffer);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                type = (v4l2_buf_type)br.ReadUInt32();
                bounds.Buffer = br.ReadBytes(bounds.GetSize());
                defrect.Buffer = br.ReadBytes(defrect.GetSize());
                pixelaspect.Buffer = br.ReadBytes(pixelaspect.GetSize());
            }
        }
    };

    public class v4l2_crop : V4L2Struct
    {
        public v4l2_buf_type type;
        public v4l2_rect c = new v4l2_rect();

        public override int GetSize()
        {
            return 4 + c.GetSize();
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write((UInt32)type);
                bw.Write(c.Buffer);
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                type = (v4l2_buf_type)br.ReadUInt32();
                c.Buffer = br.ReadBytes(c.GetSize());
            }
        }
    };

    public class v4l2_selection : V4L2Struct
    {
        public v4l2_buf_type type;
        public UInt32 target;
        public UInt32 flags;
        public v4l2_rect r = new v4l2_rect();
        public UInt32[] reserved = new UInt32[9];

        public override int GetSize()
        {
            return 4 * 12 + r.GetSize();
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write((UInt32)type);
                bw.Write(target);
                bw.Write(flags);
                bw.Write(r.Buffer);
                for(int i=0; i< 9;i++)
                {
                    bw.Write(reserved[i]);
                }
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                type = (v4l2_buf_type)br.ReadUInt32();
                target = br.ReadUInt32();
                flags = br.ReadUInt32();
                r.Buffer = br.ReadBytes(r.GetSize());
                for (int i=0; i<9; i++)
                {
                    reserved[i] = br.ReadUInt32();
                }
            }
        }
    };

    public class v4l2_standard : V4L2Struct
    {
        public UInt32 index;
        public UInt64 id;
        public byte[] name = new byte[24];
        /// <summary>
        /// Frames, not fields
        /// </summary>
        public v4l2_fract frameperiod = new v4l2_fract();
        public UInt32 framelines;
        public UInt32[] reserved = new UInt32[4];

        public override int GetSize()
        {
            return 4 * 8 + 24 + frameperiod.GetSize();
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(index);
                bw.Write(id);
                for(int i=0; i< 24;i++)
                {
                    bw.Write(name[i]);
                }
                bw.Write(frameperiod.Buffer);
                bw.Write(framelines);
                for (int i = 0; i < 4; i++)
                {
                    bw.Write(reserved[i]);
                }
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                index = br.ReadUInt32();
                id = br.ReadUInt64();
                for (int i = 0; i < 24; i++)
                {
                    name[i] = br.ReadByte();
                }
                frameperiod.Buffer = br.ReadBytes(frameperiod.GetSize());
                framelines = br.ReadUInt32();
                for (int i = 0; i < 4; i++)
                {
                    reserved[i] = br.ReadUInt32();
                }
            }
        }
    };

    public class v4l2_bt_timings : V4L2Struct
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
        public v4l2_fract picture_aspect = new v4l2_fract();
        public byte cea861_vic;
        public byte hdmi_vic;
        public byte[] reserved = new byte[46];

        public override int GetSize()
        {
            return 4 * 17 + 48 + picture_aspect.GetSize();
        }

        public override byte[] Buffer
        {
            get
            {
                ms.SetLength(0);
                bw.Write(width);
                bw.Write(height);
                bw.Write(interlaced);
                bw.Write(polarities);
                bw.Write(pixelclock);
                bw.Write(hfrontporch);
                bw.Write(hsync);
                bw.Write(hbackporch);
                bw.Write(vfrontporch);
                bw.Write(vsync);
                bw.Write(vbackporch);
                bw.Write(il_vfrontporch);
                bw.Write(il_vsync);
                bw.Write(il_vbackporch);
                bw.Write(standards);
                bw.Write(flags);
                bw.Write(picture_aspect.Buffer);
                bw.Write(cea861_vic);
                bw.Write(hdmi_vic);
                for (int i = 0; i < 46; i++)
                {
                    bw.Write(reserved[i]);
                }
                return ms.ToArray();
            }
            set
            {
                ms.SetLength(0);
                ms.Write(value);
                ms.Position = 0;
                width = br.ReadUInt32();
                height = br.ReadUInt32();
                interlaced = br.ReadUInt32();
                polarities = br.ReadUInt32();
                pixelclock = br.ReadUInt64();
                hfrontporch = br.ReadUInt32();
                hsync = br.ReadUInt32();
                hbackporch = br.ReadUInt32();
                vfrontporch = br.ReadUInt32();
                vsync = br.ReadUInt32();
                vbackporch = br.ReadUInt32();
                il_vfrontporch = br.ReadUInt32();
                il_vsync = br.ReadUInt32();
                il_vbackporch = br.ReadUInt32();
                standards = br.ReadUInt32();
                flags = br.ReadUInt32();
                picture_aspect.Buffer = br.ReadBytes(picture_aspect.GetSize());
                cea861_vic = br.ReadByte();
                hdmi_vic = br.ReadByte();
                for (int i = 0; i < 46; i++)
                {
                    reserved[i] = br.ReadByte();
                }
            }
        }
    } //__attribute__((packed));

    public class v4l2_dv_timings
    {
        UInt32 type;
        //union {
		v4l2_bt_timings  bt;
		UInt32[] reserved = new UInt32[32];
        //};
    } //__attribute__((packed));

    public class v4l2_enum_dv_timings
    {
        UInt32 index;
        UInt32 pad;
        UInt32[] reserved = new UInt32[2];
        v4l2_dv_timings timings;
    };

    public class v4l2_bt_timings_cap
    {
        UInt32 min_width;
        UInt32 max_width;
        UInt32 min_height;
        UInt32 max_height;
        UInt64 min_pixelclock;
        UInt64 max_pixelclock;
        UInt32 standards;
        UInt32 capabilities;
        UInt32[] reserved = new UInt32[16];
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
    };

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
    };

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
    };

    public class v4l2_control
    {
        UInt32 id;
        Int32 value;
    };

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
    };

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
        UInt32[] dims = new  UInt32[(int)V4L2_CTRL.MAX_DIMS];
        UInt32[] reserved = new UInt32[32];
    };

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
    };

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
    };

    public class v4l2_frequency
    {
        UInt32 tuner;
        v4l2_tuner_type type;
        UInt32 frequency;
        UInt32[] reserved = new UInt32[8];
    };

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
    };

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
    };

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
    };

    public class v4l2_audioout
    {
        UInt32 index;
        byte[] name = new byte[32];
        UInt32 capability;
        UInt32 mode;
        UInt32[] reserved = new UInt32[2];
    };

    public class v4l2_enc_idx_entry
    {
        UInt64 offset;
        UInt64 pts;
        UInt32 length;
        UInt32 flags;
        UInt32[] reserved = new UInt32[2];
    };

    public class v4l2_enc_idx
    {
        UInt32 entries;
        UInt32 entries_cap;
        UInt32[] reserved = new UInt32[4];
        v4l2_enc_idx_entry[] entry = new v4l2_enc_idx_entry[(int)V4L2_ENC_IDX.ENTRIES];
    };

    public class v4l2_encoder_cmd
    {
        UInt32 cmd;
        UInt32 flags;
        UInt32[] raw_data = new UInt32[8];
    };

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
    };

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
    };

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
    };

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
    };

    public class v4l2_sliced_vbi_data
    {
        UInt32 id;
        UInt32 field;          /* 0: first field, 1: second field */
        UInt32 line;           /* 1-23 */
        UInt32 reserved;       /* must be 0 */
        byte[] data = new byte[48];
    };

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

    public class v4l2_plane_pix_format
    {
        UInt32 sizeimage;
        UInt32 bytesperline;
        UInt16[] reserved = new UInt16[6];
    } //__attribute__((packed));

    public class v4l2_pix_format_mplane
    {
        UInt32 width;
        UInt32 height;
        UInt32 pixelformat;
        UInt32 field;
        UInt32 colorspace;

        v4l2_plane_pix_format[] plane_fmt = new v4l2_plane_pix_format[(int)VIDEO_MAX.PLANES];
	    byte num_planes;
        byte flags;
        //union {
		    byte ycbcr_enc;
            byte hsv_enc;
        //};
        byte quantization;
        byte xfer_func;
        byte[] reserved = new byte[7];
    } //__attribute__((packed));

    public class v4l2_sdr_format
    {
        UInt32 pixelformat;
        UInt32 buffersize;
        byte[] reserved = new byte[24];
    } //__attribute__((packed));

    public class v4l2_meta_format
    {
        UInt32 dataformat;
        UInt32 buffersize;
    } //__attribute__((packed));

    //public class v4l2_format
    //{
    //    UInt32 type;
    //    //union {
    //        v4l2_pix_format      pix;     /* V4L2_BUF_TYPE_VIDEO_CAPTURE */
    //        v4l2_pix_format_mplane   pix_mp;  /* V4L2_BUF_TYPE_VIDEO_CAPTURE_MPLANE */
    //        v4l2_window      win;     /* V4L2_BUF_TYPE_VIDEO_OVERLAY */
		  //  v4l2_vbi_format      vbi;     /* V4L2_BUF_TYPE_VBI_CAPTURE */
		  //  v4l2_sliced_vbi_format   sliced;  /* V4L2_BUF_TYPE_SLICED_VBI_CAPTURE */
		  //  v4l2_sdr_format      sdr;     /* V4L2_BUF_TYPE_SDR_CAPTURE */
		  //  v4l2_meta_format     meta;    /* V4L2_BUF_TYPE_META_CAPTURE */
		  //  byte[] raw_data = new byte[200];                   /* user-defined */
    //    //} fmt;
    //};

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