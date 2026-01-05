using System;
using PX.Data;
using PX.Data.EP;
using PX.Objects.FA;
using PX.Objects.EP;

namespace PPSAAssetValidation
{
    [Serializable]
    [PXCacheName("Asset Life Reassessment Line")]
    public class AVLifeReassessmentLine : PX.Data.PXBqlTable, IBqlTable, IAssign
    {
        #region ReassessmentID
        [PXDBInt(IsKey = true)]
        [PXDBDefault(typeof(AVLifeReassessment.reassessmentID))]
        [PXParent(typeof(Select<AVLifeReassessment,
            Where<AVLifeReassessment.reassessmentID, Equal<Current<AVLifeReassessmentLine.reassessmentID>>>>))]
        public virtual int? ReassessmentID { get; set; }
        public abstract class reassessmentID : PX.Data.BQL.BqlInt.Field<reassessmentID> { }
        #endregion

        #region LineID
        [PXDBInt(IsKey = true)]
        [PXLineNbr(typeof(AVLifeReassessment.lineCntr))]
        [PXUIField(DisplayName = "Line Nbr.", Enabled = false)]
        public virtual int? LineID { get; set; }
        public abstract class lineID : PX.Data.BQL.BqlInt.Field<lineID> { }
        #endregion

        #region AssetID
        [PXDBInt]
        [PXUIField(DisplayName = "Asset ID", Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<FixedAsset.assetID,
            Where<FixedAsset.status, NotEqual<FixedAssetStatus.disposed>,
                And<FixedAsset.depreciable, Equal<True>>>>),
            SubstituteKey = typeof(FixedAsset.assetCD),
            DescriptionField = typeof(FixedAsset.description))]
        [PXDefault]
        public virtual int? AssetID { get; set; }
        public abstract class assetID : PX.Data.BQL.BqlInt.Field<assetID> { }
        #endregion

        #region AssetCD
        [PXString(15, IsUnicode = true)]
        [PXUIField(DisplayName = "Asset CD", Enabled = false)]
        public virtual string AssetCD { get; set; }
        public abstract class assetCD : PX.Data.BQL.BqlString.Field<assetCD> { }
        #endregion

        #region Description
        [PXString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Description", Enabled = false)]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region ClassID
        [PXString(30, IsUnicode = true)]
        [PXUIField(DisplayName = "Asset Class", Enabled = false)]
        public virtual string ClassID { get; set; }
        public abstract class classID : PX.Data.BQL.BqlString.Field<classID> { }
        #endregion

        #region ReceiptDate
        [PXDate]
        [PXUIField(DisplayName = "Receipt Date", Enabled = false)]
        public virtual DateTime? ReceiptDate { get; set; }
        public abstract class receiptDate : PX.Data.BQL.BqlDateTime.Field<receiptDate> { }
        #endregion

        #region DepreciateFromDate
        [PXDate]
        [PXUIField(DisplayName = "Place-in-Service Date", Enabled = false)]
        public virtual DateTime? DepreciateFromDate { get; set; }
        public abstract class depreciateFromDate : PX.Data.BQL.BqlDateTime.Field<depreciateFromDate> { }
        #endregion

        #region OrigAcquisitionCost
        [PXDecimal(4)]
        [PXUIField(DisplayName = "Orig. Acquisition Cost", Enabled = false)]
        public virtual decimal? OrigAcquisitionCost { get; set; }
        public abstract class origAcquisitionCost : PX.Data.BQL.BqlDecimal.Field<origAcquisitionCost> { }
        #endregion

        #region Department
        [PXString(50, IsUnicode = true)]
        [PXUIField(DisplayName = "Department", Enabled = false)]
        public virtual string Department { get; set; }
        public abstract class department : PX.Data.BQL.BqlString.Field<department> { }
        #endregion

        #region Status
        [PXString(1, IsFixed = true)]
        [PXUIField(DisplayName = "Status", Enabled = false)]
        [FixedAssetStatus.List]
        public virtual string Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region OrigUsefulLife
        [PXDBDecimal(4)]
        [PXUIField(DisplayName = "Useful Life, Years", Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0")]
        public virtual decimal? OrigUsefulLife { get; set; }
        public abstract class origUsefulLife : PX.Data.BQL.BqlDecimal.Field<origUsefulLife> { }
        #endregion

        #region RemainingLife
        [PXDecimal(4)]
        [PXUIField(DisplayName = "Remaining Asset Life", Enabled = false)]
        public virtual decimal? RemainingLife { get; set; }
        public abstract class remainingLife : PX.Data.BQL.BqlDecimal.Field<remainingLife> { }
        #endregion

        #region LifeAdjustmentYears
        [PXDBDecimal(4)]
        [PXUIField(DisplayName = "Life Adjustment, Years")]
        [PXDefault(TypeCode.Decimal, "0.0")]
        public virtual decimal? LifeAdjustmentYears { get; set; }
        public abstract class lifeAdjustmentYears : PX.Data.BQL.BqlDecimal.Field<lifeAdjustmentYears> { }
        #endregion

        #region NewUsefulLife
        [PXDBDecimal(4)]
        [PXUIField(DisplayName = "New Useful Life")]
        public virtual decimal? NewUsefulLife { get; set; }
        public abstract class newUsefulLife : PX.Data.BQL.BqlDecimal.Field<newUsefulLife> { }
        #endregion

        #region Comments
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Comments")]
        public virtual string Comments { get; set; }
        public abstract class comments : PX.Data.BQL.BqlString.Field<comments> { }
        #endregion

        #region Condition
        [PXString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Condition", Enabled = false)]
        [AVAssetCondition.ListAttribute]
        public virtual string Condition { get; set; }
        public abstract class condition : PX.Data.BQL.BqlString.Field<condition> { }
        #endregion

        #region Audit Fields
        [PXDBTimestamp]
        public virtual byte[] TStamp { get; set; }
        public abstract class tStamp : PX.Data.BQL.BqlByteArray.Field<tStamp> { }

        [PXDBCreatedByID]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }

        [PXDBCreatedByScreenID]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }

        [PXDBCreatedDateTime]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }

        [PXDBLastModifiedByID]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }

        [PXDBLastModifiedByScreenID]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }

        [PXDBLastModifiedDateTime]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }

        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion

        #region Hold
        [PXDBBool]
        [PXUIField(DisplayName = "Hold")]
        [PXDefault(false)]
        public virtual bool? Hold { get; set; }
        public abstract class hold : PX.Data.BQL.BqlBool.Field<hold> { }
        #endregion

        #region Approved
        [PXDBBool]
        [PXUIField(DisplayName = "Approved", Enabled = false)]
        [PXDefault(false)]
        public virtual bool? Approved { get; set; }
        public abstract class approved : PX.Data.BQL.BqlBool.Field<approved> { }
        #endregion

        #region Rejected
        [PXDBBool]
        [PXUIField(DisplayName = "Rejected", Enabled = false)]
        [PXDefault(false)]
        public virtual bool? Rejected { get; set; }
        public abstract class rejected : PX.Data.BQL.BqlBool.Field<rejected> { }
        #endregion

        #region OwnerID
        [PXDBInt]
        [PXUIField(DisplayName = "Owner")]
        public virtual int? OwnerID { get; set; }
        public abstract class ownerID : PX.Data.BQL.BqlInt.Field<ownerID> { }
        #endregion

        #region WorkgroupID
        [PXDBInt]
        [PXUIField(DisplayName = "Workgroup")]
        [PXSelector(typeof(Search<EPEmployee.workgroupID>))]
        public virtual int? WorkgroupID { get; set; }
        public abstract class workgroupID : PX.Data.BQL.BqlInt.Field<workgroupID> { }
        #endregion
              
    }
}