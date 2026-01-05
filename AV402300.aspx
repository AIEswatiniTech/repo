<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true"  
    ValidateRequest="false" CodeFile="AV402300.aspx.cs" Inherits="Page_AV402300" Title="Asset Life Reassessment" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>
<%@ Register Assembly="PX.Web.UI" Namespace="PX.Web.UI" TagPrefix="px" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="server">
    <px:PXDataSource ID="ds" runat="server" PrimaryView="Document" TypeName="PPSAAssetValidation.AVLifeReassessmentEntry" Visible="True">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand Name="Save" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="True" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
            <px:PXDSCallbackCommand Name="SubmitForApproval" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="Approve" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="Reject" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="Release" CommitChanges="True" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>

<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Document" Width="100%" 
        Caption="Asset Life Reassessment" NoteIndicator="True" FilesIndicator="True" 
        ActivityIndicator="True" ActivityField="NoteActivity" DefaultControlID="edStatus">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M" />
            <px:PXDropDown ID="edStatus" runat="server" DataField="Status" />
            <px:PXDateTimeEdit ID="edReassessmentDate" runat="server" DataField="ReassessmentDate" />
            
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" />
            <px:PXSelector ID="edBranchID" runat="server" DataField="BranchID" />
        </Template>
    </px:PXFormView>
</asp:Content>

<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="server">
    <px:PXGrid ID="grid" runat="server" DataSourceID="ds" SkinID="Details" Width="100%" 
        Height="400px" Caption="Assets" CaptionVisible="True" 
        SyncPosition="True" KeepPosition="True">
        <Levels>
            <px:PXGridLevel DataMember="Lines">
                <RowTemplate>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M" />
                    <px:PXNumberEdit ID="edLineID" runat="server" DataField="LineID" />
                    <px:PXSelector ID="edAssetID" runat="server" DataField="AssetID" AutoRefresh="True" CommitChanges="True" />
                    <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" />
                    <px:PXTextEdit ID="edClassID" runat="server" DataField="ClassID" />
                    <px:PXDateTimeEdit ID="edReceiptDate" runat="server" DataField="ReceiptDate" />
                    <px:PXDateTimeEdit ID="edDepreciateFromDate" runat="server" DataField="DepreciateFromDate" />
                    <px:PXNumberEdit ID="edOrigAcquisitionCost" runat="server" DataField="OrigAcquisitionCost" />
                    <px:PXTextEdit ID="edDepartment" runat="server" DataField="Department" />
                    <px:PXDropDown ID="edStatus" runat="server" DataField="Status" />
                    <px:PXDropDown ID="edCondition" runat="server" DataField="Condition" />
                    <px:PXNumberEdit ID="edOrigUsefulLife" runat="server" DataField="OrigUsefulLife" />
                    <px:PXNumberEdit ID="edRemainingLife" runat="server" DataField="RemainingLife" />
                    <px:PXNumberEdit ID="edNewUsefulLife" runat="server" DataField="NewUsefulLife" CommitChanges="True" />
                    <px:PXCheckBox ID="chkHold" runat="server" DataField="Hold" CommitChanges="True" />
                    <px:PXCheckBox ID="chkApproved" runat="server" DataField="Approved" />
                    <px:PXCheckBox ID="chkRejected" runat="server" DataField="Rejected" />
                </RowTemplate>
                <Columns>
                    <px:PXGridColumn DataField="LineID" Width="60" TextAlign="Right" />
                    <px:PXGridColumn DataField="AssetID" Width="130" DisplayMode="Hint" CommitChanges="True" />
                    <px:PXGridColumn DataField="Description" Width="200" />
                    <px:PXGridColumn DataField="ClassID" Width="100" />
                    <px:PXGridColumn DataField="ReceiptDate" Width="90" />
                    <px:PXGridColumn DataField="DepreciateFromDate" Width="90" />
                    <px:PXGridColumn DataField="OrigAcquisitionCost" Width="120" TextAlign="Right" />
                    <px:PXGridColumn DataField="Department" Width="120" />
                    <px:PXGridColumn DataField="Status" Width="80" Type="DropDownList" />
                    <px:PXGridColumn DataField="Condition" Width="100" Type="DropDownList" />
                    <px:PXGridColumn DataField="OrigUsefulLife" Width="100" TextAlign="Right" />
                    <px:PXGridColumn DataField="RemainingLife" Width="120" TextAlign="Right" />
                    <px:PXGridColumn DataField="NewUsefulLife" Width="120" TextAlign="Right" CommitChanges="True" />
                    <px:PXGridColumn DataField="Hold" Width="60" Type="CheckBox" TextAlign="Center" />
                    <px:PXGridColumn DataField="Approved" Width="80" Type="CheckBox" TextAlign="Center" />
                    <px:PXGridColumn DataField="Rejected" Width="80" Type="CheckBox" TextAlign="Center" />
                </Columns>
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Enabled="True" MinHeight="200" Container="Window" />
        <ActionBar>
            <Actions>
                <AddNew MenuVisible="True" ToolBarVisible="Top" />
                <Delete MenuVisible="True" ToolBarVisible="Top" />
            </Actions>
            <CustomItems>
                <px:PXToolBarButton Text="First Page" CommandName="FirstPage" CommandSourceID="ds" />
                <px:PXToolBarButton Text="Previous Page" CommandName="PreviousPage" CommandSourceID="ds" />
                <px:PXToolBarButton Text="Next Page" CommandName="NextPage" CommandSourceID="ds" />
                <px:PXToolBarButton Text="Last Page" CommandName="LastPage" CommandSourceID="ds" />
            </CustomItems>
        </ActionBar>
        <Mode InitNewRow="True" AllowUpload="False" />
    </px:PXGrid>
    
    <div style="margin-top: 10px; padding: 10px; background-color: #f0f0f0; border: 1px solid #ccc; border-radius: 4px;">
        <asp:Label ID="paginationLabel" runat="server" Text="Loading..." Style="font-weight: bold; font-size: 14px;" />
    </div>
    
    <!-- Approval View -->
    <px:PXGrid ID="gridApproval" runat="server" DataSourceID="ds" SkinID="Details" Width="100%" 
        Height="200px" Caption="Approvals" CaptionVisible="True" 
        SyncPosition="True" KeepPosition="True" AllowPaging="True">
        <Levels>
            <px:PXGridLevel DataMember="Approval">
                <Columns>
                    <px:PXGridColumn DataField="ApprovalStepID" Width="80" />
                    <px:PXGridColumn DataField="WorkgroupID" Width="100" />
                    <px:PXGridColumn DataField="OwnerID" Width="100" />
                    <px:PXGridColumn DataField="Approved" Width="80" Type="CheckBox" TextAlign="Center" />
                    <px:PXGridColumn DataField="Rejected" Width="80" Type="CheckBox" TextAlign="Center" />
                    <px:PXGridColumn DataField="CreatedDateTime" Width="150" />
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Enabled="True" MinHeight="100" Container="Window" />
    </px:PXGrid>