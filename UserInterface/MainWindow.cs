using System;
using System.Windows.Forms;
using ComponentFactory.Krypton.Docking;
using ComponentFactory.Krypton.Navigator;
using ComponentFactory.Krypton.Toolkit;
using DataManagement;
using Model;
using RenderEngine;
using RenderEngine.Scene;
using Message = MessageHandling.Message;

namespace UserInterface
{
    public partial class MainWindow : KryptonForm, IObserver
    {
        private AbstractModel _meshModel;
        private SubtractionModel _subtractionModel;
        OpenTkControl _openTkControl = new OpenTkControl();
        public MainWindow()
        {
            InitializeComponent();
            _meshModel = new MeshModel();
            _subtractionModel = new SubtractionModel();
            _meshModel.AttachModelObserver(SceneModel.Instance);
            _subtractionModel.AttachModelObserver(SceneModel.Instance);
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            // Setup docking functionality
            KryptonDockingWorkspace w = kryptonDockingManager1.ManageWorkspace(kryptonDockableWorkspace1);
            kryptonDockingManager1.ManageControl(kryptonPanel1, w);
            kryptonDockingManager1.ManageFloating(this);

            // Add initial docking pages
            kryptonDockingManager1.AddToWorkspace("Workspace", new KryptonPage[] { NewDocument()});
        }

        private KryptonPage NewDocument()
        {
            KryptonPage page = NewPage("Document ", 0, _openTkControl);

            // Document pages cannot be docked or auto hidden
            page.ClearFlags(KryptonPageFlags.DockingAllowAutoHidden | KryptonPageFlags.DockingAllowDocked);

            return page;
        }

        private KryptonPage NewPage(string name, int image, Control content)
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
            _subtractionModel.BuildSnapshotList();
        }

        private void ImportTool_Click(object sender, EventArgs e)
        {

        }
    }
}
