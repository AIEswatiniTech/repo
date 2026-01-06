using System;
using PX.Data;

namespace PPSAAssetValidation
{
    [Serializable]
    [PXCacheName("Asset Life Reassessment Setup")]
    public class AVLifeReassessmentSetup : IBqlTable
    {
        #region SetupID
        [PXDBInt(IsKey = true)]
        [PXDefault(1)]
        public virtual int? SetupID { get; set; }
        public abstract class setupID : PX.Data.BQL.BqlInt.Field<setupID> { }
        #endregion

        #region ApprovalsEnabled
        [PXDBBool]
        [PXUIField(DisplayName = "Approvals Enabled")]
        [PXDefault(false)]
        public virtual bool? ApprovalsEnabled { get; set; }
        public abstract class approvalsEnabled : PX.Data.BQL.BqlBool.Field<approvalsEnabled> { }
        #endregion

        #region AssignmentMapID
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Assignment Map")]
        [PXSelector(typeof(Search<EPAssignmentMap.assignmentMapID>))]
        public virtual string AssignmentMapID { get; set; }
        public abstract class assignmentMapID : PX.Data.BQL.BqlString.Field<assignmentMapID> { }
        #endregion

        #region NumberingSequenceID
        [PXDBInt]
        [PXUIField(DisplayName = "Numbering Sequence")]
        [PXSelector(typeof(Search<PX.Objects.CS.Numbering.numberingID>),
            SubstituteKey = typeof(PX.Objects.CS.Numbering.numberingCD))]
        public virtual int? NumberingSequenceID { get; set; }
        public abstract class numberingSequenceID : PX.Data.BQL.BqlInt.Field<numberingSequenceID> { }
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
    }
}
