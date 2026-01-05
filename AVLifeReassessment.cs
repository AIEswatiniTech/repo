using System;
using PX.Data;
using PX.Objects.CS;

namespace PPSAAssetValidation
{
    [Serializable]
    [PXCacheName("Asset Life Reassessment")]
    [PXPrimaryGraph(typeof(AVLifeReassessmentEntry))]
    public class AVLifeReassessment : PX.Data.PXBqlTable, IBqlTable
    {
        #region ReassessmentID
        [PXDBIdentity(IsKey = true)]
        public virtual int? ReassessmentID { get; set; }
        public abstract class reassessmentID : PX.Data.BQL.BqlInt.Field<reassessmentID> { }
        #endregion

        #region RefNbr
        [PXDBString(15, IsUnicode = true, InputMask = ">CCCCCCCCCCCCCCC")]
        [PXDefault]
        [PXUIField(DisplayName = "Reference Nbr.", Visibility = PXUIVisibility.SelectorVisible)]
        [PXSelector(typeof(Search<AVLifeReassessment.refNbr>),
            typeof(AVLifeReassessment.refNbr),
            typeof(AVLifeReassessment.status),
            typeof(AVLifeReassessment.reassessmentDate),
            typeof(AVLifeReassessment.description))]
        [AutoNumber(typeof(AVLifeReassessment.reassessmentDate), typeof(AVLifeReassessment.refNbr))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion

        #region Status
        [PXDBString(10, IsFixed = true)]
        [PXDefault(AVLifeReassessmentStatus.OnHold)]
        [AVLifeReassessmentStatus.List]
        [PXUIField(DisplayName = "Status", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
        public virtual string Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region ReassessmentDate
        [PXDBDate]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "Reassessment Date", Visibility = PXUIVisibility.SelectorVisible)]
        public virtual DateTime? ReassessmentDate { get; set; }
        public abstract class reassessmentDate : PX.Data.BQL.BqlDateTime.Field<reassessmentDate> { }
        #endregion

        #region Description
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Description")]
        [PXDefault(PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
        #endregion

        #region BranchID
        [PXDBInt]
        [PXDefault(typeof(AccessInfo.branchID))]
        [PXUIField(DisplayName = "Branch")]
        [PXSelector(typeof(PX.Objects.GL.Branch.branchID),
            SubstituteKey = typeof(PX.Objects.GL.Branch.branchCD),
            DescriptionField = typeof(PX.Objects.GL.Branch.acctName))]
        public virtual int? BranchID { get; set; }
        public abstract class branchID : PX.Data.BQL.BqlInt.Field<branchID> { }
        #endregion

        #region LineCntr
        [PXDBInt]
        [PXDefault(0)]
        public virtual int? LineCntr { get; set; }
        public abstract class lineCntr : PX.Data.BQL.BqlInt.Field<lineCntr> { }
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