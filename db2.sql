-- ====================================================================
--  Asset Verification - Full Initialization Script (Safe to Run Anytime)
--  Creates tables, adds columns, applies defaults, and updates data
-- ====================================================================

/* ============================================================
   1. CREATE TABLE: AVVerification (HEADER) IF NOT EXISTS
   ============================================================ */
IF NOT EXISTS (
    SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[AVVerification]') AND type = N'U'
)
BEGIN
    CREATE TABLE [dbo].[AVVerification] (
        [VerificationID]       INT IDENTITY(1,1) NOT NULL,
        [RefNbr]               NVARCHAR(15)     NOT NULL,
        [Status]               NVARCHAR(10)     NOT NULL,
        [VerificationDate]     DATETIME         NULL,
        [Description]          NVARCHAR(256)    NULL,
        [AssetClassID]         NVARCHAR(10)     NULL,
        [BranchID]             INT              NULL,
        [LineCntr]             INT              NULL,
        [TotalExpectedQty]     DECIMAL(28,6)    NULL,
        [TotalActualQty]       DECIMAL(28,6)    NULL,
        [TotalVarianceQty]     DECIMAL(28,6)    NULL,
        [Building]             NVARCHAR(50)     NULL,
        [Custodian]            NVARCHAR(50)     NULL,
        [Department]           NVARCHAR(50)     NULL,
        [TStamp]               ROWVERSION       NOT NULL,
        [CreatedByID]          UNIQUEIDENTIFIER NULL,
        [CreatedByScreenID]    NCHAR(8)         NULL,
        [CreatedDateTime]      DATETIME         NULL,
        [LastModifiedByID]     UNIQUEIDENTIFIER NULL,
        [LastModifiedByScreenID] NCHAR(8)       NULL,
        [LastModifiedDateTime] DATETIME         NULL,
        [NoteID]               UNIQUEIDENTIFIER NULL,
        CONSTRAINT [PK_AVVerification] 
            PRIMARY KEY CLUSTERED ([VerificationID] ASC)
    );
END;
GO


/* ============================================================
   2. CREATE TABLE: AVVerificationLine (DETAIL) IF NOT EXISTS
   ============================================================ */
IF NOT EXISTS (
    SELECT * FROM sys.objects 
    WHERE object_id = OBJECT_ID(N'[dbo].[AVVerificationLine]') AND type = N'U'
)
BEGIN
    CREATE TABLE [dbo].[AVVerificationLine] (
        [VerificationID]     INT            NOT NULL,
        [LineID]             INT            NOT NULL,
        [AssetID]            INT            NOT NULL,
        [ExpectedQty]        DECIMAL(28,6)  NULL,
        [CountQuantity]      DECIMAL(28,6)  NULL,
        [VarianceQuantity]   DECIMAL(28,6)  NULL,
        [Condition]          NVARCHAR(10)   NULL,
        [MarkedForDisposal]  BIT            NULL,
        [Comments]           NVARCHAR(256)  NULL,
        [BranchID]           INT            NULL,
        [Building]           NVARCHAR(50)   NULL,
        [Floor]              NVARCHAR(20)   NULL,
        [Room]               NVARCHAR(50)   NULL,
        [AssetQuantity]      DECIMAL(28,6)  NULL,
        [TStamp]             ROWVERSION     NOT NULL,
        [CreatedByID]        UNIQUEIDENTIFIER NULL,
        [CreatedByScreenID]  NCHAR(8)         NULL,
        [CreatedDateTime]    DATETIME         NULL,
        [LastModifiedByID]   UNIQUEIDENTIFIER NULL,
        [LastModifiedByScreenID] NCHAR(8)     NULL,
        [LastModifiedDateTime] DATETIME       NULL,
        [NoteID]             UNIQUEIDENTIFIER NULL,
        CONSTRAINT [PK_AVVerificationLine] 
            PRIMARY KEY CLUSTERED ([VerificationID], [LineID])
    );
END;
GO


/* ============================================================
   3. ADD FOREIGN KEY (only if missing)
   ============================================================ */
IF NOT EXISTS (
    SELECT * FROM sys.foreign_keys 
    WHERE name = 'FK_AVVerificationLine_AVVerification'
      AND parent_object_id = OBJECT_ID('[dbo].[AVVerificationLine]')
)
BEGIN
    ALTER TABLE [dbo].[AVVerificationLine]
    ADD CONSTRAINT [FK_AVVerificationLine_AVVerification]
        FOREIGN KEY ([VerificationID])
        REFERENCES [dbo].[AVVerification]([VerificationID])
        ON DELETE CASCADE;
END;
GO


/* ============================================================
   4. ADD MISSING COLUMNS (safe checks)
   ============================================================ */

-- Add Building column
IF COL_LENGTH('dbo.AVVerification', 'Building') IS NULL
BEGIN
    ALTER TABLE dbo.AVVerification ADD [Building] NVARCHAR(50) NULL;
END;

-- Add Custodian column
IF COL_LENGTH('dbo.AVVerification', 'Custodian') IS NULL
BEGIN
    ALTER TABLE dbo.AVVerification ADD [Custodian] NVARCHAR(50) NULL;
END;

-- Add Department column
IF COL_LENGTH('dbo.AVVerification', 'Department') IS NULL
BEGIN
    ALTER TABLE dbo.AVVerification ADD [Department] NVARCHAR(50) NULL;
END;


GO

/* ============================================================
   5. UPDATE EXISTING ROWS (set default decimal values)
   ============================================================ */
UPDATE dbo.AVVerification
SET 
    TotalExpectedQty = ISNULL(TotalExpectedQty, 0.0),
    TotalActualQty   = ISNULL(TotalActualQty, 0.0)
WHERE
    TotalExpectedQty IS NULL OR TotalActualQty IS NULL;
GO


/* ============================================================
   6. ADD DEFAULT CONSTRAINTS (if missing)
   ============================================================ */
-- Default for TotalExpectedQty
IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    JOIN sys.columns c 
      ON dc.parent_object_id = c.object_id
     AND dc.parent_column_id = c.column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.AVVerification')
      AND c.name = 'TotalExpectedQty'
)
BEGIN
    ALTER TABLE dbo.AVVerification
    ADD CONSTRAINT DF_AVVerification_TotalExpectedQty
        DEFAULT (0.0) FOR TotalExpectedQty;
END;

-- Default for TotalActualQty
IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    JOIN sys.columns c 
      ON dc.parent_object_id = c.object_id
     AND dc.parent_column_id = c.column_id
    WHERE dc.parent_object_id = OBJECT_ID('dbo.AVVerification')
      AND c.name = 'TotalActualQty'
)
BEGIN
    ALTER TABLE dbo.AVVerification
    ADD CONSTRAINT DF_AVVerification_TotalActualQty
        DEFAULT (0.0) FOR TotalActualQty;
END;

GO