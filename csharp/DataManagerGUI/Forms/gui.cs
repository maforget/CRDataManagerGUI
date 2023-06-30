using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DataManagerGUI
{
    public class gui : Form
    {
        #region Handy Fields and Properties
        string iniFile
        {
            get
            { return this.startupPath + "dataman.ini"; }
        }
        string userFile
        {
            get
            { return this.startupPath + "user.ini"; }
        }
        string ruleFile
        {
            get
            { return this.startupPath + "dataman.dat"; }
        }
        string templatesFile
        {
            get
            { return this.startupPath + "templates.xml"; }
        }
        string _currentFile;
        string startupPath;
        string currentRuleFile
        {
            get
            { return _currentFile; }
            set
            {
                _currentFile = value;
                if (value == "")
                {
                    tsslCurrentFile.Text = "New File";
                    tsslLastSave.Text = "Never";
                }
                else if (File.Exists(_currentFile))
                {
                    FileInfo tmp = new FileInfo(_currentFile);
                    tsslCurrentFile.Text = "Current File: " + _currentFile;
                    tsslLastSave.Text = "Last Saved: " + tmp.LastWriteTime.ToString("yyyy/MM/dd hh:mm");
                }
                else
                {
                    tsslLastSave.Text = "Never";
                    tsslCurrentFile.Text = "Current File: " + _currentFile;
                }
            }
        }
        string currentTime
        {
            get
            {
                return "Last Saved: " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
            }
        }

        public bool FileChangedSinceSearch { get; set; }

        public bool FileChanged
        {
            get
            {
                return _fileIsChanged;
            }
            set
            {
                _fileIsChanged = value;
                if (!FileChangedSinceSearch)
                    FileChangedSinceSearch = true;
                if (value)
                    tsslChanged.Text = "Profile Changed: Yes";
                else
                    tsslChanged.Text = "Profile Changed: No";
                //TODO: Disable future save button.
                tsbSave.Enabled = _fileIsChanged;
            }
        }

        private bool _fileIsChanged;

        string RecovoryFolderPath
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "Data Manager" + Path.DirectorySeparatorChar.ToString();
            }
        }
        string GUIVersion
        {
            get
            {
                string strReturn = "";
                string[] versionInfo = Application.ProductVersion.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < 3; i++)
                    strReturn += versionInfo[i] + ".";
                strReturn += "r" + versionInfo[3];
                return strReturn;
            }
        }
        public dmCollection Collection { get; set; }
        const string FileFilters = "CR Data Manager Default Rules|*.dat|CR Data Manager Rules|*.dmr|All files (*.*)|*.*";
        TreeNode AlternativelySelectedNode;

        #endregion

        #region Controls
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.BindingSource CollectionBinder;
        private System.Windows.Forms.BindingSource CollectionGroupBinder;
        private System.Windows.Forms.BindingSource CollectionRulesetBinder;
        private System.Windows.Forms.BindingSource GroupBinder;
        private System.Windows.Forms.BindingSource GroupGroupBinder;
        private System.Windows.Forms.BindingSource GroupRulesetBinder;
        private System.Windows.Forms.BindingSource RulesetBinder;
        private System.Windows.Forms.BindingSource RuleBinder;
        private System.Windows.Forms.BindingSource ActionBinder;

        private System.Windows.Forms.ImageList imageList1;

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.BindingSource UserInfoBinder;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel tsslCurrentFile;
        private BackgroundWorker SearchWorker;
        private ToolStripStatusLabel tsslChanged;
        private DataGridViewTextBoxColumn dgvtcTemplateField;
        private ContextMenuStrip cmsMenu;
        private ToolStripMenuItem tsmiContextCopy;
        private ToolStripMenuItem tsmiContextCut;
        private ToolStripMenuItem tsmiContextPaste;
        private ToolStripMenuItem tsmiContextDelete;
        private ToolStripMenuItem tsmiContextExportGroup;
        private ToolStripSeparator tssContext1;
        private DataGridViewTextBoxColumn dgvtcRulesetActionField;
        private ToolStripMenuItem tsmiCopyAll;
        private BindingSource SearchResultsTreeBinder;
        private BindingSource GroupFiltersAndDefaultsBinders;
        private DataGridViewTextBoxColumn completeFieldDataGridViewTextBoxColumn1;
        private BindingSource GroupFiltersBinder;
        private DataGridViewTextBoxColumn completeFieldDataGridViewTextBoxColumn;
        private BindingSource GroupDefaultsBinder;
        private ToolStripProgressBar tspbProgress;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private TabPage tabSearch;
        private ToolStripContainer toolStripContainer2;
        private DataGridView dataGridView3;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn groupDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn pathDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn nodeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn lengthDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn matchDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn locationDataGridViewTextBoxColumn;
        private Label label6;
        private ToolStrip tsSearch;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox tscbSearchType;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripLabel tsLabelSearchField;
        private ToolStripComboBox tscbSearchField;
        private ToolStripLabel tsLabelSearchModifier;
        private ToolStripComboBox tscbSearchModifier;
        private ToolStripLabel tslblSearchValue;
        private ToolStripTextBox tstbValue;
        private ToolStripButton tsbRunSearch;
        private ToolStripSeparator toolStripSeparator5;
        private TabPage tabEdit;
        private SplitContainer splitContainer1;
        private ToolStripContainer toolStripContainer1;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton tsmiFile;
        private ToolStripMenuItem tsmiAbout;
        private ToolStripMenuItem tsmiSettings;
        private ToolStripSeparator tssSeparator1;
        private ToolStripMenuItem tsmiProfileImport;
        private ToolStripMenuItem tsmiMerge;
        private ToolStripMenuItem tsmiRevert;
        private ToolStripMenuItem tsmiLoadDefault;
        private ToolStripMenuItem tsmiLoad;
        private ToolStripMenuItem tsmiNew;
        private ToolStripMenuItem tsmiSaveAsDefault;
        private ToolStripMenuItem tsmiSaveAs;
        private ToolStripMenuItem tsmiSave;
        private ToolStripButton tsbSave;
        private TreeViewReorder tvCollectionTree;
        private Panel pnlRulesets;
        private ParameterManager dmpRules;
        private ParameterManager dmpActions;
        private ComboBox comboBox1;
        private Button btnRulesetRulesMoveDown;
        private Button btnRulesetRulesMoveUp;
        private Button btnActionMoveDown;
        private Button btnActionMoveUp;
        private Button btnRulesetReparse;
        private PictureBox pictureBox2;
        private TextBox txtRulesetReparse;
        private Button btnRulesetRuleRemove;
        private Button btnRulesetRuleUpdate;
        private Button btnRulesetRuleClear;
        private TextBox txtRulesetComment;
        private Button btnRulesetRuleAdd;
        private TextBox txtRulesetName;
        private DataGridView dgvRulesetRules;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dgvtcRulesetRuleModifier;
        private DataGridViewTextBoxColumn dgvtcRulesetRuleValue;
        private DataGridView dgvRulesetActions;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dgvtcRulesetActionModifier;
        private DataGridViewTextBoxColumn dgvtcRulesetActionValue;
        private Button btnRulesetActionAdd;
        private Label label5;
        private Label label2;
        private Button btnRulesetActionClear;
        private PictureBox pictureBox1;
        private Label label3;
        private Button btnRulesetActionUpdate;
        private Button btnRulesetActionRemove;
        private Panel pnlGroups;
        private TabControl tabGroupTabs;
        private TabPage tabPage3;
        private Label label19;
        private Button btnGroupRulesetRemove;
        private Button btnGroupGroupRemove;
        private Button btnGroupRulesetMoveDown;
        private Button button1;
        private DataGridView dgvGroupRulesets;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn quickViewDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn commentDataGridViewTextBoxColumn;
        private Button btnGroupRulesetMoveUp;
        private DataGridView dgvGroupGroups;
        private DataGridViewTextBoxColumn dgvtcGroupGroupName;
        private DataGridViewTextBoxColumn dgvtcGroupGroupComment;
        private DataGridViewTextBoxColumn dgvtcGroupGroupGroupCount;
        private DataGridViewTextBoxColumn dgvtcGroupGroupRulesetCount;
        private Button btnGroupGroupMoveDown;
        private Button btnGroupRulesetAdd;
        private DataGridView dataGridView5;
        private Button btnGroupGroupMoveUp;
        private Button btnGroupGroupAdd;
        private Label label8;
        private Button button2;
        private Label label13;
        private Label label7;
        private TabPage tabGrpRuleset;
        private ParameterManager pmGroupDefaultActions;
        private ParameterManager pmGroupFilters;
        private Button btnGroupFiltersMoveDown;
        private Button btnGroupFiltersMoveUp;
        private ComboBox comboBox2;
        private Button btnGroupDefaultMoveDown;
        private Button btnGroupDefaultMoveUp;
        private PictureBox pictureBox3;
        private Button btnGroupFilterRemove;
        private Button btnGroupFilterUpdate;
        private Button btnGroupFilterClear;
        private Button btnGroupFilterAdd;
        private DataGridView dgvGroupFilters;
        private DataGridViewTextBoxColumn Field;
        private DataGridViewTextBoxColumn modifierDataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn2;
        private DataGridView dgvGroupDefaults;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn modifierDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn1;
        private Button btnGroupDefaultAdd;
        private Label label21;
        private Label label22;
        private Button btnGroupDefaultClear;
        private PictureBox pictureBox4;
        private Label label23;
        private Button btnGroupDefaultUpdate;
        private Button btnGroupDefaultRemove;
        private Label lblGroupOverview;
        private Label commentLabel;
        private TextBox txtGroupComment;
        private Label nameLabel;
        private TextBox txtGroupName;
        private Panel pnlGeneral;
        private Button btnCollectionRulesetMoveDown;
        private Button btnCollectionRulesetMoveUp;
        private Button btnCollectionGroupMoveDown;
        private Button btnCollectionGroupMoveUp;
        private Label label20;
        private Label label4;
        private Label label18;
        private TextBox txtCollectionNotes;
        private TextBox textBox2;
        private TextBox txtCollectionAuthor;
        private Label label17;
        private Label label14;
        private Label label15;
        private Label label16;
        private Button btnCollectionRulesetAdd;
        private Button btnCollectionGroupAdd;
        private DataGridView dgvCollectionRulesets;
        private DataGridViewTextBoxColumn dgvtcCollectionRulesetName;
        private DataGridViewTextBoxColumn dgvtcCollectionRulesetQuickView;
        private DataGridViewTextBoxColumn dgvtcCollectionRulesetComment;
        private Button btnCollectionRulesetsRemove;
        private DataGridView dgvCollectionGroups;
        private DataGridViewTextBoxColumn dgvtcCollectionGroupName;
        private DataGridViewTextBoxColumn dgvtcCollectionGroupComment;
        private DataGridViewTextBoxColumn dgvtcCollectionGroupGroupCount;
        private DataGridViewTextBoxColumn dgvtcCollectionGroupRulesetCount;
        private Button btnCollectionGroupRemove;
        private TabControl tabControl1;
        private ToolStripSeparator tssContext2;
        private ToolStripMenuItem tsmiAddGroup;
        private ToolStripMenuItem tsmiAddRuleset;
        private ToolStripStatusLabel tsslLastSave;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label nameLabel1;
            System.Windows.Forms.Label commentLabel1;
            System.Windows.Forms.Label lblRulesetOriginalText;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(gui));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.RulesetBinder = new System.Windows.Forms.BindingSource(this.components);
            this.RuleBinder = new System.Windows.Forms.BindingSource(this.components);
            this.ActionBinder = new System.Windows.Forms.BindingSource(this.components);
            this.GroupRulesetBinder = new System.Windows.Forms.BindingSource(this.components);
            this.GroupBinder = new System.Windows.Forms.BindingSource(this.components);
            this.GroupGroupBinder = new System.Windows.Forms.BindingSource(this.components);
            this.GroupFiltersAndDefaultsBinders = new System.Windows.Forms.BindingSource(this.components);
            this.GroupFiltersBinder = new System.Windows.Forms.BindingSource(this.components);
            this.GroupDefaultsBinder = new System.Windows.Forms.BindingSource(this.components);
            this.CollectionBinder = new System.Windows.Forms.BindingSource(this.components);
            this.CollectionRulesetBinder = new System.Windows.Forms.BindingSource(this.components);
            this.CollectionGroupBinder = new System.Windows.Forms.BindingSource(this.components);
            this.UserInfoBinder = new System.Windows.Forms.BindingSource(this.components);
            this.SearchResultsTreeBinder = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiContextCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCopyAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiContextCut = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiContextPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiContextDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tssContext1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiContextExportGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnGroupDefaultRemove = new System.Windows.Forms.Button();
            this.btnGroupDefaultUpdate = new System.Windows.Forms.Button();
            this.btnGroupDefaultClear = new System.Windows.Forms.Button();
            this.btnGroupDefaultAdd = new System.Windows.Forms.Button();
            this.btnGroupFilterAdd = new System.Windows.Forms.Button();
            this.btnGroupFilterClear = new System.Windows.Forms.Button();
            this.btnGroupFilterUpdate = new System.Windows.Forms.Button();
            this.btnGroupFilterRemove = new System.Windows.Forms.Button();
            this.btnGroupDefaultMoveUp = new System.Windows.Forms.Button();
            this.btnGroupDefaultMoveDown = new System.Windows.Forms.Button();
            this.btnGroupFiltersMoveUp = new System.Windows.Forms.Button();
            this.btnGroupFiltersMoveDown = new System.Windows.Forms.Button();
            this.btnGroupGroupMoveUp = new System.Windows.Forms.Button();
            this.btnGroupGroupMoveDown = new System.Windows.Forms.Button();
            this.btnGroupRulesetMoveUp = new System.Windows.Forms.Button();
            this.btnGroupRulesetMoveDown = new System.Windows.Forms.Button();
            this.btnRulesetActionRemove = new System.Windows.Forms.Button();
            this.btnRulesetActionUpdate = new System.Windows.Forms.Button();
            this.btnRulesetActionClear = new System.Windows.Forms.Button();
            this.btnRulesetActionAdd = new System.Windows.Forms.Button();
            this.btnRulesetRuleAdd = new System.Windows.Forms.Button();
            this.btnRulesetRuleClear = new System.Windows.Forms.Button();
            this.btnRulesetRuleUpdate = new System.Windows.Forms.Button();
            this.btnRulesetRuleRemove = new System.Windows.Forms.Button();
            this.btnRulesetReparse = new System.Windows.Forms.Button();
            this.btnActionMoveUp = new System.Windows.Forms.Button();
            this.btnActionMoveDown = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslCurrentFile = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslLastSave = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslChanged = new System.Windows.Forms.ToolStripStatusLabel();
            this.tspbProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.SearchWorker = new System.ComponentModel.BackgroundWorker();
            this.tabSearch = new System.Windows.Forms.TabPage();
            this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nodeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lengthDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.matchDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.tsSearch = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tscbSearchType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsLabelSearchField = new System.Windows.Forms.ToolStripLabel();
            this.tscbSearchField = new System.Windows.Forms.ToolStripComboBox();
            this.tsLabelSearchModifier = new System.Windows.Forms.ToolStripLabel();
            this.tscbSearchModifier = new System.Windows.Forms.ToolStripComboBox();
            this.tslblSearchValue = new System.Windows.Forms.ToolStripLabel();
            this.tstbValue = new System.Windows.Forms.ToolStripTextBox();
            this.tsbRunSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tabEdit = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsmiFile = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiProfileImport = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMerge = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRevert = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLoadDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAsDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tvCollectionTree = new DataManagerGUI.TreeViewReorder();
            this.pnlRulesets = new System.Windows.Forms.Panel();
            this.dmpRules = new DataManagerGUI.ParameterManager();
            this.dmpActions = new DataManagerGUI.ParameterManager();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnRulesetRulesMoveDown = new System.Windows.Forms.Button();
            this.btnRulesetRulesMoveUp = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.txtRulesetReparse = new System.Windows.Forms.TextBox();
            this.txtRulesetComment = new System.Windows.Forms.TextBox();
            this.txtRulesetName = new System.Windows.Forms.TextBox();
            this.dgvRulesetRules = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtcRulesetRuleModifier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtcRulesetRuleValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvRulesetActions = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtcRulesetActionModifier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtcRulesetActionValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlGroups = new System.Windows.Forms.Panel();
            this.tabGroupTabs = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label19 = new System.Windows.Forms.Label();
            this.btnGroupRulesetRemove = new System.Windows.Forms.Button();
            this.btnGroupGroupRemove = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dgvGroupRulesets = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quickViewDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvGroupGroups = new System.Windows.Forms.DataGridView();
            this.dgvtcGroupGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtcGroupGroupComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtcGroupGroupGroupCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtcGroupGroupRulesetCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnGroupRulesetAdd = new System.Windows.Forms.Button();
            this.dataGridView5 = new System.Windows.Forms.DataGridView();
            this.btnGroupGroupAdd = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabGrpRuleset = new System.Windows.Forms.TabPage();
            this.pmGroupDefaultActions = new DataManagerGUI.ParameterManager();
            this.pmGroupFilters = new DataManagerGUI.ParameterManager();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.dgvGroupFilters = new System.Windows.Forms.DataGridView();
            this.Field = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifierDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvGroupDefaults = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modifierDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label23 = new System.Windows.Forms.Label();
            this.lblGroupOverview = new System.Windows.Forms.Label();
            this.commentLabel = new System.Windows.Forms.Label();
            this.txtGroupComment = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.pnlGeneral = new System.Windows.Forms.Panel();
            this.btnCollectionRulesetMoveDown = new System.Windows.Forms.Button();
            this.btnCollectionRulesetMoveUp = new System.Windows.Forms.Button();
            this.btnCollectionGroupMoveDown = new System.Windows.Forms.Button();
            this.btnCollectionGroupMoveUp = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtCollectionNotes = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.txtCollectionAuthor = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.btnCollectionRulesetAdd = new System.Windows.Forms.Button();
            this.btnCollectionGroupAdd = new System.Windows.Forms.Button();
            this.dgvCollectionRulesets = new System.Windows.Forms.DataGridView();
            this.tsmiAddGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAddRuleset = new System.Windows.Forms.ToolStripMenuItem();
            this.tssContext2 = new System.Windows.Forms.ToolStripSeparator();
            this.dgvtcCollectionRulesetName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtcCollectionRulesetQuickView = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtcCollectionRulesetComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCollectionRulesetsRemove = new System.Windows.Forms.Button();
            this.dgvCollectionGroups = new System.Windows.Forms.DataGridView();
            this.dgvtcCollectionGroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtcCollectionGroupComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtcCollectionGroupGroupCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtcCollectionGroupRulesetCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCollectionGroupRemove = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            nameLabel1 = new System.Windows.Forms.Label();
            commentLabel1 = new System.Windows.Forms.Label();
            lblRulesetOriginalText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RulesetBinder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RuleBinder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActionBinder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupRulesetBinder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupBinder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupGroupBinder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupFiltersAndDefaultsBinders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupFiltersBinder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupDefaultsBinder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CollectionBinder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CollectionRulesetBinder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CollectionGroupBinder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserInfoBinder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchResultsTreeBinder)).BeginInit();
            this.cmsMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabSearch.SuspendLayout();
            this.toolStripContainer2.ContentPanel.SuspendLayout();
            this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.tsSearch.SuspendLayout();
            this.tabEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlRulesets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRulesetRules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRulesetActions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlGroups.SuspendLayout();
            this.tabGroupTabs.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupRulesets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupGroups)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).BeginInit();
            this.tabGrpRuleset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupFilters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupDefaults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.pnlGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCollectionRulesets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCollectionGroups)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameLabel1
            // 
            nameLabel1.AutoSize = true;
            nameLabel1.Location = new System.Drawing.Point(53, 8);
            nameLabel1.Name = "nameLabel1";
            nameLabel1.Size = new System.Drawing.Size(38, 13);
            nameLabel1.TabIndex = 0;
            nameLabel1.Text = "Name:";
            // 
            // commentLabel1
            // 
            commentLabel1.AutoSize = true;
            commentLabel1.Location = new System.Drawing.Point(337, 8);
            commentLabel1.Name = "commentLabel1";
            commentLabel1.Size = new System.Drawing.Size(54, 13);
            commentLabel1.TabIndex = 2;
            commentLabel1.Text = "Comment:";
            // 
            // lblRulesetOriginalText
            // 
            lblRulesetOriginalText.AutoSize = true;
            lblRulesetOriginalText.Location = new System.Drawing.Point(22, 36);
            lblRulesetOriginalText.Name = "lblRulesetOriginalText";
            lblRulesetOriginalText.Size = new System.Drawing.Size(28, 13);
            lblRulesetOriginalText.TabIndex = 4;
            lblRulesetOriginalText.Text = "Text";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Dataman");
            this.imageList1.Images.SetKeyName(1, "Warning");
            this.imageList1.Images.SetKeyName(2, "Ruleset");
            this.imageList1.Images.SetKeyName(3, "Group");
            this.imageList1.Images.SetKeyName(4, "GroupOK");
            this.imageList1.Images.SetKeyName(5, "GroupHasItems");
            this.imageList1.Images.SetKeyName(6, "GroupAdd");
            this.imageList1.Images.SetKeyName(7, "delete_folder.png");
            this.imageList1.Images.SetKeyName(8, "ItemAdd");
            this.imageList1.Images.SetKeyName(9, "ItemOK");
            this.imageList1.Images.SetKeyName(10, "DeleteItem");
            this.imageList1.Images.SetKeyName(11, "Info");
            this.imageList1.Images.SetKeyName(12, "MoveUp");
            this.imageList1.Images.SetKeyName(13, "MoveDown");
            this.imageList1.Images.SetKeyName(14, "Comment");
            this.imageList1.Images.SetKeyName(15, "Refresh");
            this.imageList1.Images.SetKeyName(16, "Search");
            this.imageList1.Images.SetKeyName(17, "Add");
            this.imageList1.Images.SetKeyName(18, "Delete");
            this.imageList1.Images.SetKeyName(19, "OK");
            this.imageList1.Images.SetKeyName(20, "GroupFolder");
            this.imageList1.Images.SetKeyName(21, "GroupFolderOpened");
            this.imageList1.Images.SetKeyName(22, "Cut");
            this.imageList1.Images.SetKeyName(23, "Paste");
            this.imageList1.Images.SetKeyName(24, "Document");
            // 
            // RulesetBinder
            // 
            this.RulesetBinder.DataSource = typeof(DataManagerGUI.dmRuleset);
            this.RulesetBinder.PositionChanged += new System.EventHandler(this.RuleBinder_PositionChanged);
            // 
            // RuleBinder
            // 
            this.RuleBinder.DataMember = "Rules";
            this.RuleBinder.DataSource = this.RulesetBinder;
            this.RuleBinder.CurrentItemChanged += new System.EventHandler(this.RuleBinder_CurrentItemChanged);
            this.RuleBinder.PositionChanged += new System.EventHandler(this.RuleBinder_PositionChanged);
            // 
            // ActionBinder
            // 
            this.ActionBinder.DataMember = "Actions";
            this.ActionBinder.DataSource = this.RulesetBinder;
            this.ActionBinder.CurrentItemChanged += new System.EventHandler(this.ActionBinder_CurrentItemChanged);
            this.ActionBinder.PositionChanged += new System.EventHandler(this.ActionBinder_PositionChanged);
            // 
            // GroupRulesetBinder
            // 
            this.GroupRulesetBinder.DataMember = "Rulesets";
            this.GroupRulesetBinder.DataSource = this.GroupBinder;
            this.GroupRulesetBinder.PositionChanged += new System.EventHandler(this.GroupRulesetBinder_PositionChanged);
            //
            // GroupBinder
            // 
            this.GroupBinder.DataSource = typeof(DataManagerGUI.dmGroup);
            // 
            // GroupGroupBinder
            // 
            this.GroupGroupBinder.DataMember = "Groups";
            this.GroupGroupBinder.DataSource = this.GroupBinder;
            this.GroupGroupBinder.PositionChanged += new System.EventHandler(this.GroupGroupBinder_PositionChanged);
            // 
            // GroupFiltersAndDefaultsBinders
            // 
            this.GroupFiltersAndDefaultsBinders.DataMember = "FiltersAndDefaults";
            this.GroupFiltersAndDefaultsBinders.DataSource = this.GroupBinder;
            // 
            // GroupFiltersBinder
            // 
            this.GroupFiltersBinder.DataMember = "Rules";
            this.GroupFiltersBinder.DataSource = this.GroupFiltersAndDefaultsBinders;
            this.GroupFiltersBinder.CurrentItemChanged += new System.EventHandler(this.GroupFiltersBinder_CurrentItemChanged);
            this.GroupFiltersBinder.PositionChanged += new System.EventHandler(this.GroupFiltersBinder_PositionChanged);
            // 
            // GroupDefaultsBinder
            // 
            this.GroupDefaultsBinder.DataMember = "Actions";
            this.GroupDefaultsBinder.DataSource = this.GroupFiltersAndDefaultsBinders;
            this.GroupDefaultsBinder.CurrentItemChanged += new System.EventHandler(this.GroupDefaultsBinder_CurrentItemChanged);
            this.GroupDefaultsBinder.PositionChanged += new System.EventHandler(this.GroupDefaultsBinder_PositionChanged);
            // 
            // CollectionBinder
            // 
            this.CollectionBinder.DataSource = typeof(DataManagerGUI.dmCollection);
            // 
            // CollectionRulesetBinder
            // 
            this.CollectionRulesetBinder.DataMember = "Rulesets";
            this.CollectionRulesetBinder.DataSource = this.CollectionBinder;
            this.CollectionRulesetBinder.PositionChanged += new System.EventHandler(this.CollectionRulesetBinder_PositionChanged);
            // 
            // CollectionGroupBinder
            // 
            this.CollectionGroupBinder.DataMember = "Groups";
            this.CollectionGroupBinder.DataSource = this.CollectionBinder;
            this.CollectionGroupBinder.PositionChanged += new System.EventHandler(this.CollectionGroupBinder_PositionChanged);
            // 
            // UserInfoBinder
            // 
            this.UserInfoBinder.DataSource = typeof(DataManagerGUI.dmUserInfo);
            // 
            // SearchResultsTreeBinder
            // 
            this.SearchResultsTreeBinder.DataSource = typeof(DataManagerGUI.TreeSearchResults);
            this.SearchResultsTreeBinder.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.SearchResultsTreeBinder_ListChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 26);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 26);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 26);
            // 
            // cmsMenu
            // 
            this.cmsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiContextCopy,
            this.tsmiCopyAll,
            this.tsmiContextCut,
            this.tsmiContextPaste,
            this.tsmiContextDelete,
            this.tssContext1,
            this.tsmiContextExportGroup,
            this.tssContext2,
            this.tsmiAddGroup,
            this.tsmiAddRuleset});
            this.cmsMenu.Name = "cmsMenu";
            this.cmsMenu.Size = new System.Drawing.Size(145, 192);
            this.cmsMenu.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.cmsMenu_Closing);
            this.cmsMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmsMenu_Opening);
            // 
            // tsmiContextCopy
            // 
            this.tsmiContextCopy.Image = ((System.Drawing.Image)(resources.GetObject("tsmiContextCopy.Image")));
            this.tsmiContextCopy.Name = "tsmiContextCopy";
            this.tsmiContextCopy.Size = new System.Drawing.Size(144, 22);
            this.tsmiContextCopy.Text = "Copy";
            this.tsmiContextCopy.Click += new System.EventHandler(this.tsmiContextCopy_Click);
            // 
            // tsmiCopyAll
            // 
            this.tsmiCopyAll.Name = "tsmiCopyAll";
            this.tsmiCopyAll.Size = new System.Drawing.Size(144, 22);
            this.tsmiCopyAll.Text = "Copy All";
            this.tsmiCopyAll.Click += new System.EventHandler(this.tsmiCopyAll_Click);
            // 
            // tsmiContextCut
            // 
            this.tsmiContextCut.Image = ((System.Drawing.Image)(resources.GetObject("tsmiContextCut.Image")));
            this.tsmiContextCut.Name = "tsmiContextCut";
            this.tsmiContextCut.Size = new System.Drawing.Size(144, 22);
            this.tsmiContextCut.Text = "Cut";
            this.tsmiContextCut.Click += new System.EventHandler(this.tsmiContextCut_Click);
            // 
            // tsmiContextPaste
            // 
            this.tsmiContextPaste.Image = ((System.Drawing.Image)(resources.GetObject("tsmiContextPaste.Image")));
            this.tsmiContextPaste.Name = "tsmiContextPaste";
            this.tsmiContextPaste.Size = new System.Drawing.Size(144, 22);
            this.tsmiContextPaste.Text = "Paste";
            this.tsmiContextPaste.Click += new System.EventHandler(this.tsmiContextPaste_Click);
            // 
            // tsmiContextDelete
            // 
            this.tsmiContextDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsmiContextDelete.Image")));
            this.tsmiContextDelete.Name = "tsmiContextDelete";
            this.tsmiContextDelete.Size = new System.Drawing.Size(144, 22);
            this.tsmiContextDelete.Text = "Delete";
            this.tsmiContextDelete.Click += new System.EventHandler(this.tsmiContextDelete_Click);
            // 
            // tssContext1
            // 
            this.tssContext1.Name = "tssContext1";
            this.tssContext1.Size = new System.Drawing.Size(141, 6);
            // 
            // tsmiContextExportGroup
            // 
            this.tsmiContextExportGroup.Image = ((System.Drawing.Image)(resources.GetObject("tsmiContextExportGroup.Image")));
            this.tsmiContextExportGroup.Name = "tsmiContextExportGroup";
            this.tsmiContextExportGroup.Size = new System.Drawing.Size(144, 22);
            this.tsmiContextExportGroup.Text = "Export Group";
            this.tsmiContextExportGroup.Click += new System.EventHandler(this.tsmiGroupExport_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // btnGroupDefaultRemove
            // 
            this.btnGroupDefaultRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupDefaultRemove.ImageIndex = 18;
            this.btnGroupDefaultRemove.ImageList = this.imageList1;
            this.btnGroupDefaultRemove.Location = new System.Drawing.Point(442, 437);
            this.btnGroupDefaultRemove.Name = "btnGroupDefaultRemove";
            this.btnGroupDefaultRemove.Size = new System.Drawing.Size(104, 23);
            this.btnGroupDefaultRemove.TabIndex = 41;
            this.btnGroupDefaultRemove.Text = "Remove";
            this.btnGroupDefaultRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnGroupDefaultRemove, "Removes the selected action from this Ruleset.");
            this.btnGroupDefaultRemove.UseVisualStyleBackColor = true;
            this.btnGroupDefaultRemove.Click += new System.EventHandler(this.btnGroupDefaultRemove_Click);
            // 
            // btnGroupDefaultUpdate
            // 
            this.btnGroupDefaultUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupDefaultUpdate.ImageIndex = 15;
            this.btnGroupDefaultUpdate.ImageList = this.imageList1;
            this.btnGroupDefaultUpdate.Location = new System.Drawing.Point(234, 437);
            this.btnGroupDefaultUpdate.Name = "btnGroupDefaultUpdate";
            this.btnGroupDefaultUpdate.Size = new System.Drawing.Size(104, 23);
            this.btnGroupDefaultUpdate.TabIndex = 39;
            this.btnGroupDefaultUpdate.Text = "Update";
            this.btnGroupDefaultUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnGroupDefaultUpdate, "Change the currently selected \r\nAction to what is specified above.");
            this.btnGroupDefaultUpdate.UseVisualStyleBackColor = true;
            this.btnGroupDefaultUpdate.Click += new System.EventHandler(this.btnGroupDefaultUpdate_Click);
            // 
            // btnGroupDefaultClear
            // 
            this.btnGroupDefaultClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupDefaultClear.ImageIndex = 7;
            this.btnGroupDefaultClear.ImageList = this.imageList1;
            this.btnGroupDefaultClear.Location = new System.Drawing.Point(546, 437);
            this.btnGroupDefaultClear.Name = "btnGroupDefaultClear";
            this.btnGroupDefaultClear.Size = new System.Drawing.Size(104, 23);
            this.btnGroupDefaultClear.TabIndex = 42;
            this.btnGroupDefaultClear.Text = "Clear Actions";
            this.btnGroupDefaultClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnGroupDefaultClear, "Removes all actions from this Ruleset");
            this.btnGroupDefaultClear.UseVisualStyleBackColor = true;
            this.btnGroupDefaultClear.Click += new System.EventHandler(this.btnGroupDefaultClear_Click);
            // 
            // btnGroupDefaultAdd
            // 
            this.btnGroupDefaultAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupDefaultAdd.ImageIndex = 17;
            this.btnGroupDefaultAdd.ImageList = this.imageList1;
            this.btnGroupDefaultAdd.Location = new System.Drawing.Point(338, 437);
            this.btnGroupDefaultAdd.Name = "btnGroupDefaultAdd";
            this.btnGroupDefaultAdd.Size = new System.Drawing.Size(104, 23);
            this.btnGroupDefaultAdd.TabIndex = 40;
            this.btnGroupDefaultAdd.Text = "Add";
            this.btnGroupDefaultAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnGroupDefaultAdd, "Add a new Action as specfied above");
            this.btnGroupDefaultAdd.UseVisualStyleBackColor = true;
            this.btnGroupDefaultAdd.Click += new System.EventHandler(this.btnGroupDefaultAdd_Click);
            // 
            // btnGroupFilterAdd
            // 
            this.btnGroupFilterAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupFilterAdd.ImageIndex = 17;
            this.btnGroupFilterAdd.ImageList = this.imageList1;
            this.btnGroupFilterAdd.Location = new System.Drawing.Point(357, 210);
            this.btnGroupFilterAdd.Name = "btnGroupFilterAdd";
            this.btnGroupFilterAdd.Size = new System.Drawing.Size(104, 23);
            this.btnGroupFilterAdd.TabIndex = 30;
            this.btnGroupFilterAdd.Text = "Add";
            this.btnGroupFilterAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnGroupFilterAdd, "Add a new Rule as specfied above");
            this.btnGroupFilterAdd.UseVisualStyleBackColor = true;
            this.btnGroupFilterAdd.Click += new System.EventHandler(this.btnGroupFilterAdd_Click);
            // 
            // btnGroupFilterClear
            // 
            this.btnGroupFilterClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupFilterClear.ImageIndex = 7;
            this.btnGroupFilterClear.ImageList = this.imageList1;
            this.btnGroupFilterClear.Location = new System.Drawing.Point(565, 210);
            this.btnGroupFilterClear.Name = "btnGroupFilterClear";
            this.btnGroupFilterClear.Size = new System.Drawing.Size(104, 23);
            this.btnGroupFilterClear.TabIndex = 33;
            this.btnGroupFilterClear.Text = "Clear Rules";
            this.btnGroupFilterClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnGroupFilterClear, "Clear All Rules from this Ruleset");
            this.btnGroupFilterClear.UseVisualStyleBackColor = true;
            this.btnGroupFilterClear.Click += new System.EventHandler(this.btnGroupFilterClear_Click);
            // 
            // btnGroupFilterUpdate
            // 
            this.btnGroupFilterUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupFilterUpdate.ImageIndex = 15;
            this.btnGroupFilterUpdate.ImageList = this.imageList1;
            this.btnGroupFilterUpdate.Location = new System.Drawing.Point(253, 210);
            this.btnGroupFilterUpdate.Name = "btnGroupFilterUpdate";
            this.btnGroupFilterUpdate.Size = new System.Drawing.Size(104, 23);
            this.btnGroupFilterUpdate.TabIndex = 31;
            this.btnGroupFilterUpdate.Text = "Update";
            this.btnGroupFilterUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnGroupFilterUpdate, "Change the currently selected \r\nRule to what is specified above.");
            this.btnGroupFilterUpdate.UseVisualStyleBackColor = true;
            this.btnGroupFilterUpdate.Click += new System.EventHandler(this.btnGroupFilterUpdate_Click);
            // 
            // btnGroupFilterRemove
            // 
            this.btnGroupFilterRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupFilterRemove.ImageIndex = 18;
            this.btnGroupFilterRemove.ImageList = this.imageList1;
            this.btnGroupFilterRemove.Location = new System.Drawing.Point(461, 210);
            this.btnGroupFilterRemove.Name = "btnGroupFilterRemove";
            this.btnGroupFilterRemove.Size = new System.Drawing.Size(104, 23);
            this.btnGroupFilterRemove.TabIndex = 32;
            this.btnGroupFilterRemove.Text = "Remove";
            this.btnGroupFilterRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnGroupFilterRemove, "Removes the selected action from this Ruleset.");
            this.btnGroupFilterRemove.UseVisualStyleBackColor = true;
            this.btnGroupFilterRemove.Click += new System.EventHandler(this.btnGroupFilterRemove_Click);
            // 
            // btnGroupDefaultMoveUp
            // 
            this.btnGroupDefaultMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupDefaultMoveUp.Enabled = false;
            this.btnGroupDefaultMoveUp.ImageIndex = 12;
            this.btnGroupDefaultMoveUp.ImageList = this.imageList1;
            this.btnGroupDefaultMoveUp.Location = new System.Drawing.Point(647, 349);
            this.btnGroupDefaultMoveUp.Name = "btnGroupDefaultMoveUp";
            this.btnGroupDefaultMoveUp.Size = new System.Drawing.Size(25, 25);
            this.btnGroupDefaultMoveUp.TabIndex = 36;
            this.toolTip1.SetToolTip(this.btnGroupDefaultMoveUp, "Move selected Group up.");
            this.btnGroupDefaultMoveUp.UseVisualStyleBackColor = true;
            this.btnGroupDefaultMoveUp.Click += new System.EventHandler(this.btnGroupDefaultMoveUp_Click);
            // 
            // btnGroupDefaultMoveDown
            // 
            this.btnGroupDefaultMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupDefaultMoveDown.Enabled = false;
            this.btnGroupDefaultMoveDown.ImageIndex = 13;
            this.btnGroupDefaultMoveDown.ImageList = this.imageList1;
            this.btnGroupDefaultMoveDown.Location = new System.Drawing.Point(647, 375);
            this.btnGroupDefaultMoveDown.Name = "btnGroupDefaultMoveDown";
            this.btnGroupDefaultMoveDown.Size = new System.Drawing.Size(25, 25);
            this.btnGroupDefaultMoveDown.TabIndex = 37;
            this.toolTip1.SetToolTip(this.btnGroupDefaultMoveDown, "Move selected Group down.");
            this.btnGroupDefaultMoveDown.UseVisualStyleBackColor = true;
            this.btnGroupDefaultMoveDown.Click += new System.EventHandler(this.btnGroupDefaultMoveDown_Click);
            // 
            // btnGroupFiltersMoveUp
            // 
            this.btnGroupFiltersMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupFiltersMoveUp.Enabled = false;
            this.btnGroupFiltersMoveUp.ImageIndex = 12;
            this.btnGroupFiltersMoveUp.ImageList = this.imageList1;
            this.btnGroupFiltersMoveUp.Location = new System.Drawing.Point(646, 114);
            this.btnGroupFiltersMoveUp.Name = "btnGroupFiltersMoveUp";
            this.btnGroupFiltersMoveUp.Size = new System.Drawing.Size(25, 25);
            this.btnGroupFiltersMoveUp.TabIndex = 44;
            this.toolTip1.SetToolTip(this.btnGroupFiltersMoveUp, "Move selected Group up.");
            this.btnGroupFiltersMoveUp.UseVisualStyleBackColor = true;
            this.btnGroupFiltersMoveUp.Click += new System.EventHandler(this.btnGroupFiltersMoveUp_Click);
            // 
            // btnGroupFiltersMoveDown
            // 
            this.btnGroupFiltersMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupFiltersMoveDown.Enabled = false;
            this.btnGroupFiltersMoveDown.ImageIndex = 13;
            this.btnGroupFiltersMoveDown.ImageList = this.imageList1;
            this.btnGroupFiltersMoveDown.Location = new System.Drawing.Point(646, 140);
            this.btnGroupFiltersMoveDown.Name = "btnGroupFiltersMoveDown";
            this.btnGroupFiltersMoveDown.Size = new System.Drawing.Size(25, 25);
            this.btnGroupFiltersMoveDown.TabIndex = 45;
            this.toolTip1.SetToolTip(this.btnGroupFiltersMoveDown, "Move selected Group down.");
            this.btnGroupFiltersMoveDown.UseVisualStyleBackColor = true;
            this.btnGroupFiltersMoveDown.Click += new System.EventHandler(this.btnGroupFiltersMoveDown_Click);
            // 
            // btnGroupGroupMoveUp
            // 
            this.btnGroupGroupMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupGroupMoveUp.Enabled = false;
            this.btnGroupGroupMoveUp.ImageIndex = 12;
            this.btnGroupGroupMoveUp.ImageList = this.imageList1;
            this.btnGroupGroupMoveUp.Location = new System.Drawing.Point(649, 152);
            this.btnGroupGroupMoveUp.Name = "btnGroupGroupMoveUp";
            this.btnGroupGroupMoveUp.Size = new System.Drawing.Size(25, 25);
            this.btnGroupGroupMoveUp.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btnGroupGroupMoveUp, "Move selected Group up.");
            this.btnGroupGroupMoveUp.UseVisualStyleBackColor = true;
            this.btnGroupGroupMoveUp.Click += new System.EventHandler(this.btnGroupGroupMoveUp_Click);
            // 
            // btnGroupGroupMoveDown
            // 
            this.btnGroupGroupMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupGroupMoveDown.Enabled = false;
            this.btnGroupGroupMoveDown.ImageIndex = 13;
            this.btnGroupGroupMoveDown.ImageList = this.imageList1;
            this.btnGroupGroupMoveDown.Location = new System.Drawing.Point(649, 178);
            this.btnGroupGroupMoveDown.Name = "btnGroupGroupMoveDown";
            this.btnGroupGroupMoveDown.Size = new System.Drawing.Size(25, 25);
            this.btnGroupGroupMoveDown.TabIndex = 7;
            this.toolTip1.SetToolTip(this.btnGroupGroupMoveDown, "Move selected Group down.");
            this.btnGroupGroupMoveDown.UseVisualStyleBackColor = true;
            this.btnGroupGroupMoveDown.Click += new System.EventHandler(this.btnGroupGroupMoveDown_Click);
            // 
            // btnGroupRulesetMoveUp
            // 
            this.btnGroupRulesetMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupRulesetMoveUp.Enabled = false;
            this.btnGroupRulesetMoveUp.ImageIndex = 12;
            this.btnGroupRulesetMoveUp.ImageList = this.imageList1;
            this.btnGroupRulesetMoveUp.Location = new System.Drawing.Point(649, 372);
            this.btnGroupRulesetMoveUp.Name = "btnGroupRulesetMoveUp";
            this.btnGroupRulesetMoveUp.Size = new System.Drawing.Size(25, 25);
            this.btnGroupRulesetMoveUp.TabIndex = 12;
            this.toolTip1.SetToolTip(this.btnGroupRulesetMoveUp, "Move selected Ruleset up");
            this.btnGroupRulesetMoveUp.UseVisualStyleBackColor = true;
            this.btnGroupRulesetMoveUp.Click += new System.EventHandler(this.btnGroupRulesetMoveUp_Click);
            // 
            // btnGroupRulesetMoveDown
            // 
            this.btnGroupRulesetMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupRulesetMoveDown.Enabled = false;
            this.btnGroupRulesetMoveDown.ImageIndex = 13;
            this.btnGroupRulesetMoveDown.ImageList = this.imageList1;
            this.btnGroupRulesetMoveDown.Location = new System.Drawing.Point(649, 398);
            this.btnGroupRulesetMoveDown.Name = "btnGroupRulesetMoveDown";
            this.btnGroupRulesetMoveDown.Size = new System.Drawing.Size(25, 25);
            this.btnGroupRulesetMoveDown.TabIndex = 13;
            this.toolTip1.SetToolTip(this.btnGroupRulesetMoveDown, "Move Selected Ruleset down");
            this.btnGroupRulesetMoveDown.UseVisualStyleBackColor = true;
            this.btnGroupRulesetMoveDown.Click += new System.EventHandler(this.btnGroupRulesetMoveDown_Click);
            // 
            // btnRulesetActionRemove
            // 
            this.btnRulesetActionRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRulesetActionRemove.ImageIndex = 18;
            this.btnRulesetActionRemove.ImageList = this.imageList1;
            this.btnRulesetActionRemove.Location = new System.Drawing.Point(446, 574);
            this.btnRulesetActionRemove.Name = "btnRulesetActionRemove";
            this.btnRulesetActionRemove.Size = new System.Drawing.Size(104, 23);
            this.btnRulesetActionRemove.TabIndex = 21;
            this.btnRulesetActionRemove.Text = "Remove";
            this.btnRulesetActionRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnRulesetActionRemove, "Removes the selected action from this Ruleset.");
            this.btnRulesetActionRemove.UseVisualStyleBackColor = true;
            this.btnRulesetActionRemove.Click += new System.EventHandler(this.btnRulesetActionRemove_Click);
            // 
            // btnRulesetActionUpdate
            // 
            this.btnRulesetActionUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRulesetActionUpdate.ImageIndex = 15;
            this.btnRulesetActionUpdate.ImageList = this.imageList1;
            this.btnRulesetActionUpdate.Location = new System.Drawing.Point(238, 574);
            this.btnRulesetActionUpdate.Name = "btnRulesetActionUpdate";
            this.btnRulesetActionUpdate.Size = new System.Drawing.Size(104, 23);
            this.btnRulesetActionUpdate.TabIndex = 19;
            this.btnRulesetActionUpdate.Text = "Update";
            this.btnRulesetActionUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnRulesetActionUpdate, "Change the currently selected \r\nAction to what is specified above.");
            this.btnRulesetActionUpdate.UseVisualStyleBackColor = true;
            this.btnRulesetActionUpdate.Click += new System.EventHandler(this.btnRulesetActionUpdate_Click);
            // 
            // btnRulesetActionClear
            // 
            this.btnRulesetActionClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRulesetActionClear.ImageIndex = 7;
            this.btnRulesetActionClear.ImageList = this.imageList1;
            this.btnRulesetActionClear.Location = new System.Drawing.Point(550, 574);
            this.btnRulesetActionClear.Name = "btnRulesetActionClear";
            this.btnRulesetActionClear.Size = new System.Drawing.Size(104, 23);
            this.btnRulesetActionClear.TabIndex = 22;
            this.btnRulesetActionClear.Text = "Clear Actions";
            this.btnRulesetActionClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnRulesetActionClear, "Removes all actions from this Ruleset");
            this.btnRulesetActionClear.UseVisualStyleBackColor = true;
            this.btnRulesetActionClear.Click += new System.EventHandler(this.btnRulesetActionClear_Click);
            // 
            // btnRulesetActionAdd
            // 
            this.btnRulesetActionAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRulesetActionAdd.ImageIndex = 17;
            this.btnRulesetActionAdd.ImageList = this.imageList1;
            this.btnRulesetActionAdd.Location = new System.Drawing.Point(342, 574);
            this.btnRulesetActionAdd.Name = "btnRulesetActionAdd";
            this.btnRulesetActionAdd.Size = new System.Drawing.Size(104, 23);
            this.btnRulesetActionAdd.TabIndex = 20;
            this.btnRulesetActionAdd.Text = "Add";
            this.btnRulesetActionAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnRulesetActionAdd, "Add a new Action as specfied above");
            this.btnRulesetActionAdd.UseVisualStyleBackColor = true;
            this.btnRulesetActionAdd.Click += new System.EventHandler(this.btnRulesetActionAdd_Click);
            // 
            // btnRulesetRuleAdd
            // 
            this.btnRulesetRuleAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRulesetRuleAdd.ImageIndex = 17;
            this.btnRulesetRuleAdd.ImageList = this.imageList1;
            this.btnRulesetRuleAdd.Location = new System.Drawing.Point(363, 308);
            this.btnRulesetRuleAdd.Name = "btnRulesetRuleAdd";
            this.btnRulesetRuleAdd.Size = new System.Drawing.Size(104, 23);
            this.btnRulesetRuleAdd.TabIndex = 10;
            this.btnRulesetRuleAdd.Text = "Add";
            this.btnRulesetRuleAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnRulesetRuleAdd, "Add a new Rule as specfied above");
            this.btnRulesetRuleAdd.UseVisualStyleBackColor = true;
            this.btnRulesetRuleAdd.Click += new System.EventHandler(this.btnRulesetRuleAdd_Click);
            // 
            // btnRulesetRuleClear
            // 
            this.btnRulesetRuleClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRulesetRuleClear.ImageIndex = 7;
            this.btnRulesetRuleClear.ImageList = this.imageList1;
            this.btnRulesetRuleClear.Location = new System.Drawing.Point(571, 308);
            this.btnRulesetRuleClear.Name = "btnRulesetRuleClear";
            this.btnRulesetRuleClear.Size = new System.Drawing.Size(104, 23);
            this.btnRulesetRuleClear.TabIndex = 13;
            this.btnRulesetRuleClear.Text = "Clear Rules";
            this.btnRulesetRuleClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnRulesetRuleClear, "Clear All Rules from this Ruleset");
            this.btnRulesetRuleClear.UseVisualStyleBackColor = true;
            this.btnRulesetRuleClear.Click += new System.EventHandler(this.btnRulesetRuleClear_Click);
            // 
            // btnRulesetRuleUpdate
            // 
            this.btnRulesetRuleUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRulesetRuleUpdate.ImageIndex = 15;
            this.btnRulesetRuleUpdate.ImageList = this.imageList1;
            this.btnRulesetRuleUpdate.Location = new System.Drawing.Point(259, 308);
            this.btnRulesetRuleUpdate.Name = "btnRulesetRuleUpdate";
            this.btnRulesetRuleUpdate.Size = new System.Drawing.Size(104, 23);
            this.btnRulesetRuleUpdate.TabIndex = 11;
            this.btnRulesetRuleUpdate.Text = "Update";
            this.btnRulesetRuleUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnRulesetRuleUpdate, "Change the currently selected \r\nRule to what is specified above.");
            this.btnRulesetRuleUpdate.UseVisualStyleBackColor = true;
            this.btnRulesetRuleUpdate.Click += new System.EventHandler(this.btnRulesetRuleUpdate_Click);
            // 
            // btnRulesetRuleRemove
            // 
            this.btnRulesetRuleRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRulesetRuleRemove.ImageIndex = 18;
            this.btnRulesetRuleRemove.ImageList = this.imageList1;
            this.btnRulesetRuleRemove.Location = new System.Drawing.Point(467, 308);
            this.btnRulesetRuleRemove.Name = "btnRulesetRuleRemove";
            this.btnRulesetRuleRemove.Size = new System.Drawing.Size(104, 23);
            this.btnRulesetRuleRemove.TabIndex = 12;
            this.btnRulesetRuleRemove.Text = "Remove";
            this.btnRulesetRuleRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnRulesetRuleRemove, "Removes the selected action from this Ruleset.");
            this.btnRulesetRuleRemove.UseVisualStyleBackColor = true;
            this.btnRulesetRuleRemove.Click += new System.EventHandler(this.btnRulesetRuleRemove_Click);
            // 
            // btnRulesetReparse
            // 
            this.btnRulesetReparse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRulesetReparse.ImageIndex = 15;
            this.btnRulesetReparse.ImageList = this.imageList1;
            this.btnRulesetReparse.Location = new System.Drawing.Point(655, 30);
            this.btnRulesetReparse.Name = "btnRulesetReparse";
            this.btnRulesetReparse.Size = new System.Drawing.Size(25, 25);
            this.btnRulesetReparse.TabIndex = 6;
            this.toolTip1.SetToolTip(this.btnRulesetReparse, "You can text edit the ruleset, cut and paste \r\na ruleset from text. And press thi" +
        "s button to\r\nmodify the ruleset. This will replace the \r\ncurrent ruleset.");
            this.btnRulesetReparse.UseVisualStyleBackColor = true;
            this.btnRulesetReparse.Click += new System.EventHandler(this.btnRulesetReparse_Click);
            // 
            // btnActionMoveUp
            // 
            this.btnActionMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActionMoveUp.Enabled = false;
            this.btnActionMoveUp.ImageIndex = 12;
            this.btnActionMoveUp.ImageList = this.imageList1;
            this.btnActionMoveUp.Location = new System.Drawing.Point(658, 482);
            this.btnActionMoveUp.Name = "btnActionMoveUp";
            this.btnActionMoveUp.Size = new System.Drawing.Size(25, 25);
            this.btnActionMoveUp.TabIndex = 16;
            this.toolTip1.SetToolTip(this.btnActionMoveUp, "Move selected Group up.");
            this.btnActionMoveUp.UseVisualStyleBackColor = true;
            this.btnActionMoveUp.Click += new System.EventHandler(this.btnActionMoveUp_Click);
            // 
            // btnActionMoveDown
            // 
            this.btnActionMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActionMoveDown.Enabled = false;
            this.btnActionMoveDown.ImageIndex = 13;
            this.btnActionMoveDown.ImageList = this.imageList1;
            this.btnActionMoveDown.Location = new System.Drawing.Point(658, 508);
            this.btnActionMoveDown.Name = "btnActionMoveDown";
            this.btnActionMoveDown.Size = new System.Drawing.Size(25, 25);
            this.btnActionMoveDown.TabIndex = 17;
            this.toolTip1.SetToolTip(this.btnActionMoveDown, "Move selected Group down.");
            this.btnActionMoveDown.UseVisualStyleBackColor = true;
            this.btnActionMoveDown.Click += new System.EventHandler(this.btnActionMoveDown_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslCurrentFile,
            this.tsslLastSave,
            this.tsslChanged,
            this.tspbProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 639);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(922, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslCurrentFile
            // 
            this.tsslCurrentFile.Name = "tsslCurrentFile";
            this.tsslCurrentFile.Size = new System.Drawing.Size(648, 17);
            this.tsslCurrentFile.Spring = true;
            this.tsslCurrentFile.Text = "tsslCurrentFile";
            this.tsslCurrentFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsslLastSave
            // 
            this.tsslLastSave.Name = "tsslLastSave";
            this.tsslLastSave.Size = new System.Drawing.Size(99, 17);
            this.tsslLastSave.Text = "Last Saved: Never";
            // 
            // tsslChanged
            // 
            this.tsslChanged.Name = "tsslChanged";
            this.tsslChanged.Size = new System.Drawing.Size(160, 17);
            this.tsslChanged.Text = "Changed Since Last Save: Yes";
            // 
            // tspbProgress
            // 
            this.tspbProgress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tspbProgress.Name = "tspbProgress";
            this.tspbProgress.Size = new System.Drawing.Size(100, 16);
            this.tspbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.tspbProgress.Visible = false;
            // 
            // SearchWorker
            // 
            this.SearchWorker.WorkerReportsProgress = true;
            this.SearchWorker.WorkerSupportsCancellation = true;
            this.SearchWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.SearchWorker_DoWork);
            this.SearchWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.SearchWorker_ProgressChanged);
            this.SearchWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.SearchWorker_RunWorkerCompleted);
            // 
            // tabSearch
            // 
            this.tabSearch.Controls.Add(this.toolStripContainer2);
            this.tabSearch.Location = new System.Drawing.Point(4, 22);
            this.tabSearch.Name = "tabSearch";
            this.tabSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabSearch.Size = new System.Drawing.Size(914, 610);
            this.tabSearch.TabIndex = 1;
            this.tabSearch.Text = "Search";
            this.tabSearch.UseVisualStyleBackColor = true;
            // 
            // toolStripContainer2
            // 
            // 
            // toolStripContainer2.ContentPanel
            // 
            this.toolStripContainer2.ContentPanel.Controls.Add(this.dataGridView3);
            this.toolStripContainer2.ContentPanel.Controls.Add(this.label6);
            this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(908, 579);
            this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer2.Location = new System.Drawing.Point(3, 3);
            this.toolStripContainer2.Name = "toolStripContainer2";
            this.toolStripContainer2.Size = new System.Drawing.Size(908, 604);
            this.toolStripContainer2.TabIndex = 1;
            this.toolStripContainer2.Text = "toolStripContainer2";
            // 
            // toolStripContainer2.TopToolStripPanel
            // 
            this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.tsSearch);
            // 
            // dataGridView3
            // 
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView3.AllowUserToDeleteRows = false;
            this.dataGridView3.AllowUserToOrderColumns = true;
            this.dataGridView3.AllowUserToResizeRows = false;
            this.dataGridView3.AutoGenerateColumns = false;
            this.dataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn1,
            this.groupDataGridViewTextBoxColumn,
            this.pathDataGridViewTextBoxColumn,
            this.nodeDataGridViewTextBoxColumn,
            this.lengthDataGridViewTextBoxColumn,
            this.matchDataGridViewTextBoxColumn,
            this.locationDataGridViewTextBoxColumn});
            this.dataGridView3.DataSource = this.SearchResultsTreeBinder;
            this.dataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView3.Location = new System.Drawing.Point(0, 31);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.ReadOnly = true;
            this.dataGridView3.RowHeadersVisible = false;
            this.dataGridView3.Size = new System.Drawing.Size(908, 548);
            this.dataGridView3.TabIndex = 4;
            this.dataGridView3.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView3_CellContentDoubleClick);
            // 
            // nameDataGridViewTextBoxColumn1
            // 
            this.nameDataGridViewTextBoxColumn1.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn1.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn1.Name = "nameDataGridViewTextBoxColumn1";
            this.nameDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // groupDataGridViewTextBoxColumn
            // 
            this.groupDataGridViewTextBoxColumn.DataPropertyName = "Group";
            this.groupDataGridViewTextBoxColumn.HeaderText = "Group";
            this.groupDataGridViewTextBoxColumn.Name = "groupDataGridViewTextBoxColumn";
            this.groupDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pathDataGridViewTextBoxColumn
            // 
            this.pathDataGridViewTextBoxColumn.DataPropertyName = "Path";
            this.pathDataGridViewTextBoxColumn.HeaderText = "Path";
            this.pathDataGridViewTextBoxColumn.Name = "pathDataGridViewTextBoxColumn";
            this.pathDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nodeDataGridViewTextBoxColumn
            // 
            this.nodeDataGridViewTextBoxColumn.DataPropertyName = "Node";
            this.nodeDataGridViewTextBoxColumn.HeaderText = "Node";
            this.nodeDataGridViewTextBoxColumn.Name = "nodeDataGridViewTextBoxColumn";
            this.nodeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lengthDataGridViewTextBoxColumn
            // 
            this.lengthDataGridViewTextBoxColumn.DataPropertyName = "Length";
            this.lengthDataGridViewTextBoxColumn.HeaderText = "Length";
            this.lengthDataGridViewTextBoxColumn.Name = "lengthDataGridViewTextBoxColumn";
            this.lengthDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // matchDataGridViewTextBoxColumn
            // 
            this.matchDataGridViewTextBoxColumn.DataPropertyName = "Match";
            this.matchDataGridViewTextBoxColumn.HeaderText = "Match";
            this.matchDataGridViewTextBoxColumn.Name = "matchDataGridViewTextBoxColumn";
            this.matchDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // locationDataGridViewTextBoxColumn
            // 
            this.locationDataGridViewTextBoxColumn.DataPropertyName = "Location";
            this.locationDataGridViewTextBoxColumn.HeaderText = "Location";
            this.locationDataGridViewTextBoxColumn.Name = "locationDataGridViewTextBoxColumn";
            this.locationDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(908, 31);
            this.label6.TabIndex = 3;
            this.label6.Text = "Search Results:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsSearch
            // 
            this.tsSearch.Dock = System.Windows.Forms.DockStyle.None;
            this.tsSearch.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsSearch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tscbSearchType,
            this.toolStripSeparator4,
            this.tsLabelSearchField,
            this.tscbSearchField,
            this.tsLabelSearchModifier,
            this.tscbSearchModifier,
            this.tslblSearchValue,
            this.tstbValue,
            this.tsbRunSearch,
            this.toolStripSeparator5});
            this.tsSearch.Location = new System.Drawing.Point(0, 0);
            this.tsSearch.Name = "tsSearch";
            this.tsSearch.Size = new System.Drawing.Size(908, 25);
            this.tsSearch.Stretch = true;
            this.tsSearch.TabIndex = 0;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(82, 22);
            this.toolStripLabel1.Text = "Search Mode: ";
            // 
            // tscbSearchType
            // 
            this.tscbSearchType.Items.AddRange(new object[] {
            "Rule/Action Parameter Search",
            "Group/Ruleset Name Search",
            "Search All"});
            this.tscbSearchType.Name = "tscbSearchType";
            this.tscbSearchType.Size = new System.Drawing.Size(121, 25);
            this.tscbSearchType.SelectedIndexChanged += new System.EventHandler(this.tscbSearchType_SelectedIndexChanged);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsLabelSearchField
            // 
            this.tsLabelSearchField.Name = "tsLabelSearchField";
            this.tsLabelSearchField.Size = new System.Drawing.Size(32, 22);
            this.tsLabelSearchField.Text = "Field";
            this.tsLabelSearchField.Visible = false;
            // 
            // tscbSearchField
            // 
            this.tscbSearchField.Name = "tscbSearchField";
            this.tscbSearchField.Size = new System.Drawing.Size(95, 25);
            this.tscbSearchField.Visible = false;
            this.tscbSearchField.TextChanged += new System.EventHandler(this.tscbSearchField_SelectedIndexChanged);
            // 
            // tsLabelSearchModifier
            // 
            this.tsLabelSearchModifier.Name = "tsLabelSearchModifier";
            this.tsLabelSearchModifier.Size = new System.Drawing.Size(52, 22);
            this.tsLabelSearchModifier.Text = "Modifier";
            this.tsLabelSearchModifier.Visible = false;
            // 
            // tscbSearchModifier
            // 
            this.tscbSearchModifier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbSearchModifier.Name = "tscbSearchModifier";
            this.tscbSearchModifier.Size = new System.Drawing.Size(95, 25);
            this.tscbSearchModifier.Visible = false;
            // 
            // tslblSearchValue
            // 
            this.tslblSearchValue.Name = "tslblSearchValue";
            this.tslblSearchValue.Size = new System.Drawing.Size(35, 22);
            this.tslblSearchValue.Text = "Value";
            // 
            // tstbValue
            // 
            this.tstbValue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tstbValue.Name = "tstbValue";
            this.tstbValue.Size = new System.Drawing.Size(95, 25);
            // 
            // tsbRunSearch
            // 
            this.tsbRunSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRunSearch.Image = ((System.Drawing.Image)(resources.GetObject("tsbRunSearch.Image")));
            this.tsbRunSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunSearch.Name = "tsbRunSearch";
            this.tsbRunSearch.Size = new System.Drawing.Size(23, 22);
            this.tsbRunSearch.Text = "Search";
            this.tsbRunSearch.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tabEdit
            // 
            this.tabEdit.Controls.Add(this.splitContainer1);
            this.tabEdit.Location = new System.Drawing.Point(4, 22);
            this.tabEdit.Name = "tabEdit";
            this.tabEdit.Padding = new System.Windows.Forms.Padding(3);
            this.tabEdit.Size = new System.Drawing.Size(914, 610);
            this.tabEdit.TabIndex = 0;
            this.tabEdit.Text = "Edit";
            this.tabEdit.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.toolStripContainer1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnlRulesets);
            this.splitContainer1.Panel2.Controls.Add(this.pnlGroups);
            this.splitContainer1.Panel2.Controls.Add(this.pnlGeneral);
            this.splitContainer1.Panel2MinSize = 676;
            this.splitContainer1.Size = new System.Drawing.Size(908, 604);
            this.splitContainer1.SplitterDistance = 216;
            this.splitContainer1.TabIndex = 1;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tvCollectionTree);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(216, 604);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(216, 604);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Padding = new System.Windows.Forms.Padding(0, 0, 25, 25);
            this.toolStripContainer1.TopToolStripPanelVisible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsbSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(108, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Visible = false;
            // 
            // tsmiFile
            // 
            this.tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAbout,
            this.tsmiSettings,
            this.tssSeparator1,
            this.tsmiProfileImport,
            this.tsmiMerge,
            this.tsmiRevert,
            this.tsmiLoadDefault,
            this.tsmiLoad,
            this.tsmiNew,
            this.tsmiSaveAsDefault,
            this.tsmiSaveAs,
            this.tsmiSave});
            this.tsmiFile.Image = ((System.Drawing.Image)(resources.GetObject("tsmiFile.Image")));
            this.tsmiFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsmiFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Size = new System.Drawing.Size(54, 22);
            this.tsmiFile.Text = "File";
            this.tsmiFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tsmiFile.DropDownOpening += new System.EventHandler(this.tsmiFile_DropDownOpening);
            // 
            // tsmiAbout
            // 
            this.tsmiAbout.Name = "tsmiAbout";
            this.tsmiAbout.Size = new System.Drawing.Size(253, 22);
            this.tsmiAbout.Text = "About";
            this.tsmiAbout.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tsmiSettings
            // 
            this.tsmiSettings.Name = "tsmiSettings";
            this.tsmiSettings.Size = new System.Drawing.Size(253, 22);
            this.tsmiSettings.Text = "Settings";
            this.tsmiSettings.Click += new System.EventHandler(this.tsmiSettings_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(250, 6);
            // 
            // tsmiProfileImport
            // 
            this.tsmiProfileImport.Name = "tsmiProfileImport";
            this.tsmiProfileImport.ShortcutKeyDisplayString = "Ctrl+I";
            this.tsmiProfileImport.Size = new System.Drawing.Size(253, 22);
            this.tsmiProfileImport.Text = "Import...";
            this.tsmiProfileImport.ToolTipText = resources.GetString("tsmiProfileImport.ToolTipText");
            this.tsmiProfileImport.Click += new System.EventHandler(this.tsmiProfileImport_Click);
            // 
            // tsmiMerge
            // 
            this.tsmiMerge.Image = ((System.Drawing.Image)(resources.GetObject("tsmiMerge.Image")));
            this.tsmiMerge.Name = "tsmiMerge";
            this.tsmiMerge.ShortcutKeyDisplayString = "Ctrl+M";
            this.tsmiMerge.Size = new System.Drawing.Size(253, 22);
            this.tsmiMerge.Text = "Merge...";
            this.tsmiMerge.ToolTipText = "Merge Current Profile With Another Profile File";
            this.tsmiMerge.Click += new System.EventHandler(this.tsmiMerge_Click);
            // 
            // tsmiRevert
            // 
            this.tsmiRevert.Image = ((System.Drawing.Image)(resources.GetObject("tsmiRevert.Image")));
            this.tsmiRevert.Name = "tsmiRevert";
            this.tsmiRevert.ShortcutKeyDisplayString = "Ctrl+R";
            this.tsmiRevert.Size = new System.Drawing.Size(253, 22);
            this.tsmiRevert.Text = "Revert";
            this.tsmiRevert.ToolTipText = "Reload the currently loaded file from last save.";
            this.tsmiRevert.Click += new System.EventHandler(this.tsmiRevert_Click);
            // 
            // tsmiLoadDefault
            // 
            this.tsmiLoadDefault.Image = ((System.Drawing.Image)(resources.GetObject("tsmiLoadDefault.Image")));
            this.tsmiLoadDefault.Name = "tsmiLoadDefault";
            this.tsmiLoadDefault.ShortcutKeyDisplayString = "Crtl+Shift+O";
            this.tsmiLoadDefault.Size = new System.Drawing.Size(253, 22);
            this.tsmiLoadDefault.Text = "Load Default Profile";
            this.tsmiLoadDefault.Click += new System.EventHandler(this.tsmiLoadDefault_Click);
            // 
            // tsmiLoad
            // 
            this.tsmiLoad.Image = ((System.Drawing.Image)(resources.GetObject("tsmiLoad.Image")));
            this.tsmiLoad.Name = "tsmiLoad";
            this.tsmiLoad.ShortcutKeyDisplayString = "Ctrl+O";
            this.tsmiLoad.Size = new System.Drawing.Size(253, 22);
            this.tsmiLoad.Text = "Load...";
            this.tsmiLoad.ToolTipText = "Open A new Profile File";
            this.tsmiLoad.Click += new System.EventHandler(this.tsmiLoad_Click);
            // 
            // tsmiNew
            // 
            this.tsmiNew.Image = ((System.Drawing.Image)(resources.GetObject("tsmiNew.Image")));
            this.tsmiNew.Name = "tsmiNew";
            this.tsmiNew.ShortcutKeyDisplayString = "Ctrl+N";
            this.tsmiNew.Size = new System.Drawing.Size(253, 22);
            this.tsmiNew.Text = "New";
            this.tsmiNew.Click += new System.EventHandler(this.tsmiNew_Click);
            // 
            // tsmiSaveAsDefault
            // 
            this.tsmiSaveAsDefault.Name = "tsmiSaveAsDefault";
            this.tsmiSaveAsDefault.ShortcutKeyDisplayString = "Ctrl+D";
            this.tsmiSaveAsDefault.Size = new System.Drawing.Size(253, 22);
            this.tsmiSaveAsDefault.Text = "Save As Default";
            this.tsmiSaveAsDefault.ToolTipText = "Save this file as the default dataman profile.";
            this.tsmiSaveAsDefault.Click += new System.EventHandler(this.tsmiSaveAsDefault_Click);
            // 
            // tsmiSaveAs
            // 
            this.tsmiSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("tsmiSaveAs.Image")));
            this.tsmiSaveAs.Name = "tsmiSaveAs";
            this.tsmiSaveAs.ShortcutKeyDisplayString = "Ctrl +Shift+S";
            this.tsmiSaveAs.Size = new System.Drawing.Size(253, 22);
            this.tsmiSaveAs.Text = "Save As...";
            this.tsmiSaveAs.ToolTipText = "Save File with different name.";
            this.tsmiSaveAs.Click += new System.EventHandler(this.tsmiSaveAs_Click);
            // 
            // tsmiSave
            // 
            this.tsmiSave.Image = ((System.Drawing.Image)(resources.GetObject("tsmiSave.Image")));
            this.tsmiSave.Name = "tsmiSave";
            this.tsmiSave.ShortcutKeyDisplayString = "Ctrl+S";
            this.tsmiSave.Size = new System.Drawing.Size(253, 22);
            this.tsmiSave.Text = "Save";
            this.tsmiSave.ToolTipText = "Save the currenly loaded profile file.";
            this.tsmiSave.Click += new System.EventHandler(this.tsmiSave_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbSave.Image")));
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(51, 22);
            this.tsbSave.Text = "Save";
            this.tsbSave.Click += new System.EventHandler(this.tsmiSave_Click);
            // 
            // tvCollectionTree
            // 
            this.tvCollectionTree.AllowDrop = true;
            this.tvCollectionTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvCollectionTree.HideSelection = false;
            this.tvCollectionTree.HotTracking = true;
            this.tvCollectionTree.ImageIndex = 0;
            this.tvCollectionTree.ImageList = this.imageList1;
            this.tvCollectionTree.Location = new System.Drawing.Point(0, 0);
            this.tvCollectionTree.Name = "tvCollectionTree";
            this.tvCollectionTree.SelectedImageIndex = 0;
            this.tvCollectionTree.Size = new System.Drawing.Size(216, 604);
            this.tvCollectionTree.TabIndex = 0;
            this.tvCollectionTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvCollectionTree_AfterLabelEdit);
            this.tvCollectionTree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvCollectionTree_ItemDrag);
            this.tvCollectionTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvCollectionTree_AfterSelect);
            this.tvCollectionTree.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvCollectionTree_DragDrop);
            this.tvCollectionTree.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvCollectionTree_DragEnter);
            this.tvCollectionTree.DragOver += new System.Windows.Forms.DragEventHandler(this.tvCollectionTree_DragOver);
            this.tvCollectionTree.DragLeave += new System.EventHandler(this.tvCollectionTree_DragLeave);
            this.tvCollectionTree.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.tvCollectionTree_GiveFeedback);
            this.tvCollectionTree.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.tvCollectionTree_QueryContinueDrag);
            this.tvCollectionTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tvCollectionTree_MouseUp);
            // 
            // pnlRulesets
            // 
            this.pnlRulesets.Controls.Add(this.dmpRules);
            this.pnlRulesets.Controls.Add(this.dmpActions);
            this.pnlRulesets.Controls.Add(this.comboBox1);
            this.pnlRulesets.Controls.Add(this.btnRulesetRulesMoveDown);
            this.pnlRulesets.Controls.Add(this.btnRulesetRulesMoveUp);
            this.pnlRulesets.Controls.Add(this.btnActionMoveDown);
            this.pnlRulesets.Controls.Add(this.btnActionMoveUp);
            this.pnlRulesets.Controls.Add(this.btnRulesetReparse);
            this.pnlRulesets.Controls.Add(this.pictureBox2);
            this.pnlRulesets.Controls.Add(lblRulesetOriginalText);
            this.pnlRulesets.Controls.Add(this.txtRulesetReparse);
            this.pnlRulesets.Controls.Add(this.btnRulesetRuleRemove);
            this.pnlRulesets.Controls.Add(this.btnRulesetRuleUpdate);
            this.pnlRulesets.Controls.Add(this.btnRulesetRuleClear);
            this.pnlRulesets.Controls.Add(commentLabel1);
            this.pnlRulesets.Controls.Add(this.txtRulesetComment);
            this.pnlRulesets.Controls.Add(this.btnRulesetRuleAdd);
            this.pnlRulesets.Controls.Add(nameLabel1);
            this.pnlRulesets.Controls.Add(this.txtRulesetName);
            this.pnlRulesets.Controls.Add(this.dgvRulesetRules);
            this.pnlRulesets.Controls.Add(this.dgvRulesetActions);
            this.pnlRulesets.Controls.Add(this.btnRulesetActionAdd);
            this.pnlRulesets.Controls.Add(this.label5);
            this.pnlRulesets.Controls.Add(this.label2);
            this.pnlRulesets.Controls.Add(this.btnRulesetActionClear);
            this.pnlRulesets.Controls.Add(this.pictureBox1);
            this.pnlRulesets.Controls.Add(this.label3);
            this.pnlRulesets.Controls.Add(this.btnRulesetActionUpdate);
            this.pnlRulesets.Controls.Add(this.btnRulesetActionRemove);
            this.pnlRulesets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRulesets.Location = new System.Drawing.Point(0, 0);
            this.pnlRulesets.Name = "pnlRulesets";
            this.pnlRulesets.Size = new System.Drawing.Size(688, 604);
            this.pnlRulesets.TabIndex = 4;
            // 
            // dmpRules
            // 
            this.dmpRules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dmpRules.Location = new System.Drawing.Point(11, 269);
            this.dmpRules.MaximumSize = new System.Drawing.Size(2000, 33);
            this.dmpRules.MinimumSize = new System.Drawing.Size(612, 33);
            this.dmpRules.Name = "dmpRules";
            this.dmpRules.ParameterTypeRestriction = DataManagerGUI.ParameterType.Rule;
            this.dmpRules.Size = new System.Drawing.Size(670, 33);
            this.dmpRules.TabIndex = 25;
            // 
            // dmpActions
            // 
            this.dmpActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dmpActions.Location = new System.Drawing.Point(14, 539);
            this.dmpActions.MaximumSize = new System.Drawing.Size(2000, 33);
            this.dmpActions.MinimumSize = new System.Drawing.Size(612, 33);
            this.dmpActions.Name = "dmpActions";
            this.dmpActions.ParameterTypeRestriction = DataManagerGUI.ParameterType.Action;
            this.dmpActions.Size = new System.Drawing.Size(670, 33);
            this.dmpActions.TabIndex = 24;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.RulesetBinder, "RuleMode", true));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "AND",
            "OR"});
            this.comboBox1.Location = new System.Drawing.Point(544, 66);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(130, 21);
            this.comboBox1.TabIndex = 23;
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnRulesetRulesMoveDown
            // 
            this.btnRulesetRulesMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRulesetRulesMoveDown.Enabled = false;
            this.btnRulesetRulesMoveDown.ImageIndex = 13;
            this.btnRulesetRulesMoveDown.ImageList = this.imageList1;
            this.btnRulesetRulesMoveDown.Location = new System.Drawing.Point(658, 243);
            this.btnRulesetRulesMoveDown.Name = "btnRulesetRulesMoveDown";
            this.btnRulesetRulesMoveDown.Size = new System.Drawing.Size(25, 25);
            this.btnRulesetRulesMoveDown.TabIndex = 17;
            this.btnRulesetRulesMoveDown.UseVisualStyleBackColor = true;
            this.btnRulesetRulesMoveDown.Click += new System.EventHandler(this.btnRulesetRulesMoveDown_Click);
            // 
            // btnRulesetRulesMoveUp
            // 
            this.btnRulesetRulesMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRulesetRulesMoveUp.Enabled = false;
            this.btnRulesetRulesMoveUp.ImageIndex = 12;
            this.btnRulesetRulesMoveUp.ImageList = this.imageList1;
            this.btnRulesetRulesMoveUp.Location = new System.Drawing.Point(658, 217);
            this.btnRulesetRulesMoveUp.Name = "btnRulesetRulesMoveUp";
            this.btnRulesetRulesMoveUp.Size = new System.Drawing.Size(25, 25);
            this.btnRulesetRulesMoveUp.TabIndex = 16;
            this.btnRulesetRulesMoveUp.UseVisualStyleBackColor = true;
            this.btnRulesetRulesMoveUp.Click += new System.EventHandler(this.btnRulesetRulesMoveUp_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.Black;
            this.pictureBox2.Location = new System.Drawing.Point(55, 76);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(400, 1);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // txtRulesetReparse
            // 
            this.txtRulesetReparse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRulesetReparse.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.RulesetBinder, "OriginalText", true));
            this.txtRulesetReparse.Location = new System.Drawing.Point(56, 33);
            this.txtRulesetReparse.Name = "txtRulesetReparse";
            this.txtRulesetReparse.Size = new System.Drawing.Size(594, 20);
            this.txtRulesetReparse.TabIndex = 5;
            // 
            // txtRulesetComment
            // 
            this.txtRulesetComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRulesetComment.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.RulesetBinder, "Comment", true));
            this.txtRulesetComment.Location = new System.Drawing.Point(397, 4);
            this.txtRulesetComment.Name = "txtRulesetComment";
            this.txtRulesetComment.Size = new System.Drawing.Size(284, 20);
            this.txtRulesetComment.TabIndex = 3;
            this.txtRulesetComment.Validating += new System.ComponentModel.CancelEventHandler(this.txtRulesetComment_Validating);
            this.txtRulesetComment.Validated += new System.EventHandler(this.txtRulesetComment_Validated);
            // 
            // txtRulesetName
            // 
            this.txtRulesetName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.RulesetBinder, "Name", true));
            this.txtRulesetName.Location = new System.Drawing.Point(97, 4);
            this.txtRulesetName.Name = "txtRulesetName";
            this.txtRulesetName.Size = new System.Drawing.Size(234, 20);
            this.txtRulesetName.TabIndex = 1;
            this.txtRulesetName.TextChanged += new System.EventHandler(this.txtRulesetName_TextChanged);
            this.txtRulesetName.Validating += new System.ComponentModel.CancelEventHandler(this.txtRulesetName_Validating);
            this.txtRulesetName.Validated += new System.EventHandler(this.txtRulesetName_Validated);
            // 
            // dgvRulesetRules
            // 
            this.dgvRulesetRules.AllowUserToAddRows = false;
            this.dgvRulesetRules.AllowUserToDeleteRows = false;
            this.dgvRulesetRules.AllowUserToResizeRows = false;
            this.dgvRulesetRules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRulesetRules.AutoGenerateColumns = false;
            this.dgvRulesetRules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRulesetRules.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvRulesetRules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dgvtcRulesetRuleModifier,
            this.dgvtcRulesetRuleValue});
            this.dgvRulesetRules.DataSource = this.RuleBinder;
            this.dgvRulesetRules.Location = new System.Drawing.Point(11, 89);
            this.dgvRulesetRules.MultiSelect = false;
            this.dgvRulesetRules.Name = "dgvRulesetRules";
            this.dgvRulesetRules.ReadOnly = true;
            this.dgvRulesetRules.RowHeadersVisible = false;
            this.dgvRulesetRules.RowHeadersWidth = 25;
            this.dgvRulesetRules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRulesetRules.Size = new System.Drawing.Size(643, 179);
            this.dgvRulesetRules.TabIndex = 8;
            this.dgvRulesetRules.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvRulesetRules_MouseUp);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Field";
            this.dataGridViewTextBoxColumn3.HeaderText = "Field";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dgvtcRulesetRuleModifier
            // 
            this.dgvtcRulesetRuleModifier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgvtcRulesetRuleModifier.DataPropertyName = "Modifier";
            this.dgvtcRulesetRuleModifier.HeaderText = "Modifier";
            this.dgvtcRulesetRuleModifier.Name = "dgvtcRulesetRuleModifier";
            this.dgvtcRulesetRuleModifier.ReadOnly = true;
            this.dgvtcRulesetRuleModifier.Width = 150;
            // 
            // dgvtcRulesetRuleValue
            // 
            this.dgvtcRulesetRuleValue.DataPropertyName = "Value";
            this.dgvtcRulesetRuleValue.HeaderText = "Value";
            this.dgvtcRulesetRuleValue.Name = "dgvtcRulesetRuleValue";
            this.dgvtcRulesetRuleValue.ReadOnly = true;
            this.dgvtcRulesetRuleValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvRulesetActions
            // 
            this.dgvRulesetActions.AllowUserToAddRows = false;
            this.dgvRulesetActions.AllowUserToDeleteRows = false;
            this.dgvRulesetActions.AllowUserToResizeRows = false;
            this.dgvRulesetActions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRulesetActions.AutoGenerateColumns = false;
            this.dgvRulesetActions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRulesetActions.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvRulesetActions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dgvtcRulesetActionModifier,
            this.dgvtcRulesetActionValue});
            this.dgvRulesetActions.DataSource = this.ActionBinder;
            this.dgvRulesetActions.Location = new System.Drawing.Point(12, 350);
            this.dgvRulesetActions.MultiSelect = false;
            this.dgvRulesetActions.Name = "dgvRulesetActions";
            this.dgvRulesetActions.ReadOnly = true;
            this.dgvRulesetActions.RowHeadersVisible = false;
            this.dgvRulesetActions.RowHeadersWidth = 25;
            this.dgvRulesetActions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRulesetActions.Size = new System.Drawing.Size(642, 185);
            this.dgvRulesetActions.TabIndex = 15;
            this.dgvRulesetActions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvRulesetActions_MouseUp);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Field";
            this.dataGridViewTextBoxColumn2.HeaderText = "Field";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dgvtcRulesetActionModifier
            // 
            this.dgvtcRulesetActionModifier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgvtcRulesetActionModifier.DataPropertyName = "Modifier";
            this.dgvtcRulesetActionModifier.HeaderText = "Modifier";
            this.dgvtcRulesetActionModifier.Name = "dgvtcRulesetActionModifier";
            this.dgvtcRulesetActionModifier.ReadOnly = true;
            this.dgvtcRulesetActionModifier.Width = 150;
            // 
            // dgvtcRulesetActionValue
            // 
            this.dgvtcRulesetActionValue.DataPropertyName = "Value";
            this.dgvtcRulesetActionValue.HeaderText = "Value";
            this.dgvtcRulesetActionValue.Name = "dgvtcRulesetActionValue";
            this.dgvtcRulesetActionValue.ReadOnly = true;
            this.dgvtcRulesetActionValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(464, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Rule Mode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Rules";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(67, 337);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(607, 1);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 330);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Actions";
            //
            // pnlGroups
            // 
            this.pnlGroups.Controls.Add(this.tabGroupTabs);
            this.pnlGroups.Controls.Add(this.lblGroupOverview);
            this.pnlGroups.Controls.Add(this.commentLabel);
            this.pnlGroups.Controls.Add(this.txtGroupComment);
            this.pnlGroups.Controls.Add(this.nameLabel);
            this.pnlGroups.Controls.Add(this.txtGroupName);
            this.pnlGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGroups.Location = new System.Drawing.Point(0, 0);
            this.pnlGroups.Name = "pnlGroups";
            this.pnlGroups.Size = new System.Drawing.Size(688, 604);
            this.pnlGroups.TabIndex = 3;
            // 
            // tabGroupTabs
            // 
            this.tabGroupTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabGroupTabs.Controls.Add(this.tabPage3);
            this.tabGroupTabs.Controls.Add(this.tabGrpRuleset);
            this.tabGroupTabs.Location = new System.Drawing.Point(3, 112);
            this.tabGroupTabs.Name = "tabGroupTabs";
            this.tabGroupTabs.SelectedIndex = 0;
            this.tabGroupTabs.Size = new System.Drawing.Size(685, 489);
            this.tabGroupTabs.TabIndex = 16;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label19);
            this.tabPage3.Controls.Add(this.btnGroupRulesetRemove);
            this.tabPage3.Controls.Add(this.btnGroupGroupRemove);
            this.tabPage3.Controls.Add(this.btnGroupRulesetMoveDown);
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Controls.Add(this.dgvGroupRulesets);
            this.tabPage3.Controls.Add(this.btnGroupRulesetMoveUp);
            this.tabPage3.Controls.Add(this.dgvGroupGroups);
            this.tabPage3.Controls.Add(this.btnGroupGroupMoveDown);
            this.tabPage3.Controls.Add(this.btnGroupRulesetAdd);
            this.tabPage3.Controls.Add(this.dataGridView5);
            this.tabPage3.Controls.Add(this.btnGroupGroupMoveUp);
            this.tabPage3.Controls.Add(this.btnGroupGroupAdd);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.button2);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(677, 463);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Groups & Rulesets";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(6, 7);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(134, 16);
            this.label19.TabIndex = 4;
            this.label19.Text = "Groups In This Group";
            this.label19.UseMnemonic = false;
            // 
            // btnGroupRulesetRemove
            // 
            this.btnGroupRulesetRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupRulesetRemove.Enabled = false;
            this.btnGroupRulesetRemove.ImageIndex = 18;
            this.btnGroupRulesetRemove.ImageList = this.imageList1;
            this.btnGroupRulesetRemove.Location = new System.Drawing.Point(560, 430);
            this.btnGroupRulesetRemove.Name = "btnGroupRulesetRemove";
            this.btnGroupRulesetRemove.Size = new System.Drawing.Size(83, 23);
            this.btnGroupRulesetRemove.TabIndex = 15;
            this.btnGroupRulesetRemove.Text = "Remove";
            this.btnGroupRulesetRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGroupRulesetRemove.UseVisualStyleBackColor = true;
            this.btnGroupRulesetRemove.Click += new System.EventHandler(this.btnGroupRulesetRemove_Click);
            // 
            // btnGroupGroupRemove
            // 
            this.btnGroupGroupRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupGroupRemove.Enabled = false;
            this.btnGroupGroupRemove.ImageIndex = 18;
            this.btnGroupGroupRemove.ImageList = this.imageList1;
            this.btnGroupGroupRemove.Location = new System.Drawing.Point(560, 209);
            this.btnGroupGroupRemove.Name = "btnGroupGroupRemove";
            this.btnGroupGroupRemove.Size = new System.Drawing.Size(83, 23);
            this.btnGroupGroupRemove.TabIndex = 9;
            this.btnGroupGroupRemove.Text = "Remove";
            this.btnGroupGroupRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGroupGroupRemove.UseVisualStyleBackColor = true;
            this.btnGroupGroupRemove.Click += new System.EventHandler(this.btnGroupGroupRemove_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Enabled = false;
            this.button1.ImageIndex = 18;
            this.button1.Location = new System.Drawing.Point(560, 209);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Remove";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnGroupGroupRemove_Click);
            // 
            // dgvGroupRulesets
            // 
            this.dgvGroupRulesets.AllowUserToAddRows = false;
            this.dgvGroupRulesets.AllowUserToDeleteRows = false;
            this.dgvGroupRulesets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGroupRulesets.AutoGenerateColumns = false;
            this.dgvGroupRulesets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGroupRulesets.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvGroupRulesets.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvGroupRulesets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGroupRulesets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.quickViewDataGridViewTextBoxColumn,
            this.commentDataGridViewTextBoxColumn});
            this.dgvGroupRulesets.DataSource = this.GroupRulesetBinder;
            this.dgvGroupRulesets.Location = new System.Drawing.Point(1, 254);
            this.dgvGroupRulesets.MultiSelect = false;
            this.dgvGroupRulesets.Name = "dgvGroupRulesets";
            this.dgvGroupRulesets.RowHeadersWidth = 25;
            this.dgvGroupRulesets.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvGroupRulesets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGroupRulesets.Size = new System.Drawing.Size(642, 168);
            this.dgvGroupRulesets.TabIndex = 11;
            this.dgvGroupRulesets.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGroupRulesets_CellEndEdit);
            this.dgvGroupRulesets.Sorted += new System.EventHandler(this.DataGrid_Sorted);
            this.dgvGroupRulesets.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvGroupRulesets_MouseUp);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // quickViewDataGridViewTextBoxColumn
            // 
            this.quickViewDataGridViewTextBoxColumn.DataPropertyName = "QuickView";
            this.quickViewDataGridViewTextBoxColumn.HeaderText = "QuickView";
            this.quickViewDataGridViewTextBoxColumn.Name = "quickViewDataGridViewTextBoxColumn";
            this.quickViewDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // commentDataGridViewTextBoxColumn
            // 
            this.commentDataGridViewTextBoxColumn.DataPropertyName = "Comment";
            this.commentDataGridViewTextBoxColumn.HeaderText = "Comment";
            this.commentDataGridViewTextBoxColumn.Name = "commentDataGridViewTextBoxColumn";
            // 
            // dgvGroupGroups
            // 
            this.dgvGroupGroups.AllowUserToAddRows = false;
            this.dgvGroupGroups.AllowUserToDeleteRows = false;
            this.dgvGroupGroups.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGroupGroups.AutoGenerateColumns = false;
            this.dgvGroupGroups.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGroupGroups.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvGroupGroups.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvGroupGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGroupGroups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtcGroupGroupName,
            this.dgvtcGroupGroupComment,
            this.dgvtcGroupGroupGroupCount,
            this.dgvtcGroupGroupRulesetCount});
            this.dgvGroupGroups.DataSource = this.GroupGroupBinder;
            this.dgvGroupGroups.Location = new System.Drawing.Point(3, 30);
            this.dgvGroupGroups.MultiSelect = false;
            this.dgvGroupGroups.Name = "dgvGroupGroups";
            this.dgvGroupGroups.RowHeadersWidth = 25;
            this.dgvGroupGroups.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvGroupGroups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGroupGroups.Size = new System.Drawing.Size(642, 173);
            this.dgvGroupGroups.TabIndex = 5;
            this.dgvGroupGroups.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGroupGroups_CellEndEdit);
            this.dgvGroupGroups.Sorted += new System.EventHandler(this.DataGrid_Sorted);
            this.dgvGroupGroups.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvGroupGroups_MouseUp);
            // 
            // dgvtcGroupGroupName
            // 
            this.dgvtcGroupGroupName.DataPropertyName = "Name";
            this.dgvtcGroupGroupName.HeaderText = "Name";
            this.dgvtcGroupGroupName.Name = "dgvtcGroupGroupName";
            // 
            // dgvtcGroupGroupComment
            // 
            this.dgvtcGroupGroupComment.DataPropertyName = "Comment";
            this.dgvtcGroupGroupComment.HeaderText = "Comment";
            this.dgvtcGroupGroupComment.Name = "dgvtcGroupGroupComment";
            this.dgvtcGroupGroupComment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtcGroupGroupGroupCount
            // 
            this.dgvtcGroupGroupGroupCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dgvtcGroupGroupGroupCount.DataPropertyName = "GroupCount";
            this.dgvtcGroupGroupGroupCount.HeaderText = "Groups";
            this.dgvtcGroupGroupGroupCount.Name = "dgvtcGroupGroupGroupCount";
            this.dgvtcGroupGroupGroupCount.ReadOnly = true;
            this.dgvtcGroupGroupGroupCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvtcGroupGroupGroupCount.Width = 47;
            // 
            // dgvtcGroupGroupRulesetCount
            // 
            this.dgvtcGroupGroupRulesetCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dgvtcGroupGroupRulesetCount.DataPropertyName = "RulesetCount";
            this.dgvtcGroupGroupRulesetCount.HeaderText = "Rulesets";
            this.dgvtcGroupGroupRulesetCount.Name = "dgvtcGroupGroupRulesetCount";
            this.dgvtcGroupGroupRulesetCount.ReadOnly = true;
            this.dgvtcGroupGroupRulesetCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvtcGroupGroupRulesetCount.Width = 54;
            // 
            // btnGroupRulesetAdd
            // 
            this.btnGroupRulesetAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupRulesetAdd.ImageIndex = 17;
            this.btnGroupRulesetAdd.ImageList = this.imageList1;
            this.btnGroupRulesetAdd.Location = new System.Drawing.Point(475, 430);
            this.btnGroupRulesetAdd.Name = "btnGroupRulesetAdd";
            this.btnGroupRulesetAdd.Size = new System.Drawing.Size(83, 23);
            this.btnGroupRulesetAdd.TabIndex = 14;
            this.btnGroupRulesetAdd.Text = "Add";
            this.btnGroupRulesetAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGroupRulesetAdd.UseVisualStyleBackColor = true;
            this.btnGroupRulesetAdd.Click += new System.EventHandler(this.btnGroupRulesetAdd_Click);
            // 
            // dataGridView5
            // 
            this.dataGridView5.AllowUserToAddRows = false;
            this.dataGridView5.AllowUserToDeleteRows = false;
            this.dataGridView5.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView5.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView5.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dataGridView5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView5.Location = new System.Drawing.Point(3, 30);
            this.dataGridView5.MultiSelect = false;
            this.dataGridView5.Name = "dataGridView5";
            this.dataGridView5.RowHeadersWidth = 25;
            this.dataGridView5.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView5.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView5.Size = new System.Drawing.Size(642, 173);
            this.dataGridView5.TabIndex = 5;
            this.dataGridView5.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGroupGroups_CellEndEdit);
            this.dataGridView5.Sorted += new System.EventHandler(this.DataGrid_Sorted);
            this.dataGridView5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvGroupGroups_MouseUp);
            // 
            // btnGroupGroupAdd
            // 
            this.btnGroupGroupAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGroupGroupAdd.ImageIndex = 17;
            this.btnGroupGroupAdd.ImageList = this.imageList1;
            this.btnGroupGroupAdd.Location = new System.Drawing.Point(475, 209);
            this.btnGroupGroupAdd.Name = "btnGroupGroupAdd";
            this.btnGroupGroupAdd.Size = new System.Drawing.Size(83, 23);
            this.btnGroupGroupAdd.TabIndex = 8;
            this.btnGroupGroupAdd.Text = "Add";
            this.btnGroupGroupAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGroupGroupAdd.UseVisualStyleBackColor = true;
            this.btnGroupGroupAdd.Click += new System.EventHandler(this.btnGroupGroupAdd_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(5, 232);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(143, 16);
            this.label8.TabIndex = 10;
            this.label8.Text = "Rulesets In This Group";
            this.label8.UseMnemonic = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.ImageIndex = 17;
            this.button2.Location = new System.Drawing.Point(475, 209);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(83, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Add";
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnGroupGroupAdd_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(6, 7);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(134, 16);
            this.label13.TabIndex = 4;
            this.label13.Text = "Groups In This Group";
            this.label13.UseMnemonic = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(5, 232);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(143, 16);
            this.label7.TabIndex = 10;
            this.label7.Text = "Rulesets In This Group";
            this.label7.UseMnemonic = false;
            // 
            // tabGrpRuleset
            // 
            this.tabGrpRuleset.Controls.Add(this.pmGroupDefaultActions);
            this.tabGrpRuleset.Controls.Add(this.pmGroupFilters);
            this.tabGrpRuleset.Controls.Add(this.btnGroupFiltersMoveDown);
            this.tabGrpRuleset.Controls.Add(this.btnGroupFiltersMoveUp);
            this.tabGrpRuleset.Controls.Add(this.comboBox2);
            this.tabGrpRuleset.Controls.Add(this.btnGroupDefaultMoveDown);
            this.tabGrpRuleset.Controls.Add(this.btnGroupDefaultMoveUp);
            this.tabGrpRuleset.Controls.Add(this.pictureBox3);
            this.tabGrpRuleset.Controls.Add(this.btnGroupFilterRemove);
            this.tabGrpRuleset.Controls.Add(this.btnGroupFilterUpdate);
            this.tabGrpRuleset.Controls.Add(this.btnGroupFilterClear);
            this.tabGrpRuleset.Controls.Add(this.btnGroupFilterAdd);
            this.tabGrpRuleset.Controls.Add(this.dgvGroupFilters);
            this.tabGrpRuleset.Controls.Add(this.dgvGroupDefaults);
            this.tabGrpRuleset.Controls.Add(this.btnGroupDefaultAdd);
            this.tabGrpRuleset.Controls.Add(this.label21);
            this.tabGrpRuleset.Controls.Add(this.label22);
            this.tabGrpRuleset.Controls.Add(this.btnGroupDefaultClear);
            this.tabGrpRuleset.Controls.Add(this.pictureBox4);
            this.tabGrpRuleset.Controls.Add(this.label23);
            this.tabGrpRuleset.Controls.Add(this.btnGroupDefaultUpdate);
            this.tabGrpRuleset.Controls.Add(this.btnGroupDefaultRemove);
            this.tabGrpRuleset.Location = new System.Drawing.Point(4, 22);
            this.tabGrpRuleset.Name = "tabGrpRuleset";
            this.tabGrpRuleset.Padding = new System.Windows.Forms.Padding(3);
            this.tabGrpRuleset.Size = new System.Drawing.Size(677, 463);
            this.tabGrpRuleset.TabIndex = 1;
            this.tabGrpRuleset.Text = "Filters & Defaults";
            this.tabGrpRuleset.UseVisualStyleBackColor = true;
            this.tabGrpRuleset.Click += new System.EventHandler(this.tabPage4_Click);
            // 
            // pmGroupDefaultActions
            // 
            this.pmGroupDefaultActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pmGroupDefaultActions.Location = new System.Drawing.Point(8, 403);
            this.pmGroupDefaultActions.MaximumSize = new System.Drawing.Size(2000, 33);
            this.pmGroupDefaultActions.MinimumSize = new System.Drawing.Size(612, 33);
            this.pmGroupDefaultActions.Name = "pmGroupDefaultActions";
            this.pmGroupDefaultActions.ParameterTypeRestriction = DataManagerGUI.ParameterType.Action;
            this.pmGroupDefaultActions.Size = new System.Drawing.Size(661, 33);
            this.pmGroupDefaultActions.TabIndex = 47;
            // 
            // pmGroupFilters
            // 
            this.pmGroupFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pmGroupFilters.Location = new System.Drawing.Point(8, 174);
            this.pmGroupFilters.MaximumSize = new System.Drawing.Size(2000, 33);
            this.pmGroupFilters.MinimumSize = new System.Drawing.Size(612, 33);
            this.pmGroupFilters.Name = "pmGroupFilters";
            this.pmGroupFilters.ParameterTypeRestriction = DataManagerGUI.ParameterType.Rule;
            this.pmGroupFilters.Size = new System.Drawing.Size(661, 33);
            this.pmGroupFilters.TabIndex = 46;
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox2.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.GroupFiltersAndDefaultsBinders, "RuleMode", true));
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "AND",
            "OR"});
            this.comboBox2.Location = new System.Drawing.Point(541, 8);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(102, 21);
            this.comboBox2.TabIndex = 43;
            this.comboBox2.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox3.BackColor = System.Drawing.Color.Black;
            this.pictureBox3.Location = new System.Drawing.Point(96, 18);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(353, 1);
            this.pictureBox3.TabIndex = 24;
            this.pictureBox3.TabStop = false;
            // 
            // dgvGroupFilters
            // 
            this.dgvGroupFilters.AllowUserToAddRows = false;
            this.dgvGroupFilters.AllowUserToDeleteRows = false;
            this.dgvGroupFilters.AllowUserToResizeRows = false;
            this.dgvGroupFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGroupFilters.AutoGenerateColumns = false;
            this.dgvGroupFilters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGroupFilters.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvGroupFilters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Field,
            this.modifierDataGridViewTextBoxColumn1,
            this.valueDataGridViewTextBoxColumn2});
            this.dgvGroupFilters.DataSource = this.GroupFiltersBinder;
            this.dgvGroupFilters.Location = new System.Drawing.Point(8, 31);
            this.dgvGroupFilters.MultiSelect = false;
            this.dgvGroupFilters.Name = "dgvGroupFilters";
            this.dgvGroupFilters.ReadOnly = true;
            this.dgvGroupFilters.RowHeadersVisible = false;
            this.dgvGroupFilters.RowHeadersWidth = 25;
            this.dgvGroupFilters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGroupFilters.Size = new System.Drawing.Size(635, 137);
            this.dgvGroupFilters.TabIndex = 28;
            this.dgvGroupFilters.Sorted += new System.EventHandler(this.DataGrid_Sorted);
            this.dgvGroupFilters.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvGroupFilters_MouseUp);
            // 
            // Field
            // 
            this.Field.DataPropertyName = "Field";
            this.Field.HeaderText = "Field";
            this.Field.Name = "Field";
            this.Field.ReadOnly = true;
            // 
            // modifierDataGridViewTextBoxColumn1
            // 
            this.modifierDataGridViewTextBoxColumn1.DataPropertyName = "Modifier";
            this.modifierDataGridViewTextBoxColumn1.HeaderText = "Modifier";
            this.modifierDataGridViewTextBoxColumn1.Name = "modifierDataGridViewTextBoxColumn1";
            this.modifierDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // valueDataGridViewTextBoxColumn2
            // 
            this.valueDataGridViewTextBoxColumn2.DataPropertyName = "Value";
            this.valueDataGridViewTextBoxColumn2.HeaderText = "Value";
            this.valueDataGridViewTextBoxColumn2.Name = "valueDataGridViewTextBoxColumn2";
            this.valueDataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dgvGroupDefaults
            // 
            this.dgvGroupDefaults.AllowUserToAddRows = false;
            this.dgvGroupDefaults.AllowUserToDeleteRows = false;
            this.dgvGroupDefaults.AllowUserToResizeRows = false;
            this.dgvGroupDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGroupDefaults.AutoGenerateColumns = false;
            this.dgvGroupDefaults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGroupDefaults.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvGroupDefaults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.modifierDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn1});
            this.dgvGroupDefaults.DataSource = this.GroupDefaultsBinder;
            this.dgvGroupDefaults.Location = new System.Drawing.Point(8, 263);
            this.dgvGroupDefaults.MultiSelect = false;
            this.dgvGroupDefaults.Name = "dgvGroupDefaults";
            this.dgvGroupDefaults.ReadOnly = true;
            this.dgvGroupDefaults.RowHeadersVisible = false;
            this.dgvGroupDefaults.RowHeadersWidth = 25;
            this.dgvGroupDefaults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGroupDefaults.Size = new System.Drawing.Size(635, 135);
            this.dgvGroupDefaults.TabIndex = 35;
            this.dgvGroupDefaults.Sorted += new System.EventHandler(this.DataGrid_Sorted);
            this.dgvGroupDefaults.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvGroupDefaults_MouseUp);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Field";
            this.dataGridViewTextBoxColumn1.HeaderText = "Field";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // modifierDataGridViewTextBoxColumn
            // 
            this.modifierDataGridViewTextBoxColumn.DataPropertyName = "Modifier";
            this.modifierDataGridViewTextBoxColumn.HeaderText = "Modifier";
            this.modifierDataGridViewTextBoxColumn.Name = "modifierDataGridViewTextBoxColumn";
            this.modifierDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // valueDataGridViewTextBoxColumn1
            // 
            this.valueDataGridViewTextBoxColumn1.DataPropertyName = "Value";
            this.valueDataGridViewTextBoxColumn1.HeaderText = "Value";
            this.valueDataGridViewTextBoxColumn1.Name = "valueDataGridViewTextBoxColumn1";
            this.valueDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(461, 11);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(68, 13);
            this.label21.TabIndex = 25;
            this.label21.Text = "Rule Mode";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(10, 12);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(77, 13);
            this.label22.TabIndex = 26;
            this.label22.Text = "Group Rules";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox4.BackColor = System.Drawing.Color.Black;
            this.pictureBox4.Location = new System.Drawing.Point(103, 249);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(560, 1);
            this.pictureBox4.TabIndex = 27;
            this.pictureBox4.TabStop = false;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(12, 242);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(87, 13);
            this.label23.TabIndex = 34;
            this.label23.Text = "Group Actions";
            // 
            // lblGroupOverview
            // 
            this.lblGroupOverview.AutoSize = true;
            this.lblGroupOverview.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGroupOverview.Location = new System.Drawing.Point(13, 9);
            this.lblGroupOverview.Name = "lblGroupOverview";
            this.lblGroupOverview.Size = new System.Drawing.Size(268, 39);
            this.lblGroupOverview.TabIndex = 6;
            this.lblGroupOverview.Text = "Group Overview";
            this.lblGroupOverview.UseMnemonic = false;
            // 
            // commentLabel
            // 
            this.commentLabel.AutoSize = true;
            this.commentLabel.Location = new System.Drawing.Point(7, 89);
            this.commentLabel.Name = "commentLabel";
            this.commentLabel.Size = new System.Drawing.Size(54, 13);
            this.commentLabel.TabIndex = 2;
            this.commentLabel.Text = "Comment:";
            this.commentLabel.UseMnemonic = false;
            // 
            // txtGroupComment
            // 
            this.txtGroupComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGroupComment.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.GroupBinder, "Comment", true));
            this.txtGroupComment.Location = new System.Drawing.Point(67, 86);
            this.txtGroupComment.Name = "txtGroupComment";
            this.txtGroupComment.Size = new System.Drawing.Size(587, 20);
            this.txtGroupComment.TabIndex = 3;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(7, 63);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(38, 13);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Name:";
            this.nameLabel.UseMnemonic = false;
            // 
            // txtGroupName
            // 
            this.txtGroupName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.GroupBinder, "Name", true));
            this.txtGroupName.Location = new System.Drawing.Point(67, 60);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(248, 20);
            this.txtGroupName.TabIndex = 1;
            this.txtGroupName.Validated += new System.EventHandler(this.txtGroupName_Validated);
            // 
            // pnlGeneral
            // 
            this.pnlGeneral.Controls.Add(this.btnCollectionRulesetMoveDown);
            this.pnlGeneral.Controls.Add(this.btnCollectionRulesetMoveUp);
            this.pnlGeneral.Controls.Add(this.btnCollectionGroupMoveDown);
            this.pnlGeneral.Controls.Add(this.btnCollectionGroupMoveUp);
            this.pnlGeneral.Controls.Add(this.label20);
            this.pnlGeneral.Controls.Add(this.label4);
            this.pnlGeneral.Controls.Add(this.label18);
            this.pnlGeneral.Controls.Add(this.txtCollectionNotes);
            this.pnlGeneral.Controls.Add(this.textBox2);
            this.pnlGeneral.Controls.Add(this.txtCollectionAuthor);
            this.pnlGeneral.Controls.Add(this.label17);
            this.pnlGeneral.Controls.Add(this.label14);
            this.pnlGeneral.Controls.Add(this.label15);
            this.pnlGeneral.Controls.Add(this.label16);
            this.pnlGeneral.Controls.Add(this.btnCollectionRulesetAdd);
            this.pnlGeneral.Controls.Add(this.btnCollectionGroupAdd);
            this.pnlGeneral.Controls.Add(this.dgvCollectionRulesets);
            this.pnlGeneral.Controls.Add(this.btnCollectionRulesetsRemove);
            this.pnlGeneral.Controls.Add(this.dgvCollectionGroups);
            this.pnlGeneral.Controls.Add(this.btnCollectionGroupRemove);
            this.pnlGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGeneral.Location = new System.Drawing.Point(0, 0);
            this.pnlGeneral.Name = "pnlGeneral";
            this.pnlGeneral.Size = new System.Drawing.Size(688, 604);
            this.pnlGeneral.TabIndex = 2;
            this.pnlGeneral.Visible = false;
            // 
            // btnCollectionRulesetMoveDown
            // 
            this.btnCollectionRulesetMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollectionRulesetMoveDown.Enabled = false;
            this.btnCollectionRulesetMoveDown.ImageIndex = 13;
            this.btnCollectionRulesetMoveDown.ImageList = this.imageList1;
            this.btnCollectionRulesetMoveDown.Location = new System.Drawing.Point(650, 392);
            this.btnCollectionRulesetMoveDown.Name = "btnCollectionRulesetMoveDown";
            this.btnCollectionRulesetMoveDown.Size = new System.Drawing.Size(25, 25);
            this.btnCollectionRulesetMoveDown.TabIndex = 10;
            this.btnCollectionRulesetMoveDown.UseVisualStyleBackColor = true;
            this.btnCollectionRulesetMoveDown.Click += new System.EventHandler(this.btnCollectionRulesetMoveDown_Click);
            // 
            // btnCollectionRulesetMoveUp
            // 
            this.btnCollectionRulesetMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollectionRulesetMoveUp.Enabled = false;
            this.btnCollectionRulesetMoveUp.ImageIndex = 12;
            this.btnCollectionRulesetMoveUp.ImageList = this.imageList1;
            this.btnCollectionRulesetMoveUp.Location = new System.Drawing.Point(650, 366);
            this.btnCollectionRulesetMoveUp.Name = "btnCollectionRulesetMoveUp";
            this.btnCollectionRulesetMoveUp.Size = new System.Drawing.Size(25, 25);
            this.btnCollectionRulesetMoveUp.TabIndex = 9;
            this.btnCollectionRulesetMoveUp.UseVisualStyleBackColor = true;
            this.btnCollectionRulesetMoveUp.Click += new System.EventHandler(this.btnCollectionRulesetMoveUp_Click);
            // 
            // btnCollectionGroupMoveDown
            // 
            this.btnCollectionGroupMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollectionGroupMoveDown.Enabled = false;
            this.btnCollectionGroupMoveDown.ImageIndex = 13;
            this.btnCollectionGroupMoveDown.ImageList = this.imageList1;
            this.btnCollectionGroupMoveDown.Location = new System.Drawing.Point(650, 202);
            this.btnCollectionGroupMoveDown.Name = "btnCollectionGroupMoveDown";
            this.btnCollectionGroupMoveDown.Size = new System.Drawing.Size(25, 25);
            this.btnCollectionGroupMoveDown.TabIndex = 4;
            this.btnCollectionGroupMoveDown.UseVisualStyleBackColor = true;
            this.btnCollectionGroupMoveDown.Click += new System.EventHandler(this.btnCollectionGroupMoveDown_Click);
            // 
            // btnCollectionGroupMoveUp
            // 
            this.btnCollectionGroupMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollectionGroupMoveUp.Enabled = false;
            this.btnCollectionGroupMoveUp.ImageIndex = 12;
            this.btnCollectionGroupMoveUp.ImageList = this.imageList1;
            this.btnCollectionGroupMoveUp.Location = new System.Drawing.Point(650, 176);
            this.btnCollectionGroupMoveUp.Name = "btnCollectionGroupMoveUp";
            this.btnCollectionGroupMoveUp.Size = new System.Drawing.Size(25, 25);
            this.btnCollectionGroupMoveUp.TabIndex = 3;
            this.btnCollectionGroupMoveUp.UseVisualStyleBackColor = true;
            this.btnCollectionGroupMoveUp.Click += new System.EventHandler(this.btnCollectionGroupMoveUp_Click);
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(20, 501);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(35, 13);
            this.label20.TabIndex = 15;
            this.label20.Text = "Notes";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(479, 473);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Compatible Version:";
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(20, 470);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 13);
            this.label18.TabIndex = 10;
            this.label18.Text = "Name";
            // 
            // txtCollectionNotes
            // 
            this.txtCollectionNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCollectionNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.CollectionBinder, "Notes", true));
            this.txtCollectionNotes.Location = new System.Drawing.Point(67, 501);
            this.txtCollectionNotes.Multiline = true;
            this.txtCollectionNotes.Name = "txtCollectionNotes";
            this.txtCollectionNotes.Size = new System.Drawing.Size(612, 96);
            this.txtCollectionNotes.TabIndex = 16;
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.CollectionBinder, "Version", true));
            this.textBox2.Location = new System.Drawing.Point(585, 470);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(93, 20);
            this.textBox2.TabIndex = 14;
            // 
            // txtCollectionAuthor
            // 
            this.txtCollectionAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCollectionAuthor.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.CollectionBinder, "Author", true));
            this.txtCollectionAuthor.Location = new System.Drawing.Point(67, 466);
            this.txtCollectionAuthor.Name = "txtCollectionAuthor";
            this.txtCollectionAuthor.Size = new System.Drawing.Size(162, 20);
            this.txtCollectionAuthor.TabIndex = 14;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(7, 446);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(70, 16);
            this.label17.TabIndex = 13;
            this.label17.Text = "Author Info";
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(18, 266);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(199, 16);
            this.label14.TabIndex = 7;
            this.label14.Text = "Orphaned Rulesets In Collection";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(14, 62);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(127, 16);
            this.label15.TabIndex = 1;
            this.label15.Text = "Groups In Collection";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(13, 9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(452, 39);
            this.label16.TabIndex = 0;
            this.label16.Text = "Ruleset Collection Overview";
            // 
            // btnCollectionRulesetAdd
            // 
            this.btnCollectionRulesetAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollectionRulesetAdd.ImageIndex = 17;
            this.btnCollectionRulesetAdd.ImageList = this.imageList1;
            this.btnCollectionRulesetAdd.Location = new System.Drawing.Point(484, 423);
            this.btnCollectionRulesetAdd.Name = "btnCollectionRulesetAdd";
            this.btnCollectionRulesetAdd.Size = new System.Drawing.Size(79, 23);
            this.btnCollectionRulesetAdd.TabIndex = 11;
            this.btnCollectionRulesetAdd.Text = "Add";
            this.btnCollectionRulesetAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCollectionRulesetAdd.UseVisualStyleBackColor = true;
            this.btnCollectionRulesetAdd.Click += new System.EventHandler(this.btnCollectionRulesetsAdd_Click);
            // 
            // btnCollectionGroupAdd
            // 
            this.btnCollectionGroupAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollectionGroupAdd.ImageIndex = 17;
            this.btnCollectionGroupAdd.ImageList = this.imageList1;
            this.btnCollectionGroupAdd.Location = new System.Drawing.Point(484, 233);
            this.btnCollectionGroupAdd.Name = "btnCollectionGroupAdd";
            this.btnCollectionGroupAdd.Size = new System.Drawing.Size(79, 23);
            this.btnCollectionGroupAdd.TabIndex = 5;
            this.btnCollectionGroupAdd.Text = "Add";
            this.btnCollectionGroupAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCollectionGroupAdd.UseVisualStyleBackColor = true;
            this.btnCollectionGroupAdd.Click += new System.EventHandler(this.btnCollectionGroupAdd_Click);
            // 
            // dgvCollectionRulesets
            // 
            this.dgvCollectionRulesets.AllowUserToAddRows = false;
            this.dgvCollectionRulesets.AllowUserToDeleteRows = false;
            this.dgvCollectionRulesets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCollectionRulesets.AutoGenerateColumns = false;
            this.dgvCollectionRulesets.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCollectionRulesets.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvCollectionRulesets.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvCollectionRulesets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCollectionRulesets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtcCollectionRulesetName,
            this.dgvtcCollectionRulesetQuickView,
            this.dgvtcCollectionRulesetComment});
            this.dgvCollectionRulesets.DataSource = this.CollectionRulesetBinder;
            this.dgvCollectionRulesets.Location = new System.Drawing.Point(14, 288);
            this.dgvCollectionRulesets.MultiSelect = false;
            this.dgvCollectionRulesets.Name = "dgvCollectionRulesets";
            this.dgvCollectionRulesets.RowHeadersWidth = 25;
            this.dgvCollectionRulesets.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCollectionRulesets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCollectionRulesets.Size = new System.Drawing.Size(630, 129);
            this.dgvCollectionRulesets.TabIndex = 8;
            this.dgvCollectionRulesets.TabStop = false;
            this.dgvCollectionRulesets.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCollectionRulesets_CellEndEdit);
            this.dgvCollectionRulesets.Sorted += new System.EventHandler(this.DataGrid_Sorted);
            this.dgvCollectionRulesets.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvCollectionRulesets_MouseUp);
            // 
            // dgvtcCollectionRulesetName
            // 
            this.dgvtcCollectionRulesetName.DataPropertyName = "Name";
            this.dgvtcCollectionRulesetName.HeaderText = "Name";
            this.dgvtcCollectionRulesetName.Name = "dgvtcCollectionRulesetName";
            // 
            // dgvtcCollectionRulesetQuickView
            // 
            this.dgvtcCollectionRulesetQuickView.DataPropertyName = "QuickView";
            this.dgvtcCollectionRulesetQuickView.HeaderText = "QuickView";
            this.dgvtcCollectionRulesetQuickView.Name = "dgvtcCollectionRulesetQuickView";
            this.dgvtcCollectionRulesetQuickView.ReadOnly = true;
            // 
            // dgvtcCollectionRulesetComment
            // 
            this.dgvtcCollectionRulesetComment.DataPropertyName = "Comment";
            this.dgvtcCollectionRulesetComment.HeaderText = "Comment";
            this.dgvtcCollectionRulesetComment.Name = "dgvtcCollectionRulesetComment";
            // 
            // btnCollectionRulesetsRemove
            // 
            this.btnCollectionRulesetsRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollectionRulesetsRemove.Enabled = false;
            this.btnCollectionRulesetsRemove.ImageIndex = 18;
            this.btnCollectionRulesetsRemove.ImageList = this.imageList1;
            this.btnCollectionRulesetsRemove.Location = new System.Drawing.Point(565, 423);
            this.btnCollectionRulesetsRemove.Name = "btnCollectionRulesetsRemove";
            this.btnCollectionRulesetsRemove.Size = new System.Drawing.Size(79, 23);
            this.btnCollectionRulesetsRemove.TabIndex = 12;
            this.btnCollectionRulesetsRemove.Text = "Remove";
            this.btnCollectionRulesetsRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCollectionRulesetsRemove.UseVisualStyleBackColor = true;
            this.btnCollectionRulesetsRemove.Click += new System.EventHandler(this.btnCollectionRulesetsRemove_Click);
            // 
            // dgvCollectionGroups
            // 
            this.dgvCollectionGroups.AllowUserToAddRows = false;
            this.dgvCollectionGroups.AllowUserToDeleteRows = false;
            this.dgvCollectionGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCollectionGroups.AutoGenerateColumns = false;
            this.dgvCollectionGroups.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCollectionGroups.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvCollectionGroups.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvCollectionGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCollectionGroups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtcCollectionGroupName,
            this.dgvtcCollectionGroupComment,
            this.dgvtcCollectionGroupGroupCount,
            this.dgvtcCollectionGroupRulesetCount});
            this.dgvCollectionGroups.DataSource = this.CollectionGroupBinder;
            this.dgvCollectionGroups.Location = new System.Drawing.Point(11, 83);
            this.dgvCollectionGroups.MultiSelect = false;
            this.dgvCollectionGroups.Name = "dgvCollectionGroups";
            this.dgvCollectionGroups.RowHeadersWidth = 25;
            this.dgvCollectionGroups.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCollectionGroups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCollectionGroups.Size = new System.Drawing.Size(633, 144);
            this.dgvCollectionGroups.TabIndex = 2;
            this.dgvCollectionGroups.TabStop = false;
            this.dgvCollectionGroups.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCollectionGroups_CellEndEdit);
            this.dgvCollectionGroups.Sorted += new System.EventHandler(this.DataGrid_Sorted);
            this.dgvCollectionGroups.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvCollectionGroups_MouseUp);
            // 
            // dgvtcCollectionGroupName
            // 
            this.dgvtcCollectionGroupName.DataPropertyName = "Name";
            this.dgvtcCollectionGroupName.HeaderText = "Name";
            this.dgvtcCollectionGroupName.Name = "dgvtcCollectionGroupName";
            // 
            // dgvtcCollectionGroupComment
            // 
            this.dgvtcCollectionGroupComment.DataPropertyName = "Comment";
            this.dgvtcCollectionGroupComment.HeaderText = "Comment";
            this.dgvtcCollectionGroupComment.Name = "dgvtcCollectionGroupComment";
            // 
            // dgvtcCollectionGroupGroupCount
            // 
            this.dgvtcCollectionGroupGroupCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dgvtcCollectionGroupGroupCount.DataPropertyName = "GroupCount";
            this.dgvtcCollectionGroupGroupCount.HeaderText = "Groups";
            this.dgvtcCollectionGroupGroupCount.Name = "dgvtcCollectionGroupGroupCount";
            this.dgvtcCollectionGroupGroupCount.ReadOnly = true;
            this.dgvtcCollectionGroupGroupCount.Width = 66;
            // 
            // dgvtcCollectionGroupRulesetCount
            // 
            this.dgvtcCollectionGroupRulesetCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dgvtcCollectionGroupRulesetCount.DataPropertyName = "RulesetCount";
            this.dgvtcCollectionGroupRulesetCount.HeaderText = "Rulesets";
            this.dgvtcCollectionGroupRulesetCount.Name = "dgvtcCollectionGroupRulesetCount";
            this.dgvtcCollectionGroupRulesetCount.ReadOnly = true;
            this.dgvtcCollectionGroupRulesetCount.Width = 73;
            // 
            // btnCollectionGroupRemove
            // 
            this.btnCollectionGroupRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCollectionGroupRemove.Enabled = false;
            this.btnCollectionGroupRemove.ImageIndex = 18;
            this.btnCollectionGroupRemove.ImageList = this.imageList1;
            this.btnCollectionGroupRemove.Location = new System.Drawing.Point(565, 233);
            this.btnCollectionGroupRemove.Name = "btnCollectionGroupRemove";
            this.btnCollectionGroupRemove.Size = new System.Drawing.Size(79, 23);
            this.btnCollectionGroupRemove.TabIndex = 6;
            this.btnCollectionGroupRemove.Text = "Remove";
            this.btnCollectionGroupRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCollectionGroupRemove.UseVisualStyleBackColor = true;
            this.btnCollectionGroupRemove.Click += new System.EventHandler(this.btnCollectionGroupRemove_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabEdit);
            this.tabControl1.Controls.Add(this.tabSearch);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(922, 636);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tsmiAddGroup
            // 
            this.tsmiAddGroup.Image = global::DataManagerGUI.Properties.Resources.add_to_folder;
            this.tsmiAddGroup.Name = "tsmiAddGroup";
            this.tsmiAddGroup.Size = new System.Drawing.Size(144, 22);
            this.tsmiAddGroup.Text = "Add Group";
            // 
            // tsmiAddRuleset
            // 
            this.tsmiAddRuleset.Image = global::DataManagerGUI.Properties.Resources.add_page;
            this.tsmiAddRuleset.Name = "tsmiAddRuleset";
            this.tsmiAddRuleset.Size = new System.Drawing.Size(144, 22);
            this.tsmiAddRuleset.Text = "Add Ruleset";
            // 
            // gui
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(922, 661);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "gui";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CR Data Manager Configuration Editor";
            this.toolTip1.SetToolTip(this, " ");
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.gui_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.gui_FormClosed);
            this.Shown += new System.EventHandler(this.gui_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.RulesetBinder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RuleBinder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ActionBinder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupRulesetBinder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupBinder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupGroupBinder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupFiltersAndDefaultsBinders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupFiltersBinder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GroupDefaultsBinder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CollectionBinder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CollectionRulesetBinder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CollectionGroupBinder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserInfoBinder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SearchResultsTreeBinder)).EndInit();
            this.cmsMenu.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabSearch.ResumeLayout(false);
            this.toolStripContainer2.ContentPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer2.TopToolStripPanel.PerformLayout();
            this.toolStripContainer2.ResumeLayout(false);
            this.toolStripContainer2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.tsSearch.ResumeLayout(false);
            this.tsSearch.PerformLayout();
            this.tabEdit.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlRulesets.ResumeLayout(false);
            this.pnlRulesets.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRulesetRules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRulesetActions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlGroups.ResumeLayout(false);
            this.pnlGroups.PerformLayout();
            this.tabGroupTabs.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupRulesets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupGroups)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView5)).EndInit();
            this.tabGrpRuleset.ResumeLayout(false);
            this.tabGrpRuleset.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupFilters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroupDefaults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.pnlGeneral.ResumeLayout(false);
            this.pnlGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCollectionRulesets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCollectionGroups)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Form

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public gui()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Program.DebugAppend("Setting Paths...");
            this.startupPath = Application.StartupPath + Path.DirectorySeparatorChar;
            Program.DebugAppend("Data Manager Path is: " + startupPath);
            Program.DebugAppend("Data Manager Default Rule File is: " + ruleFile);
            Program.DebugAppend("User File path is: " + userFile);
            Program.DebugAppend("Verifying files...");
            Program.DebugAppend("Setting FieldType Limitations...");
            Global.InitializeGlobals(iniFile);
            Program.DebugAppend("Initializing Controls");
            InitializeComponent();
            //TODO: Once Filters and defaults are available for use remove next line
            //tabGroupTabs.TabPages.Remove(tabGrpRuleset);
            Program.DebugAppend("Loading user file");
            ReadUserFile();
            dmpActions.SetUserInfo((dmUserInfo)UserInfoBinder[0]);
            dmpRules.SetUserInfo((dmUserInfo)UserInfoBinder[0]);
            pmGroupFilters.SetUserInfo((dmUserInfo)UserInfoBinder[0]);
            pmGroupDefaultActions.SetUserInfo((dmUserInfo)UserInfoBinder[0]);
            Program.DebugAppend("Filling tree items...");
            comboBox1.DataSource = Enum.GetValues(typeof(RulesetModes));
            comboBox2.DataSource = Enum.GetValues(typeof(RulesetModes));
            VerifyFiles();
            tsSearch.Visible = true;
            tscbSearchType.SelectedIndex = 0;
            toolStrip1.Visible = true;
        }

        private NumberFormatInfo CreateNumFormat()
        {
            return CultureInfo.GetCultureInfo("en-US").NumberFormat;
        }

        private void VerifyFiles()
        {
            if (!Directory.Exists(RecovoryFolderPath)) Directory.CreateDirectory(RecovoryFolderPath);

            //try to find the default rule file
            if (!System.IO.File.Exists(ruleFile))
            {
                Program.DebugAppend("Default Ruleset Collection not found, searching for backup");
                if (File.Exists(RecovoryFolderPath + "dataman.dmr"))
                {
                    Program.DebugAppend("Backup Ruleset Collection Found, copying to default");
                    File.Copy(RecovoryFolderPath + "dataman.dmr", ruleFile, true);
                }
            }
            else
            {
                if (!File.Exists(RecovoryFolderPath + "dataman.dmr"))
                {
                    File.Copy(ruleFile, RecovoryFolderPath + "dataman.dmr", true);
                }
            }

            Program.DebugAppend("Loading default Ruleset Collection");

            this.currentRuleFile = ruleFile;
            FileChanged = false;
            tsslCurrentFile.Text = "Current File: " + currentRuleFile;
        }

        private void ReadUserFile()
        {
            if (!File.Exists(userFile))
            {
                if (File.Exists(RecovoryFolderPath + "user.ini"))
                    File.Copy(RecovoryFolderPath + "user.ini", userFile, true);
            }
            dmUserInfo tmp = new dmUserInfo(userFile);
            UserInfoBinder.Clear();
            UserInfoBinder.Add(tmp);
        }

        private void gui_Shown(object sender, EventArgs e)
        {
            Global.CurrentVersion = Global.CurrentVersion.Split(new char[] { 'r' })[0];
            Program.DebugAppend("Filling Combo boxes...");
            AddToolStripComboBoxItems();
            Program.DebugAppend("Setting default combobox items...");
            Program.DebugAppend("Loading default Ruleset Collection..");
            LoadFile(ruleFile);
            Program.DebugAppend("Setting versions...");
            this.Text = "Data Manager GUI v" + GUIVersion + " Data Manager v" + Global.CurrentVersion;
            PopulateTree();
            Program.DebugAppend("Showing GUI to user");
        }

        private void AddToolStripComboBoxItems()
        {
            tscbSearchField.Items.Clear();
            tscbSearchField.Items.Add("");
            tscbSearchField.Items.AddRange(Global.GetAllFields());
            if (tscbSearchField.Items.Count > 0) tscbSearchField.SelectedIndex = 0;
        }

        private void gui_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
            {
                //save recovery file;
                //SaveFile(RecovoryFolderPath + "dataman-recovery.dmr");
            }
            else
            {
                if (this.FileChanged)
                    using (Question tmpQ = new Question("Profile has changed since last save, would you like to save before exiting?"))
                    {
                        if (tmpQ.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                        {
                            tsmiSave_Click(sender, e);
                        }
                        e.Cancel = tmpQ.DialogResult == System.Windows.Forms.DialogResult.Cancel;
                    }
            }

        }

        private void gui_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //save user.ini file
                UserFileSave(userFile);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                this.tsmiSave_Click(tsmiSave, new EventArgs());
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.S))
            {
                this.tsmiSaveAs_Click(tsmiSaveAs, new EventArgs());
                return true;
            }
            else if (keyData == (Keys.Control | Keys.D))
            {
                this.tsmiSaveAsDefault_Click(tsmiSaveAsDefault, new EventArgs());
                return true;
            }
            else if (keyData == (Keys.Control | Keys.N))
            {
                this.tsmiNew_Click(tsmiNew, new EventArgs());
                return true;
            }
            else if (keyData == (Keys.Control | Keys.M))
            {
                this.tsmiMerge_Click(tsmiMerge, new EventArgs());
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.O))
            {
                this.tsmiLoadDefault_Click(tsmiLoadDefault, new EventArgs());
                return true;
            }
            else if (keyData == (Keys.Control | Keys.O))
            {
                this.tsmiLoad_Click(tsmiLoad, new EventArgs());
                return true;
            }
            else if (keyData == (Keys.Control | Keys.R))
            {
                this.tsmiRevert_Click(tsmiRevert, new EventArgs());
                return true;
            }
            else if (keyData == (Keys.Control | Keys.I))
            {
                tsmiProfileImport_Click(tsmiRevert, new EventArgs());
                return true;
            }

            return false;
        }

        #endregion

        #region Text Edit

        //string searchLabelText = "search ...";

        //private void textBoxSearch_DoubleClick(object sender, EventArgs e)
        //{
        //    FindString();
        //}

        //private void FindString()
        //{
        //    if (textBoxSearch.Text.Trim() == "" , textBoxSearch.Text == searchLabelText)
        //    {
        //        return;
        //    }

        //    string myText = textBox1.Text.ToLower();

        //    try
        //    {
        //        int pos = myText.IndexOf(textBoxSearch.Text.ToLower());
        //        textBox1.SelectionStart = pos;
        //        textBox1.SelectionLength = textBoxSearch.Text.Length;
        //        textBox1.Focus();
        //        textBox1.ScrollToCaret();
        //    }
        //    catch
        //    {
        //        MessageBox.Show(string.Format("End of rule set reached: \"{0}\" was not found.", textBoxSearch.Text),
        //            "Data Manager for ComicRack");
        //        SetLineInfo();
        //    }
        //}

        //private void SetLineInfo()
        //{

        //}

        //private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    WriteRuleFile();
        //    LoadGroups();
        //}

        //private void LoadGroups()
        //{
        //    throw new NotImplementedException();
        //}

        //private void WriteRuleFile()
        //{
        //    throw new NotImplementedException();
        //}

        //private void textBoxSearch_Enter(object sender, EventArgs e)
        //{
        //    textBoxSearch.ForeColor = System.Drawing.Color.Black;
        //    if (textBoxSearch.Text == searchLabelText)
        //    {
        //        textBoxSearch.Text = "";
        //    }
        //}

        //private void textBoxSearch_Leave(object sender, EventArgs e)
        //{
        //    if (textBoxSearch.Text == "")
        //    {
        //        textBoxSearch.ForeColor = System.Drawing.Color.Gray;
        //        textBoxSearch.Text = searchLabelText;
        //    }
        //}

        //private void buttonFind_Click(object sender, EventArgs e)
        //{
        //    FindString();
        //}

        //private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (tabControl1.SelectedIndex == 1)
        //    {
        //        textBox1.Text = CompileResults();
        //    }
        //}

        #endregion

        #region TreeView

        private void PopulateTree()
        {
            tvCollectionTree.Nodes.Clear();

            if (Collection == null)
            {
                Collection = new dmCollection();
            }

            //initial node
            TreeNode collectionNode = CreateCollectionNode();

            tvCollectionTree.Nodes.Add(collectionNode);

            CreateResidualsNode();

            for (int i = 0; i < Collection.GroupCount; i++)
            {
                //createNode
                TreeNode ndItem = CreateGroupNode(Collection.Groups[i]);
                //populate groups
                tvCollectionTree.Nodes[0].Nodes.Add(ndItem);
            }

            for (int i = 0; i < Collection.RulesetCount; i++)
            {
                TreeNode ndItem = CreateRulesetNode(Collection.Rulesets[i]);
                tvCollectionTree.Nodes[0].Nodes.Add(ndItem);
            }

            tvCollectionTree.SelectedNode = tvCollectionTree.Nodes[0];
            tvCollectionTree.SelectedNode.Expand();
        }

        private TreeNodeGroup CreateCollectionNode()
        {
            TreeNodeGroup tmp = new TreeNodeGroup("Ruleset Collection");
            tmp.Name = "ndCollection";

            tmp.ImageIndex = 0;
            tmp.SelectedImageIndex = 0;

            tmp.Tag = Collection;

            return tmp;
        }

        private void CreateResidualsNode()
        {
            TreeNodeGroup ndResidual = CreateGroupNode(Collection.Disabled);
            ndResidual.Text = "Disabled Rules";
            ndResidual.Name = "ndDisabled";
            ndResidual.SelectedImageIndex = 18;
            ndResidual.ImageIndex = 18;
            tvCollectionTree.Nodes.Add(ndResidual);
        }

        private void CreateVariablesNode()
        {
            TreeNode ndVariables = new TreeNode("Variables", 1, 1);
            ndVariables.Name = "ndVariables";
            tvCollectionTree.Nodes[0].Nodes.Insert(0, ndVariables);
        }

        private TreeNodeGroup CreateGroupNode(dmGroup item)
        {
            TreeNodeGroup ndItem = new TreeNodeGroup(item.Name);
            ndItem.Tag = item;
            ndItem.ImageIndex = 20;
            ndItem.SelectedImageIndex = 21;

            for (int i = 0; i < item.Groups.Count; i++)
            {
                //create and add group nodes for children groups
                TreeNodeGroup tmpNode = CreateGroupNode(item.Groups[i]);
                ndItem.Nodes.Add(tmpNode);
            }

            //create Ruleset Nodes for Group
            for (int i = 0; i < item.Rulesets.Count; i++)
            {
                //create and add group nodes for children groups
                TreeNode tmpNode = CreateRulesetNode(item.Rulesets[i]);
                ndItem.Nodes.Add(tmpNode);
            }


            return ndItem;
        }

        private TreeNodeGroup InsertGroupNode(int index, dmGroup item, TreeNode ParentNode)
        {
            TreeNodeGroup ndItem = new TreeNodeGroup(item.Name);
            ndItem.Tag = item;
            ndItem.ImageIndex = 2;
            ndItem.SelectedImageIndex = 3;

            for (int i = 0; i < item.GroupCount; i++)
            {
                ndItem.Nodes.Add(CreateGroupNode(item.Groups[i]));
            }

            foreach (dmRuleset rsItem in item.Rulesets)
            {
                ndItem.Nodes.Add(CreateRulesetNode(rsItem));
                // ndItem.ContextMenuStrip = cmsTreeMenuRulesetGroups;
            }


            ParentNode.Nodes.Insert(index, ndItem);

            return ndItem;
        }

        private TreeNode CreateRulesetNode(dmRuleset rsItem)
        {
            TreeNode ndItem = new TreeNode(rsItem.Name);
            ndItem.Tag = rsItem;
            ndItem.ImageIndex = 2;
            ndItem.SelectedImageIndex = 2;
            //ndItem.ContextMenuStrip = cmsTreeMenuRulests;
            return ndItem;
        }

        private void tvCollectionTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode tsNode = (sender as ContextMenuStrip)?.Tag as TreeNode;
            bool viaContextMenu = tsNode == null ? false : true;

            if (!viaContextMenu)
            {
                pnlGeneral.Visible = false;
                pnlGroups.Visible = false;
                pnlRulesets.Visible = false;
            }

            GroupBinder.Clear();
            RulesetBinder.Clear();

            switch (tvCollectionTree.SelectedNode.Name)
            {
                default:
                    object item = tsNode?.Tag ?? tvCollectionTree.SelectedNode.Tag;
                    if (item != null)
                    {
                        if (item.GetType() == typeof(dmRuleset))
                        {
                            RulesetBinder.Add(item);
                            if (!viaContextMenu) pnlRulesets.Visible = true;
                        }

                        else if (item.GetType() == typeof(dmGroup))
                        {
                            if (!viaContextMenu) pnlGroups.Visible = true;
                            GroupBinder.Add(item);
                        }
                        else if (item.GetType() == typeof(dmCollection))
                        {
                            if (!viaContextMenu) pnlGeneral.Visible = true;
                            CollectionBinder.ResetBindings(false);
                        }
                    }
                    break;
            }
        }


        private void tvCollectionTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (tvCollectionTree.SelectedNode.Tag != null)
            {
                string NewName = e.Label;

                if (tvCollectionTree.SelectedNode.Tag.GetType() == typeof(dmGroup))
                {
                    ((dmGroup)tvCollectionTree.SelectedNode.Tag).Name = NewName;
                    GroupBinder.ResetBindings(false);
                }
                if (tvCollectionTree.SelectedNode.Tag.GetType() == typeof(dmRuleset))
                {
                    ((dmRuleset)tvCollectionTree.SelectedNode.Tag).Name = NewName;
                    RulesetBinder.ResetBindings(false);
                }
            }

        }

        private void tvCollectionTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (((TreeNode)e.Item).Tag != null)
                if (((TreeNode)e.Item).Tag.GetType() != typeof(dmCollection))
                {
                    this.tvCollectionTree.DoDragDrop((TreeNode)e.Item, DragDropEffects.Move);
                    // End dragging image
                }
        }

        private void tvCollectionTree_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode NewNode = tvCollectionTree.GetTreeNode(e);
            if (NewNode != null)
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                Type DestinationNodeType = DestinationNode.Tag.GetType();
                Type NewNodeType = NewNode.Tag.GetType();

                if (DestinationNode != NewNode)
                {
                    if (NewNode.Tag != null)
                    {
                        if (NewNodeType != typeof(dmCollection))
                        {
                            NodePosition? position = tvCollectionTree.nodePosition;
                            if (position == null) return;

                            if (NewNodeType == typeof(dmRuleset))
                            {
                                if (position == NodePosition.In)
                                {
                                    dmContainer DestinationContainer = (dmContainer)DestinationNode.Tag;
                                    dmContainer SourceContainer = ((dmContainer)NewNode.Parent?.Tag);
                                    dmRuleset newRuleset = (dmRuleset)NewNode.Tag;
                                    if (DestinationContainer == null || SourceContainer == null) return;

                                    SourceContainer.RemoveRuleset(newRuleset);
                                    DestinationContainer.AddRuleset(newRuleset);
                                    //delete the treenode from the tree
                                    tvCollectionTree.Nodes.Remove(NewNode);
                                    //add the node to the tree
                                    DestinationNode.Nodes.Add(NewNode);
                                    FileChanged = true;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(tvCollectionTree.NodeMap))
                                    {
                                        dmContainer DestinationContainer = (dmContainer)DestinationNode.Parent?.Tag;
                                        dmContainer SourceContainer = ((dmContainer)NewNode.Parent?.Tag);
                                        dmRuleset newRuleset = (dmRuleset)NewNode.Tag;
                                        if (DestinationContainer == null || SourceContainer == null) return;

                                        string[] NodeIndexes = tvCollectionTree.NodeMap.Split('|');
                                        int newIndexFull = Int32.Parse(NodeIndexes[NodeIndexes.Length - 1]);
                                        int newIndexRuleset = newIndexFull - DestinationContainer.GroupCount;

                                        //add the Ruleset to the Group/Collection it's dropped on
                                        DestinationContainer.InsertRuleset(newIndexRuleset, newRuleset);
                                        //remove the ruleset from it's own Group/Collection
                                        var oldIndexFull = newIndexFull <= NewNode.Index ? NewNode.Index + 1 : NewNode.Index; 
                                        int oldIndexRuleset = oldIndexFull - SourceContainer.GroupCount;
                                        SourceContainer.RemoveRulesetAt(oldIndexRuleset);

                                        //add the node to the tree
                                        DestinationNode.Parent.Nodes.Insert(newIndexFull, (TreeNode)NewNode.Clone());
                                        //delete the treenode from the tree
                                        NewNode.Parent?.Nodes.RemoveAt(oldIndexFull);
                                        //this.tvCollectionTree.SelectedNode = InsertCollection[newIndex])];

                                        FileChanged = true;
                                    }
                                }
                            }
                            else if (NewNodeType == typeof(dmGroup))
                            {
                                dmGroup newGroup = (dmGroup)NewNode.Tag;

                                dmContainer SourceContainer = ((dmContainer)NewNode.Parent?.Tag);
                                if (SourceContainer == null) return;

                                if (position == NodePosition.In)
                                {
                                    dmContainer DestinationContainer = (dmContainer)DestinationNode.Tag;
                                    if (DestinationContainer == null) return;

                                    SourceContainer.RemoveGroup(newGroup);
                                    DestinationContainer.AddGroup(newGroup);
                                    //delete the treenode from the tree
                                    tvCollectionTree.Nodes.Remove(NewNode);
                                    //add the node to the tree
                                    DestinationNode.Nodes.Insert(DestinationContainer.IndexOfGroup(newGroup), NewNode);
                                    FileChanged = true;
                                }
                                else if (!string.IsNullOrEmpty(tvCollectionTree.NodeMap))
                                {
                                    dmContainer DestinationContainer = (dmContainer)DestinationNode.Parent?.Tag;
                                    if (DestinationContainer == null) return;

                                    string[] NodeIndexes = tvCollectionTree.NodeMap.Split('|');
                                    int newIndexFull = Int32.Parse(NodeIndexes[NodeIndexes.Length - 1]);

                                    //add the Ruleset to the Group/Collection it's dropped on
                                    DestinationContainer.InsertGroup(newIndexFull, newGroup);
                                    //remove the ruleset from it's own Group/Collection
                                    var oldIndex = newIndexFull <= NewNode.Index ? NewNode.Index + 1 : NewNode.Index;
                                    SourceContainer.RemoveGroupAt(oldIndex);

                                    //add the node to the tree
                                    DestinationNode.Parent.Nodes.Insert(newIndexFull, (TreeNode)NewNode.Clone());
                                    //delete the treenode from the tree
                                    NewNode.Parent?.Nodes.RemoveAt(oldIndex);
                                    FileChanged = true;
                                }
                            }
                        }
                    }
                }
            }
            CollectionBinder.ResetBindings(false);
        }

        private void tvCollectionTree_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void tvCollectionTree_DragLeave(object sender, EventArgs e)
        {

        }

        private void tvCollectionTree_DragOver(object sender, DragEventArgs e)
        {
            TreeNode NewNode = tvCollectionTree.GetTreeNode(e);
            if (NewNode != null)
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                if (DestinationNode != null && DestinationNode.Tag != null)
                {
                    Type DestinationNodeType = DestinationNode.Tag.GetType();
                    Type NewNodeType = NewNode.Tag.GetType();

                    if (DestinationNodeType.BaseType == typeof(dmContainer))
                    {
                        if (DestinationNode.FullPath.StartsWith(NewNode.FullPath))
                        {
                            e.Effect = DragDropEffects.None;
                        }
                        else
                            e.Effect = DragDropEffects.Move;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.Move;
                    }

                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }

        }

        private void tvCollectionTree_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {

        }

        private void tvCollectionTree_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void UpdateChildNodeName(TreeNode tmpNode, int nChildNodeIndex)
        {
            if (tmpNode.Tag == null) return;
            TreeNode UpdateNode = tmpNode.Nodes[nChildNodeIndex];
            if (UpdateNode.Tag == null) return;


            if (UpdateNode.Tag.GetType().BaseType == typeof(dmContainer))
            {
                if (tmpNode.Text != ((dmContainer)UpdateNode.Tag).Name)
                {
                    UpdateNode.Text = ((dmContainer)UpdateNode.Tag).Name;
                    FileChanged = true;
                }
            }
            else
            {
                if (UpdateNode.Text != ((dmRuleset)UpdateNode.Tag).Name)
                {
                    UpdateNode.Text = ((dmRuleset)UpdateNode.Tag).Name;
                    FileChanged = true;
                }
            }
        }

        private void tvCollectionTree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ItemType clipBoardItem = WhatsInTheClipboard();
                //select node under the cursor
                TreeNode tmpNode = tvCollectionTree.GetNodeAt(new Point(e.X, e.Y));
                //if node is not null find out what kind of item we are dealing with
                if (tmpNode != null)
                {
                    //tmpNode.BackColor = System.Drawing.Color.Yellow;
                    AlternativelySelectedNode = tmpNode;
                    cmsMenu.Show(tvCollectionTree, new Point(AlternativelySelectedNode.Bounds.Left, AlternativelySelectedNode.Bounds.Bottom), ToolStripDropDownDirection.BelowRight);
                }
            }
        }

        #endregion

        #region Binders Handling

        #region Collection Related
        void CollectionGroupBinder_PositionChanged(object sender, EventArgs e)
        {
            btnCollectionGroupRemove.Enabled = CollectionGroupBinder.Position > -1;
            btnCollectionGroupMoveUp.Enabled = CollectionGroupBinder.Position > 0;
            btnCollectionGroupMoveDown.Enabled = CollectionGroupBinder.Position < CollectionGroupBinder.Count - 1 && CollectionGroupBinder.Position > -1;
        }
        private void CollectionRulesetBinder_PositionChanged(object sender, EventArgs e)
        {
            btnCollectionRulesetsRemove.Enabled = CollectionRulesetBinder.Position > -1;
            btnCollectionRulesetMoveUp.Enabled = CollectionRulesetBinder.Position > 0;
            btnCollectionRulesetMoveDown.Enabled = CollectionRulesetBinder.Position < CollectionRulesetBinder.Count - 1 && CollectionRulesetBinder.Position > -1;
        }
        #endregion
        #region Group Related
        private void GroupGroupBinder_PositionChanged(object sender, EventArgs e)
        {
            btnGroupGroupRemove.Enabled = GroupGroupBinder.Position > -1;
            btnGroupGroupMoveUp.Enabled = GroupGroupBinder.Position > 0;
            btnGroupGroupMoveDown.Enabled = GroupGroupBinder.Position < GroupGroupBinder.Count - 1 && GroupGroupBinder.Position > -1;
        }
        private void GroupRulesetBinder_PositionChanged(object sender, EventArgs e)
        {
            btnGroupRulesetRemove.Enabled = GroupRulesetBinder.Position > -1;
            btnGroupRulesetMoveUp.Enabled = GroupRulesetBinder.Position > 0;
            btnGroupRulesetMoveDown.Enabled = GroupRulesetBinder.Position < GroupRulesetBinder.Count - 1 && GroupRulesetBinder.Position > -1;
        }
        private void GroupFiltersBinder_PositionChanged(object sender, EventArgs e)
        {
            //enable/disable add/remove/update/clear buttons
            btnGroupFilterClear.Enabled = GroupFiltersBinder.Count > 0;
            btnGroupFilterRemove.Enabled = btnGroupFilterUpdate.Enabled = GroupFiltersBinder.Position > -1;

            btnGroupFiltersMoveUp.Enabled = GroupFiltersBinder.Position > 0;
            btnGroupFiltersMoveDown.Enabled = GroupFiltersBinder.Position < GroupFiltersBinder.Count - 1 && GroupFiltersBinder.Count > 0;
        }
        private void GroupFiltersBinder_CurrentItemChanged(object sender, EventArgs e)
        {
            if (GroupFiltersBinder.Current == null)
                return;

            dmParameters tmp = (dmRule)GroupFiltersBinder.Current;
            pmGroupFilters.SetDataManagerParameter(tmp);
        }
        private void GroupDefaultsBinder_PositionChanged(object sender, EventArgs e)
        {
            //enable/disable add/remove/update/clear buttons
            btnGroupDefaultClear.Enabled = GroupDefaultsBinder.Count > 0;
            btnGroupDefaultRemove.Enabled = btnGroupDefaultUpdate.Enabled = GroupDefaultsBinder.Position > -1;

            btnGroupDefaultMoveUp.Enabled = GroupDefaultsBinder.Position > 0;
            btnGroupDefaultMoveDown.Enabled = GroupDefaultsBinder.Position < GroupDefaultsBinder.Count - 1 && GroupDefaultsBinder.Count > 0;
        }
        private void GroupDefaultsBinder_CurrentItemChanged(object sender, EventArgs e)
        {
            if (GroupDefaultsBinder.Current == null)
                return;

            dmParameters tmp = (dmAction)GroupDefaultsBinder.Current;
            pmGroupDefaultActions.SetDataManagerParameter(tmp);
        }
        #endregion
        #region Rule Related
        private void RuleBinder_PositionChanged(object sender, EventArgs e)
        {
            btnRulesetRuleUpdate.Enabled = RuleBinder.Count > 0;
            btnRulesetRuleClear.Enabled = RuleBinder.Count > 0;
            btnRulesetRuleRemove.Enabled = btnRulesetRuleUpdate.Enabled = RuleBinder.Position > -1;
            btnRulesetRulesMoveUp.Enabled = RuleBinder.Position > 0;
            btnRulesetRulesMoveDown.Enabled = RuleBinder.Position < RuleBinder.Count - 1 && RuleBinder.Position > -1;
        }
        private void RuleBinder_CurrentItemChanged(object sender, EventArgs e)
        {
            if (RuleBinder.Current == null) return;

            dmParameters tmp = (dmParameters)RuleBinder.Current;
            SetRule(tmp);
        }
        private void SetRule(dmParameters rule)
        {
            dmpRules.SetDataManagerParameter(rule);
        }
        #endregion
        #region Action Related
        private void ActionBinder_PositionChanged(object sender, EventArgs e)
        {
            btnRulesetActionUpdate.Enabled = ActionBinder.Count > 0;
            btnRulesetActionClear.Enabled = ActionBinder.Count > 0;
            btnRulesetActionRemove.Enabled = btnRulesetActionUpdate.Enabled = ActionBinder.Position > -1;
            btnActionMoveUp.Enabled = ActionBinder.Position > 0;
            btnActionMoveDown.Enabled = ActionBinder.Position < ActionBinder.Count - 1 && ActionBinder.Position > -1;
        }

        private void ActionBinder_CurrentItemChanged(object sender, EventArgs e)
        {
            if (ActionBinder.Current == null) return;

            //load the action into the action editor
            dmParameters tmp = (dmParameters)ActionBinder.Current;

            SetAction(tmp);
        }

        private void SetAction(dmParameters action)
        {
            dmpActions.SetDataManagerParameter(action);
        }
        #endregion

        #endregion

        #region Collection Panel

        private void btnCollectionGroupAdd_Click(object sender, EventArgs e)
        {
            //get the treenode cause we'll be adding a node to it
            TreeNode tsNode = (sender as ToolStripMenuItem)?.Tag as TreeNode;
            TreeNode tmpNode = tsNode ?? tvCollectionTree.Nodes[0];

            //create new group
            dmGroup tmpGroup = new dmGroup((dmContainer)tmpNode.Tag);


            CollectionGroupBinder.Add(tmpGroup);
            CollectionGroupBinder.Position = CollectionGroupBinder.IndexOf(tmpGroup);
            TreeNode newNode = CreateGroupNode(tmpGroup);
            //Insert in tree
            tmpNode.Nodes.Insert(Collection.IndexOfGroup(tmpGroup), newNode);
            tmpNode.Expand();
            dgvCollectionGroups.Focus();
            FileChanged = true;
        }

        private void btnCollectionGroupRemove_Click(object sender, EventArgs e)
        {
            if (CollectionGroupBinder.Position < 0) return;
            int index = CollectionGroupBinder.Position;
            CollectionGroupBinder.RemoveCurrent();
            tvCollectionTree.SelectedNode.Nodes.RemoveAt(index);
            FileChanged = true;
        }

        private void btnCollectionRulesetsAdd_Click(object sender, EventArgs e)
        {
            TreeNode tsNode = (sender as ToolStripMenuItem)?.Tag as TreeNode;
            TreeNode tmpNode = tsNode ?? tvCollectionTree.Nodes[0];
            dmContainer dmnItem = (dmContainer)tmpNode.Tag;

            dmRuleset tmpRS = new dmRuleset(dmnItem);

            //put it in the collection.
            CollectionRulesetBinder.Add(tmpRS);
            CollectionRulesetBinder.Position = CollectionRulesetBinder.Count - 1;
            TreeNode nd = CreateRulesetNode(tmpRS);
            //put it in the tree
            tmpNode.Nodes.Add(nd);
            tmpNode.Expand();
            dgvCollectionRulesets.Focus();
            FileChanged = true;
        }

        private void btnCollectionRulesetsRemove_Click(object sender, EventArgs e)
        {
            if (CollectionRulesetBinder.Position < 0) return;

            //remove from tree
            int fix = Collection.Groups.Count;

            //use position of current to find index of ruleset
            tvCollectionTree.Nodes[0].Nodes.RemoveAt(fix + CollectionRulesetBinder.Position);

            CollectionRulesetBinder.RemoveCurrent();
            FileChanged = true;
        }

        private void btnCollectionGroupMoveUp_Click(object sender, EventArgs e)
        {
            //Get Current index
            int index = CollectionGroupBinder.Position;

            if (index - 1 < 0) return;
            //Pop it out of the list
            dmGroup tmp = (dmGroup)CollectionGroupBinder.Current;
            CollectionGroupBinder.RemoveCurrent();
            //move it to new index
            CollectionGroupBinder.Insert(index - 1, tmp);
            //select it
            CollectionGroupBinder.Position = index - 1;
            //pop out the treenode
            tvCollectionTree.SelectedNode.Nodes.RemoveAt(index);
            //move it to the new index
            TreeNode tmpNode = CreateGroupNode(tmp);
            tvCollectionTree.SelectedNode.Nodes.Insert(index - 1, tmpNode);
            FileChanged = true;
        }

        private void btnCollectionGroupMoveDown_Click(object sender, EventArgs e)
        {
            //Get Current index
            int index = CollectionGroupBinder.Position;

            if (index + 1 >= CollectionGroupBinder.Count) return;
            //Pop it out of the list
            dmGroup tmp = (dmGroup)CollectionGroupBinder.Current;
            CollectionGroupBinder.RemoveCurrent();
            //move it to new index
            CollectionGroupBinder.Insert(index + 1, tmp);
            //select it
            CollectionGroupBinder.Position = index + 1;
            //pop out the treenode
            tvCollectionTree.SelectedNode.Nodes.RemoveAt(index);
            //move it to the new index
            TreeNode tmpNode = CreateGroupNode(tmp);
            tvCollectionTree.SelectedNode.Nodes.Insert(index + 1, tmpNode);
            FileChanged = true;
        }

        private void btnCollectionRulesetMoveUp_Click(object sender, EventArgs e)
        {
            //Get Current index
            int index = CollectionRulesetBinder.Position;

            if (index - 1 < 0) return;
            //Pop it out of the list
            dmRuleset tmp = (dmRuleset)CollectionRulesetBinder.Current;
            CollectionRulesetBinder.RemoveCurrent();
            //move it to new index
            CollectionRulesetBinder.Insert(index - 1, tmp);
            //select it
            CollectionRulesetBinder.Position = index - 1;
            //pop out the treenode
            tvCollectionTree.SelectedNode.Nodes.RemoveAt(index + CollectionGroupBinder.Count);
            //move it to the new index
            TreeNode tmpNode = CreateRulesetNode(tmp);
            tvCollectionTree.SelectedNode.Nodes.Insert(index - 1 + CollectionGroupBinder.Count, tmpNode);
            FileChanged = true;
        }

        private void btnCollectionRulesetMoveDown_Click(object sender, EventArgs e)
        {
            //Get Current index
            int index = CollectionRulesetBinder.Position;

            if (index + 1 >= CollectionRulesetBinder.Count) return;
            //Pop it out of the list
            dmRuleset tmp = (dmRuleset)CollectionRulesetBinder.Current;
            CollectionRulesetBinder.RemoveCurrent();
            //move it to new index
            CollectionRulesetBinder.Insert(index + 1, tmp);
            //select it
            CollectionRulesetBinder.Position = index + 1;
            //pop out the treenode
            tvCollectionTree.SelectedNode.Nodes.RemoveAt(index + CollectionGroupBinder.Count);
            //move it to the new index
            TreeNode tmpNode = CreateRulesetNode(tmp);
            tvCollectionTree.SelectedNode.Nodes.Insert(index + 1 + CollectionGroupBinder.Count, tmpNode);
            FileChanged = true;
        }

        private void dgvCollectionGroups_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (tvCollectionTree.SelectedNode == null || e.ColumnIndex != 0) return;

            TreeNode tmpNode = tvCollectionTree.SelectedNode;
            int index = e.RowIndex;

            UpdateChildNodeName(tmpNode, index);
        }

        private void dgvCollectionRulesets_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (tvCollectionTree.SelectedNode == null || e.ColumnIndex != 0) return;

            TreeNode tmpNode = tvCollectionTree.SelectedNode;
            int index = ((dmContainer)tvCollectionTree.SelectedNode.Tag).GroupCount + e.RowIndex;

            UpdateChildNodeName(tmpNode, index);
        }

        private void dgvCollectionGroups_MouseUp(object sender, MouseEventArgs e)
        {
            Point tmpPoint = dgvCollectionGroups.PointToClient(Cursor.Position);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataGridView.HitTestInfo HitTest = dgvCollectionGroups.HitTest(tmpPoint.X, tmpPoint.Y);

                if (HitTest.RowIndex > -1)
                {
                    CollectionGroupBinder.Position = HitTest.RowIndex;
                }
                cmsMenu.Show(dgvCollectionGroups, tmpPoint, ToolStripDropDownDirection.Default);
            }
        }

        private void dgvCollectionRulesets_MouseUp(object sender, MouseEventArgs e)
        {
            Point tmpPoint = dgvCollectionRulesets.PointToClient(Cursor.Position);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataGridView.HitTestInfo HitTest = dgvCollectionRulesets.HitTest(tmpPoint.X, tmpPoint.Y);

                if (HitTest.RowIndex > -1)
                {
                    CollectionRulesetBinder.Position = HitTest.RowIndex;
                }
                cmsMenu.Show(dgvCollectionRulesets, tmpPoint, ToolStripDropDownDirection.Default);
            }
        }

        #endregion

        #region Group Panel

        private void btnGroupGroupRemove_Click(object sender, EventArgs e)
        {
            if (GroupGroupBinder.Position < 0) return;

            tvCollectionTree.SelectedNode.Nodes.RemoveAt(GroupGroupBinder.Position);

            GroupGroupBinder.RemoveCurrent();
            FileChanged = true;
        }

        private void btnGroupGroupAdd_Click(object sender, EventArgs e)
        {
            //get tree node
            TreeNode tsNode = (sender as ToolStripMenuItem)?.Tag as TreeNode;
            TreeNode myNode = tsNode ?? tvCollectionTree.SelectedNode;
            //get group
            dmContainer dmnTemp = (dmContainer)myNode.Tag;

            //create a group
            dmGroup newGroup = new dmGroup(dmnTemp);

            //add to Binder
            GroupGroupBinder.Add(newGroup);
            GroupGroupBinder.Position = GroupGroupBinder.IndexOf(newGroup);

            //create a node for the group
            TreeNode newNode = CreateGroupNode(newGroup);

            //add the new node
            myNode.Nodes.Insert(dmnTemp.IndexOfGroup(newGroup), newNode);

            FileChanged = true;

            //expand so you can see new node
            myNode.Expand();
            dgvGroupGroups.Focus();

        }

        private void btnGroupRulesetRemove_Click(object sender, EventArgs e)
        {
            if (GroupRulesetBinder.Position < 0) return;

            //remove from tree
            int fix = GroupGroupBinder.Count;

            //use position of current to find index of ruleset
            tvCollectionTree.SelectedNode.Nodes.RemoveAt(fix + GroupRulesetBinder.Position);

            GroupRulesetBinder.RemoveCurrent();
            FileChanged = true;
        }

        private void btnGroupRulesetAdd_Click(object sender, EventArgs e)
        {
            TreeNode tsNode = (sender as ToolStripMenuItem)?.Tag as TreeNode;
            TreeNode tmpNode = tsNode ?? tvCollectionTree.SelectedNode;
            dmContainer dmnItem = (dmContainer)tmpNode.Tag;
         
            dmRuleset tmpRS = new dmRuleset(dmnItem);

            //put it in the group.
            GroupRulesetBinder.Add(tmpRS);
            GroupRulesetBinder.Position = GroupRulesetBinder.Count - 1;

            TreeNode nd = CreateRulesetNode(tmpRS);
            //put it in the tree
            tmpNode.Nodes.Add(nd);
            tmpNode.Expand();
            dgvGroupRulesets.Focus();
            FileChanged = true;
        }

        private void btnGroupGroupMoveUp_Click(object sender, EventArgs e)
        {
            //Get Current index
            int index = GroupGroupBinder.Position;

            if (index - 1 < 0) return;
            //Pop it out of the list
            dmGroup tmp = (dmGroup)GroupGroupBinder.Current;
            GroupGroupBinder.RemoveCurrent();
            //move it to new index
            GroupGroupBinder.Insert(index - 1, tmp);
            //select it
            GroupGroupBinder.Position = index - 1;
            //pop out the treenode
            tvCollectionTree.SelectedNode.Nodes.RemoveAt(index);
            //move it to the new index
            TreeNode tmpNode = CreateGroupNode(tmp);
            tvCollectionTree.SelectedNode.Nodes.Insert(index - 1, tmpNode);

            FileChanged = true;
        }

        private void btnGroupGroupMoveDown_Click(object sender, EventArgs e)
        {
            //Get Current index
            int index = GroupGroupBinder.Position;

            if (index + 1 >= GroupGroupBinder.Count) return;
            //Pop it out of the list
            dmGroup tmp = (dmGroup)GroupGroupBinder.Current;
            GroupGroupBinder.RemoveCurrent();
            //move it to new index
            GroupGroupBinder.Insert(index + 1, tmp);
            //select it
            GroupGroupBinder.Position = index + 1;
            //pop out the treenode
            tvCollectionTree.SelectedNode.Nodes.RemoveAt(index);
            //move it to the new index
            TreeNode tmpNode = CreateGroupNode(tmp);
            tvCollectionTree.SelectedNode.Nodes.Insert(index + 1, tmpNode);
            FileChanged = true;
        }

        private void btnGroupRulesetMoveUp_Click(object sender, EventArgs e)
        {
            //Get Current index
            int index = GroupRulesetBinder.Position;

            if (index - 1 < 0) return;
            //Pop it out of the list
            dmRuleset tmp = (dmRuleset)GroupRulesetBinder.Current;
            GroupRulesetBinder.RemoveCurrent();
            //move it to new index
            GroupRulesetBinder.Insert(index - 1, tmp);
            //select it
            GroupRulesetBinder.Position = index - 1;
            //pop out the treenode
            tvCollectionTree.SelectedNode.Nodes.RemoveAt(index + GroupGroupBinder.Count);
            //move it to the new index
            TreeNode tmpNode = CreateRulesetNode(tmp);
            tvCollectionTree.SelectedNode.Nodes.Insert(index - 1 + GroupGroupBinder.Count, tmpNode);
            FileChanged = true;
        }

        private void btnGroupRulesetMoveDown_Click(object sender, EventArgs e)
        {
            //Get Current index
            int index = GroupRulesetBinder.Position;

            if (index + 1 >= GroupRulesetBinder.Count) return;
            //Pop it out of the list
            dmRuleset tmp = (dmRuleset)GroupRulesetBinder.Current;
            GroupRulesetBinder.RemoveCurrent();
            //move it to new index
            GroupRulesetBinder.Insert(index + 1, tmp);
            //select it
            GroupRulesetBinder.Position = index + 1;
            //pop out the treenode
            tvCollectionTree.SelectedNode.Nodes.RemoveAt(index + GroupGroupBinder.Count);
            //move it to the new index
            TreeNode tmpNode = CreateRulesetNode(tmp);
            tvCollectionTree.SelectedNode.Nodes.Insert(index + 1 + GroupGroupBinder.Count, tmpNode);
            FileChanged = true;
        }

        private void txtGroupName_Validated(object sender, EventArgs e)
        {
            if (tvCollectionTree.SelectedNode.Tag.GetType() != typeof(dmGroup)) return;
            //group
            dmGroup tmp = (dmGroup)tvCollectionTree.SelectedNode.Tag;
            //SelectedNode
            if (tvCollectionTree.SelectedNode.Text != tmp.Name)
            {
                tvCollectionTree.SelectedNode.Text = tmp.Name;
                FileChanged = true;
            }
        }

        private void dgvGroupGroups_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (tvCollectionTree.SelectedNode == null || e.ColumnIndex != 0) return;

            TreeNode tmpNode = tvCollectionTree.SelectedNode;
            int index = e.RowIndex;

            UpdateChildNodeName(tmpNode, index);
        }

        private void dgvGroupRulesets_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (tvCollectionTree.SelectedNode == null || e.ColumnIndex != 0) return;

            TreeNode tmpNode = tvCollectionTree.SelectedNode;
            int index = ((dmContainer)tvCollectionTree.SelectedNode.Tag).GroupCount + e.RowIndex;

            UpdateChildNodeName(tmpNode, index);
        }

        private void dgvGroupGroups_MouseUp(object sender, MouseEventArgs e)
        {
            Point tmpPoint = dgvGroupGroups.PointToClient(Cursor.Position);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataGridView.HitTestInfo HitTest = dgvGroupGroups.HitTest(tmpPoint.X, tmpPoint.Y);

                if (HitTest.RowIndex > -1)
                {
                    GroupGroupBinder.Position = HitTest.RowIndex;
                }
                cmsMenu.Show(dgvGroupGroups, tmpPoint, ToolStripDropDownDirection.Default);
            }
        }

        private void dgvGroupRulesets_MouseUp(object sender, MouseEventArgs e)
        {
            Point tmpPoint = dgvGroupRulesets.PointToClient(Cursor.Position);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataGridView.HitTestInfo HitTest = dgvGroupRulesets.HitTest(tmpPoint.X, tmpPoint.Y);

                if (HitTest.RowIndex > -1)
                {
                    GroupRulesetBinder.Position = HitTest.RowIndex;
                }
                cmsMenu.Show(dgvGroupRulesets, tmpPoint, ToolStripDropDownDirection.Default);
            }
        }

        #endregion

        #region Ruleset Panel

        private void txtRulesetName_Validated(object sender, EventArgs e)
        {
            if (tvCollectionTree.SelectedNode.Tag.GetType() != typeof(dmRuleset)) return;
            if (tvCollectionTree.SelectedNode.Text != txtRulesetName.Text)
            {
                tvCollectionTree.SelectedNode.Text = txtRulesetName.Text;
                FileChanged = true;
            }
            RulesetBinder.ResetCurrentItem();
        }
        private void MessageBoxErrorShow(string strMessage)
        {
            MessageBox.Show(strMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void MessageBoxErrorShow(string strMessage, string strCaption)
        {
            MessageBox.Show(strMessage, strCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void btnRulesetReparse_Click(object sender, EventArgs e)
        {
            if (RulesetBinder.Position < 0) return;
            ((dmRuleset)RulesetBinder.Current).Reparse(txtRulesetReparse.Text);
            FileChanged = true;
            RulesetBinder.ResetCurrentItem();
        }
        private void txtRulesetName_TextChanged(object sender, EventArgs e)
        {
            txtRulesetComment.Enabled = !string.IsNullOrEmpty(txtRulesetName.Text);
        }

        #region Rules

        private void btnRulesetActionUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                dmParameters tmp = GetActionResult();
                if (!ActionBinder.Contains(tmp))
                {
                    ((dmParameters)ActionBinder.Current).Copy(tmp);
                    ActionBinder.ResetCurrentItem();
                    RulesetBinder.ResetCurrentItem();
                    FileChanged = true;
                }
                else if (ActionBinder.IndexOf(tmp) != ActionBinder.Position)
                    MessageBoxErrorShow("Updating this action in this manner will produce a duplicate action.", "Duplicate Found!");
            }
            catch (Exception ex)
            {
                MessageBoxErrorShow(ex.Message);
            }
        }
        private void btnRulesetRuleAdd_Click(object sender, EventArgs e)
        {
            try
            {
                dmParameters tmp = GetRuleResults();

                if (!RuleBinder.List.Contains(tmp))
                {
                    RuleBinder.Add(tmp);
                    RuleBinder.Position = RuleBinder.IndexOf(tmp);
                    RulesetBinder.ResetCurrentItem();
                    FileChanged = true;
                }
                else
                {
                    MessageBoxErrorShow("This exact rule already exists, duplicating it will give no benefits.", "Duplicate Rule");
                }
            }
            catch (Exception ex)
            {
                MessageBoxErrorShow(ex.Message, "Error");
            }
        }
        private void btnRulesetRuleRemove_Click(object sender, EventArgs e)
        {
            if (RuleBinder.Position < 0) return;

            RuleBinder.RemoveCurrent();
            RuleBinder_PositionChanged(sender, e);
            RulesetBinder.ResetCurrentItem();
            FileChanged = true;
        }
        private void btnRulesetRuleClear_Click(object sender, EventArgs e)
        {
            RuleBinder.Clear();
            RulesetBinder.ResetCurrentItem();
            FileChanged = true;
        }
        private void btnRulesetRuleUpdate_Click(object sender, EventArgs e)
        {
            if (RuleBinder.Position < 0) return;
            try
            {
                dmParameters tmp = GetRuleResults();
                if (!RuleBinder.Contains(tmp))
                {
                    ((dmParameters)RuleBinder.Current).Copy(tmp);
                    RuleBinder.ResetCurrentItem();
                    RulesetBinder.ResetCurrentItem();
                    FileChanged = true;
                }
                else if (RuleBinder.IndexOf(tmp) != RuleBinder.Position)
                    MessageBoxErrorShow("Updating this Rule in this manner will produce a duplicate", "Duplicate Rule");
            }
            catch (Exception ex)
            {
                MessageBoxErrorShow(ex.Message);
            }
        }
        private void dgvRulesetRules_MouseUp(object sender, MouseEventArgs e)
        {
            Point tmpPoint = dgvRulesetRules.PointToClient(Cursor.Position);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataGridView.HitTestInfo HitTest = dgvRulesetRules.HitTest(tmpPoint.X, tmpPoint.Y);

                if (HitTest.RowIndex > -1)
                {
                    RuleBinder.Position = HitTest.RowIndex;
                }
                cmsMenu.Show(dgvRulesetRules, tmpPoint, ToolStripDropDownDirection.Default);
            }
        }
        private void btnRulesetRulesMoveUp_Click(object sender, EventArgs e)
        {
            //pop item out
            dmRule item = (dmRule)RuleBinder.Current;
            //get new index from current index
            int index = RuleBinder.Position - 1;
            //remove item
            RuleBinder.RemoveCurrent();
            //add back at new index
            RuleBinder.Insert(index, item);
            //select item again
            RuleBinder.Position = index;
        }
        private void btnRulesetRulesMoveDown_Click(object sender, EventArgs e)
        {
            //pop item out
            dmRule item = (dmRule)RuleBinder.Current;
            //get new index from current index
            int index = RuleBinder.Position + 1;
            //remove item
            RuleBinder.RemoveCurrent();
            //add back at new index
            RuleBinder.Insert(index, item);
            //select item again
            RuleBinder.Position = index;
        }

        #endregion
        #region Actions

        private dmParameters GetRuleResults()
        {
            return dmpRules.GetDataManagerParameters();
        }

        private void btnRulesetActionClear_Click(object sender, EventArgs e)
        {
            ActionBinder.Clear();
            RulesetBinder.ResetCurrentItem();
            FileChanged = true;
        }

        private void btnRulesetActionRemove_Click(object sender, EventArgs e)
        {
            if (ActionBinder.Position < 0) return;
            ActionBinder.RemoveCurrent();
            RulesetBinder.ResetCurrentItem();
            FileChanged = true;
        }

        private void btnRulesetActionAdd_Click(object sender, EventArgs e)
        {
            try
            {
                dmParameters tmp = GetActionResult();
                if (!ActionBinder.List.Contains(tmp))
                {
                    ActionBinder.Add(tmp);
                    ActionBinder.Position = ActionBinder.IndexOf(tmp);
                    RulesetBinder.ResetCurrentItem();
                    FileChanged = true;
                }
                else
                    MessageBoxErrorShow("This exact action already exists, duplicating it will give no benefits.", "Action Exists");
            }
            catch (Exception ex)
            {
                MessageBoxErrorShow(ex.Message);
            }

        }

        private void dgvRulesetActions_MouseUp(object sender, MouseEventArgs e)
        {
            Point tmpPoint = dgvRulesetActions.PointToClient(Cursor.Position);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataGridView.HitTestInfo HitTest = dgvRulesetActions.HitTest(tmpPoint.X, tmpPoint.Y);

                if (HitTest.RowIndex > -1)
                {
                    ActionBinder.Position = HitTest.RowIndex;
                }
                cmsMenu.Show(dgvRulesetActions, tmpPoint, ToolStripDropDownDirection.Default);
            }
        }

        private dmParameters GetActionResult()
        {
            return dmpActions.GetDataManagerParameters();
        }

        private void btnActionMoveDown_Click(object sender, EventArgs e)
        {
            //Get Current index
            int index = ActionBinder.Position;

            if (index + 1 >= ActionBinder.Count) return;
            //Pop it out of the list
            dmParameters tmp = (dmParameters)ActionBinder.Current;
            ActionBinder.RemoveCurrent();
            //move it to new index
            ActionBinder.Insert(index + 1, tmp);
            //select it
            ActionBinder.Position = index + 1;
            FileChanged = true;
        }

        private void btnActionMoveUp_Click(object sender, EventArgs e)
        {
            //Get Current index
            int index = ActionBinder.Position;

            if (index - 1 < 0) return;
            //Pop it out of the list
            dmParameters tmp = (dmParameters)ActionBinder.Current;
            ActionBinder.RemoveCurrent();
            //move it to new index
            ActionBinder.Insert(index - 1, tmp);
            //select it
            ActionBinder.Position = index - 1;

            FileChanged = true;
        }

        #endregion


        /// <summary>
        /// Disables the comment textbox if no name is set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>



        #endregion

        #region Ressidual & Variables Panels
        private void txtCollectionresidual_Validated(object sender, EventArgs e)
        {

        }

        #endregion

        #region Tools

        #region File

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox tmp = new AboutBox();
            tmp.ShowDialog();
        }

        private void tsmiLoad_Click(object sender, EventArgs e)
        {
            using (Question tmpQuest = new Question())
            {
                if (FileChanged)
                {
                    if (tmpQuest.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                    {
                        if (tmpQuest.DialogResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            SaveFile(currentRuleFile);
                            this.tsslLastSave.Text = currentTime;
                            FileChanged = false;
                        }
                    }
                }
                using (OpenFileDialog ofdOpen = new OpenFileDialog())
                {
                    ofdOpen.Title = "Select a file to open";
                    ofdOpen.AutoUpgradeEnabled = true;
                    ofdOpen.InitialDirectory = RecovoryFolderPath;
                    ofdOpen.Filter = FileFilters;
                    ofdOpen.DefaultExt = ".dat";
                    ofdOpen.FilterIndex = 0;

                    if (ofdOpen.ShowDialog() == DialogResult.OK)
                    {
                        LoadFile(ofdOpen.FileName);
                    }
                }
            }
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            ValidateChildren();
            if (!string.IsNullOrEmpty(currentRuleFile))
            {
                SaveFile(currentRuleFile);
                tsslLastSave.Text = currentTime;
                FileChanged = false;
            }
            else
            {
                tsmiSaveAs_Click(sender, e);
            }
        }

        private void tsmiSaveAs_Click(object sender, EventArgs e)
        {
            ValidateChildren();
            using (SaveFileDialog tmp = new SaveFileDialog())
            {
                tmp.AutoUpgradeEnabled = true;
                tmp.OverwritePrompt = true;
                tmp.Filter = FileFilters;
                tmp.FilterIndex = 0;
                tmp.AddExtension = true;
                tmp.DefaultExt = ".dmr";
                tmp.InitialDirectory = this.startupPath;

                if (tmp.ShowDialog() == DialogResult.OK)
                {
                    SaveFile(tmp.FileName);
                    currentRuleFile = tmp.FileName;
                    FileChanged = false;
                }
            }
        }

        private void tsmiSaveAsDefault_Click(object sender, EventArgs e)
        {
            SaveFile(ruleFile);
        }

        private void tsmiMerge_Click(object sender, EventArgs e)
        {
            if (FileChanged)
            {
                using (Question tmpQues = new Question())
                {
                    if (tmpQues.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                    {
                        if (tmpQues.DialogResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (!string.IsNullOrEmpty(currentRuleFile))
                            {
                                SaveFile(currentRuleFile);
                                FileChanged = false;
                            }
                            else
                            {
                                tsmiSaveAs_Click(sender, e);
                            }
                        }
                    }
                    else return;
                }
            }

            using (OpenFileDialog ofdOpen = new OpenFileDialog())
            {
                ofdOpen.Title = "Select a file to merge with current";
                ofdOpen.AutoUpgradeEnabled = true;
                ofdOpen.InitialDirectory = RecovoryFolderPath;
                ofdOpen.RestoreDirectory = true;
                ofdOpen.Filter = FileFilters;
                ofdOpen.DefaultExt = ".dat";
                ofdOpen.FilterIndex = 0;

                if (ofdOpen.ShowDialog() == DialogResult.OK)
                {
                    tvCollectionTree.Nodes.Clear();
                    CollectionBinder.Clear();
                    Collection.Merge(File.ReadAllLines(ofdOpen.FileName));
                    CollectionBinder.Add(Collection);
                    PopulateTree();
                    currentRuleFile = "";
                    this.FileChanged = true;
                }
            }
        }

        private void tsmiRevert_Click(object sender, EventArgs e)
        {
            using (Question quest = new Question("All changes since last save will be lost\r\nAre you sure you want to do this?"))
            {
                if (!string.IsNullOrEmpty(currentRuleFile))
                {
                    LoadFile(currentRuleFile);
                }
                else
                {
                    tsmiLoad_Click(sender, e);
                }
            }
        }

        private void tsmiLoadDefault_Click(object sender, EventArgs e)
        {
            using (Question tmpQuest = new Question())
            {
                if (FileChanged)
                {
                    if (tmpQuest.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                    {
                        if (tmpQuest.DialogResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (!string.IsNullOrEmpty(currentRuleFile))
                            {
                                SaveFile(currentRuleFile);
                                tsslLastSave.Text = currentTime;
                            }
                            else
                            {
                                tsmiSaveAs_Click(sender, e);
                            }
                        }
                        LoadFile(ruleFile);
                    }
                }
                else
                    LoadFile(ruleFile);
            }
        }

        private void LoadFile(string strRuleFile)
        {
            tvCollectionTree.Nodes.Clear();
            CollectionBinder.Clear();
            Collection = new dmCollection(strRuleFile);
            CollectionBinder.Add(Collection);
            currentRuleFile = strRuleFile;
            this.FileChanged = false;
            PopulateTree();
        }

        private void SaveFile(string strFilePath)
        {

            string[] tmpData = Collection.TextVersion.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);


            if (strFilePath == ruleFile)
            {
                if (((dmUserInfo)UserInfoBinder.Current).ConfirmOverwrite)
                {
                    using (Question tmpQuest = new Question("This will overwrite your default configuration for Data Manager\r\nAre you sure you want to continue?"))
                    {
                        if (tmpQuest.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                        {
                            Collection.Save(strFilePath);
                            if (!Directory.Exists(RecovoryFolderPath))
                            {
                                Directory.CreateDirectory(RecovoryFolderPath);
                            }
                            Collection.Save(RecovoryFolderPath + "dataman.dmr");
                            UserFileSave(userFile);
                            currentRuleFile = ruleFile;
                            FileChanged = false;
                        }
                    }
                }
                else
                {
                    Collection.Save(strFilePath);
                    if (!Directory.Exists(RecovoryFolderPath))
                    {
                        Directory.CreateDirectory(RecovoryFolderPath);
                    }
                    Collection.Save(RecovoryFolderPath + "dataman.dmr");
                    UserFileSave(userFile);
                    currentRuleFile = ruleFile;
                    FileChanged = false;
                }
            }
            else
            {
                Collection.Save(strFilePath);
                currentRuleFile = strFilePath;
                FileChanged = false;
            }
        }

        private void UserFileSave(string userFile)
        {
            ((dmUserInfo)UserInfoBinder[0]).WriteFile(userFile);
            ((dmUserInfo)UserInfoBinder[0]).WriteFile(RecovoryFolderPath + "user.ini");
        }

        private void tsmiNew_Click(object sender, EventArgs e)
        {
            if (FileChanged)
            {
                Question tmpQuestion = new Question("File has changed since last save.\r\nWould you like to save before continuing?");
                DialogResult tmpResult = tmpQuestion.ShowDialog();
                if (tmpResult == System.Windows.Forms.DialogResult.Yes)
                {
                    tsmiSave_Click(sender, e);
                }
                else if (tmpResult == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }
            //clear CollectionBinder
            CollectionBinder.Clear();
            //clear the tree
            tvCollectionTree.Nodes.Clear();
            //reset the collection to a fresh new file
            Collection = new dmCollection();
            //Add Collection to the binder
            CollectionBinder.Add(Collection);
            //populate the tree
            PopulateTree();
            //set current file to nothing
            currentRuleFile = "";
            this.FileChanged = false;
            tsslCurrentFile.Text = "New File";
            tsslLastSave.Text = "Never";
        }

        private void tsmiFile_DropDownOpening(object sender, EventArgs e)
        {

        }

        private void tsmiProfileImport_Click(object sender, EventArgs e)
        {
            using (Question tmpQues = new Question())
            {
                if (tmpQues.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                {
                    if (tmpQues.DialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (!string.IsNullOrEmpty(currentRuleFile))
                        {
                            SaveFile(currentRuleFile);
                        }
                        else
                        {
                            tsmiSaveAs_Click(sender, e);
                        }
                    }
                    using (OpenFileDialog ofdOpen = new OpenFileDialog())
                    {
                        ofdOpen.Title = "Select a file to import to current";
                        ofdOpen.AutoUpgradeEnabled = true;
                        ofdOpen.InitialDirectory = RecovoryFolderPath;
                        ofdOpen.RestoreDirectory = true;
                        ofdOpen.Filter = FileFilters;
                        ofdOpen.DefaultExt = ".dat";
                        ofdOpen.FilterIndex = 0;

                        if (ofdOpen.ShowDialog() == DialogResult.OK)
                        {
                            tvCollectionTree.Nodes.Clear();
                            CollectionBinder.Clear();
                            Collection.Import(File.ReadAllLines(ofdOpen.FileName));
                            CollectionBinder.Add(Collection);
                            PopulateTree();
                            FileChanged = true;
                        }
                    }
                }
            }
        }

        #endregion

        #region Right-Click Context Menu

        private void cmsMenu_Opening(object sender, CancelEventArgs e)
        {
            tssContext1.Visible = false;
            tsmiContextCopy.Enabled = false;
            tsmiContextCut.Enabled = false;
            tsmiContextDelete.Enabled = false;
            tsmiContextExportGroup.Visible = false;
            tsmiCopyAll.Visible = false;
            tssContext2.Visible = false;
            tsmiAddGroup.Visible = false;
            tsmiAddRuleset.Visible = false;

            #region Tree Handling
            if (((ContextMenuStrip)sender).SourceControl == tvCollectionTree)
            {
                if (AlternativelySelectedNode != null)
                {
                    if (AlternativelySelectedNode != tvCollectionTree.SelectedNode)
                        AlternativelySelectedNode.BackColor = System.Drawing.Color.Yellow;

                    if (AlternativelySelectedNode.Tag != null)
                    {

                        tsmiAddGroup.Click -= btnGroupGroupAdd_Click;
                        tsmiAddGroup.Click -= btnCollectionGroupAdd_Click;
                        tsmiAddRuleset.Click -= btnCollectionRulesetsAdd_Click;
                        tsmiAddRuleset.Click -= btnGroupRulesetAdd_Click;

                        tsmiContextExportGroup.Tag = AlternativelySelectedNode;
                        tsmiContextCut.Tag = AlternativelySelectedNode;
                        tsmiContextCopy.Tag = AlternativelySelectedNode;
                        tsmiContextDelete.Tag = AlternativelySelectedNode;
                        tsmiContextPaste.Tag = AlternativelySelectedNode;
                        tsmiAddGroup.Tag = AlternativelySelectedNode;
                        tsmiAddRuleset.Tag = AlternativelySelectedNode;
                        cmsMenu.Tag = AlternativelySelectedNode;

                        //determine by type what menus should be enabled
                        Type tmpType = AlternativelySelectedNode.Tag.GetType();
                        ItemType clipboardContents = WhatsInTheClipboard();

                        if (tmpType == typeof(dmCollection))
                        {
                            // Only allow paste if there is a group or ruleset in clipboard
                            tsmiContextPaste.Enabled = clipboardContents == ItemType.Group || clipboardContents == ItemType.Ruleset;
                            tssContext2.Visible = tsmiAddGroup.Visible = tsmiAddGroup.Enabled = tsmiAddRuleset.Visible = tsmiAddRuleset.Enabled = true;
                            tsmiAddGroup.Click += btnCollectionGroupAdd_Click;
                            tsmiAddRuleset.Click += btnCollectionRulesetsAdd_Click;
                            tvCollectionTree_AfterSelect(sender, null);
                        }
                        else if (tmpType == typeof(dmGroup))
                        {
                            // Only allow paste if there is a group or ruleset in clipboard
                            tsmiContextPaste.Enabled = clipboardContents == ItemType.Group || clipboardContents == ItemType.Ruleset;
                            // Allow copy cut and delete
                            tsmiContextCopy.Enabled = tsmiContextCut.Enabled = tsmiContextDelete.Enabled = true;
                            //enable Group Export
                            tssContext1.Visible = tsmiContextExportGroup.Visible = tsmiContextExportGroup.Enabled = true;
                            tssContext2.Visible = tsmiAddGroup.Visible = tsmiAddGroup.Enabled = tsmiAddRuleset.Visible = tsmiAddRuleset.Enabled = true;
                            tsmiAddGroup.Click += btnGroupGroupAdd_Click;
                            tsmiAddRuleset.Click += btnGroupRulesetAdd_Click;
                            tvCollectionTree_AfterSelect(sender, null);
                        }
                        else if (tmpType == typeof(dmRuleset))
                        {
                            // Allow copy cut and delete
                            tsmiContextCopy.Enabled = tsmiContextCut.Enabled = tsmiContextDelete.Enabled = true;
                            tsmiContextPaste.Enabled = false;
                        }
                    }
                }
            }
            #endregion
            #region DataGrid Handling
            else if (((ContextMenuStrip)sender).SourceControl.GetType() == typeof(DataGridView))
            {
                Point tmpPoint = ((DataGridView)((ContextMenuStrip)sender).SourceControl).PointToClient(Cursor.Position);

                #region Collection Groups
                if (((DataGridView)((ContextMenuStrip)sender).SourceControl).Name == "dgvCollectionGroups")
                {
                    tsmiContextExportGroup.Tag = CollectionGroupBinder;
                    tsmiContextCut.Tag = CollectionGroupBinder;
                    tsmiContextCopy.Tag = CollectionGroupBinder;
                    tsmiContextDelete.Tag = CollectionGroupBinder;
                    tsmiContextPaste.Tag = CollectionGroupBinder;
                    tsmiContextExportGroup.Tag = CollectionGroupBinder;
                    tsmiContextExportGroup.Visible = true;

                    int tmpRowIndex = dgvCollectionGroups.HitTest(tmpPoint.X, tmpPoint.Y).RowIndex;
                    // Allow copy cut and delete according to if there is an item beneath the menus top left location
                    tsmiContextCopy.Enabled = tsmiContextCut.Enabled = tsmiContextDelete.Enabled = tsmiContextExportGroup.Enabled = tmpRowIndex > -1;

                    //only allow past if there is a group in the clipboard
                    tsmiContextPaste.Enabled = WhatsInTheClipboard() == ItemType.Group;
                }
                #endregion
                #region Collection Rulesets
                else if (((DataGridView)((ContextMenuStrip)sender).SourceControl).Name == "dgvCollectionRulesets")
                {
                    tsmiContextExportGroup.Tag = CollectionRulesetBinder;
                    tsmiContextCut.Tag = CollectionRulesetBinder;
                    tsmiContextCopy.Tag = CollectionRulesetBinder;
                    tsmiContextDelete.Tag = CollectionRulesetBinder;
                    tsmiContextPaste.Tag = CollectionRulesetBinder;

                    int tmpRowIndex = dgvCollectionRulesets.HitTest(tmpPoint.X, tmpPoint.Y).RowIndex;
                    // Allow copy cut and delete according to if there is an item beneath the menus top left location
                    tsmiContextCopy.Enabled = tsmiContextCut.Enabled = tsmiContextDelete.Enabled = tmpRowIndex > -1;

                    //only allow past if there is a ruleset in the clipboard
                    tsmiContextPaste.Enabled = WhatsInTheClipboard() == ItemType.Ruleset;
                }
                #endregion
                #region Group Groups
                else if (((DataGridView)((ContextMenuStrip)sender).SourceControl).Name == "dgvGroupGroups")
                {
                    tsmiContextExportGroup.Tag = GroupGroupBinder;
                    tsmiContextCut.Tag = GroupGroupBinder;
                    tsmiContextCopy.Tag = GroupGroupBinder;
                    tsmiContextDelete.Tag = GroupGroupBinder;
                    tsmiContextPaste.Tag = GroupGroupBinder;
                    tsmiContextExportGroup.Tag = GroupGroupBinder;
                    tsmiContextExportGroup.Visible = true;

                    int tmpRowIndex = dgvGroupGroups.HitTest(tmpPoint.X, tmpPoint.Y).RowIndex;
                    // Allow copy cut and delete according to if there is an item beneath the menus top left location
                    tsmiContextCopy.Enabled = tsmiContextCut.Enabled = tsmiContextDelete.Enabled = tsmiContextExportGroup.Enabled = tmpRowIndex > -1;

                    //only allow past if there is a group in the clipboard
                    tsmiContextPaste.Enabled = WhatsInTheClipboard() == ItemType.Group;
                }
                #endregion
                #region Group Rulesets
                else if (((DataGridView)((ContextMenuStrip)sender).SourceControl).Name == "dgvGroupRulesets")
                {
                    tsmiContextExportGroup.Tag = GroupRulesetBinder;
                    tsmiContextCut.Tag = GroupRulesetBinder;
                    tsmiContextCopy.Tag = GroupRulesetBinder;
                    tsmiContextDelete.Tag = GroupRulesetBinder;
                    tsmiContextPaste.Tag = GroupRulesetBinder;

                    int tmpRowIndex = dgvGroupRulesets.HitTest(tmpPoint.X, tmpPoint.Y).RowIndex;
                    // Allow copy cut and delete according to if there is an item beneath the menus top left location
                    tsmiContextCopy.Enabled = tsmiContextCut.Enabled = tsmiContextDelete.Enabled = tmpRowIndex > -1;

                    //only allow past if there is a ruleset in the clipboard
                    tsmiContextPaste.Enabled = WhatsInTheClipboard() == ItemType.Ruleset;
                }
                #endregion
                #region Rules
                else if (((DataGridView)((ContextMenuStrip)sender).SourceControl).Name == "dgvRulesetRules")
                {
                    tsmiContextExportGroup.Tag = RuleBinder;
                    tsmiContextCut.Tag = RuleBinder;
                    tsmiContextCopy.Tag = RuleBinder;
                    tsmiContextDelete.Tag = RuleBinder;
                    tsmiContextPaste.Tag = RuleBinder;
                    tsmiCopyAll.Tag = RuleBinder;

                    int tmpRowIndex = dgvRulesetRules.HitTest(tmpPoint.X, tmpPoint.Y).RowIndex;
                    // Allow copy cut and delete according to if there is an item beneath the menus top left location
                    tsmiContextCopy.Enabled = tsmiContextCut.Enabled = tsmiContextDelete.Enabled = tmpRowIndex > -1;

                    //enable paste according to whether or not a rule exists in the clipboard
                    tsmiContextPaste.Enabled = WhatsInTheClipboard() == ItemType.Rule || WhatsInTheClipboard() == ItemType.ListOfRules;

                    //enable the user to copy All
                    tsmiCopyAll.Visible = true;
                    tsmiCopyAll.Enabled = RuleBinder.Count > 0;
                }
                else if (((DataGridView)((ContextMenuStrip)sender).SourceControl).Name == "dgvGroupFilters")
                {
                    tsmiContextExportGroup.Tag = GroupFiltersBinder;
                    tsmiContextCut.Tag = GroupFiltersBinder;
                    tsmiContextCopy.Tag = GroupFiltersBinder;
                    tsmiContextDelete.Tag = GroupFiltersBinder;
                    tsmiContextPaste.Tag = GroupFiltersBinder;
                    tsmiCopyAll.Tag = GroupFiltersBinder;

                    int tmpRowIndex = dgvGroupFilters.HitTest(tmpPoint.X, tmpPoint.Y).RowIndex;
                    // Allow copy cut and delete according to if there is an item beneath the menus top left location
                    tsmiContextCopy.Enabled = tsmiContextCut.Enabled = tsmiContextDelete.Enabled = tmpRowIndex > -1;

                    //enable paste according to whether or not a rule exists in the clipboard
                    tsmiContextPaste.Enabled = WhatsInTheClipboard() == ItemType.Rule || WhatsInTheClipboard() == ItemType.ListOfRules;

                    //enable the user to copy All
                    tsmiCopyAll.Visible = true;
                    tsmiCopyAll.Enabled = RuleBinder.Count > 0;
                }
                #endregion
                #region Actions
                else if (((DataGridView)((ContextMenuStrip)sender).SourceControl).Name == "dgvRulesetActions")
                {
                    tsmiContextExportGroup.Tag = ActionBinder;
                    tsmiContextCut.Tag = ActionBinder;
                    tsmiContextCopy.Tag = ActionBinder;
                    tsmiContextDelete.Tag = ActionBinder;
                    tsmiContextPaste.Tag = ActionBinder;
                    tsmiCopyAll.Tag = ActionBinder;

                    int tmpRowIndex = dgvRulesetActions.HitTest(tmpPoint.X, tmpPoint.Y).RowIndex;
                    // Allow copy cut and delete according to if there is an item beneath the menus top left location
                    tsmiContextCopy.Enabled = tsmiContextCut.Enabled = tsmiContextDelete.Enabled = tmpRowIndex > -1;

                    //enable paste according to whether or not a action exists in the clipboard
                    tsmiContextPaste.Enabled = WhatsInTheClipboard() == ItemType.Action || WhatsInTheClipboard() == ItemType.ListOfActions;

                    //enable the user to copy All
                    tsmiCopyAll.Visible = true;
                    tsmiCopyAll.Enabled = ActionBinder.Count > 0;
                }
                else if (((DataGridView)((ContextMenuStrip)sender).SourceControl).Name == "dgvGroupDefaults")
                {
                    tsmiContextExportGroup.Tag = GroupDefaultsBinder;
                    tsmiContextCut.Tag = GroupDefaultsBinder;
                    tsmiContextCopy.Tag = GroupDefaultsBinder;
                    tsmiContextDelete.Tag = GroupDefaultsBinder;
                    tsmiContextPaste.Tag = GroupDefaultsBinder;
                    tsmiCopyAll.Tag = GroupDefaultsBinder;

                    int tmpRowIndex = dgvRulesetActions.HitTest(tmpPoint.X, tmpPoint.Y).RowIndex;
                    // Allow copy cut and delete according to if there is an item beneath the menus top left location
                    tsmiContextCopy.Enabled = tsmiContextCut.Enabled = tsmiContextDelete.Enabled = tmpRowIndex > -1;

                    //enable paste according to whether or not a action exists in the clipboard
                    tsmiContextPaste.Enabled = WhatsInTheClipboard() == ItemType.Action || WhatsInTheClipboard() == ItemType.ListOfActions;

                    //enable the user to copy All
                    tsmiCopyAll.Visible = true;
                    tsmiCopyAll.Enabled = ActionBinder.Count > 0;
                }
                #endregion
            }
            #endregion
        }

        private void cmsMenu_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (AlternativelySelectedNode != null)
            {
                AlternativelySelectedNode.BackColor = System.Drawing.Color.White;
                AlternativelySelectedNode = null;
            }
        }

        object GetClipBoardItemFromBinder(BindingSource binder)
        {
            return binder.Current;
        }

        private void tsmiContextCopy_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            if (((ToolStripMenuItem)sender).Tag is TreeNode)
            {
                TreeNode tmpNode = (TreeNode)((ToolStripMenuItem)sender).Tag;
                Program.SendToClipboard(tmpNode.Tag);
            }
            else if (((ToolStripMenuItem)sender).Tag.GetType() == typeof(BindingSource))
            {
                object tmpObject = GetClipBoardItemFromBinder((BindingSource)((ToolStripMenuItem)sender).Tag);
                if (tmpObject != null) Program.SendToClipboard(tmpObject);
            }
        }

        private void tsmiContextCut_Click(object sender, EventArgs e)
        {
            TreeNode operativeNode = null;
            object item = null;
            Clipboard.Clear();

            if (((ToolStripMenuItem)sender).Tag is TreeNode)
            {
                operativeNode = (TreeNode)((ToolStripMenuItem)sender).Tag;
                item = operativeNode.Tag;
                Program.SendToClipboard(item);
                tsmiContextDelete_Click(tsmiContextDelete, e);
                FileChanged = true;
            }
            else if (((ToolStripMenuItem)sender).Tag.GetType() == typeof(BindingSource))
            {
                BindingSource binder = (BindingSource)((ToolStripMenuItem)sender).Tag;
                item = GetClipBoardItemFromBinder(binder);
                if (item != null) Program.SendToClipboard(item);
                else return;
                //use delete function to remove
                tsmiContextDelete_Click(tsmiContextDelete, e);
                FileChanged = true;
                RulesetBinder.ResetCurrentItem();
            }
        }

        private void tsmiContextPaste_Click(object sender, EventArgs e)
        {
            ItemType tmpType = WhatsInTheClipboard();
            IDataObject tmp = Clipboard.GetDataObject();

            if (((ToolStripMenuItem)sender).Tag == null) return;
            switch (tmpType)
            {
                #region Rules & Actions
                case ItemType.Action:
                    if (((ToolStripMenuItem)sender).Tag.GetType() == typeof(BindingSource))
                    {
                        string tmpParameters = (string)tmp.GetData(typeof(dmAction).FullName);
                        ((BindingSource)((ToolStripMenuItem)sender).Tag).Add(new dmAction(XElement.Parse(tmpParameters)));
                        FileChanged = true;
                        RulesetBinder.ResetCurrentItem();
                    }
                    break;
                case ItemType.ListOfActions:
                    if (((ToolStripMenuItem)sender).Tag.GetType() == typeof(BindingSource))
                    {
                        string tmpParameters = (string)tmp.GetData(typeof(List<dmAction>).FullName);
                        XElement xElement = XElement.Parse(tmpParameters);
                        foreach (XElement item in xElement.Elements("action"))
                        {
                            ((BindingSource)((ToolStripMenuItem)sender).Tag).Add(new dmAction(item));
                            FileChanged = true;
                            RulesetBinder.ResetCurrentItem();
                        }
                    }
                    break;
                case ItemType.Rule:
                    if (((ToolStripMenuItem)sender).Tag.GetType() == typeof(BindingSource))
                    {
                        string tmpParameters = (string)tmp.GetData(typeof(dmRule).FullName);
                        ((BindingSource)((ToolStripMenuItem)sender).Tag).Add(new dmRule(XElement.Parse(tmpParameters)));
                        FileChanged = true;
                        RulesetBinder.ResetCurrentItem();
                    }
                    break;
                case ItemType.ListOfRules:
                    if (((ToolStripMenuItem)sender).Tag.GetType() == typeof(BindingSource))
                    {
                        string tmpParameters = (string)tmp.GetData(typeof(List<dmRule>).FullName);
                        XElement xElement = XElement.Parse(tmpParameters);
                        foreach (XElement item in xElement.Elements("rule"))
                        {
                            ((BindingSource)((ToolStripMenuItem)sender).Tag).Add(new dmRule(item));
                            FileChanged = true;
                            RulesetBinder.ResetCurrentItem();
                        }
                    }
                    break;
                #endregion
                #region Ruleset
                case ItemType.Ruleset:
                    #region TreeNode As Tag
                    if (((ToolStripMenuItem)sender).Tag is TreeNode)
                    {
                        TreeNode item = (TreeNode)((ToolStripMenuItem)sender).Tag;
                        if (item == tvCollectionTree.SelectedNode)
                        {
                            if (item.Tag.GetType() == typeof(dmCollection))
                            {
                                dmRuleset tmpRS = new dmRuleset((dmCollection)item.Tag, XElement.Parse((string)tmp.GetData(typeof(dmRuleset).FullName)));
                                CollectionRulesetBinder.Add(tmpRS);
                                //create the new Node
                                TreeNode newRSNode = CreateRulesetNode(tmpRS);
                                //add the new node
                                item.Nodes.Add(newRSNode);
                                FileChanged = true;
                            }
                            else if (item.Tag.GetType() == typeof(dmGroup))
                            {
                                dmRuleset tmpRS = new dmRuleset((dmContainer)item.Tag, XElement.Parse((string)tmp.GetData(typeof(dmRuleset).FullName)));
                                GroupRulesetBinder.Add(tmpRS);
                                //create the new Node
                                TreeNode newRSNode = CreateRulesetNode(tmpRS);
                                //add the new node
                                item.Nodes.Add(newRSNode);
                                FileChanged = true;
                            }
                        }
                        else if (item.Tag.GetType().BaseType == typeof(dmContainer))
                        {
                            dmRuleset tmpRS = new dmRuleset((dmContainer)item.Tag, XElement.Parse((string)tmp.GetData(typeof(dmRuleset).FullName)));
                            ((dmContainer)item.Tag).AddRuleset(tmpRS);
                            //create the new Node
                            TreeNode newRSNode = CreateRulesetNode(tmpRS);
                            //add the new node
                            item.Nodes.Add(newRSNode);
                            FileChanged = true;
                        }
                    }
                    #endregion
                    #region Binder As Tag
                    else if (((ToolStripMenuItem)sender).Tag.GetType() == typeof(BindingSource))
                    {
                        BindingSource tmpSource = (BindingSource)((ToolStripMenuItem)sender).Tag;
                        if (tmpSource == CollectionRulesetBinder)
                        {
                            dmRuleset tmpRS = new dmRuleset((dmContainer)Collection, XElement.Parse((string)tmp.GetData(typeof(dmRuleset).FullName)));
                            CollectionRulesetBinder.Add(tmpRS);
                            //create the new Node
                            TreeNode newRSNode = CreateRulesetNode(tmpRS);
                            //add the new node
                            tvCollectionTree.SelectedNode.Nodes.Add(newRSNode);
                            FileChanged = true;
                        }
                        else if (tmpSource == GroupRulesetBinder)
                        {
                            dmRuleset tmpRS = new dmRuleset((dmContainer)tvCollectionTree.SelectedNode.Tag, XElement.Parse((string)tmp.GetData(typeof(dmRuleset).FullName)));
                            GroupRulesetBinder.Add(tmpRS);
                            //create the new Node
                            TreeNode newRSNode = CreateRulesetNode(tmpRS);
                            //add the new node
                            tvCollectionTree.SelectedNode.Nodes.Add(newRSNode);
                            FileChanged = true;
                        }
                    }
                    #endregion
                    break;
                #endregion
                #region Group
                case ItemType.Group:
                    string GroupInfo = (string)tmp.GetData(typeof(dmGroup).FullName);
                    ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
                    //create the treenode for the group

                    if (menuItem.Tag is TreeNode)
                    {
                        TreeNode operativeNode = (TreeNode)menuItem.Tag;

                        if (operativeNode == tvCollectionTree.SelectedNode)
                        {
                            //if we're dealing with the currently selected node use Binder instead
                            if (operativeNode.Tag.GetType() == typeof(dmGroup))
                            {
                                dmGroup tmpGroup = new dmGroup((dmContainer)operativeNode.Tag, XElement.Parse(GroupInfo));
                                TreeNode newNode = CreateGroupNode(tmpGroup);
                                GroupGroupBinder.Add(tmpGroup);
                                tvCollectionTree.SelectedNode.Nodes.Insert(GroupGroupBinder.IndexOf(tmpGroup), newNode);
                                FileChanged = true;
                            }
                            else if (operativeNode.Tag.GetType() == typeof(dmCollection))
                            {
                                dmGroup tmpGroup = new dmGroup((dmContainer)operativeNode.Tag, XElement.Parse(GroupInfo));
                                TreeNode newNode = CreateGroupNode(tmpGroup);
                                CollectionGroupBinder.Add(tmpGroup);
                                tvCollectionTree.SelectedNode.Nodes.Insert(CollectionGroupBinder.IndexOf(tmpGroup), newNode);
                                FileChanged = true;
                            }

                        }
                        else if (operativeNode.Tag.GetType().BaseType == typeof(dmContainer))
                        {
                            dmContainer container = (dmContainer)operativeNode.Tag;
                            dmGroup tmpGroup = new dmGroup((dmContainer)operativeNode.Tag, XElement.Parse(GroupInfo));
                            TreeNode newNode = CreateGroupNode(tmpGroup);
                            container.AddGroup(tmpGroup);
                            operativeNode.Nodes.Insert(container.IndexOfGroup(tmpGroup), newNode);
                            FileChanged = true;
                        }
                    }
                    else if (menuItem.Tag.GetType() == typeof(BindingSource))
                    {
                        TreeNode operativeNode = tvCollectionTree.SelectedNode;
                        //are we dealing with a container node?
                        if (operativeNode.Tag.GetType().BaseType == typeof(dmContainer))
                        {
                            dmContainer container = (dmContainer)operativeNode.Tag;
                            //add the Group to the container
                            dmGroup tmpGroup = new dmGroup((dmContainer)operativeNode.Tag, XElement.Parse(GroupInfo));
                            TreeNode newNode = CreateGroupNode(tmpGroup);
                            container.AddGroup(tmpGroup);
                            //add the treenode to the operativenode
                            operativeNode.Nodes.Add(newNode);
                            //report that file is changed
                            FileChanged = true;
                        }
                        //otherwise do nothing
                    }
                    break;
                #endregion
                default:
                    break;
            }
        }

        private void tsmiContextDelete_Click(object sender, EventArgs e)
        {
            TreeNode parentNode = null;
            TreeNode operativeNode = null;
            object item = null;

            if (((ToolStripMenuItem)sender).Tag is TreeNode)
            {
                operativeNode = (TreeNode)((ToolStripMenuItem)sender).Tag;
                item = operativeNode.Tag;
                //use indicated treenode to get parent                
                parentNode = operativeNode.Parent;

                if (parentNode != null)
                {
                    if (parentNode.Tag.GetType().BaseType == typeof(dmContainer))
                    {
                        dmContainer container = (dmContainer)parentNode.Tag;

                        if (item.GetType() == typeof(dmRuleset))
                        {
                            container.RemoveRuleset((dmRuleset)item);
                        }
                        else if (item.GetType() == typeof(dmGroup))
                        {
                            container.RemoveGroup((dmGroup)item);
                        }
                        operativeNode.Remove();
                        FileChanged = true;
                        RulesetBinder.ResetCurrentItem();
                    }
                }
            }
            else if (((ToolStripMenuItem)sender).Tag.GetType() == typeof(BindingSource))
            {
                BindingSource binder = (BindingSource)((ToolStripMenuItem)sender).Tag;
                item = GetClipBoardItemFromBinder(binder);

                //parentNode is current node
                parentNode = tvCollectionTree.SelectedNode;
                //get operative node from binder position
                if (binder == CollectionGroupBinder)
                    operativeNode = parentNode.Nodes[binder.IndexOf(item)];
                else if (binder == CollectionRulesetBinder)
                    operativeNode = parentNode.Nodes[binder.IndexOf(item) + CollectionGroupBinder.Count];
                else if (binder == GroupRulesetBinder)
                    operativeNode = parentNode.Nodes[binder.IndexOf(item) + GroupGroupBinder.Count];
                else if (binder == GroupGroupBinder)
                    operativeNode = parentNode.Nodes[binder.IndexOf(item)];
                else operativeNode = null;

                if (binder.Position > -1)
                {
                    binder.RemoveCurrent();
                    if (operativeNode != null)
                    {
                        operativeNode.Remove();
                    }
                    FileChanged = true;
                    RulesetBinder.ResetCurrentItem();
                }
            }
        }

        public ItemType WhatsInTheClipboard()
        {
            ItemType tmpType = ItemType.NothingUseable;
            if (Clipboard.ContainsData(typeof(dmRule).FullName))
                tmpType = ItemType.Rule;
            if (Clipboard.ContainsData(typeof(dmAction).FullName))
                tmpType = ItemType.Action;
            if (Clipboard.ContainsData(typeof(dmRuleset).FullName))
                tmpType = ItemType.Ruleset;
            if (Clipboard.ContainsData(typeof(dmGroup).FullName))
                tmpType = ItemType.Group;
            if (Clipboard.ContainsData(typeof(List<dmRule>).FullName))
                tmpType = ItemType.ListOfRules;
            if (Clipboard.ContainsData(typeof(List<dmAction>).FullName))
                tmpType = ItemType.ListOfActions;
            return tmpType;
        }

        #region Group Specific

        private void tsmiGroupExport_Click(object sender, EventArgs e)
        {
            //Create a seperate RulesetCollection From Group

            ToolStripMenuItem tsmiTemp = (ToolStripMenuItem)sender;
            dmCollection tmp = null;

            if (tsmiTemp.Tag is TreeNode)
            {
                TreeNode tmpNode = (TreeNode)tsmiTemp.Tag;

                object item = tmpNode.Tag;
                if (item.GetType() == typeof(dmGroup))
                {
                    tmp = new dmCollection();
                    tmp.AddGroup((dmGroup)item);
                }
            }
            else if (tsmiTemp.Tag.GetType() == typeof(BindingSource))
            {
                BindingSource binder = (BindingSource)tsmiTemp.Tag;

                object item = binder.Current;
                if (item.GetType() == typeof(dmGroup))
                {
                    tmp = new dmCollection();
                    tmp.AddGroup((dmGroup)item);
                }
            }

            if (tmp != null)
            {
                using (SaveFileDialog tmpSave = new SaveFileDialog())
                {
                    tmpSave.AutoUpgradeEnabled = true;
                    tmpSave.OverwritePrompt = true;
                    tmpSave.Filter = FileFilters;
                    tmpSave.FilterIndex = 1;
                    tmpSave.AddExtension = true;
                    tmpSave.DefaultExt = ".dmr";

                    if (tmpSave.ShowDialog() == DialogResult.OK)
                    {
                        using (Question tmpQues = new Question("Would you like to export the current Author signature with this file"))
                        {
                            if (tmpQues.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                            {
                                if (tmpQues.DialogResult == System.Windows.Forms.DialogResult.Yes)
                                { tmp.Author = ((dmCollection)CollectionBinder.Current).Author; }
                                File.WriteAllText(tmpSave.FileName, tmp.TextVersion);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

        #endregion

        public void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabSearch)
            {
                CollectionBinder.ResetBindings(metadataChanged: false);
                if (FileChangedSinceSearch)
                {
                    SearchResultsTreeBinder.Clear();
                    FileChangedSinceSearch = false;
                }
            }
        }

        private void DataGrid_Sorted(object sender, EventArgs e)
        {
            //kill the decendants
            tvCollectionTree.SelectedNode.Nodes.Clear();
            //rebuild the tree 
            for (int i = 0; i < ((dmContainer)tvCollectionTree.SelectedNode.Tag).Groups.Count; i++)
            {
                TreeNode tmpNode = CreateGroupNode(((dmContainer)tvCollectionTree.SelectedNode.Tag).Groups[i]);
                tvCollectionTree.SelectedNode.Nodes.Add(tmpNode);
            }
            for (int i = 0; i < ((dmContainer)tvCollectionTree.SelectedNode.Tag).Rulesets.Count; i++)
            {
                TreeNode tmpNode = CreateRulesetNode(((dmContainer)tvCollectionTree.SelectedNode.Tag).Rulesets[i]);
                tvCollectionTree.SelectedNode.Nodes.Add(tmpNode);
            }
            FileChanged = true;
        }
        private void txtRulesetComment_Validated(object sender, EventArgs e)
        {
            RulesetBinder.ResetCurrentItem();
        }

        private void txtRulesetComment_Validating(object sender, CancelEventArgs e)
        {
            if (RulesetBinder.Position < 0) return;
            if (((dmRuleset)RulesetBinder.Current).Comment != txtRulesetComment.Text)
                FileChanged = true;
        }

        private void txtRulesetName_Validating(object sender, CancelEventArgs e)
        {
            if (RulesetBinder.Position < 0) return;
            if (((dmRuleset)RulesetBinder.Current).Name == txtRulesetName.Text)
                FileChanged = true;
        }

        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            SettingsConfig tmp = new SettingsConfig((dmUserInfo)UserInfoBinder[0]);
            tmp.ShowDialog();
            UserFileSave(userFile);
        }

        private void tsmiCopyAll_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tmpItem = (ToolStripMenuItem)sender;
            if (tmpItem.Tag.GetType() == typeof(BindingSource))
            {
                BindingSource tmpSource = (BindingSource)tmpItem.Tag;

                if (tmpSource == RuleBinder)
                {
                    List<dmRule> list = new List<dmRule>();
                    foreach (dmRule item in RuleBinder.OfType<dmRule>())
                    {
                        list.Add(item);
                    }
                    Program.SendToClipboard(list);
                    return;
                }
                List<dmAction> list2 = new List<dmAction>();
                foreach (dmAction item2 in ActionBinder.OfType<dmAction>())
                {
                    list2.Add(item2);
                }
                Program.SendToClipboard(list2);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FileChanged = true;
        }

        private void SearchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool NameSearch = (bool)((object[])e.Argument)[0];
            bool ParameterSearch = (bool)((object[])e.Argument)[1];
            bool GeneralSearch = (bool)((object[])e.Argument)[2];
            string strField = (string)((object[])e.Argument)[3];
            string strModifier = (string)((object[])e.Argument)[4];
            string strValue = (string)((object[])e.Argument)[5];

            Regex tmpRegEx = new Regex(strValue);

            List<TreeNode> rulesetNodes = new List<TreeNode>(CollectRulesets(tvCollectionTree.Nodes[0]));
            rulesetNodes.AddRange(CollectRulesets(tvCollectionTree.Nodes[1]));
            List<TreeNode> groupNodes = new List<TreeNode>(CollectGroups(tvCollectionTree.Nodes[0]));
            groupNodes.AddRange(CollectGroups(tvCollectionTree.Nodes[1]));


            int operationCount = groupNodes.Count + rulesetNodes.Count;

            #region Parameter Search

            if (ParameterSearch || GeneralSearch)
            {
                string fieldString = !string.IsNullOrWhiteSpace(strField) ? "^.*?" + strField.ToLowerInvariant() + "[^.]*" : ".*?";
                string modifierString = !string.IsNullOrWhiteSpace(strModifier) ? "^" + strModifier.ToLowerInvariant() + "$*" : ".*?";
                string valueString = !string.IsNullOrWhiteSpace(strValue) ? "^.*?" + strValue.ToLowerInvariant() + ".*$" : ".*$";

                string strRegExp = string.Format("<<{0}\\.{1}:{2}>>", new string[] { fieldString, modifierString, valueString });
                tmpRegEx = new Regex(strRegExp, RegexOptions.IgnoreCase);

                if (!SearchWorker.CancellationPending)
                {
                    for (int i = 0; i < rulesetNodes.Count; i++)
                    {
                        for (int j = 0; j < ((dmRuleset)rulesetNodes[i].Tag).Rules.Count; j++)
                        {
                            dmRule dmRule2 = ((dmRuleset)rulesetNodes[i].Tag).Rules[j];
                            if (!SearchWorker.CancellationPending)
                                if (Regex.Match(dmRule2.Field, fieldString, RegexOptions.IgnoreCase).Success &&
                                    Regex.Match(dmRule2.Modifier, modifierString, RegexOptions.IgnoreCase).Success &&
                                    Regex.Match(dmRule2.Value, valueString, RegexOptions.IgnoreCase).Success)
                                    SearchWorker.ReportProgress((int)((100f / (float)operationCount) * (float)i), new TreeSearchResults(rulesetNodes[i], "Ruleset Rule #" + (j + 1)));
                        }
                        if (!SearchWorker.CancellationPending)
                        {
                            for (int j = 0; j < ((dmRuleset)rulesetNodes[i].Tag).Actions.Count; j++)
                            {
                                dmAction dmAction2 = ((dmRuleset)rulesetNodes[i].Tag).Actions[j];
                                if (!SearchWorker.CancellationPending)
                                    if (Regex.Match(dmAction2.Field, fieldString, RegexOptions.IgnoreCase).Success &&
                                        Regex.Match(dmAction2.Modifier, modifierString, RegexOptions.IgnoreCase).Success &&
                                        Regex.Match(dmAction2.Value, valueString, RegexOptions.IgnoreCase).Success)
                                        SearchWorker.ReportProgress((int)((100f / (float)operationCount) * (float)i), new TreeSearchResults(rulesetNodes[i], "Ruleset Action #" + (j + 1)));
                            }
                        }
                    }
                }
                if (!SearchWorker.CancellationPending)
                {
                    for (int i = 0; i < groupNodes.Count; i++)
                    {
                        for (int j = 0; j < ((dmGroup)groupNodes[i].Tag).FiltersAndDefaults.Rules.Count; j++)
                        {
                            dmRule dmRule2 = ((dmGroup)groupNodes[i].Tag).FiltersAndDefaults.Rules[j];
                            if (!SearchWorker.CancellationPending)
                                if (Regex.Match(dmRule2.Field, fieldString, RegexOptions.IgnoreCase).Success &&
                                    Regex.Match(dmRule2.Modifier, modifierString, RegexOptions.IgnoreCase).Success &&
                                    Regex.Match(dmRule2.Value, valueString, RegexOptions.IgnoreCase).Success)
                                    SearchWorker.ReportProgress((int)((100f / (float)operationCount) * (float)i), new TreeSearchResults(groupNodes[i], "Group Rule #" + (j + 1)));
                        }
                        if (!SearchWorker.CancellationPending)
                        {
                            for (int j = 0; j < ((dmGroup)groupNodes[i].Tag).FiltersAndDefaults.Actions.Count; j++)
                            {
                                dmAction dmAction2 = ((dmGroup)groupNodes[i].Tag).FiltersAndDefaults.Actions[j];
                                if (!SearchWorker.CancellationPending)
                                    if (Regex.Match(dmAction2.Field, fieldString, RegexOptions.IgnoreCase).Success &&
                                        Regex.Match(dmAction2.Modifier, modifierString, RegexOptions.IgnoreCase).Success &&
                                        Regex.Match(dmAction2.Value, valueString, RegexOptions.IgnoreCase).Success)
                                        SearchWorker.ReportProgress((int)((100f / (float)operationCount) * (float)i), new TreeSearchResults(groupNodes[i], "GroupAction #" + (j + 1)));
                            }
                        }
                    }
                }
            }
            #endregion
            #region NameSearch
            //also runs if General Search is enabled
            if (NameSearch || GeneralSearch)
            {
                tmpRegEx = new Regex(strValue, RegexOptions.IgnoreCase);

                if (!SearchWorker.CancellationPending)
                {
                    for (int i = 0; i < rulesetNodes.Count; i++)
                    {
                        if (!SearchWorker.CancellationPending)
                            if (tmpRegEx.Match(((dmRuleset)rulesetNodes[i].Tag).Name).Success)
                                SearchWorker.ReportProgress((int)(((float)operationCount / 100f) * (float)i), new TreeSearchResults(rulesetNodes[i], "Rules/Actions"));
                    }
                    for (int i = 0; i < groupNodes.Count; i++)
                    {
                        if (!SearchWorker.CancellationPending)
                            if (tmpRegEx.Match(((dmGroup)groupNodes[i].Tag).Name).Success)
                                SearchWorker.ReportProgress((int)((((float)operationCount / 100f) * ((float)rulesetNodes.Count + groupNodes.Count + 1))), new TreeSearchResults(groupNodes[i], "Group Filters & Defaults"));
                    }
                }
            }

            if (GeneralSearch)
            {
                //search everthing else
                for (int i = 0; i < groupNodes.Count; i++)
                {
                    //search in other fields
                    if (tmpRegEx.Match(((dmGroup)groupNodes[i].Tag).Comment).Success)
                        SearchWorker.ReportProgress(100, new TreeSearchResults(groupNodes[i], "Group Comment"));
                    if (tmpRegEx.Match(((dmRuleset)rulesetNodes[i].Tag).Comment).Success)
                        SearchWorker.ReportProgress(100, new TreeSearchResults(groupNodes[i], "Ruleset Comment"));
                }
            }
            #endregion

        }

        private void SearchWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<ProgressChangedEventArgs>(SearchWorker_ProgressChanged), new object[] { sender, e });
                return;
            }

            if (e.ProgressPercentage < 101)
                tspbProgress.Value = e.ProgressPercentage;
            else
            { }
            if (e.UserState != null)
                AddSearchResults((SearchResults)e.UserState);
        }

        delegate void SearchResultsAdd(SearchResults srResults);

        private void AddSearchResults(SearchResults srSearchResults)
        {
            if (InvokeRequired)
            {
                Invoke(new SearchResultsAdd(AddSearchResults), new object[] { srSearchResults });
                return;
            }

            if (srSearchResults.GetType() == typeof(TreeSearchResults))
                SearchResultsTreeBinder.Add(srSearchResults);
        }

        private void SearchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tspbProgress.Value = 0;
            tspbProgress.Visible = false;
            UnlockSearch();
        }

        void LockSearch()
        {
            tsbRunSearch.Image = DataManagerGUI.Properties.Resources.delete;
            tscbSearchField.Enabled = tscbSearchModifier.Enabled = tstbValue.Enabled = false;
        }

        void UnlockSearch()
        {
            if (tabControl1.InvokeRequired)
            {
                Invoke(new Action(UnlockSearch));
                return;
            }

            tsbRunSearch.Image = DataManagerGUI.Properties.Resources.Search;

            tscbSearchField.Enabled = tscbSearchModifier.Enabled = tstbValue.Enabled = true;
        }

        private void tscbSearchField_SelectedIndexChanged(object sender, EventArgs e)
        {
            tscbSearchModifier.Items.Clear();
            tscbSearchModifier.Items.Add("");
            if (tscbSearchField.Text != "")
                tscbSearchModifier.Items.AddRange(Global.GetAllAllowedModifiers(tscbSearchField.Text));
            else
                tscbSearchModifier.Items.AddRange(Global.GetAllModifiers());



            if (tscbSearchModifier.Items.Count > 0) tscbSearchModifier.SelectedIndex = 0;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (SearchWorker.IsBusy)
            {
                SearchWorker.CancelAsync();
                return;
            }

            SearchResultsTreeBinder.Clear();

            bool bNameSearch = tscbSearchType.Text == "" || tscbSearchType.Text == "Group/Ruleset Name Search";
            bool bParameterSearch = tscbSearchType.Text == "Rule/Action Parameter Search";
            bool bGeneralSearch = tscbSearchType.Text == "Search All";

            /*
             Rule/Action Parameter Search
             Group/Ruleset Name Search
             *Search All
             */


            label6.Text = "Search Results:";


            if (bParameterSearch)
            {
                string strTemp = tscbSearchField.Text;
                if (tscbSearchField.Text == "") label6.Text += "  [Field: Any]";
                else label6.Text += string.Format("  [Value: {0}]", strTemp);

                strTemp = tscbSearchType.Text;
                if (tscbSearchModifier.Text == "") label6.Text += "  [Modifier: Any]";
                else label6.Text += string.Format("  [Value: {0}]", strTemp);

                strTemp = tstbValue.Text;
                if (tstbValue.Text == "") label6.Text += "  [Value: Any]";
                else label6.Text += string.Format(" [Value: {0}]", strTemp);
            }
            if (bNameSearch)
                label6.Text = string.Format("Group/Ruleset Name Search: {0}", tstbValue.Text);
            if (bGeneralSearch)
                label6.Text = string.Format("Search All: {0}", tstbValue.Text);

            LockSearch();
            tspbProgress.Visible = true;

            SearchWorker.RunWorkerAsync(new object[] { bNameSearch, bParameterSearch, bGeneralSearch, tscbSearchField.Text, tscbSearchModifier.Text, tstbValue.Text });
        }

        private TreeNode[] CollectRulesets(TreeNode node)
        {
            List<TreeNode> Results = new List<TreeNode>();

            if (node.GetType() == typeof(dmRuleset))
                Results.Add(node);

            foreach (TreeNode item in node.Nodes)
            {
                if (item.Tag.GetType() == typeof(dmGroup))
                {
                    Results.AddRange(CollectRulesets(item));
                }
                if (item.Tag.GetType() == typeof(dmRuleset))
                {
                    Results.Add(item);
                }
            }
            return Results.ToArray();
        }

        private IEnumerable<TreeNode> CollectGroups(TreeNode node)
        {
            List<TreeNode> Results = new List<TreeNode>();

            if (node.Tag.GetType() == typeof(dmGroup))
                Results.Add(node);

            foreach (TreeNode item in node.Nodes)
            {
                if (item.Tag.GetType() == typeof(dmGroup))
                {
                    Results.AddRange(CollectGroups(item));
                }
            }
            return Results.ToArray();
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            tvCollectionTree.SelectedNode = ((TreeSearchResults)SearchResultsTreeBinder[e.RowIndex]).Node;
            tabControl1.SelectTab(tabEdit);
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void tscbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tsLabelSearchField.Visible = tsLabelSearchModifier.Visible = tscbSearchField.Visible = tscbSearchModifier.Visible =
                tscbSearchType.Text == "Rule/Action Parameter Search";
        }

        private void SearchResultsTreeBinder_ListChanged(object sender, ListChangedEventArgs e)
        {

        }

        #region Group Filters and Defaults
        private void btnGroupFilterUpdate_Click(object sender, EventArgs e)
        {
            //check an item is selected
            if (GroupFiltersBinder.Position < 0)
            {
                if (MessageBox.Show("No Item is selected would you like to add the rule to the list instead?", "No rule selected", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    btnGroupFilterAdd_Click(sender, e);
                return;
            }

            dmParameters tmp = pmGroupFilters.GetDataManagerParameters();

            //check value is not the same
            if (tmp.ToString() == ((dmParameters)GroupFiltersBinder.Current).ToString())
                return; //Do nothing

            //check that no other filter conflicts
            /*for (int i = 0; i < GroupFiltersBinder.Count; i++)
            {
                *if Intercept((DataManagerParameters)GroupFiltersBinder[i], tmp)
                    return 
            }
            */

            //all else failing change current value to new value
            GroupFiltersBinder[GroupFiltersBinder.Position] = tmp;
            FileChanged = true;
        }
        private void btnGroupFilterAdd_Click(object sender, EventArgs e)
        {
            dmParameters tmp = pmGroupFilters.GetDataManagerParameters();
            for (int i = 0; i < GroupFiltersBinder.Count; i++)
            {
                if (((dmParameters)GroupFiltersBinder[i]).ToString() == tmp.ToString())
                {
                    MessageBox.Show("Adding this Rule as a filter results in a duplicate and serves no purpose", "Duplicate Filter Detected");
                    return;
                }
            }
            GroupFiltersBinder.Add(tmp);
            FileChanged = true;
        }
        private void btnGroupFilterRemove_Click(object sender, EventArgs e)
        {
            if (GroupFiltersBinder.Position < 0) return;

            GroupFiltersBinder.RemoveCurrent();
            FileChanged = true;
        }
        private void btnGroupFilterClear_Click(object sender, EventArgs e)
        {
            if (GroupFiltersBinder.Count < 1) return;

            GroupFiltersBinder.Clear();
            FileChanged = true;
        }
        private void btnGroupFiltersMoveUp_Click(object sender, EventArgs e)
        {
            //pop item out
            dmRule item = (dmRule)GroupFiltersBinder.Current;
            //get new index from current index
            int index = GroupFiltersBinder.Position - 1;
            //remove item
            GroupFiltersBinder.RemoveCurrent();
            //add back at new index
            GroupFiltersBinder.Insert(index, item);
            //select item again
            GroupFiltersBinder.Position = index;
        }
        private void btnGroupFiltersMoveDown_Click(object sender, EventArgs e)
        {
            //pop item out
            dmRule item = (dmRule)GroupFiltersBinder.Current;
            int index = GroupFiltersBinder.Position + 1;
            GroupFiltersBinder.RemoveCurrent();
            GroupFiltersBinder.Insert(index, item);
            //select item again
            GroupFiltersBinder.Position = index;
        }
        private void btnGroupDefaultUpdate_Click(object sender, EventArgs e)
        {
            //check an item is selected
            if (GroupDefaultsBinder.Position < 0)
            {
                if (MessageBox.Show("No Item is selected would you like to add the rule to the list instead?", "No rule selected", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    btnGroupDefaultAdd_Click(sender, e);
                return;
            }

            dmParameters tmp = pmGroupDefaultActions.GetDataManagerParameters();

            //check value is not the same
            if (tmp.ToString() == ((dmParameters)GroupDefaultsBinder.Current).ToString())
                return; //Do nothing

            //check that no other filter conflicts
            /*for (int i = 0; i < GroupDefaultsBinder.Count; i++)
            {
                *if Intercept((DataManagerParameters)GroupDefaultsBinder[i], tmp)
                    return 
            }
            */

            //all else failing change current value to new value
            GroupDefaultsBinder[GroupDefaultsBinder.Position] = tmp;
            GroupDefaultsBinder_PositionChanged(sender, e);
            FileChanged = true;
        }
        private void btnGroupDefaultAdd_Click(object sender, EventArgs e)
        {
            dmAction tmp = (dmAction)pmGroupDefaultActions.GetDataManagerParameters();
            for (int i = 0; i < GroupDefaultsBinder.Count; i++)
            {
                if (((dmAction)GroupDefaultsBinder[i]).ToString() == tmp.ToString())
                {
                    MessageBox.Show("Adding this action as a default results in a duplicate and serves no purpose", "Duplicate Default Detected");
                    return;
                }
            }
            GroupDefaultsBinder.Add(tmp);
            GroupDefaultsBinder_PositionChanged(sender, e);
            FileChanged = true;
        }
        private void btnGroupDefaultRemove_Click(object sender, EventArgs e)
        {
            if (GroupDefaultsBinder.Position < 0) return;

            GroupDefaultsBinder.RemoveCurrent();
            GroupDefaultsBinder_PositionChanged(sender, e);
            FileChanged = true;
        }
        private void btnGroupDefaultClear_Click(object sender, EventArgs e)
        {
            if (GroupDefaultsBinder.Count < 1) return;

            GroupDefaultsBinder.Clear();
            FileChanged = true;
        }
        private void btnGroupDefaultMoveUp_Click(object sender, EventArgs e)
        {
            //pop item out
            dmAction item = (dmAction)GroupDefaultsBinder.Current;
            //get new index from current index
            int index = GroupDefaultsBinder.Position - 1;
            //remove item
            GroupDefaultsBinder.RemoveCurrent();
            //add back at new index
            GroupDefaultsBinder.Insert(index, item);
            //select item again
            GroupDefaultsBinder.Position = index;
        }
        private void btnGroupDefaultMoveDown_Click(object sender, EventArgs e)
        {
            //pop item out
            dmAction item = (dmAction)GroupDefaultsBinder.Current;
            //get new index from current index
            int index = GroupDefaultsBinder.Position + 1;
            //remove item
            GroupDefaultsBinder.RemoveCurrent();
            //add back at new index
            GroupDefaultsBinder.Insert(index, item);
            //select item again
            GroupDefaultsBinder.Position = index;
        }

        #endregion

        private void saveAsXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog tmp = new SaveFileDialog();
            tmp.Filter = FileFilters;
            tmp.OverwritePrompt = true;
            tmp.InitialDirectory = this.RecovoryFolderPath;
            tmp.AddExtension = true;
            if (tmp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Collection.Save(tmp.FileName);
            }
        }

        private void dgvGroupFilters_MouseUp(object sender, MouseEventArgs e)
        {
            Point tmpPoint = dgvGroupFilters.PointToClient(Cursor.Position);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataGridView.HitTestInfo HitTest = dgvGroupFilters.HitTest(tmpPoint.X, tmpPoint.Y);

                if (HitTest.RowIndex > -1)
                {
                    GroupFiltersBinder.Position = HitTest.RowIndex;
                }
                cmsMenu.Show(dgvGroupFilters, tmpPoint, ToolStripDropDownDirection.Default);
            }
        }

        private void dgvGroupDefaults_MouseUp(object sender, MouseEventArgs e)
        {
            Point tmpPoint = dgvGroupDefaults.PointToClient(Cursor.Position);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataGridView.HitTestInfo HitTest = dgvGroupDefaults.HitTest(tmpPoint.X, tmpPoint.Y);

                if (HitTest.RowIndex > -1)
                {
                    GroupDefaultsBinder.Position = HitTest.RowIndex;
                }
                cmsMenu.Show(dgvGroupDefaults, tmpPoint, ToolStripDropDownDirection.Default);
            }
        }

        private void dataGridView3_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}