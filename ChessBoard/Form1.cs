using SlimDX;
using SlimDX.Direct3D9;

namespace ChessBoard
{
    public partial class Form1 : Form
    {
        private Direct3D m_oD3D;
        private Device m_oD3D_Device;
        private PresentParameters m_d3dpp;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!InitializeDirect3D())
            {
                MessageBox.Show("Failed Loading DirectX", "Error");
                Application.Exit();
                return;
            }
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (m_oD3D == null) return;
            if (m_oD3D_Device == null) return;
            if (ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                m_d3dpp.BackBufferWidth = ClientSize.Width;
                m_d3dpp.BackBufferHeight = ClientSize.Height;
                m_oD3D_Device.Reset(m_d3dpp);
            }
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            RenderScene();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DestroyDirect3D();
        }

        private bool InitializeDirect3D()
        {
            bool bResult = false;
            m_d3dpp = new PresentParameters();
            m_d3dpp.SwapEffect = SwapEffect.Discard;
            m_d3dpp.Windowed = true;
            m_d3dpp.EnableAutoDepthStencil = false;
            try
            {

                m_oD3D = new Direct3D();
                var nAdapter = m_oD3D.Adapters.DefaultAdapter.Adapter;
                var d3ddm = m_oD3D.GetAdapterDisplayMode(nAdapter);
                m_d3dpp.BackBufferFormat = d3ddm.Format;
                m_d3dpp.BackBufferWidth = ClientSize.Width;
                m_d3dpp.BackBufferHeight = ClientSize.Height;
                m_oD3D_Device = new Device
                (
                    m_oD3D, nAdapter,
                    DeviceType.Hardware,
                    Handle,
                    CreateFlags.SoftwareVertexProcessing,
                    m_d3dpp
                );
                bResult = !(m_oD3D_Device is null);
            }
            catch
            {
                bResult = false;
            }
            return bResult;
        }

        private void DestroyDirect3D()
        {
            if (m_oD3D_Device != null)
                m_oD3D_Device.Dispose();
            if (m_oD3D != null)
                m_oD3D.Dispose();
        }



        private void RenderScene()
        {
            int k = 0;
            int width = ClientSize.Width / 8;
            int height = ClientSize.Height / 8;
            Color4 d3dColor1 = new Color4(Color.Aqua);
            Color4 d3dColor2 = new Color4(Color.Brown);
            Rectangle[] rects = new Rectangle[32];
            if (m_oD3D is null) return;
            if (m_oD3D_Device is null) return;
            Result d3dResult = m_oD3D_Device.TestCooperativeLevel();
            if (d3dResult == ResultCode.DeviceLost) return;
            if (d3dResult == ResultCode.DeviceNotReset)
            {
                m_oD3D_Device.Reset(m_d3dpp);
                Invalidate();
            }
            m_oD3D_Device.Clear(ClearFlags.Target, d3dColor1, 0.0f, 0);
            for (int i = 0; i < 8; i++)
            {
                for(int j = i % 2; j < 8; j += 2)
                {
                    int left = i * width;
                    int top = j * height;
                    rects[k] = new Rectangle(left, top, width, height);
                    k++;
                }
            }
            m_oD3D_Device.Clear(ClearFlags.Target, d3dColor2, 0.0f, 0, rects);
            m_oD3D_Device.BeginScene();
            m_oD3D_Device.EndScene();
            m_oD3D_Device.Present();
        }
    }
}