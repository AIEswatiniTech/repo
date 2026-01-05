<%@ Page Language="C#" 
    MasterPageFile="~/MasterPages/FormDetail.master" 
    AutoEventWireup="true" 
    ValidateRequest="false" 
    CodeFile="AV402200.aspx.cs" 
    Inherits="Page_AV402200" 
    Title="Asset Verification" 
    EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>
<%@ Register Assembly="PX.Web.UI" Namespace="PX.Web.UI" TagPrefix="px" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="server">
    <px:PXDataSource 
        ID="ds" 
        runat="server" 
        PrimaryView="MasterView" 
        TypeName="PPSAAssetValidation.AVVerificationEntry" 
        Visible="True"
        OnDataBound="ds_DataBound">
        
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand Name="Save" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="First" PostData="Self" StartNewGroup="True" />
            <px:PXDSCallbackCommand Name="Last" PostData="Self" />
            <px:PXDSCallbackCommand Name="RemoveHold" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="Submit" CommitChanges="True" />
            <px:PXDSCallbackCommand Name="CancelVerification" CommitChanges="True" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>

<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="server">
    <px:PXFormView 
        ID="form" 
        runat="server" 
        DataSourceID="ds" 
        DataMember="MasterView" 
        Width="100%" 
        Caption="Asset Verification" 
        NoteIndicator="True" 
        FilesIndicator="True" 
        DefaultControlID="edRefNbr">
        
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M" />
            
            <px:PXSelector ID="edRefNbr" runat="server" DataField="RefNbr" CommitChanges="True" />
            <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" />

            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M" />
            <px:PXDropDown ID="edAssetStatusFilter" runat="server" DataField="AssetStatusFilter" CommitChanges="True" />
            <px:PXSelector ID="edAssetClassID" runat="server" DataField="AssetClassID" CommitChanges="True" />
            <px:PXSelector ID="edBranchID" runat="server" DataField="BranchID" CommitChanges="True" />

            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M" />
            <px:PXSelector ID="edBuilding" runat="server" DataField="Building" CommitChanges="True" />
            <px:PXSelector ID="edCustodian" runat="server" DataField="Custodian" CommitChanges="True" />
            <px:PXSelector ID="edDepartment" runat="server" DataField="Department" CommitChanges="True" />
        </Template>
    </px:PXFormView>
</asp:Content>

<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="server">
    <px:PXGrid 
        ID="grid" 
        runat="server" 
        DataSourceID="ds" 
        SkinID="Details" 
        Width="100%" 
        Height="400px" 
        Caption="Asset Verification Details" 
        CaptionVisible="True" 
        SyncPosition="True" 
        KeepPosition="True">
        
        <Levels>
            <px:PXGridLevel DataMember="DetailsView">
                <RowTemplate>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M" />

                    <px:PXNumberEdit ID="edLineID" runat="server" DataField="LineID" Enabled="False" />
                    <px:PXDropDown ID="edLineStatus" runat="server" DataField="Status" Enabled="False" />
                    <px:PXNumberEdit ID="edCount" runat="server" DataField="LineID" Enabled="False" />
                    <px:PXSelector ID="edAssetID" runat="server" DataField="AssetID" CommitChanges="True" AutoRefresh="True" />
                    <px:PXTextEdit ID="edAssetDescription" runat="server" DataField="AssetDescription" Enabled="False" />
                    <px:PXCheckBox ID="chkMarkedForDisposal" runat="server" DataField="MarkedForDisposal" CommitChanges="True" />
                    <px:PXSelector ID="edBranchID_Line" runat="server" DataField="BranchID" Enabled="False" />
                    <px:PXSelector ID="edBuilding_Line" runat="server" DataField="Building" Enabled="False" />
                    <px:PXDropDown ID="edCondition" runat="server" DataField="Condition" CommitChanges="True" />
                    <px:PXTextEdit ID="edFloor" runat="server" DataField="Floor" Enabled="False" />
                    <px:PXTextEdit ID="edRoom" runat="server" DataField="Room" Enabled="False" />
                    <px:PXSelector ID="edCustodian_Line" runat="server" DataField="Custodian" Enabled="False" />
                    <px:PXSelector ID="edDepartment_Line" runat="server" DataField="Department" Enabled="False" />
                    <px:PXSelector ID="edAssetClass_Line" runat="server" DataField="AssetClass" Enabled="False" />
                    <px:PXNumberEdit ID="edAssetQuantity" runat="server" DataField="AssetQuantity" Enabled="False" />
                    <px:PXNumberEdit ID="edExpectedQty" runat="server" DataField="ExpectedQty" Enabled="False" />
                    <px:PXNumberEdit ID="edCountQuantity" runat="server" DataField="CountQuantity" CommitChanges="True" />
                    <px:PXNumberEdit ID="edVarianceQty" runat="server" DataField="VarianceQuantity" Enabled="False" />
                    <px:PXTextEdit ID="edComments" runat="server" DataField="Comments" CommitChanges="True" />
                </RowTemplate>

                <Columns>
                    <px:PXGridColumn DataField="Status" Width="90" Type="DropDownList" />
                    <px:PXGridColumn DataField="LineID" Width="60" Label="Count" />
                    <px:PXGridColumn DataField="AssetID" Width="130" DisplayMode="Hint" CommitChanges="True" />
                    <px:PXGridColumn DataField="AssetDescription" Width="200" />
                    <px:PXGridColumn DataField="MarkedForDisposal" Width="100" Type="CheckBox" TextAlign="Center" />
                    <px:PXGridColumn DataField="BranchID" Width="130" />
                    <px:PXGridColumn DataField="Building" Width="120" />
                    <px:PXGridColumn DataField="Condition" Width="100" CommitChanges="True" />
                    <px:PXGridColumn DataField="Floor" Width="80" />
                    <px:PXGridColumn DataField="Room" Width="100" />
                    <px:PXGridColumn DataField="Custodian" Width="120" />
                    <px:PXGridColumn DataField="Department" Width="120" />
                    <px:PXGridColumn DataField="AssetClass" Width="100" />
                    <px:PXGridColumn DataField="AssetQuantity" Width="100" TextAlign="Right" />
                    <px:PXGridColumn DataField="ExpectedQty" Width="100" TextAlign="Right" />
                    <px:PXGridColumn DataField="CountQuantity" Width="100" TextAlign="Right" CommitChanges="True" />
                    <px:PXGridColumn DataField="VarianceQuantity" Width="100" TextAlign="Right" />
                    <px:PXGridColumn DataField="Comments" Width="200" />
                </Columns>
            </px:PXGridLevel>
        </Levels>

        <AutoSize Enabled="True" MinHeight="200" Container="Window" />

        <ActionBar>
            <CustomItems>
                <px:PXToolBarButton Text="First Page" CommandName="FirstPage" CommandSourceID="ds" />
                <px:PXToolBarButton Text="Previous Page" CommandName="PreviousPage" CommandSourceID="ds" />
                <px:PXToolBarButton Text="Next Page" CommandName="NextPage" CommandSourceID="ds" />
                <px:PXToolBarButton Text="Last Page" CommandName="LastPage" CommandSourceID="ds" />
            </CustomItems>
        </ActionBar>
        
    </px:PXGrid>
    
    <div style="margin-top: 10px; padding: 10px; background-color: #f0f0f0; border: 1px solid #ccc; border-radius: 4px;">
        <asp:Label ID="paginationLabel" runat="server" Text="Loading..." Style="font-weight: bold; font-size: 14px;" />
    </div>
</asp:Content>
