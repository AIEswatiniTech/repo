<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true"  
    ValidateRequest="false" CodeFile="AV400000.aspx.cs" Inherits="Page_AV400000" Title="Asset Life Reassessment Setup" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>
<%@ Register Assembly="PX.Web.UI" Namespace="PX.Web.UI" TagPrefix="px" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="server">
    <px:PXDataSource ID="ds" runat="server" PrimaryView="Setup" TypeName="PPSAAssetValidation.AVLifeReassessmentSetupMaint" Visible="True">
        <CallbackCommands>
            <px:PXDSCallbackCommand Name="Insert" PostData="Self" />
            <px:PXDSCallbackCommand Name="Save" CommitChanges="True" />
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>

<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Setup" Width="100%" 
        Caption="Asset Life Reassessment Setup" DefaultControlID="edApprovalsEnabled">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M" />
            <px:PXCheckBox ID="chkApprovalsEnabled" runat="server" DataField="ApprovalsEnabled" CommitChanges="True" />
            <px:PXSelector ID="edAssignmentMapID" runat="server" DataField="AssignmentMapID" CommitChanges="True" AllowEdit="True" />
            <px:PXNumberEdit ID="edNumberingSequence" runat="server" DataField="NumberingSequenceID" />
        </Template>
    </px:PXFormView>
</asp:Content>

<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="server">
    <px:PXGrid ID="grid" runat="server" DataSourceID="ds" SkinID="Details" Width="100%" 
        Height="400px" Caption="Approval Entities" CaptionVisible="True">
        <Levels>
            <px:PXGridLevel DataMember="ApprovalSetups">
                <RowTemplate>
                    <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M" />
                    <px:PXCheckBox ID="chkIsActive" runat="server" DataField="IsActive" />
                    <px:PXSelector ID="edWorkgroupID" runat="server" DataField="ApprovalWorkgroupID" />
                    <px:PXTextEdit ID="edDescription" runat="server" DataField="Description" />
                </RowTemplate>
                <Columns>
                    <px:PXGridColumn DataField="IsActive" Width="80" Type="CheckBox" TextAlign="Center" />
                    <px:PXGridColumn DataField="ApprovalWorkgroupID" Width="150" />
                    <px:PXGridColumn DataField="Description" Width="300" />
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Enabled="True" MinHeight="200" Container="Window" />
    </px:PXGrid>
</asp:Content>
