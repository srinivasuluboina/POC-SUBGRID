<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="POC_MultiGrid.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://code.jquery.com/jquery-3.4.1.min.js"></script>
</head>
<body>

    <form id="form1" runat="server">
        <div style="width:50%;">
            <div style="text-align: right;margin: 20px;">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
            </div>
            <%-- <asp:GridView ID="gvPrintResponse" runat="server">

        </asp:GridView>--%>

            <asp:GridView ID="gvPrintResponse" runat="server" AutoGenerateColumns="false" PageSize="1000" AllowPaging="true"
                CssClass="Grid" EmptyDataRowStyle-ForeColor="Red" EmptyDataText="Currently no data entered for this section"
                DataKeyNames="QuestionID" OnRowDataBound="gvPrintResponse_OnRowDataBound">
                <%----%>
                <Columns>
                    <asp:TemplateField ItemStyle-Width="50%" HeaderText="Items">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblPrintQuestion" Text='<%#Eval("Description") %>'></asp:Label>
                            <asp:Panel ID="pnlOrders" runat="server">
                                <asp:GridView ID="gvSubquestions" runat="server" DataKeyNames="SubQuestionID" AutoGenerateColumns="false" CssClass="ChildGrid">
                                    <Columns>
                                        <asp:BoundField ItemStyle-Width="150px" DataField="SubQuestion" HeaderText="SubQuestion" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="Yes">
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="radio   m-l-sm" Text=" " ID="chkPrintYes" runat="server" GroupName="a" Checked="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="No">
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="radio   m-l-sm" Text=" " ID="chkPrintNo" runat="server" GroupName="a" Checked="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="Yes">
                        <ItemTemplate>
                            <asp:RadioButton CssClass="radio radio-primary  m-l-sm" Text=" " ID="chkPrintYes" runat="server" GroupName="a" Checked="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="No">
                        <ItemTemplate>
                            <asp:RadioButton CssClass="radio radio-primary  m-l-sm" Text=" " ID="chkPrintNo" runat="server" GroupName="a" Checked="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
<script>
    $(document).ready(function () {
        $('.radio-primary input:radio').change(function () {
            if ($(this).val() == 'chkPrintYes') {
                $(this).parents('tr').find('.ChildGrid').css('display', 'block');
            }
            else {
                $(this).parents('tr').find('.ChildGrid').css('display', 'none');
            }

        });
    });
</script>
</html>
