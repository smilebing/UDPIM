using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CSharpWin
{
    public partial class TraFransfersFileControl : UserControl
    {
        #region Fields

        private Image _image;
        private string _comment;
        private string _fileName;
        private long _fileSize;
        private long _traFransfersSize;
        private bool _isSend = true;
        private DateTime _startTime;
        private DateTime _lastTime = DateTime.Now;

        #endregion

        #region Event

        public event LabelClickEventHandler LabelClick;

        #endregion

        #region Constructors

        public TraFransfersFileControl()
        {
            InitializeComponent();
            InitText();
            InitEvents();
            InitData();
        }

        #endregion

        #region Public Method

        public void SetTag(int flag)
        {
            labelCancel.Tag = flag;
        }

        #endregion

        #region Properties

        [Browsable(false)]
        public Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                if (_image != null)
                {
                    picIcon.Image = _image;
                }
            }
        }

        [Browsable(false)]
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    comment.Text = _comment;
                }
            }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        [Browsable(false)]
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    fileName.Text = _fileName;
                    toolTip.SetToolTip(fileName, _fileName);
                }
            }
        }

        [Browsable(false)]
        public long FileSize
        {
            get { return _fileSize; }
            set
            {
                if (_fileSize != value)
                {
                    _fileSize = value;
                    trafransferInfo.Text = GetText(_fileSize);
                }
            }
        }

        [Browsable(false)]
        public long TraFransfersSize
        {
            get { return _traFransfersSize; }
            set
            {
                if (_traFransfersSize != value)
                {
                    _traFransfersSize = value;
                    float size = _traFransfersSize / (_fileSize * 1.0f);
                    if (_fileSize != 0)
                    {
                        int barValue = (int)(size * 100);
                        if (barValue > 100)
                            barValue = 100;
                        progressBar.Value = barValue;
                    }
                    TimeSpan ts = DateTime.Now - _lastTime;
                    if (ts.TotalSeconds > 0.3)
                    {
                        trafransferInfo.Text = string.Format("{0}/{1}",
                            GetText(_traFransfersSize), GetText(_fileSize));
                        labelSpeed.Text = GetSpeedText();
                        toolTip.SetToolTip(progressBar, size.ToString("0.0%"));
                        _lastTime = DateTime.Now;
                    }
                }
            }
        }

        [Browsable(false)]
        [DefaultValue(true)]
        public bool IsSend
        {
            get { return _isSend; }
            set
            {
                _isSend = value;
                if (_isSend)
                {
                    labelSave.Hide();
                    labelReceive.Hide();
                }
                else
                {
                    labelSave.Show();
                    labelReceive.Show();
                }

                InitText();
            }
        }

        #endregion

        #region Init

        private void InitData()
        {
            labelCancel.Tag = 0;
            labelSave.Tag = 1;
            labelReceive.Tag = 2;
        }

        private void InitText()
        {
            if (_isSend)
            {
                labelCancel.Text = "取消";
            }
            else
            {
                labelCancel.Text = "拒绝";
            }

            labelSave.Text = "另存为...";
            labelReceive.Text = "接收";
        }

        private void InitEvents()
        {
            labelReceive.MouseEnter += new EventHandler(OnLabelMouseEnter);
            labelReceive.MouseLeave += new EventHandler(OnLabelMouseLeave);
            labelReceive.MouseClick += new MouseEventHandler(OnLabelClick);

            labelSave.MouseEnter += new EventHandler(OnLabelMouseEnter);
            labelSave.MouseLeave += new EventHandler(OnLabelMouseLeave);
            labelSave.MouseClick += new MouseEventHandler(OnLabelClick);

            labelCancel.MouseEnter += new EventHandler(OnLabelMouseEnter);
            labelCancel.MouseLeave += new EventHandler(OnLabelMouseLeave);
            labelCancel.MouseClick += new MouseEventHandler(OnLabelClick);
        }

        #endregion

        #region Event Methods

        void OnLabelClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Label label = sender as Label;
                LabelClickEventArgs ce = new LabelClickEventArgs((int)label.Tag);
                OnLabelClick(ce);
            }
        }

        void OnLabelMouseLeave(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if (label != null)
                label.ForeColor = Color.Blue;
        }

        void OnLabelMouseEnter(object sender, EventArgs e)
        {
            Label label = sender as Label;
            if (label != null)
                label.ForeColor = Color.Red;
        }

        #endregion

        #region Protected Methods

        protected virtual void OnLabelClick(LabelClickEventArgs e)
        {
            if (LabelClick != null)
                LabelClick(this, e);
        }

        #endregion

        #region Help Methods

        private string GetText(double size)
        {
            if (size < 1024)
                return string.Format("{0} B", size.ToString());
            else if (size < 1024 * 1024)
                return string.Format("{0} KB", (size / 1024.0f).ToString("0.0"));
            else
                return string.Format("{0} MB", (size / (1024.0f * 1024.0f)).ToString("0.0"));
        }

        private string GetSpeedText()
        {
            TimeSpan span = DateTime.Now - _startTime;
            double speed = _traFransfersSize / span.TotalSeconds;
            return string.Format("{0}/s", GetText(speed));
        }

        #endregion
    }
}
