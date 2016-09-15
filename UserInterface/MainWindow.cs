using System;
using System.Windows.Forms;
using CNCSpecific;
using CNCSpecific.Milling;
using ComponentFactory.Krypton.Docking;
using ComponentFactory.Krypton.Navigator;
using ComponentFactory.Krypton.Toolkit;
using Model;
using RenderEngine.Rendering.Scene;
using Shared.Geometry;
using Shared.Helper;
using Shared.Import;
using Message = MessageHandling.Message;

namespace UserInterface
{
    public partial class MainWindow : KryptonForm, IObserver
    {
        private MeshModel _meshModel;
        OpenTkControl _openTkControl = new OpenTkControl();
        private GCodeEditor _gCodeEditor;
        private GCodeOutput _gCodeOutput;

        public MainWindow()
        {
            InitializeComponent();

            _gCodeOutput = new GCodeOutput();
            _gCodeEditor = new GCodeEditor(_gCodeOutput);

            _meshModel = new MeshModel();
            _meshModel.AttachModelObserver(SceneModel.Instance);
            SubtractionModel.Instance.AttachModelObserver(SceneModel.Instance);
            _meshModel.AttachModelObserver(SubtractionModel.Instance);
        }

        private void CreateBasicTool()
        {
            var meshes = FileHelper.LoadFileFromDropbox(@"\BooleanOpEnv\Blender\Collada_Files\CNC_Milling\Cylinder1.dae");
            _meshModel.AddTool(meshes[0]);
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            // Setup docking functionality
            KryptonDockingWorkspace w = kryptonDockingManager1.ManageWorkspace(kryptonDockableWorkspace1);
            kryptonDockingManager1.ManageControl(kryptonPanel1, w);
            kryptonDockingManager1.ManageFloating(this);

            // Add initial docking pages
            kryptonDockingManager1.AddToWorkspace("Workspace", new KryptonPage[] { NewDocument()});
            kryptonDockingManager1.AddDockspace("Control", DockingEdge.Right, new KryptonPage[] { NewInput("G-Code", _gCodeEditor), NewInput("Hints", new GCodeOutput()), NewInput("Errors", new GCodeOutput()), NewInput("Tools", new GCodeOutput()), NewInput("Macros", new GCodeOutput()), NewInput("Info", new GCodeOutput()) });

            kryptonDockingManager1.AddDockspace("Control", DockingEdge.Bottom, new KryptonPage[] { NewInput("G-Code Info", _gCodeOutput), NewInput("Output", new GCodeOutput()) });
            CreateBasicTool();

        }

        private KryptonPage NewInput(String text, UserControl content)
        {
            return NewPage(text, content);
        }
        
        private KryptonPage NewDocument()
        {
            KryptonPage page = NewPage("Document ", _openTkControl);

            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);

            return page;
        }

        private KryptonPage NewPage(string name, Control content)
        {
            // Create new page with title and image
            KryptonPage p = new KryptonPage();
            p.Text = name ;
            p.TextTitle = name ;
            p.TextDescription = name ;
            //p.ImageSmall = imageListSmall.Images[image];

            // Add the control for display inside the page
            content.Dock = DockStyle.Fill;
            p.Controls.Add(content);


            return p;
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Collada (.dae)|*.dae";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            DialogResult result = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (result == DialogResult.OK)
            {
                try
                {
                    MeshImporter m = new MeshImporter();
                    var meshes = m.GenerateMeshes(openFileDialog1.FileName);
                    ((MeshModel) _meshModel).ClearMeshList();
                    ((MeshModel)_meshModel).AddRoughParts(meshes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void Notified(AbstractModel abstractModel, Message m)
        {
            
        }

        private void BuildButton_Click(object sender, EventArgs e)
        {
            SubtractionModel.Instance.BuildSnapshotList();
        }

        private void ImportTool_Click(object sender, EventArgs e)
        {

        }

        private void kryptonRibbonGroupButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files|*.txt";

            // Call the ShowDialog method to show the dialog box.
            DialogResult userClickedOK = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == DialogResult.OK)
            {
                GCodeParser parser = new GCodeParser();
                parser.Import(openFileDialog1.FileName);
                parser.Parse();
                _gCodeEditor.SetCode(parser.CodeString);

                //SubtractionModel.Instance.NCProgram = parser.NCProgram;
                SubtractionModel.Instance.NCProgram = TestProgram();
            }
        }

        private NCProgram TestProgram()
        {
            var program = new NCProgram();
            program.AddPath(250, 0, 180);
            program.AddPath(0, -150, 0);
            program.AddPath(-450, 0, 0);
            program.AddPath(0, 0, -400);
            program.AddPath(380, 0, 0);
            program.AddPath(0, 0, 400);
            program.AddPath(-200, 0, -200);
            program.AddPath(200, 0, -200);
            program.AddPath(-200, 0, 2000);
            program.AddPath(0, -30, 0);
            program.AddPath(0, 60, 0);
            program.AddPath(30, 0, 30);
            program.AddPath(0, -60, 0);
            program.AddPath(0, 60, 0);
            program.AddPath(-100, 0, -100);
            program.AddPath(0, -60, 0);
            program.AddPath(0, 60, 0);
            program.AddPath(0, 0, 100);
            program.AddPath(0, -60, 0);
            return program;
        }

        private void kryptonRibbonGroupButton2_Click(object sender, EventArgs e)
        {
            RoughPartSpecDialog d = new RoughPartSpecDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                //_meshModel.GenerateBox(d.X, d.Y, d.Z);
                _meshModel.GenerateBox(200, 40, 200);
            }
        }
    }
}
