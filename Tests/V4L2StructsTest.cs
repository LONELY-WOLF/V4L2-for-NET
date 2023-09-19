using System.Numerics;
using V4L2_for_NET;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public unsafe class V4L2StructsTest
    {
        IntPtr data_ptr;
        byte* byte_ptr;

        public V4L2StructsTest()
        {
            // Prealloc 1M of memory
            data_ptr = Marshal.AllocHGlobal(1_000_000);
            byte_ptr = (byte*)data_ptr.ToPointer();
        }

        ~V4L2StructsTest()
        {
            Marshal.FreeHGlobal(data_ptr);
        }

        void V4L2StructSizeISCorrect(V4L2Struct s)
        {
            s.GetPointer();
            Assert.AreEqual(s.GetSize(), s.GetStreamPosition());
            s.UpdateFromUnmanaged();
            //Assert.AreEqual(V4L2Struct.NativeSize, s.GetSize());
            Assert.AreEqual(s.GetSize(), s.GetStreamPosition());
        }

        [TestMethod]
        public void v4l2_rect_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_rect(byte_ptr));
        }

        [TestMethod]
        public void v4l2_fract_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_fract(byte_ptr));
        }

        [TestMethod]
        public void v4l2_area_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_area());
        }

        [TestMethod]
        public void v4l2_capability_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_capability());
        }

        [TestMethod]
        public void v4l2_pix_format_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_pix_format(byte_ptr));
        }

        [TestMethod]
        public void v4l2_fmtdesc_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_fmtdesc());
        }

        [TestMethod]
        public void v4l2_frmsize_discrete_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_frmsize_discrete(byte_ptr));
        }

        [TestMethod]
        public void v4l2_frmsize_stepwise_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_frmsize_stepwise(byte_ptr));
        }

        [TestMethod]
        public void v4l2_frmsizeenum_SizeIsCorrect()
        {
            v4l2_frmsizeenum f = new v4l2_frmsizeenum();
            f.type = v4l2_frmsizetypes.DISCRETE;
            V4L2StructSizeISCorrect(f);
        }

        [TestMethod]
        public void v4l2_frmival_stepwise_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_frmival_stepwise(byte_ptr));
        }

        [TestMethod]
        public void v4l2_frmivalenum_SizeIsCorrect()
        {
            v4l2_frmivalenum f = new v4l2_frmivalenum();
            f.type = v4l2_frmivaltypes.DISCRETE;
            V4L2StructSizeISCorrect(new v4l2_frmivalenum());
        }

        [TestMethod]
        public void v4l2_timecode_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_timecode(byte_ptr));
        }

        [TestMethod]
        public void v4l2_jpegcompression_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_jpegcompression());
        }

        [TestMethod]
        public void v4l2_requestbuffers_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_requestbuffers());
        }

        [TestMethod]
        public void v4l2_plane_SizeIsCorrect()
        {
            v4l2_plane p = new v4l2_plane(byte_ptr);
            p.length = 0x11112222;
            p.userptr = 0x123456789ABCDEF;
            V4L2StructSizeISCorrect(p);
        }

        [TestMethod]
        public void timeval_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new timeval(byte_ptr));
        }

        [TestMethod]
        public void v4l2_buffer_SizeIsCorrect()
        {
            v4l2_buffer buf = new v4l2_buffer();
            buf.type = v4l2_buf_type.VIDEO_CAPTURE;
            buf.memory = v4l2_memory.USERPTR;
            V4L2StructSizeISCorrect(buf);
        }

        [TestMethod]
        public void v4l2_exportbuffer_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_exportbuffer());
        }

        [TestMethod]
        public void v4l2_framebuffer_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_framebuffer());
        }

        [TestMethod]
        public void v4l2_clip_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_clip(byte_ptr));
        }

        [TestMethod]
        public void v4l2_window_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_window(byte_ptr));
        }

        [TestMethod]
        public void v4l2_captureparm_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_captureparm());
        }

        [TestMethod]
        public void v4l2_outputparm_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_outputparm());
        }

        [TestMethod]
        public void v4l2_cropcap_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_cropcap());
        }

        [TestMethod]
        public void v4l2_crop_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_crop());
        }

        [TestMethod]
        public void v4l2_selection_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_selection());
        }

        [TestMethod]
        public void v4l2_standard_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_standard());
        }

        [TestMethod]
        public void v4l2_bt_timings_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_bt_timings());
        }

        [TestMethod]
        public void v4l2_plane_pix_format_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_plane_pix_format(byte_ptr));
        }

        [TestMethod]
        public void v4l2_pix_format_mplane_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_pix_format_mplane(byte_ptr));
        }

        [TestMethod]
        public void v4l2_meta_format_SizeIsCorrect()
        {
            V4L2StructSizeISCorrect(new v4l2_meta_format(byte_ptr));
        }

        [TestMethod]
        public void v4l2_format_SizeIsCorrect()
        {
            v4l2_format f = new v4l2_format();
            f.type = v4l2_buf_type.META_CAPTURE;
            V4L2StructSizeISCorrect(f);
        }
    }
}