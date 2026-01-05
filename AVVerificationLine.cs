using System;
using PX.Data;
using PX.Objects.FA;
using PX.Objects.CS;
using FixedAsset = PX.Objects.FA.FixedAsset;
using FixedAssetStatus = PX.Objects.FA.FixedAssetStatus;

namespace PPSAAssetValidation
{
    [Serializable]
    [PXCacheName("Asset Verification Line")]
    public class AVVerificationLine : PX.Data.PXBqlTable, PX.Data.IBqlTable
    {
        #region VerificationID (FK to header)
        [PXDBInt(IsKey = true)]
        [PXDBDefault(typeof(AVVerification.verificationID))]
        [PXParent(typeof(Select<AVVerification, Where<AVVerification.verificationID, Equal<Current<AVVerificationLine.verificationID>>>>))]
        public virtual int? VerificationID { get; set; }
        public abstract class verificationID : PX.Data.BQL.BqlInt.Field<verificationID> { }
        #endregion

        #region LineID
        [PXDBInt(IsKey = true)]
        [PXLineNbr(typeof(AVVerification.lineCntr))]
        [PXUIField(DisplayName = "Line Nbr.", Enabled = false)]
        public virtual int? LineID { get; set; }
        public abstract class lineID : PX.Data.BQL.BqlInt.Field<lineID> { }
        #endregion

        #region Status
        [PXString(20, IsFixed = true)]
        [PXDefault(FixedAssetStatus.Active)]
        [FixedAssetStatus.List]
        [PXUIField(DisplayName = "Status", Enabled = false)]
        public virtual string Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region AssetID
        [PXDBInt]
        [PXUIField(DisplayName = "Asset ID", Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<FixedAsset.assetID>),
            SubstituteKey = typeof(FixedAsset.assetCD),
            DescriptionField = typeof(FixedAsset.description))]
        [PXDefault]
        public virtual int? AssetID { get; set; }
        public abstract class assetID : PX.Data.BQL.BqlInt.Field<assetID> { }
        #endregion

        #region AssetStatus
        [PXString(1, IsFixed = true)]
        [PXUIField(DisplayName = "Asset Status", Enabled = false)]
        [FixedAssetStatus.List]
        public virtual string AssetStatus { get; set; }
        public abstract class assetStatus : PX.Data.BQL.BqlString.Field<assetStatus> { }
        #endregion

        #region AssetDescription
        [PXString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Asset Description", Enabled = false)]
        public virtual string AssetDescription { get; set; }
        public abstract class assetDescription : PX.Data.BQL.BqlString.Field<assetDescription> { }
        #endregion

        #region BranchID
        [PXInt]
        [PXUIField(DisplayName = "Branch", Enabled = false)]
        [PXSelector(typeof(PX.Objects.GL.Branch.branchID),
            SubstituteKey = typeof(PX.Objects.GL.Branch.branchCD),
            DescriptionField = typeof(PX.Objects.GL.Branch.acctName))]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region Building
        [PXInt]
        [PXUIField(DisplayName = "Building", Enabled = false)]
        public virtual int? Building { get; set; }
        public abstract class building : PX.Data.BQL.BqlInt.Field<building> { }
        #endregion

        #region Floor
        [PXString(5, IsUnicode = true)]
        [PXUIField(DisplayName = "Floor", Enabled = false)]
        public virtual string Floor { get; set; }
        public abstract class floor : PX.Data.BQL.BqlString.Field<floor> { }
        #endregion

        #region Room
        [PXString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "Room", Enabled = false)]
        public virtual string Room { get; set; }
        public abstract class room : PX.Data.BQL.BqlString.Field<room> { }
        #endregion

        #region Custodian
        [PXInt]
        [PXUIField(DisplayName = "Custodian", Enabled = false)]
        [PXSelector(typeof(Search<PX.Objects.EP.EPEmployee.bAccountID>),
            SubstituteKey = typeof(PX.Objects.EP.EPEmployee.acctCD),
            DescriptionField = typeof(PX.Objects.EP.EPEmployee.acctName))]
        public virtual int? Custodian { get; set; }
        public abstract class custodian : PX.Data.BQL.BqlInt.Field<custodian> { }
        #endregion

        #region Department
        [PXString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Department", Enabled = false)]
        [PXSelector(typeof(Search<PX.Objects.EP.EPDepartment.departmentID>),
            DescriptionField = typeof(PX.Objects.EP.EPDepartment.description))]
        public virtual string Department { get; set; }
        public abstract class department : PX.Data.BQL.BqlString.Field<department> { }
        #endregion

        #region AssetClass
        [PXInt]
        [PXUIField(DisplayName = "Asset Class", Enabled = false)]
        [PXSelector(typeof(Search<FAClass.classID>),
            SubstituteKey = typeof(FAClass.assetCD),
            DescriptionField = typeof(FAClass.description))]
        public virtual int? AssetClass { get; set; }
        public abstract class assetClass : PX.Data.BQL.BqlInt.Field<assetClass> { }
        #endregion

        #region Asset
        [PXString(60, IsUnicode = true)]
        [PXUIField(DisplayName = "Asset", Enabled = false)]
        public virtual string Asset { get; set; }
        public abstract class asset : PX.Data.BQL.BqlString.Field<asset> { }
        #endregion

        #region AssetQuantity
        [PXDecimal(2)]
        [PXUIField(DisplayName = "Asset Quantity", Enabled = false)]
        [PXDefault(TypeCode.Decimal, "1.0")]
        public virtual decimal? AssetQuantity { get; set; }
        public abstract class assetQuantity : PX.Data.BQL.BqlDecimal.Field<assetQuantity> { }
        #endregion

        #region ExpectedQty
        [PXDecimal(2)]
        [PXUIField(DisplayName = "Expected Quantity", Enabled = false)]
        [PXDefault(TypeCode.Decimal, "1.0")]
        public virtual decimal? ExpectedQty { get; set; }
        public abstract class expectedQty : PX.Data.BQL.BqlDecimal.Field<expectedQty> { }
        #endregion

        #region CountQuantity
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = "Count Quantity")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        public virtual decimal? CountQuantity { get; set; }
        public abstract class countQuantity : PX.Data.BQL.BqlDecimal.Field<countQuantity> { }
        #endregion

        #region VarianceQuantity
        [PXDBDecimal(2)]
        [PXUIField(DisplayName = "Variance Quantity", Enabled = false)]
        [PXFormula(typeof(Sub<AVVerificationLine.expectedQty, AVVerificationLine.countQuantity>))]
        [PXDefault(TypeCode.Decimal, "0.0")]
        public virtual decimal? VarianceQuantity { get; set; }
        public abstract class varianceQuantity : PX.Data.BQL.BqlDecimal.Field<varianceQuantity> { }
        #endregion

        #region Condition
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Condition")]
        [AVAssetCondition.List]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string Condition { get; set; }
        public abstract class condition : PX.Data.BQL.BqlString.Field<condition> { }
        #endregion

        #region MarkedForDisposal
        [PXDBBool]
        [PXUIField(DisplayName = "Marked For Disposal")]
        [PXDefault(false)]
        public virtual bool? MarkedForDisposal { get; set; }
        public abstract class markedForDisposal : PX.Data.BQL.BqlBool.Field<markedForDisposal> { }
        #endregion

        #region Comments
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Comments")]
        public virtual string Comments { get; set; }
        public abstract class comments : PX.Data.BQL.BqlString.Field<comments> { }
        #endregion

        #region TStamp
        [PXDBTimestamp]
        public virtual byte[] TStamp { get; set; }
        public abstract class tStamp : PX.Data.BQL.BqlByteArray.Field<tStamp> { }
        #endregion

        #region CreatedByID
        [PXDBCreatedByID]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion

        #region CreatedByScreenID
        [PXDBCreatedByScreenID]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion

        #region CreatedDateTime
        [PXDBCreatedDateTime]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion

        #region LastModifiedByID
        [PXDBLastModifiedByID]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion

        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion

        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion

        #region NoteID
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
    }
}
