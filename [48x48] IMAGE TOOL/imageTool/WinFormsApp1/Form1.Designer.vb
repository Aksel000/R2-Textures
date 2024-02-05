<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        GroupBox1 = New GroupBox()
        PortTextBox = New TextBox()
        Label8 = New Label()
        labl3 = New Label()
        DBNameTxt = New TextBox()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        ConnectButton = New Button()
        Pwd = New TextBox()
        User = New TextBox()
        ipaddress = New TextBox()
        GroupBox2 = New GroupBox()
        Panel1 = New Panel()
        ItemMap = New PictureBox()
        GroupBox3 = New GroupBox()
        itemMapName = New ComboBox()
        Label10 = New Label()
        ItemName = New TextBox()
        Label9 = New Label()
        ComboBox1 = New ComboBox()
        Label7 = New Label()
        Label6 = New Label()
        Label5 = New Label()
        Label4 = New Label()
        Button3 = New Button()
        ReadButton = New Button()
        TextBoxY = New TextBox()
        TextBoxX = New TextBox()
        itemID = New TextBox()
        GroupBox4 = New GroupBox()
        PictureBox1 = New PictureBox()
        GroupBox1.SuspendLayout()
        GroupBox2.SuspendLayout()
        Panel1.SuspendLayout()
        CType(ItemMap, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox3.SuspendLayout()
        GroupBox4.SuspendLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(PortTextBox)
        GroupBox1.Controls.Add(Label8)
        GroupBox1.Controls.Add(labl3)
        GroupBox1.Controls.Add(DBNameTxt)
        GroupBox1.Controls.Add(Label3)
        GroupBox1.Controls.Add(Label2)
        GroupBox1.Controls.Add(Label1)
        GroupBox1.Controls.Add(ConnectButton)
        GroupBox1.Controls.Add(Pwd)
        GroupBox1.Controls.Add(User)
        GroupBox1.Controls.Add(ipaddress)
        GroupBox1.Location = New Point(828, 11)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(265, 159)
        GroupBox1.TabIndex = 0
        GroupBox1.TabStop = False
        GroupBox1.Text = "Database Connectivity"
        ' 
        ' PortTextBox
        ' 
        PortTextBox.Location = New Point(207, 19)
        PortTextBox.Name = "PortTextBox"
        PortTextBox.Size = New Size(42, 23)
        PortTextBox.TabIndex = 1
        PortTextBox.Text = "1433"
        ' 
        ' Label8
        ' 
        Label8.AutoSize = True
        Label8.Location = New Point(171, 22)
        Label8.Name = "Label8"
        Label8.Size = New Size(29, 15)
        Label8.TabIndex = 10
        Label8.Text = "Port"
        ' 
        ' labl3
        ' 
        labl3.AutoSize = True
        labl3.Location = New Point(6, 48)
        labl3.Name = "labl3"
        labl3.Size = New Size(54, 15)
        labl3.TabIndex = 9
        labl3.Text = "DBName"
        ' 
        ' DBNameTxt
        ' 
        DBNameTxt.Location = New Point(66, 45)
        DBNameTxt.Name = "DBNameTxt"
        DBNameTxt.PlaceholderText = "FNLParm"
        DBNameTxt.Size = New Size(183, 23)
        DBNameTxt.TabIndex = 2
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(24, 99)
        Label3.Name = "Label3"
        Label3.Size = New Size(30, 15)
        Label3.TabIndex = 7
        Label3.Text = "Pwd"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(21, 73)
        Label2.Name = "Label2"
        Label2.Size = New Size(30, 15)
        Label2.TabIndex = 7
        Label2.Text = "User"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(7, 22)
        Label1.Name = "Label1"
        Label1.Size = New Size(17, 15)
        Label1.TabIndex = 6
        Label1.Text = "IP"
        ' 
        ' ConnectButton
        ' 
        ConnectButton.Location = New Point(66, 119)
        ConnectButton.Name = "ConnectButton"
        ConnectButton.Size = New Size(183, 26)
        ConnectButton.TabIndex = 5
        ConnectButton.Text = "Connect"
        ConnectButton.UseVisualStyleBackColor = True
        ' 
        ' Pwd
        ' 
        Pwd.Location = New Point(66, 94)
        Pwd.Name = "Pwd"
        Pwd.PasswordChar = "*"c
        Pwd.PlaceholderText = "pass"
        Pwd.Size = New Size(183, 23)
        Pwd.TabIndex = 4
        ' 
        ' User
        ' 
        User.Location = New Point(66, 71)
        User.Name = "User"
        User.PlaceholderText = "sa"
        User.Size = New Size(183, 23)
        User.TabIndex = 3
        ' 
        ' ipaddress
        ' 
        ipaddress.Location = New Point(66, 19)
        ipaddress.Name = "ipaddress"
        ipaddress.PlaceholderText = "192.168.1.2"
        ipaddress.Size = New Size(99, 23)
        ipaddress.TabIndex = 0
        ' 
        ' GroupBox2
        ' 
        GroupBox2.Controls.Add(Panel1)
        GroupBox2.Location = New Point(10, 11)
        GroupBox2.Name = "GroupBox2"
        GroupBox2.Size = New Size(812, 551)
        GroupBox2.TabIndex = 1
        GroupBox2.TabStop = False
        GroupBox2.Text = "Item map"
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(ItemMap)
        Panel1.Location = New Point(6, 19)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(800, 526)
        Panel1.TabIndex = 0
        ' 
        ' ItemMap
        ' 
        ItemMap.Location = New Point(0, 0)
        ItemMap.Name = "ItemMap"
        ItemMap.Size = New Size(800, 526)
        ItemMap.TabIndex = 0
        ItemMap.TabStop = False
        ' 
        ' GroupBox3
        ' 
        GroupBox3.Controls.Add(itemMapName)
        GroupBox3.Controls.Add(Label10)
        GroupBox3.Controls.Add(ItemName)
        GroupBox3.Controls.Add(Label9)
        GroupBox3.Controls.Add(ComboBox1)
        GroupBox3.Controls.Add(Label7)
        GroupBox3.Controls.Add(Label6)
        GroupBox3.Controls.Add(Label5)
        GroupBox3.Controls.Add(Label4)
        GroupBox3.Controls.Add(Button3)
        GroupBox3.Controls.Add(ReadButton)
        GroupBox3.Controls.Add(TextBoxY)
        GroupBox3.Controls.Add(TextBoxX)
        GroupBox3.Controls.Add(itemID)
        GroupBox3.Controls.Add(GroupBox4)
        GroupBox3.Location = New Point(828, 175)
        GroupBox3.Name = "GroupBox3"
        GroupBox3.Size = New Size(265, 386)
        GroupBox3.TabIndex = 2
        GroupBox3.TabStop = False
        GroupBox3.Text = "Data settings"
        ' 
        ' itemMapName
        ' 
        itemMapName.FormattingEnabled = True
        itemMapName.Location = New Point(66, 164)
        itemMapName.Name = "itemMapName"
        itemMapName.Size = New Size(183, 23)
        itemMapName.TabIndex = 7
        ' 
        ' Label10
        ' 
        Label10.AutoSize = True
        Label10.Location = New Point(16, 191)
        Label10.Name = "Label10"
        Label10.Size = New Size(39, 15)
        Label10.TabIndex = 17
        Label10.Text = "Name"
        Label10.TextAlign = ContentAlignment.TopRight
        ' 
        ' ItemName
        ' 
        ItemName.Enabled = False
        ItemName.Location = New Point(66, 191)
        ItemName.Name = "ItemName"
        ItemName.Size = New Size(183, 23)
        ItemName.TabIndex = 8
        ' 
        ' Label9
        ' 
        Label9.AutoSize = True
        Label9.Location = New Point(20, 141)
        Label9.Name = "Label9"
        Label9.Size = New Size(32, 15)
        Label9.TabIndex = 15
        Label9.Text = "Mod"
        Label9.TextAlign = ContentAlignment.TopRight
        ' 
        ' ComboBox1
        ' 
        ComboBox1.FormattingEnabled = True
        ComboBox1.Items.AddRange(New Object() {"DT_ItemResource", "DT_SkillPack"})
        ComboBox1.Location = New Point(66, 137)
        ComboBox1.Name = "ComboBox1"
        ComboBox1.Size = New Size(183, 23)
        ComboBox1.TabIndex = 6
        ComboBox1.Text = "DT_ItemResource"
        ' 
        ' Label7
        ' 
        Label7.AutoSize = True
        Label7.Location = New Point(16, 169)
        Label7.Name = "Label7"
        Label7.Size = New Size(40, 15)
        Label7.TabIndex = 12
        Label7.Text = "Image"
        Label7.TextAlign = ContentAlignment.TopRight
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(150, 276)
        Label6.Name = "Label6"
        Label6.Size = New Size(14, 15)
        Label6.TabIndex = 10
        Label6.Text = "Y"
        ' 
        ' Label5
        ' 
        Label5.AutoSize = True
        Label5.Location = New Point(44, 276)
        Label5.Name = "Label5"
        Label5.Size = New Size(14, 15)
        Label5.TabIndex = 9
        Label5.Text = "X"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Location = New Point(31, 220)
        Label4.Name = "Label4"
        Label4.Size = New Size(21, 15)
        Label4.TabIndex = 8
        Label4.Text = "IID"
        ' 
        ' Button3
        ' 
        Button3.Enabled = False
        Button3.Location = New Point(66, 299)
        Button3.Name = "Button3"
        Button3.Size = New Size(183, 26)
        Button3.TabIndex = 13
        Button3.Text = "Update Data"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' ReadButton
        ' 
        ReadButton.Enabled = False
        ReadButton.Location = New Point(66, 243)
        ReadButton.Name = "ReadButton"
        ReadButton.Size = New Size(183, 26)
        ReadButton.TabIndex = 10
        ReadButton.Text = "Read"
        ReadButton.UseVisualStyleBackColor = True
        ' 
        ' TextBoxY
        ' 
        TextBoxY.Enabled = False
        TextBoxY.Location = New Point(171, 274)
        TextBoxY.Name = "TextBoxY"
        TextBoxY.Size = New Size(78, 23)
        TextBoxY.TabIndex = 12
        ' 
        ' TextBoxX
        ' 
        TextBoxX.Enabled = False
        TextBoxX.Location = New Point(66, 274)
        TextBoxX.Name = "TextBoxX"
        TextBoxX.Size = New Size(77, 23)
        TextBoxX.TabIndex = 11
        ' 
        ' itemID
        ' 
        itemID.Enabled = False
        itemID.Location = New Point(66, 217)
        itemID.Name = "itemID"
        itemID.Size = New Size(183, 23)
        itemID.TabIndex = 9
        ' 
        ' GroupBox4
        ' 
        GroupBox4.Controls.Add(PictureBox1)
        GroupBox4.Location = New Point(93, 19)
        GroupBox4.Name = "GroupBox4"
        GroupBox4.Size = New Size(108, 102)
        GroupBox4.TabIndex = 0
        GroupBox4.TabStop = False
        GroupBox4.Text = "item Icon"
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Location = New Point(30, 31)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(56, 48)
        PictureBox1.TabIndex = 0
        PictureBox1.TabStop = False
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1101, 574)
        Controls.Add(GroupBox3)
        Controls.Add(GroupBox2)
        Controls.Add(GroupBox1)
        MaximizeBox = False
        Name = "Form1"
        Text = "Item Icon Modify Tool"
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        GroupBox2.ResumeLayout(False)
        Panel1.ResumeLayout(False)
        CType(ItemMap, ComponentModel.ISupportInitialize).EndInit()
        GroupBox3.ResumeLayout(False)
        GroupBox3.PerformLayout()
        GroupBox4.ResumeLayout(False)
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ItemMap As PictureBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents ConnectButton As Button
    Friend WithEvents Pwd As TextBox
    Friend WithEvents User As TextBox
    Friend WithEvents ipaddress As TextBox
    Friend WithEvents TextBoxX As TextBox
    Friend WithEvents itemID As TextBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents Button3 As Button
    Friend WithEvents ReadButton As Button
    Friend WithEvents TextBoxY As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label7 As Label
    Friend WithEvents labl3 As Label
    Friend WithEvents DBNameTxt As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents PortTextBox As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents ComboBox1 As ComboBox
    Friend WithEvents Label10 As Label
    Friend WithEvents ItemName As TextBox
    Friend WithEvents itemMapName As ComboBox
End Class
