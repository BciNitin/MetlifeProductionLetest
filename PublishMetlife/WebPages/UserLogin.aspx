<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserLogin.aspx.cs" Inherits="UserLogin"
    Title="MetLife : ATS - User Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>
<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <title>MetLife : ASSET TACKING SYSTEM</title>
    <link rel="Shortcut Icon" href="../images/BasicScan.ico" />
    <link rel="stylesheet" href="../css/style.css" type="text/css" />

    <script type="text/javascript" src="http://code.jquery.com/jquery-latest.min.js"></script>

    <script type="text/javascript" src="../js/jquery-latest.min.js"></script>

    <link rel="stylesheet" type="text/css" href="../slidecss/style.css" />

    <script type="text/javascript" src="../slidejs/jquery-1.2.6.min.js"></script>

    <script type="text/javascript" src="../slidejs/jquery-easing-1.3.pack.js"></script>

    <script type="text/javascript" src="../slidejs/jquery-easing-compatibility.1.2.pack.js"></script>

    <script type="text/javascript" src="../slidejs/coda-slider.1.1.1.pack.js"></script>

    <script type="text/javascript">
        var theInt = null;
        var $crosslink, $navthumb;
        var curclicked = 0;

        theInterval = function(cur) {
            clearInterval(theInt);

            if (typeof cur != 'undefined')
                curclicked = cur;

            $crosslink.removeClass("active-thumb");
            $navthumb.eq(curclicked).parent().addClass("active-thumb");
            $(".stripNav ul li a").eq(curclicked).trigger('click');

            theInt = setInterval(function() {
                $crosslink.removeClass("active-thumb");
                $navthumb.eq(curclicked).parent().addClass("active-thumb");
                $(".stripNav ul li a").eq(curclicked).trigger('click');
                curclicked++;
                if (6 == curclicked)
                    curclicked = 0;

            }, 3000);
        };

        $(function() {

            $("#main-photo-slider").codaSlider();

            $navthumb = $(".nav-thumb");
            $crosslink = $(".cross-link");

            $navthumb
			.click(function() {
			    var $this = $(this);
			    theInterval($this.parent().attr('href').slice(1) - 1);
			    return false;
			});

            theInterval();
        });

        function eToggle(anctag, darg) {
            var ele = document.getElementById(darg);
            var text = document.getElementById(anctag);
            if (ele.style.display == "block") {
                ele.style.display = "none";
                text.innerHTML = ">> Forgot Password";
            }
            else {
                ele.style.display = "block";
                text.innerHTML = ">> Forgot Password";
            }
        }
        function ValidateLogin() {
            alert('Please Note : Select company name.');
            document.getElementById('<%= ddlAssetType.ClientID %>').focus();
        }
        function ValidateForgtPswd() {
            var AssetType = document.getElementById('<%= ddlAssetType.ClientID %>').value;
            var Company = document.getElementById('<%= ddlAssetType.ClientID %>').value;
            if (AssetType == "SELECT") {
                alert('Please Note : Select Asset Type.');
                document.getElementById('<%= ddlAssetType.ClientID %>').focus();
                return false;
            }
            else if (Company == "-- Select Asset Type --") {
                alert('Please Note : Select Company/Location.');
                document.getElementById('<%= ddlAssetType.ClientID %>').focus();
                return false;
            }
            else {
                return true;
            }
        }
        function noBack() {
            window.history.forward(1);
        }
    </script>

</head>
<body class="gray" onload="noBack();">
    <form id="form1" runat="server">
    <act:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </act:ToolkitScriptManager>
    <div id="pageFrame" align="center">
        <div id="pageContainer" align="left">
            <div id="topnav">
                <div id="topnavInner">
                    <div id="logo">
                        <img src="../images/MetLife.png" alt="CompanyLogo" />
                    </div>
                </div>
            </div>
            <div id="wrapper">
            </div>
            <div id="wrapper1">
                <div id="left">
                    <div id="page-wrap">
                        <div class="slider-wrap">
                            <div id="main-photo-slider" class="csw">
                                <div class="panelContainer">
                                    <div class="panel" title="Panel 1">
                                        <div class="wrapper">
                                            <img src="../slideimages/image065.jpg" width="419px" height="285px" alt="temp" />
                                        </div>
                                    </div>
                                    <div class="panel" title="Panel 2">
                                        <div class="wrapper">
                                            <img src="../slideimages/AssetManagement.jpg" width="419px" height="285px" alt="temp" />
                                        </div>
                                    </div>
                                    <div class="panel" title="Panel 3">
                                        <div class="wrapper">
                                            <img src="../slideimages/shutterstock.jpg" width="419px" height="285px" alt="scotch egg" />
                                        </div>
                                    </div>
                                    <div class="panel" title="Panel 4">
                                        <div class="wrapper">
                                            <img src="../slideimages/New4.jpg" width="419px" height="285px" alt="temp" />
                                        </div>
                                    </div>
                                    <div class="panel" title="Panel 5">
                                        <div class="wrapper">
                                            <img src="../slideimages/Asset Areas.jpg" width="419px" height="285px" alt="temp" />
                                        </div>
                                    </div>
                                    <div class="panel" title="Panel 6">
                                        <div class="wrapper">
                                            <img src="../slideimages/globalmarketing.jpg" style="width: 419; height: 285px" alt="temp" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <a href="#1" class="cross-link active-thumb">
                                <img src="../slideimages/image065_menu.jpg" width="60px" height="40px" class="nav-thumb"
                                    alt="temp-thumb" />
                            </a>
                            <div id="movers-row">
                                <div>
                                    <a href="#2" class="cross-link">
                                        <img src="../slideimages/AssetMngt_menu.jpg" width="60px" height="40px" class="nav-thumb"
                                            alt="temp-thumb" /></a></div>
                                <div>
                                    <a href="#3" class="cross-link">
                                        <img src="../slideimages/shutterstock_menu.jpg" width="60px" height="40px" class="nav-thumb"
                                            alt="temp-thumb" /></a></div>
                                <div>
                                    <a href="#4" class="cross-link">
                                        <img src="../slideimages/New4_Menu.jpg" width="60px" height="40px" class="nav-thumb"
                                            alt="temp-thumb" /></a></div>
                                <div>
                                    <a href="#5" class="cross-link">
                                        <img src="../slideimages/Asset_Menu.jpg" width="60px" height="40px" class="nav-thumb"
                                            alt="temp-thumb" /></a></div>
                                <div>
                                    <a href="#6" class="cross-link">
                                        <img src="../slideimages/marketing_menu.jpg" width="60px" height="40px" class="nav-thumb"
                                            alt="temp-thumb" /></a></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="right">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="page-wrap1">
                                <div id="div1" style="padding-top: 65px; padding-left: 45px; font: small-caps bold 20px  Helvetica, Arial;
                                    color: White;">
                                    ATS - USER LOGIN
                                </div>
                                <div id="div2"    style="width: 260px; padding-top: 10px; padding-left: 45px; font: normal bold 15px  Helvetica, Arial;
                                    color: White;">
                                    Select Asset Type :
                                </div>
                                <div id="div12" Visible="false"  style="width: 260px; padding-top: 5px; padding-left: 45px; font: normal 14px  Helvetica, Arial;
                                    color: White;">
                                    <asp:DropDownList ID="ddlAssetType" TabIndex="1" runat="server" CssClass="dropdownlist"
                                        Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged"
                                        ToolTip="Select Asset Type">
                                        <asp:ListItem Text="-- Select Asset Type --" Value="SELECT"></asp:ListItem>                                        
                                        <asp:ListItem Text="IT ASSET" Value="IT"></asp:ListItem>
                                        <asp:ListItem Text="Facilities" Value="Facilities"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                    <asp:RequiredFieldValidator ID="rfvAssetType" runat="server" ValidationGroup="Signin"
                                        ControlToValidate="ddlAssetType" InitialValue="SELECT" ErrorMessage="Please Select Asset Type."></asp:RequiredFieldValidator>
                                </div>
                                <%--<div id="div11" style="width: 260px; padding-top: 10px; padding-left: 45px; font: normal bold 15px  Helvetica, Arial;
                                    color: White;">
                                    Select Company/Location :
                                </div>
                                <div id="div10" style="width: 260px; padding-top: 5px; padding-left: 45px; font: normal 14px  Helvetica, Arial;
                                    color: White;">
                                    <asp:DropDownList ID="ddlCompany" TabIndex="2" runat="server" CssClass="dropdownlist"
                                        Width="200px" ToolTip="Select Company Name">
                                    </asp:DropDownList>
                                    <br />
                                </div>--%>
                                <div id="div3" style="width: 260px; padding-top: 10px; padding-left: 45px; font: normal bold 15px  Helvetica, Arial;
                                    color: White;">
                                    Enter User ID :
                                </div>
                                <div id="div5" style="width: 260px; padding-top: 5px; padding-left: 45px; font: normal 14px  Helvetica, Arial;
                                    color: White;">
                                    <asp:TextBox ID="txtUserId" autocomplete="off" style="margin-left:0px !important;" ValidationGroup="Signin" TabIndex="3"
                                        MaxLength="25" CssClass="textbox" runat="server" ToolTip="Enter User ID" Width="200px"
                                        Height="18px"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ValidationGroup="Signin" ID="rfvUserId" runat="server"
                                        ControlToValidate="txtUserId" ErrorMessage="Please enter user id."></asp:RequiredFieldValidator>
                                </div>
                                <div id="div4" style="width: 260px; padding-top: 7px; padding-left: 45px; font: normal bold 15px  Helvetica, Arial;
                                    color: White;">
                                    Enter Password :
                                </div>
                                <div id="div6" style="width: 260px; padding-top: 5px; padding-left: 45px; font: normal 14px  Helvetica, Arial;
                                    color: White;">
                                    <asp:TextBox ID="txtPassword" autocomplete="off" style="margin-left:0px !important;" CssClass="textbox" TabIndex="4"
                                        MaxLength="25" ValidationGroup="Signin" runat="server" ToolTip="Enter Password"
                                        TextMode="Password" Width="200px" Height="18px" oncopy="return false;" onpaste="return false;"
                                        oncut="return false;"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ValidationGroup="Signin"
                                        ControlToValidate="txtPassword" ErrorMessage="Please enter password."></asp:RequiredFieldValidator>
                                </div>
                                <div id="div7" style="width: 260px; padding-top: 5px; padding-left: 45px; font: normal bold 14px  Helvetica, Arial;
                                    color: White;">
                                    <asp:ImageButton ID="btnSignIn" TabIndex="5" runat="server" ImageUrl="../images/Signin.png"
                                        ToolTip="Click here to sign in" ValidationGroup="Signin" CausesValidation="true"
                                        OnClick="btnSignIn_Click" />
                                </div>
                                <div id="div9" style="width: 260px; text-align: left; padding-top: 3px; padding-left: 45px;
                                    font: normal bold 14px  Helvetica, Arial; color: White;">
                                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                </div>
                               <%-- <div id="div8" style="width: 260px; padding-top: 2px; padding-left: 45px; font: normal bold 14px  Helvetica, Arial;
                                    color: White;">
                                    <asp:LinkButton ID="btnForgotPswd" OnClick="btnForgotPswd_Click" TabIndex="6" OnClientClick="return ValidateForgtPswd();"
                                        runat="server" Style="color: White;">>> Forgot Password</asp:LinkButton>
                                </div>--%>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSignIn" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <br />
            <br />
            <div id="footer">
                <div class="copyright">
                    <span>Copyright &copy; 2014. All Rights Reserved.</span>
                </div>
                <div class="designedby">
                    <span>Designed & Developed By <a href="http://www.mobivue.in" target="_blank">
                        BCIL</a></span>
                </div>
            </div>
        </div>
    </div>
    <div>
    </div>
    </form>
</body>
</html>
