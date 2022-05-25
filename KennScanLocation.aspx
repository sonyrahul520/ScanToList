<%@ Page Title="" Language="C#" MasterPageFile="~/EOLQAMaster.Master" AutoEventWireup="true" CodeBehind="KennScanLocation.aspx.cs" Inherits="IMKelly_EOLQA.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentHeaderButtons" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodyContent" runat="server">
    <div class ="row" style="margin-top:50px;">
        <div class ="row">
             <asp:Label runat="server" CssClass="h3">Kenn Check Function</asp:Label>
        </div>
        <div class ="row" style=" border-top:20px; padding-top:50px;">
            <div class="col-xs-3">
             
        </div>
        <div class ="col-xs-6">

           
            <div class ="row" >
                <asp:Label runat="server" CssClass="h3">Scan Value</asp:Label>
                <asp:TextBox ID= "tbScan" runat="server"   AutoPostBack ="True" OnTextChanged="tbScan_TextChanged" ></asp:TextBox>
               

            </div>
            <div class ="row"  style=" padding-top:20px;">
                <asp:Label runat="server" CssClass="h3">Location</asp:Label>
                <asp:TextBox ID="tbLocation" runat="server"  AutoPostBack ="True"></asp:TextBox>
                <asp:RequiredFieldValidator ID="locReq" ControlToValidate ="tbLocation"  ErrorMessage="Please enter a value.<br />"
                    EnableClientScript="False"
                    Display="Dynamic"
                    runat="server" />
                </div>
             <div class="row" style=" padding-top:20px;">
            <asp:Button ID="submit_button" runat="server" Text="Submit" onclick ="submit_button_Click" class="btn btn-primary" />
            
             </div>
           </div>
        <div class="col-xs-3"></div>
              </div>
         
             <asp:ListView runat ="server" id ="lvValues" OnItemDataBound ="IvLays_ItemDataBound">
                 <ItemTemplate>
                        <div class ="row">
                            <asp:Label runat ="server" ID="lblScanValues"></asp:Label>
                        </div>
                 </ItemTemplate>
             </asp:ListView>
           
       
            
        </div>
      
       
        
</asp:Content>
