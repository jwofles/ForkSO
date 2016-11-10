﻿using FSO.Client.Controllers;
using FSO.Client.UI.Controls;
using FSO.Client.UI.Framework;
using FSO.Client.UI.Screens;
using FSO.Client.Utils;
using FSO.Common.DataService.Model;
using FSO.Common.Utils;
using FSO.SimAntics.NetPlay.Model.Commands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSO.Client.UI.Panels
{
    public class UIPersonPage : UIContainer
    {
        public UIImage BackgroundContractedImage { get; set; }
        public UIImage BackgroundExpandedImage { get; set; }
        public UIImage BackgroundNameImage { get; set; }
        public UISim SimBox { get; set; }

        /** Auto wired **/
        public UIButton ExpandButton { get; set; }
        public UIButton ExpandedCloseButton { get; set; }
        public UIButton ContractButton { get; set; }
        public UIButton ContractedCloseButton { get; set; }

        public UIButton FriendshipWebButton { get; set; }
        public UILabel NameText { get; set; }

        /** Tabs **/
        public UIButton DescriptionTabButton { get; set; }
        public UIButton AccomplishmentsTabButton { get; set; }
        public UIButton RelationshipsTabButton { get; set; }
        public UIButton OptionsTabButton { get; set; }

        /** Description tab **/
        public UIImage DescriptionTabBackgroundImage { get; set; }
        public UIImage DescriptionTabImage { get; set; }
        public UIImage DescriptionBackgroundReadImage { get; set; }
        public UIImage DescriptionBackgroundWriteImage { get; set; }

        public UITextEdit DescriptionText { get; set; }
        public UISlider DescriptionSlider { get; set; }
        public UIButton DescriptionScrollUpButton { get; set; }
        public UIButton DescriptionScrollDownButton { get; set; }
        public UILabel AgeText { get; set; }

        /**
         * Accomplishments Tab
         */
        public UIButton SkillsSubTabButton { get; set; }
        public UIButton JobsSubTabButton { get; set; }
        public UIImage AccomplishmentsTabBackgroundImage { get; set; }
        public UIImage AccomplishmentsTabImage { get; set; }
        public UIImage AccomplishmentsBackgroundImage { get; set; }

        /** Skills **/

        public UISkillBar MechanicalSkillBar;
        public UISkillBar CookingSkillBar;
        public UISkillBar CharismaSkillBar;
        public UISkillBar LogicSkillBar;
        public UISkillBar BodySkillBar;
        public UISkillBar CreativitySkillBar;

        public UISkillBar[] SkillBars;

        public UILabel LockPointsLabel { get; set; }

        /** Jobs **/
        public UITextEdit JobsText { get; set; }
        public UISlider JobsSlider { get; set; }
        public UIButton JobsScrollUpButton { get; set; }
        public UIButton JobsScrollDownButton { get; set; }
        public UIButton JobsHelpButton { get; set; }

        public UIImage SkillsSubTabBackgroundImage { get; set; }
        public UIImage SkillsSubTabImage { get; set; }
        public UIImage SkillsBackgroundImage { get; set; }

        public UIImage JobsSubTabBackgroundImage { get; set; }
        public UIImage JobsSubTabImage { get; set; }
        public UIImage JobsBackgroundImage { get; set; }
        public UIImage JobsHelpButtonBackgroundImage { get; set; }

        /** Relationships **/
        public UIButton OutgoingSubTabButton { get; set; }
        public UIButton IncomingSubTabButton { get; set; }
        public UILabel DailyRelationship { get; set; }
        public UILabel LifetimeRelationship { get; set; }

        public UIImage RelationshipsTabBackgroundImage { get; set; }
        public UIImage RelationshipsTabImage { get; set; }
        public UIImage RelationshipsBackgroundImage { get; set; }

        public UIImage OutgoingSubTabBackgroundImage { get; set; }
        public UIImage OutgoingSubTabImage { get; set; }

        public UIImage IncomingSubTabBackgroundImage { get; set; }
        public UIImage IncomingSubTabImage { get; set; }

        public UIRelationshipBar STRBar { get; set; }
        public UIRelationshipBar LTRBar { get; set; }


        /** Options **/
        public UIButton AdmitCheckBox { get; set; }
        public UIButton BanCheckBox { get; set; }
        public UIButton InviteButton { get; set; }
        public UIButton KickOutButton { get; set; }
        public UIButton IgnoreButton { get; set; }
        public UIButton MessageButton { get; set; }
        public UIButton FindSimButton { get; set; }
        public UIButton FindHouseButton { get; set; }

        /** Person & Privacy **/
        public Texture2D FindPersonButtonImage { get; set; }
        public Texture2D FindPrivacyOnButtonImage { get; set; }
        public Texture2D FindPrivacyOffButtonImage { get; set; }

        public UIImage OptionsTabBackgroundImage { get; set; }
        public UIImage OptionsTabImage { get; set; }
        public UIImage OptionsBackgroundImage { get; set; }

        public UIImage SelfRimImage { get; set; }
        public UIImage FriendRimImage { get; set; }
        public UIImage EnemyRimImage { get; set; }
        public UIImage NeutralRimImage { get; set; }
        public UIImage OfflineSelfBackgroundImage { get; set; }
        public UIImage OfflineFriendBackgroundImage { get; set; }
        public UIImage OfflineEnemyBackgroundImage { get; set; }
        public UIImage OfflineNeutralBackgroundImage { get; set; }

        /**
         * Skills Progress Bars 
         */

        private bool Open = true;

        /**
         * Model
         */
        public Binding<Avatar> CurrentAvatar { get; internal set; }
        public Binding<Avatar> MyAvatar { get; internal set; }
        public override Vector2 Size { get; set; }

        private UIPersonPageTab _Tab = UIPersonPageTab.Description;
        private UIAccomplishmentsTab _AccomplishmentsTab = UIAccomplishmentsTab.Skills;
        private UIRelationshipsTab _RelationshipsTab = UIRelationshipsTab.Outgoing;
        private string OriginalDescription;
        private string JobAlertText;
        private bool LocalDataChange;

        private int TotalLocks = 20;
        private int UsedLocks = 0;

        private int RelOutSTR = 0;
        private int RelOutLTR = 0;
        private int RelInSTR = 0;
        private int RelInLTR = 0;

        private static byte[] VMSkillMap = new byte[]
        {
            5,
            2,
            1,
            4,
            0,
            3,
        };

        public UIPersonPage()
        {
            BackgroundContractedImage = new UIImage();
            this.AddAt(0, BackgroundContractedImage);
            BackgroundExpandedImage = new UIImage();
            this.AddAt(0, BackgroundExpandedImage);
            BackgroundNameImage = new UIImage();
            this.Add(BackgroundNameImage);

            SelfRimImage = new UIImage();
            Add(SelfRimImage);
            FriendRimImage = new UIImage();
            Add(FriendRimImage);
            EnemyRimImage = new UIImage();
            Add(EnemyRimImage);
            NeutralRimImage = new UIImage();
            Add(NeutralRimImage);
            OfflineSelfBackgroundImage = new UIImage();
            Add(OfflineSelfBackgroundImage);
            OfflineFriendBackgroundImage = new UIImage();
            Add(OfflineFriendBackgroundImage);
            OfflineEnemyBackgroundImage = new UIImage();
            Add(OfflineEnemyBackgroundImage);
            OfflineNeutralBackgroundImage = new UIImage();
            Add(OfflineNeutralBackgroundImage);

            /** Description tab **/
            DescriptionTabBackgroundImage = new UIImage();
            Add(DescriptionTabBackgroundImage);
            DescriptionTabImage = new UIImage();
            Add(this.DescriptionTabImage);
            DescriptionBackgroundReadImage = new UIImage();
            Add(this.DescriptionBackgroundReadImage);
            DescriptionBackgroundWriteImage = new UIImage();
            Add(this.DescriptionBackgroundWriteImage);

            /** Accomplishments tab **/
            AccomplishmentsTabBackgroundImage = new UIImage();
            Add(AccomplishmentsTabBackgroundImage);
            AccomplishmentsTabImage = new UIImage();
            Add(AccomplishmentsTabImage);
            AccomplishmentsBackgroundImage = new UIImage();
            Add(AccomplishmentsBackgroundImage);

            SkillsSubTabBackgroundImage = new UIImage();
            Add(SkillsSubTabBackgroundImage);
            SkillsSubTabImage = new UIImage();
            Add(SkillsSubTabImage);
            SkillsBackgroundImage = new UIImage();
            Add(SkillsBackgroundImage);

            JobsSubTabBackgroundImage = new UIImage();
            Add(JobsSubTabBackgroundImage);
            JobsSubTabImage = new UIImage();
            Add(JobsSubTabImage);
            JobsBackgroundImage = new UIImage();
            Add(JobsBackgroundImage);
            JobsHelpButtonBackgroundImage = new UIImage();
            Add(JobsHelpButtonBackgroundImage);


            RelationshipsTabBackgroundImage = new UIImage();
            Add(RelationshipsTabBackgroundImage);
            RelationshipsTabImage = new UIImage();
            Add(RelationshipsTabImage);
            RelationshipsBackgroundImage = new UIImage();
            Add(RelationshipsBackgroundImage);
            OutgoingSubTabBackgroundImage = new UIImage();
            Add(OutgoingSubTabBackgroundImage);
            OutgoingSubTabImage = new UIImage();
            Add(OutgoingSubTabImage);
            IncomingSubTabBackgroundImage = new UIImage();
            Add(IncomingSubTabBackgroundImage);
            IncomingSubTabImage = new UIImage();
            Add(IncomingSubTabImage);


            /** Options **/
            OptionsTabBackgroundImage = new UIImage();
            Add(OptionsTabBackgroundImage);
            OptionsTabImage = new UIImage();
            Add(OptionsTabImage);
            OptionsBackgroundImage = new UIImage();
            Add(OptionsBackgroundImage);

            var ui = this.RenderScript("personpage.uis");

            MechanicalSkillBar = ui.Create<UISkillBar>("MechanicalSkillBarArea");
            CookingSkillBar = ui.Create<UISkillBar>("CookingSkillBarArea");
            CharismaSkillBar = ui.Create<UISkillBar>("CharismaSkillBarArea");
            LogicSkillBar = ui.Create<UISkillBar>("LogicSkillBarArea");
            BodySkillBar = ui.Create<UISkillBar>("BodySkillBarArea");
            CreativitySkillBar = ui.Create<UISkillBar>("CreativitySkillBarArea");

            SkillBars = new UISkillBar[] {
                MechanicalSkillBar,
                CookingSkillBar,
                CharismaSkillBar,
                LogicSkillBar,
                BodySkillBar,
                CreativitySkillBar,
            };

            for (int i = 0; i < SkillBars.Length; i++)
            {
                var bar = SkillBars[i];
                bar.NumericId = 699 - i;
                Add(SkillBars[i]);
                bar.SkillID = i;
                bar.OnSkillLock += (skillLock) =>
                {
                    var screen = GameFacade.Screens.CurrentUIScreen as CoreGameScreen;

                    if (CurrentAvatar != null && CurrentAvatar.Value != null && FindController<CoreGameScreenController>().IsMe(CurrentAvatar.Value.Avatar_Id))
                    {
                        var skills = CurrentAvatar.Value.Avatar_Skills;
                        var dot = "Avatar_Skills.AvatarSkills_LockLv_" + GameFacade.Strings.GetString("189", (17 + bar.SkillID).ToString());
                        LocalDataChange = true;
                        skills.AvatarSkills_LockLv_Mechanical = (ushort)MechanicalSkillBar.LockLevel;
                        skills.AvatarSkills_LockLv_Cooking = (ushort)CookingSkillBar.LockLevel;
                        skills.AvatarSkills_LockLv_Charisma = (ushort)CharismaSkillBar.LockLevel;
                        skills.AvatarSkills_LockLv_Logic = (ushort)LogicSkillBar.LockLevel;
                        skills.AvatarSkills_LockLv_Body = (ushort)BodySkillBar.LockLevel;
                        skills.AvatarSkills_LockLv_Creativity = (ushort)CreativitySkillBar.LockLevel;
                        LocalDataChange = false;
                        if (screen.vm == null) FindController<PersonPageController>().SaveValue(CurrentAvatar.Value, dot);
                    }

                    if (screen.vm != null)
                    {
                        screen.vm.SendCommand(new VMNetSkillLockCmd()
                        {
                            SkillID = VMSkillMap[bar.SkillID],
                            LockLevel = (byte)bar.LockLevel
                        });
                    }

                    UpdateLockCounts();
                };
            }

            STRBar = ui.Create<UIRelationshipBar>("STRBar");
            LTRBar = ui.Create<UIRelationshipBar>("LTRBar");
            Add(STRBar);
            Add(LTRBar);
            STRBar.NumericId = 1009;
            LTRBar.NumericId = 1010;
            LTRBar.Value = -50;

            SimBox = ui.Create<UISim>("Person3dView");
            SimBox.AutoRotate = true;
            this.Add(SimBox);

            //modify skill page a little to fix its layout for now
            this.ChildrenWithinIdRange(600, 699).ForEach(x => {
                if (x is UILabel)
                {
                    var lbl = ((UILabel)x);
                    lbl.Y -= 5;
                    if (x.NumericId != 606)
                    {
                        lbl.Alignment = TextAlignment.Right;
                    }
                }
                x.X -= 8;
            });

            BackgroundNameImage.With9Slice(20, 20, 0, 0);

            /**
             * Wire up behavior
             */

            /** Scroll bars **/
            this.DescriptionSlider.AttachButtons(DescriptionScrollUpButton, DescriptionScrollDownButton, 1);
            this.DescriptionText.AttachSlider(this.DescriptionSlider);

            /** Tab Buttons **/
            this.DescriptionTabButton.OnButtonClick += new ButtonClickDelegate(TabButton_OnButtonClick);
            this.AccomplishmentsTabButton.OnButtonClick += new ButtonClickDelegate(TabButton_OnButtonClick);
            this.RelationshipsTabButton.OnButtonClick += new ButtonClickDelegate(TabButton_OnButtonClick);
            this.OptionsTabButton.OnButtonClick += new ButtonClickDelegate(TabButton_OnButtonClick);
            this.SkillsSubTabButton.OnButtonClick += new ButtonClickDelegate(AccompSubTabButton_OnButtonClick);
            this.JobsSubTabButton.OnButtonClick += new ButtonClickDelegate(AccompSubTabButton_OnButtonClick);
            this.OutgoingSubTabButton.OnButtonClick += new ButtonClickDelegate(RelationshipsTabButton_OnButtonClick);
            this.IncomingSubTabButton.OnButtonClick += new ButtonClickDelegate(RelationshipsTabButton_OnButtonClick);

            /** Drag **/
            UIUtils.MakeDraggable(BackgroundContractedImage, this, true);
            UIUtils.MakeDraggable(BackgroundExpandedImage, this, true);

            
            /** Open / close **/
            ContractButton.OnButtonClick += (UIElement e) => {
                SetOpen(false);
            };
            ExpandButton.OnButtonClick += (UIElement e) => {
                SetOpen(true);
            };
            MessageButton.OnButtonClick += (UIElement e) =>{
                FindController<CoreGameScreenController>().CallAvatar(CurrentAvatar.Value.Avatar_Id);
            };
            FindHouseButton.OnButtonClick += (UIElement e) =>{
                FindController<CoreGameScreenController>().ShowLotPage(CurrentAvatar.Value.Avatar_LotGridXY);
            };
            FindSimButton.OnButtonClick += FindSimClicked;

            ContractedCloseButton.OnButtonClick += (UIElement e) =>{
                FindController<PersonPageController>().Close();
            };
            ExpandedCloseButton.OnButtonClick += (UIElement e) => {
                FindController<PersonPageController>().Close();
            };

            JobsHelpButton.OnButtonClick += ShowJobInfo;

            /** Default state **/
            CurrentTab = UIPersonPageTab.Description;
            CurrentAccomplishmentsTab = UIAccomplishmentsTab.Skills;
            CurrentRelationshipsTab = UIRelationshipsTab.Outgoing;

            CurrentAvatar = new Binding<Avatar>()
                .WithBinding(this, "HeadOutfitId", "Avatar_Appearance.AvatarAppearance_HeadOutfitID")
                .WithBinding(this, "SimBox.Avatar.BodyOutfitId", "Avatar_Appearance.AvatarAppearance_BodyOutfitID")
                .WithBinding(this, "AvatarName", "Avatar_Name")
                .WithMultiBinding(x =>
                {
                    Redraw();
                }, "Avatar_Name", "Avatar_IsOnline", "Avatar_Description", "Avatar_PrivacyMode");

            MyAvatar = new Binding<Avatar>()
                .WithMultiBinding(x =>
                {
                    RelationshipChange();
                }, "Avatar_FriendshipVec");

            Redraw();
            Size = BackgroundExpandedImage.Size.ToVector2();
        }

        private void FindSimClicked(UIElement button)
        {
            if (FindSimButton.Texture == FindPersonButtonImage)
            {
                FindController<PersonPageController>().FindAvatarLocation();
            }
            else
            {
                byte toggleValue = 0;
                string message = "";
                if (FindSimButton.Texture == FindPrivacyOnButtonImage)
                {
                    toggleValue = 0;
                    message = GameFacade.Strings.GetString("189", "54"); //is enabled, want to disable
                }
                else
                {
                    toggleValue = 1;
                    message = GameFacade.Strings.GetString("189", "55"); //is disabled, want to enable
                }

                UIAlert alert = null;
                alert = UIScreen.GlobalShowAlert(new UIAlertOptions()
                {
                    Title = GameFacade.Strings.GetString("189", "53"),
                    Message = message,
                    Buttons = new UIAlertButton[] {
                        new UIAlertButton(UIAlertButtonType.Yes, (btn) => {
                            if (CurrentAvatar.Value != null) {
                                CurrentAvatar.Value.Avatar_PrivacyMode = toggleValue;
                                FindController<PersonPageController>().SaveValue(CurrentAvatar.Value, "Avatar_PrivacyMode");
                            }
                            UIScreen.RemoveDialog(alert);
                            }),
                        new UIAlertButton(UIAlertButtonType.No, (btn) => UIScreen.RemoveDialog(alert))
                        },
                }, true);
            }
        }

        private void ShowJobInfo(UIElement button)
        {
            UIScreen.GlobalShowAlert(new UIAlertOptions()
            {
                Title = GameFacade.Strings.GetString("189", "64"),
                Message = JobAlertText,
                Buttons = UIAlertButton.Ok(),
            }, true);
        }

        public void TrySaveDescription()
        {
            if (CurrentAvatar != null && CurrentAvatar.Value != null && FindController<CoreGameScreenController>().IsMe(CurrentAvatar.Value.Avatar_Id)
                && DescriptionText.CurrentText != CurrentAvatar.Value.Avatar_Description)
            {
                CurrentAvatar.Value.Avatar_Description = DescriptionText.CurrentText;
                FindController<PersonPageController>().SaveDescription(CurrentAvatar.Value);
            }
        }

        public ulong HeadOutfitId
        {
            set
            {
                //4514010628109
                SimBox.Avatar.HeadOutfitId = value;
            }
            get
            {
                return SimBox.Avatar.HeadOutfitId;
            }
        }

        public string AvatarName
        {
            set
            {
                NameText.Caption = value;
                ResizeNameLabel();
            }
            get
            {
                return NameText.Caption;
            }
        }

        private void ResizeNameLabel()
        {
            var style = NameText.CaptionStyle;
            var width = style.MeasureString(NameText.Caption).X;
            var backgroundWidth = width + 40.0f;
            backgroundWidth = Math.Max(backgroundWidth, 106);

            BackgroundNameImage.SetSize(backgroundWidth, BackgroundNameImage.Height);
            BackgroundNameImage.Position = new Vector2(103.0f - (backgroundWidth / 2.0f), 0.0f);
            //var textX = BackgroundNameImage.X + ((BackgroundNameImage.Width / 2.0f) - (width / 2.0f));

            NameText.Size = new Vector2(BackgroundNameImage.Width, BackgroundNameImage.Height);
            NameText.Position = new Vector2(BackgroundNameImage.Position.X, 0);
        }

        void RelationshipsTabButton_OnButtonClick(UIElement button)
        {
            if (button == this.OutgoingSubTabButton)
            {
                CurrentRelationshipsTab = UIRelationshipsTab.Outgoing;
            }
            else if (button == this.IncomingSubTabButton)
            {
                CurrentRelationshipsTab = UIRelationshipsTab.Incoming;
            }
        }

        void AccompSubTabButton_OnButtonClick(UIElement button)
        {
            if (button == this.SkillsSubTabButton)
            {
                CurrentAccomplishmentsTab = UIAccomplishmentsTab.Skills;
            }
            else if (button == this.JobsSubTabButton)
            {
                CurrentAccomplishmentsTab = UIAccomplishmentsTab.Jobs;
            }
        }

        void TabButton_OnButtonClick(UIElement button)
        {
            if (button == this.DescriptionTabButton)
            {
                CurrentTab = UIPersonPageTab.Description;
            }
            else if (button == this.AccomplishmentsTabButton)
            {
                CurrentTab = UIPersonPageTab.Accomplishments;
            }
            else if (button == this.RelationshipsTabButton)
            {
                CurrentTab = UIPersonPageTab.Relationships;
            }
            else if (button == this.OptionsTabButton)
            {
                CurrentTab = UIPersonPageTab.Options;
            }
            FindController<PersonPageController>().ChangeTopic();
        }

        public UIPersonPageTab CurrentTab
        {
            get
            {
                return _Tab;
            }
            set
            {
                _Tab = value;
                DescriptionTabButton.Selected = _Tab == UIPersonPageTab.Description;
                AccomplishmentsTabButton.Selected = _Tab == UIPersonPageTab.Accomplishments;
                RelationshipsTabButton.Selected = _Tab == UIPersonPageTab.Relationships;
                OptionsTabButton.Selected = _Tab == UIPersonPageTab.Options;
                Redraw();
            }
        }

        public UIAccomplishmentsTab CurrentAccomplishmentsTab
        {
            get
            {
                return _AccomplishmentsTab;
            }
            set
            {
                _AccomplishmentsTab = value;
                SkillsSubTabButton.Selected = _AccomplishmentsTab == UIAccomplishmentsTab.Skills;
                JobsSubTabButton.Selected = _AccomplishmentsTab == UIAccomplishmentsTab.Jobs;
                Redraw();
            }
        }

        public UIRelationshipsTab CurrentRelationshipsTab
        {
            get
            {
                return _RelationshipsTab;
            }
            set
            {
                _RelationshipsTab = value;
                OutgoingSubTabButton.Selected = _RelationshipsTab == UIRelationshipsTab.Outgoing;
                IncomingSubTabButton.Selected = _RelationshipsTab == UIRelationshipsTab.Incoming;
                Redraw();
            }
        }


        private int bodyID = 0;

        public void SetOpen(bool open)
        {
            this.Open = open;
            Redraw();
            FindController<PersonPageController>()?.ForceRefreshData(_Tab);
        }

        private void PopulateJobsText(Avatar ava)
        {
            HashSet<JobLevel> jobs = null;
            JobLevel currentJob = null;
            if (ava.Avatar_JobLevelVec != null)
            {
                jobs = new HashSet<JobLevel>(ava.Avatar_JobLevelVec);
                currentJob = jobs.Where(x => x.JobLevel_JobType == ava.Avatar_CurrentJob).FirstOrDefault();
            }
            StringBuilder outText = new StringBuilder();
            outText.Append(GameFacade.Strings.GetString("189", "60")+"\r\n"); //current title
            if (ava.Avatar_CurrentJob == 0 || jobs == null || currentJob == null)
            {
                outText.Append(GameFacade.Strings.GetString("189", "61") + "\r\n\r\n"); //unemployed
                JobAlertText = GameFacade.Strings.GetString("189", "66");
            }
            else
            {
                jobs.Remove(currentJob);
                var title = GameFacade.Strings.GetString("272", (((currentJob.JobLevel_JobType - 1) * 11) + currentJob.JobLevel_JobGrade + 1).ToString());
                outText.Append(title);
                outText.Append("\r\n\r\n");
                if (jobs.Count > 0)
                {
                    //remaining other jobs
                    outText.Append(GameFacade.Strings.GetString("189", "62") + "\r\n"); //other titles
                    foreach (var job in jobs)
                    {
                        outText.Append(GameFacade.Strings.GetString("272", (((job.JobLevel_JobType - 1) * 11) + job.JobLevel_JobGrade + 1).ToString()));
                        outText.Append("\r\n");
                    }
                }
                int poolTime = currentJob.JobLevel_JobType;
                poolTime = (poolTime > 2) ? (poolTime - 1) : poolTime;
                JobAlertText = GameFacade.Strings.GetString("189", "65", new string[] {
                    GameFacade.Strings.GetString("189", (67+currentJob.JobLevel_JobType).ToString()),
                    title,
                    GameFacade.Strings.GetString("189", (73+poolTime*2).ToString()),
                    GameFacade.Strings.GetString("189", (74+poolTime*2).ToString()),
                    (currentJob.JobLevel_JobGrade == 10) ? GameFacade.Strings.GetString("189", "79") :
                    GameFacade.Strings.GetString("272", (((currentJob.JobLevel_JobType - 1) * 11) + currentJob.JobLevel_JobGrade + 2).ToString())
                });
            }
            JobsText.CurrentText = outText.ToString();
            JobsText.SetSize(JobsText.Width, 160);
        }

        private void UpdateLockCounts()
        {
            UsedLocks = 0;
            foreach (var bar in SkillBars) UsedLocks += bar.LockLevel;
            LockPointsLabel.Caption = GameFacade.Strings.GetString("189", "49", new string[] { UsedLocks.ToString(), TotalLocks.ToString() });
            foreach (var bar in SkillBars) bar.FreeLocks = TotalLocks - UsedLocks;
        }

        private void Redraw()
        {
            if (LocalDataChange) return;
            NameText.Y = 5;
            var isOpen = Open == true;
            var isClosed = Open == false;
            var isOnline = false;
            var isMe = false;
            var hasProperty = false;
            var privacyOn = false;

            if (CurrentAvatar != null && CurrentAvatar.Value != null)
            {
                isOnline = CurrentAvatar.Value.Avatar_IsOnline;
                isMe = FindController<CoreGameScreenController>().IsMe(CurrentAvatar.Value.Avatar_Id);
                hasProperty = CurrentAvatar.Value.Avatar_LotGridXY != 0;

                if (OriginalDescription != CurrentAvatar.Value.Avatar_Description)
                {
                    OriginalDescription = CurrentAvatar.Value.Avatar_Description;
                    DescriptionText.CurrentText = OriginalDescription;
                }
                
                if (CurrentAvatar.Value.Avatar_Skills != null)
                {
                    var skills = CurrentAvatar.Value.Avatar_Skills;
                    MechanicalSkillBar.SkillLevel = skills.AvatarSkills_Mechanical;
                    CookingSkillBar.SkillLevel = skills.AvatarSkills_Cooking;
                    CharismaSkillBar.SkillLevel = skills.AvatarSkills_Charisma;
                    LogicSkillBar.SkillLevel = skills.AvatarSkills_Logic;
                    BodySkillBar.SkillLevel = skills.AvatarSkills_Body;
                    CreativitySkillBar.SkillLevel = skills.AvatarSkills_Creativity;

                    MechanicalSkillBar.LockLevel = skills.AvatarSkills_LockLv_Mechanical;
                    CookingSkillBar.LockLevel = skills.AvatarSkills_LockLv_Cooking;
                    CharismaSkillBar.LockLevel = skills.AvatarSkills_LockLv_Charisma;
                    LogicSkillBar.LockLevel = skills.AvatarSkills_LockLv_Logic;
                    BodySkillBar.LockLevel = skills.AvatarSkills_LockLv_Body;
                    CreativitySkillBar.LockLevel = skills.AvatarSkills_LockLv_Creativity;
                }
                TotalLocks = CurrentAvatar.Value.Avatar_SkillsLockPoints;
                privacyOn = CurrentAvatar.Value.Avatar_PrivacyMode > 0;
                UpdateLockCounts();
                PopulateJobsText(CurrentAvatar.Value);
            }
            else PopulateJobsText(new Avatar());

            RelationshipChange();

            foreach (var bar in SkillBars) bar.DisableLock = !isMe;

            var isFriend = RelOutLTR >= 50;
            var isEnemy = RelOutLTR <= -50;
            var isNeutral = !(isFriend || isEnemy);

            SelfRimImage.Visible = isOnline && isMe;
            OfflineSelfBackgroundImage.Visible = !isOnline && isMe;
            if (!isMe) { 
                FindSimButton.Texture = FindPersonButtonImage;
                FindSimButton.Tooltip = GameFacade.Strings.GetString("189", "8");
            } else
            {
                FindSimButton.Texture = (privacyOn) ? FindPrivacyOnButtonImage : FindPrivacyOffButtonImage;
                FindSimButton.Tooltip = GameFacade.Strings.GetString("189", (privacyOn) ? "57":"56");
            }
            FriendRimImage.Visible = isOnline && isFriend;
            EnemyRimImage.Visible = isOnline && isEnemy;
            NeutralRimImage.Visible = isOnline && isNeutral;
            OfflineFriendBackgroundImage.Visible = !isOnline && isFriend;
            OfflineEnemyBackgroundImage.Visible = !isOnline && isEnemy;
            OfflineNeutralBackgroundImage.Visible = !isOnline && isNeutral;

            MessageButton.Disabled = isMe || !isOnline;

            BackgroundContractedImage.Visible = isClosed;
            BackgroundExpandedImage.Visible = isOpen;
            ContractButton.Visible = isOpen;
            ContractedCloseButton.Visible = isClosed;
            ExpandButton.Visible = isClosed;
            ExpandedCloseButton.Visible = isOpen;

            var isDesc = _Tab == UIPersonPageTab.Description;
            var isAccomp = _Tab == UIPersonPageTab.Accomplishments;
            var isSkills = isAccomp && (_AccomplishmentsTab == UIAccomplishmentsTab.Skills);
            var isJobs = isAccomp && (_AccomplishmentsTab == UIAccomplishmentsTab.Jobs);
            var isRelationships = _Tab == UIPersonPageTab.Relationships;
            var isOutgoing = _RelationshipsTab == UIRelationshipsTab.Outgoing;
            var isIncoming = _RelationshipsTab == UIRelationshipsTab.Incoming;
            var isOptions = _Tab == UIPersonPageTab.Options;

            FindHouseButton.Disabled = !hasProperty;

            /** Tab Images **/
            this.DescriptionTabButton.Visible = isOpen;
            this.DescriptionTabBackgroundImage.Visible = isOpen && !isDesc;
            this.DescriptionTabImage.Visible = isOpen && isDesc;
            this.DescriptionBackgroundReadImage.Visible = isOpen && isDesc && !isMe;
            this.DescriptionBackgroundWriteImage.Visible = isOpen && isDesc && isMe;
            this.DescriptionText.Mode = (isMe) ? UITextEditMode.Editor : UITextEditMode.ReadOnly;

            this.AccomplishmentsTabButton.Visible = isOpen;
            this.AccomplishmentsTabBackgroundImage.Visible = isOpen && !isAccomp;
            this.AccomplishmentsTabImage.Visible = isOpen && isAccomp;
            this.AccomplishmentsBackgroundImage.Visible = isOpen && isAccomp;
            this.SkillsSubTabBackgroundImage.Visible = isOpen && isAccomp && !isSkills;
            this.SkillsSubTabImage.Visible = isOpen && isAccomp && isSkills;
            this.SkillsBackgroundImage.Visible = isOpen && isAccomp && isSkills;
            this.JobsSubTabBackgroundImage.Visible = isOpen && isAccomp && !isJobs;
            this.JobsSubTabImage.Visible = isOpen && isAccomp && isJobs;
            this.JobsBackgroundImage.Visible = isOpen && isAccomp && isJobs;
            this.JobsHelpButtonBackgroundImage.Visible = false;

            RelationshipsTabButton.Visible = isOpen;
            RelationshipsTabBackgroundImage.Visible = isOpen && !isRelationships;
            RelationshipsTabImage.Visible = isOpen && isRelationships;
            RelationshipsBackgroundImage.Visible = isOpen && isRelationships;
            OutgoingSubTabBackgroundImage.Visible = isOpen && isRelationships && !isOutgoing;
            OutgoingSubTabImage.Visible = isOpen && isRelationships && isOutgoing;
            IncomingSubTabBackgroundImage.Visible = isOpen && isRelationships && !isIncoming;
            IncomingSubTabImage.Visible = isOpen && isRelationships && isIncoming;

            OptionsTabButton.Visible = isOpen;
            OptionsTabBackgroundImage.Visible = isOpen && !isOptions;
            OptionsTabImage.Visible = isOpen && isOptions;
            OptionsBackgroundImage.Visible = isOpen && isOptions;

            RelationshipsTabButton.Disabled = isMe;
            OptionsTabButton.Disabled = isMe;

            if (isClosed)
            {
                this.ChildrenWithinIdRange(400, 1299).ForEach(x => x.Visible = false);
                return;
            }

            /** Description tab **/
            this.ChildrenWithinIdRange(400, 499).ForEach(x => x.Visible = isDesc);


            /** Accomplishments **/
            this.ChildrenWithinIdRange(500, 599).ForEach(x => x.Visible = isAccomp);
            this.ChildrenWithinIdRange(600, 699).ForEach(x => x.Visible = isSkills);
            this.ChildrenWithinIdRange(900, 999).ForEach(x => x.Visible = isJobs);

            /** Relationships **/
            this.ChildrenWithinIdRange(1000, 1099).ForEach(x => x.Visible = isRelationships);

            /** Options **/
            this.ChildrenWithinIdRange(700, 799).ForEach(x => x.Visible = isOptions);
        }

        public void RelationshipChange()
        {
            RelOutSTR = 0;
            RelOutLTR = 0;
            RelInSTR = 0;
            RelInLTR = 0;
            if (MyAvatar != null && MyAvatar.Value != null && MyAvatar.Value.Avatar_FriendshipVec != null && CurrentAvatar.Value != null)
            {
                var rels = new List<Relationship>(MyAvatar.Value.Avatar_FriendshipVec);
                foreach (var rel in rels)
                {
                    if (rel.Relationship_TargetID == CurrentAvatar.Value.Avatar_Id)
                    {
                        if (rel.Relationship_IsOutgoing)
                        {
                            RelOutSTR = rel.Relationship_STR;
                            RelOutLTR = rel.Relationship_LTR;
                        } else
                        {
                            RelInSTR = rel.Relationship_STR;
                            RelInLTR = rel.Relationship_LTR;
                        }
                    }
                }
            }

            var isOutgoing = _RelationshipsTab == UIRelationshipsTab.Outgoing;
            var str = isOutgoing ? RelOutSTR : RelInSTR;
            var ltr = isOutgoing ? RelOutLTR : RelInLTR;

            STRBar.Value = str;
            LTRBar.Value = ltr;
            DailyRelationship.Caption = str.ToString();
            LifetimeRelationship.Caption = ltr.ToString();
        }
    }

    public enum UIPersonPageTab
    {
        Description,
        Accomplishments,
        Relationships,
        Options
    }

    public enum UIAccomplishmentsTab
    {
        Skills,
        Jobs
    }

    public enum UIRelationshipsTab
    {
        Outgoing,
        Incoming
    }
}
