using System;
using PX.Data;
using PX.Data.EP;
using PX.Objects.EP;

namespace PPSAAssetValidation
{
    [Serializable]
    [PXCacheName("Asset Life Reassessment Approval Setup")]
    public class AVLifeReassessmentApprovalSetup : PX.Data.IBqlTable
    {
        #region SetupID
        [PXDBIdentity(IsKey = true)]
        public virtual int? SetupID { get; set; }
        public abstract class setupID : PX.Data.BQL.BqlInt.Field<setupID> { }
        #endregion

        #region ApprovalWorkgroupID
        [PXDBInt]
        [PXUIField(DisplayName = "Approval Workgroup")]
        [PXSelector(typeof(Search<EPEmployee.workgroupID>))]
        [PXDefault]
        public virtual int? ApprovalWorkgroupID { get; set; }
        public abstract class approvalWorkgroupID : PX.Data.BQL.BqlInt.Field<approvalWorkgroupID> { }
        #endregion

        #region AssignmentMapID
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Assignment Map")]
        [PXSelector(typeof(Search<EPAssignmentMap.assignmentMapID>))]
        public virtual string AssignmentMapID { get; set; }
        public abstract class assignmentMapID : PX.Data.BQL.BqlString.Field<assignmentMapID> { }
        #endregion

        #region IsActive
        [PXDBBool]
        [PXUIField(DisplayName = "Active")]
        [PXDefault(true)]
        public virtual bool? IsActive { get; set; }
        public abstract class isActive : PX.Data.BQL.BqlBool.Field<isActive> { }
        #endregion

        #region Description
        [PXDBString(256, IsUnicode = true)]
        [PXUIField(DisplayName = "Description")]
        public virtual string Description { get; set; }
        public abstract class description : PX.Data.BQL.BqlString.Field<description> { }
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
