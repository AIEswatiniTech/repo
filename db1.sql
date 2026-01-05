-- =============================================
-- Asset Life Reassessment Tables
-- Creates tables for tracking useful life adjustments with approval workflow
-- =============================================

-- Create Asset Life Reassessment Header Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AVLifeReassessment]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AVLifeReassessment](
        [ReassessmentID] [int] IDENTITY(1,1) NOT NULL,
        [RefNbr] [nvarchar](15) NOT NULL,
        [Status] [nvarchar](10) NOT NULL,
        [ReassessmentDate] [datetime] NOT NULL,
        [Description] [nvarchar](256) NULL,
        [BranchID] [int] NULL,
        [LineCntr] [int] NULL DEFAULT 0,
        [NoteID] [uniqueidentifier] NULL,
        [TStamp] [timestamp] NOT NULL,
        [CreatedByID] [uniqueidentifier] NULL,
        [CreatedByScreenID] [nvarchar](8) NULL,
        [CreatedDateTime] [datetime] NULL,
        [LastModifiedByID] [uniqueidentifier] NULL,
        [LastModifiedByScreenID] [nvarchar](8) NULL,
        [LastModifiedDateTime] [datetime] NULL,
        CONSTRAINT [PK_AVLifeReassessment] PRIMARY KEY CLUSTERED ([ReassessmentID] ASC)
    )
END
GO

-- Create unique index on RefNbr
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AVLifeReassessment_RefNbr' AND object_id = OBJECT_ID('AVLifeReassessment'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_AVLifeReassessment_RefNbr] 
    ON [dbo].[AVLifeReassessment]([RefNbr] ASC)
END
GO

-- Create Asset Life Reassessment Line Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AVLifeReassessmentLine]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AVLifeReassessmentLine](
        [ReassessmentID] [int] NOT NULL,
        [LineID] [int] NOT NULL,
        [AssetID] [int] NOT NULL,
        [AssetCD] [nvarchar](15) NULL,
        [Description] [nvarchar](256) NULL,
        [ClassID] [nvarchar](30) NULL,
        [ReceiptDate] [datetime] NULL,
        [DepreciateFromDate] [datetime] NULL,
        [OrigAcquisitionCost] [decimal](19, 4) NULL,
        [Department] [nvarchar](50) NULL,
        [Status] [nvarchar](1) NULL,
        [Condition] [nvarchar](10) NULL,
        [OrigUsefulLife] [decimal](8, 4) NULL,
        [RemainingLife] [decimal](8, 4) NULL,
        [LifeAdjustmentYears] [decimal](8, 4) NULL,
        [NewUsefulLife] [decimal](8, 4) NULL,
        [Comments] [nvarchar](256) NULL,
        [Hold] [bit] NULL DEFAULT 1,
        [Approved] [bit] NULL DEFAULT 0,
        [Rejected] [bit] NULL DEFAULT 0,
        [OwnerID] [uniqueidentifier] NULL,
        [WorkgroupID] [int] NULL,
        [TStamp] [timestamp] NOT NULL,
        [CreatedByID] [uniqueidentifier] NULL,
        [CreatedByScreenID] [nvarchar](8) NULL,
        [CreatedDateTime] [datetime] NULL,
        [LastModifiedByID] [uniqueidentifier] NULL,
        [LastModifiedByScreenID] [nvarchar](8) NULL,
        [LastModifiedDateTime] [datetime] NULL,
        [NoteID] [uniqueidentifier] NULL,
        CONSTRAINT [PK_AVLifeReassessmentLine] PRIMARY KEY CLUSTERED 
        (
            [ReassessmentID] ASC,
            [LineID] ASC
        )
    )
END
GO

-- Add foreign key constraint
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AVLifeReassessmentLine_AVLifeReassessment')
BEGIN
    ALTER TABLE [dbo].[AVLifeReassessmentLine]  WITH CHECK ADD  
    CONSTRAINT [FK_AVLifeReassessmentLine_AVLifeReassessment] 
    FOREIGN KEY([ReassessmentID])
    REFERENCES [dbo].[AVLifeReassessment] ([ReassessmentID])
    ON DELETE CASCADE
END
GO

-- Add missing approval columns to existing table (safe check)
IF COL_LENGTH('dbo.AVLifeReassessmentLine', 'Hold') IS NULL
BEGIN
    ALTER TABLE dbo.AVLifeReassessmentLine ADD [Hold] [bit] NULL DEFAULT 1;
END
GO

IF COL_LENGTH('dbo.AVLifeReassessmentLine', 'Approved') IS NULL
BEGIN
    ALTER TABLE dbo.AVLifeReassessmentLine ADD [Approved] [bit] NULL DEFAULT 0;
END
GO

IF COL_LENGTH('dbo.AVLifeReassessmentLine', 'Rejected') IS NULL
BEGIN
    ALTER TABLE dbo.AVLifeReassessmentLine ADD [Rejected] [bit] NULL DEFAULT 0;
END
GO

IF COL_LENGTH('dbo.AVLifeReassessmentLine', 'OwnerID') IS NULL
BEGIN
    ALTER TABLE dbo.AVLifeReassessmentLine ADD [OwnerID] [uniqueidentifier] NULL;
END
GO

IF COL_LENGTH('dbo.AVLifeReassessmentLine', 'WorkgroupID') IS NULL
BEGIN
    ALTER TABLE dbo.AVLifeReassessmentLine ADD [WorkgroupID] [int] NULL;
END
GO

IF COL_LENGTH('dbo.AVLifeReassessmentLine', 'Condition') IS NULL
BEGIN
    ALTER TABLE dbo.AVLifeReassessmentLine ADD [Condition] [nvarchar](10) NULL;
END
GO

-- Create Asset Life Reassessment Approval Setup Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AVLifeReassessmentApprovalSetup]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AVLifeReassessmentApprovalSetup](
        [SetupID] [int] IDENTITY(1,1) NOT NULL,
        [ApprovalWorkgroupID] [int] NULL,
        [AssignmentMapID] [nvarchar](10) NULL,
        [IsActive] [bit] NULL DEFAULT 1,
        [Description] [nvarchar](256) NULL,
        [TStamp] [timestamp] NOT NULL,
        [CreatedByID] [uniqueidentifier] NULL,
        [CreatedByScreenID] [nvarchar](8) NULL,
        [CreatedDateTime] [datetime] NULL,
        [LastModifiedByID] [uniqueidentifier] NULL,
        [LastModifiedByScreenID] [nvarchar](8) NULL,
        [LastModifiedDateTime] [datetime] NULL,
        CONSTRAINT [PK_AVLifeReassessmentApprovalSetup] PRIMARY KEY CLUSTERED ([SetupID] ASC)
    )
END
GO

PRINT 'Asset Life Reassessment tables created successfully with approval workflow columns'
GO