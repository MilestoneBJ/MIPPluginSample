using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using HelloWorldMIPPlugin.Admin;
using HelloWorldMIPPlugin.Background;
using HelloWorldMIPPlugin.Client;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace VideoOS.CustomDevelopment.HelloWorldMIPPlugin
{
    /// <summary>
    /// The PluginDefinition is the ‘entry’ point to any plugin.  New plug-ins must be added here.
    /// This is the starting point for any plugin development and the class MUST be available for a plugin to be loaded.  
    /// Several PluginDefinitions are allowed to be available within one DLL.
    /// Here the references to all other plugin known objects and classes are defined.
    /// The class is an abstract class where all implemented methods and properties need to be declared with override.
    /// The class is constructed when the environment is loading the DLL.
    /// </summary>
    public class HelloWorldMIPPluginDefinition : PluginDefinition
    {
        private static System.Drawing.Image _treeNodeImage;
        private static System.Drawing.Image _topTreeNodeImage;

        internal static Guid HelloWorldMIPPluginPluginId = new Guid("a52d7206-46b3-430c-94cf-907a6901eae5"); // changed in development-V
        internal static Guid HelloWorldMIPPluginKind = new Guid("a4f1045c-f79d-4304-814d-2df30da32083"); // Changed by development-C
        internal static Guid HelloWorldMIPPluginSidePanel = new Guid("dd15bc38-d46d-4ef5-b05a-ea31ac90c7ee");
        internal static Guid HelloWorldMIPPluginViewItemPlugin = new Guid("ebad386d-0759-40aa-b754-5540ad0ed36f");
        internal static Guid HelloWorldMIPPluginOptionsDialog = new Guid("3a993897-a4da-4517-ae35-0070062b6c41");
        internal static Guid HelloWorldMIPPluginBackgroundPlugin = new Guid("83c7d018-5b40-4a8f-b272-575e55038ffb");
        internal static Guid HelloWorldMIPPluginWorkSpacePluginId = new Guid("ef973a5c-7b2a-434f-8bb2-b53293c280b4");
        internal static Guid HelloWorldMIPPluginWorkSpaceViewItemPluginId = new Guid("c7206cd1-0310-4db6-8f72-302a64b5e8dd");


        #region Private fields

        private UserControl _treeNodeInofUserControl;

        //
        // Note that all the plugin are constructed during application start, and the constructors
        // should only contain code that references their own dll, e.g. resource load.

        private List<BackgroundPlugin> _backgroundPlugins = new List<BackgroundPlugin>();
        private List<OptionsDialogPlugin> _optionsDialogPlugins = new List<OptionsDialogPlugin>();
        private List<ViewItemPlugin> _viewItemPlugin = new List<ViewItemPlugin>();
        private List<ItemNode> _itemNodes = new List<ItemNode>();
        private List<SidePanelPlugin> _sidePanelPlugins = new List<SidePanelPlugin>();
        private List<String> _messageIdStrings = new List<string>();
        private List<SecurityAction> _securityActions = new List<SecurityAction>();
        private List<WorkSpacePlugin> _workSpacePlugins = new List<WorkSpacePlugin>();

        #endregion

        #region Initialization

        /// <summary>
        /// Load resources 
        /// </summary>
        static HelloWorldMIPPluginDefinition()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string name = assembly.GetName().Name;

            System.IO.Stream pluginStream = assembly.GetManifestResourceStream(name + ".Resources.HelloWorldMIPPlugin.bmp");
            if (pluginStream != null)
                _treeNodeImage = System.Drawing.Image.FromStream(pluginStream);
            System.IO.Stream configStream = assembly.GetManifestResourceStream(name + ".Resources.Server.png");
            if (configStream != null)
                _topTreeNodeImage = System.Drawing.Image.FromStream(configStream);
        }


        /// <summary>
        /// Get the icon for the plugin
        /// </summary>
        internal static Image TreeNodeImage
        {
            get { return _treeNodeImage; }
        }

        #endregion

        /// <summary>
        /// This method is called when the environment is up and running.
        /// Registration of Messages via RegisterReceiver can be done at this point.
        /// </summary>
        public override void Init()
        {
            // Populate all relevant lists with your plugins etc.
            _itemNodes.Add(new ItemNode(HelloWorldMIPPluginKind, Guid.Empty,
                                         "HelloWorldMIPPlugin", _treeNodeImage,
                                         "HelloWorldMIPPlugins", _treeNodeImage,
                                         Category.Text, true,
                                         ItemsAllowed.Many,
                                         new HelloWorldMIPPluginItemManager(HelloWorldMIPPluginKind),
                                         null
                                         ));
            if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
            {
                _workSpacePlugins.Add(new HelloWorldMIPPluginWorkSpacePlugin());
                _sidePanelPlugins.Add(new HelloWorldMIPPluginSidePanelPlugin());
                _viewItemPlugin.Add(new HelloWorldMIPPluginViewItemPlugin());
                _viewItemPlugin.Add(new HelloWorldMIPPluginWorkSpaceViewItemPlugin());
            }

            _optionsDialogPlugins.Add(new HelloWorldMIPPluginOptionsDialogPlugin());
            _backgroundPlugins.Add(new HelloWorldMIPPluginBackgroundPlugin());
        }

        /// <summary>
        /// The main application is about to be in an undetermined state, either logging off or exiting.
        /// You can release resources at this point, it should match what you acquired during Init, so additional call to Init() will work.
        /// </summary>
        public override void Close()
        {
            _itemNodes.Clear();
            _sidePanelPlugins.Clear();
            _viewItemPlugin.Clear();
            _optionsDialogPlugins.Clear();
            _backgroundPlugins.Clear();
            _workSpacePlugins.Clear();
        }
        /// <summary>
        /// Return any new messages that this plugin can use in SendMessage or PostMessage,
        /// or has a Receiver set up to listen for.
        /// The suggested format is: "YourCompany.Area.MessageId"
        /// </summary>
        public override List<string> PluginDefinedMessageIds
        {
            get
            {
                return _messageIdStrings;
            }
        }

        /// <summary>
        /// If authorization is to be used, add the SecurityActions the entire plugin 
        /// would like to be available.  E.g. Application level authorization.
        /// </summary>
        public override List<SecurityAction> SecurityActions
        {
            get
            {
                return _securityActions;
            }
            set
            {
            }
        }

        #region Identification Properties

        /// <summary>
        /// Gets the unique id identifying this plugin component
        /// </summary>
        public override Guid Id
        {
            get
            {
                return HelloWorldMIPPluginPluginId;
            }
        }

        /// <summary>
        /// This Guid can be defined on several different IPluginDefinitions with the same value,
        /// and will result in a combination of this top level ProductNode for several plugins.
        /// Set to Guid.Empty if no sharing is enabled.
        /// </summary>
        public override Guid SharedNodeId
        {
            get
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Define name of top level Tree node - e.g. A product name
        /// </summary>
        public override string Name
        {
            get { return "HelloWorldMIPPlugin"; }
        }

        /// <summary>
        /// Your company name
        /// </summary>
        public override string Manufacturer
        {
            get
            {
                return "Your Company name";
            }
        }

        /// <summary>
        /// Version of this plugin.
        /// </summary>
        public override string VersionString
        {
            get
            {
                return "1.0.0.0";
            }
        }

        /// <summary>
        /// Icon to be used on top level - e.g. a product or company logo
        /// </summary>
        public override System.Drawing.Image Icon
        {
            get { return _topTreeNodeImage; }
        }

        #endregion


        #region Administration properties

        /// <summary>
        /// A list of server side configuration items in the administrator
        /// </summary>
        public override List<ItemNode> ItemNodes
        {
            get { return _itemNodes; }
        }

        /// <summary>
        /// A user control to display when the administrator clicks on the top TreeNode
        /// </summary>
        public override UserControl GenerateUserControl()
        {
            _treeNodeInofUserControl = new HelpPage();
            return _treeNodeInofUserControl;
        }

        /// <summary>
        /// This property can be set to true, to be able to display your own help UserControl on the entire panel.
        /// When this is false - a standard top and left side is added by the system.
        /// </summary>
        public override bool UserControlFillEntirePanel
        {
            get { return false; }
        }
        #endregion

        #region Client related methods and properties

        /// <summary>
        /// A list of Client side definitions for Smart Client
        /// </summary>
        public override List<ViewItemPlugin> ViewItemPlugins
        {
            get { return _viewItemPlugin; }
        }

        /// <summary>
        /// An extension plugin running in the Smart Client to add more choices on the Options dialog.
        /// </summary>
        public override List<OptionsDialogPlugin> OptionsDialogPlugins
        {
            get { return _optionsDialogPlugins; }
        }

        /// <summary> 
        /// An extension plugin to add to the side panel of the Smart Client.
        /// </summary>
        public override List<SidePanelPlugin> SidePanelPlugins
        {
            get { return _sidePanelPlugins; }
        }

        /// <summary>
        /// Return the workspace plugins
        /// </summary>
        public override List<WorkSpacePlugin> WorkSpacePlugins
        {
            get { return _workSpacePlugins; }
        }
        #endregion


        /// <summary>
        /// Create and returns the background task.
        /// </summary>
        public override List<VideoOS.Platform.Background.BackgroundPlugin> BackgroundPlugins
        {
            get { return _backgroundPlugins; }
        }

    }
}
