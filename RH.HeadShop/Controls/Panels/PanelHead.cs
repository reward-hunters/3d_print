using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using RH.HeadEditor.Helpers;
using RH.HeadShop.Helpers;
using RH.HeadShop.IO;
using RH.HeadShop.Render;
using RH.HeadShop.Render.Controllers;

namespace RH.HeadShop.Controls.Panels
{
    public partial class PanelHead : UserControlEx
    {
        #region Var

        public EventHandler OnDelete;
        public EventHandler OnDuplicate;
        public EventHandler OnSave;
        public EventHandler OnUndo;

        public EventHandler OnShapeTool;

        private bool frontTab;

        #endregion

        public PanelHead()
        {
            InitializeComponent();
            frontTab = true;


            if (ProgramCore.PluginMode)
            {
                btnFlipLeft.Visible = false;
                btnFlipRight.Visible = false;
            }

            if (ProgramCore.Project != null)
                ResetButtons();

            ReInitializeControl(frontTab);
        }

        private void ReInitializeControl(bool isFrontTab)
        {
            btnAutodots.Visible = isFrontTab;
         

            //TODO: uncomment if need return  
            //  btnDots.Visible = isFrontTab;

        }


        #region Supported void's

        public void ResetModeTools(bool resetMirror = true)
        {
            if (btnMirror.Tag.ToString() == "1" && resetMirror)
                btnMirror_Click(null, EventArgs.Empty);

            if (btnShapeTool.Tag.ToString() == "1")
                btnShapeTool_Click(null, EventArgs.Empty);

            if (btnLasso.Tag.ToString() == "1")
                btnLasso_Click(null, EventArgs.Empty);

            if (btnAutodots.Tag.ToString() == "1")
                btnAutodots_Click(null, EventArgs.Empty);

            /*     if (btnDots.Tag.ToString() == "1")
                     btnDots_Click(null, EventArgs.Empty);*/

            if (btnPolyLine.Tag.ToString() == "1")
                btnPolyLine_Click(null, EventArgs.Empty);

            if (btnFlipLeft.Tag.ToString() == "1")
                btnFlipLeft_Click(null, EventArgs.Empty);
            if (btnFlipRight.Tag.ToString() == "1")
                btnFlipRight_Click(null, EventArgs.Empty);

            ResetButtons();
        }
        public void ResetButtons()
        {
            if (ProgramCore.Project.AutodotsUsed)
            {
                btnMirror.Enabled = false;
                btnAutodots.Enabled = true;

                btnLasso.Enabled = false;

                btnDots.Enabled = true;
                btnPolyLine.Enabled = true;
                btnShapeTool.Enabled = true;

                btnFlipLeft.Enabled = false;
                btnFlipRight.Enabled = false;

                btnProfile.Enabled = true;
            }
            else
            {
                btnMirror.Enabled = false;
                btnAutodots.Enabled = true;

                btnLasso.Enabled = false;

                btnDots.Enabled = false;
                btnPolyLine.Enabled = false;
                btnShapeTool.Enabled = false;

                btnFlipLeft.Enabled = false;
                btnFlipRight.Enabled = false;

                btnProfile.Enabled = false;
            }
        }

        public void SetPanelLogic()
        {
            switch (ProgramCore.MainForm.ctrlRenderControl.Mode)
            {
                case Mode.HeadShapeFirstTime:
                case Mode.HeadShape:
                    {
                        btnMirror.Enabled = true;
                        btnAutodots.Enabled = false;

                        btnLasso.Enabled = false;

                        btnDots.Enabled = false;
                        btnPolyLine.Enabled = false;
                        btnShapeTool.Enabled = true;

                        btnFlipLeft.Enabled = true;
                        btnFlipRight.Enabled = true;
                    }
                    break;
                case Mode.HeadAutodots:
                case Mode.HeadAutodotsFirstTime:
                    {
                        btnMirror.Enabled = false;
                        btnAutodots.Enabled = true;

                        btnLasso.Enabled = true;

                        btnDots.Enabled = false;
                        btnPolyLine.Enabled = false;
                        btnShapeTool.Enabled = false;

                        btnFlipLeft.Enabled = ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.HeadAutodots;
                        btnFlipRight.Enabled = ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.HeadAutodots;
                    }
                    break;
                case Mode.HeadFlipLeft:
                case Mode.HeadFlipRight:
                    break;
                /*        case Mode.HeadShapedots:
                            btnMirror.Enabled = false;
                            btnAutodots.Enabled = false;

                            btnLasso.Enabled = true;

                            btnDots.Enabled = ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.HeadShapedots;
                            btnPolyLine.Enabled = ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.HeadLine;
                            btnShapeTool.Enabled = false;

                            btnFlipLeft.Enabled = true;
                            btnFlipRight.Enabled = true;
                            break;*/
                case Mode.HeadLine:
                    btnMirror.Enabled = false;
                    btnAutodots.Enabled = false;

                    btnLasso.Enabled = false;

                    btnDots.Enabled = false;
                    btnPolyLine.Enabled = ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.HeadLine;
                    btnShapeTool.Enabled = false;

                    btnFlipLeft.Enabled = true;
                    btnFlipRight.Enabled = true;
                    break;
                case Mode.HeadAutodotsLassoStart:
                case Mode.HeadAutodotsLassoActive:
                //   case Mode.HeadShapedotsLassoStart:
                //   case Mode.HeadShapedotsLassoActive:
                case Mode.LassoStart:
                case Mode.LassoActive:
                    break;
                default:
                    ResetButtons();
                    break;
            }
        }
        public void DisableShape()
        {
            btnShapeTool_Click(null, EventArgs.Empty);
        }

        private void SetDefaultHeadRotation()
        {
            if (ProgramCore.MainForm.HeadFront)         // поворот головы по дефолту для данных режимов
                ProgramCore.MainForm.ctrlRenderControl.OrtoTop();
            else
                ProgramCore.MainForm.ctrlRenderControl.OrtoRight();
        }
        private void UpdateNormals()
        {
            foreach (var part in ProgramCore.MainForm.ctrlRenderControl.headMeshesController.RenderMesh.Parts)
                part.UpdateNormals();
        }

        private void UpdateFlipEnable(FlipType flip)
        {
            switch (flip)
            {
                case FlipType.LeftToRight:
                    EnableFlipLeftToRight();
                    break;
                case FlipType.RightToLeft:
                    EnableFlipRightToLeft();
                    break;
            }
        }
        private void EnableFlipLeftToRight()
        {
            btnFlipLeft.Tag = "1";
            btnFlipRight.Tag = "2";

            btnFlipLeft.Image = Properties.Resources.btnToRightPressed;
            btnFlipRight.Image = Properties.Resources.btnToRightNormal;
        }
        private void EnableFlipRightToLeft()
        {
            btnFlipRight.Tag = "1";
            btnFlipLeft.Tag = "2";

            btnFlipRight.Image = Properties.Resources.btnToLeftPressed;
            btnFlipLeft.Image = Properties.Resources.btnToRightNormal;
        }
        private void DisableFlip()
        {
            btnFlipRight.Tag = "2";
            btnFlipLeft.Tag = "2";

            btnFlipRight.Image = Properties.Resources.btnToLeftNormal;
            btnFlipLeft.Image = Properties.Resources.btnToRightNormal;
        }

        public void UpdateProfileSmoothing(bool isSmoothing)
        {
              lblProfileSmoothing.Visible = trackProfileSmoothing.Visible = isSmoothing;
        }

        #endregion

        #region Form's event

        public void btnMirror_Click(object sender, EventArgs e)
        {
            if (btnMirror.Tag.ToString() == "2")
            {
                btnMirror.Tag = "1";

                btnMirror.BackColor = SystemColors.ControlDarkDark;
                btnMirror.ForeColor = Color.White;

                ProgramCore.MainForm.ctrlRenderControl.ToolMirrored = true;
                ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.HeadShape;
            }
            else
            {
                btnMirror.Tag = "2";

                btnMirror.BackColor = SystemColors.Control;
                btnMirror.ForeColor = Color.Black;

                ProgramCore.MainForm.ctrlRenderControl.ToolMirrored = false;
                ProgramCore.MainForm.ctrlRenderControl.headController.ClearPointsSelection();
            }
            SetPanelLogic();
        }

        private void btnSave_MouseDown(object sender, MouseEventArgs e)
        {
            btnSave.BackColor = SystemColors.ControlDarkDark;
            btnSave.ForeColor = Color.White;
        }
        private void btnSave_MouseUp(object sender, MouseEventArgs e)
        {
            btnSave.BackColor = SystemColors.Control;
            btnSave.ForeColor = Color.Black;

            if (OnSave != null)
                OnSave(this, EventArgs.Empty);
        }

        private void btnDelete_MouseDown(object sender, MouseEventArgs e)
        {
            btnDelete.BackColor = SystemColors.ControlDarkDark;
            btnDelete.ForeColor = Color.White;
        }
        private void btnDelete_MouseUp(object sender, MouseEventArgs e)
        {
            btnDelete.BackColor = SystemColors.Control;
            btnDelete.ForeColor = Color.Black;

            if (OnDelete != null)
                OnDelete(this, EventArgs.Empty);
        }

        private void btnUndo_MouseDown(object sender, MouseEventArgs e)
        {
            btnUndo.BackColor = SystemColors.ControlDarkDark;
            btnUndo.ForeColor = Color.White;
        }
        private void btnUndo_MouseUp(object sender, MouseEventArgs e)
        {
            btnUndo.BackColor = SystemColors.Control;
            btnUndo.ForeColor = Color.Black;

            if (OnUndo != null)
                OnUndo(this, EventArgs.Empty);
        }

        public void btnLasso_Click(object sender, EventArgs e)
        {
            if (btnLasso.Tag.ToString() == "2")
            {
                btnLasso.Tag = "1";

                btnLasso.BackColor = SystemColors.ControlDarkDark;
                btnLasso.ForeColor = Color.White;

                switch (ProgramCore.MainForm.ctrlRenderControl.Mode)
                {
                    case Mode.HeadAutodots:
                    case Mode.HeadAutodotsLassoStart:
                    case Mode.HeadAutodotsLassoActive:
                        break;
                    default:
                        ProgramCore.MainForm.ctrlRenderControl.headController.ClearPointsSelection();
                        break;
                }

                if (ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.HeadAutodots || ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.HeadAutodotsFirstTime)
                    ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.HeadAutodotsLassoStart;
                /*  else if (ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.HeadShapedots)
                      ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.HeadShapedotsLassoStart;*/

                SetPanelLogic();
            }
            else
            {
                btnLasso.Tag = "2";

                btnLasso.BackColor = SystemColors.Control;
                btnLasso.ForeColor = Color.Black;

                if (ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.HeadAutodotsLassoStart || ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.HeadAutodotsLassoActive)
                {
                    ProgramCore.MainForm.ctrlTemplateImage.SelectAutodotsByLasso();
                    ProgramCore.MainForm.ctrlRenderControl.Mode = ProgramCore.Project.AutodotsUsed ? Mode.HeadAutodots : Mode.HeadAutodotsFirstTime;
                }
                /*     else if (ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.HeadShapedotsLassoStart || ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.HeadShapedotsLassoActive)
                     {
                         ProgramCore.MainForm.ctrlTemplateImage.SelectShapedotsByLasso();
                         ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.HeadShapedots;
                     }
                     */
                SetPanelLogic();
            }
        }

        public void btnShapeTool_Click(object sender, EventArgs e)
        {
            if (btnShapeTool.Tag.ToString() == "2")
            {
                btnShapeTool.Tag = "1";
                btnShapeTool.Image = Properties.Resources.btnHandPressed1;

                ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.HeadShapeFirstTime;
                ProgramCore.MainForm.EnableRotating();

                ProgramCore.MainForm.frmFreeHand.cbMirror.Enabled = false;
                ProgramCore.MainForm.ctrlTemplateImage.UpdateUserCenterPositions(false, true);

                UpdateFlipEnable(ProgramCore.Project.ShapeFlip);
                SetPanelLogic();

                if (OnShapeTool != null && sender != null)
                    OnShapeTool(this, EventArgs.Empty);

                if (frontTab && UserConfig.ByName("Options")["Tutorials", "Freehand", "1"] == "1")
                    ProgramCore.MainForm.frmTutFreehand.ShowDialog(this);
            }
            else
            {
                btnShapeTool.Tag = "2";
                btnShapeTool.Image = Properties.Resources.btnHandNormal1;

                btnDots.Enabled = btnPolyLine.Enabled = btnShapeTool.Enabled = false;
                ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.None;
                ProgramCore.MainForm.DisableRotating();
                ProgramCore.MainForm.ctrlRenderControl.HeadShapeController.EndShape();

                ProgramCore.MainForm.frmFreeHand.BeginUpdate();
                ProgramCore.MainForm.frmFreeHand.cbMirror.Enabled = false;
                ProgramCore.MainForm.frmFreeHand.cbMirror.Checked = false;
                ProgramCore.MainForm.frmFreeHand.EndUpdate();

                SetPanelLogic();
                SetDefaultHeadRotation();
                UpdateNormals();

                if (OnShapeTool != null && sender != null)
                    OnShapeTool(this, EventArgs.Empty);
            }
        }

        public void btnAutodots_Click(object sender, EventArgs e)
        {
            if (IsUpdating)
                return;

            BeginUpdate();
            if (btnAutodots.Tag.ToString() == "2")
            {
                btnAutodots.Tag = "1";

                SetDefaultHeadRotation();
                ProgramCore.MainForm.DisableRotating();

                btnAutodots.BackColor = SystemColors.ControlDarkDark;
                btnAutodots.ForeColor = Color.White;

                if (ProgramCore.Project.AutodotsUsed)
                    ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.HeadAutodots;
                else
                {
                    ProgramCore.MainForm.ctrlRenderControl.headMeshesController.InitializeTexturing(ProgramCore.MainForm.ctrlRenderControl.autodotsShapeHelper.GetBaseDots(), HeadController.GetIndices());
                    ProgramCore.MainForm.ctrlRenderControl.autodotsShapeHelper.Transform(ProgramCore.MainForm.ctrlRenderControl.headMeshesController.TexturingInfo.Points.ToArray());
                    ProgramCore.MainForm.ctrlRenderControl.headController.StartAutodots();

                    ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.HeadAutodotsFirstTime;

                }
                ProgramCore.MainForm.ctrlTemplateImage.UpdateUserCenterPositions(false, true);


                btnFlipLeft.Visible = true;
                btnFlipRight.Visible = true;
                UpdateFlipEnable(ProgramCore.Project.TextureFlip);
                SetPanelLogic();

                if (frontTab && UserConfig.ByName("Options")["Tutorials", "Autodots", "1"] == "1")
                    ProgramCore.MainForm.frmTutAutodots.ShowDialog(this);
            }
            else
            {
                btnAutodots.Tag = "2";

                btnAutodots.BackColor = SystemColors.Control;
                btnAutodots.ForeColor = Color.Black;

                if (!ProgramCore.Project.AutodotsUsed)
                {
                    //заменть все шейп-точки на новые
                    //    ProgramCore.MainForm.ctrlRenderControl.headController.UpdateAllShapedotsFromAutodots();

                    for (var i = 0; i < ProgramCore.MainForm.ctrlRenderControl.headMeshesController.RenderMesh.Parts.Count; i++)
                    {
                        var part = ProgramCore.MainForm.ctrlRenderControl.headMeshesController.RenderMesh.Parts[i];
                        if (part.Texture == 0)
                        {
                            part.Texture = ProgramCore.MainForm.ctrlRenderControl.HeadTextureId;
                            part.TextureName = ProgramCore.MainForm.ctrlRenderControl.GetTexturePath(part.Texture);
                        }
                    }

                    ProgramCore.Project.AutodotsUsed = true;
                    btnProfile.Enabled = true;
                    ProgramCore.MainForm.ctrlRenderControl.headController.EndAutodots();
                    ProgramCore.MainForm.ctrlRenderControl.ApplySmoothedTextures();

                    for (var i = 0; i < ProgramCore.MainForm.ctrlRenderControl.headController.AutoDots.Count; i++)      // после слияние с ShapeDots. Проверить!
                    {
                        var p = ProgramCore.MainForm.ctrlRenderControl.headController.AutoDots[i];
                        ProgramCore.MainForm.ctrlRenderControl.autodotsShapeHelper.Transform(p.Value, i); // точка в мировых координатах
                    }
                }

                btnFlipLeft.Visible = false;
                btnFlipRight.Visible = false;
                ProgramCore.MainForm.ctrlRenderControl.CalcReflectedBitmaps();
                ProgramCore.MainForm.ctrlTemplateImage.RectTransformMode = false;
                ProgramCore.MainForm.EnableRotating();
                ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.None;
                SetPanelLogic();
                DisableFlip();
            }
            EndUpdate();
        }
        private void btnDots_Click(object sender, EventArgs e)
        {
            /*        if (btnDots.Tag.ToString() == "2")
                    {
                        btnDots.Tag = "1";
                        btnPolyLine.Tag = btnShapeTool.Tag = "2";

                        btnDots.Image = Properties.Resources.btnDotsPressed;
                        btnPolyLine.Image = Properties.Resources.btnPolyLineNormal;
                        btnShapeTool.Image = Properties.Resources.btnHandNormal1;

                        ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.HeadShapedots;
                        ProgramCore.MainForm.ctrlTemplateImage.UpdateUserCenterPositions(false, true);
                        ProgramCore.MainForm.DisableRotating();

                        UpdateFlipEnable(ProgramCore.Project.ShapeFlip);
                        SetPanelLogic();

                        if (frontTab && UserConfig.ByName("Options")["Tutorials", "Shapedots", "1"] == "1")
                            ProgramCore.MainForm.frmTutShapedots.ShowDialog(this);
                    }
                    else
                    {
                        btnDots.Tag = "2";
                        btnDots.Image = Properties.Resources.btnDotsNormal;
                        UpdateNormals();

                        ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.None;
                        ProgramCore.MainForm.EnableRotating();
                        DisableFlip();
                        SetPanelLogic();
                    }*/
        }

        public readonly Tutorials.frmLineToolTutorial frmTutLineTool = new Tutorials.frmLineToolTutorial();
        public void btnPolyLine_Click(object sender, EventArgs e)
        {
            if (UserConfig.ByName("Options")["Tutorials", "LineTool", "1"] == "1")
                frmTutLineTool.ShowDialog(this);

            if (btnPolyLine.Tag.ToString() == "2")
            {
                if (ProgramCore.MainForm.HeadProfile && ProgramCore.MainForm.ctrlTemplateImage.ControlPointsMode != ProfileControlPointsMode.None)
                {
                    MessageBox.Show("Set Control Points !", "HeadShop", MessageBoxButtons.OK);
                    return; // значит загрузили картинку, но не назначили ей опорные точки. нельзя ниче делатЬ!
                }

                ++ProgramCore.MainForm.ctrlRenderControl.historyController.currentGroup;
                btnPolyLine.Tag = "1";
                btnDots.Tag = btnShapeTool.Tag = "2";

                btnPolyLine.Image = Properties.Resources.btnPolyLinePressed;
                btnDots.Image = Properties.Resources.btnDotsNormal;
                btnShapeTool.Image = Properties.Resources.btnHandNormal1;

                SetDefaultHeadRotation();
                ProgramCore.MainForm.DisableRotating();


                ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.HeadLine;
                ProgramCore.MainForm.ctrlTemplateImage.UpdateUserCenterPositions(false, true);

                UpdateFlipEnable(ProgramCore.Project.ShapeFlip);
                SetPanelLogic();
                if (ProgramCore.MainForm.HeadProfile)
                {
                    ProgramCore.MainForm.ctrlRenderControl.HeadLineMode = MeshPartType.ProfileTop;
                    ProgramCore.MainForm.ctrlTemplateImage.UpdateProfileLocation();
                }
            }
            else
            {
                btnPolyLine.Tag = "2";
                btnPolyLine.Image = Properties.Resources.btnPolyLineNormal;

                var userPoints = ProgramCore.MainForm.ctrlRenderControl.headController.AllPoints.Select(x => x.Value).ToList();
                if (userPoints.Count >= 3)          // если твое условие - просто не выполняем ничего. но интерфейсно все равно отключить надо!
                {
                    #region История (undo)

                    Dictionary<Guid, MeshUndoInfo> undoInfo;
                    ProgramCore.MainForm.ctrlRenderControl.headMeshesController.GetUndoInfo(out undoInfo);
                    var isProfile = ProgramCore.MainForm.HeadProfile;
                    var teInfo = isProfile ? ProgramCore.MainForm.ctrlRenderControl.autodotsShapeHelper.ShapeProfileInfo : ProgramCore.MainForm.ctrlRenderControl.autodotsShapeHelper.ShapeInfo;
                    var historyElem = new HistoryHeadShapeLines(undoInfo, ProgramCore.MainForm.ctrlRenderControl.headController.Lines, teInfo, isProfile);
                    historyElem.Group = ProgramCore.MainForm.ctrlRenderControl.historyController.currentGroup;
                    ProgramCore.MainForm.ctrlRenderControl.historyController.Add(historyElem);

                    #endregion

                    ProgramCore.MainForm.ctrlTemplateImage.FinishLine();
                    switch (ProgramCore.MainForm.ctrlRenderControl.HeadLineMode)
                    {
                        case MeshPartType.LEye:
                        case MeshPartType.REye:
                            userPoints.RemoveAt(userPoints.Count - 1);
                            var center = ProgramCore.MainForm.ctrlRenderControl.HeadLineMode == MeshPartType.LEye ? ProgramCore.Project.LeftEyeUserCenter : ProgramCore.Project.RightEyeCenter;
                            center = MirroredHeadPoint.UpdateWorldPoint(center);

                            ProgramCore.MainForm.ctrlRenderControl.autodotsShapeHelper.Transform(ProgramCore.MainForm.ctrlRenderControl.HeadLineMode, userPoints, center);
                            break;
                        case MeshPartType.Nose: //тут центр не нужен,сказал воваш.. 
                            ProgramCore.MainForm.ctrlRenderControl.autodotsShapeHelper.Transform(ProgramCore.MainForm.ctrlRenderControl.HeadLineMode, userPoints, Vector2.Zero);
                            break;
                        case MeshPartType.Lip: // ТУТ ЦЕНТР???
                            ProgramCore.MainForm.ctrlRenderControl.autodotsShapeHelper.Transform(ProgramCore.MainForm.ctrlRenderControl.HeadLineMode, userPoints, Vector2.Zero);
                            break;
                        case MeshPartType.Head:
                            userPoints.RemoveAt(userPoints.Count - 1);
                            var leftTop = new Vector2(ProgramCore.Project.LeftEyeUserCenter.X, Math.Max(ProgramCore.Project.LeftEyeUserCenter.Y, ProgramCore.Project.RightEyeUserCenter.Y));
                            var rightBottom = new Vector2(ProgramCore.Project.RightEyeUserCenter.X, ProgramCore.Project.MouthUserCenter.Y);

                            var eyesMouthRect = new RectangleF(leftTop.X, leftTop.Y, rightBottom.X - leftTop.X, rightBottom.Y - leftTop.Y);
                            center = new Vector2(eyesMouthRect.X + eyesMouthRect.Width * 0.5f, eyesMouthRect.Y + eyesMouthRect.Height * 0.5f);
                            center = MirroredHeadPoint.UpdateWorldPoint(center);

                            ProgramCore.MainForm.ctrlRenderControl.autodotsShapeHelper.Transform(ProgramCore.MainForm.ctrlRenderControl.HeadLineMode, userPoints, center);
                            break;
                    }
                }

                ProgramCore.MainForm.ctrlRenderControl.headController.Lines.Clear();

                ProgramCore.MainForm.EnableRotating();
                UpdateNormals();

                ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.None;
                ProgramCore.MainForm.ctrlRenderControl.HeadLineMode = MeshPartType.None;

                ProgramCore.MainForm.ctrlTemplateImage.LineSelectionMode = false;
                ProgramCore.MainForm.ctrlTemplateImage.ResetProfileRects();
                ProgramCore.MainForm.ctrlRenderControl.ProfileFaceRect = RectangleF.Empty;

                DisableFlip();
                SetPanelLogic();
            }
        }

        public void btnFlipLeft_Click(object sender, EventArgs e)
        {
            if (btnFlipLeft.Tag.ToString() == "2")
            {
                btnFlipLeft.Tag = "1";
                btnFlipRight.Tag = "2";

                btnFlipLeft.Image = Properties.Resources.btnToRightPressed;
                btnFlipRight.Image = Properties.Resources.btnToLeftNormal;

                switch (ProgramCore.MainForm.ctrlRenderControl.Mode)
                {
                    case Mode.HeadLine:
                        //    case Mode.HeadShapedots:
                        ProgramCore.MainForm.ctrlRenderControl.headMeshesController.Mirror(true, 0);
                        ProgramCore.Project.ShapeFlip = FlipType.LeftToRight;

                        ProgramCore.MainForm.ctrlRenderControl.headController.AutoDots.ClearSelection();
                        ProgramCore.MainForm.ctrlRenderControl.headController.ShapeDots.ClearSelection();
                        ProgramCore.MainForm.ctrlTemplateImage.RectTransformMode = false;
                        break;
                    case Mode.HeadAutodotsFirstTime:
                    case Mode.HeadAutodots:
                        ProgramCore.MainForm.ctrlRenderControl.FlipLeft(true);
                        ProgramCore.MainForm.ctrlRenderControl.headMeshesController.Mirror(true, 0);                // добавлено после слияниея с shapedots!

                        ProgramCore.Project.TextureFlip = FlipType.LeftToRight;

                        ProgramCore.MainForm.ctrlRenderControl.headController.AutoDots.ClearSelection();            // добавлено после слияниея с shapedots!
                        ProgramCore.MainForm.ctrlRenderControl.headController.ShapeDots.ClearSelection();
                        ProgramCore.MainForm.ctrlTemplateImage.RectTransformMode = false;
                        break;
                }

                SetPanelLogic();

                if (frontTab && UserConfig.ByName("Options")["Tutorials", "Mirror", "1"] == "1")
                    ProgramCore.MainForm.frmTutMirror.ShowDialog(this);
            }
            else
            {
                btnFlipLeft.Tag = "2";
                btnFlipLeft.Image = Properties.Resources.btnToRightNormal;

                switch (ProgramCore.MainForm.ctrlRenderControl.Mode)
                {
                    case Mode.HeadLine:
                        //     case Mode.HeadShapedots:
                        ProgramCore.MainForm.ctrlRenderControl.headMeshesController.UndoMirror();
                        ProgramCore.Project.ShapeFlip = FlipType.None;
                        break;
                    case Mode.HeadAutodotsFirstTime:
                    case Mode.HeadAutodots:
                        ProgramCore.MainForm.ctrlRenderControl.headMeshesController.UndoMirror();     // после слияние с ShapeDots. Проверить!
                        ProgramCore.MainForm.ctrlRenderControl.FlipLeft(false);
                        ProgramCore.Project.TextureFlip = FlipType.None;
                        break;
                }
            }
        }
        public void btnFlipRight_Click(object sender, EventArgs e)
        {
            if (btnFlipRight.Tag.ToString() == "2")
            {
                btnFlipRight.Tag = "1";
                btnFlipLeft.Tag = "2";

                btnFlipRight.Image = Properties.Resources.btnToLeftPressed;
                btnFlipLeft.Image = Properties.Resources.btnToRightNormal;

                switch (ProgramCore.MainForm.ctrlRenderControl.Mode)
                {
                    case Mode.HeadLine:
                        //       case Mode.HeadShapedots:
                        ProgramCore.MainForm.ctrlRenderControl.headMeshesController.Mirror(false, 0);
                        ProgramCore.Project.ShapeFlip = FlipType.RightToLeft;

                        ProgramCore.MainForm.ctrlRenderControl.headController.AutoDots.ClearSelection();
                        ProgramCore.MainForm.ctrlRenderControl.headController.ShapeDots.ClearSelection();
                        ProgramCore.MainForm.ctrlTemplateImage.RectTransformMode = false;
                        break;
                    case Mode.HeadAutodotsFirstTime:
                    case Mode.HeadAutodots:
                        ProgramCore.MainForm.ctrlRenderControl.headMeshesController.Mirror(false, 0);       // после слияние с ShapeDots. Проверить!

                        ProgramCore.MainForm.ctrlRenderControl.FlipRight(true);
                        ProgramCore.Project.TextureFlip = FlipType.RightToLeft;

                        ProgramCore.MainForm.ctrlRenderControl.headController.AutoDots.ClearSelection();        // после слияние с ShapeDots. Проверить!
                        ProgramCore.MainForm.ctrlRenderControl.headController.ShapeDots.ClearSelection();
                        ProgramCore.MainForm.ctrlTemplateImage.RectTransformMode = false;
                        break;
                }

                if (frontTab && UserConfig.ByName("Options")["Tutorials", "Mirror", "1"] == "1")
                    ProgramCore.MainForm.frmTutMirror.ShowDialog(this);
            }
            else
            {
                btnFlipRight.Tag = "2";
                btnFlipRight.Image = Properties.Resources.btnToLeftNormal;

                switch (ProgramCore.MainForm.ctrlRenderControl.Mode)
                {
                    case Mode.HeadLine:
                        //    case Mode.HeadShapedots:
                        ProgramCore.MainForm.ctrlRenderControl.headMeshesController.UndoMirror();
                        ProgramCore.Project.ShapeFlip = FlipType.None;
                        break;
                    case Mode.HeadAutodotsFirstTime:
                    case Mode.HeadAutodots:
                        ProgramCore.MainForm.ctrlRenderControl.headMeshesController.UndoMirror();           // после слияние с ShapeDots. Проверить!
                        ProgramCore.MainForm.ctrlRenderControl.FlipRight(false);
                        ProgramCore.Project.TextureFlip = FlipType.None;
                        break;
                }

                SetPanelLogic();
            }
        }

        #endregion

        private void btnProfile_Click(object sender, EventArgs e)
        {
            if (IsUpdating)
                return;

            if (ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.SetCustomControlPoints || ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.SetCustomPoints || ProgramCore.MainForm.ctrlRenderControl.Mode == Mode.SetCustomProfilePoints)
                return;

            BeginUpdate();
            if (btnProfile.Tag.ToString() == "2")
            {
                btnProfile.Tag = "1";
                frontTab = false;

                btnProfile.BackColor = SystemColors.ControlDarkDark;
                btnProfile.ForeColor = Color.White;

                ProgramCore.MainForm.ctrlRenderControl.StagesDeactivate(-1);

                ProgramCore.MainForm.HeadMode = true;
                ProgramCore.MainForm.HeadProfile = true;
                ProgramCore.MainForm.HeadFront = ProgramCore.MainForm.HeadFeature = false;
                ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.None;
                ProgramCore.MainForm.ctrlTemplateImage.btnCopyProfileImg.Visible = true;
                ProgramCore.MainForm.ctrlTemplateImage.btnNewProfilePict.Visible = true;

                ProgramCore.MainForm.ctrlRenderControl.camera.ResetCamera(true);
                ProgramCore.MainForm.ctrlRenderControl.OrtoRight();          // поворачиваем морду как надо

                ProgramCore.MainForm.EnableRotating();

                ProgramCore.MainForm.ctrlRenderControl.autodotsShapeHelper.UpdateProfileLines();
                ProgramCore.MainForm.ctrlTemplateImage.InitializeProfileControlPoints();

                if (ProgramCore.Project.ProfileImage == null) // если нет профиля - просто копируем ебало справа
                {
                    ProgramCore.MainForm.ctrlRenderControl.Render();             // специально два раза, чтобы переинициализировать буфер. хз с чем это связано.
                    ProgramCore.MainForm.ctrlRenderControl.Render();
                    ProgramCore.MainForm.ctrlTemplateImage.btnCopyProfileImg_MouseUp(null, null);
                }
                else
                {
                    ProgramCore.MainForm.ctrlTemplateImage.SetTemplateImage(ProgramCore.Project.ProfileImage);
                    ProgramCore.MainForm.ctrlTemplateImage.RecalcProfilePoints();
                }

                if (ProgramCore.Project.CustomHeadNeedProfileSetup)     // произвольная модель. при первом включении нужно произвести первоначальную настройку.
                    ProgramCore.MainForm.ctrlRenderControl.SetCustomProfileSetup();

                ProgramCore.MainForm.ctrlTemplateImage.UpdateProfileLocation();

                if (UserConfig.ByName("Options")["Tutorials", "Profile", "1"] == "1")
                    ProgramCore.MainForm.frmTutProfile.ShowDialog(this);
            }
            else
            {
                btnProfile.Tag = "2";

                btnProfile.BackColor = SystemColors.Control;
                btnProfile.ForeColor = Color.Black;

                frontTab = true;

                ProgramCore.MainForm.HeadMode = true;
                ProgramCore.MainForm.HeadFront = true;
                ProgramCore.MainForm.HeadProfile = ProgramCore.MainForm.HeadFeature = false;

                if (sender != null)         // иначе это во время загрузки программы. и не надо менять мод!
                    ProgramCore.MainForm.ctrlRenderControl.Mode = Mode.None;

                ProgramCore.MainForm.ctrlTemplateImage.btnCopyProfileImg.Visible = false;
                ProgramCore.MainForm.ctrlTemplateImage.btnNewProfilePict.Visible = false;
                ProgramCore.MainForm.ctrlRenderControl.OrtoTop();            // поворачиваем морду как надо
                ProgramCore.MainForm.EnableRotating();
                ProgramCore.MainForm.ctrlTemplateImage.SetTemplateImage(ProgramCore.Project.FrontImage);       // возвращаем как было, после изменения профиля лица

                if (UserConfig.ByName("Options")["Tutorials", "Autodots", "1"] == "1")
                    ProgramCore.MainForm.frmTutAutodots.ShowDialog(this);
            }

            ReInitializeControl(frontTab);
            EndUpdate();
        }

        private void trackProfileSmoothing_MouseUp(object sender, MouseEventArgs e)
        {
            ProgramCore.MainForm.ctrlTemplateImage.ProfileSmoothing.Smooth(trackProfileSmoothing.Value / (float)(trackProfileSmoothing.Maximum));

        }

        private void trackProfileSmoothing_ValueChanged(object sender, decimal value)
        {

        }
    }
}
