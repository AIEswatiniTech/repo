using System;
using PX.Data;
using PX.Objects.FA;
using PX.Objects.CS;

namespace PPSAAssetValidation
{
    [Serializable]
    [PXCacheName("Asset Verification")]
    [PXPrimaryGraph(typeof(AVVerificationEntry))]
    public class AVVerification : PX.Data.PXBqlTable, IBqlTable
    {
        #region VerificationID
        [PXDBIdentity(IsKey = true)]
        public virtual int? VerificationID { get; set; }
        public abstract class verificationID : PX.Data.BQL.BqlInt.Field<verificationID> { }
        #endregion

        #region RefNbr
        [PXDBString(15, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [PXUIField(DisplayName = "Ref. Nbr.", Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<AVVerification.refNbr>),
            SubstituteKey = typeof(AVVerification.refNbr),
            DescriptionField = typeof(AVVerification.description))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion

        #region Status
        [PXDBString(20, IsFixed = true)]
        [AVVerificationStatus.List]
        [PXUIField(DisplayName = "Status", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
        public virtual string Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region VerificationDate
        [PXDBDate]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "Date", Visibility = PXUIVisibility.SelectorVisible)]
        public virtual DateTime? VerificationDate { get; set; }
        public abstract class verificationDate : PX.Data.BQL.BqlDateTime.Field<verificationDate> { }
        #endregion

        #region Description
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Description")]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region BranchID (Filter)
        [PXInt]
        [PXUIField(DisplayName = "Branch")]
        [PXSelector(typeof(Search<PX.Objects.GL.Branch.branchID>),
            SubstituteKey = typeof(PX.Objects.GL.Branch.branchCD),
            DescriptionField = typeof(PX.Objects.GL.Branch.acctName))]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region AssetClassID (Filter)
        [PXInt]
        [PXUIField(DisplayName = "Asset Class")]
        [PXSelector(typeof(Search<FAClass.classID>),
            SubstituteKey = typeof(FAClass.assetCD),
            DescriptionField = typeof(FAClass.description))]
        public virtual int? AssetClassID { get; set; }
        public abstract class assetClassID : PX.Data.BQL.BqlInt.Field<assetClassID> { }
        #endregion

        #region Building (Filter)
        // Building is stored on FALocationHistory; there is no FABuilding DAC in base product.
        // We store building as int and expose a selector from FALocationHistory.buildingID values.
        [PXInt]
        [PXUIField(DisplayName = "Building")]
        [PXSelector(typeof(Search<FALocationHistory.buildingID>),
            SubstituteKey = typeof(FALocationHistory.buildingID))]
        public virtual int? Building { get; set; }
        public abstract class building : PX.Data.BQL.BqlInt.Field<building> { }
        #endregion

        #region Custodian (Filter)
        [PXInt]
        [PXUIField(DisplayName = "Custodian")]
        [PXSelector(typeof(Search<PX.Objects.EP.EPEmployee.bAccountID>),
            SubstituteKey = typeof(PX.Objects.EP.EPEmployee.acctCD),
            DescriptionField = typeof(PX.Objects.EP.EPEmployee.acctName))]
        public virtual int? Custodian { get; set; }
        public abstract class custodian : PX.Data.BQL.BqlInt.Field<custodian> { }
        #endregion

        #region Department (Filter)
        [PXString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Department")]
        [PXSelector(typeof(Search<PX.Objects.EP.EPDepartment.departmentID>))]
        public virtual string Department { get; set; }
        public abstract class department : PX.Data.BQL.BqlString.Field<department> { }
        #endregion

        #region AssetStatusFilter (Filter)
        [PXString(1, IsFixed = true)]
        [PXUIField(DisplayName = "Asset Status")]
        [PXDefault("", PersistingCheck = PXPersistingCheck.Nothing)]
        [FixedAssetStatus.List]
        public virtual string AssetStatusFilter { get; set; }
        public abstract class assetStatusFilter : PX.Data.BQL.BqlString.Field<assetStatusFilter> { }
        #endregion

        #region LineCntr
        [PXDBInt]
        [PXDefault(0)]
        public virtual int? LineCntr { get; set; }
        public abstract class lineCntr : PX.Data.BQL.BqlInt.Field<lineCntr> { }
        #endregion

        #region TotalExpectedQty
        [PXDecimal(2)]
        [PXUIField(DisplayName = "Total Expected Qty", Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? TotalExpectedQty { get; set; }
        public abstract class totalExpectedQty : PX.Data.BQL.BqlDecimal.Field<totalExpectedQty> { }
        #endregion

        #region TotalActualQty
        [PXDecimal(2)]
        [PXUIField(DisplayName = "Total Actual Qty", Enabled = false)]
        [PXDefault(TypeCode.Decimal, "0.0", PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? TotalActualQty { get; set; }
        public abstract class totalActualQty : PX.Data.BQL.BqlDecimal.Field<totalActualQty> { }
        #endregion

        #region TotalVarianceQty
        [PXDecimal(2)]
        [PXUIField(DisplayName = "Total Variance Qty", Enabled = false)]
        public virtual decimal? TotalVarianceQty { get; set; }
        public abstract class totalVarianceQty : PX.Data.BQL.BqlDecimal.Field<totalVarianceQty> { }
        #endregion

        #region NoteID
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
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
        #endregion
    }
}
